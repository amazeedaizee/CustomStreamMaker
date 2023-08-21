namespace CustomStreamMaker
{
    partial class AssetLoadWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssetLoadWindow));
            this.Wait_Label = new System.Windows.Forms.Label();
            this.AssetToMem_Progress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // Wait_Label
            // 
            this.Wait_Label.AutoSize = true;
            this.Wait_Label.Location = new System.Drawing.Point(21, 19);
            this.Wait_Label.Name = "Wait_Label";
            this.Wait_Label.Size = new System.Drawing.Size(171, 13);
            this.Wait_Label.TabIndex = 0;
            this.Wait_Label.Text = "Loading in assets from the game....";
            this.Wait_Label.UseWaitCursor = true;
            // 
            // AssetToMem_Progress
            // 
            this.AssetToMem_Progress.Location = new System.Drawing.Point(24, 53);
            this.AssetToMem_Progress.Name = "AssetToMem_Progress";
            this.AssetToMem_Progress.Size = new System.Drawing.Size(350, 23);
            this.AssetToMem_Progress.TabIndex = 1;
            this.AssetToMem_Progress.UseWaitCursor = true;
            // 
            // AssetLoadWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 107);
            this.ControlBox = false;
            this.Controls.Add(this.AssetToMem_Progress);
            this.Controls.Add(this.Wait_Label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AssetLoadWindow";
            this.Text = "Please wait...";
            this.UseWaitCursor = true;
            this.Activated += new System.EventHandler(this.AssetLoadWindow_Activated);
            this.Shown += new System.EventHandler(this.AssetLoadWindow_Shown);
            this.Enter += new System.EventHandler(this.AssetLoadWindow_Enter);
            this.Validated += new System.EventHandler(this.AssetLoadWindow_Validated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Wait_Label;
        private System.Windows.Forms.ProgressBar AssetToMem_Progress;
    }
}