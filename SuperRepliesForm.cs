using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CustomStreamMaker
{
    public partial class SuperRepliesForm : Form
    {
        StreamEditor streamEditor;
        ChatSays chatComment;
        internal List<KAngelSays> kAngelSaysDupe = new();
        string _currentAnim = "";
        public SuperRepliesForm(StreamEditor streamEditor, ChatSays chat)
        {
            chatComment = chat;
            this.streamEditor = streamEditor;
            InitializeComponent();
        }

        private void SuperRepliesForm_Load(object sender, EventArgs e)
        {
            KAnimReply_List.Items.AddRange(ThatOneLongListOfAnimationsOriginallyInTheGame.list);
            SuperChatComment.Text = chatComment.Comment;
            SetBackgroundPreview(streamEditor.settings.StartingBackground);
            LoadInReplies();
            SetNewSpritePreview(_currentAnim);
            SuperRepliesListView.ClearSelection();
            SuperRepliesListView.CurrentCell = null;
            if (SuperRepliesListView.SelectedRows.Count > 0)
            {
                AddEditSuperReply_Button.Text = "Save Reply";
            }
            CheckIfAtReplyLimit();

        }
        private void LoadInReplies()
        {
            if (chatComment.Replies.Count <= 1)
            {
                _currentAnim = chatComment.Replies[0].AnimName;
                return;
            }
            for (int i = 1; i < chatComment.Replies.Count; i++)
            {
                AddToReplyListView(chatComment.Replies[i]);
            }
            _currentAnim = chatComment.Replies[1].AnimName;
            KAnimReply_List.Text = _currentAnim;
            KAngelReply_Text.Text = chatComment.Replies[1].Dialogue;
        }

        private void AddToReplyListView(KAngelSays kChat)
        {
            CustomAsset customAsset = null;
            if (CustomAssetExtractor.customAssets.Count > 0 && CustomAssetExtractor.customAssets.Exists(a => a.fileName == kChat.AnimName && a.customAssetType == CustomAssetType.Sprite))
            {
                customAsset = CustomAssetExtractor.customAssets.Find(a => a.fileName == kChat.AnimName && a.customAssetType == CustomAssetType.Sprite);
            }
            SuperRepliesListView.Rows.Add($"{kChat.Dialogue}\n({kChat.AnimName})");
            kAngelSaysDupe.Add(new(kChat.AnimName, kChat.Dialogue, customAsset));
        }

        private void EditToReplyListView(int index)
        {

            var newReply = new KAngelSays(_currentAnim, KAngelReply_Text.Text);
            if (CustomAssetExtractor.customAssets.Count > 0 && CustomAssetExtractor.customAssets.Exists(a => a.fileName == newReply.AnimName && a.customAssetType == CustomAssetType.Sprite))
            {
                var customAsset = CustomAssetExtractor.customAssets.Find(a => a.fileName == newReply.AnimName && a.customAssetType == CustomAssetType.Sprite);
                newReply.SetCustomAnim(customAsset);
            }
            DeleteFromReplyView(index);
            kAngelSaysDupe.Insert(index, newReply);
            SuperRepliesListView.Rows.Insert(index, $"{newReply.Dialogue}\n({newReply.AnimName})");
        }

        private void DeleteFromReplyView(int index)
        {
            kAngelSaysDupe.RemoveAt(index);
            SuperRepliesListView.Rows.RemoveAt(index);
        }

        private void SetNewSpritePreview(string imgName)
        {
            SpritePreview.Image?.Dispose();
            SpritePreview.Image = AssetExtractor.GetCachedSprite(imgName);
        }

        private void SetBackgroundPreview(StreamBackground streamBg)
        {
            SpritePreview.BackgroundImage?.Dispose();
            if (streamBg == StreamBackground.Black)
            {
                SpritePreview.BackgroundImage = null;
                SpritePreview.BackColor = Color.Black;
            }
            else if (streamBg == StreamBackground.Void)
            {
                SpritePreview.BackgroundImage = null;
                SpritePreview.BackColor = Color.White;
            }
            else
            {
                SpritePreview.BackColor = Color.FromKnownColor(KnownColor.Control);
                SpritePreview.BackgroundImage = streamEditor.currentBackground;
            }
        }

        private void SuperRepliesForm_Click(object sender, EventArgs e)
        {
            ActiveControl = null;
            SuperRepliesListView.ClearSelection();
        }

        private void KAnimReply_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentAnim = (string)KAnimReply_List.SelectedItem;
            SetNewSpritePreview(_currentAnim);
        }

        private void KAnimReply_List_Leave(object sender, EventArgs e)
        {
            KAnimReply_List.Text.ToLower();
            streamEditor.ValidateAnimationValue(ref KAnimReply_List, ref _currentAnim);
            _currentAnim = KAnimReply_List.Text;
            SetNewSpritePreview(_currentAnim);
        }

        private void SuperChatComment_Click(object sender, EventArgs e)
        {
            ActiveControl = null;
            SuperRepliesListView.ClearSelection();
        }

        private void AddEditSuperReply_Button_Click(object sender, EventArgs e)
        {
            streamEditor.ValidateAnimationValue(ref KAnimReply_List, ref _currentAnim);
            if (SuperRepliesListView.SelectedRows.Count > 0)
            {
                EditToReplyListView(SuperRepliesListView.SelectedRows[0].Index);
                CheckIfAtReplyLimit();
                return;
            }
            AddToReplyListView(new KAngelSays(_currentAnim, KAngelReply_Text.Text));
            SuperRepliesListView.ClearSelection();
            CheckIfAtReplyLimit();
        }

        private void SuperChatComment_MouseClick(object sender, MouseEventArgs e)
        {
            ActiveControl = null;
            SuperRepliesListView.ClearSelection();
        }

        private void SuperRepliesListView_SelectionChanged(object sender, EventArgs e)
        {
            if (SuperRepliesListView.SelectedRows.Count > 0 && kAngelSaysDupe.Count > 0)
            {
                AddEditSuperReply_Button.Enabled = true;
                var index = SuperRepliesListView.SelectedRows[0].Index;
                KAnimReply_List.SelectedItem = kAngelSaysDupe[index].AnimName;
                KAngelReply_Text.Text = kAngelSaysDupe[index].Dialogue;
                AddEditSuperReply_Button.Text = "Save Reply";
                return;
            }
            AddEditSuperReply_Button.Text = "Add Reply";
            CheckIfAtReplyLimit();
        }

        private void SuperRepliesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var firstReply = new KAngelSays(streamEditor._currentSuperReplies[0].AnimName, streamEditor._currentSuperReplies[0].Dialogue, streamEditor._currentSuperReplies[0].customAnim);
            streamEditor._currentSuperReplies = new List<KAngelSays>() { firstReply };
            streamEditor._currentSuperReplies.AddRange(kAngelSaysDupe);
            Dispose();
        }

        private void SuperRepliesListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && SuperRepliesListView.SelectedRows.Count > 0)
            {
                DeleteFromReplyView(SuperRepliesListView.SelectedRows[0].Index);
                SuperRepliesListView.ClearSelection();
                CheckIfAtReplyLimit();
            }
        }

        private void CheckIfAtReplyLimit()
        {
            if (kAngelSaysDupe.Count >= 3 && SuperRepliesListView.SelectedRows.Count == 0)
                AddEditSuperReply_Button.Enabled = false;
            else AddEditSuperReply_Button.Enabled = true;
        }

        private void KAnimReply_List_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ActiveControl = null;
            }
        }

        private void KAngelReply_Text_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ActiveControl = null;
            }
        }
    }
}
