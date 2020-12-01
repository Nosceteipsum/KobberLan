using KobberLan.Code;
using KobberLan.Gui;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using WK.Libraries.BetterFolderBrowserNS;

//-------------------------------------------------------------
namespace KobberLan
//-------------------------------------------------------------
{
    //-------------------------------------------------------------
    public partial class KobberLan : Form
    //-------------------------------------------------------------
    {
        private int countBroadcast;
        private Broadcast broadcast;
        private Communication communication;
        private List<SuggestedGame> suggestedGames;
        private SuggestInternetGame suggestForm;
        private ChooseNetworkInterface chooseNetworkInterface;

        //-------------------------------------------------------------
        public KobberLan()
        //-------------------------------------------------------------
        {
            suggestForm = null;
            broadcast = null;
            communication = null;
            countBroadcast = 2;
            suggestedGames = new List<SuggestedGame>();

            InitializeComponent();

            Torrent.Get().SetGuiReference(this);
            Log.Get().SetGuiReference(this);
        }

        //-------------------------------------------------------------
        public void AddSuggestedGame(DTO_Suggestion suggestion, string path = null, bool ownSuggestions = false, string remoteIP = "")
        //-------------------------------------------------------------
        {
            //Check if title exist
            if(!suggestedGames.Any(L => L.GetKey().Equals(suggestion.key) ))
            {
                //Add suggested game
                SuggestedGame suggestedGame;
                if(suggestion.type == SuggestionType.Internet)
                    suggestedGame = new SuggestedGameInternetControl(suggestion, this);
                else if (ownSuggestions)
                {
                    //Add game as owner
                    suggestedGame = new SuggestedGameOwnerControl(suggestion, path, this, ownSuggestions);
                }
                else
                {
                    suggestedGame = new SuggestedGameControl(suggestion, this);
                    if(Directory.Exists(Helper.GetDirection() + "\\" + suggestion.key))
                    {
                        //Already own the game, tell server about it
                        DTO_AlreadyOwnIt alreadyOwnIt = new DTO_AlreadyOwnIt() { address = Helper.GetHostIP(), key = suggestion.key };
                        communication.ClientSend(alreadyOwnIt, remoteIP);
                    }
                }
                    

                flowLayoutPanel_SuggestedGames.Controls.Add(suggestedGame);
                suggestedGames.Add(suggestedGame);
            }
            else 
            {
                //Warning, tried to add already exisiting game
                Log.Get().Write("Game " + suggestion.key + " already exist. Note this would happen if BroadCast is active, all player gets suggested game again.", Log.LogType.Info);
            }
        }

        //-------------------------------------------------------------
        public void SendLike(DTO_Like like)
        //-------------------------------------------------------------
        {
            //Send packet to LAN
            Log.Get().Write("Communication client prepare to send like");
            communication.ClientSend(like);

            //Update own GUI with like
            GotLike(like);
        }

        //-------------------------------------------------------------
        public void SendTorrentStatus(DTO_TorrentStatus torrentStatus,string IP)
        //-------------------------------------------------------------
        {
            Log.Get().Write("KobberLan DTO_TorrentStatus sending to host");

            //Send packet to LAN
            communication.ClientSend(torrentStatus, IP);

            //Update own GUI
            GotTorrentStatus(torrentStatus);
        }

        //-------------------------------------------------------------
        public void SendGameStatus(DTO_GameStatus gameStatus)
        //-------------------------------------------------------------
        {
            //Send packet to LAN
            Log.Get().Write("Communication client prepare to send GameStatus");
            communication.ClientSend(gameStatus);

            //Update own GUI with like
            GotGameStatus(gameStatus);
        }

        //-------------------------------------------------------------
        public void SendTorrent(DTO_Torrent torrent)
        //-------------------------------------------------------------
        {
            Log.Get().Write("KobberLan DTO_Torrent sending to network");
            //torrentTrackingServer = true; //Enable test port

            //Send packet to LAN
            communication.ClientSend(torrent);

            //Called from TorrentThread
            Invoke(new Action(() =>
            {
                //-------------------------------------------------------------
                //Update own SuggestedGame with torrent info
                //-------------------------------------------------------------
                GotTorrent(torrent);

            }));

        }

        //-------------------------------------------------------------
        public bool UpdateProgressBar(MonoTorrent.Client.TorrentState type, int progress,string title)
        //-------------------------------------------------------------
        {
            SuggestedGame suggestedGameControl = suggestedGames.Where(L => L.GetKey().Equals(title)).FirstOrDefault();
            if (suggestedGameControl == null)
            {
                if(type != MonoTorrent.Client.TorrentState.Stopped && type != MonoTorrent.Client.TorrentState.Stopping)
                {
                    Log.Get().Write("KobberLan UpdateProgressbar unknown title: " + title, Log.LogType.Error);
                }

                return false;
            }
            else
            {
                suggestedGameControl.UpdateProgressBar(type, progress);
                return true;
            }
        }

        //-------------------------------------------------------------
        public void GotGameStatus(DTO_GameStatus gameStatus)
        //-------------------------------------------------------------
        {
            SuggestedGame suggestedGameControl = suggestedGames.Where(L => L.GetKey().Equals(gameStatus.key)).FirstOrDefault();
            if(suggestedGameControl == null)
            {
                Log.Get().Write("KobberLan DTO_GameStatus unknown title: " + gameStatus.key, Log.LogType.Error);
            }
            else
            {
                suggestedGameControl.UpdateGameStatus(gameStatus);
            }
        }

        //-------------------------------------------------------------
        public void GotTorrentStatus(DTO_TorrentStatus torrentStatus)
        //-------------------------------------------------------------
        {
            SuggestedGame suggestedGameControl = suggestedGames.Where(L => L.GetKey().Equals(torrentStatus.key)).FirstOrDefault();
            if (suggestedGameControl == null)
            {
                Log.Get().Write("KobberLan DTO_TorrentStatus unknown title: " + torrentStatus.key, Log.LogType.Error);
            }
            else
            {
                //Remove game
                if(torrentStatus.status == TorrentStatusType.Remove)
                {
                    Torrent.Get().StopSharing(torrentStatus.key);
                    suggestedGameControl.Remove();
                    suggestedGameControl.Dispose();
                    suggestedGames.Remove(suggestedGameControl);
                    flowLayoutPanel_SuggestedGames.Controls.Remove(suggestedGameControl);
                }
                else //Update controller status
                {
                    suggestedGameControl.UpdateStats(torrentStatus);
                }
            }
        }

        //-------------------------------------------------------------
        public void GotTorrent(DTO_Torrent torrent)
        //-------------------------------------------------------------
        {
            SuggestedGame suggestedGameControl = suggestedGames.Where(L => L.GetKey().Equals(torrent.key)).FirstOrDefault();
            if (suggestedGameControl == null)
            {
                Log.Get().Write("KobberLan DTO_Torrent unknown title: " + torrent.key, Log.LogType.Error);
            }
            else
            {
                suggestedGameControl.UpdateTorrent(torrent);
            }
        }

        //-------------------------------------------------------------
        public void GotLike(DTO_Like like)
        //-------------------------------------------------------------
        {
            SuggestedGame suggestedGameControl = suggestedGames.Where(L => L.GetKey().Equals(like.key)).FirstOrDefault();
            if(suggestedGameControl == null)
            {
                Log.Get().Write("KobberLan DTO_Like unknown title: " + like.key, Log.LogType.Error);
            }
            else
            {
                suggestedGameControl.UpdateLike(like);
            }
        }

        //-------------------------------------------------------------
        public void GotAlreadyOwnIt(DTO_AlreadyOwnIt alreadyOwnIt)
        //-------------------------------------------------------------
        {
            SuggestedGame suggestedGameControl = suggestedGames.Where(L => L.GetKey().Equals(alreadyOwnIt.key) && L.GetType() == typeof(SuggestedGameOwnerControl)).FirstOrDefault();
            if(suggestedGameControl == null)
            {
                Log.Get().Write("KobberLan DTO_AlreadyOwnIt unknown title: " + alreadyOwnIt.key, Log.LogType.Error);
            }
            else
            {
                ((SuggestedGameOwnerControl)suggestedGameControl).IncreasePeer(alreadyOwnIt.address.ToString());
            }
        }

        //-------------------------------------------------------------
        public void UpdatePlayersAmount(SynchronizedCollection<IPAddress> players)
        //-------------------------------------------------------------
        {
            otherPlayersJoinedToolStripMenuItem.Text = "Other players joined: " + players.Count.ToString("D2");
            SetIconNumber(players.Count);
            communication.SetPlayers(players);
        }

        //-------------------------------------------------------------
        private void SetIconNumber(int players)
        //-------------------------------------------------------------
        {
            //Update taskbar icon
            if(players < 0)notifyIcon1.Icon = new System.Drawing.Icon(@"icons//mesh0.ico");
            else if (players > 0 && players <= 9) notifyIcon1.Icon = new System.Drawing.Icon(@"icons//mesh" + players + ".ico");
            else if (players > 9)notifyIcon1.Icon = new System.Drawing.Icon(@"icons//meshX.ico");

            //Update main icon
            this.Icon = notifyIcon1.Icon;
        }

        //-------------------------------------------------------------
        private void KobberLan_FormClosing(object sender, FormClosingEventArgs e)
        //-------------------------------------------------------------
        {
            //Minimize instead
            if(e.CloseReason == CloseReason.UserClosing)
            {
                Hide();
                e.Cancel = true;
            }
            else
            {
                //-------------------------------------------------------------
                //Close broadcast
                //-------------------------------------------------------------
                if (broadcast != null)
                {
                    broadcast.CloseAll();
                    broadcast = null;
                }

                //-------------------------------------------------------------
                //Close communication
                //-------------------------------------------------------------
                if (communication != null)
                {
                    communication.CloseAll();
                    communication = null;
                }

                //-------------------------------------------------------------
                //Remove icon
                //-------------------------------------------------------------
                notifyIcon1.Visible = false;            
                }
        }

        //-------------------------------------------------------------
        private void KobberLan_Load(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            //-------------------------------------------------------------
            //Check if computer has multiple network interfaces
            //-------------------------------------------------------------
            chooseNetworkInterface = new ChooseNetworkInterface();
            if(chooseNetworkInterface.GetActiveNetworkInterfaces().Count > 1)
            {
                //-------------------------------------------------------------
                //Only show dialog if more than 1 network interfaces
                //-------------------------------------------------------------
                chooseNetworkInterface.ShowDialog();
            }

            //-------------------------------------------------------------
            //Init default values
            //-------------------------------------------------------------
            notifyIcon1.Text = "KobberLan";
            notifyIcon1.Visible = true;
            broadcast = new Broadcast(this);
            communication = new Communication(this);

            //-------------------------------------------------------------
            // Start start icon (0 found)
            //-------------------------------------------------------------
            SetIconNumber(0);

            //-------------------------------------------------------------
            //Start server listening for broadcast
            //-------------------------------------------------------------
            broadcast.ServerStartListeningBroadCast();

            //-------------------------------------------------------------
            //Start server listening for Communication
            //-------------------------------------------------------------
            communication.ServerStartListening();

            //-------------------------------------------------------------
            //Show ports
            //-------------------------------------------------------------
            //label_PortBroadcast.Text = "Broadcast {" + Broadcast.BROADCAST_PORT +"} UDP";
            //label_CommPort.Text = "Comm {" + Communication.COMMUNICATION_PORT + "} TCP";
            //label_TorrentPort.Text = "Torrent {" + Torrent.TRACKERUDP_PORT + "} UDP";

            //-------------------------------------------------------------
            //Start client broadcast timer
            //-------------------------------------------------------------
            BroadCastTimer.Start();

            //-------------------------------------------------------------
            //Add default usercontrol with SuggestGame button
            //-------------------------------------------------------------
            OverviewControl overviewControl = new OverviewControl(this);
            flowLayoutPanel_SuggestedGames.Controls.Add(overviewControl);
        }

        //-------------------------------------------------------------
        private void BroadCastTimer_Tick(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            if(broadcast != null)
            {
                if(countBroadcast > 0)
                {
                    countBroadcast--;
                    broadcast.ClientSendBroadcast();

                    if(countBroadcast <= 0)
                    {
                        resendBroadcastToolStripMenuItem.Enabled = true;
                        BroadCastTimer.Enabled = false;
                    }
                }
            }
        }

        //-------------------------------------------------------------
        private void button_broadcast_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            broadcast.ClientSendBroadcast();
            countBroadcast = 1;
            resendBroadcastToolStripMenuItem.Enabled = false;
            BroadCastTimer.Enabled = true;
        }

        //-------------------------------------------------------------
        public void UpdatePlayerSuggested(string IP)
        //-------------------------------------------------------------
        {
            var suggestedGameControls = suggestedGames.Where(L => L.GetType() == typeof(SuggestedGameOwnerControl) ).ToList();
            foreach(SuggestedGame suggested in suggestedGameControls)
            {
                //Send packet to LAN
                Log.Get().Write("Communication client prepare to send suggested");
                communication.ClientSend(suggested.GetSuggestion(), IP);
            }
        }

        //-------------------------------------------------------------
        private void button_SuggestGame_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            var betterFolderBrowser = new BetterFolderBrowser();
            betterFolderBrowser.Title = "Select folder...";
            betterFolderBrowser.RootFolder = Helper.GetDirection();
            betterFolderBrowser.Multiselect = true;

            if (betterFolderBrowser.ShowDialog(this) == DialogResult.OK)
            {
                foreach(var folder in betterFolderBrowser.SelectedFolders)
                {
                    SuggestGameLoadFromPath(folder);
                }
            }
            else //User aborted Dialog box
            {
                return;
            }
        }

        //-------------------------------------------------------------
        public void SuggestGameLoadFromPath(string folder)
        //-------------------------------------------------------------
        {
            DTO_Suggestion game = new DTO_Suggestion()
            {
                type = Code.SuggestionType.HDD,
                title = "Unknown",
                author = Helper.GetHostIP().ToString(),
                imageCover = Properties.Resources.no_cover //Default image
            };

            //Set uniq key (foldername) and default title
            game.key = game.title = new DirectoryInfo(folder).Name;

            //Check for cover image
            string coverPath = folder + "\\_kobberlan.jpg";
            if (File.Exists(coverPath))
            {
                game.imageCover = Image.FromFile(coverPath);
            }

            //Load JSON settings file
            DTO_SuggestionSettings.LoadData(folder, ref game);

            //Send packet to LAN
            Log.Get().Write("Communication client prepare to send suggested");
            communication.ClientSend(game);

            //Update own GUI with suggested game
            AddSuggestedGame(game, folder, true);
        }

        //-------------------------------------------------------------
        private void button_SuggestInternetGame_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            //Show Internet form
            if(suggestForm == null || suggestForm.GetClosedStatus() == true)
            {
                suggestForm = new SuggestInternetGame(this);
                suggestForm.Show(this);
            } //Focus on Internet form
            else
            {
                suggestForm.BringToFront();
                suggestForm.Focus();
            }
        }

        //-------------------------------------------------------------
        public void SuggestInternetGame(SuggestInternetGame suggestForm)
        //-------------------------------------------------------------
        {
            DTO_Suggestion game = new DTO_Suggestion()
            {
                type = Code.SuggestionType.Internet,
                title = "Unknown",
                author = Helper.GetHostIP().ToString(),
                imageCover = Properties.Resources.no_cover //Default image
            };

            game.description = suggestForm.GetDescription();
            game.title = suggestForm.GetTitle(); //Key / title is the same
            game.key = suggestForm.GetTitle(); //Key / title is the same
            game.version = suggestForm.GetVersion();
            game.startGame = suggestForm.GetStartLocation();
            game.maxPlayers = suggestForm.GetMaxPlayers();

            //Get images
            var imageBigUrl = suggestForm.GetImageBig();
            var imageCoverUrl = suggestForm.GetImageCover();

            //Get images from Internet
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imageCoverUrl);
            game.imageCover = Bitmap.FromStream(stream);
            stream.Close();
            stream = client.OpenRead(imageBigUrl);
            game.imageBig = Bitmap.FromStream(stream);
            stream.Close();
            client.Dispose();

            //Send suggestion to LAN
            Log.Get().Write("Communication client prepare to send Internet suggested");
            communication.ClientSend(game);

            //Update own GUI with suggested game
            AddSuggestedGame(game);

            //Destroy InternetForm
            suggestForm.Dispose();
            suggestForm = null;
        }

        //-------------------------------------------------------------
        private void KobberLan_Resize(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        //-------------------------------------------------------------
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        //-------------------------------------------------------------
        {
            if(e.Button == MouseButtons.Left)
            {
                Show();
                this.WindowState = FormWindowState.Normal;
            }
        }

        //-------------------------------------------------------------
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            Application.Exit();
        }

        /*
        //-------------------------------------------------------------
        private void CheckPortStatus()
        //-------------------------------------------------------------
        {
            
            //-------------------------------------------------------------
            //Set to grey
            //-------------------------------------------------------------
            pictureBox_PortCommunication.Image = Properties.Resources.OpenClosedDot_grey;
            pictureBox_PortBroadcast.Image = Properties.Resources.OpenClosedDot_grey;
            pictureBox_PortTorrent.Image = Properties.Resources.OpenClosedDot_grey;
            pictureBox_PortCommunication.Refresh();
            pictureBox_PortBroadcast.Refresh();
            pictureBox_PortTorrent.Refresh();

            //-------------------------------------------------------------
            //Check Communication port
            //-------------------------------------------------------------
            if (Helper.IsTCPPortWorking(Communication.COMMUNICATION_PORT)) 
            {
                pictureBox_PortCommunication.Image = Properties.Resources.OpenClosedDot_green;
            }
            else
            {
                pictureBox_PortCommunication.Image = Properties.Resources.OpenClosedDot_red;
                button_PortOpen.Enabled = true;
            }

            //-------------------------------------------------------------
            //Check Broadcast port
            //-------------------------------------------------------------
            if (Helper.IsUDPPortWorking(Broadcast.BROADCAST_PORT, false))
            {
                pictureBox_PortBroadcast.Image = Properties.Resources.OpenClosedDot_green;
            }
            else
            {
                pictureBox_PortBroadcast.Image = Properties.Resources.OpenClosedDot_red;
                button_PortOpen.Enabled = true;
            }

            //-------------------------------------------------------------
            //Check Tracking port (Only check if sharing/tracking torrents)
            //-------------------------------------------------------------
            if (torrentTrackingServer)
            {
                if (Helper.IsUDPPortWorking(Torrent.TRACKERUDP_PORT, true))
                {
                    pictureBox_PortTorrent.Image = Properties.Resources.OpenClosedDot_green;
                }
                else
                {
                    pictureBox_PortTorrent.Image = Properties.Resources.OpenClosedDot_red;
                    button_PortOpen.Enabled = true;
                }
            }
            
        }

        //-------------------------------------------------------------
        private void button_portcheck_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            CheckPortStatus();
        }

        //-------------------------------------------------------------
        private void button_PortOpen_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            button_PortOpen.Enabled = false;

            //Helper.AddPortExceptionToWindowsFireWall(Communication.COMMUNICATION_PORT);
            Helper.AddApplicationExceptionToWindowsFireWall();
        }
        */

        //-------------------------------------------------------------
        private void button_inspiration_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            //Read inspiration url from ApplicationConfig
            string inspirationUrl = ConfigurationManager.AppSettings.Get("Kobberlan:InspirationUrl");
            ProcessStartInfo sInfo = new ProcessStartInfo(inspirationUrl);
            Process.Start(sInfo);
        }

        //-------------------------------------------------------------
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            //Show About form
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        //-------------------------------------------------------------
        public void SetErrors(int errors)
        //-------------------------------------------------------------
        {
            logs00ToolStripMenuItem.BackColor = Color.Red;
            errors00ToolStripMenuItem.Text = "Errors: " + errors.ToString("D2");
        }

        //-------------------------------------------------------------
        public void SetWarnings(int warnings)
        //-------------------------------------------------------------
        {
            if(logs00ToolStripMenuItem.BackColor != Color.Red)
                logs00ToolStripMenuItem.BackColor = Color.LightYellow;

            warnings00ToolStripMenuItem.Text = "Warnings: " + warnings.ToString("D2");
        }

        //-------------------------------------------------------------
        private void errorsToolStripMenuItem_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            if(Log.Get().Errors() > 0)
            {
                string path = "file:///" + Application.StartupPath + "/Logs/error.html";
                path = path.Replace(@"\", @"/");
                try
                {
                    ProcessStartInfo sInfo = new ProcessStartInfo(path);
                    Process.Start(sInfo);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Could not open log file: " + path + Environment.NewLine + ex, "Error");
                }
            }
        }

        //-------------------------------------------------------------
        private void warningsToolStripMenuItem_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            if (Log.Get().Warnings() > 0)
            {
                string path = "file:///" + Application.StartupPath + "/Logs/warning.html";
                path = path.Replace(@"\", @"/");
                try
                {
                    ProcessStartInfo sInfo = new ProcessStartInfo(path);
                    Process.Start(sInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not open log file: " + path + Environment.NewLine + ex, "Error");
                }
            }
        }

        //-------------------------------------------------------------
        public void ShowBallonTip(string title,string description,int time = 10000)
        //-------------------------------------------------------------
        {
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(time, title, description, ToolTipIcon.Info);

        }

        //-------------------------------------------------------------
        private void errors00ToolStripMenuItem_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            errorsToolStripMenuItem_Click(sender, e);
        }

        //-------------------------------------------------------------
        private void networkInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            chooseNetworkInterface.ShowDialog();
        }
    }
}
