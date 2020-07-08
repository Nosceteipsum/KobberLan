using KobberLan.Code;
using System;
using System.Windows.Forms;
using WK.Libraries.BetterFolderBrowserNS;

namespace KobberLan.Gui
{
    //-------------------------------------------------------------
    public partial class SuggestInternetGame : Form
    //-------------------------------------------------------------
    {
        private KobberLan kobberLan;
        private bool closed;

        //-------------------------------------------------------------
        public SuggestInternetGame(KobberLan parent)
        //-------------------------------------------------------------
        {
            closed = false;
            kobberLan = parent;
            InitializeComponent();
        }

        //-------------------------------------------------------------
        private void button_Cancel_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            closed = true;
            this.Close();
        }

        //-------------------------------------------------------------
        private void button_Clear_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            textBox_Title.Clear();
            textBox_Description.Clear();
            textBox_GameUrl.Clear();
            textBox_ImageBig.Clear();
            textBox_ImageCover.Clear();
            textBox_Versions.Clear();
            textBox_MaxPlayers.Clear();
        }

        //-------------------------------------------------------------
        private void button_Accept_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            //Title must not be empty
            if(string.IsNullOrEmpty(textBox_Title.Text))
            {
                MessageBox.Show("Title must not be empty","Error");
                return;
            }

            kobberLan.Invoke(new Action(() =>
            {
                kobberLan.SuggestInternetGame(this);
            }));

            closed = true;
            this.Close();
        }

        //-------------------------------------------------------------
        public bool GetClosedStatus()
        //-------------------------------------------------------------
        {
            return closed;
        }

        //-------------------------------------------------------------
        public string GetDescription()
        //-------------------------------------------------------------
        {
            return textBox_Description.Text;
        }
        //-------------------------------------------------------------
        public string GetImageBig()
        //-------------------------------------------------------------
        {
            return textBox_ImageBig.Text;
        }
        //-------------------------------------------------------------
        public string GetImageCover()
        //-------------------------------------------------------------
        {
            return textBox_ImageCover.Text;
        }
        //-------------------------------------------------------------
        public string GetTitle()
        //-------------------------------------------------------------
        {
            return textBox_Title.Text;
        }
        //-------------------------------------------------------------
        public string GetVersion()
        //-------------------------------------------------------------
        {
            return textBox_Versions.Text;
        }

        //-------------------------------------------------------------
        public int GetMaxPlayers()
        //-------------------------------------------------------------
        {
            return String.IsNullOrEmpty(textBox_MaxPlayers.Text) ? 0 : Convert.ToInt32(textBox_MaxPlayers.Text);
        }

        //-------------------------------------------------------------
        public string GetStartLocation()
        //-------------------------------------------------------------
        {
            return textBox_GameUrl.Text;
        }

        //-------------------------------------------------------------
        private void button_Load_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON file|*.json";
            openFileDialog.Title = "Save JSON config File";
            openFileDialog.InitialDirectory = Helper.GetDirection();
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                DTO_SuggestionInternet suggestion = DTO_SuggestionInternet.LoadData(openFileDialog.FileName);

                textBox_Description.Text = suggestion.description;
                textBox_ImageCover.Text = suggestion.imageCover;
                textBox_ImageBig.Text = suggestion.imageBig;
                textBox_Title.Text = suggestion.title;
                textBox_GameUrl.Text = suggestion.url;
                textBox_Versions.Text = suggestion.version;
                textBox_MaxPlayers.Text = suggestion.maxPlayers.ToString();
            }

        }

        //-------------------------------------------------------------
        private void button_Save_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            DTO_SuggestionInternet suggestion = new DTO_SuggestionInternet() { description = textBox_Description.Text, imageCover = textBox_ImageCover.Text, maxPlayers = string.IsNullOrEmpty(textBox_MaxPlayers.Text) ? 0 : Convert.ToInt32(textBox_MaxPlayers.Text), imageBig = textBox_ImageBig.Text, title = textBox_Title.Text, url = textBox_GameUrl.Text, version = textBox_Versions.Text };

            //Choose folder to save
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON file|*.json";
            saveFileDialog.Title = "Save JSON config File";
            saveFileDialog.InitialDirectory = Helper.GetDirection();
            saveFileDialog.FileName = suggestion.title;
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                DTO_SuggestionInternet.SaveData(saveFileDialog.FileName, suggestion);
            }

        }

        //-------------------------------------------------------------
        private void SuggestInternetGame_FormClosed(object sender, FormClosedEventArgs e)
        //-------------------------------------------------------------
        {
            closed = true;
        }
    }
}
