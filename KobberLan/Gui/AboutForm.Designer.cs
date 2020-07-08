namespace KobberLan.Gui
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label_Version = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label_Version_Value = new System.Windows.Forms.Label();
            this.pictureBox_DonateBitcoin = new System.Windows.Forms.PictureBox();
            this.pictureBox_Paypal = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DonateBitcoin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Paypal)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Coded by:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Github:";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(107, 64);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(87, 20);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Source link";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label_Version
            // 
            this.label_Version.AutoSize = true;
            this.label_Version.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Version.Location = new System.Drawing.Point(21, 187);
            this.label_Version.Name = "label_Version";
            this.label_Version.Size = new System.Drawing.Size(67, 20);
            this.label_Version.TabIndex = 3;
            this.label_Version.Text = "Version:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(21, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "Libraries:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(21, 234);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 20);
            this.label5.TabIndex = 5;
            this.label5.Text = "Donate:";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(107, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(228, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "Tommy Kobberø Andersen";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(107, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(228, 62);
            this.label6.TabIndex = 7;
            this.label6.Text = "MonoTorrent, Desharp, BetterFolderBrowser and NewtonsoftJson";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Version_Value
            // 
            this.label_Version_Value.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Version_Value.Location = new System.Drawing.Point(107, 186);
            this.label_Version_Value.Name = "label_Version_Value";
            this.label_Version_Value.Size = new System.Drawing.Size(228, 23);
            this.label_Version_Value.TabIndex = 8;
            this.label_Version_Value.Text = "1.0";
            this.label_Version_Value.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox_DonateBitcoin
            // 
            this.pictureBox_DonateBitcoin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_DonateBitcoin.Image = global::KobberLan.Properties.Resources.bitcoin_donate;
            this.pictureBox_DonateBitcoin.Location = new System.Drawing.Point(111, 320);
            this.pictureBox_DonateBitcoin.Name = "pictureBox_DonateBitcoin";
            this.pictureBox_DonateBitcoin.Size = new System.Drawing.Size(198, 64);
            this.pictureBox_DonateBitcoin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_DonateBitcoin.TabIndex = 10;
            this.pictureBox_DonateBitcoin.TabStop = false;
            this.pictureBox_DonateBitcoin.Click += new System.EventHandler(this.pictureBox_DonateBitcoin_Click);
            // 
            // pictureBox_Paypal
            // 
            this.pictureBox_Paypal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_Paypal.Image = global::KobberLan.Properties.Resources.paypal_donate;
            this.pictureBox_Paypal.Location = new System.Drawing.Point(111, 234);
            this.pictureBox_Paypal.Name = "pictureBox_Paypal";
            this.pictureBox_Paypal.Size = new System.Drawing.Size(198, 77);
            this.pictureBox_Paypal.TabIndex = 9;
            this.pictureBox_Paypal.TabStop = false;
            this.pictureBox_Paypal.Click += new System.EventHandler(this.pictureBox_Paypal_Click);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 405);
            this.Controls.Add(this.pictureBox_DonateBitcoin);
            this.Controls.Add(this.pictureBox_Paypal);
            this.Controls.Add(this.label_Version_Value);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label_Version);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AboutForm";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DonateBitcoin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Paypal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label_Version;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label_Version_Value;
        private System.Windows.Forms.PictureBox pictureBox_Paypal;
        private System.Windows.Forms.PictureBox pictureBox_DonateBitcoin;
    }
}