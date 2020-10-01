using System;
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

        private List<IPAddress> playerList;

        private Thread threadServer;
        private bool threadServerActive;

        private TcpListener communicationServer = null;
        private TcpClient communicationClient = null;

        private KobberLan kobberLanGui;

        //-------------------------------------------------------------
        public Communication(KobberLan gui)
        //-------------------------------------------------------------
        {
            //Read port from ApplicationConfig
            COMMUNICATION_PORT = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Port:CommunicationTCP"));

            kobberLanGui = gui;
        }

        //-------------------------------------------------------------
        public void SetPlayers(List<IPAddress> players)
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
        }

        //-------------------------------------------------------------
        private bool ClientSendData(byte[] data, string IP)
        //-------------------------------------------------------------
        {
            Log.Get().Write("Communication client sending to: " + IP + ", size: " + data.Length);

            try
            {
                communicationClient = new TcpClient();
                communicationClient.Connect(IP, COMMUNICATION_PORT);

                var stream = communicationClient.GetStream();

                //Send data size
                byte[] dataSize = BitConverter.GetBytes(data.Length);
                if(dataSize.Length != sizeof(int))
                {
                    Log.Get().Write("Int wrong size", Log.LogType.Error);
                }
                stream.Write(dataSize, 0, dataSize.Length);

                //Send data
                stream.Write(data, 0, data.Length);

                stream.Close();
                communicationClient.Close();
                return true;
            }
            catch (Exception ex)
            {
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

            Log.Get().Write("Communication client prepare data");
            byte[] data = Helper.ObjectToByteArray(DTO);

            if (String.IsNullOrEmpty(IP))
            {
                //Send packet to all known players
                List<IPAddress> playerRemoveList = new List<IPAddress>();
                foreach (IPAddress player in playerList)
                {
                    Log.Get().Write("Communication send DTO to client: " + IP);
                    if (false == ClientSendData(data, player.ToString()))
                    {
                        //Failed to send to client, remove from list
                        Log.Get().Write("Removing " + player.ToString() + " client from list", Log.LogType.Warning);
                        playerRemoveList.Add(player);
                    }
                }
                foreach (IPAddress player in playerRemoveList) { playerList.Remove(player); }
            }
            else // Specic player only (when rejoining or new player open KobberParty)
            {
                Log.Get().Write("Communication send DTO to specific client: " + IP);
                ClientSendData(data, IP);
            }

        }

        //-------------------------------------------------------------
        public void CloseAll()
        //-------------------------------------------------------------
        {
            Log.Get().Write("Communication closing threads");
            threadServerActive = false;
            if (communicationServer != null) communicationServer.Stop();
            if (communicationClient != null) communicationClient.Close();
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
                    s.SendTimeout    = 3000;
                    s.ReceiveTimeout = 3000;

                    //Read first 4 bytes (size of packet)
                    byte[] dataSizeArray = new byte[sizeof(int)];
                    s.Receive(dataSizeArray, 0, sizeof(int), SocketFlags.None);
                    int dataSize = BitConverter.ToInt32(dataSizeArray, 0);
                    Log.Get().Write("Communication server got size packet from client. datasize: " + dataSize);

                    //Handle data from client
                    while (s.Available > 0 && bytesReceived.Count != dataSize)
                    {
                        if(watch.ElapsedMilliseconds > 5000)
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
            else
            {
                Log.Get().Write("Communication server got unknown message", Log.LogType.Warning);
            }

        }

    }
}
