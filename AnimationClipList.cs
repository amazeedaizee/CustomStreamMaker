using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CustomStreamMaker
{
    public partial class AnimationClipList : Form
    {
        public List<string> animList;

        public delegate void ClosingEvent();
        public ClosingEvent OnClosingConfirm;
        public AnimationClipList(List<string> list)
        {
            animList = list;
            InitializeComponent();
        }

        private void AnimationClipListBox_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            if (e.KeyCode == Keys.Delete)
            {
                for (int i = 0; i <= AnimationClipListBox.SelectedIndices.Count; i++)
                {
                    if (AnimationClipListBox.SelectedIndices.Count == 0) break;
                    i = 0;
                    var index = AnimationClipListBox.SelectedIndices[0];
                    AnimationClipListBox.Items.RemoveAt(index);
                    animList.RemoveAt(index);
                }
            }
            */
        }

        private void AnimationClipList_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < animList.Count; i++)
            {
                AnimationClipListBox.Items.Add(animList[i]);
            }
        }

        private void AnimationClipList_FormClosing(object sender, FormClosingEventArgs e)
        {
            var newAnimList = new List<string>();
            for (int i = 0; i < AnimationClipListBox.SelectedIndices.Count; i++)
            {
                newAnimList.Add(animList[AnimationClipListBox.SelectedIndices[i]]);
            }
            animList = newAnimList;
            OnClosingConfirm?.Invoke();
            Dispose();
        }

        private void AnimationClipConfirm_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
