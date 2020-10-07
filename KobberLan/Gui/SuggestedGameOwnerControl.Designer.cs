namespace KobberLan
{
    partial class SuggestedGameOwnerControl
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
            this.button_ShareGet = new System.Windows.Forms.Button();
            this.label_likes = new System.Windows.Forms.Label();
            this.label_Downloading = new System.Windows.Forms.Label();
            this.label_Peers = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label_ProgresBar = new System.Windows.Forms.Label();
            this.button_Play = new System.Windows.Forms.Button();
            this.button_Clear = new System.Windows.Forms.Button();
            this.label_Ingame = new System.Windows.Forms.Label();
            this.panel_Ingame = new System.Windows.Forms.Panel();
            this.pictureBox_Ingame = new System.Windows.Forms.PictureBox();
            this.pictureBox_Peers = new System.Windows.Forms.PictureBox();
            this.pictureBox_Downloaded = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox_Cover = new System.Windows.Forms.PictureBox();
            this.panel_Ingame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Ingame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Peers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Downloaded)).BeginInit();
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
            this.button_like.Location = new System.Drawing.Point(6, 284);
            this.button_like.Name = "button_like";
            this.button_like.Size = new System.Drawing.Size(44, 23);
            this.button_like.TabIndex = 2;
            this.button_like.Text = "Like";
            this.button_like.UseVisualStyleBackColor = true;
            this.button_like.Click += new System.EventHandler(this.button_like_Click);
            // 
            // button_ShareGet
            // 
            this.button_ShareGet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ShareGet.Location = new System.Drawing.Point(56, 284);
            this.button_ShareGet.Name = "button_ShareGet";
            this.button_ShareGet.Size = new System.Drawing.Size(44, 23);
            this.button_ShareGet.TabIndex = 3;
            this.button_ShareGet.Text = "Share";
            this.button_ShareGet.UseVisualStyleBackColor = true;
            this.button_ShareGet.Click += new System.EventHandler(this.button_ShareGet_Click);
            // 
            // label_likes
            // 
            this.label_likes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_likes.Location = new System.Drawing.Point(22, 253);
            this.label_likes.Name = "label_likes";
            this.label_likes.Size = new System.Drawing.Size(32, 16);
            this.label_likes.TabIndex = 6;
            this.label_likes.Text = "00";
            // 
            // label_Downloading
            // 
            this.label_Downloading.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Downloading.Location = new System.Drawing.Point(82, 254);
            this.label_Downloading.Name = "label_Downloading";
            this.label_Downloading.Size = new System.Drawing.Size(68, 16);
            this.label_Downloading.TabIndex = 8;
            this.label_Downloading.Text = "00 / 00";
            this.label_Downloading.Visible = false;
            // 
            // label_Peers
            // 
            this.label_Peers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Peers.Location = new System.Drawing.Point(176, 253);
            this.label_Peers.Name = "label_Peers";
            this.label_Peers.Size = new System.Drawing.Size(29, 16);
            this.label_Peers.TabIndex = 10;
            this.label_Peers.Text = "01";
            this.label_Peers.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(6, 198);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(196, 23);
            this.progressBar.Step = 1;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 11;
            this.progressBar.Visible = false;
            // 
            // label_ProgresBar
            // 
            this.label_ProgresBar.Location = new System.Drawing.Point(6, 221);
            this.label_ProgresBar.Name = "label_ProgresBar";
            this.label_ProgresBar.Size = new System.Drawing.Size(196, 23);
            this.label_ProgresBar.TabIndex = 12;
            this.label_ProgresBar.Text = "Status";
            this.label_ProgresBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_ProgresBar.Visible = false;
            // 
            // button_Play
            // 
            this.button_Play.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Play.Enabled = false;
            this.button_Play.Location = new System.Drawing.Point(156, 284);
            this.button_Play.Name = "button_Play";
            this.button_Play.Size = new System.Drawing.Size(44, 23);
            this.button_Play.TabIndex = 13;
            this.button_Play.Text = "Play";
            this.button_Play.UseVisualStyleBackColor = true;
            this.button_Play.Click += new System.EventHandler(this.button_Play_Click);
            // 
            // button_Clear
            // 
            this.button_Clear.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Clear.Location = new System.Drawing.Point(106, 284);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(44, 23);
            this.button_Clear.TabIndex = 14;
            this.button_Clear.Text = "Clear";
            this.button_Clear.UseVisualStyleBackColor = true;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // label_Ingame
            // 
            this.label_Ingame.BackColor = System.Drawing.Color.Transparent;
            this.label_Ingame.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Ingame.Location = new System.Drawing.Point(36, 9);
            this.label_Ingame.Name = "label_Ingame";
            this.label_Ingame.Size = new System.Drawing.Size(29, 16);
            this.label_Ingame.TabIndex = 16;
            this.label_Ingame.Text = "00";
            this.label_Ingame.Visible = false;
            // 
            // panel_Ingame
            // 
            this.panel_Ingame.Controls.Add(this.label_Ingame);
            this.panel_Ingame.Controls.Add(this.pictureBox_Ingame);
            this.panel_Ingame.Location = new System.Drawing.Point(136, 221);
            this.panel_Ingame.Name = "panel_Ingame";
            this.panel_Ingame.Size = new System.Drawing.Size(69, 32);
            this.panel_Ingame.TabIndex = 17;
            this.panel_Ingame.Visible = false;
            // 
            // pictureBox_Ingame
            // 
            this.pictureBox_Ingame.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_Ingame.Image = global::KobberLan.Properties.Resources._16x16;
            this.pictureBox_Ingame.Location = new System.Drawing.Point(14, 9);
            this.pictureBox_Ingame.Name = "pictureBox_Ingame";
            this.pictureBox_Ingame.Size = new System.Drawing.Size(16, 16);
            this.pictureBox_Ingame.TabIndex = 15;
            this.pictureBox_Ingame.TabStop = false;
            this.pictureBox_Ingame.Visible = false;
            // 
            // pictureBox_Peers
            // 
            this.pictureBox_Peers.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox_Peers.Image = global::KobberLan.Properties.Resources.Person;
            this.pictureBox_Peers.Location = new System.Drawing.Point(160, 255);
            this.pictureBox_Peers.Name = "pictureBox_Peers";
            this.pictureBox_Peers.Size = new System.Drawing.Size(16, 16);
            this.pictureBox_Peers.TabIndex = 9;
            this.pictureBox_Peers.TabStop = false;
            this.pictureBox_Peers.Visible = false;
            // 
            // pictureBox_Downloaded
            // 
            this.pictureBox_Downloaded.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox_Downloaded.Image = global::KobberLan.Properties.Resources.Floppy;
            this.pictureBox_Downloaded.Location = new System.Drawing.Point(65, 255);
            this.pictureBox_Downloaded.Name = "pictureBox_Downloaded";
            this.pictureBox_Downloaded.Size = new System.Drawing.Size(21, 16);
            this.pictureBox_Downloaded.TabIndex = 7;
            this.pictureBox_Downloaded.TabStop = false;
            this.pictureBox_Downloaded.Visible = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox2.Image = global::KobberLan.Properties.Resources.ThumbsUp;
            this.pictureBox2.Location = new System.Drawing.Point(7, 254);
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
            // SuggestedGameOwnerControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.panel_Ingame);
            this.Controls.Add(this.button_Clear);
            this.Controls.Add(this.button_Play);
            this.Controls.Add(this.label_ProgresBar);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.label_Peers);
            this.Controls.Add(this.pictureBox_Peers);
            this.Controls.Add(this.label_Downloading);
            this.Controls.Add(this.pictureBox_Downloaded);
            this.Controls.Add(this.label_likes);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.button_ShareGet);
            this.Controls.Add(this.button_like);
            this.Controls.Add(this.label_GameTitle);
            this.Controls.Add(this.pictureBox_Cover);
            this.Name = "SuggestedGameOwnerControl";
            this.Size = new System.Drawing.Size(205, 314);
            this.panel_Ingame.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Ingame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Peers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Downloaded)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Cover)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Cover;
        private System.Windows.Forms.Label label_GameTitle;
        private System.Windows.Forms.Button button_like;
        private System.Windows.Forms.Button button_ShareGet;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label_likes;
        private System.Windows.Forms.Label label_Downloading;
        private System.Windows.Forms.PictureBox pictureBox_Downloaded;
        private System.Windows.Forms.Label label_Peers;
        private System.Windows.Forms.PictureBox pictureBox_Peers;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label_ProgresBar;
        private System.Windows.Forms.Button button_Play;
        private System.Windows.Forms.Button button_Clear;
        private System.Windows.Forms.Label label_Ingame;
        private System.Windows.Forms.PictureBox pictureBox_Ingame;
        private System.Windows.Forms.Panel panel_Ingame;
    }
}
