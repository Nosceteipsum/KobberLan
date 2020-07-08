using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonoTorrent;
using MonoTorrent.BEncoding;
using MonoTorrent.Client;
using MonoTorrent.Tracker;
using MonoTorrent.Tracker.Listeners;

namespace KobberLan.Code
{
    //-------------------------------------------------------------
    // Singleton pattern
    public class Torrent
    //-------------------------------------------------------------
    {
        public static int TRACKERHTTP_PORT;
        public static int TRACKERUDP_PORT;
        public static int CLIENT_PORT;

        private static Torrent torrent;
        private TrackerServer trackerServer;
        private ITrackerListener listenerHttp;
        private ITrackerListener listenerUdp;

        private ClientEngine engine;
        private EngineSettings settings;
        private List<TorrentManager> torrentManagers;

        private KobberLan kobberLan;

        //-------------------------------------------------------------
        private Torrent()
        //-------------------------------------------------------------
        {
            TRACKERHTTP_PORT = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Port:TorrentTrackerHTTP"));
            TRACKERUDP_PORT  = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Port:TorrentTrackerUDP"));
            CLIENT_PORT      = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Port:TorrentClient"));

            torrentManagers = new List<TorrentManager>();
            trackerServer = null;
            engine = null;
            settings = null;
        }

        //-------------------------------------------------------------
        public static Torrent Get()
        //-------------------------------------------------------------
        {
            if (torrent == null)
            {
                torrent = new Torrent();
            }

            return torrent;
        }

        //-------------------------------------------------------------
        public void SetGuiReference(KobberLan kobberLanRef)
        //-------------------------------------------------------------
        {
            kobberLan = kobberLanRef;
        }

        //-------------------------------------------------------------
        public void DeleteTorrentFile(string path)
        //-------------------------------------------------------------
        {
            string pathTorrent = path + "\\_kobberlan.torrent";

            //-------------------------------------------------------------
            //Check if torrent file exist
            //-------------------------------------------------------------
            if (File.Exists(pathTorrent))
            {
                try
                {
                    File.Delete(pathTorrent);
                }
                catch(Exception ex)
                {
                    Log.Get().Write("Could not delete torrent file: " + pathTorrent + " error: " + ex.Message, Log.LogType.Error);
                }
            }
            else
            {
                Log.Get().Write("File not found for deleting: " + pathTorrent, Log.LogType.Warning);
            }
        }

        //-------------------------------------------------------------
        public async Task<byte[]> CreateTorrent(string path, string title)
        //-------------------------------------------------------------
        {
            byte[] result = null;
            string pathTorrent = path + "\\_kobberlan.torrent";

            //-------------------------------------------------------------
            //Check if torrent file exist
            //-------------------------------------------------------------
            if (File.Exists(pathTorrent))
            {
                Log.Get().Write("Load torrent from path: " + pathTorrent);
                try
                {
                    int fileLength = (int)new System.IO.FileInfo(pathTorrent).Length;
                    result = new byte[fileLength];
                    FileStream fileStream = File.Open(pathTorrent, FileMode.Open);
                    fileStream.Read(result, 0, fileLength);
                    fileStream.Close();
                }
                catch(Exception ex)
                {
                    Log.Get().Write("Failed to load torrent file: " + ex,Log.LogType.Error);
                }
            }
            else
            {
                //-------------------------------------------------------------
                //Create new torrent file
                //-------------------------------------------------------------
                Log.Get().Write("Creating torrent file for path: " + path);

                TorrentCreator c = new TorrentCreator();
                c.CreatedBy = "Lan";
                c.Publisher = "Lan";
                c.Comment = title;
                c.Private = true; //Do not share the torrent with other tracker. (Disable DHT or peer exchange)

                //-------------------------------------------------------------
                // Every time a piece has been hashed, this event will fire.
                //-------------------------------------------------------------
                c.Hashed += HashedDelegate;

                //-------------------------------------------------------------
                //Path to directory
                //-------------------------------------------------------------
                ITorrentFileSource fileSource = new TorrentFileSource(path);

                //-------------------------------------------------------------
                //Create torrent
                //-------------------------------------------------------------
                BEncodedDictionary dict = await c.CreateAsync(fileSource);
                result = dict.Encode();
                Log.Get().Write("Creating torrent file in memory. Size: " + result.Length);

                //-------------------------------------------------------------
                //Save torrent to HDD (To increase loadtime next time)
                //-------------------------------------------------------------
                string createTorrentFile = ConfigurationManager.AppSettings.Get("Torrent:CreateTorrentFiles");
                if(Convert.ToBoolean(createTorrentFile))
                {
                    Log.Get().Write("Saving torrent file to path: " + pathTorrent);
                    try
                    {
                        FileStream fileStream = File.Create(pathTorrent);
                        fileStream.Write(result, 0, result.Length);
                        fileStream.Close();
                    }
                    catch (Exception ex)
                    {
                        Log.Get().Write("Failed to save torrent file: " + ex, Log.LogType.Error);
                    }
                }
            }

            return result;
        }

        //-------------------------------------------------------------
        public void StartTracker(byte[] torrentData)
        //-------------------------------------------------------------
        {
            Log.Get().Write("Starting torrent tracker server, http on port: " + TRACKERHTTP_PORT + " udp on port: " + TRACKERUDP_PORT);
            if(!Helper.IsPortAvailable(TRACKERHTTP_PORT))
            {
                Log.Get().Write("Failed starting torrent tracker server on http port: " + TRACKERHTTP_PORT + " Port in use", Log.LogType.Error);
            }
            if (!Helper.IsPortAvailable(TRACKERUDP_PORT))
            {
                Log.Get().Write("Failed starting torrent tracker server on UDP port: " + TRACKERUDP_PORT + " Port in use", Log.LogType.Error);
            }

            //-------------------------------------------------------------
            //Create tracker server
            //-------------------------------------------------------------
            if (trackerServer == null)
            {
                trackerServer = new TrackerServer();
                listenerHttp = TrackerListenerFactory.CreateHttp(IPAddress.Any, TRACKERHTTP_PORT); // http://localhost:{TRACKER_PORT}/announce
                listenerUdp = TrackerListenerFactory.CreateUdp(IPAddress.Any, TRACKERUDP_PORT); // http://localhost:{TRACKER_PORT}/announce

                //Add logning
                trackerServer.PeerAnnounced += delegate (object o, AnnounceEventArgs e) { Log.Get().Write("TrackerServer PeerAnnounced"); };
                trackerServer.PeerScraped += delegate (object o, ScrapeEventArgs e) { Log.Get().Write("TrackerServer PeerScrape"); };
                trackerServer.PeerTimedOut += delegate (object o, TimedOutEventArgs e) { Log.Get().Write("TrackerServer Peer timeout"); };
                listenerHttp.AnnounceReceived += delegate (object o, AnnounceRequest e) { Log.Get().Write("TrackerListenerHTTP Announce received"); };
                listenerHttp.ScrapeReceived += delegate (object o, TrackerScrapeRequest e) { Log.Get().Write("TrackerListenerHTTP Scrape received"); };
                listenerHttp.StatusChanged += delegate (object o, EventArgs e) 
                {
                    Log.Get().Write("TrackerListenerHttp Status changed: " + listenerHttp.Status);  //Typecast not working here, protectionlevel error. :( /*((MonoTorrent.Tracker.Listeners.HttpTrackerListener)o)*/
                };
                listenerUdp.AnnounceReceived += delegate (object o, AnnounceRequest e) { Log.Get().Write("TrackerListenerUDP Announce received"); };
                listenerUdp.ScrapeReceived += delegate (object o, TrackerScrapeRequest e) { Log.Get().Write("TrackerListenerUDP Scrape received"); };
                listenerUdp.StatusChanged += delegate (object o, EventArgs e)
                {
                    Log.Get().Write("TrackerListenerUdp Status changed: " + listenerUdp.Status);
                };

                //Start tracking server
                trackerServer.RegisterListener(listenerHttp);
                trackerServer.RegisterListener(listenerUdp);
                listenerUdp.Start();
                listenerHttp.Start();
                trackerServer.AllowUnregisteredTorrents = false; // If an announce request is received for a torrent which is not registered with the tracker an error will be returned.
                Log.Get().Write("TrackerListener listener status http:" + listenerHttp.Status + " udp status: " + listenerUdp.Status);
            }

            //-------------------------------------------------------------
            //Add new torrent to tracker
            //-------------------------------------------------------------
            MonoTorrent.Torrent torrent = MonoTorrent.Torrent.Load(torrentData);
            InfoHashTrackable trackable = new InfoHashTrackable(torrent);
            trackerServer.Add(trackable);
            Log.Get().Write("Adding torrent to tracker server, torrent size: " + torrentData.Length);

            //-------------------------------------------------------------
            //Seed file
            //-------------------------------------------------------------
            Log.Get().Write("Prepare to seed file");
            DownloadTorrent(torrent);
        }

        //-------------------------------------------------------------
        public void StopSharing(string key)
        //-------------------------------------------------------------
        {
            if(!torrentManagers.Any(t => t.Torrent.Name.Equals(key)))
            {
                Log.Get().Write("TorrentManagers could not find torrent to stop: " + key, Log.LogType.Warning);
                return;
            }

            Log.Get().Write("TorrentManagers stopping torrent: " + key);
            var torrent = torrentManagers.Where(t => t.Torrent.Name.Equals(key) ).FirstOrDefault();
            torrent.StopAsync(new TimeSpan(0,0,3));
        }


        //-------------------------------------------------------------
        public string GetTorrentName(byte[] torrentData)
        //-------------------------------------------------------------
        {
            MonoTorrent.Torrent torrent = MonoTorrent.Torrent.Load(torrentData);
            return torrent.Name;
        }

        //-------------------------------------------------------------
        public byte[] InsertAnnounce(byte[] torrentData)
        //-------------------------------------------------------------
        {
            MonoTorrent.Torrent torrent = MonoTorrent.Torrent.Load(torrentData);
            torrent.AnnounceUrls.Clear();
            torrent.AnnounceUrls.Add(new RawTrackerTier(new[] { "http://" + Helper.getHostIP().ToString() + ":" + TRACKERHTTP_PORT + "/announce", "udp://" + Helper.getHostIP().ToString() + ":" + TRACKERUDP_PORT }));

            byte[] result = null;
            TorrentEditor torrentEditor = new TorrentEditor(torrent);
            torrentEditor.Announces.Clear();
            torrentEditor.Announces.Add(new RawTrackerTier(new[] { "http://" + Helper.getHostIP().ToString() + ":" + TRACKERHTTP_PORT + "/announce", "udp://" + Helper.getHostIP().ToString() + ":" + TRACKERUDP_PORT }));
            var dict = torrentEditor.ToDictionary();
            result = dict.Encode();

            return result;
        }

        //-------------------------------------------------------------
        public void DownloadTorrent(byte[] torrentData)
        //-------------------------------------------------------------
        {
            Log.Get().Write("Client downloading files from torrent with size: " + torrentData.Length);
            MonoTorrent.Torrent torrent = MonoTorrent.Torrent.Load(torrentData);
            DownloadTorrent(torrent);
        }

        //-------------------------------------------------------------
        public void DownloadTorrent(MonoTorrent.Torrent torrent)
        //-------------------------------------------------------------
        {
            if (torrent == null)
            {
                Log.Get().Write("torrent is null", Log.LogType.Error);
            }

            if (torrent.AnnounceUrls.Count <= 0)
            {
                Log.Get().Write("torrent missing announce", Log.LogType.Error);
            }

            string path = Helper.GetDirection();
            Log.Get().Write("Client downloading files torrent: " + torrent.Name);

            if (!Helper.IsPortAvailable(CLIENT_PORT))
            {
                Log.Get().Write("Failed starting client downloading file on port: " + CLIENT_PORT + " Port in use", Log.LogType.Error);
            }

            if(settings == null || engine == null)
            {
                //Settings
                settings = new EngineSettings();
                settings.AllowedEncryption = EncryptionTypes.All;
                settings.PreferEncryption = true;
                settings.SavePath = path;
                settings.ListenPort = CLIENT_PORT;

                //Create client engine
                engine = new ClientEngine(settings);
            }

            //Check if torrent exist (reStarting a torrent)
            var torrentManagersResult = torrentManagers.Where(t => t.Torrent.Name.Equals(torrent.Name)).ToList();
            if(torrentManagersResult.Count >= 1)
            {
                Log.Get().Write("ReStarting torrent: " + torrent.Name);
                torrentManagersResult.FirstOrDefault().StartAsync();
            }
            else //New torrent
            {
                Log.Get().Write("Preparing new torrent: " + torrent.Name);
                var torrentSettings = new TorrentSettings();
                torrentSettings.AllowDht = false;
                torrentSettings.AllowPeerExchange = true;
                TorrentManager torrentManager = new TorrentManager(torrent, path, torrentSettings);

                //Add logning
                torrentManager.ConnectionAttemptFailed += delegate (object o, ConnectionAttemptFailedEventArgs e) { Log.Get().Write("TorrentManager connectionAttemptFailed", Log.LogType.Error); };
                torrentManager.PeerConnected += delegate (object o, PeerConnectedEventArgs e) { Log.Get().Write("TorrentManager PeerConnected"); };
                torrentManager.PeerDisconnected += delegate (object o, PeerDisconnectedEventArgs e) { Log.Get().Write("TorrentManager PeerDisconnected"); };
                torrentManager.PeersFound += delegate (object o, PeersAddedEventArgs e) { Log.Get().Write("TorrentManager PeersAdded"); };
                torrentManager.PieceHashed += pieceHashedDelegate;

                torrentManager.TorrentStateChanged += delegate (object o, TorrentStateChangedEventArgs e)
                {
                    Log.Get().Write("TorrentManager TorrentStateChanged, oldstate: " + e.OldState + " , newState: " + e.NewState);
                    if (kobberLan != null)
                    {
                        kobberLan.Invoke(new Action(() =>
                        {
                            bool status = kobberLan.UpdateProgressBar(e.NewState, (int)(torrentManager.Progress), torrentManager.Torrent.Name);
                            if (status == false)
                            {
                                torrentManager.StopAsync();
                                Log.Get().Write("TorrentHandler failed", Log.LogType.Error);
                            }

                        }));
                    }
                };

                engine.CriticalException += delegate (object o, CriticalExceptionEventArgs e) { Log.Get().Write("TorrentEngine CriticalException", Log.LogType.Error); };
                engine.StatsUpdate += delegate (object o, StatsUpdateEventArgs e)
                {
                    if (o.GetType() == typeof(MonoTorrent.Client.ClientEngine))
                    {
                        ClientEngine clientEngine = (ClientEngine)o;

                        Log.Get().Write("TorrentEngine Statsupdate running: " + clientEngine.IsRunning +
                                        " , torrent Seeds: " + torrentManager.Peers.Seeds + ", Leech: " + torrentManager.Peers.Leechs +
                                        " , progress: " + torrentManager.Progress
                                        );

                        if (kobberLan != null)
                        {
                            kobberLan.Invoke(new Action(() =>
                            {

                                bool status = kobberLan.UpdateProgressBar(torrentManager.State, (int)(torrentManager.Progress), torrentManager.Torrent.Name);
                                if(status == false)
                                {
                                    torrentManager.StopAsync();
                                    Log.Get().Write("TorrentHandler failed", Log.LogType.Error);
                                }

                            }));
                        }
                    }
                    else
                    {
                        Log.Get().Write("TorrentEngine Statsupdate: Unknown", Log.LogType.Error);
                    }

                };

                //Start downloading
                engine.Register(torrentManager);
                engine.StartAllAsync();

                //Add torrent to list
                torrentManagers.Add(torrentManager);
            }

        }

        //-------------------------------------------------------------
        public void pieceHashedDelegate(object o, PieceHashedEventArgs e)
        //-------------------------------------------------------------
        {
            if (e != null && o != null)
            {
                TorrentManager torrentManager = (TorrentManager)o;
                if (torrentManager.State == TorrentState.Stopped)
                {
                    return;
                }

                if (kobberLan != null && e.TorrentManager != null && e.TorrentManager.Torrent != null)
                {
                    kobberLan.Invoke(new Action(() =>
                    {
                        bool status = kobberLan.UpdateProgressBar(TorrentState.Hashing, (int)(e.Progress * 100.0), e.TorrentManager.Torrent.Name); // e.TorrentManager.State
                        if (status == false)
                        {
                            torrentManager.PieceHashed -= pieceHashedDelegate;
                            torrentManager.StopAsync();
                            Log.Get().Write("PieceHashed failed",Log.LogType.Error);
                        }
                    }));
                }
            }
        }

        //-------------------------------------------------------------
        public void HashedDelegate(object o, TorrentCreatorEventArgs torrentEvent)
        //-------------------------------------------------------------
        {
            Log.Get().Write("Current File is " + torrentEvent.FileCompletion * 100 + "% hashed");
            Log.Get().Write("Overall " + torrentEvent.OverallCompletion * 100 + "% hashed");
            Log.Get().Write("Total data to hash: " + torrentEvent.OverallSize);

            if (kobberLan != null)
            {
                kobberLan.Invoke(new Action(() =>
                {
                    TorrentCreator c = (TorrentCreator)o;
                    if (String.IsNullOrEmpty(c.Comment))
                        return;

                    bool status = kobberLan.UpdateProgressBar(TorrentState.Metadata, (int)(torrentEvent.OverallCompletion * 100.0), c.Comment);
                    if (status == false)
                    {
                        c.Hashed -= HashedDelegate;
                        c.Comment = "";
                        Log.Get().Write("Hashed failed", Log.LogType.Error);
                    }
                }));
            }
        }
            

    }
}
