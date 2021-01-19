
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KobberLan.Code
{
    //-------------------------------------------------------------
    public class Broadcast
    //-------------------------------------------------------------
    {
        public static int BROADCAST_PORT;

        private string BROADCAST_MESSAGE_SEARCH = "B";
        private string BROADCAST_MESSAGE_DISCONNECT = "D";
        private string BROADCAST_PORTCHECK = "P";

        private Thread threadServer;
        private Thread threadClient;

        private bool threadServerActive;
        private bool threadClientActive;

        private bool firewallBlocking;

        private UdpClient broadcastServer = null;
        private UdpClient broadcastClient = null;

        private KobberLan kobberLanGui;

        private SynchronizedCollection<IPAddress> playersFound;

        private readonly object syncLock = new object();

        //-------------------------------------------------------------
        public Broadcast(KobberLan gui)
        //-------------------------------------------------------------
        {
            firewallBlocking = true;

            //Read port from ApplicationConfig
            BROADCAST_PORT = Convert.ToInt32( ConfigurationManager.AppSettings.Get("Port:BroadcastUDP") );

            playersFound = new SynchronizedCollection<IPAddress>();
            kobberLanGui = gui;

            Log.Get().Write("Broadcast own IP: " + Helper.GetHostIP() );
        }

        //-------------------------------------------------------------
        public void CloseAll()
        //-------------------------------------------------------------
        {
            Log.Get().Write("Broadcast Closing threads");
            threadServerActive = false;
            threadClientActive = false;
            if(broadcastServer != null)broadcastServer.Close();
            if(broadcastClient != null)broadcastClient.Close();
        }

        //-------------------------------------------------------------
        public bool FirewallProblem()
        //-------------------------------------------------------------
        {
            return firewallBlocking;
        }

        //-------------------------------------------------------------
        // Create and start the server thread
        public void ServerStartListeningBroadCast()
        //-------------------------------------------------------------
        {
            Log.Get().Write("Broadcast starting server thread");
            threadServer = new Thread(new ThreadStart(ServerListenBroadCast_Search));
            threadServerActive = true;
            threadServer.Start();
            threadClientActive = true;
        }

        //-------------------------------------------------------------
        private void ServerListenBroadCast_Search()
        //-------------------------------------------------------------
        {
            if (!Helper.IsPortAvailable(BROADCAST_PORT))
            {
                Log.Get().Write("Failed starting broadcast server on port: " + BROADCAST_PORT + " Port in use", Log.LogType.Error);
            }

            broadcastServer = new UdpClient(BROADCAST_PORT);

            while (threadServerActive)
            {
                try
                {
                    Log.Get().Write("Broadcast server waiting for client message");
                    var ClientEp = new IPEndPoint(IPAddress.Any, BROADCAST_PORT); //Note, reference and auto filled

                    //Block until message from a client
                    var ClientRequestData = broadcastServer.Receive(ref ClientEp);
                    var ClientRequest = Encoding.ASCII.GetString(ClientRequestData);
                    Log.Get().Write("Broadcast server received " + ClientRequest + " from " + ClientEp.Address.ToString() + ", sending response");
                    
                    //Send response to client
                    broadcastServer.Send(Encoding.ASCII.GetBytes(BROADCAST_MESSAGE_SEARCH), BROADCAST_MESSAGE_SEARCH.Length, ClientEp);

                    //Handle message
                    HandleMessage(ClientRequest, ClientEp);
                }
                catch (SocketException socketEx)
                {
                    if (threadServerActive == false) { } // Ignore, program shutting down
                    else
                        Log.Get().Write("Broadcast serverBroadcast Socket exception: " + socketEx, Log.LogType.Error);
                }
                catch(Exception ex)
                {
                    Log.Get().Write("Broadcast serverBroadcast exception: " + ex, Log.LogType.Error);
                    threadServerActive = false;
                }
            }
        }

        //-------------------------------------------------------------
        public void ClientSendBroadcast()
        //-------------------------------------------------------------
        {
            //-------------------------------------------------------------
            //Prepare new Broadcast request
            //-------------------------------------------------------------
            if (broadcastClient == null)
            {
                broadcastClient = new UdpClient();
                broadcastClient.EnableBroadcast = true;
            }

            //-------------------------------------------------------------
            //Handle ClientBroadCast Request in a thread
            //-------------------------------------------------------------
            Log.Get().Write("Broadcast starting client thread");
            threadClient = new Thread(new ThreadStart(ClientBroadcast));
            threadClient.Start();
        }

        //-------------------------------------------------------------
        public void ClientSendBroadCast_Disconnect()
        //-------------------------------------------------------------
        {
            Log.Get().Write("Broadcast client broadcasting disconnect");
            broadcastClient.Send(Encoding.ASCII.GetBytes(BROADCAST_MESSAGE_DISCONNECT), BROADCAST_MESSAGE_DISCONNECT.Length, new IPEndPoint(Helper.GetHostIPBroadcastAddress(), BROADCAST_PORT));
        }

        //-------------------------------------------------------------
        private void ClientBroadcast()
        //-------------------------------------------------------------
        {
            Log.Get().Write("Broadcast client broadcasting message");
            broadcastClient.Send(Encoding.ASCII.GetBytes(BROADCAST_MESSAGE_SEARCH), BROADCAST_MESSAGE_SEARCH.Length, new IPEndPoint(Helper.GetHostIPBroadcastAddress(), BROADCAST_PORT));

            while(threadClientActive)
            {
                try
                {
                    IPEndPoint broadcastAdress = new IPEndPoint(IPAddress.Any, BROADCAST_PORT); //Note, reference and auto filled

                    //Block until message from a server/listener
                    var ServerResponseData = broadcastClient.Receive(ref broadcastAdress);

                    //Handle message
                    string ServerResponse = Encoding.ASCII.GetString(ServerResponseData);
                    Log.Get().Write("Broadcast client received message " + ServerResponse + " from " + broadcastAdress.Address.ToString() + ", sending response");
                    HandleMessage(ServerResponse, broadcastAdress);
                }
                catch (SocketException socketEx)
                {
                    if (threadServerActive == false) { } // Ignore, program shutting down
                    else
                    {
                        Log.Get().Write("Broadcast client Socket exception: " + socketEx, Log.LogType.Error);
                    }
                }
                catch (Exception ex)
                {
                    Log.Get().Write("Broadcast client exception: " + ex, Log.LogType.Error);
                    threadClientActive = false;
                }
            }
        }

        //-------------------------------------------------------------
        private void HandleMessage(string msg, IPEndPoint otherIPAddress)
        //-------------------------------------------------------------
        {
            //-------------------------------------------------------------
            //Multi thread can call this, make sure to handle 1 at a time
            //-------------------------------------------------------------
            lock (syncLock)
            {
                //-------------------------------------------------------------
                //Port check
                //-------------------------------------------------------------
                if (msg.Equals(BROADCAST_PORTCHECK.ToString()))
                {
                    Log.Get().Write("Portcheck handling"); //Ignore message
                }
                //-------------------------------------------------------------
                //Broadcast message - DISCONNECT
                //-------------------------------------------------------------
                else if (msg.Equals(BROADCAST_MESSAGE_DISCONNECT.ToString()))
                {
                    Log.Get().Write("Broadcast handling message. Another player is disconnecting: " + otherIPAddress.Address.ToString());

                    //Ignore own server
                    if (Helper.GetHostIP().ToString().Equals(otherIPAddress.Address.ToString()))
                    {
                        Log.Get().Write("Broadcast disconnect ignore own server", Log.LogType.Info);
                    }
                    else
                    {
                        //Update gui
                        kobberLanGui.Invoke(new Action(() =>
                        {
                            kobberLanGui.RemovePlayer(otherIPAddress.Address.ToString());
                        }));
                    }
                }
                //-------------------------------------------------------------
                //Broadcast message - SEARCH
                //-------------------------------------------------------------
                else if (msg.Equals(BROADCAST_MESSAGE_SEARCH.ToString()))
                {
                    Log.Get().Write("Broadcast handling message. Another player responded by broadcast: " + otherIPAddress.Address.ToString());

                    //Ignore own server
                    if (Helper.GetHostIP().ToString().Equals(otherIPAddress.Address.ToString()))
                    {
                        firewallBlocking = false;
                        Log.Get().Write("Broadcast search ignore own server", Log.LogType.Info);
                    }
                    else
                    {
                        //Add if unknown player
                        if (!playersFound.Contains(otherIPAddress.Address))
                        {
                            Log.Get().Write("Broadcast added new player to list.");
                            playersFound.Add(otherIPAddress.Address);

                            //Update gui
                            kobberLanGui.Invoke(new Action(() =>
                            {
                                kobberLanGui.UpdatePlayersAmount(playersFound);
                            }));

                            //Send suggested game (Only to new players)
                            kobberLanGui.Invoke(new Action(() =>
                            {
                                kobberLanGui.UpdatePlayerSuggested(otherIPAddress.Address.ToString());
                            }));
                        }
                        else
                        {
                            Log.Get().Write("Broadcast player already exist", Log.LogType.Info);
                        }
                    }
                }
                //-------------------------------------------------------------
                //Unknown message
                //-------------------------------------------------------------
                else
                {
                    Log.Get().Write("Broadcast unknown message received: " + msg, Log.LogType.Error);
                }

            }

        }


    }   
}
