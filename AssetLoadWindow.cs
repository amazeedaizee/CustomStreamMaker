using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomStreamMaker
{
    public partial class AssetLoadWindow : Form
    {
        internal Progress<int> progress = new();
        public AssetLoadWindow()
        {
            InitializeComponent();
        }

        private async void AssetLoadWindow_Load(object sender, EventArgs e)
        {

        }

        public async Task<bool> StartCachingAssets()
        {
            await LoadAssetsToMemory(progress);
            return true;
        }
        private async Task LoadAssetsToMemory(IProgress<int> progress)
        {
            CreateNewDirectoryIfNull();
            progress.Report(0);
            await Task.Run(AssetExtractor.CacheBackgrounds);
            progress.Report(50);
            await Task.Run(AssetExtractor.CacheSprites);
            progress.Report(100);
            await Task.Run(AssetExtractor.CacheAudio);
            bool isAllCachedToMemory = AssetExtractor.SaveToMemory && (AssetExtractor.CachedBackgrounds.Count < 12 || AssetExtractor.CachedSprites.Count < 352 || AssetExtractor.CachedMusic.Count < 128);
            bool isAllCachedToFiles = !AssetExtractor.SaveToMemory && (Directory.GetFiles(AssetExtractor.BackgroundDirectory).Length < 12 || Directory.GetFiles(AssetExtractor.SpriteDirectory).Length < 352 || Directory.GetFiles(AssetExtractor.AudioDirectory).Length < 128);
            if (isAllCachedToMemory || isAllCachedToFiles)
            {
                MessageBox.Show("Some assets were either not loaded properly or are missing. Previews of these broken assets are disabled.", "Error");
            }
            else Properties.Settings.Default.IsAssetsLoaded = true;
            Properties.Settings.Default.Save();

        }
        private void CreateNewDirectoryIfNull()
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CustomStreamMaker";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                Directory.CreateDirectory(dir + @"\CachedBackgrounds");
                Directory.CreateDirectory(dir + @"\CachedSprites");
                Directory.CreateDirectory(dir + @"\CachedAudio");
            }

        }
        private async void AssetLoadWindow_Shown(object sender, EventArgs e)
        {
            progress = new Progress<int>(value => { AssetToMem_Progress.Value = value; });
            await StartCachingAssets();
            Close();
            Dispose();
        }

        private void AssetLoadWindow_Enter(object sender, EventArgs e)
        {

        }

        private void AssetLoadWindow_Activated(object sender, EventArgs e)
        {

        }

        private void AssetLoadWindow_Validated(object sender, EventArgs e)
        {

        }
    }
}
