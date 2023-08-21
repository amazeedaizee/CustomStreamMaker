namespace CustomStreamMaker
{
    partial class CustomAssetPreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomAssetPreview));
            this.CustomAssetsPreviewPic = new System.Windows.Forms.PictureBox();
            this.CustomAssetsListView = new System.Windows.Forms.ListView();
            this.Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FilePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AddBackground_Button = new System.Windows.Forms.Button();
            this.AddAnimation_AssetLz4 = new System.Windows.Forms.Button();
            this.AddAnimation_AddressableLz4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CustomAssetsPreviewPic)).BeginInit();
            this.SuspendLayout();
            // 
            // CustomAssetsPreviewPic
            // 
            this.CustomAssetsPreviewPic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CustomAssetsPreviewPic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CustomAssetsPreviewPic.Location = new System.Drawing.Point(13, 13);
            this.CustomAssetsPreviewPic.Name = "CustomAssetsPreviewPic";
            this.CustomAssetsPreviewPic.Size = new System.Drawing.Size(348, 227);
            this.CustomAssetsPreviewPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CustomAssetsPreviewPic.TabIndex = 0;
            this.CustomAssetsPreviewPic.TabStop = false;
            // 
            // CustomAssetsListView
            // 
            this.CustomAssetsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Type,
            this.Name,
            this.FilePath});
            this.CustomAssetsListView.FullRowSelect = true;
            this.CustomAssetsListView.HideSelection = false;
            this.CustomAssetsListView.Location = new System.Drawing.Point(377, 12);
            this.CustomAssetsListView.Name = "CustomAssetsListView";
            this.CustomAssetsListView.Size = new System.Drawing.Size(541, 391);
            this.CustomAssetsListView.TabIndex = 3;
            this.CustomAssetsListView.UseCompatibleStateImageBehavior = false;
            this.CustomAssetsListView.View = System.Windows.Forms.View.Details;
            this.CustomAssetsListView.SelectedIndexChanged += new System.EventHandler(this.CustomAssetsListView_SelectedIndexChanged);
            this.CustomAssetsListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CustomAssetsListView_KeyDown);
            // 
            // Type
            // 
            this.Type.Text = "Type";
            // 
            // Name
            // 
            this.Name.Text = "Name";
            this.Name.Width = 173;
            // 
            // FilePath
            // 
            this.FilePath.Text = "Path";
            this.FilePath.Width = 299;
            // 
            // AddBackground_Button
            // 
            this.AddBackground_Button.Location = new System.Drawing.Point(13, 258);
            this.AddBackground_Button.Name = "AddBackground_Button";
            this.AddBackground_Button.Size = new System.Drawing.Size(348, 39);
            this.AddBackground_Button.TabIndex = 0;
            this.AddBackground_Button.Text = "Import Image For Background (jpg, png)";
            this.AddBackground_Button.UseVisualStyleBackColor = true;
            this.AddBackground_Button.Click += new System.EventHandler(this.AddBackground_Button_Click);
            // 
            // AddAnimation_AssetLz4
            // 
            this.AddAnimation_AssetLz4.Location = new System.Drawing.Point(12, 319);
            this.AddAnimation_AssetLz4.Name = "AddAnimation_AssetLz4";
            this.AddAnimation_AssetLz4.Size = new System.Drawing.Size(348, 39);
            this.AddAnimation_AssetLz4.TabIndex = 1;
            this.AddAnimation_AssetLz4.Text = "Import Animation Clip From Asset Bundle";
            this.AddAnimation_AssetLz4.UseVisualStyleBackColor = true;
            this.AddAnimation_AssetLz4.Click += new System.EventHandler(this.AddAnimation_AssetLz4_Click);
            // 
            // AddAnimation_AddressableLz4
            // 
            this.AddAnimation_AddressableLz4.Location = new System.Drawing.Point(12, 364);
            this.AddAnimation_AddressableLz4.Name = "AddAnimation_AddressableLz4";
            this.AddAnimation_AddressableLz4.Size = new System.Drawing.Size(348, 39);
            this.AddAnimation_AddressableLz4.TabIndex = 2;
            this.AddAnimation_AddressableLz4.Text = "Import Animation Clip From Addressable Bundle";
            this.AddAnimation_AddressableLz4.UseVisualStyleBackColor = true;
            this.AddAnimation_AddressableLz4.Click += new System.EventHandler(this.AddAnimation_AddressableLz4_Click);
            // 
            // CustomAssetPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 423);
            this.Controls.Add(this.AddAnimation_AddressableLz4);
            this.Controls.Add(this.AddAnimation_AssetLz4);
            this.Controls.Add(this.AddBackground_Button);
            this.Controls.Add(this.CustomAssetsListView);
            this.Controls.Add(this.CustomAssetsPreviewPic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "Custom Asset Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CustomAssetPreview_FormClosing);
            this.Load += new System.EventHandler(this.CustomAssetPreview_Load);
            this.Click += new System.EventHandler(this.CustomAssetPreview_Click);
            ((System.ComponentModel.ISupportInitialize)(this.CustomAssetsPreviewPic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox CustomAssetsPreviewPic;
        private System.Windows.Forms.ListView CustomAssetsListView;
        private System.Windows.Forms.Button AddBackground_Button;
        private System.Windows.Forms.Button AddAnimation_AssetLz4;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.ColumnHeader Name;
        private System.Windows.Forms.ColumnHeader FilePath;
        private System.Windows.Forms.Button AddAnimation_AddressableLz4;
    }
}