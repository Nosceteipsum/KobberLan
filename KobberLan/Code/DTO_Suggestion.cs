using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;

namespace KobberLan.Code
{
    //-------------------------------------------------------------
    [Serializable]
    public class DTO_Suggestion
    //-------------------------------------------------------------
    {
        public string key; //foldername used as key (both in torrentengine and kobberlan)
        public SuggestionType type;
        public string title;
        public string author;
        public Image imageCover;
        public byte[] torrent;

        //From JSON config file
        public Image imageBig;
        public string description;
        public string startGame;
        public string startGameParams;
        public string startServer;
        public string startServerParams;
        public string version;
        public int maxPlayers;
    }

    //-------------------------------------------------------------
    [Serializable]
    public enum SuggestionType
    //-------------------------------------------------------------
    {
        HDD,
        Internet
    }

}
