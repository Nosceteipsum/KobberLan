namespace KobberLan
{
    partial class SuggestedGameInternetControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_GameTitle = new System.Windows.Forms.Label();
            this.button_like = new System.Windows.Forms.Button();
            this.label_likes = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox_Cover = new System.Windows.Forms.PictureBox();
            this.button_Play = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Cover)).BeginInit();
            this.SuspendLayout();
            // 
            // label_GameTitle
            // 
            this.label_GameTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_GameTitle.AutoEllipsis = true;
            this.label_GameTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_GameTitle.Location = new System.Drawing.Point(-2, 0);
            this.label_GameTitle.Name = "label_GameTitle";
            this.label_GameTitle.Size = new System.Drawing.Size(209, 41);
            this.label_GameTitle.TabIndex = 1;
            this.label_GameTitle.Text = "{GameTitle}";
            this.label_GameTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label_GameTitle.Paint += new System.Windows.Forms.PaintEventHandler(this.label_GameTitle_Paint);
            // 
            // button_like
            // 
            this.button_like.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_like.Location = new System.Drawing.Point(12, 284);
            this.button_like.Name = "button_like";
            this.button_like.Size = new System.Drawing.Size(50, 23);
            this.button_like.TabIndex = 2;
            this.button_like.Text = "Like";
            this.button_like.UseVisualStyleBackColor = true;
            this.button_like.Click += new System.EventHandler(this.button_like_Click);
            // 
            // label_likes
            // 
            this.label_likes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_likes.Location = new System.Drawing.Point(30, 253);
            this.label_likes.Name = "label_likes";
            this.label_likes.Size = new System.Drawing.Size(32, 16);
            this.label_likes.TabIndex = 6;
            this.label_likes.Text = "00";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox2.Image = global::KobberLan.Properties.Resources.ThumbsUp;
            this.pictureBox2.Location = new System.Drawing.Point(10, 254);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox_Cover
            // 
            this.pictureBox_Cover.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_Cover.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBox_Cover.Location = new System.Drawing.Point(6, 44);
            this.pictureBox_Cover.Name = "pictureBox_Cover";
            this.pictureBox_Cover.Size = new System.Drawing.Size(196, 200);
            this.pictureBox_Cover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Cover.TabIndex = 0;
            this.pictureBox_Cover.TabStop = false;
            // 
            // button_Play
            // 
            this.button_Play.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Play.Enabled = false;
            this.button_Play.Location = new System.Drawing.Point(148, 284);
            this.button_Play.Name = "button_Play";
            this.button_Play.Size = new System.Drawing.Size(50, 23);
            this.button_Play.TabIndex = 13;
            this.button_Play.Text = "Play";
            this.button_Play.UseVisualStyleBackColor = true;
            this.button_Play.Click += new System.EventHandler(this.button_Play_Click);
            // 
            // SuggestedGameInternetControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.button_Play);
            this.Controls.Add(this.label_likes);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.button_like);
            this.Controls.Add(this.label_GameTitle);
            this.Controls.Add(this.pictureBox_Cover);
            this.Name = "SuggestedGameInternetControl";
            this.Size = new System.Drawing.Size(205, 314);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Cover)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Cover;
        private System.Windows.Forms.Label label_GameTitle;
        private System.Windows.Forms.Button button_like;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label_likes;
        private System.Windows.Forms.Button button_Play;
    }
}
