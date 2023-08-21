namespace CustomStreamMaker
{
    partial class CachedAnimationClips
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CachedAnimationClips));
            this.CachedAnimationsTreeView = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CachedAnimationsTreeView
            // 
            this.CachedAnimationsTreeView.Location = new System.Drawing.Point(12, 55);
            this.CachedAnimationsTreeView.Name = "CachedAnimationsTreeView";
            this.CachedAnimationsTreeView.Size = new System.Drawing.Size(349, 352);
            this.CachedAnimationsTreeView.TabIndex = 0;
            this.CachedAnimationsTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CachedAnimationsTreeView_KeyDown);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(327, 43);
            this.label1.TabIndex = 2;
            this.label1.Text = "\nSelect and delete which ones you want to exclude from importing.\r\n\r\n";
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(12, 415);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(171, 34);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(190, 415);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(171, 34);
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // CachedAnimationClips
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 461);
            this.ControlBox = false;
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CachedAnimationsTreeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CachedAnimationClips";
            this.Text = "Cached Animation Clips";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CachedAnimationClips_FormClosing);
            this.Load += new System.EventHandler(this.CachedAnimationClips_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView CachedAnimationsTreeView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
    }
}