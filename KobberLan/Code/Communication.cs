using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KobberLan.Code
{
    //-------------------------------------------------------------
    public class Communication
    //-------------------------------------------------------------
    {
        public static int COMMUNICATION_PORT;

        private SynchronizedCollection<IPAddress> playerList;

        private Thread threadServer;
        private bool threadServerActive;

        private TcpListener communicationServer = null;
        private TcpClient communicationClient = null;

        private CancellationTokenSource threadClient;

        private KobberLan kobberLanGui;

        private BlockingCollection<QueueNetwork> queueNetwork;

        //-------------------------------------------------------------
        private class QueueNetwork
        //-------------------------------------------------------------
        {
            public object DTO;
            public string IP;
        }

        //-------------------------------------------------------------
        public Communication(KobberLan gui)
        //-------------------------------------------------------------
        {
            queueNetwork = new BlockingCollection<QueueNetwork>();

            //Read port from ApplicationConfig
            COMMUNICATION_PORT = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Port:CommunicationTCP"));

            kobberLanGui = gui;
        }

        //-------------------------------------------------------------
        public void SetPlayers(SynchronizedCollection<IPAddress> players)
        //-------------------------------------------------------------
        {
            playerList = players;
        }

        //-------------------------------------------------------------
        // Create and start the server thread
        public void ServerStartListening()
        //-------------------------------------------------------------
        {
            Log.Get().Write("Communication starting server thread");
            threadServer = new Thread(new ThreadStart(ServerListen));
            threadServerActive = true;
            threadServer.Start();

            //Start client thread
            Log.Get().Write("Communication starting client thread");
            threadClient = new CancellationTokenSource();
            var task = Task.Run(() => ThreadClientSend(threadClient.Token), threadClient.Token);
        }

        //-------------------------------------------------------------
        private async Task<bool> ClientSendData(byte[] data, string IP, IPAddress player)
        //-------------------------------------------------------------
        {
            Log.Get().Write("Communication client sending to: " + IP + ", size: " + data.Length);
            if(player == null)player = playerList.SingleOrDefault(p => p.ToString().Equals(IP));

            try
            {
                communicationClient = new TcpClient();
                await communicationClient.ConnectAsync(IP, COMMUNICATION_PORT);

                var stream = communicationClient.GetStream();

                //Send data size
                byte[] dataSize = BitConverter.GetBytes(data.Length);
                if (dataSize.Length != sizeof(int))
                {
                    Log.Get().Write("Int wrong size", Log.LogType.Error);
                }
                await stream.WriteAsync(dataSize, 0, dataSize.Length);

                //Send data
                await stream.WriteAsync(data, 0, data.Length);

                stream.Close();
                communicationClient.Close();
                return true;
            }
            catch (SocketException ex)
            {
                if(ex.SocketErrorCode.Equals("ConnectionRefused") ||
                   ex.SocketErrorCode.Equals("TimedOut"))
                {
                    Log.Get().Write("Communication client Socketexception (Player disconnected or firewall?) size:" + data.Length + " ip:" + IP + " ex:" + ex, Log.LogType.Warning);
                }
                else
                {
                    Log.Get().Write("Communication client Socketexception (Firewall problem) size:" + data.Length + " ip:" + IP + " ex:" + ex, Log.LogType.Error);
                }

                Log.Get().Write("Removing " + player?.ToString() + " client from list", Log.LogType.Warning);
                if(player != null)
                {
                    playerList.Remove(player); //Failed to send to client, remove from list

                    //Update gui
                    kobberLanGui.Invoke(new Action(() =>
                    {
                        kobberLanGui.UpdatePlayersAmount(playerList);
                    }));

                }

                return false;
            }
            catch (Exception ex)
            {
                Log.Get().Write("Removing " + player?.ToString() + " client from list", Log.LogType.Warning);
                if (player != null)
                {
                    playerList.Remove(player); //Failed to send to client, remove from list

                    //Update gui
                    kobberLanGui.Invoke(new Action(() =>
                    {
                        kobberLanGui.UpdatePlayersAmount(playerList);
                    }));

                }
                Log.Get().Write("Communication client exception: " + ex, Log.LogType.Error);
                return false;
            }
        }

        //-------------------------------------------------------------
        public void ClientSend(object DTO,string IP = "")
        //-------------------------------------------------------------
        {
            if(playerList == null || playerList.Count == 0)
            {
                Log.Get().Write("Communication client no players found", Log.LogType.Info);
                return;
            }

            queueNetwork.Add(new QueueNetwork() {DTO = DTO, IP = IP });
            kobberLanGui.UpdateQueueText(queueNetwork.Count());
        }

        //-------------------------------------------------------------
        private void ThreadClientSend(CancellationToken token)
        //-------------------------------------------------------------
        {
            Log.Get().Write("Starting Client Thread");

            while(!token.IsCancellationRequested)
            {
                if(queueNetwork.Count > 0)
                {
                    QueueNetwork packet = queueNetwork.Take();
                    kobberLanGui.UpdateQueueText(queueNetwork.Count());

                    //Check if player is online if IP is specified
                    if(!String.IsNullOrEmpty(packet.IP))
                    {
                        bool playerExist = false;
                        foreach (IPAddress player in playerList)
                        {
                            if (player.ToString().Equals(packet.IP))
                                playerExist = true;
                        }
                        if(!playerExist)
                        {
                            continue;
                        }
                    }

                    //Start communication
                    Log.Get().Write("Communication client prepare data");
                    byte[] data = Helper.ObjectToByteArray(packet.DTO);

                    if (String.IsNullOrEmpty(packet.IP))
                    {
                        //Send packet to all known players
                        foreach (IPAddress player in playerList)
                        {
                            Log.Get().Write("Communication send DTO to client: " + packet.IP);
                            ClientSendData(data, player.ToString(), player).Wait();
                        }
                    }
                    else // Specic player only (when rejoining or new player open KobberParty)
                    {
                        Log.Get().Write("Communication send DTO to specific client: " + packet.IP);
                        ClientSendData(data, packet.IP, null).Wait();
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }

            Log.Get().Write("Closing Client Thread");
        }

        //-------------------------------------------------------------
        public void CloseAll()
        //-------------------------------------------------------------
        {
            Log.Get().Write("Communication closing threads");
            threadServerActive = false;
            if (communicationServer != null) communicationServer.Stop();
            if (communicationClient != null) communicationClient.Close();
            threadClient.Cancel();
        }

        //-------------------------------------------------------------
        private void ServerListen()
        //-------------------------------------------------------------
        {
            if (!Helper.IsPortAvailable(COMMUNICATION_PORT))
            {
                Log.Get().Write("Failed starting communication server on port: " + COMMUNICATION_PORT + " Port in use", Log.LogType.Error);
            }

            communicationServer = new TcpListener(Helper.GetHostIP() , COMMUNICATION_PORT); // IPAddress.Any
            communicationServer.Start();
            while (threadServerActive)
            {
                try
                {
                    List<byte> bytesReceived = new List<byte>();

                    //Block until message from a client
                    Log.Get().Write("Communication server is running at port: " + COMMUNICATION_PORT + " at interface: " + communicationServer.LocalEndpoint);
                    Socket s = communicationServer.AcceptSocket();
                    Log.Get().Write("Communication server got connection from client: " + s.RemoteEndPoint.ToString() );

                    //Ignore own IP
                    IPEndPoint remoteIP = (IPEndPoint)s.RemoteEndPoint;
                    if (remoteIP.Address.Equals("127.0.0.1") || remoteIP.Address.Equals(Helper.GetHostIP()))
                    {
                        Log.Get().Write("Ignore own connection");
                        s.Close();
                        continue;
                    }

                    //Timeout value
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    s.SendTimeout    = Convert.ToInt32(ConfigurationManager.AppSettings.Get("CommunicationSocket:SendTimeout"));
                    s.ReceiveTimeout = Convert.ToInt32(ConfigurationManager.AppSettings.Get("CommunicationSocket:ReceiveTimeout"));

                    //Read first 4 bytes (size of packet)
                    byte[] dataSizeArray = new byte[sizeof(int)];
                    s.Receive(dataSizeArray, 0, sizeof(int), SocketFlags.None);
                    int dataSize = BitConverter.ToInt32(dataSizeArray, 0);
                    Log.Get().Write("Communication server got size packet from client. datasize: " + dataSize);

                    //Handle data from client
                    while (bytesReceived.Count != dataSize)
                    {
                        if(watch.ElapsedMilliseconds > Convert.ToInt32(ConfigurationManager.AppSettings.Get("CommunicationSocket:WatchDogTimeOut")))
                        {
                            throw new Exception("Communication server, WatchDog timeout");
                        }

                        byte[] nextByte = new byte[1];
                        s.Receive(nextByte, 0, 1, SocketFlags.None);
                        bytesReceived.AddRange(nextByte);
                    }

                    //Handle message
                    Log.Get().Write("Communication server got packet from client. size: " + bytesReceived.Count);
                    HandleMessage(bytesReceived.ToArray(), remoteIP.Address.ToString());

                    //Close connection
                    s.Close();

                    //Clear data
                    bytesReceived.Clear();
                    bytesReceived = null;
                }
                catch (SocketException socketEx)
                {
                    if (threadServerActive == false) { } // Ignore, program shutting down
                    else
                        Log.Get().Write("Communication server socket exception: " + socketEx, Log.LogType.Error);
                }
                catch (Exception ex)
                {
                    Log.Get().Write("Communication server exception: " + ex, Log.LogType.Error);
                }
            }

        }

        //-------------------------------------------------------------
        private void HandleMessage(byte [] data,string RemoteIP)
        //-------------------------------------------------------------
        {
            Object dataObject = Helper.ByteArrayToObject(data);
            if(dataObject.GetType() == typeof(DTO_Suggestion))
            {
                DTO_Suggestion suggestion = (DTO_Suggestion)dataObject;
                Log.Get().Write("Communication server handle Suggestiongame: " + suggestion.title);
                kobberLanGui.Invoke(new Action(() =>
                {
                    kobberLanGui.AddSuggestedGame(suggestion, null, false, RemoteIP);
                }));
            }
            else if (dataObject.GetType() == typeof(DTO_Like))
            {
                DTO_Like like = (DTO_Like)dataObject;
                Log.Get().Write("Communication server handle Like: " + like.key);
                kobberLanGui.Invoke(new Action(() =>
                {
                    kobberLanGui.GotLike(like);
                }));
            }
            else if (dataObject.GetType() == typeof(DTO_AlreadyOwnIt))
            {
                DTO_AlreadyOwnIt alreadyOwnIt = (DTO_AlreadyOwnIt)dataObject;
                Log.Get().Write("Communication server handle alreadyOwnIt: " + alreadyOwnIt.key);
                kobberLanGui.Invoke(new Action(() =>
                {
                    kobberLanGui.GotAlreadyOwnIt(alreadyOwnIt);
                }));
            }
            else if (dataObject.GetType() == typeof(DTO_Torrent))
            {
                DTO_Torrent torrent = (DTO_Torrent)dataObject;
                Log.Get().Write("Communication server handle torrent: " + torrent.key);
                kobberLanGui.Invoke(new Action(() =>
                {
                    kobberLanGui.GotTorrent(torrent);
                }));
            }
            else if (dataObject.GetType() == typeof(DTO_TorrentStatus))
            {
                DTO_TorrentStatus torrent = (DTO_TorrentStatus)dataObject;
                Log.Get().Write("Communication server handle torrentStatus: " + torrent.key);
                kobberLanGui.Invoke(new Action(() =>
                {
                    kobberLanGui.GotTorrentStatus(torrent);
                }));
            }
            else if (dataObject.GetType() == typeof(DTO_GameStatus))
            {
                DTO_GameStatus gameStatus = (DTO_GameStatus)dataObject;
                Log.Get().Write("Communication server handle gameStatus: " + gameStatus.key);
                kobberLanGui.Invoke(new Action(() =>
                {
                    kobberLanGui.GotGameStatus(gameStatus);
                }));
            }
            else if (dataObject.GetType() == typeof(DTO_RequestAllSuggestions))
            {
                DTO_RequestAllSuggestions gameStatus = (DTO_RequestAllSuggestions)dataObject;
                Log.Get().Write("Communication server handle client get all Suggestions: " + gameStatus.address.ToString());
                kobberLanGui.Invoke(new Action(() =>
                {
                    kobberLanGui.UpdatePlayerSuggested(gameStatus.address.ToString());
                }));
            }
            else
            {
                Log.Get().Write("Communication server got unknown message", Log.LogType.Warning);
            }

        }

    }
}
