using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KobberLan.Code
{
    //-------------------------------------------------------------
    [Serializable]
    class DTO_SuggestionSettings
    //-------------------------------------------------------------
    {
        public string title             = "";
        public string description       = "";
        public string image             = "";
        public string startGame         = "";
        public string startGameParams   = "";
        public string startServer       = "";
        public string startServerParams = "";
        public string version           = "";
        public int? maxPlayers = 0;

        //-------------------------------------------------------------
        public static void LoadData(string path,ref DTO_Suggestion suggestion)
        //-------------------------------------------------------------
        {
            //Check for settings file
            string settingPath = path + "\\_kobberlan.config";
            if (File.Exists(settingPath))
            {
                DTO_SuggestionSettings settings = JsonConvert.DeserializeObject<DTO_SuggestionSettings>(File.ReadAllText(settingPath));

                //Fill data to suggestion object
                suggestion.description = settings.description;
                suggestion.startGame = settings.startGame;
                suggestion.startGameParams = settings.startGameParams;
                suggestion.startServer = settings.startServer;
                suggestion.startServerParams = settings.startServerParams;
                suggestion.version = settings.version;
                suggestion.title = settings.title;
                suggestion.maxPlayers = settings.maxPlayers == null ? 0 : settings.maxPlayers.Value;

                //Load big image if available
                string imagePath = path + "\\" + settings.image;
                if (File.Exists(imagePath))
                {
                    suggestion.imageBig = Image.FromFile(imagePath);
                }
                else
                {
                    suggestion.imageBig = suggestion.imageCover; //Default image if missing
                }
            }
            else
            {
                suggestion.imageBig = suggestion.imageCover; //Default image if missing
            }
        }

    }
}
