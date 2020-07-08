using System;

namespace KobberLan.Code
{
    //-------------------------------------------------------------
    [Serializable]
    public class DTO_Torrent
    //-------------------------------------------------------------
    {
        public byte[] torrent;
        public string key; //Used to match suggestedgame
    }
}
