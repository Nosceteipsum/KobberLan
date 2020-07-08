using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KobberLan.Gui
{
    //-------------------------------------------------------------
    public partial class ChooseGameControl : UserControl
    //-------------------------------------------------------------
    {
        private ChooseGame parentForm;
        private string gameFolder;

        //-------------------------------------------------------------
        public ChooseGameControl(ChooseGame chooseGame, Image image, string folder)
        //-------------------------------------------------------------
        {
            InitializeComponent();
            parentForm = chooseGame;
            gameFolder = folder;
            pictureBox_Game.Image = image;
        }

        //-------------------------------------------------------------
        private void pictureBox_Game_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            parentForm.GameChosen(gameFolder);
        }
    }
}
