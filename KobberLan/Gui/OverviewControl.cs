using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using KobberLan.Code;

namespace KobberLan.Gui
{
    //-------------------------------------------------------------
    public partial class OverviewControl : UserControl
    //-------------------------------------------------------------
    {
        private KobberLan parentGui;

        //-------------------------------------------------------------
        public OverviewControl(KobberLan kobberLan)
        //-------------------------------------------------------------
        {
            InitializeComponent();
            parentGui = kobberLan;

            //-------------------------------------------------------------
            //Count number of games in folder
            //-------------------------------------------------------------
            string gamesFolder = Helper.GetDirection();
            int directoryCount = System.IO.Directory.GetDirectories(gamesFolder).Length;
            label_GamesFound.Text = directoryCount.ToString();
        }

        //-------------------------------------------------------------
        private void button_SuggestGame_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            ChooseGame chosseGame = new ChooseGame(parentGui);
            chosseGame.ShowDialog();
        }
    }
}
