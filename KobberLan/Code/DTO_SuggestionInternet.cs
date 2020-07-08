using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;

namespace KobberLan.Code
{
    //-------------------------------------------------------------
    [Serializable]
    public class DTO_SuggestionInternet
    //-------------------------------------------------------------
    {
        public string title;
        public string imageCover;
        public string imageBig;
        public string description;
        public string url;
        public string version;
        public int maxPlayers;

        //Save/Load settings to json file
        //-------------------------------------------------------------
        public static void SaveData(string path, DTO_SuggestionInternet suggestion)
        //-------------------------------------------------------------
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(suggestion));
        }

        //-------------------------------------------------------------
        public static DTO_SuggestionInternet LoadData(string path)
        //-------------------------------------------------------------
        {
            //Check for settings file
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<DTO_SuggestionInternet>(File.ReadAllText(path));
            }

            return null;
        }

    }

}
