using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KobberLan.Code
{
    //-------------------------------------------------------------
    public class Helper
    //-------------------------------------------------------------
    {
        private static UnicastIPAddressInformation ip;

        //-------------------------------------------------------------
        public static string GetDirection()
        //-------------------------------------------------------------
        {
            var path = ConfigurationManager.AppSettings.Get("Kobberlan:GamesDirectory");
            if (string.IsNullOrEmpty(path))
            {
                path = Application.StartupPath + "\\Games";
            }

            return path;
        }

        //-------------------------------------------------------------
        public static byte[] ObjectToByteArray(Object obj)
        //-------------------------------------------------------------
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        //-------------------------------------------------------------
        public static Object ByteArrayToObject(byte[] arrBytes)
        //-------------------------------------------------------------
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        //-------------------------------------------------------------
        public static void SetHostIP(UnicastIPAddressInformation IP)
        //-------------------------------------------------------------
        {
            ip = IP;
        }

        //-------------------------------------------------------------
        public static IPAddress GetHostIP()
        //-------------------------------------------------------------
        {
            return ip.Address;
        }

        //-------------------------------------------------------------
        public static IPAddress GetHostIPBroadcastAddress()
        //-------------------------------------------------------------
        {
            return GetBroadcastAddress(ip.Address, ip.IPv4Mask);
        }

        //-------------------------------------------------------------
        public static IPAddress GetBroadcastAddress(IPAddress address, IPAddress mask)
        //-------------------------------------------------------------
        {
            uint ipAddress = BitConverter.ToUInt32(address.GetAddressBytes(), 0);
            uint ipMaskV4 = BitConverter.ToUInt32(mask.GetAddressBytes(), 0);
            uint broadCastIpAddress = ipAddress | ~ipMaskV4;

            return new IPAddress(BitConverter.GetBytes(broadCastIpAddress));
        }

        //-------------------------------------------------------------
        public static bool IsPortAvailable(int port)
        //-------------------------------------------------------------
        {
            bool isAvailable = true;

            // Evaluate current system tcp connections. This is the same information provided
            // by the netstat command line application, just in .Net strongly-typed object
            // form.  We will look through the list, and if our port we would like to use
            // in our TcpClient is occupied, we will set isAvailable to false.
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == port)
                {
                    isAvailable = false;
                    break;
                }
            }

            return isAvailable;
        }


        /*
        //-------------------------------------------------------------
        public static void AddPortExceptionToWindowsFireWall(int portToAdd)
        //-------------------------------------------------------------
        {
            INetFwOpenPorts ports;
            INetFwOpenPort port = GetInstance("INetOpenPort") as INetFwOpenPort;
            port.Port = portToAdd;
            port.Name = "KobberLan";
            port.Enabled = true; // enable the port

            Type NetFwMgrType = Type.GetTypeFromProgID("HNetCfg.FwMgr", false);
            INetFwMgr mgr = (INetFwMgr)Activator.CreateInstance(NetFwMgrType);

            ports = (INetFwOpenPorts)mgr.LocalPolicy.CurrentProfile.GloballyOpenPorts;
            ports.Add(port);
        }

        //-------------------------------------------------------------
        public static void AddApplicationExceptionToWindowsFireWall()
        //-------------------------------------------------------------
        {
            if(isAppFound(Application.ProductName) == false)
            {
                INetFwAuthorizedApplications applications;
                INetFwAuthorizedApplication application = GetInstance("INetAuthApp") as INetFwAuthorizedApplication;
                application.Name = Application.ProductName;
                application.ProcessImageFileName = Application.ExecutablePath;
                application.Enabled = true;

                //Add this application to AuthorizedApplications collection
                Type NetFwMgrType = Type.GetTypeFromProgID("HNetCfg.FwMgr", false);
                INetFwMgr mgr = (INetFwMgr)Activator.CreateInstance(NetFwMgrType);
                applications = (INetFwAuthorizedApplications)mgr.LocalPolicy.CurrentProfile.AuthorizedApplications;
                applications.Add(application);
            }
        }

        //-------------------------------------------------------------
        private static bool isAppFound(string appName)
        //-------------------------------------------------------------
        {
            bool boolResult = false;
            Type progID = null;
            INetFwMgr firewall = null;
            INetFwAuthorizedApplications apps = null;
            INetFwAuthorizedApplication app = null;
            try
            {
                progID = Type.GetTypeFromProgID("HNetCfg.FwMgr");
                firewall = Activator.CreateInstance(progID) as INetFwMgr;
                if (firewall.LocalPolicy.CurrentProfile.FirewallEnabled)
                {
                    apps = firewall.LocalPolicy.CurrentProfile.AuthorizedApplications;
                    IEnumerator appEnumerate = apps.GetEnumerator();
                    while ((appEnumerate.MoveNext()))
                    {
                        app = appEnumerate.Current as INetFwAuthorizedApplication;
                        if (app.Name == appName)
                        {
                            boolResult = true;
                            Log.Get().Write("Application is already authorized in firewall.", Log.LogType.Warning);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Get().Write(ex.Message, Log.LogType.Error);
            }
            finally
            {
                if (progID != null) progID = null;
                if (firewall != null) firewall = null;
                if (apps != null) apps = null;
                if (app != null) app = null;
            }
            return boolResult;
        }

        //-------------------------------------------------------------
        private static object GetInstance(string typeName)
        //-------------------------------------------------------------
        {
            Type tpResult = null;
            switch (typeName)
            {
                case "INetFwMgr":
                    tpResult = Type.GetTypeFromCLSID(new Guid("{304CE942-6E39-40D8-943A-B913C40C9CD4}"));
                    return Activator.CreateInstance(tpResult);
                case "INetAuthApp":
                    tpResult = Type.GetTypeFromCLSID(new Guid("{EC9846B3-2762-4A6B-A214-6ACB603462D2}"));
                    return Activator.CreateInstance(tpResult);
                case "INetOpenPort":
                    tpResult = Type.GetTypeFromCLSID(new Guid("{0CA545C6-37AD-4A6C-BF92-9F7610067EF5}"));
                    return Activator.CreateInstance(tpResult);
                default:
                    throw new Exception("Unknown type name: " + typeName);
            }
        }

        //-------------------------------------------------------------
        public static bool IsTCPPortWorking(int port)
        //-------------------------------------------------------------
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    Log.Get().Write("Trying to connect to local TCP port: " + port);
                    tcpClient.Connect("127.0.0.1", port);
                    Log.Get().Write("Success connecting to local TCP port: " + port);
                    tcpClient.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Get().Write("Failed to connect to local TCP port: " + port + " ,ex: " + ex, Log.LogType.Warning);
                    tcpClient.Close();
                    return false;
                }
            }
        }

        //-------------------------------------------------------------
        public static bool IsUDPPortWorking(int port, bool TrackerProtocolTest)
        //-------------------------------------------------------------
        {
            using (UdpClient udpClient = new UdpClient())
            {
                try
                {
                    byte[] data = { 80 }; // 'P'

                    if(TrackerProtocolTest)
                    {
                        // Client
                        //int64_t	connection_id	Must be initialized to 0x41727101980 in network byte order. This will identify the protocol.
                        //int32_t	action	0 for a connection request
                        //int32_t	transaction_id	Randomized by client.
                        data = new byte[16];
                        byte[] temp = BitConverter.GetBytes(0x41727101980).Reverse().ToArray();
                        byte[] temp2 = BitConverter.GetBytes(0);
                        byte[] temp3 = BitConverter.GetBytes(new Random().Next(0, 65535));
                        Array.Copy(temp, 0, data, 0, 8);
                        Array.Copy(temp2, 0, data, 8, 4);
                        Array.Copy(temp3, 0, data, 12, 4);

                        // Server response
                        //int32_t	action	Describes the type of packet, in this case it should be 0, for connect. If 3 (for error) see errors.
                        //int32_t	transaction_id	Must match the transaction_id sent from the client.
                        //int64_t	connection_id	A connection id, this is used when further information is exchanged with the tracker, to identify you. This connection id can be reused for multiple requests, but if it's cached for too long, it will not be valid anymore.
                    }

                    Log.Get().Write("Trying to connect to local UDP port: " + port);
                    udpClient.Connect("127.0.0.1", port);
                    udpClient.Send(data, data.Length);

                    var ClientEp = new IPEndPoint(IPAddress.Any, 0);
                    byte[] response = udpClient.Receive(ref ClientEp);

                    if(TrackerProtocolTest == false && response != null && response[0] == 66)
                    {
                        Log.Get().Write("Success connecting to to local UDP port: " + port);
                        udpClient.Close();
                        return true;
                    }
                    else if (TrackerProtocolTest == true && response != null && response[4] == data[12] && response[5] == data[13])
                    {
                        Log.Get().Write("Success connecting to to local UDP port: " + port);
                        udpClient.Close();
                        return true;
                    }
                    else //unknown response
                    {
                        udpClient.Close();
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    Log.Get().Write("Failed to connect to local UDP port: " + port + " ,ex: " + ex, Log.LogType.Warning);
                    udpClient.Close();
                    return false;
                }
            }
        }
        */

    }
}
