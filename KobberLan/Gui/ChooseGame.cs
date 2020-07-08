using KobberLan.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//-------------------------------------------------------------
namespace KobberLan.Gui
//-------------------------------------------------------------
{
    //-------------------------------------------------------------
    public partial class ChooseGame : Form
    //-------------------------------------------------------------
    {
        private KobberLan parentGui;

        //-------------------------------------------------------------
        public ChooseGame(KobberLan kobberLan)
        //-------------------------------------------------------------
        {
            InitializeComponent();
            parentGui = kobberLan;

            //-------------------------------------------------------------
            //Show all possible games
            //-------------------------------------------------------------
            string gamesFolder = Helper.GetDirection();
            string[] directories = System.IO.Directory.GetDirectories(gamesFolder);
            foreach(var folder in directories)
            {
                AddGame(folder);
            }

        }

        //-------------------------------------------------------------
        public void GameChosen(string folder)
        //-------------------------------------------------------------
        {
            parentGui.SuggestGameLoadFromPath(folder);
            Close();
        }

        //-------------------------------------------------------------
        private void AddGame(string folder)
        //-------------------------------------------------------------
        {
            //Check for cover image
            Image image = Properties.Resources.no_cover;
            folder = Path.GetFullPath(folder);
            string folderImage = folder + "\\_kobberlan.jpg";
            if (File.Exists(folderImage))
            {
                image = Image.FromFile(folderImage);
            }

            ChooseGameControl gameControl = new ChooseGameControl(this, image, folder); 
            flowLayoutPanel_Games.Controls.Add(gameControl);
        }
    }
}
