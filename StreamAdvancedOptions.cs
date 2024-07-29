using System;
using System.IO;
using System.Windows.Forms;

namespace CustomStreamMaker
{
    public partial class StreamAdvancedOptions : Form
    {
        StreamEditor editor;
        public StreamAdvancedOptions(StreamEditor streamEditor)
        {
            editor = streamEditor;
            InitializeComponent();
        }

        private void StreamAdvancedOptions_Load(object sender, EventArgs e)
        {
            HasIntro_Check.Checked = editor.settings.IsIntroPlaying;
            IsDarkAngelIntro_Check.Checked = editor.settings.IsDarkAngelPlaying;
            HasCustomFollowers_Check.Checked = editor.settings.HasCustomFollowerCount;
            CustomFollowers_Numeric.Value = editor.settings.CustomFollowerCount;
            InvertColors_Check.Checked = editor.settings.IsInvertedColors;
            RemoveBorders_Check.Checked = editor.settings.isBordersOff;
            IsDarkAngelIntro_Check.Enabled = HasIntro_Check.Enabled;
            CustomFollowers_Numeric.Enabled = HasCustomFollowers_Check.Checked;
            HasEndScreen_Check.Checked = editor.settings.hasEndScreen;
            CustomDay_Numeric.Enabled = CustomDay_Check.Checked;
            CustomDay_Check.Checked = editor.settings.HasCustomDay;
            CustomDay_Numeric.Value = editor.settings.CustomDay;
            HasCustomEndScreen_Check.Enabled = HasEndScreen_Check.Enabled;
            HasCustomEndScreen_Check.Checked = editor.settings.HasCustomEndScreen;
            OpenEndScreenImg_Button.Enabled = HasCustomEndScreen_Check.Checked;
            CustomEndScreen_Text.Enabled = HasCustomEndScreen_Check.Checked;
            if (File.Exists(editor.settings.CustomEndScreenPath))
                CustomEndScreen_Text.Text = editor.settings.CustomEndScreenPath;
            else CustomEndScreen_Text.Text = "";
            GameChair_Checked.Checked = editor.settings.HasChair;
            DarkStream_Check.Checked = editor.settings.hasDarkInterface;
        }

        private void HasIntro_Check_CheckedChanged(object sender, EventArgs e)
        {
            editor.settings.IsIntroPlaying = HasIntro_Check.Checked;

        }

        private void IsDarkAngelIntro_Check_CheckedChanged(object sender, EventArgs e)
        {
            editor.settings.IsDarkAngelPlaying = IsDarkAngelIntro_Check.Checked;
        }

        private void HasCustomFollowers_Check_CheckedChanged(object sender, EventArgs e)
        {
            editor.settings.HasCustomFollowerCount = HasCustomFollowers_Check.Checked;
            CustomFollowers_Numeric.Enabled = HasCustomFollowers_Check.Checked;
        }

        private void CustomFollowers_Numeric_ValueChanged(object sender, EventArgs e)
        {
            editor.settings.CustomFollowerCount = (int)CustomFollowers_Numeric.Value;
        }

        private void InvertColors_Check_CheckedChanged(object sender, EventArgs e)
        {
            editor.settings.IsInvertedColors = InvertColors_Check.Checked;
        }

        private void RemoveBorders_Check_CheckedChanged(object sender, EventArgs e)
        {
            editor.settings.isBordersOff = RemoveBorders_Check.Checked;
        }

        private void StreamAdvancedOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            ValidateCustomEndScreen();
            Dispose();
        }

        private void HasEndScreen_Check_CheckedChanged(object sender, EventArgs e)
        {
            editor.settings.hasEndScreen = HasEndScreen_Check.Checked;
            if (!HasEndScreen_Check.Checked)
                HasCustomEndScreen_Check.Checked = false;
            HasCustomEndScreen_Check.Enabled = HasEndScreen_Check.Checked && HasEndScreen_Check.Enabled;
        }

        private void CustomDay_Check_CheckedChanged(object sender, EventArgs e)
        {
            editor.settings.HasCustomDay = CustomDay_Check.Checked;
            CustomDay_Numeric.Enabled = CustomDay_Check.Checked;
        }

        private void CustomDay_Numeric_ValueChanged(object sender, EventArgs e)
        {
            editor.settings.CustomDay = (int)CustomDay_Numeric.Value;
        }

        private void HasCustomEndScreen_Check_CheckedChanged(object sender, EventArgs e)
        {
            editor.settings.HasCustomEndScreen = HasCustomEndScreen_Check.Checked;
            CustomEndScreen_Text.Enabled = HasCustomEndScreen_Check.Checked;
            OpenEndScreenImg_Button.Enabled = HasCustomEndScreen_Check.Checked;
        }

        private void ValidateCustomEndScreen()
        {
            if (!editor.settings.HasCustomEndScreen)
            {
                CustomEndScreen_Text.Text = "";
                return;
            }
            if (!File.Exists(editor.settings.CustomEndScreenPath))
            {
                CustomEndScreen_Text.Text = "";
                return;
            }
            editor.settings.CustomEndScreenPath = CustomEndScreen_Text.Text;
        }
        private void CustomEndScreen_Text_Leave(object sender, EventArgs e)
        {
            ValidateCustomEndScreen();
        }

        private void CustomEndScreen_Text_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ActiveControl = null;
            }
        }

        private void OpenEndScreenImg_Button_Click(object sender, EventArgs e)
        {
            SetCustomEndScreenFile();
        }

        private void CustomEndScreen_Text_DoubleClick(object sender, EventArgs e)
        {
            SetCustomEndScreenFile();
        }

        private void SetCustomEndScreenFile()
        {
            OpenFileDialog openNsoStream = new OpenFileDialog();
            openNsoStream.InitialDirectory = string.IsNullOrEmpty(Properties.Settings.Default.EndScreenDirectory) ? Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) : Properties.Settings.Default.EndScreenDirectory;
            openNsoStream.Filter = "png File (*.png)|*.png|jpg File (*.jpg)|*.jpg";
            openNsoStream.FilterIndex = 1;
            openNsoStream.RestoreDirectory = true;
            if (openNsoStream.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.EndScreenDirectory = Path.GetDirectoryName(openNsoStream.FileName);
                if (!CustomAssetExtractor.CheckIfImageFileExists(openNsoStream.FileName, out var message))
                {
                    MessageBox.Show(message, "Could not load image file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CustomEndScreen_Text.Text = openNsoStream.FileName;
                editor.settings.CustomEndScreenPath = CustomEndScreen_Text.Text;
            }

        }

        private void GameChair_Checked_CheckedChanged(object sender, EventArgs e)
        {
            editor.settings.HasChair = GameChair_Checked.Checked;
        }

        private void DarkStream_Check_CheckedChanged(object sender, EventArgs e)
        {
            editor.settings.hasDarkInterface = DarkStream_Check.Checked;
            CheckIfDarkEnabled();

        }

        private void CheckIfDarkEnabled()
        {
            HasIntro_Check.Enabled = !editor.settings.hasDarkInterface;
            HasEndScreen_Check.Enabled = !editor.settings.hasDarkInterface;
            GameChair_Checked.Enabled = !editor.settings.hasDarkInterface;
        }

        private void HasIntro_Check_EnabledChanged(object sender, EventArgs e)
        {
            IsDarkAngelIntro_Check.Enabled = HasIntro_Check.Enabled;
        }

        private void HasEndScreen_Check_EnabledChanged(object sender, EventArgs e)
        {
            HasCustomEndScreen_Check.Enabled = HasEndScreen_Check.Enabled;
            CustomEndScreen_Text.Enabled = HasCustomEndScreen_Check.Enabled;
            OpenEndScreenImg_Button.Enabled = HasCustomEndScreen_Check.Enabled;
        }
    }
}
