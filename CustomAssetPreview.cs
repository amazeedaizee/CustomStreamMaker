using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CustomStreamMaker
{
    public partial class CustomAssetPreview : Form
    {
        internal StreamEditor streamEditor;
        List<CustomAsset> assetList;
        CustomAsset currentPreview;
        bool isDeleting;
        public CustomAssetPreview(StreamEditor streamEditor)
        {
            assetList = CustomAssetExtractor.customAssets;
            InitializeComponent();
            this.streamEditor = streamEditor;
        }

        internal void ReloadAssetListView()
        {
            CustomAssetsListView.BeginUpdate();
            CustomAssetsListView.Items.Clear();
            for (int i = 0; i < assetList.Count; i++)
            {
                var filePath = assetList[i].filePath.Replace("?", "");
                ListViewItem item = CustomAssetsListView.Items.Add(assetList[i].customAssetType.ToString());
                item.SubItems.Add(assetList[i].fileName);
                item.SubItems.Add(filePath);
            }
            CustomAssetsListView.EndUpdate();
        }
        private void CustomAssetPreview_Load(object sender, EventArgs e)
        {
            ReloadAssetListView();
        }

        private void DisablePreview()
        {
            CustomAssetsPreviewPic.Image = null;
            currentPreview = null;
        }

        private void DeleteSelected()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CustomStreamMaker\CustomAnimationClips";
            if (CustomAssetsListView.SelectedItems.Count == 0)
                return;
            for (int i = 0; i <= CustomAssetsListView.SelectedItems.Count; i++)
            {
                if (CustomAssetsListView.SelectedItems.Count == 0) break;
                isDeleting = true;
                i = 0;
                int index = CustomAssetsListView.SelectedIndices[0];
                if (currentPreview == assetList[index])
                {
                    CustomAssetsPreviewPic.Image?.Dispose();
                    DisablePreview();
                }
                var bundleName = Path.GetFileNameWithoutExtension(assetList[index].filePath);
                var pathToCache = Path.Combine(path, bundleName, assetList[index].fileName);
                if (assetList[index].customAssetFileType != CustomAssetFileType.ImageFile && File.Exists(pathToCache))
                    File.Delete(Path.Combine(path, bundleName, assetList[index].fileName));
                assetList.RemoveAt(index);
                CustomAssetsListView.Items.RemoveAt(index);
            }
            streamEditor.CheckUnvalidCustomAssets();
            CustomAssetExtractor.CheckForEmptyAnimFolder();
            CustomAssetExtractor.SaveCustomAssets();
            isDeleting = false;
        }

        private void DeleteCachedAssetIfSprite(CustomAsset customAsset)
        {
            if (customAsset.customAssetType != CustomAssetType.Sprite)
                return;
            var customPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CustomStreamMaker\CustomAnimationClips";
            var filePathToSprite = Path.Combine(customPath, Path.GetFileNameWithoutExtension(customAsset.filePath), customAsset.fileName);
            if (!File.Exists(filePathToSprite))
                return;
            File.Delete(filePathToSprite);
        }
        private void CustomAssetsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isDeleting)
                return;
            CustomAssetsPreviewPic.Image?.Dispose();
            if (CustomAssetsListView.SelectedItems.Count == 0)
            {
                DisablePreview();
                return;
            }
            int index = CustomAssetsListView.SelectedIndices[0];
            if (assetList[index].customAssetType == CustomAssetType.Background)
            {
                CustomAssetsPreviewPic.Image = AssetExtractor.GetCachedBackground(assetList[index].fileName);
            }
            else CustomAssetsPreviewPic.Image = AssetExtractor.GetCachedSprite(assetList[index].fileName);
            currentPreview = assetList[index];
        }

        private void CustomAssetsListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var confirm = MessageBox.Show("Are you sure you want to delete these assets?\nThis action can't be undone.", "Wait!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm != DialogResult.Yes)
                    return;
                DeleteSelected();
            }
        }

        private void CustomAssetPreview_Click(object sender, EventArgs e)
        {
            CustomAssetsListView.SelectedIndices.Clear();
        }

        private void AddBackground_Button_Click(object sender, EventArgs e)
        {
            CustomAssetExtractor.ImportImage();
            ReloadAssetListView();
        }

        private void AddAnimation_AssetLz4_Click(object sender, EventArgs e)
        {
            CustomAssetExtractor.ImportSpriteFromAssetBundle(true);
            ReloadAssetListView();
        }

        private void AddAnimation_AddressableLz4_Click(object sender, EventArgs e)
        {
            CustomAssetExtractor.ImportSpriteFromAddressable(true);
            ReloadAssetListView();
        }

        private void CustomAssetPreview_FormClosing(object sender, FormClosingEventArgs e)
        {
            CustomAssetExtractor.SaveCustomAssets();
            streamEditor.customAssetPreview = null;
            Dispose();
        }
    }
}
