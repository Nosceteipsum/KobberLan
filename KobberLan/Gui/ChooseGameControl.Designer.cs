namespace KobberLan.Gui
{
    partial class ChooseGameControl
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
            this.pictureBox_Game = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Game)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_Game
            // 
            this.pictureBox_Game.Location = new System.Drawing.Point(0, 3);
            this.pictureBox_Game.Name = "pictureBox_Game";
            this.pictureBox_Game.Size = new System.Drawing.Size(256, 253);
            this.pictureBox_Game.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Game.TabIndex = 0;
            this.pictureBox_Game.TabStop = false;
            this.pictureBox_Game.Click += new System.EventHandler(this.pictureBox_Game_Click);
            // 
            // ChooseGameControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox_Game);
            this.Name = "ChooseGameControl";
            this.Size = new System.Drawing.Size(256, 256);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Game)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Game;
    }
}
