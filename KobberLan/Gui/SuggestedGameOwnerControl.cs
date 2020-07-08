using System;
using System.Collections.Generic;
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
    public partial class SuggestedGameOwnerControl : SuggestedGame
    //-------------------------------------------------------------
    {
        private string path;
        private KobberLan kobberLan;

        private TorrentState state;
        private int metaProgress;

        private int torrentPeers;
        private int torrentDownloadCompleted;

        //-------------------------------------------------------------
        public SuggestedGameOwnerControl(DTO_Suggestion dto, string pathHDD, KobberLan parent)
        //-------------------------------------------------------------
        {
            //Init control 
            InitializeComponent();

            torrentPeers = 0;
            torrentDownloadCompleted = 0;

            metaProgress = 0;
            state = TorrentState.Starting;
            kobberLan = parent;
            path = pathHDD;
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
        }

        //-------------------------------------------------------------
        public override string GetTitle()
        //-------------------------------------------------------------
        {
            return label_GameTitle.Text;
        }

        //-------------------------------------------------------------
        public override void UpdateStats(DTO_TorrentStatus torrentStatus)
        //-------------------------------------------------------------
        {
            if (torrentStatus.status == TorrentStatusType.Finished)
                torrentDownloadCompleted++;
            if (torrentStatus.status == TorrentStatusType.Starting)
                torrentPeers++;

            label_Downloading.Text = torrentDownloadCompleted.ToString("D2");
            label_Peers.Text = torrentPeers.ToString("D2");
        }

        //-------------------------------------------------------------
        public override void UpdateProgressBar(TorrentState type,int progress)
        //-------------------------------------------------------------
        {
            if (type == TorrentState.Stopped)
            {
                progressBar.Visible = false;
                label_ProgresBar.Visible = false;
            }
            //Hide when starting to seed
            else if (type == TorrentState.Seeding)
            {
                //Hide progress info
                progressBar.Enabled = false;
                progressBar.Visible = false;
                label_ProgresBar.Visible = false;

                //Stop sharing enabled
                button_ShareGet.Enabled = true;
            }
            else if(type == TorrentState.Metadata)
            {
                //Creating torrent file
                label_ProgresBar.Text = "Creating torrent data";
                progressBar.Visible = true;
                label_ProgresBar.Visible = true;

                //Prevent bar totalcompleted jumping around
                if (metaProgress < progress)
                {
                    metaProgress = progress; 
                }
                progressBar.Value = metaProgress;
            }
            else if (state == type)
            {
                progressBar.Value = progress;
                progressBar.Visible = true;
                label_ProgresBar.Visible = true;
                label_ProgresBar.Text = type.ToString() + " " + progress.ToString("D2") + "%";
                label_ProgresBar.Refresh();
            }
            else if (type == TorrentState.Downloading)
            {
                progressBar.Visible = false;
                label_ProgresBar.Visible = true;
                label_ProgresBar.Text = "Incomplete, please delete .torrent file";
                label_ProgresBar.Refresh();
                Log.Get().Write("Game " + GetTitle() + " is incomplete, please delete _kobberlan.torrent file to recreate torrent again.", Log.LogType.Warning);

                button_ShareGet.Enabled = false;
                Torrent.Get().StopSharing(dto_suggestion.key);
                button_Clear.Enabled = true;
            }
            else if (type == TorrentState.Error)
            {
                progressBar.Visible = false;
                label_ProgresBar.Visible = true;
                label_ProgresBar.Text = "Torrent error";
                label_ProgresBar.Refresh();

                Log.Get().Write("Game " + GetTitle() + " could not be shared, unknown error.", Log.LogType.Error);
                Torrent.Get().StopSharing(dto_suggestion.key);
            }
            else
            {
                progressBar.Value = 0;
                state = type;
            }

        }

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
        public override void UpdateTorrent(DTO_Torrent torrent)
        //-------------------------------------------------------------
        {
            //-------------------------------------------------------------
            //Change layout
            //-------------------------------------------------------------
            label_Peers.Visible = true;
            label_Downloading.Visible = true;
            pictureBox_Peers.Visible = true;
            pictureBox_Downloaded.Visible = true;

            //-------------------------------------------------------------
            //Update torrent info (To send to new clients)
            //-------------------------------------------------------------
            dto_suggestion.torrent = torrent.torrent;
        }

        //-------------------------------------------------------------
        private void button_ShareGet_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            if(button_ShareGet.Text.Equals("Share"))
            {
                Log.Get().Write("Start sharing: " + dto_suggestion.key);
                button_ShareGet.Enabled = false;
                button_Clear.Enabled = false;
                button_ShareGet.Text = "Stop";

                //-------------------------------------------------------------
                //Run in own thread (To avoid locking gui)
                //-------------------------------------------------------------
                new Thread(() =>
                {
                    DTO_Torrent torrent = new DTO_Torrent() { key = dto_suggestion.key }; 

                    //-------------------------------------------------------------
                    //Start creating torrent (hashing)
                    //-------------------------------------------------------------
                    torrent.torrent = Torrent.Get().CreateTorrent(path, dto_suggestion.key).Result;

                    //-------------------------------------------------------------
                    //Check torrent name with key (Stop if mismatch)
                    //-------------------------------------------------------------
                    string torrentKey = Torrent.Get().GetTorrentName(torrent.torrent);
                    if (!torrentKey.Equals(torrent.key))
                    {
                        Log.Get().Write("Torrent name not maching key, trying to delete file. torrentKey:'" + torrentKey + "' suggestkey:'" + torrent.key + "'", Log.LogType.Warning);

                        //-------------------------------------------------------------
                        //Delete torrent file
                        //-------------------------------------------------------------
                        Torrent.Get().DeleteTorrentFile(path);

                        //-------------------------------------------------------------
                        //ReCreate torrent file
                        //-------------------------------------------------------------
                        torrent.torrent = Torrent.Get().CreateTorrent(path, dto_suggestion.key).Result;

                        if (!Torrent.Get().GetTorrentName(torrent.torrent).Equals(torrent.key))
                        {
                            Log.Get().Write("Torrentfile name not maching key, aborting", Log.LogType.Error);
                            return;
                        }
                    }

                    //-------------------------------------------------------------
                    //Insert announce ip
                    //-------------------------------------------------------------
                    torrent.torrent = Torrent.Get().InsertAnnounce(torrent.torrent);

                    //-------------------------------------------------------------
                    //Share the torrent with other
                    //-------------------------------------------------------------
                    kobberLan.SendTorrent(torrent);

                    //-------------------------------------------------------------
                    //Start tracker
                    //-------------------------------------------------------------
                    Torrent.Get().StartTracker(torrent.torrent);

                }).Start();
            }
            else if(button_ShareGet.Text.Equals("Stop"))
            {
                Log.Get().Write("Stop sharing: " + dto_suggestion.key);

                button_ShareGet.Enabled = false;
                Torrent.Get().StopSharing(dto_suggestion.key);

                button_ShareGet.Enabled = true;
                button_ShareGet.Text = "Share"; //Possible to start sharing again
            }

        }

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
            if(string.IsNullOrEmpty(dto_suggestion.startServer))
            {
                var path = Helper.GetDirection();
                ExecuteFile(dto_suggestion.startGame, path + "\\" + dto_suggestion.key, dto_suggestion.startGameParams);
            }
            else
            {
                //-------------------------------------------------------------
                //Let user choose between server/client
                //-------------------------------------------------------------
                DialogResult result = MessageBox.Show("Run as client? [Yes] Or server [No]","Server or client",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    var path = Helper.GetDirection();
                    ExecuteFile(dto_suggestion.startGame, path + "\\" + dto_suggestion.key, dto_suggestion.startGameParams);
                }
                else if (result == DialogResult.No)
                {
                    var path = Helper.GetDirection();
                    ExecuteFile(dto_suggestion.startServer, path + "\\Games\\" + dto_suggestion.key, dto_suggestion.startServerParams);
                }
                else
                {
                    //Ignore
                }

            }
        }

        //-------------------------------------------------------------
        private void button_Clear_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            DTO_TorrentStatus torrentStatus = new DTO_TorrentStatus() { key = dto_suggestion.key, address = Helper.getHostIP(), status = TorrentStatusType.Remove };
            kobberLan.SendTorrentStatus(torrentStatus, ""); //Empty IP, broadcast to all connected clients
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

    }


}
