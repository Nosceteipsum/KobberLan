namespace KobberLan.Gui
{
    partial class OverviewControl
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
            this.button_SuggestGame = new System.Windows.Forms.Button();
            this.label_GamesFound = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_SuggestGame
            // 
            this.button_SuggestGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_SuggestGame.Location = new System.Drawing.Point(4, 102);
            this.button_SuggestGame.Name = "button_SuggestGame";
            this.button_SuggestGame.Size = new System.Drawing.Size(198, 210);
            this.button_SuggestGame.TabIndex = 8;
            this.button_SuggestGame.Text = "Suggest game";
            this.button_SuggestGame.UseVisualStyleBackColor = true;
            this.button_SuggestGame.Click += new System.EventHandler(this.button_SuggestGame_Click);
            // 
            // label_GamesFound
            // 
            this.label_GamesFound.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label_GamesFound.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_GamesFound.Location = new System.Drawing.Point(4, 35);
            this.label_GamesFound.Name = "label_GamesFound";
            this.label_GamesFound.Size = new System.Drawing.Size(198, 64);
            this.label_GamesFound.TabIndex = 7;
            this.label_GamesFound.Text = "12345";
            this.label_GamesFound.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(4, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(198, 33);
            this.label2.TabIndex = 6;
            this.label2.Text = "Games found:";
            // 
            // OverviewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.button_SuggestGame);
            this.Controls.Add(this.label_GamesFound);
            this.Controls.Add(this.label2);
            this.Name = "OverviewControl";
            this.Size = new System.Drawing.Size(205, 315);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_SuggestGame;
        private System.Windows.Forms.Label label_GamesFound;
        private System.Windows.Forms.Label label2;
    }
}
