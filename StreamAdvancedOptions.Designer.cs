namespace CustomStreamMaker
{
    partial class StreamAdvancedOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StreamAdvancedOptions));
            this.StreamIntro_Group = new System.Windows.Forms.GroupBox();
            this.IsDarkAngelIntro_Check = new System.Windows.Forms.CheckBox();
            this.HasIntro_Check = new System.Windows.Forms.CheckBox();
            this.StreamInterface_Group = new System.Windows.Forms.GroupBox();
            this.DarkStream_Check = new System.Windows.Forms.CheckBox();
            this.CustomDay_Numeric = new System.Windows.Forms.NumericUpDown();
            this.CustomFollowers_Numeric = new System.Windows.Forms.NumericUpDown();
            this.CustomDay_Check = new System.Windows.Forms.CheckBox();
            this.HasCustomFollowers_Check = new System.Windows.Forms.CheckBox();
            this.GameBorders_Group = new System.Windows.Forms.GroupBox();
            this.RemoveBorders_Check = new System.Windows.Forms.CheckBox();
            this.InvertColors_Check = new System.Windows.Forms.CheckBox();
            this.StreamEnd_Group = new System.Windows.Forms.GroupBox();
            this.OpenEndScreenImg_Button = new System.Windows.Forms.Button();
            this.CustomEndScreen_Text = new System.Windows.Forms.TextBox();
            this.HasCustomEndScreen_Check = new System.Windows.Forms.CheckBox();
            this.HasEndScreen_Check = new System.Windows.Forms.CheckBox();
            this.Chair_Group = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GameChair_Checked = new System.Windows.Forms.CheckBox();
            this.StreamIntro_Group.SuspendLayout();
            this.StreamInterface_Group.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustomDay_Numeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomFollowers_Numeric)).BeginInit();
            this.GameBorders_Group.SuspendLayout();
            this.StreamEnd_Group.SuspendLayout();
            this.Chair_Group.SuspendLayout();
            this.SuspendLayout();
            // 
            // StreamIntro_Group
            // 
            this.StreamIntro_Group.Controls.Add(this.IsDarkAngelIntro_Check);
            this.StreamIntro_Group.Controls.Add(this.HasIntro_Check);
            this.StreamIntro_Group.Location = new System.Drawing.Point(13, 13);
            this.StreamIntro_Group.Name = "StreamIntro_Group";
            this.StreamIntro_Group.Size = new System.Drawing.Size(305, 83);
            this.StreamIntro_Group.TabIndex = 0;
            this.StreamIntro_Group.TabStop = false;
            this.StreamIntro_Group.Text = "Stream Intro";
            // 
            // IsDarkAngelIntro_Check
            // 
            this.IsDarkAngelIntro_Check.AutoSize = true;
            this.IsDarkAngelIntro_Check.Location = new System.Drawing.Point(10, 48);
            this.IsDarkAngelIntro_Check.Name = "IsDarkAngelIntro_Check";
            this.IsDarkAngelIntro_Check.Size = new System.Drawing.Size(174, 17);
            this.IsDarkAngelIntro_Check.TabIndex = 1;
            this.IsDarkAngelIntro_Check.Text = "Use Dark Angel Transformation";
            this.IsDarkAngelIntro_Check.UseVisualStyleBackColor = true;
            this.IsDarkAngelIntro_Check.CheckedChanged += new System.EventHandler(this.IsDarkAngelIntro_Check_CheckedChanged);
            // 
            // HasIntro_Check
            // 
            this.HasIntro_Check.AutoSize = true;
            this.HasIntro_Check.Location = new System.Drawing.Point(10, 22);
            this.HasIntro_Check.Name = "HasIntro_Check";
            this.HasIntro_Check.Size = new System.Drawing.Size(186, 17);
            this.HasIntro_Check.TabIndex = 0;
            this.HasIntro_Check.Text = "Open With Stream Transformation";
            this.HasIntro_Check.UseVisualStyleBackColor = true;
            this.HasIntro_Check.CheckedChanged += new System.EventHandler(this.HasIntro_Check_CheckedChanged);
            this.HasIntro_Check.EnabledChanged += new System.EventHandler(this.HasIntro_Check_EnabledChanged);
            // 
            // StreamInterface_Group
            // 
            this.StreamInterface_Group.Controls.Add(this.DarkStream_Check);
            this.StreamInterface_Group.Controls.Add(this.CustomDay_Numeric);
            this.StreamInterface_Group.Controls.Add(this.CustomFollowers_Numeric);
            this.StreamInterface_Group.Controls.Add(this.CustomDay_Check);
            this.StreamInterface_Group.Controls.Add(this.HasCustomFollowers_Check);
            this.StreamInterface_Group.Location = new System.Drawing.Point(13, 103);
            this.StreamInterface_Group.Name = "StreamInterface_Group";
            this.StreamInterface_Group.Size = new System.Drawing.Size(305, 124);
            this.StreamInterface_Group.TabIndex = 0;
            this.StreamInterface_Group.TabStop = false;
            this.StreamInterface_Group.Text = "Stream Interface";
            // 
            // DarkStream_Check
            // 
            this.DarkStream_Check.AutoSize = true;
            this.DarkStream_Check.Location = new System.Drawing.Point(9, 28);
            this.DarkStream_Check.Name = "DarkStream_Check";
            this.DarkStream_Check.Size = new System.Drawing.Size(152, 17);
            this.DarkStream_Check.TabIndex = 6;
            this.DarkStream_Check.Text = "Use Dark Stream Interface";
            this.DarkStream_Check.UseVisualStyleBackColor = true;
            this.DarkStream_Check.CheckedChanged += new System.EventHandler(this.DarkStream_Check_CheckedChanged);
            // 
            // CustomDay_Numeric
            // 
            this.CustomDay_Numeric.Location = new System.Drawing.Point(172, 64);
            this.CustomDay_Numeric.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.CustomDay_Numeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CustomDay_Numeric.Name = "CustomDay_Numeric";
            this.CustomDay_Numeric.Size = new System.Drawing.Size(120, 20);
            this.CustomDay_Numeric.TabIndex = 3;
            this.CustomDay_Numeric.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.CustomDay_Numeric.ValueChanged += new System.EventHandler(this.CustomDay_Numeric_ValueChanged);
            // 
            // CustomFollowers_Numeric
            // 
            this.CustomFollowers_Numeric.Location = new System.Drawing.Point(172, 90);
            this.CustomFollowers_Numeric.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.CustomFollowers_Numeric.Name = "CustomFollowers_Numeric";
            this.CustomFollowers_Numeric.Size = new System.Drawing.Size(120, 20);
            this.CustomFollowers_Numeric.TabIndex = 5;
            this.CustomFollowers_Numeric.ValueChanged += new System.EventHandler(this.CustomFollowers_Numeric_ValueChanged);
            // 
            // CustomDay_Check
            // 
            this.CustomDay_Check.AutoSize = true;
            this.CustomDay_Check.Location = new System.Drawing.Point(9, 65);
            this.CustomDay_Check.Name = "CustomDay_Check";
            this.CustomDay_Check.Size = new System.Drawing.Size(108, 17);
            this.CustomDay_Check.TabIndex = 2;
            this.CustomDay_Check.Text = "Use Custom Day ";
            this.CustomDay_Check.UseVisualStyleBackColor = true;
            this.CustomDay_Check.CheckedChanged += new System.EventHandler(this.CustomDay_Check_CheckedChanged);
            // 
            // HasCustomFollowers_Check
            // 
            this.HasCustomFollowers_Check.AutoSize = true;
            this.HasCustomFollowers_Check.Location = new System.Drawing.Point(9, 91);
            this.HasCustomFollowers_Check.Name = "HasCustomFollowers_Check";
            this.HasCustomFollowers_Check.Size = new System.Drawing.Size(156, 17);
            this.HasCustomFollowers_Check.TabIndex = 4;
            this.HasCustomFollowers_Check.Text = "Use Custom Follower Count";
            this.HasCustomFollowers_Check.UseVisualStyleBackColor = true;
            this.HasCustomFollowers_Check.CheckedChanged += new System.EventHandler(this.HasCustomFollowers_Check_CheckedChanged);
            // 
            // GameBorders_Group
            // 
            this.GameBorders_Group.Controls.Add(this.RemoveBorders_Check);
            this.GameBorders_Group.Controls.Add(this.InvertColors_Check);
            this.GameBorders_Group.Location = new System.Drawing.Point(13, 313);
            this.GameBorders_Group.Name = "GameBorders_Group";
            this.GameBorders_Group.Size = new System.Drawing.Size(305, 86);
            this.GameBorders_Group.TabIndex = 0;
            this.GameBorders_Group.TabStop = false;
            this.GameBorders_Group.Text = "Game Borders";
            // 
            // RemoveBorders_Check
            // 
            this.RemoveBorders_Check.AutoSize = true;
            this.RemoveBorders_Check.Location = new System.Drawing.Point(10, 48);
            this.RemoveBorders_Check.Name = "RemoveBorders_Check";
            this.RemoveBorders_Check.Size = new System.Drawing.Size(105, 17);
            this.RemoveBorders_Check.TabIndex = 7;
            this.RemoveBorders_Check.Text = "Remove Borders";
            this.RemoveBorders_Check.UseVisualStyleBackColor = true;
            this.RemoveBorders_Check.CheckedChanged += new System.EventHandler(this.RemoveBorders_Check_CheckedChanged);
            // 
            // InvertColors_Check
            // 
            this.InvertColors_Check.AutoSize = true;
            this.InvertColors_Check.Location = new System.Drawing.Point(10, 22);
            this.InvertColors_Check.Name = "InvertColors_Check";
            this.InvertColors_Check.Size = new System.Drawing.Size(119, 17);
            this.InvertColors_Check.TabIndex = 6;
            this.InvertColors_Check.Text = "Invert Border Colors";
            this.InvertColors_Check.UseVisualStyleBackColor = true;
            this.InvertColors_Check.CheckedChanged += new System.EventHandler(this.InvertColors_Check_CheckedChanged);
            // 
            // StreamEnd_Group
            // 
            this.StreamEnd_Group.Controls.Add(this.OpenEndScreenImg_Button);
            this.StreamEnd_Group.Controls.Add(this.CustomEndScreen_Text);
            this.StreamEnd_Group.Controls.Add(this.HasCustomEndScreen_Check);
            this.StreamEnd_Group.Controls.Add(this.HasEndScreen_Check);
            this.StreamEnd_Group.Location = new System.Drawing.Point(13, 405);
            this.StreamEnd_Group.Name = "StreamEnd_Group";
            this.StreamEnd_Group.Size = new System.Drawing.Size(305, 106);
            this.StreamEnd_Group.TabIndex = 0;
            this.StreamEnd_Group.TabStop = false;
            this.StreamEnd_Group.Text = "Stream End";
            // 
            // OpenEndScreenImg_Button
            // 
            this.OpenEndScreenImg_Button.Location = new System.Drawing.Point(9, 69);
            this.OpenEndScreenImg_Button.Name = "OpenEndScreenImg_Button";
            this.OpenEndScreenImg_Button.Size = new System.Drawing.Size(56, 23);
            this.OpenEndScreenImg_Button.TabIndex = 10;
            this.OpenEndScreenImg_Button.Text = "Browse";
            this.OpenEndScreenImg_Button.UseVisualStyleBackColor = true;
            this.OpenEndScreenImg_Button.Click += new System.EventHandler(this.OpenEndScreenImg_Button_Click);
            // 
            // CustomEndScreen_Text
            // 
            this.CustomEndScreen_Text.Location = new System.Drawing.Point(72, 71);
            this.CustomEndScreen_Text.Name = "CustomEndScreen_Text";
            this.CustomEndScreen_Text.Size = new System.Drawing.Size(220, 20);
            this.CustomEndScreen_Text.TabIndex = 11;
            this.CustomEndScreen_Text.DoubleClick += new System.EventHandler(this.CustomEndScreen_Text_DoubleClick);
            this.CustomEndScreen_Text.Leave += new System.EventHandler(this.CustomEndScreen_Text_Leave);
            this.CustomEndScreen_Text.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.CustomEndScreen_Text_PreviewKeyDown);
            // 
            // HasCustomEndScreen_Check
            // 
            this.HasCustomEndScreen_Check.AutoSize = true;
            this.HasCustomEndScreen_Check.Location = new System.Drawing.Point(10, 45);
            this.HasCustomEndScreen_Check.Name = "HasCustomEndScreen_Check";
            this.HasCustomEndScreen_Check.Size = new System.Drawing.Size(142, 17);
            this.HasCustomEndScreen_Check.TabIndex = 9;
            this.HasCustomEndScreen_Check.Text = "Has Custom End Screen";
            this.HasCustomEndScreen_Check.UseVisualStyleBackColor = true;
            this.HasCustomEndScreen_Check.CheckedChanged += new System.EventHandler(this.HasCustomEndScreen_Check_CheckedChanged);
            // 
            // HasEndScreen_Check
            // 
            this.HasEndScreen_Check.AutoSize = true;
            this.HasEndScreen_Check.Location = new System.Drawing.Point(10, 22);
            this.HasEndScreen_Check.Name = "HasEndScreen_Check";
            this.HasEndScreen_Check.Size = new System.Drawing.Size(104, 17);
            this.HasEndScreen_Check.TabIndex = 8;
            this.HasEndScreen_Check.Text = "Has End Screen";
            this.HasEndScreen_Check.UseVisualStyleBackColor = true;
            this.HasEndScreen_Check.CheckedChanged += new System.EventHandler(this.HasEndScreen_Check_CheckedChanged);
            this.HasEndScreen_Check.EnabledChanged += new System.EventHandler(this.HasEndScreen_Check_EnabledChanged);
            // 
            // Chair_Group
            // 
            this.Chair_Group.Controls.Add(this.label1);
            this.Chair_Group.Controls.Add(this.GameChair_Checked);
            this.Chair_Group.Location = new System.Drawing.Point(12, 235);
            this.Chair_Group.Name = "Chair_Group";
            this.Chair_Group.Size = new System.Drawing.Size(305, 72);
            this.Chair_Group.TabIndex = 0;
            this.Chair_Group.TabStop = false;
            this.Chair_Group.Text = "Stream Chair";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(11, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "* Only applicable to some backgrounds";
            // 
            // GameChair_Checked
            // 
            this.GameChair_Checked.AutoSize = true;
            this.GameChair_Checked.Checked = true;
            this.GameChair_Checked.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GameChair_Checked.Location = new System.Drawing.Point(10, 22);
            this.GameChair_Checked.Name = "GameChair_Checked";
            this.GameChair_Checked.Size = new System.Drawing.Size(111, 17);
            this.GameChair_Checked.TabIndex = 6;
            this.GameChair_Checked.Text = "Has Gaming Chair";
            this.GameChair_Checked.UseVisualStyleBackColor = true;
            this.GameChair_Checked.CheckedChanged += new System.EventHandler(this.GameChair_Checked_CheckedChanged);
            // 
            // StreamAdvancedOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 529);
            this.Controls.Add(this.Chair_Group);
            this.Controls.Add(this.GameBorders_Group);
            this.Controls.Add(this.StreamInterface_Group);
            this.Controls.Add(this.StreamEnd_Group);
            this.Controls.Add(this.StreamIntro_Group);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StreamAdvancedOptions";
            this.Text = "Other Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StreamAdvancedOptions_FormClosing);
            this.Load += new System.EventHandler(this.StreamAdvancedOptions_Load);
            this.StreamIntro_Group.ResumeLayout(false);
            this.StreamIntro_Group.PerformLayout();
            this.StreamInterface_Group.ResumeLayout(false);
            this.StreamInterface_Group.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustomDay_Numeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomFollowers_Numeric)).EndInit();
            this.GameBorders_Group.ResumeLayout(false);
            this.GameBorders_Group.PerformLayout();
            this.StreamEnd_Group.ResumeLayout(false);
            this.StreamEnd_Group.PerformLayout();
            this.Chair_Group.ResumeLayout(false);
            this.Chair_Group.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox StreamIntro_Group;
        private System.Windows.Forms.CheckBox IsDarkAngelIntro_Check;
        private System.Windows.Forms.CheckBox HasIntro_Check;
        private System.Windows.Forms.GroupBox StreamInterface_Group;
        private System.Windows.Forms.CheckBox HasCustomFollowers_Check;
        private System.Windows.Forms.NumericUpDown CustomFollowers_Numeric;
        private System.Windows.Forms.GroupBox GameBorders_Group;
        private System.Windows.Forms.CheckBox RemoveBorders_Check;
        private System.Windows.Forms.CheckBox InvertColors_Check;
        private System.Windows.Forms.GroupBox StreamEnd_Group;
        private System.Windows.Forms.CheckBox HasEndScreen_Check;
        private System.Windows.Forms.NumericUpDown CustomDay_Numeric;
        private System.Windows.Forms.CheckBox CustomDay_Check;
        private System.Windows.Forms.CheckBox HasCustomEndScreen_Check;
        private System.Windows.Forms.TextBox CustomEndScreen_Text;
        private System.Windows.Forms.Button OpenEndScreenImg_Button;
        private System.Windows.Forms.GroupBox Chair_Group;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox GameChair_Checked;
        private System.Windows.Forms.CheckBox DarkStream_Check;
    }
}