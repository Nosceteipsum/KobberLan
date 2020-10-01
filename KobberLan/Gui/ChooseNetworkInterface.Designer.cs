namespace KobberLan.Gui
{
    partial class ChooseNetworkInterface
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseNetworkInterface));
            this.listBox_Interfaces = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ActiveInterface = new System.Windows.Forms.TextBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox_Interfaces
            // 
            this.listBox_Interfaces.FormattingEnabled = true;
            this.listBox_Interfaces.Location = new System.Drawing.Point(15, 25);
            this.listBox_Interfaces.Name = "listBox_Interfaces";
            this.listBox_Interfaces.Size = new System.Drawing.Size(623, 199);
            this.listBox_Interfaces.TabIndex = 0;
            this.listBox_Interfaces.SelectedIndexChanged += new System.EventHandler(this.listBox_Interfaces_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Network interfaces:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 239);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Active interface:";
            // 
            // textBox_ActiveInterface
            // 
            this.textBox_ActiveInterface.Location = new System.Drawing.Point(15, 255);
            this.textBox_ActiveInterface.Name = "textBox_ActiveInterface";
            this.textBox_ActiveInterface.ReadOnly = true;
            this.textBox_ActiveInterface.Size = new System.Drawing.Size(623, 20);
            this.textBox_ActiveInterface.TabIndex = 3;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(563, 290);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 4;
            this.button_ok.Text = "Ok";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // ChooseNetworkInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 325);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.textBox_ActiveInterface);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox_Interfaces);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChooseNetworkInterface";
            this.Text = "KobberLan - ChooseNetworkInterface";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Interfaces;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ActiveInterface;
        private System.Windows.Forms.Button button_ok;
    }
}