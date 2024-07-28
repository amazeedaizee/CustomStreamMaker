using NAudio.Wave;
using Newtonsoft.Json;
using NGO;
using ngov3;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomStreamMaker
{
    public enum SoundPlayingAt
    {
        None, StartingMusic, PlayMusic
    }
    public partial class StreamEditor : Form
    {
        internal CustomAssetPreview customAssetPreview = null;

        string _currentFile;

        List<EditHistory> _undoHistory = new();
        List<EditHistory> _redoHistory = new();

        WaveOut soundPreview;
        WaveOut musicPreview;
        SoundPlayingAt soundPlayingAt;
        List<string> animList = new List<string>(ThatOneLongListOfAnimationsOriginallyInTheGame.list);
        internal StreamSettings savedSettings;
        internal StreamSettings settings = new();

        bool playingListChanged;
        int undoCountOnSave = 0;
        PlayingObject lastUndoObjOnSave;

        Dictionary<string, StreamBackground> backgroundDict = new Dictionary<string, StreamBackground>()
        {

                    {"bg_stream", StreamBackground.Default},
                    {"bg_stream_shield_silver", StreamBackground.Silver},
                    {"bg_stream_shield_gold" ,StreamBackground.Gold},
                    {"bg_stream_angel_lv1", StreamBackground.MileOne},
                    {"bg_stream_angel_lv2", StreamBackground.MileTwo},
                    {"bg_stream_angel_lv3", StreamBackground.MileThree},
                    {"bg_stream_angel_lv4", StreamBackground.MileFour},
                    {"bg_stream_angel_lv5", StreamBackground.MileFive},
                    {"bg_stream_kyouso", StreamBackground.Guru},
                    {"bg_stream_horror", StreamBackground.Horror},
                    {"bg_stream_mansion", StreamBackground.BigHouse},
                    {"bg_stream_sayonara", StreamBackground.Roof},
                    {"black", StreamBackground.Black},
                    {"white", StreamBackground.Void}

        };

        int _newPlayingTypeIndex;

        string _newCurrentKAnim;
        string _newCurrentKDialogue;
        bool _newIsHateComment;
        string _newCurrentSE;
        string _newCurrentMusic;
        string _newCurrentEffect;
        string _newCurrentChatComment;
        ChatCommentType _newCurrentChatCommentType;
        string _newCurrentKAnimReply;
        string _newCurrentKReply;
        string _newCurrentHateComment;
        internal List<KAngelSays> _newCurrentSuperReplies;


        internal Image currentBackground;
        string _currentKAnim;
        string _currentKAnimReply;
        string _currentSE;
        string _currentMusic;
        internal List<KAngelSays> _currentSuperReplies = new() { new("stream_cho_akaruku", "") };


        public StreamEditor()
        {
            InitializeComponent();
        }

        private void StreamEditor_Load(object sender, EventArgs e)
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CustomStreamMaker";
            if (!Directory.Exists(dir))
            {
                Properties.Settings.Default.IsAssetsLoaded = false;
                Properties.Settings.Default.Save();
            }

            var loadAssets = new AssetLoadWindow();
            PlaySE_Group.Location = new Point(799, 78);
            PlayMusic_Group.Location = new Point(799, 78);
            BorderEffect_Group.Location = new Point(799, 78);
            KAngelDialogue_Group.Location = new Point(799, 78);
            HateComment_Group.Location = new Point(799, 248);
            KAnim_React_Group.Location = new Point(799, 78);
            if (!(Properties.Settings.Default.IsAssetsLoaded && loadAssets.IsAllLoaded()))
                loadAssets.ShowDialog();
            else loadAssets.Dispose();
            ChangeVisibilityForPlayingObjEditors();
            InitializeLists();
            InitializeDefaultFields();
            SetStreamLoaderVisibility();
            StreamPlayingList.ClearSelection();
            CustomAssetExtractor.LoadCustomAssets();
            CustomAssetExtractor.CheckForMissingFilesAtStart();
            GC.Collect();
        }

        private void SetStreamLoaderVisibility()
        {
            if (!GameLocation.IsGameModded(out string modPath))
                return;
            modPath += @"\CustomStreamLoader";
            streamLoaderToolStripMenuItem.Visible = Directory.Exists(modPath);

        }

        private async Task SetMusicPreview(string audioName)
        {
            StopMusicifPlaying();
            musicPreview = AssetExtractor.GetCachedAudio(audioName);
            if (musicPreview == null)
                return;
            var playAudio = () =>
            {
                musicPreview.Play();
                while (musicPreview != null && musicPreview.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
                Thread.Sleep(100);
            };
            while (musicPreview != null)
            {
                await Task.Run(playAudio);
                if (musicPreview != null && musicPreview.PlaybackState == PlaybackState.Stopped)
                    musicPreview = AssetExtractor.GetCachedAudio(audioName);
            }


        }
        private async Task SetSoundEffectPreview(string audioName)
        {
            StopSoundEffectIfPlaying();
            soundPreview = AssetExtractor.GetCachedAudio(audioName);
            if (soundPreview == null)
                return;
            soundPreview.PlaybackStopped += (object sender, StoppedEventArgs e) => { soundPreview.Dispose(); };
            var playAudio = () =>
            {
                soundPreview.Play();
                while (soundPreview != null && soundPreview.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
                Thread.Sleep(100);
            };
            while (soundPreview != null)
            {
                await Task.Run(playAudio);
                if (soundPreview != null && soundPreview.PlaybackState == PlaybackState.Stopped)
                {
                    soundPreview.Dispose();
                    soundPreview = null;
                    PlayCurrentSE_Button.Text = "▶️";
                }
            }
        }

        private void SetNewSpritePreview(string imgName)
        {
            if (string.IsNullOrEmpty(imgName))
                return;
            SpritePreview.Image?.Dispose();
            SpritePreview.Image = AssetExtractor.GetCachedSprite(imgName);
        }

        private void SetNewBackgroundPreview(string imgName)
        {
            currentBackground?.Dispose();
            SpritePreview.BackgroundImage?.Dispose();
            if (imgName == "black")
            {
                SpritePreview.BackgroundImage = null;
                SpritePreview.BackColor = Color.Black;
            }
            else if (imgName == "white")
            {
                SpritePreview.BackgroundImage = null;
                SpritePreview.BackColor = Color.White;
            }
            else
            {
                SpritePreview.BackColor = Color.FromKnownColor(KnownColor.Control);
                currentBackground = AssetExtractor.GetCachedBackground(imgName);
                SpritePreview.BackgroundImage = currentBackground;
            }

        }

        private StreamBackground SetBackgroundValue(string bgName)
        {
            if (backgroundDict.TryGetValue(bgName, out var value))
            {
                settings.CustomBackground = null;
                return value;
            }
            else
            {
                settings.CustomBackground = CustomAssetExtractor.customAssets.Find(a => a.fileName == bgName && a.customAssetType == CustomAssetType.Background);
                return StreamBackground.None;
            }
        }

        private void InitializeNewStreamSettings()
        {
            settings.StringTitle = string.Empty;
            settings.StartingBackground = backgroundDict[StartingBackground_List.Text];
            settings.StartingAnimation = StartingAnimation_List.Text;
            settings.StartingMusic = (SoundType)Enum.Parse(typeof(SoundType), StartingMusic_List.Text);
            settings.ChatSettings = StreamChatSettings.Normal;
            settings.StartingEffect = (EffectType)Enum.Parse(typeof(EffectType), StartingEffect_List.Text);
            settings.EffectIntensity = 0;
            settings.ReactionAnimation = KAnim_React_List.Text;
        }

        private void InitializeEnabledTrackbar()
        {
            if (settings.StartingEffect == EffectType.Kenjo)
            {
                EffectIntensity_Trackbar.Enabled = false;
                return;
            }
            EffectIntensity_Trackbar.Enabled = true;
        }

        private void SyncExportedSettings()
        {
            StreamTItle_Text.Text = settings.StringTitle;
            StartingAnimation_List.Text = settings.StartingAnimation;
            StartingMusic_List.Text = settings.StartingMusic.ToString();
            if (settings.StartingBackground == StreamBackground.None)
            {
                if (settings.CustomBackground != null && File.Exists(settings.CustomBackground.filePath))
                {
                    StartingBackground_List.Text = settings.CustomBackground.fileName;
                }
                else StartingBackground_List.SelectedIndex = 0;
            }
            else StartingBackground_List.SelectedIndex = (int)settings.StartingBackground;
            StartingEffect_List.SelectedIndex = (int)settings.StartingEffect;
            EffectIntensity_Trackbar.Value = (int)settings.EffectIntensity * 100;
            switch (settings.ChatSettings)
            {
                case StreamChatSettings.Normal:
                    ChatStatusNormal_Radio.Checked = true;
                    break;
                case StreamChatSettings.Celebration:
                    ChatStatusParty_Radio.Checked = true;
                    break;
                case StreamChatSettings.Uncontrollable:
                    ChatStatusUncontrol_Radio.Checked = true;
                    break;
            }
            StreamPlayingList.Rows.Clear();
            for (int i = 0; i < settings.PlayingList.Count; i++)
            {
                var playingObject = settings.PlayingList[i];
                var row = StreamPlayingList.Rows.Add(SetPlayingType(playingObject));
                var desc = SetPlayingDesc(playingObject);
                StreamPlayingList.Rows[row].Cells[1].Value = desc;
            }
            SetNewBackgroundPreview(StartingBackground_List.Text);
            SetNewSpritePreview(StartingAnimation_List.Text);
        }
        private void InitializeLists()
        {
            KAnim_List.Items.AddRange(ThatOneLongListOfAnimationsOriginallyInTheGame.list);
            KAnim_SuperReply_List.Items.AddRange(ThatOneLongListOfAnimationsOriginallyInTheGame.list);
            StartingAnimation_List.Items.AddRange(ThatOneLongListOfAnimationsOriginallyInTheGame.list);
            KAnim_React_List.Items.AddRange(ThatOneLongListOfAnimationsOriginallyInTheGame.list);
            StartingMusic_List.Items.AddRange(CreateMusicList(true));
            PlayMusic_List.Items.AddRange(CreateMusicList(true));
            PlaySE_List.Items.AddRange(CreateMusicList(false));
            StartingEffect_List.Items.AddRange(Enum.GetNames(typeof(EffectType)));
            BorderEffect_List.Items.AddRange(CreateEffectList());
            StartingBackground_List.Items.AddRange(CreateBackgroundList());
        }

        private void InitializeDefaultFields()
        {
            StreamTItle_Text.Text = "";
            StartingAnimation_List.Text = "stream_cho_akaruku";
            KAnim_SuperReply_List.Text = "stream_cho_akaruku";
            KAnim_React_List.Text = "stream_cho_reaction1";
            KAnim_List.Text = "stream_cho_akaruku";
            _currentKAnim = "stream_cho_akaruku";
            _currentKAnimReply = "stream_cho_akaruku";
            ChatStatusNormal_Radio.Checked = true;
            StartingMusic_List.Text = "BGM_mainloop_normal";
            PlayMusic_List.Text = "BGM_mainloop_normal";
            _currentMusic = "BGM_mainloop_normal";
            PlaySE_List.Text = "SE_Tetehen";
            _currentSE = "SE_Tetehen";
            StartingEffect_List.Text = "Kenjo";
            EffectIntensity_Trackbar.Value = 0;
            BorderEffect_In_Radio.Checked = true;
            BorderEffect_List.Text = "None";
            StartingBackground_List.Text = "bg_stream";
            HateComment_Text.Text = string.Empty;
            KAngelDialogue_Text.Text = string.Empty;
            ChatComment_TextBox.Text = string.Empty;
            IsStressComment_Check.Checked = false;
            IsSuperChat_Check.Checked = false;
            KAngelReply_TextBox.Text = string.Empty;
            EnableSuperChatReply();
            EnableHateCallout();
            InitializeNewStreamSettings();
            InitializeEnabledTrackbar();
            InitializeEffectIntensity();
            SetNewBackgroundPreview("bg_stream");
            SetNewSpritePreview("stream_cho_akaruku");
        }

        private string[] CreateEffectList()
        {
            string[] s =
                {
                   "None",
                   "Kenjo",
                   "Body",
                   "Chainsaw",
                   "Danger",
                   "Gothic",
                   "Porori",
                   "Ide"
                };
            return s;
        }

        private string[] CreateBackgroundList()
        {
            string[] s =
                {
                    "bg_stream",
                    "bg_stream_shield_silver",
                    "bg_stream_shield_gold",
                    "bg_stream_angel_lv1",
                    "bg_stream_angel_lv2",
                    "bg_stream_angel_lv3",
                    "bg_stream_angel_lv4",
                    "bg_stream_angel_lv5",
                    "bg_stream_kyouso",
                    "bg_stream_horror",
                    "bg_stream_mansion",
                    "bg_stream_sayonara",
                    "black",
                    "white"
                };
            return s;
        }
        private string[] CreateMusicList(bool isBGM)
        {
            List<string> list = new List<string>(Enum.GetNames(typeof(SoundType)));

            return isBGM ? list.FindAll(s => s.StartsWith("BGM_")).ToArray() : list.FindAll(s => s.StartsWith("SE_")).ToArray();
        }

        private void InitializeEffectIntensity()
        {
            if (!EffectIntensity_Trackbar.Enabled)
            {
                settings.EffectIntensity = 0;
                IntensityNum.Text = "0";
                return;
            }
            settings.EffectIntensity = (EffectIntensity_Trackbar.Value / 100f);
            IntensityNum.Text = settings.EffectIntensity.ToString();
        }

        private void EnableSuperChatReply()
        {
            SuperChatReply_Group.Enabled = IsSuperChat_Check.Checked;
            ChangeFileLabelIfUnsaved();
        }

        private void EnableHateCallout()
        {
            HateComment_Group.Enabled = IsHateCallout_Check.Checked;
            ChangeFileLabelIfUnsaved();
        }

        private void EffectIntensity_Trackbar_Scroll(object sender, EventArgs e)
        {
            InitializeEffectIntensity();
            ChangeFileLabelIfUnsaved();
        }

        private void ChatStatusParty_Radio_CheckedChanged(object sender, EventArgs e)
        {
            settings.ChatSettings = StreamChatSettings.Celebration;
            ChangeFileLabelIfUnsaved();
        }

        private void ChatStatusUncontrol_Radio_CheckedChanged(object sender, EventArgs e)
        {
            settings.ChatSettings = StreamChatSettings.Uncontrollable;
            ChangeFileLabelIfUnsaved();
        }

        private void ChatStatusNormal_Radio_CheckedChanged(object sender, EventArgs e)
        {
            settings.ChatSettings = StreamChatSettings.Normal;
            ChangeFileLabelIfUnsaved();
        }

        private void ChangeVisibilityForPlayingObjEditors()
        {
            AddSaveToPlayingList_Button.Enabled = true;
            switch (PlayingType_List.SelectedIndex)
            {
                case 0:
                    KAngelDialogue_Group.Visible = true;
                    HateComment_Group.Visible = true;
                    ChatComment_Group.Visible = false;
                    SuperChatReply_Group.Visible = false;
                    PlaySE_Group.Visible = false;
                    PlayMusic_Group.Visible = false;
                    BorderEffect_Group.Visible = false;
                    KAnim_React_Group.Visible = false;
                    EnableHateCallout();
                    SetNewSpritePreview(_currentKAnim);
                    return;
                case 1:
                    KAngelDialogue_Group.Visible = false;
                    HateComment_Group.Visible = false;
                    ChatComment_Group.Visible = true;
                    SuperChatReply_Group.Visible = true;
                    PlaySE_Group.Visible = false;
                    PlayMusic_Group.Visible = false;
                    BorderEffect_Group.Visible = false;
                    KAnim_React_Group.Visible = false;
                    EnableSuperChatReply();
                    return;
                case 2:
                    KAngelDialogue_Group.Visible = false;
                    HateComment_Group.Visible = false;
                    ChatComment_Group.Visible = false;
                    SuperChatReply_Group.Visible = false;
                    PlaySE_Group.Visible = true;
                    PlayMusic_Group.Visible = false;
                    BorderEffect_Group.Visible = false;
                    KAnim_React_Group.Visible = false;
                    return;
                case 3:
                    KAngelDialogue_Group.Visible = false;
                    HateComment_Group.Visible = false;
                    ChatComment_Group.Visible = false;
                    SuperChatReply_Group.Visible = false;
                    PlaySE_Group.Visible = false;
                    PlayMusic_Group.Visible = true;
                    BorderEffect_Group.Visible = false;
                    KAnim_React_Group.Visible = false;
                    return;
                case 4:
                    KAngelDialogue_Group.Visible = false;
                    HateComment_Group.Visible = false;
                    ChatComment_Group.Visible = false;
                    SuperChatReply_Group.Visible = false;
                    PlaySE_Group.Visible = false;
                    PlayMusic_Group.Visible = false;
                    BorderEffect_Group.Visible = true;
                    KAnim_React_Group.Visible = false;
                    return;
                case 11:
                    KAnim_React_Group.Visible = true;
                    CheckIfReactionAnimExists();
                    break;
                default:
                    KAnim_React_Group.Visible = false;
                    break;
            }
            KAngelDialogue_Group.Visible = false;
            HateComment_Group.Visible = false;
            ChatComment_Group.Visible = false;
            SuperChatReply_Group.Visible = false;
            PlaySE_Group.Visible = false;
            PlayMusic_Group.Visible = false;
            BorderEffect_Group.Visible = false;
        }

        private void PlayingType_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeVisibilityForPlayingObjEditors();
        }

        private void IsStressComment_Check_CheckedChanged(object sender, EventArgs e)
        {
            if (IsStressComment_Check.Checked && IsSuperChat_Check.Checked)
            {
                IsSuperChat_Check.Checked = false;
                EnableSuperChatReply();
            }
        }

        private void IsSuperChat_Check_CheckedChanged(object sender, EventArgs e)
        {
            if (IsStressComment_Check.Checked && IsSuperChat_Check.Checked)
            {
                IsStressComment_Check.Checked = false;
            }
            EnableSuperChatReply();
            ChangeFileLabelIfUnsaved();
        }

        private void IsHateCallout_Check_CheckedChanged(object sender, EventArgs e)
        {
            EnableHateCallout();
            ChangeFileLabelIfUnsaved();
        }

        private bool ValidateBackgroundValue()
        {
            var text = StartingBackground_List.Text;
            if (!(animList.Contains(text) || (CustomAssetExtractor.customAssets.Count > 0 && CustomAssetExtractor.customAssets.Exists(a => a.fileName == text && a.customAssetType == CustomAssetType.Background))))
            {
                try
                {
                    StartingBackground_List.SelectedItem = StartingBackground_List.Items[(int)settings.StartingBackground];
                }
                catch { StartingBackground_List.Text = "bg_stream"; }
                return false;
            }
            StartingBackground_List.SelectedItem = text;
            return true;
        }

        private bool ValidateSoundValue(ComboBox box, ref SoundType type, bool isBGM)
        {
            var text = box.Text;
            if (!CreateMusicList(isBGM).Contains(text))
            {
                box.SelectedItem = type.ToString();
                return false;
            }
            box.SelectedItem = text;
            return true;
        }


        internal bool ValidateStartingAnimationValue(ComboBox box, ref string s)
        {
            var forbiddenList = ThatOneLongListOfAnimationsOriginallyInTheGame.forbidden;
            var text = box.Text;
            if (!(animList.Contains(text) || (CustomAssetExtractor.customAssets.Count > 0 && CustomAssetExtractor.customAssets.Exists(a => a.fileName == text && a.customAssetType == CustomAssetType.Sprite))) || forbiddenList.Contains(text))
            {
                box.Text = s;
                return false;
            }
            box.Text = text;
            return true;
        }
        internal bool ValidateAnimationValue(ref ComboBox box, ref string s)
        {
            var forbiddenList = ThatOneLongListOfAnimationsOriginallyInTheGame.forbidden;
            var text = box.Text;
            if (!(animList.Contains(text) || text == "" || (CustomAssetExtractor.customAssets.Count > 0 && CustomAssetExtractor.customAssets.Exists(a => a.fileName == text && a.customAssetType == CustomAssetType.Sprite))) || forbiddenList.Contains(text))
            {
                box.Text = s;
                return false;
            }
            box.Text = text;
            return true;
        }
        private void StartingAnimation_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.StartingAnimation = (string)StartingAnimation_List.SelectedItem;
            SetNewSpritePreview(settings.StartingAnimation);
            ChangeFileLabelIfUnsaved();
        }

        private void StartingAnimation_List_Leave(object sender, EventArgs e)
        {
            StartingAnimation_List.Text.ToLower();
            ValidateStartingAnimationValue(StartingAnimation_List, ref settings.StartingAnimation);
            settings.StartingAnimation = StartingAnimation_List.Text;
            if (!animList.Contains(settings.StartingAnimation) && (CustomAssetExtractor.customAssets.Count > 0 && CustomAssetExtractor.customAssets.Exists(a => a.fileName == settings.StartingAnimation && a.customAssetType == CustomAssetType.Sprite)))
            {
                var refAsset = CustomAssetExtractor.customAssets.Find(a => a.fileName == settings.StartingAnimation && a.customAssetType == CustomAssetType.Sprite);
                settings.CustomStartingAnimation = new CustomAsset(CustomAssetType.Sprite, refAsset.customAssetFileType, refAsset.fileName, refAsset.filePath);
                settings.CustomStartingAnimation.picWidth = refAsset.picWidth;
                settings.CustomStartingAnimation.picHeight = refAsset.picHeight;
            }
            else settings.CustomStartingAnimation = null;
            SetNewSpritePreview(settings.StartingAnimation);
            ChangeFileLabelIfUnsaved();
        }

        private void StreamMenuStrip_Click(object sender, EventArgs e)
        {
            Focus();
        }

        private void StartingMusic_List_Leave(object sender, EventArgs e)
        {
            ValidateSoundValue(StartingMusic_List, ref settings.StartingMusic, true);
            settings.StartingMusic = (SoundType)Enum.Parse(typeof(SoundType), (string)StartingMusic_List.Text);
            StopMusicifPlaying();
            ChangeFileLabelIfUnsaved();
        }

        private void PlaySE_List_Leave(object sender, EventArgs e)
        {
            var type = (SoundType)Enum.Parse(typeof(SoundType), _currentSE);
            ValidateSoundValue(PlaySE_List, ref type, false);
            _currentSE = (string)PlaySE_List.Text;
        }

        private void PlayMusic_List_Leave(object sender, EventArgs e)
        {
            var type = (SoundType)Enum.Parse(typeof(SoundType), _currentMusic);
            ValidateSoundValue(PlayMusic_List, ref type, true);
            _currentMusic = (string)PlayMusic_List.Text;

        }

        private void StartingEffect_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.StartingEffect = (EffectType)Enum.Parse(typeof(EffectType), (string)StartingEffect_List.SelectedItem);
            InitializeEnabledTrackbar();
            InitializeEffectIntensity();
            ChangeFileLabelIfUnsaved();
        }

        private void StreamEditor_Click(object sender, EventArgs e)
        {
            ActiveControl = null;
            StreamPlayingList.ClearSelection();
        }

        private void StartingBackground_List_Leave(object sender, EventArgs e)
        {
            StartingBackground_List.Text.ToLower();
            ValidateBackgroundValue();
            settings.StartingBackground = SetBackgroundValue(StartingBackground_List.Text);
            SetNewBackgroundPreview(StartingBackground_List.Text);
            ChangeFileLabelIfUnsaved();
        }

        private void ValidateCustomValuesForPlayingList()
        {
            var bgm = (SoundType)Enum.Parse(typeof(SoundType), _currentMusic);
            var se = (SoundType)Enum.Parse(typeof(SoundType), _currentSE);
            ValidateSoundValue(PlayMusic_List, ref bgm, true);
            _currentMusic = (string)PlayMusic_List.SelectedItem;
            ValidateSoundValue(PlaySE_List, ref se, false);
            _currentSE = (string)PlaySE_List.SelectedItem;
            KAnim_List.Text.ToLower();
            ValidateAnimationValue(ref KAnim_List, ref _currentKAnim);
            _currentKAnim = KAnim_List.Text;
            KAnim_SuperReply_List.Text.ToLower();
            ValidateAnimationValue(ref KAnim_SuperReply_List, ref _currentKAnimReply);
            _currentKAnimReply = KAnim_SuperReply_List.Text;
        }
        private void SaveToPlayingList(bool isInsertNew = false, int index = 0)
        {
            ValidateCustomValuesForPlayingList();
            PlayingObject playObj = new ChatGeneral(PlayingType.ReadSuperChats);
            switch (PlayingType_List.SelectedIndex)
            {
                case 0:
                    playObj = ApplyKAngelDialogue();
                    break;
                case 1:
                    playObj = ApplyChatComment();
                    break;
                case 2:
                    SoundType se = (SoundType)Enum.Parse(typeof(SoundType), _currentSE);
                    playObj = new PlaySound(se);
                    break;
                case 3:
                    SoundType bgm = (SoundType)Enum.Parse(typeof(SoundType), _currentMusic);
                    playObj = new PlaySound(bgm);
                    break;
                case 4:
                    ChanceEffectType e = (ChanceEffectType)Enum.Parse(typeof(ChanceEffectType), BorderEffect_List.Text);
                    var bEffect = ApplyBorderTransition();
                    playObj = new PlayEffect(e, bEffect);
                    break;
                case 5:
                    playObj = new ChatGeneral(PlayingType.ChatFirst);
                    break;
                case 6:
                    playObj = new ChatGeneral(PlayingType.ChatMiddle);
                    break;
                case 7:
                    playObj = new ChatGeneral(PlayingType.ChatLast);
                    break;
                case 8:
                    playObj = new ChatGeneral(PlayingType.ChatRainbow);
                    break;
                case 9:
                    playObj = new ChatGeneral(PlayingType.ChatDelete);
                    break;
                case 10:
                    playObj = new ChatGeneral(PlayingType.ChatDeleteAll);
                    break;
                case 11:
                    playObj = new ChatGeneral(PlayingType.ReadSuperChats);
                    CheckIfReactionAnimExists();
                    AddSaveToPlayingList_Button.Enabled = false;
                    break;
            }
            if (isInsertNew)
            {
                AddToUndoHistory(EditType.Add, index, playObj);
                InsertToPlayingListView(index, playObj, false);
            }
            else if (StreamPlayingList.SelectedRows.Count > 0)
            {
                index = StreamPlayingList.SelectedRows[0].Index;
                AddToUndoHistory(EditType.Edit, index, settings.PlayingList[index], index, playObj);
                InsertToPlayingListView(index, playObj);
                if (index == settings.PlayingList.Count - 1)
                    StreamPlayingList.ClearSelection();
            }
            else
            {
                AddToUndoHistory(EditType.Add, settings.PlayingList.Count, playObj);
                AddToPlayingListView(playObj);
                StreamPlayingList.ClearSelection();
            }
        }

        private void AddToPlayingListView(PlayingObject playingObject)
        {
            settings.PlayingList.Add(playingObject);
            var row = StreamPlayingList.Rows.Add(SetPlayingType(playingObject));
            var desc = SetPlayingDesc(playingObject);
            StreamPlayingList.Rows[row].Cells[1].Value = desc;

        }

        private void InsertToPlayingListView(int index, PlayingObject playingObject, bool isReplacingExisting = true)
        {
            if (isReplacingExisting)
                DeleteFromPlayingListView(index);
            settings.PlayingList.Insert(index, playingObject);
            StreamPlayingList.Rows.Insert(index, SetPlayingType(playingObject));
            var desc = SetPlayingDesc(playingObject);
            StreamPlayingList.Rows[index].Cells[1].Value = desc;

        }

        private void DeleteFromPlayingListView(int index)
        {
            settings.PlayingList.RemoveAt(index);
            StreamPlayingList.Rows.RemoveAt(index);
            ChangeFileLabelIfUnsaved();
        }

        string SetPlayingType(PlayingObject playingObject)
        {
            switch (playingObject.PlayingType)
            {
                case PlayingType.KAngelSays:
                    return "KAngel Dialogue";
                case PlayingType.KAngelCallout:
                    return "KAngel Hater Callout";
                case PlayingType.ChatSays:
                    return "Chat Comment";
                case PlayingType.ChatSuper:
                    return "Super Chat";
                case PlayingType.ChatBad:
                    return "Stressful Chat Comment";
                case PlayingType.PlaySE:
                    return "Sound Effect";
                case PlayingType.PlayBGM:
                    return "Music Change";
                case PlayingType.PlayEffect:
                    return "Border Effect";
                case PlayingType.ChatFirst:
                    return "Random First Comments";
                case PlayingType.ChatMiddle:
                    return "Random Middle Comments";
                case PlayingType.ChatLast:
                    return "Random Last Comments";
                case PlayingType.ChatRainbow:
                    return "Rainbow Super Chat";
                case PlayingType.ChatDelete:
                    return "Delete Last Chat Comment";
                case PlayingType.ChatDeleteAll:
                    return "Delete All (Normal) Chat Comments";
                case PlayingType.ReadSuperChats:
                    return "Start Reading Chosen Super Chats";
                default:
                    return "";
            }
        }

        string SetPlayingDesc(PlayingObject playingObject)
        {
            switch (playingObject.PlayingType)
            {
                case PlayingType.KAngelSays:
                    var kAngel = playingObject as KAngelSays;
                    return $"{kAngel.Dialogue}\n({kAngel.AnimName})";
                case PlayingType.KAngelCallout:
                    var kCallout = playingObject as KAngelCallout;
                    return $"\"{kCallout.HaterComment}\"\n\n{kCallout.Dialogue}\n({kCallout.AnimName})";
                case PlayingType.ChatSays:
                case PlayingType.ChatBad:
                case PlayingType.ChatSuper:
                    var chat = playingObject as ChatSays;
                    List<string> list = new List<string>();
                    if (chat.Replies != null && chat.PlayingType == PlayingType.ChatSuper)
                    {
                        for (int i = 0; i < chat.Replies.Count; i++)
                        {
                            list.Add($"- {chat.Replies[i].Dialogue}\n({chat.Replies[i].AnimName})");
                        }
                        return $"{chat.Comment}\n\nReplies:\n\n{string.Join("\n\n", list)}";
                    }
                    return chat.Comment;
                case PlayingType.PlaySE:
                case PlayingType.PlayBGM:
                    var sound = playingObject as PlaySound;
                    return $"Playing: {sound.Audio}";
                case PlayingType.PlayEffect:
                    var effect = playingObject as PlayEffect;
                    return $"Playing Border Effect: {effect.BorderEffect}; \nin state: {effect.BorderEffectType}";
                case PlayingType.ReadSuperChats:
                    return $"Reading with animation: {settings.ReactionAnimation}";
                default: return "-----";


            }
        }

        private PlayingObject ApplyKAngelDialogue()
        {
            var kAngel = new KAngelSays(_currentKAnim, KAngelDialogue_Text.Text);
            if (CustomAssetExtractor.customAssets.Count > 0 && CustomAssetExtractor.customAssets.Exists(a => a.fileName == _currentKAnim && a.customAssetType == CustomAssetType.Sprite))
            {
                var customAsset = CustomAssetExtractor.customAssets.Find(a => a.fileName == _currentKAnim && a.customAssetType == CustomAssetType.Sprite);
                kAngel.SetCustomAnim(customAsset);
            }
            if (IsHateCallout_Check.Checked)
            {
                var kAngelCallout = new KAngelCallout(kAngel, HateComment_Text.Text);
                return kAngelCallout;
            }
            return kAngel;
        }

        private PlayingObject ApplyChatComment()
        {
            var chatCom = new ChatSays(ChatComment_TextBox.Text);
            if (!IsStressComment_Check.Checked && !IsSuperChat_Check.Checked)
                chatCom.SetNormalComment();
            else if (IsStressComment_Check.Checked)
                chatCom.SetBadComment();
            else if (IsSuperChat_Check.Checked)
            {
                List<KAngelSays> newList = new List<KAngelSays>();
                for (int i = 0; i < _currentSuperReplies.Count; i++)
                {
                    CustomAsset customAsset = null;
                    if (i == 0)
                    {
                        if (CustomAssetExtractor.customAssets.Count > 0 && CustomAssetExtractor.customAssets.Exists(a => a.fileName == _currentKAnimReply && a.customAssetType == CustomAssetType.Sprite))
                        {
                            customAsset = CustomAssetExtractor.customAssets.Find(a => a.fileName == _currentKAnimReply && a.customAssetType == CustomAssetType.Sprite);
                        }
                        newList.Add(new(_currentKAnimReply, KAngelReply_TextBox.Text, customAsset));
                        continue;
                    }
                    newList.Add(new(_currentSuperReplies[i].AnimName, _currentSuperReplies[i].Dialogue, _currentSuperReplies[i].customAnim));
                }
                chatCom.SetSuperChat(newList);
            }
            return chatCom;
        }

        private BorderEffectType ApplyBorderTransition()
        {
            if (BorderEffect_In_Radio.Checked)
                return BorderEffectType.EaseIn;
            else if (BorderEffectWinStop_Radio.Checked)
                return BorderEffectType.EaseBeforePlay;
            else if (BorderEffectWin_Radio.Checked)
                return BorderEffectType.Play;
            else return BorderEffectType.EaseOut;
        }

        private void AddSaveToPlayingList_Button_Click(object sender, EventArgs e)
        {
            SaveToPlayingList();

        }

        private void KAnim_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentKAnim = (string)KAnim_List.SelectedItem;
            SetNewSpritePreview(_currentKAnim);
        }

        private void KAnim_SuperReply_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = StreamPlayingList.SelectedRows.Count;
            if (count == 0 || (count > 0 && settings.PlayingList[StreamPlayingList.SelectedRows[0].Index].PlayingType == PlayingType.ChatSuper))
            {
                _currentKAnimReply = (string)KAnim_SuperReply_List.SelectedItem;
                SetNewSpritePreview(_currentKAnimReply);
            }
        }

        private void StreamTItle_Text_TextChanged(object sender, EventArgs e)
        {
            settings.StringTitle = StreamTItle_Text.Text;
            ChangeFileLabelIfUnsaved();
        }

        private void StreamPlayingList_SelectionChanged(object sender, EventArgs e)
        {
            ChangeEditStateBasedOnSelectedObj();
            if (StreamPlayingList.SelectedRows.Count > 0)
            {
                InsertAbove_Button.Visible = true;
                InsertBelow_Button.Visible = true;
                AddSaveToPlayingList_Button.Text = "Save To Playing List";
            }
            else
            {
                InsertAbove_Button.Visible = false;
                InsertBelow_Button.Visible = false;
                if (PlayingType_List.SelectedIndex == 11)
                    CheckIfReactionAnimExists();
                AddSaveToPlayingList_Button.Text = "Add To Playing List";
            }
        }

        private void ChangeEditStateBasedOnSelectedObj()
        {
            if (StreamPlayingList.SelectedRows.Count <= 0) return;
            var index = StreamPlayingList.SelectedRows[0].Index;
            PlayingObject obj = settings.PlayingList[index];
            switch (obj.PlayingType)
            {
                case PlayingType.KAngelSays:
                case PlayingType.KAngelCallout:
                    PlayingType_List.SelectedIndex = 0;
                    var kSays = obj as KAngelSays;
                    KAnim_List.Text = kSays.AnimName;
                    KAngelDialogue_Text.Text = kSays.Dialogue;
                    if (obj.PlayingType == PlayingType.KAngelCallout)
                    {
                        IsHateCallout_Check.Checked = true;
                        var kCallout = kSays as KAngelCallout;
                        HateComment_Text.Text = kCallout.HaterComment;
                    }
                    else
                    {
                        IsHateCallout_Check.Checked = false;
                        HateComment_Text.Text = "";
                    }
                    _currentKAnim = KAnim_List.Text;
                    SetNewSpritePreview(_currentKAnim);
                    break;
                case PlayingType.ChatSays:
                case PlayingType.ChatBad:
                case PlayingType.ChatSuper:
                    PlayingType_List.SelectedIndex = 1;
                    var chat = obj as ChatSays;
                    ChatComment_TextBox.Text = chat.Comment;
                    if (chat.PlayingType == PlayingType.ChatSuper)
                    {
                        _currentSuperReplies.Clear();
                        var firstReply = chat.Replies[0];
                        _currentSuperReplies.Add(firstReply);
                        IsStressComment_Check.Checked = false;
                        IsSuperChat_Check.Checked = true;
                        KAnim_SuperReply_List.Text = firstReply.AnimName;
                        KAngelReply_TextBox.Text = firstReply.Dialogue;
                        _currentKAnimReply = KAnim_SuperReply_List.Text;
                        SetNewSpritePreview(_currentKAnimReply);
                        CreateDuplicateReplies(chat);
                        break;
                    }
                    else if (chat.PlayingType == PlayingType.ChatBad)
                    {
                        IsSuperChat_Check.Checked = false;
                        IsStressComment_Check.Checked = true;
                    }
                    else
                    {
                        IsSuperChat_Check.Checked = false;
                        IsStressComment_Check.Checked = false;
                    }
                    if (chat.PlayingType != PlayingType.ChatSuper)
                    {
                        KAnim_SuperReply_List.Text = "stream_cho_akaruku";
                        KAngelReply_TextBox.Text = "";
                        _currentSuperReplies = new() { new("stream_cho_akaruku", "") };
                    }
                    break;
                case PlayingType.PlaySE:
                    PlaySound se = obj as PlaySound;
                    PlayingType_List.SelectedIndex = 2;
                    PlaySE_List.Text = se.Audio.ToString();
                    _currentSE = PlaySE_List.Text;
                    break;
                case PlayingType.PlayBGM:
                    PlaySound bgm = obj as PlaySound;
                    PlayingType_List.SelectedIndex = 3;
                    PlayMusic_List.Text = bgm.Audio.ToString();
                    _currentMusic = PlayMusic_List.Text;
                    break;
                case PlayingType.PlayEffect:
                    PlayEffect effect = obj as PlayEffect;
                    var effectType = (int)effect.BorderEffect;
                    PlayingType_List.SelectedIndex = 4;
                    BorderEffect_List.SelectedIndex = (int)effect.BorderEffect >= 6 ? effectType - 4 : effectType;
                    switch (effect.BorderEffectType)
                    {
                        case BorderEffectType.EaseIn:
                            BorderEffect_In_Radio.Checked = true;
                            break;
                        case BorderEffectType.EaseBeforePlay:
                            BorderEffectWinStop_Radio.Checked = true;
                            break;
                        case BorderEffectType.Play:
                            BorderEffectWin_Radio.Checked = true;
                            break;
                        case BorderEffectType.EaseOut:
                            BorderEffectOut_Radio.Checked = true;
                            break;
                    }
                    break;
                case PlayingType.ChatFirst:
                    PlayingType_List.SelectedIndex = 5;
                    break;
                case PlayingType.ChatMiddle:
                    PlayingType_List.SelectedIndex = 6;
                    break;
                case PlayingType.ChatLast:
                    PlayingType_List.SelectedIndex = 7;
                    break;
                case PlayingType.ChatRainbow:
                    PlayingType_List.SelectedIndex = 8;
                    break;
                case PlayingType.ChatDelete:
                    PlayingType_List.SelectedIndex = 9;
                    break;
                case PlayingType.ChatDeleteAll:
                    PlayingType_List.SelectedIndex = 10;
                    break;
                case PlayingType.ReadSuperChats:
                    PlayingType_List.SelectedIndex = 11;
                    SetNewSpritePreview(settings.ReactionAnimation);
                    CheckIfReactionAnimExists();
                    return;
                default:
                    PlayingType_List.SelectedIndex = -1;
                    break;

            }
            AddSaveToPlayingList_Button.Enabled = true;

            void CreateDuplicateReplies(ChatSays chat)
            {
                if (chat.Replies.Count <= 1)
                    return;
                for (int i = 1; i < chat.Replies.Count; i++)
                {
                    KAngelSays kReply = new(chat.Replies[i].AnimName, chat.Replies[i].Dialogue, chat.Replies[i].customAnim);
                    _currentSuperReplies.Add(kReply);
                }
            }
        }

        private void CheckIfReactionAnimExists()
        {
            var list = settings.PlayingList;
            if (StreamPlayingList.SelectedRows.Count > 0 && list[StreamPlayingList.SelectedRows[0].Index].PlayingType == PlayingType.ReadSuperChats)
            {
                InsertAbove_Button.Enabled = false;
                InsertBelow_Button.Enabled = false;
                AddSaveToPlayingList_Button.Enabled = true;
            }
            else if (settings.PlayingList.Exists(p => p.PlayingType == PlayingType.ReadSuperChats))
            {
                InsertAbove_Button.Enabled = false;
                InsertBelow_Button.Enabled = false;
                AddSaveToPlayingList_Button.Enabled = false;
            }
            else
            {
                InsertAbove_Button.Enabled = true;
                InsertBelow_Button.Enabled = true;
                AddSaveToPlayingList_Button.Enabled = true;
            }
        }

        private void StreamPlayingList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (StreamPlayingList.Rows[e.RowIndex].Selected)
            {
                StreamPlayingList.Rows[e.RowIndex].Selected = false;
            }
        }

        private void KAnim_List_Leave(object sender, EventArgs e)
        {
            KAnim_List.Text.ToLower();
            ValidateAnimationValue(ref KAnim_List, ref _currentKAnim);
            _currentKAnim = KAnim_List.Text;
            SetNewSpritePreview(_currentKAnim);
        }

        private void KAnim_SuperReply_List_Leave(object sender, EventArgs e)
        {
            KAnim_SuperReply_List.Text.ToLower();
            ValidateAnimationValue(ref KAnim_SuperReply_List, ref _currentKAnimReply);
            _currentKAnimReply = KAnim_SuperReply_List.Text;
            SetNewSpritePreview(_currentKAnimReply);
        }

        private void StartingBackground_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.CustomBackground = null;
            settings.StartingBackground = backgroundDict[(string)StartingBackground_List.SelectedItem];
            SetNewBackgroundPreview((string)StartingBackground_List.SelectedItem);
            ChangeFileLabelIfUnsaved();
        }

        private void PlayStartingMusic_Button_Click(object sender, EventArgs e)
        {
            if (musicPreview != null && musicPreview.PlaybackState == PlaybackState.Playing && soundPlayingAt == SoundPlayingAt.PlayMusic)
                StopMusicifPlaying();
            else if (musicPreview != null && musicPreview.PlaybackState == PlaybackState.Playing)
            {
                StopMusicifPlaying();
                return;
            };
            SetMusicPreview(settings.StartingMusic.ToString());
            soundPlayingAt = SoundPlayingAt.StartingMusic;
            PlayStartingMusic_Button.Text = "◼️";
        }

        private void StopSoundEffectIfPlaying()
        {
            if (soundPreview != null)
            {
                soundPreview.Stop();
                soundPreview.Dispose();
                soundPreview = null;
            }
            PlayCurrentSE_Button.Text = "▶️";
        }
        private void StopMusicifPlaying()
        {
            if (musicPreview != null)
            {
                musicPreview.Stop();
                musicPreview.Dispose();
                musicPreview = null;
            }
            switch (soundPlayingAt)
            {
                case SoundPlayingAt.StartingMusic:
                    PlayStartingMusic_Button.Text = "▶️";
                    break;
                case SoundPlayingAt.PlayMusic:
                    PlayCurrentMusic_Button.Text = "▶️";
                    break;
                default:
                    break;
            }
            soundPlayingAt = SoundPlayingAt.None;
        }

        private void StartingMusic_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.StartingMusic = (SoundType)Enum.Parse(typeof(SoundType), (string)StartingMusic_List.Text);
            StopMusicifPlaying();
            ChangeFileLabelIfUnsaved();
        }

        private void StartingMusic_List_Click(object sender, EventArgs e)
        {
            StopMusicifPlaying();
        }

        private void PlayCurrentSE_Button_Click(object sender, EventArgs e)
        {
            if (soundPreview != null && soundPreview.PlaybackState == PlaybackState.Playing)
            {
                StopSoundEffectIfPlaying();
                return;
            }
            SetSoundEffectPreview(_currentSE);
            PlayCurrentSE_Button.Text = "◼️";
        }

        private void PlayCurrentMusic_Button_Click(object sender, EventArgs e)
        {
            if (musicPreview != null && musicPreview.PlaybackState == PlaybackState.Playing && soundPlayingAt == SoundPlayingAt.StartingMusic)
                StopMusicifPlaying();
            else if (musicPreview != null && musicPreview.PlaybackState == PlaybackState.Playing)
            {
                StopMusicifPlaying();
                return;
            }
            SetMusicPreview(_currentMusic);
            soundPlayingAt = SoundPlayingAt.PlayMusic;
            PlayCurrentMusic_Button.Text = "◼️";
        }

        private void PlayMusic_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentMusic = (string)PlayMusic_List.SelectedItem;
        }

        private void PlaySE_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentSE = (string)PlaySE_List.SelectedItem;
        }

        private void PlayMusic_List_Click(object sender, EventArgs e)
        {
            StopMusicifPlaying();
        }

        private void PlaySE_List_Click(object sender, EventArgs e)
        {
            StopSoundEffectIfPlaying();
        }

        private void KAngelReply_TextBox_TextChanged(object sender, EventArgs e)
        {
            //_currentSuperReplies[0].Dialogue = KAngelReply_TextBox.Text;
        }

        private void AddEditReplies_TextBox_Click(object sender, EventArgs e)
        {
            ChatSays chat = new ChatSays(ChatComment_TextBox.Text, _currentSuperReplies);
            var superReplyList = new SuperRepliesForm(this, chat);
            superReplyList.ShowDialog();
        }

        private void KAnim_React_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.ReactionAnimation = (string)KAnim_React_List.SelectedItem;
            SetNewSpritePreview(settings.ReactionAnimation);
        }

        private void KAnim_React_List_Leave(object sender, EventArgs e)
        {
            KAnim_React_List.Text.ToLower();
            ValidateAnimationValue(ref KAnim_React_List, ref settings.ReactionAnimation);
            settings.ReactionAnimation = KAnim_React_List.Text;
            SetNewSpritePreview(settings.ReactionAnimation);
        }

        private void MoreOptions_LInkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StreamAdvancedOptions moreOptions = new(this);
            moreOptions.ShowDialog();
            ChangeFileLabelIfUnsaved();
        }

        private string GetSavedDirectory()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.CurrentDirectory))
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Properties.Settings.Default.CurrentDirectory;
        }

        private void SaveDirectoryToSettings(string directory)
        {
            Properties.Settings.Default.CurrentDirectory = directory;
            Properties.Settings.Default.Save();
        }

        private bool SaveExistingNSOStream()
        {
            if (string.IsNullOrEmpty(_currentFile))
            {
                return SaveNewNSOStream();

            }
            return SaveNSOStreamToFile(_currentFile);
        }

        private bool SaveNewNSOStream()
        {
            SaveFileDialog saveNsoStream = new SaveFileDialog();
            saveNsoStream.InitialDirectory = GetSavedDirectory();
            saveNsoStream.Filter = "JSON File (*.json)|*.json";
            saveNsoStream.FilterIndex = 1;
            saveNsoStream.RestoreDirectory = true;
            saveNsoStream.OverwritePrompt = true;
            if (saveNsoStream.ShowDialog() == DialogResult.OK)
            {
                return SaveNSOStreamToFile(saveNsoStream.FileName);
            }
            return false;
        }
        private bool SaveNSOStreamToFile(string filePath)
        {
            Stream stream;
            var jsonSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented };
            var treeData = JsonConvert.SerializeObject(settings, jsonSettings);

            if ((stream = File.Create(filePath)) != null)
            {
                SaveDirectoryToSettings(Path.GetDirectoryName(filePath));
                try
                {
                    stream.Write(Encoding.UTF8.GetBytes(treeData), 0, Encoding.UTF8.GetByteCount(treeData));
                    stream.Close();
                    _currentFile = filePath;
                    savedSettings = JsonConvert.DeserializeObject<StreamSettings>(File.ReadAllText(filePath), jsonSettings);
                    playingListChanged = false;
                    undoCountOnSave = _undoHistory.Count;
                    if (undoCountOnSave > 0)
                    {
                        lastUndoObjOnSave = PlayingObject.DupePlayingObj(_undoHistory[_undoHistory.Count - 1].playingObject);
                    }
                    ChangeMainFormName();
                    ChangeFileLabelIfUnsaved();
                    MessageBox.Show("Successfully saved Custom Stream!", "Success", MessageBoxButtons.OK);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not save due to error: \n\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return false;

        }

        private bool LoadNSOStreamFromFile()
        {
            OpenFileDialog openNsoStream = new OpenFileDialog();
            openNsoStream.InitialDirectory = GetSavedDirectory();
            openNsoStream.Filter = "JSON File (*.json)|*.json";
            openNsoStream.FilterIndex = 1;
            openNsoStream.RestoreDirectory = true;
            if (openNsoStream.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var fileContent = openNsoStream.OpenFile();
                    SaveDirectoryToSettings(Path.GetDirectoryName(openNsoStream.FileName));
                    using (StreamReader reader = new StreamReader(fileContent))
                    {
                        var importedTreeData = reader.ReadToEnd();
                        var jsonSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented };
                        var newTreeData = JsonConvert.DeserializeObject<StreamSettings>(importedTreeData, jsonSettings);
                        savedSettings = JsonConvert.DeserializeObject<StreamSettings>(importedTreeData, jsonSettings);
                        settings = newTreeData;
                        CustomAssetExtractor.CheckIfMissingFilesInSettings(ref settings, out bool hasChangedPaths);
                        if (hasChangedPaths)
                        {
                            playingListChanged = hasChangedPaths;
                            MessageBox.Show("Your stream has slightly been modified due to changes in your custom assets.", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }

                        _currentFile = openNsoStream.FileName;
                        _undoHistory.Clear();
                        _redoHistory.Clear();
                        SyncExportedSettings();
                        ChangeMainFormName();
                        ChangeFileLabelIfUnsaved();
                        openNsoStream.Dispose();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Could not open JSON file, either the JSON file is invalid or the JSON file does not represent a Custom Stream.", "Could not read JSON file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return false;
        }

        private void ChangeMainFormName()
        {
            if (string.IsNullOrEmpty(_currentFile))
                Text = "Custom Stream Maker";
            else Text = $"Custom Stream Maker - {Path.GetFileName(_currentFile)}";
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveNewNSOStream();
        }

        private void StreamPlayingList_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Delete && (StreamPlayingList.SelectedRows.Count > 0))
            {
                int start = StreamPlayingList.SelectedRows[0].Index;
                int count = StreamPlayingList.SelectedRows.Count;
                for (int i = start; i < (start + count); i++)
                {
                    AddToUndoHistory(EditType.Delete, i, settings.PlayingList[i]);
                    DeleteFromPlayingListView(i);

                }

            }


        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MsgIfUnsaved() == DialogResult.Cancel) return;
            LoadNSOStreamFromFile();
        }

        private void InsertAbove_Button_Click(object sender, EventArgs e)
        {
            if (StreamPlayingList.SelectedRows.Count == 0)
                return;
            var index = StreamPlayingList.SelectedRows[0].Index;
            SaveToPlayingList(true, index);
        }

        private void InsertBelow_Button_Click(object sender, EventArgs e)
        {
            if (StreamPlayingList.SelectedRows.Count == 0)
                return;
            var index = StreamPlayingList.SelectedRows[0].Index;
            SaveToPlayingList(true, index + 1);
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveExistingNSOStream();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveControl != null && ActiveControl is TextBoxBase && (ActiveControl as TextBoxBase).SelectedText != null)
            {
                (ActiveControl as TextBoxBase).Cut();
            }
            else if (StreamPlayingList.SelectedRows.Count > 0)
            {
                var index = StreamPlayingList.SelectedRows[0].Index;
                CopyExistingPlayingObj();
                AddToUndoHistory(EditType.Delete, index, settings.PlayingList[index]);
                DeleteFromPlayingListView(index);
            }
        }

        private void copyTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveControl != null && ActiveControl is TextBoxBase && (ActiveControl as TextBoxBase).SelectedText != null)
            {
                (ActiveControl as TextBoxBase).Copy();
            }
            else if (StreamPlayingList.SelectedRows.Count > 0)
            {
                CopyExistingPlayingObj();
            }
        }

        private void pasteTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject() == null)
                return;
            var data = Clipboard.GetDataObject();
            var isPlayingDupeExists = CheckClipboardForPlayingObj();
            if (ActiveControl != null && ActiveControl is TextBoxBase && Clipboard.ContainsText())
            {
                (ActiveControl as TextBoxBase).Paste();
            }
            else if (isPlayingDupeExists)
            {
                PasteExistingPlayingObj();
            }
        }

        private bool CheckClipboardForPlayingObj()
        {
            var data = Clipboard.GetDataObject();
            var isKDupeExists = data.GetDataPresent(typeof(KAngelSays));
            var isKCalloutDupeExists = data.GetDataPresent(typeof(KAngelCallout));
            var isChatDupeExists = data.GetDataPresent(typeof(ChatSays));
            var isAudioDupeExists = data.GetDataPresent(typeof(PlaySound));
            var isEffectDupeExists = data.GetDataPresent(typeof(PlayEffect));
            var isGenDupeExists = data.GetDataPresent(typeof(ChatGeneral));
            return isKDupeExists || isKCalloutDupeExists || isChatDupeExists || isAudioDupeExists || isEffectDupeExists || isGenDupeExists;
        }

        private void CopyExistingPlayingObj()
        {
            if (StreamPlayingList.SelectedRows.Count <= 0)
                return;
            var index = StreamPlayingList.SelectedRows[0].Index;
            var objDupe = PlayingObject.DupePlayingObj(settings.PlayingList[index]);
            var copiedObj = new DataObject(objDupe);
            Clipboard.SetDataObject(copiedObj);
        }

        private PlayingObject ImportPlayingDupeClipboardData()
        {
            var data = Clipboard.GetDataObject();
            var isKDupeExists = data.GetDataPresent(typeof(KAngelSays));
            var isKCalloutDupeExists = data.GetDataPresent(typeof(KAngelCallout));
            var isChatDupeExists = data.GetDataPresent(typeof(ChatSays));
            var isAudioDupeExists = data.GetDataPresent(typeof(PlaySound));
            var isEffectDupeExists = data.GetDataPresent(typeof(PlayEffect));
            var isGenDupeExists = data.GetDataPresent(typeof(ChatGeneral));
            if (isKDupeExists)
                return data.GetData(typeof(KAngelSays)) as PlayingObject;
            else if (isKCalloutDupeExists)
                return data.GetData(typeof(KAngelCallout)) as PlayingObject;
            else if (isChatDupeExists)
                return data.GetData(typeof(ChatSays)) as PlayingObject;
            else if (isAudioDupeExists)
                return data.GetData(typeof(PlaySound)) as PlayingObject;
            else if (isEffectDupeExists)
                return data.GetData(typeof(PlayEffect)) as PlayingObject;
            else if (isGenDupeExists)
                return data.GetData(typeof(ChatGeneral)) as PlayingObject;
            return null;
        }

        private void PasteExistingPlayingObj()
        {
            var pastingObj = ImportPlayingDupeClipboardData();
            var objDupe = PlayingObject.DupePlayingObj(pastingObj);
            if (objDupe.PlayingType == PlayingType.ReadSuperChats && settings.PlayingList.Exists(p => p.PlayingType == PlayingType.ReadSuperChats))
                return;
            if (objDupe is KAngelSays)
            {
                var kDupe = objDupe as KAngelSays;
                if (kDupe.customAnim != null && (CustomAssetExtractor.customAssets.Count == 0 || !CustomAssetExtractor.customAssets.Exists(a => CustomAsset.IsCustomAssetTheSame(a, kDupe.customAnim))))
                {
                    kDupe.customAnim = null;
                    kDupe.AnimName = "stream_cho_akuruku";
                }
                objDupe = kDupe;
            }
            if (StreamPlayingList.SelectedRows.Count > 0)
            {
                var index = StreamPlayingList.SelectedRows[0].Index;
                AddToUndoHistory(EditType.Edit, index, settings.PlayingList[index], index, objDupe);
                InsertToPlayingListView(index, objDupe);
                StreamPlayingList.Rows[index].Selected = true;
            }
            else AddToPlayingListView(objDupe);
        }

        private void PasteAsNewBeforeSelected()
        {
            if (StreamPlayingList.SelectedRows.Count <= 0)
                return;
            var pastingObj = ImportPlayingDupeClipboardData();
            var objDupe = PlayingObject.DupePlayingObj(pastingObj);
            if (objDupe.PlayingType == PlayingType.ReadSuperChats && settings.PlayingList.Exists(p => p.PlayingType == PlayingType.ReadSuperChats))
                return;
            if (objDupe is KAngelSays)
            {
                var kDupe = objDupe as KAngelSays;
                if (kDupe.customAnim != null && (CustomAssetExtractor.customAssets.Count == 0 || !CustomAssetExtractor.customAssets.Exists(a => CustomAsset.IsCustomAssetTheSame(a, kDupe.customAnim))))
                {
                    kDupe.customAnim = null;
                    kDupe.AnimName = "stream_cho_akuruku";
                }
                objDupe = kDupe;
            }
            var index = StreamPlayingList.SelectedRows[0].Index;
            AddToUndoHistory(EditType.Add, index, objDupe);
            InsertToPlayingListView(index, objDupe, false);
        }

        private void PasteAsNewAfterSelected()
        {
            if (StreamPlayingList.SelectedRows.Count <= 0)
                return;
            var pastingObj = ImportPlayingDupeClipboardData();
            var objDupe = PlayingObject.DupePlayingObj(pastingObj);
            if (objDupe.PlayingType == PlayingType.ReadSuperChats && settings.PlayingList.Exists(p => p.PlayingType == PlayingType.ReadSuperChats))
                return;
            if (objDupe is KAngelSays)
            {
                var kDupe = objDupe as KAngelSays;
                if (kDupe.customAnim != null && (CustomAssetExtractor.customAssets.Count == 0 || !CustomAssetExtractor.customAssets.Exists(a => CustomAsset.IsCustomAssetTheSame(a, kDupe.customAnim))))
                {
                    kDupe.customAnim = null;
                    kDupe.AnimName = "stream_cho_akuruku";
                }
                objDupe = kDupe;
            }
            var index = StreamPlayingList.SelectedRows[0].Index;
            AddToUndoHistory(EditType.Add, index + 1, objDupe);
            InsertToPlayingListView(index + 1, objDupe, false);
        }

        private void ShiftObjUp()
        {
            if (StreamPlayingList.SelectedRows.Count <= 0)
                return;
            var index = StreamPlayingList.SelectedRows[0].Index;
            if (index == 0)
                return;
            var refObj = settings.PlayingList[index];
            var shiftedObj = PlayingObject.DupePlayingObj(refObj);
            AddToUndoHistory(EditType.Edit, index, refObj, index - 1, shiftedObj);
            DeleteFromPlayingListView(index);
            InsertToPlayingListView(index - 1, shiftedObj, false);
            StreamPlayingList.Rows[index - 1].Selected = true;
        }
        private void ShiftObjDown()
        {
            if (StreamPlayingList.SelectedRows.Count <= 0)
                return;
            var index = StreamPlayingList.SelectedRows[0].Index;
            if (index == settings.PlayingList.Count - 1)
                return;
            var refObj = settings.PlayingList[index];
            var shiftedObj = PlayingObject.DupePlayingObj(refObj);
            AddToUndoHistory(EditType.Edit, index, refObj, index + 1, shiftedObj);
            DeleteFromPlayingListView(index);
            InsertToPlayingListView(index + 1, shiftedObj, false);
            StreamPlayingList.Rows[index + 1].Selected = true;
        }

        private void addCopiedAboveSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject() == null || Clipboard.ContainsText())
                return;
            PasteAsNewBeforeSelected();
        }

        private void addCopiedBelowSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject() == null || Clipboard.ContainsText())
                return;
            PasteAsNewAfterSelected();
        }

        private void moveSelectionUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShiftObjUp();
        }

        private void moveSelectionDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShiftObjDown();
        }

        private void AddToUndoHistory(EditType editType, int index, PlayingObject playingObject, int editedIndex = -1, PlayingObject editedObject = null)
        {
            EditHistory edit = new(editType, index, playingObject, editedIndex, editedObject);
            _undoHistory.Add(edit);
            _redoHistory.Clear();
            playingListChanged = true;
            ChangeFileLabelIfUnsaved();
        }

        private void UndoAction()
        {
            if (_undoHistory.Count == 0) return;
            UseEditHistory(_undoHistory, _redoHistory);
            playingListChanged = true;
            ChangeFileLabelIfUnsaved();
        }

        private void RedoAction()
        {
            if (_redoHistory.Count == 0) return;
            UseEditHistory(_redoHistory, _undoHistory);
            ChangeFileLabelIfUnsaved();
        }

        private void UseEditHistory(List<EditHistory> listToUse, List<EditHistory> listToEdit)
        {
            for (int i = listToUse.Count - 1; i >= 0; i--)
            {
                EditType redoType = listToUse[i].EditType;
                switch (redoType)
                {
                    case EditType.Delete:
                        redoType = EditType.Add;
                        break;
                    case EditType.Add:
                        redoType = EditType.Delete;
                        break;
                }

                var redoAction = new EditHistory(redoType, listToUse[i].index, listToUse[i].playingObject);
                var index = 0;
                index = listToUse[i].index;
                var main = listToUse[i].ReturnMainObject();
                if (redoType == EditType.Edit)
                {
                    var (editedIndex, editedObj) = listToUse[i].ReturnEditedObject();
                    if (editedObj is KAngelSays)
                    {

                    }
                    redoAction.index = editedIndex;
                    redoAction.playingObject = editedObj;
                    redoAction.editedIndex = main.index;
                    redoAction.editedObject = main.playObj;

                }
                listToEdit.Add(redoAction);
                if (main.playObj is KAngelSays)
                {
                    var kRedo = main.playObj as KAngelSays;
                    if (kRedo.customAnim != null && (CustomAssetExtractor.customAssets.Count == 0 || !CustomAssetExtractor.customAssets.Exists(a => CustomAsset.IsCustomAssetTheSame(a, kRedo.customAnim))))
                    {
                        kRedo.customAnim = null;
                        kRedo.AnimName = "stream_cho_akuruku";
                    }
                }
                if (listToUse[i].EditType == EditType.Add)
                {
                    DeleteFromPlayingListView(index);
                    break;
                }
                if (listToUse[i].EditType == EditType.Edit)
                {
                    DeleteFromPlayingListView(listToUse[i].editedIndex);
                }
                InsertToPlayingListView(index, main.playObj, false);
                break;
            }
            listToUse.RemoveAt(listToUse.Count - 1);
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveControl != null && ActiveControl is TextBoxBase)
            {
                (ActiveControl as TextBoxBase).Undo();
                return;
            }
            UndoAction();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveControl != null && ActiveControl is TextBoxBase)
            {
                (ActiveControl as TextBoxBase).Undo();
                return;
            }
            RedoAction();
        }

        internal void ChangeFileLabelIfUnsaved()
        {
            if (!CheckIfSaved())
                fileToolStripMenuItem.Text = "File*";
            else fileToolStripMenuItem.Text = "File";
        }

        private void EditMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var data = Clipboard.GetDataObject();
            bool isSelected = (ActiveControl != null && ActiveControl is TextBoxBase && (ActiveControl as TextBoxBase).SelectedText != null) || StreamPlayingList.SelectedRows.Count > 0;
            if (!(_undoHistory.Count > 0 || (ActiveControl != null && ActiveControl is TextBoxBase)))
                undoToolStripMenuItem1.Enabled = false;
            else undoToolStripMenuItem1.Enabled = true;
            if (!(_redoHistory.Count > 0 || (ActiveControl != null && ActiveControl is TextBoxBase)))
                redoToolStripMenuItem1.Enabled = false;
            else redoToolStripMenuItem1.Enabled = true;
            if (isSelected)
            {
                cutToolStripMenuItem1.Enabled = true;
                copyToolStripMenuItem.Enabled = true;
            }
            else
            {
                cutToolStripMenuItem1.Enabled = false;
                copyToolStripMenuItem.Enabled = false;
            }
            if (ActiveControl != null && ActiveControl is TextBoxBase && Clipboard.ContainsText() || (data != null && CheckClipboardForPlayingObj()))
                pasteToolStripMenuItem.Enabled = true;
            else pasteToolStripMenuItem.Enabled = false;
            if (StreamPlayingList.SelectedRows.Count > 0)
            {
                if (StreamPlayingList.SelectedRows[0].Index > 0)
                    moveSelectionUpToolStripMenuItem1.Enabled = true;
                else moveSelectionUpToolStripMenuItem1.Enabled = false;
                if (StreamPlayingList.SelectedRows[0].Index < settings.PlayingList.Count - 1)
                    moveSelectionDownToolStripMenuItem1.Enabled = true;
                else moveSelectionDownToolStripMenuItem1.Enabled = false;
                if (data != null && CheckClipboardForPlayingObj())
                {
                    pasteNewAboveSelectionToolStripMenuItem.Enabled = true;
                    pasteNewBelowSelectionToolStripMenuItem.Enabled = true;
                }
                else
                {
                    pasteNewAboveSelectionToolStripMenuItem.Enabled = false;
                    pasteNewBelowSelectionToolStripMenuItem.Enabled = false;
                }
            }
            else
            {
                moveSelectionUpToolStripMenuItem1.Enabled = false;
                moveSelectionDownToolStripMenuItem1.Enabled = false;
                pasteNewAboveSelectionToolStripMenuItem.Enabled = false;
                pasteNewBelowSelectionToolStripMenuItem.Enabled = false;
            }
        }

        private void EditMenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            undoToolStripMenuItem1.Enabled = true;
            redoToolStripMenuItem1.Enabled = true;
            cutToolStripMenuItem1.Enabled = true;
            copyToolStripMenuItem.Enabled = true;
            pasteToolStripMenuItem.Enabled = true;
            pasteNewAboveSelectionToolStripMenuItem.Enabled = true;
            pasteNewBelowSelectionToolStripMenuItem.Enabled = true;
            moveSelectionUpToolStripMenuItem1.Enabled = true;
            moveSelectionDownToolStripMenuItem1.Enabled = true;
        }

        private void ResetCustomStream()
        {
            StreamPlayingList.Rows.Clear();
            settings = new();
            _currentFile = null;
            _undoHistory.Clear();
            _redoHistory.Clear();
            InitializeDefaultFields();
            InitializeNewStreamSettings();
            ChangeMainFormName();
            ChangeFileLabelIfUnsaved();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MsgIfUnsaved() == DialogResult.Cancel) return;
            ResetCustomStream();
        }

        private DialogResult MsgIfUnsaved()
        {
            var isSaved = CheckIfSaved();
            if (isSaved) return DialogResult.Yes;
            var confirm = MessageBox.Show("You currently have unsaved changes! Save custom stream?", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                return SaveExistingNSOStream() ? DialogResult.Yes : DialogResult.Cancel;
            }
            return confirm;
        }
        private bool MustSaveIfUnsaved()
        {
            var isSaved = CheckIfSaved();
            if (isSaved) return true;
            var confirm = MessageBox.Show("You currently have unsaved changes! Save custom stream?", "Save?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                return SaveExistingNSOStream();
            }
            return false;
        }
        private bool CheckIfSaved()
        {
            if (_currentFile == null || savedSettings == null)
                return false;
            if (playingListChanged)
                return false;
            if (!playingListChanged && _redoHistory.Count > 0)
                return false;
            if (savedSettings.StringTitle != settings.StringTitle)
                return false;
            if (savedSettings.ChatSettings != settings.ChatSettings)
                return false;
            if (savedSettings.StartingAnimation != settings.StartingAnimation)
                return false;
            if (savedSettings.StartingBackground != settings.StartingBackground)
                return false;
            if (savedSettings.StartingMusic != settings.StartingMusic)
                return false;
            if (savedSettings.StartingEffect != settings.StartingEffect)
                return false;
            if (savedSettings.EffectIntensity != settings.EffectIntensity)
                return false;
            if (savedSettings.ReactionAnimation != settings.ReactionAnimation)
                return false;
            if (savedSettings.IsIntroPlaying != settings.IsIntroPlaying)
                return false;
            if (savedSettings.IsDarkAngelPlaying != settings.IsDarkAngelPlaying)
                return false;
            if (savedSettings.HasCustomFollowerCount != settings.HasCustomFollowerCount)
                return false;
            if (savedSettings.CustomFollowerCount != settings.CustomFollowerCount)
                return false;
            if (savedSettings.HasCustomDay != settings.HasCustomDay)
                return false;
            if (savedSettings.HasCustomDay && savedSettings.CustomDay != settings.CustomDay)
                return false;
            if (savedSettings.IsInvertedColors != settings.IsInvertedColors)
                return false;
            if (savedSettings.isBordersOff != settings.isBordersOff)
                return false;
            if (savedSettings.hasEndScreen != settings.hasEndScreen)
                return false;
            if (savedSettings.CustomEndScreenPath != settings.CustomEndScreenPath)
                return false;
            if (savedSettings.hasDarkInterface != settings.hasDarkInterface)
                return false;
            return true;
        }

        private void loadCurrentStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MustSaveIfUnsaved()) return;
            SetStreamInStreamLoader(_currentFile);
        }

        private void SetStreamInStreamLoader(string filePathOfStream)
        {
            var fileName = Path.GetFileName(filePathOfStream);
            if (!GameLocation.IsGameModded(out string modPath))
                return;
            modPath += @"\CustomStreamLoader";
            if (!Directory.Exists(modPath))
                return;
            string[] existingJSONs = Directory.GetFiles(modPath);
            foreach (string json in existingJSONs)
            {
                if (json.EndsWith(".json")) File.Delete(json);
            }
            var pathData = File.ReadAllBytes(filePathOfStream);
            using (var stream = File.Create(Path.Combine(modPath, fileName)))
            {
                stream.Write(pathData, 0, pathData.Length);
                stream.Dispose();
            }
            MessageBox.Show("Successfully set Custom Stream!", "Success", MessageBoxButtons.OK);
        }

        private void loadSavedStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openNsoStream = new OpenFileDialog();
            openNsoStream.InitialDirectory = GetSavedDirectory();
            openNsoStream.Filter = "JSON File (*.json)|*.json";
            openNsoStream.FilterIndex = 1;
            openNsoStream.RestoreDirectory = true;
            if (openNsoStream.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SaveDirectoryToSettings(Path.GetDirectoryName(openNsoStream.FileName));
                    SetStreamInStreamLoader(openNsoStream.FileName);
                }
                catch { MessageBox.Show("Could not load JSON file, either the JSON file is invalid or the JSON file does not represent a Custom Stream.", "Could not read JSON file", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void viewCustomAssetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.customAssetPreview != null)
            {
                this.customAssetPreview.Focus();
                return;
            }
            CustomAssetPreview customAssetPreview = new CustomAssetPreview(this);
            this.customAssetPreview = customAssetPreview;
            customAssetPreview.Show();
        }

        private void createBackgroundFromImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomAssetExtractor.ImportImage();
            if (this.customAssetPreview != null)
            {
                customAssetPreview.ReloadAssetListView();
            }
        }

        private void createAnimationFromAssetBundlelz4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomAssetExtractor.ImportSpriteFromAssetBundle(true);
            if (this.customAssetPreview != null)
            {
                customAssetPreview.ReloadAssetListView();
            }
        }

        private void createAnimationFromAssetBundlelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomAssetExtractor.ImportSpriteFromAssetBundle(false);
            if (this.customAssetPreview != null)
            {
                customAssetPreview.ReloadAssetListView();
            }
        }

        private void createAnimationFromAddressableBundlelz4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomAssetExtractor.ImportSpriteFromAddressable(true);
            if (this.customAssetPreview != null)
            {
                customAssetPreview.ReloadAssetListView();
            }
        }

        private void createAnimationFromAddressableBundlelzmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomAssetExtractor.ImportSpriteFromAddressable(false);
            if (this.customAssetPreview != null)
            {
                customAssetPreview.ReloadAssetListView();
            }
        }

        private void initializeAddressableCatalogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomAssetExtractor.InitializeCatalogPath();
            CheckUnvalidCustomAssets();
            if (this.customAssetPreview != null)
            {
                customAssetPreview.ReloadAssetListView();
            }
        }

        internal void CheckUnvalidCustomAssets()
        {
            bool isCustomBackNull = (settings.CustomBackground == null && settings.StartingBackground == StreamBackground.None);
            if (settings.StartingBackground == StreamBackground.None && (isCustomBackNull || CustomAssetExtractor.customAssets.Count == 0 || !CustomAssetExtractor.customAssets.Exists(a => CustomAsset.IsCustomAssetTheSame(a, settings.CustomBackground))))
            {
                settings.CustomBackground = null;
                settings.StartingBackground = StreamBackground.Default;
                StartingBackground_List.Text = "bg_stream";
                SetNewBackgroundPreview("bg_stream");
                playingListChanged = true;
            }
            if (settings.CustomStartingAnimation != null && (CustomAssetExtractor.customAssets.Count == 0 || !CustomAssetExtractor.customAssets.Exists(a => CustomAsset.IsCustomAssetTheSame(a, settings.CustomStartingAnimation))))
            {
                settings.CustomStartingAnimation = null;
                settings.StartingAnimation = "stream_cho_akaruku";
                StartingAnimation_List.Text = "stream_cho_akaruku";
                SetNewSpritePreview("stream_cho_akaruku");
                playingListChanged = true;

            }
            for (int i = 0; i < settings.PlayingList.Count; i++)
            {
                var playObj = settings.PlayingList[i];
                if (playObj.PlayingType == PlayingType.KAngelSays || playObj.PlayingType == PlayingType.KAngelCallout)
                {
                    var kObj = playObj as KAngelSays;
                    if (animList.Contains(kObj.AnimName))
                    {
                        continue;
                    }
                    if (kObj.customAnim != null && (CustomAssetExtractor.customAssets.Count == 0 || !CustomAssetExtractor.customAssets.Exists(a => CustomAsset.IsCustomAssetTheSame(a, kObj.customAnim))))
                    {
                        kObj.AnimName = "stream_cho_akaruku";
                        kObj.customAnim = null;
                        playingListChanged = true;
                    }
                }
                if (playObj is ChatSays)
                {
                    var chatObj = playObj as ChatSays;
                    if (chatObj.Replies != null && chatObj.Replies.Count > 0)
                    {
                        foreach (var kObj in chatObj.Replies)
                        {
                            if (animList.Contains(kObj.AnimName))
                            {
                                continue;
                            }
                            if (kObj.customAnim != null && (CustomAssetExtractor.customAssets.Count == 0 || !CustomAssetExtractor.customAssets.Exists(a => CustomAsset.IsCustomAssetTheSame(a, kObj.customAnim))))
                            {
                                kObj.AnimName = "stream_cho_akaruku";
                                kObj.customAnim = null;
                                if (i == 0)
                                {
                                    KAngelReply_TextBox.Text = "stream_cho_akaruku";
                                }
                                playingListChanged = true;
                            }
                        }
                    }
                }
            }
            SetNewSpritePreview(settings.StartingAnimation);
            ResetPlayingListView();
        }

        private void ResetPlayingListView()
        {
            StreamPlayingList.Rows.Clear();
            for (int i = 0; i < settings.PlayingList.Count; i++)
            {
                var playingObject = settings.PlayingList[i];
                var row = StreamPlayingList.Rows.Add(SetPlayingType(playingObject));
                var desc = SetPlayingDesc(playingObject);
                StreamPlayingList.Rows[row].Cells[1].Value = desc;
            }
        }

        private void importCachedAnimationClipsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CachedAnimationClips cachedAnimationClips = new CachedAnimationClips();
            cachedAnimationClips.ShowDialog();
        }

        private void assetsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CustomAssetExtractor.catalogPath))
            {
                initializeAddressableCatalogToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
            else initializeAddressableCatalogToolStripMenuItem.CheckState = CheckState.Checked;
            if (!CustomAssetExtractor.customAssets.Exists(a => a.filePath == "" || a.filePath.Contains("?")))
                fixAnyMissingToolStripMenuItem.Enabled = false;
            else fixAnyMissingToolStripMenuItem.Enabled = true;
        }

        private void ExitControl(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ActiveControl = null;
            }
        }

        private void StreamEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MsgIfUnsaved() == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            Properties.Settings.Default.Save();
        }

        private void fixAnyMissingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CustomAssetExtractor.customAssets.Exists(a => a.filePath == "" || a.filePath.Contains("?")))
                return;
            CustomAssetExtractor.CheckForMissingFilesAtStart();
            CustomAssetExtractor.CheckIfMissingFilesInSettings(ref settings, out var check);
        }
    }
}
