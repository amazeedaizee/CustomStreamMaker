using System;
using System.IO;
using System.Windows.Forms;

namespace CustomStreamMaker
{
    public partial class MissingFileMessage : Form
    {
        internal bool isIgnoreAll;
        internal CustomAsset missing;
        internal string newPath;
        public MissingFileMessage(CustomAsset missing)
        {
            this.missing = missing;
            InitializeComponent();
        }

        private string LoadFileType()
        {
            switch (missing.customAssetFileType)
            {
                case CustomAssetFileType.ImageFile:
                    return "Image";
                case CustomAssetFileType.AddressableBundle:
                    return "Addressable Bundle";
                default:
                    return "Asset Bundle";
            }
        }

        private void MissingFileMessage_Load(object sender, EventArgs e)
        {
            var filePath = missing.filePath.Replace("?", "");
            AssetType_Label.Text += " " + missing.customAssetType.ToString();
            Asset_Name_Label.Text += " " + missing.fileName;
            FileType_Label.Text += " " + LoadFileType();
            PastFilePath_Text.Text = filePath;
            var textHeight = (int)(8 * Math.Floor((double)(PastFilePath_Text.Text.Length / 99)));
            if (missing.customAssetFileType != CustomAssetFileType.ImageFile)
            {
                AddAddressable.Text = "Add As Addressable";
                AddAsAssetBundle.Visible = true;
            }
            else
            {
                AddAddressable.Text = "Add As Image";
                AddAsAssetBundle.Visible = false;
            }
            Size = new System.Drawing.Size(534, 362 + textHeight);
            PastFilePath_Text.Size = new System.Drawing.Size(413, 23 + textHeight);
            CustomAsset_Group.Size = new System.Drawing.Size(491, 119 + textHeight);
        }

        private string OpenNewPath()
        {
            OpenFileDialog openNsoStream = new OpenFileDialog();
            string setDirectory;
            if (missing.customAssetType == CustomAssetType.Background)
                setDirectory = string.IsNullOrEmpty(Properties.Settings.Default.ImageDirectory) ? Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) : Properties.Settings.Default.ImageDirectory;
            else setDirectory = string.IsNullOrEmpty(Properties.Settings.Default.BundleDirectory) ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : Properties.Settings.Default.BundleDirectory;
            openNsoStream.InitialDirectory = setDirectory;
            openNsoStream.Filter = missing.customAssetType == CustomAssetType.Background ? "png File (*.png)|*.png|jpg File (*.jpg)|*.jpg" : "All Files (*.*)|*.*";
            openNsoStream.FilterIndex = 1;
            openNsoStream.RestoreDirectory = true;
            if (openNsoStream.ShowDialog() == DialogResult.OK)
            {
                if (missing.customAssetType == CustomAssetType.Background)
                    Properties.Settings.Default.ImageDirectory = Path.GetDirectoryName(openNsoStream.FileName);
                else Properties.Settings.Default.BundleDirectory = Path.GetDirectoryName(openNsoStream.FileName);
                return openNsoStream.FileName;
            }
            return "";
        }

        private void SetNewAddressablePath()
        {
            var path = NewFilePath_Text.Text;
            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("Please type in a valid path.");
                return;
            }
            if (!File.Exists(path))
            {
                MessageBox.Show("Selected path doesn't exist.");
                return;
            }
            if (string.IsNullOrEmpty(CustomAssetExtractor.catalogPath))
            {
                var confirm = MessageBox.Show("Catalog for Addressables hasn't been initialized yet. Do you want to set the catalog now?", "Wait!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    if (!CustomAssetExtractor.InitializeCatalogPath())
                        return;
                }
                else return;
            }
            string catalog = File.ReadAllText(CustomAssetExtractor.catalogPath);
            if (!catalog.Contains(Path.GetFileName(path)))
            {
                MessageBox.Show("File is invalid as an Addressable Bundle. (either file isn't an Addressable Bundle, or the catalog does not contain this file)");
                return;
            }
            if (!CustomAssetExtractor.CheckIfClipExists(path, missing.fileName, out var message))
            {
                MessageBox.Show(message);
                return;
            }
            missing.customAssetFileType = CustomAssetFileType.AddressableBundle;
            missing.catalogPath = CustomAssetExtractor.catalogPath;
            newPath = path;
            Close();
        }

        private void SetNewAssetBundlePath()
        {
            var path = NewFilePath_Text.Text;
            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("Please type in a valid path.");
                return;
            }
            if (!File.Exists(path))
            {
                MessageBox.Show("Selected path doesn't exist.");
                return;
            }
            if (!CustomAssetExtractor.CheckIfClipExists(path, missing.fileName, out var message))
            {
                MessageBox.Show(message);
                return;
            }
            missing.customAssetFileType = CustomAssetFileType.AssetBundle;
            missing.catalogPath = null;
            newPath = path;
            Close();
        }


        private void SetNewImagePath()
        {
            var path = NewFilePath_Text.Text;
            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("Please type in a valid path.");
                return;
            }
            if (!File.Exists(path))
            {
                MessageBox.Show("Selected path doesn't exist.");
                return;
            }
            if (!CustomAssetExtractor.CheckIfClipExists(path, missing.fileName, out var message))
            {
                MessageBox.Show(message);
                return;
            }
            newPath = path;
            Close();
        }

        private void NewFilePath_Text_DoubleClick(object sender, EventArgs e)
        {
            NewFilePath_Text.Text = OpenNewPath();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            NewFilePath_Text.Text = OpenNewPath();
        }

        private void AddAddressable_Click(object sender, EventArgs e)
        {
            if (missing.customAssetFileType == CustomAssetFileType.ImageFile)
                SetNewImagePath();
            else SetNewAddressablePath();
        }

        private void AddAsAssetBundle_Click(object sender, EventArgs e)
        {
            SetNewAssetBundlePath();
        }

        private void DeleteAsset_Click(object sender, EventArgs e)
        {
            newPath = "";
            Close();
        }

        private void IgnoreAllAsset_Click(object sender, EventArgs e)
        {
            newPath = "";
            isIgnoreAll = true;
            Close();
        }
    }
}
