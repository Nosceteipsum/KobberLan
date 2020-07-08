using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using KobberLan.Code;
using KobberLan.Gui;
using MonoTorrent.Client;

namespace KobberLan
{
    //-------------------------------------------------------------
    public partial class SuggestedGameInternetControl : SuggestedGame
    //-------------------------------------------------------------
    {
        private KobberLan kobberLan;

        //-------------------------------------------------------------
        public SuggestedGameInternetControl(DTO_Suggestion dto, KobberLan parent)
        //-------------------------------------------------------------
        {
            //Init control 
            InitializeComponent();

            kobberLan = parent;
            dto_suggestion = dto;
            Likes = new List<IPAddress>();
            label_GameTitle.Text = dto.title;
            pictureBox_Cover.Image = dto.imageCover;

            //Set mouseover tooltip
            CustomToolTip tip = new CustomToolTip(dto, this);
            tip.InitImage(dto.imageBig);
            tip.SetToolTip(pictureBox_Cover, "Details");

            //Check for startup value
            if (!(string.IsNullOrEmpty(dto.startGame) && string.IsNullOrEmpty(dto.startServer)))
            {
                button_Play.Enabled = true;
            }

            //Show popup notification about the new game
            kobberLan.Invoke(new Action(() =>
            {
                kobberLan.ShowBallonTip("Kobberlan internet suggested", "Game: " + dto_suggestion.title);
            }));

        }

        //-------------------------------------------------------------
        public override string GetTitle()
        //-------------------------------------------------------------
        {
            return label_GameTitle.Text;
        }

        //-------------------------------------------------------------
        public override void UpdateStats(DTO_TorrentStatus torrentStatus) { }
        //-------------------------------------------------------------

        //-------------------------------------------------------------
        public override void UpdateProgressBar(TorrentState type,int progress) { }
        //-------------------------------------------------------------

        //-------------------------------------------------------------
        public override void UpdateLike(DTO_Like like)
        //-------------------------------------------------------------
        {
            if (!Likes.Contains(like.address))
            {
                Likes.Add(like.address);
                label_likes.Text = Likes.Count.ToString("D2");
            }
        }

        //-------------------------------------------------------------
        public override void UpdateTorrent(DTO_Torrent torrent) { }
        //-------------------------------------------------------------

        //-------------------------------------------------------------
        private void button_like_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            button_like.Enabled = false;

            DTO_Like like = new DTO_Like() { address = Helper.getHostIP(), key = GetKey() };
            kobberLan.SendLike(like); // Parent.Parent.Parent => Flow.Groupbox.Form
        }

        //-------------------------------------------------------------
        private void button_Play_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            //Go to suggested URL
            ProcessStartInfo sInfo = new ProcessStartInfo(dto_suggestion.startGame);
            Process.Start(sInfo);
        }

        //-------------------------------------------------------------
        private void label_GameTitle_Paint(object sender, PaintEventArgs e)
        //-------------------------------------------------------------
        {
            Font font = FontAdjusted(e.Graphics, this.Bounds.Size, label_GameTitle.Font, label_GameTitle.Text);
            if(!NearlyEqual(font.Size, label_GameTitle.Font.Size,0.01))
            {
                label_GameTitle.Font = font;
            }
        }
    }


}
