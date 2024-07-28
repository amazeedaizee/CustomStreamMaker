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
        bool isSearchTouched;
        public CustomAssetPreview(StreamEditor streamEditor)
        {
            assetList = CustomAssetExtractor.customAssets;
            InitializeComponent();
            isSearchTouched = false;
            this.streamEditor = streamEditor;
        }

        internal void ReloadAssetListView(string searchTxt = null)
        {
            int i;
            string searcher;
            CustomAssetType filter = CustomAssetType.None;
            CustomAssetsListView.BeginUpdate();
            CustomAssetsListView.Items.Clear();
            for (i = 0; i < assetList.Count; i++)
            {
                var filePath = assetList[i].filePath.Replace("?", "");
                ListViewItem item = CustomAssetsListView.Items.Add(assetList[i].customAssetType.ToString());
                item.SubItems.Add(assetList[i].fileName);
                item.SubItems.Add(filePath);
            }
            if (!string.IsNullOrWhiteSpace(searchTxt))
            {
                if (searchTxt.StartsWith("s:"))
                {
                    searcher = searchTxt.Substring(0, 2);
                    filter = CustomAssetType.Sprite;
                    searchTxt = searchTxt.Replace(searcher, "");
                }
                else if (searchTxt.StartsWith("b:"))
                {
                    searcher = searchTxt.Substring(0, 2);
                    filter = CustomAssetType.Background;
                    searchTxt = searchTxt.Replace(searcher, "");
                }
                for (i = CustomAssetsListView.Items.Count - 1; i >= 0; i--)
                {
                    if (filter != CustomAssetType.None && filter.ToString() != CustomAssetsListView.Items[i].SubItems[0].Text)
                    {
                        CustomAssetsListView.Items[i].Remove();
                        continue;
                    }
                    if (!CustomAssetsListView.Items[i].SubItems[1].Text.Contains(searchTxt) && !string.IsNullOrWhiteSpace(searchTxt))
                    {
                        CustomAssetsListView.Items[i].Remove();
                    }
                }
            }
            CustomAssetsListView.EndUpdate();
        }
        private void CustomAssetPreview_Load(object sender, EventArgs e)
        {
            ReloadAssetListView();
            SearchBar.Text = "Search Here";
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
                var item = CustomAssetsListView.SelectedItems[0];
                CustomAsset selectedAsset = assetList.Find(a => a.filePath == item.SubItems[2].Text
                                                           && a.fileName == item.SubItems[1].Text
                                                           && a.customAssetType == (CustomAssetType)Enum.Parse(typeof(CustomAssetType), item.SubItems[0].Text));
                if (currentPreview == selectedAsset)
                {
                    CustomAssetsPreviewPic.Image?.Dispose();
                    DisablePreview();
                }
                var bundleName = Path.GetFileNameWithoutExtension(selectedAsset.filePath);
                var pathToCache = Path.Combine(path, bundleName, selectedAsset.fileName);
                if (selectedAsset.customAssetFileType != CustomAssetFileType.ImageFile && File.Exists(pathToCache))
                    File.Delete(Path.Combine(path, bundleName, selectedAsset.fileName));
                assetList.Remove(selectedAsset);
                CustomAssetsListView.Items.Remove(item);
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
            var item = CustomAssetsListView.SelectedItems[0];
            CustomAsset selectedAsset = assetList.Find(a => a.filePath == item.SubItems[2].Text
                                                       && a.fileName == item.SubItems[1].Text
                                                       && a.customAssetType == (CustomAssetType)Enum.Parse(typeof(CustomAssetType), item.SubItems[0].Text));
            if (selectedAsset != null)
            {
                if (selectedAsset.customAssetType == CustomAssetType.Background)
                {
                    CustomAssetsPreviewPic.Image = AssetExtractor.GetCachedBackground(selectedAsset.fileName);
                }
                else CustomAssetsPreviewPic.Image = AssetExtractor.GetCachedSprite(selectedAsset.fileName);
                currentPreview = selectedAsset;
            }
            else { currentPreview = null; }

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

        private void SearchBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ReloadAssetListView(SearchBar.Text);
            }
        }

        private void SearchBar_Enter(object sender, EventArgs e)
        {
            if (!isSearchTouched)
            {
                isSearchTouched = true;
                SearchBar.Text = string.Empty;
            }
        }
    }
}
