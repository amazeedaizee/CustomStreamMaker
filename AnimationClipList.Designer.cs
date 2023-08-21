namespace CustomStreamMaker
{
    partial class AnimationClipList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationClipList));
            this.AnimationClipListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AnimationClipConfirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AnimationClipListBox
            // 
            this.AnimationClipListBox.FormattingEnabled = true;
            this.AnimationClipListBox.Location = new System.Drawing.Point(12, 75);
            this.AnimationClipListBox.Name = "AnimationClipListBox";
            this.AnimationClipListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.AnimationClipListBox.Size = new System.Drawing.Size(311, 303);
            this.AnimationClipListBox.TabIndex = 0;
            this.AnimationClipListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AnimationClipListBox_KeyDown);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(311, 59);
            this.label1.TabIndex = 1;
            this.label1.Text = "These Animation Clips have been found in the asset bundle.\r\n\r\nSelect which ones y" +
    "ou want to include from the list below\r\n.\r\n\r\n";
            // 
            // AnimationClipConfirm
            // 
            this.AnimationClipConfirm.Location = new System.Drawing.Point(105, 389);
            this.AnimationClipConfirm.Name = "AnimationClipConfirm";
            this.AnimationClipConfirm.Size = new System.Drawing.Size(120, 26);
            this.AnimationClipConfirm.TabIndex = 2;
            this.AnimationClipConfirm.Text = "OK";
            this.AnimationClipConfirm.UseVisualStyleBackColor = true;
            this.AnimationClipConfirm.Click += new System.EventHandler(this.AnimationClipConfirm_Click);
            // 
            // AnimationClipList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 427);
            this.ControlBox = false;
            this.Controls.Add(this.AnimationClipConfirm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AnimationClipListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnimationClipList";
            this.Text = "Animation Clips";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AnimationClipList_FormClosing);
            this.Load += new System.EventHandler(this.AnimationClipList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox AnimationClipListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AnimationClipConfirm;
    }
}