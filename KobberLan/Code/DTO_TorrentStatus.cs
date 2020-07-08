using System;
using System.Net;

namespace KobberLan.Code
{
    //-------------------------------------------------------------
    [Serializable]
    public class DTO_TorrentStatus
    //-------------------------------------------------------------
    {
        public IPAddress address;
        public string key;
        public TorrentStatusType status;
    }

    //-------------------------------------------------------------
    [Serializable]
    public enum TorrentStatusType
    //-------------------------------------------------------------
    {
        Starting,
        Finished,
        Remove
    }

}
