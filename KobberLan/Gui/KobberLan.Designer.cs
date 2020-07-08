namespace KobberLan
{
    partial class KobberLan
    {

        private System.ComponentModel.IContainer components = null;


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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KobberLan));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.BroadCastTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel_SuggestedGames = new System.Windows.Forms.FlowLayoutPanel();
            this.logs00ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errors00ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.warnings00ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.suggestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.suggestGameFromHDDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.suggestGameFromInternetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findInspirationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.broadcastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherPlayersJoinedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resendBroadcastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // BroadCastTimer
            // 
            this.BroadCastTimer.Interval = 5000;
            this.BroadCastTimer.Tick += new System.EventHandler(this.BroadCastTimer_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.suggestToolStripMenuItem,
            this.broadcastToolStripMenuItem,
            this.logs00ToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(606, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // flowLayoutPanel_SuggestedGames
            // 
            this.flowLayoutPanel_SuggestedGames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel_SuggestedGames.AutoScroll = true;
            this.flowLayoutPanel_SuggestedGames.Location = new System.Drawing.Point(0, 27);
            this.flowLayoutPanel_SuggestedGames.Name = "flowLayoutPanel_SuggestedGames";
            this.flowLayoutPanel_SuggestedGames.Size = new System.Drawing.Size(606, 637);
            this.flowLayoutPanel_SuggestedGames.TabIndex = 0;
            // 
            // logs00ToolStripMenuItem
            // 
            this.logs00ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.errors00ToolStripMenuItem,
            this.warnings00ToolStripMenuItem});
            this.logs00ToolStripMenuItem.Name = "logs00ToolStripMenuItem";
            this.logs00ToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.logs00ToolStripMenuItem.Text = "Logs";
            // 
            // errors00ToolStripMenuItem
            // 
            this.errors00ToolStripMenuItem.Name = "errors00ToolStripMenuItem";
            this.errors00ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.errors00ToolStripMenuItem.Text = "Errors: 00";
            this.errors00ToolStripMenuItem.Click += new System.EventHandler(this.errorsToolStripMenuItem_Click);
            // 
            // warnings00ToolStripMenuItem
            // 
            this.warnings00ToolStripMenuItem.Name = "warnings00ToolStripMenuItem";
            this.warnings00ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.warnings00ToolStripMenuItem.Text = "Warnings: 00";
            this.warnings00ToolStripMenuItem.Click += new System.EventHandler(this.warningsToolStripMenuItem_Click);
            // 
            // suggestToolStripMenuItem
            // 
            this.suggestToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.suggestGameFromHDDToolStripMenuItem,
            this.suggestGameFromInternetToolStripMenuItem,
            this.findInspirationToolStripMenuItem});
            this.suggestToolStripMenuItem.Name = "suggestToolStripMenuItem";
            this.suggestToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.suggestToolStripMenuItem.Text = "Suggest";
            // 
            // suggestGameFromHDDToolStripMenuItem
            // 
            this.suggestGameFromHDDToolStripMenuItem.Name = "suggestGameFromHDDToolStripMenuItem";
            this.suggestGameFromHDDToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.suggestGameFromHDDToolStripMenuItem.Text = "Suggest game from HDD";
            this.suggestGameFromHDDToolStripMenuItem.Click += new System.EventHandler(this.button_SuggestGame_Click);
            // 
            // suggestGameFromInternetToolStripMenuItem
            // 
            this.suggestGameFromInternetToolStripMenuItem.Name = "suggestGameFromInternetToolStripMenuItem";
            this.suggestGameFromInternetToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.suggestGameFromInternetToolStripMenuItem.Text = "Suggest game from Internet";
            this.suggestGameFromInternetToolStripMenuItem.Click += new System.EventHandler(this.button_SuggestInternetGame_Click);
            // 
            // findInspirationToolStripMenuItem
            // 
            this.findInspirationToolStripMenuItem.Name = "findInspirationToolStripMenuItem";
            this.findInspirationToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.findInspirationToolStripMenuItem.Text = "Find inspiration";
            this.findInspirationToolStripMenuItem.Click += new System.EventHandler(this.button_inspiration_Click);
            // 
            // broadcastToolStripMenuItem
            // 
            this.broadcastToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.otherPlayersJoinedToolStripMenuItem,
            this.resendBroadcastToolStripMenuItem});
            this.broadcastToolStripMenuItem.Name = "broadcastToolStripMenuItem";
            this.broadcastToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.broadcastToolStripMenuItem.Text = "Broadcast";
            // 
            // otherPlayersJoinedToolStripMenuItem
            // 
            this.otherPlayersJoinedToolStripMenuItem.Name = "otherPlayersJoinedToolStripMenuItem";
            this.otherPlayersJoinedToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.otherPlayersJoinedToolStripMenuItem.Text = "Other players joined: 00";
            // 
            // resendBroadcastToolStripMenuItem
            // 
            this.resendBroadcastToolStripMenuItem.Enabled = false;
            this.resendBroadcastToolStripMenuItem.Name = "resendBroadcastToolStripMenuItem";
            this.resendBroadcastToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.resendBroadcastToolStripMenuItem.Text = "Resend broadcast";
            this.resendBroadcastToolStripMenuItem.Click += new System.EventHandler(this.button_broadcast_Click);
            // 
            // KobberLan
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(606, 662);
            this.Controls.Add(this.flowLayoutPanel_SuggestedGames);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "KobberLan";
            this.Text = "KobberLan - Lan Party Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KobberLan_FormClosing);
            this.Load += new System.EventHandler(this.KobberLan_Load);
            this.Resize += new System.EventHandler(this.KobberLan_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Timer BroadCastTimer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_SuggestedGames;
        private System.Windows.Forms.ToolStripMenuItem suggestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem suggestGameFromHDDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem suggestGameFromInternetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findInspirationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logs00ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem errors00ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem warnings00ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem broadcastToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otherPlayersJoinedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resendBroadcastToolStripMenuItem;
    }
}

