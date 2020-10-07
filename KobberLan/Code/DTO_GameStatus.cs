using System;
using System.Net;

namespace KobberLan.Code
{
    //-------------------------------------------------------------
    [Serializable]
    public class DTO_GameStatus
    //-------------------------------------------------------------
    {
        public IPAddress address;
        public string key;
        public bool playing;
    }

}
