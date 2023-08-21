namespace CustomStreamMaker
{
    partial class SuperRepliesForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SuperRepliesForm));
            this.SpritePreview = new System.Windows.Forms.PictureBox();
            this.SuperRepliesListView = new System.Windows.Forms.DataGridView();
            this.KAngelDialogue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KAnimReply_List = new System.Windows.Forms.ComboBox();
            this.KAngelReply_Text = new System.Windows.Forms.TextBox();
            this.KAnimReply_Label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SuperChatComment = new System.Windows.Forms.TextBox();
            this.AddEditSuperReply_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SpritePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SuperRepliesListView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SpritePreview
            // 
            this.SpritePreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SpritePreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SpritePreview.Location = new System.Drawing.Point(12, 12);
            this.SpritePreview.Name = "SpritePreview";
            this.SpritePreview.Size = new System.Drawing.Size(348, 227);
            this.SpritePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.SpritePreview.TabIndex = 0;
            this.SpritePreview.TabStop = false;
            // 
            // SuperRepliesListView
            // 
            this.SuperRepliesListView.AllowUserToAddRows = false;
            this.SuperRepliesListView.AllowUserToDeleteRows = false;
            this.SuperRepliesListView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.SuperRepliesListView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.SuperRepliesListView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SuperRepliesListView.ColumnHeadersVisible = false;
            this.SuperRepliesListView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.KAngelDialogue});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SuperRepliesListView.DefaultCellStyle = dataGridViewCellStyle1;
            this.SuperRepliesListView.Location = new System.Drawing.Point(12, 254);
            this.SuperRepliesListView.MultiSelect = false;
            this.SuperRepliesListView.Name = "SuperRepliesListView";
            this.SuperRepliesListView.RowHeadersVisible = false;
            this.SuperRepliesListView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.SuperRepliesListView.Size = new System.Drawing.Size(348, 156);
            this.SuperRepliesListView.TabIndex = 1;
            this.SuperRepliesListView.SelectionChanged += new System.EventHandler(this.SuperRepliesListView_SelectionChanged);
            this.SuperRepliesListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SuperRepliesListView_KeyDown);
            // 
            // KAngelDialogue
            // 
            this.KAngelDialogue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.KAngelDialogue.HeaderText = "Dialogue";
            this.KAngelDialogue.Name = "KAngelDialogue";
            this.KAngelDialogue.ReadOnly = true;
            // 
            // KAnimReply_List
            // 
            this.KAnimReply_List.FormattingEnabled = true;
            this.KAnimReply_List.Location = new System.Drawing.Point(377, 180);
            this.KAnimReply_List.Name = "KAnimReply_List";
            this.KAnimReply_List.Size = new System.Drawing.Size(348, 21);
            this.KAnimReply_List.TabIndex = 2;
            this.KAnimReply_List.SelectedIndexChanged += new System.EventHandler(this.KAnimReply_List_SelectedIndexChanged);
            this.KAnimReply_List.Leave += new System.EventHandler(this.KAnimReply_List_Leave);
            this.KAnimReply_List.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.KAnimReply_List_PreviewKeyDown);
            // 
            // KAngelReply_Text
            // 
            this.KAngelReply_Text.Location = new System.Drawing.Point(377, 244);
            this.KAngelReply_Text.MaxLength = 110;
            this.KAngelReply_Text.Multiline = true;
            this.KAngelReply_Text.Name = "KAngelReply_Text";
            this.KAngelReply_Text.Size = new System.Drawing.Size(348, 99);
            this.KAngelReply_Text.TabIndex = 3;
            this.KAngelReply_Text.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.KAngelReply_Text_PreviewKeyDown);
            // 
            // KAnimReply_Label
            // 
            this.KAnimReply_Label.AutoSize = true;
            this.KAnimReply_Label.Location = new System.Drawing.Point(373, 155);
            this.KAnimReply_Label.Name = "KAnimReply_Label";
            this.KAnimReply_Label.Size = new System.Drawing.Size(90, 13);
            this.KAnimReply_Label.TabIndex = 4;
            this.KAnimReply_Label.Text = "KAngel Animation";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(374, 220);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "KAngel Dialogue";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SuperChatComment);
            this.groupBox1.Location = new System.Drawing.Point(376, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(349, 123);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Super Chat Comment";
            // 
            // SuperChatComment
            // 
            this.SuperChatComment.BackColor = System.Drawing.SystemColors.Control;
            this.SuperChatComment.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SuperChatComment.Enabled = false;
            this.SuperChatComment.Location = new System.Drawing.Point(15, 33);
            this.SuperChatComment.Multiline = true;
            this.SuperChatComment.Name = "SuperChatComment";
            this.SuperChatComment.Size = new System.Drawing.Size(319, 61);
            this.SuperChatComment.TabIndex = 0;
            this.SuperChatComment.Click += new System.EventHandler(this.SuperChatComment_Click);
            this.SuperChatComment.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SuperChatComment_MouseClick);
            // 
            // AddEditSuperReply_Button
            // 
            this.AddEditSuperReply_Button.Location = new System.Drawing.Point(377, 357);
            this.AddEditSuperReply_Button.Name = "AddEditSuperReply_Button";
            this.AddEditSuperReply_Button.Size = new System.Drawing.Size(348, 53);
            this.AddEditSuperReply_Button.TabIndex = 6;
            this.AddEditSuperReply_Button.Text = "Add Reply";
            this.AddEditSuperReply_Button.UseVisualStyleBackColor = true;
            this.AddEditSuperReply_Button.Click += new System.EventHandler(this.AddEditSuperReply_Button_Click);
            // 
            // SuperRepliesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 425);
            this.Controls.Add(this.AddEditSuperReply_Button);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.KAnimReply_Label);
            this.Controls.Add(this.KAngelReply_Text);
            this.Controls.Add(this.KAnimReply_List);
            this.Controls.Add(this.SuperRepliesListView);
            this.Controls.Add(this.SpritePreview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SuperRepliesForm";
            this.Text = "Extra Super Chat Replies";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SuperRepliesForm_FormClosing);
            this.Load += new System.EventHandler(this.SuperRepliesForm_Load);
            this.Click += new System.EventHandler(this.SuperRepliesForm_Click);
            ((System.ComponentModel.ISupportInitialize)(this.SpritePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SuperRepliesListView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox SpritePreview;
        private System.Windows.Forms.DataGridView SuperRepliesListView;
        private System.Windows.Forms.ComboBox KAnimReply_List;
        private System.Windows.Forms.TextBox KAngelReply_Text;
        private System.Windows.Forms.Label KAnimReply_Label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox SuperChatComment;
        private System.Windows.Forms.DataGridViewTextBoxColumn KAngelDialogue;
        private System.Windows.Forms.Button AddEditSuperReply_Button;
    }
}