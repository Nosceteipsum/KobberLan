using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using KobberLan.Code;
using MonoTorrent;
using KobberLan.Gui;
using Torrent = KobberLan.Code.Torrent;
using MonoTorrent.Client;
using System.IO;
using System.Diagnostics;

namespace KobberLan
{
    //-------------------------------------------------------------
    public partial class SuggestedGameControl : SuggestedGame
    //-------------------------------------------------------------
    {
        private TorrentState state;
        private bool finishedDownloaded;
        private int ingame;

        //-------------------------------------------------------------
        public SuggestedGameControl(DTO_Suggestion dto, KobberLan parent)
        //-------------------------------------------------------------
        {
            //Init control 
            InitializeComponent();

            finishedDownloaded = false;
            kobberLan = parent;
            dto_suggestion = dto;
            Likes = new List<IPAddress>();
            label_GameTitle.Text = dto.title;
            pictureBox_Cover.Image = dto.imageCover;
            state = TorrentState.Starting;

            if (dto.torrent == null)
            {
                button_Get.Enabled = false;
                button_Get.Visible = false;
            }
            else
            {
                button_Get.Enabled = true;
                button_Get.Visible = true;
            }

            //Set mouseover tooltip
            CustomToolTip tip = new CustomToolTip(dto, this);
            tip.InitImage(dto.imageBig);
            tip.SetToolTip(pictureBox_Cover, "Details");

            //-------------------------------------------------------------
            //Check for startup value
            //-------------------------------------------------------------
            if (!(string.IsNullOrEmpty(dto.startGame) && string.IsNullOrEmpty(dto.startServer)))
            {
                button_Play.Visible = true;
                button_Play.Enabled = false;

                //-------------------------------------------------------------
                //Check if player already got the game
                //-------------------------------------------------------------
                var path = Helper.GetDirection();
                if(Directory.Exists(path + "\\" + dto_suggestion.key))
                {
                    button_Play.Enabled = true;
                }

            }

            //Show popup notification about the new game
            kobberLan.Invoke(new Action(() =>
            {
                kobberLan.ShowBallonTip("Kobberlan game suggested", "Game: " + dto_suggestion.title);
            }));

        }

        //-------------------------------------------------------------
        public override string GetTitle()
        //-------------------------------------------------------------
        {
            return label_GameTitle.Text;
        }

        //-------------------------------------------------------------
        public override void UpdateProgressBar(TorrentState type, int progress)
        //-------------------------------------------------------------
        {
            if(type == TorrentState.Stopped ||
               type == TorrentState.Stopping)
            {
                progressBar.Visible = false;
                label_ProgressBar.Visible = false;
                button_Play.Enabled = true;
                Log.Get().Write("Torrent: " + dto_suggestion.key + " state: " + type.ToString());
            }
            else if(type == TorrentState.Seeding)
            {
                progressBar.Visible = false;
                label_ProgressBar.Visible = false;

                //-------------------------------------------------------------
                //Note host about finished download
                //-------------------------------------------------------------
                if (finishedDownloaded == false)
                {
                    finishedDownloaded = true;
                    DTO_TorrentStatus torrentStatus = new DTO_TorrentStatus() { key = dto_suggestion.key, address = Helper.GetHostIP(), status = TorrentStatusType.Finished };
                    kobberLan.SendTorrentStatus(torrentStatus, dto_suggestion.author);
                }

                //-------------------------------------------------------------
                //Enable stop button
                //-------------------------------------------------------------
                if(button_Play.Enabled == false) //Ignore seeding message, when stop has been called
                {
                    button_Get.Enabled = true;
                    button_Get.Text = "Stop";
                }
            }
            else if(type == TorrentState.Error)
            {
                progressBar.Visible = false;
                label_ProgressBar.Visible = true;
                label_ProgressBar.Text = "Torrent error";
                label_ProgressBar.Refresh();

                Log.Get().Write("Game " + GetTitle() + " got unknown error.", Log.LogType.Error);
                Torrent.Get().StopSharing(dto_suggestion.key);
            }
            else //Download
            {
                progressBar.Enabled = true;
                progressBar.Visible = true;
                label_ProgressBar.Visible = true;

                if(type == TorrentState.Hashing && state == TorrentState.Downloading) //Ignore hash check for every files progress (Make progress bar jump strangely)
                {
                    return;
                }
                else if (type == TorrentState.Hashing)
                {
                    state = TorrentState.Hashing;
                    label_ProgressBar.Text = "Hashing: " + progress.ToString("D2") + "%";
                }
                else if(type == TorrentState.Downloading && progress > 0)
                {
                    state = TorrentState.Downloading;
                    label_ProgressBar.Text = "Downloading: " + progress.ToString("D2") + "%";
                }
                else
                {
                    label_ProgressBar.Text = type.ToString();
                }

                progressBar.Value = progress;
                label_ProgressBar.Refresh();
            }
        }

        //-------------------------------------------------------------
        public override void UpdateStats(DTO_TorrentStatus torrentStatus) { } //Ignore
        //-------------------------------------------------------------

        //-------------------------------------------------------------
        public override void UpdateLike(DTO_Like like)
        //-------------------------------------------------------------
        {
            if(!Likes.Contains(like.address))
            {
                Likes.Add(like.address);
                label_likes.Text = Likes.Count.ToString("D2");
            }
        }

        //-------------------------------------------------------------
        public override void UpdateTorrent(DTO_Torrent torrent)
        //-------------------------------------------------------------
        {
            button_Get.Visible = true;
            button_Get.Enabled = true;
            dto_suggestion.torrent = torrent.torrent;
        }

        //-------------------------------------------------------------
        private void button_ShareGet_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            if(dto_suggestion.torrent == null)
            {
                Log.Get().Write("Torrent is null, aborting Get operation.", Log.LogType.Error);
                return;
            }

            if(button_Get.Text.Equals("Stop"))
            {
                //-------------------------------------------------------------
                //Stop seeding
                //-------------------------------------------------------------
                Torrent.Get().StopSharing(dto_suggestion.key);
            }
            else
            {
                //-------------------------------------------------------------
                //Note host about starting to download
                //-------------------------------------------------------------
                DTO_TorrentStatus torrentStatus = new DTO_TorrentStatus() { key = dto_suggestion.key, address = Helper.GetHostIP(), status = TorrentStatusType.Starting };
                kobberLan.SendTorrentStatus(torrentStatus, dto_suggestion.author);

                //-------------------------------------------------------------
                //Download torrent
                //-------------------------------------------------------------
                Torrent.Get().DownloadTorrent(dto_suggestion.torrent);
            }

            //-------------------------------------------------------------
            //Disable get/stop button
            //-------------------------------------------------------------
            button_Get.Enabled = false;

        }

        //-------------------------------------------------------------
        private void button_like_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            button_like.Enabled = false;

            DTO_Like like = new DTO_Like() { address = Helper.GetHostIP(), key = GetKey() };
            kobberLan.SendLike(like); // Parent.Parent.Parent => Flow.Groupbox.Form
        }

        //-------------------------------------------------------------
        public override void UpdateGameStatus(DTO_GameStatus gameStatus)
        //-------------------------------------------------------------
        {
            //Update amount
            if(gameStatus.playing == true)
            {
                ingame++;
            }
            else
            {
                ingame--;
            }

            //Update amount of players in label
            label_Ingame.Text = ingame.ToString("D2");

            //Show/hide ingame info
            if(ingame > 0)
            {
                panel_Ingame.Visible = true;
                pictureBox_Ingame.Visible = true;
                label_Ingame.Visible = true;
            }
            else
            {
                panel_Ingame.Visible = false;
                pictureBox_Ingame.Visible = false;
                label_Ingame.Visible = false;
            }
        }

        //-------------------------------------------------------------
        private void button_Play_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            var path = Helper.GetDirection();
            ExecuteFile(dto_suggestion.startGame, path + "\\" + dto_suggestion.key, dto_suggestion.startGameParams, dto_suggestion.key);
        }

        //-------------------------------------------------------------
        private void label_GameTitle_Paint(object sender, PaintEventArgs e)
        //-------------------------------------------------------------
        {
            Font font = FontAdjusted(e.Graphics, this.Bounds.Size, label_GameTitle.Font, label_GameTitle.Text);
            if (!NearlyEqual(font.Size, label_GameTitle.Font.Size, 0.01))
            {
                label_GameTitle.Font = font;
            }
        }

        //-------------------------------------------------------------
        public override void Remove()
        //-------------------------------------------------------------
        {
            dto_suggestion.imageBig.Dispose();
            dto_suggestion.imageCover.Dispose();
            pictureBox_Cover.Image.Dispose();
        }


    }
}
