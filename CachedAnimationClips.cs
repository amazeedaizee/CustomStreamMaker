using System;
using System.IO;
using System.Windows.Forms;

namespace CustomStreamMaker
{
    public partial class CachedAnimationClips : Form
    {
        string customPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CustomStreamMaker\CustomAnimationClips";
        public CachedAnimationClips()
        {
            InitializeComponent();
        }

        private void CachedAnimationClips_Load(object sender, EventArgs e)
        {
            LoadCachedPreview();
            if (!CheckIfNodesAreEmpty())
            {
                MessageBox.Show("Could not load animation clips:\n\nNo cached animation clips currently exist.", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
        private void LoadCachedPreview()
        {
            if (!Directory.Exists(customPath))
                return;
            var directories = Directory.GetDirectories(customPath);
            if (directories.Length == 0)
                return;
            foreach (var path in directories)
            {
                var pathName = Path.GetFileName(path);
                var bundleInfo = File.ReadAllText(Path.Combine(path, "bundlePath.txt"));
                if (bundleInfo.Contains("\n"))
                {
                    var catalog = CustomAssetExtractor.catalogPath;
                    var catalogPath = bundleInfo.Split('\n')[1];
                    if (string.IsNullOrEmpty(catalog) || !catalogPath.Contains(catalog))
                        continue;
                    pathName += " (Addressable)";
                }
                var node = CachedAnimationsTreeView.Nodes.Add(pathName);
                var files = Directory.GetFiles(path);
                if (files.Length == 0)
                    continue;
                foreach (var file in files)
                {
                    if (Path.HasExtension(file)) continue;
                    node.Nodes.Add(Path.GetFileName(file));
                }
            }
        }

        private bool CheckIfNodesAreEmpty()
        {
            if (CachedAnimationsTreeView.Nodes.Count == 0) return false;
            bool hasSubNodes = false;
            foreach (TreeNode node in CachedAnimationsTreeView.Nodes)
            {
                if (node.Nodes.Count == 0)
                    continue;
                hasSubNodes = true;
            }
            return hasSubNodes;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            SetCustomAssets();
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SetCustomAssets()
        {
            foreach (TreeNode node in CachedAnimationsTreeView.Nodes)
            {
                var subDirectory = node.Text.Replace(" (Addressable)", "");
                if (node.Nodes.Count == 0)
                    continue;
                foreach (TreeNode subNode in node.Nodes)
                {

                    var fileName = subNode.Text;
                    var bundleInfo = Path.Combine(customPath, subDirectory, "bundlePath.txt");
                    string bundlePath = File.ReadAllText(bundleInfo);
                    if (bundlePath.Contains("\n"))
                        bundlePath = bundlePath.Split('\n')[0];
                    if (CustomAssetExtractor.customAssets.Exists(a => a.filePath == bundlePath && a.fileName == fileName))
                        continue;
                    var importPic = new CustomAsset(CustomAssetType.Sprite, node.Text.Contains(" (Addressable)") ? CustomAssetFileType.AddressableBundle : CustomAssetFileType.AssetBundle, fileName, bundlePath);
                    CustomAssetExtractor.customAssets.Add(importPic);
                }
            }
        }
        private void CachedAnimationClips_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }

        private void CachedAnimationsTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                CachedAnimationsTreeView.SelectedNode.Remove();
            }
        }
    }
}
