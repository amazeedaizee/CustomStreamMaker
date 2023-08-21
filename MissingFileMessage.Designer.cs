namespace CustomStreamMaker
{
    partial class MissingFileMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MissingFileMessage));
            this.label1 = new System.Windows.Forms.Label();
            this.AddAddressable = new System.Windows.Forms.Button();
            this.AddAsAssetBundle = new System.Windows.Forms.Button();
            this.IgnoreAsset = new System.Windows.Forms.Button();
            this.CustomAsset_Group = new System.Windows.Forms.GroupBox();
            this.PastFilePath_Text = new System.Windows.Forms.TextBox();
            this.Asset_Name_Label = new System.Windows.Forms.Label();
            this.FilePath_Label = new System.Windows.Forms.Label();
            this.FileType_Label = new System.Windows.Forms.Label();
            this.AssetType_Label = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.NewFilePath_Text = new System.Windows.Forms.TextBox();
            this.IgnoreAllAsset = new System.Windows.Forms.Button();
            this.CustomAsset_Group.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.Location = new System.Drawing.Point(12, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(415, 59);
            this.label1.TabIndex = 0;
            this.label1.Text = "This custom asset\'s file cannot be found. \r\n\r\nYou can connect this asset to a new" +
    " file below, or you can ignore this error.";
            // 
            // AddAddressable
            // 
            this.AddAddressable.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.AddAddressable.Location = new System.Drawing.Point(10, 278);
            this.AddAddressable.Name = "AddAddressable";
            this.AddAddressable.Size = new System.Drawing.Size(127, 31);
            this.AddAddressable.TabIndex = 2;
            this.AddAddressable.Text = "Add As Addressable";
            this.AddAddressable.UseVisualStyleBackColor = true;
            this.AddAddressable.Click += new System.EventHandler(this.AddAddressable_Click);
            // 
            // AddAsAssetBundle
            // 
            this.AddAsAssetBundle.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.AddAsAssetBundle.Location = new System.Drawing.Point(143, 278);
            this.AddAsAssetBundle.Name = "AddAsAssetBundle";
            this.AddAsAssetBundle.Size = new System.Drawing.Size(127, 31);
            this.AddAsAssetBundle.TabIndex = 3;
            this.AddAsAssetBundle.Text = "Add As Asset Bundle";
            this.AddAsAssetBundle.UseVisualStyleBackColor = true;
            this.AddAsAssetBundle.Click += new System.EventHandler(this.AddAsAssetBundle_Click);
            // 
            // IgnoreAsset
            // 
            this.IgnoreAsset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.IgnoreAsset.Location = new System.Drawing.Point(346, 278);
            this.IgnoreAsset.Name = "IgnoreAsset";
            this.IgnoreAsset.Size = new System.Drawing.Size(77, 31);
            this.IgnoreAsset.TabIndex = 4;
            this.IgnoreAsset.Text = "Ignore";
            this.IgnoreAsset.UseVisualStyleBackColor = true;
            this.IgnoreAsset.Click += new System.EventHandler(this.DeleteAsset_Click);
            // 
            // CustomAsset_Group
            // 
            this.CustomAsset_Group.Controls.Add(this.PastFilePath_Text);
            this.CustomAsset_Group.Controls.Add(this.Asset_Name_Label);
            this.CustomAsset_Group.Controls.Add(this.FilePath_Label);
            this.CustomAsset_Group.Controls.Add(this.FileType_Label);
            this.CustomAsset_Group.Controls.Add(this.AssetType_Label);
            this.CustomAsset_Group.Location = new System.Drawing.Point(15, 13);
            this.CustomAsset_Group.Name = "CustomAsset_Group";
            this.CustomAsset_Group.Size = new System.Drawing.Size(491, 119);
            this.CustomAsset_Group.TabIndex = 2;
            this.CustomAsset_Group.TabStop = false;
            this.CustomAsset_Group.Text = "Custom Asset";
            // 
            // PastFilePath_Text
            // 
            this.PastFilePath_Text.BackColor = System.Drawing.SystemColors.Control;
            this.PastFilePath_Text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PastFilePath_Text.Location = new System.Drawing.Point(63, 82);
            this.PastFilePath_Text.Multiline = true;
            this.PastFilePath_Text.Name = "PastFilePath_Text";
            this.PastFilePath_Text.Size = new System.Drawing.Size(413, 23);
            this.PastFilePath_Text.TabIndex = 2;
            this.PastFilePath_Text.TabStop = false;
            this.PastFilePath_Text.Text = "OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO\r\n";
            // 
            // Asset_Name_Label
            // 
            this.Asset_Name_Label.AutoSize = true;
            this.Asset_Name_Label.Location = new System.Drawing.Point(7, 40);
            this.Asset_Name_Label.Name = "Asset_Name_Label";
            this.Asset_Name_Label.Size = new System.Drawing.Size(67, 13);
            this.Asset_Name_Label.TabIndex = 1;
            this.Asset_Name_Label.Text = "Asset Name:";
            // 
            // FilePath_Label
            // 
            this.FilePath_Label.AutoSize = true;
            this.FilePath_Label.Location = new System.Drawing.Point(6, 82);
            this.FilePath_Label.Name = "FilePath_Label";
            this.FilePath_Label.Size = new System.Drawing.Size(51, 13);
            this.FilePath_Label.TabIndex = 1;
            this.FilePath_Label.Text = "File Path:";
            // 
            // FileType_Label
            // 
            this.FileType_Label.AutoSize = true;
            this.FileType_Label.Location = new System.Drawing.Point(7, 60);
            this.FileType_Label.Name = "FileType_Label";
            this.FileType_Label.Size = new System.Drawing.Size(53, 13);
            this.FileType_Label.TabIndex = 1;
            this.FileType_Label.Text = "File Type:";
            // 
            // AssetType_Label
            // 
            this.AssetType_Label.AutoSize = true;
            this.AssetType_Label.Location = new System.Drawing.Point(7, 20);
            this.AssetType_Label.Name = "AssetType_Label";
            this.AssetType_Label.Size = new System.Drawing.Size(63, 13);
            this.AssetType_Label.TabIndex = 0;
            this.AssetType_Label.Text = "Asset Type:";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.BrowseButton.Location = new System.Drawing.Point(15, 209);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(68, 24);
            this.BrowseButton.TabIndex = 0;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // NewFilePath_Text
            // 
            this.NewFilePath_Text.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NewFilePath_Text.Location = new System.Drawing.Point(90, 211);
            this.NewFilePath_Text.Name = "NewFilePath_Text";
            this.NewFilePath_Text.Size = new System.Drawing.Size(416, 20);
            this.NewFilePath_Text.TabIndex = 1;
            this.NewFilePath_Text.DoubleClick += new System.EventHandler(this.NewFilePath_Text_DoubleClick);
            // 
            // IgnoreAllAsset
            // 
            this.IgnoreAllAsset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.IgnoreAllAsset.Location = new System.Drawing.Point(432, 278);
            this.IgnoreAllAsset.Name = "IgnoreAllAsset";
            this.IgnoreAllAsset.Size = new System.Drawing.Size(77, 31);
            this.IgnoreAllAsset.TabIndex = 5;
            this.IgnoreAllAsset.Text = "Ignore All";
            this.IgnoreAllAsset.UseVisualStyleBackColor = true;
            this.IgnoreAllAsset.Click += new System.EventHandler(this.IgnoreAllAsset_Click);
            // 
            // MissingFileMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 323);
            this.ControlBox = false;
            this.Controls.Add(this.NewFilePath_Text);
            this.Controls.Add(this.CustomAsset_Group);
            this.Controls.Add(this.IgnoreAllAsset);
            this.Controls.Add(this.IgnoreAsset);
            this.Controls.Add(this.AddAsAssetBundle);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.AddAddressable);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MissingFileMessage";
            this.Text = "Missing file detected";
            this.Load += new System.EventHandler(this.MissingFileMessage_Load);
            this.CustomAsset_Group.ResumeLayout(false);
            this.CustomAsset_Group.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AddAddressable;
        private System.Windows.Forms.Button AddAsAssetBundle;
        private System.Windows.Forms.Button IgnoreAsset;
        private System.Windows.Forms.GroupBox CustomAsset_Group;
        private System.Windows.Forms.Label Asset_Name_Label;
        private System.Windows.Forms.Label FileType_Label;
        private System.Windows.Forms.Label AssetType_Label;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.TextBox PastFilePath_Text;
        private System.Windows.Forms.Label FilePath_Label;
        private System.Windows.Forms.TextBox NewFilePath_Text;
        private System.Windows.Forms.Button IgnoreAllAsset;
    }
}