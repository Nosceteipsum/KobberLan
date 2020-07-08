using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KobberLan.Gui
{
    //-------------------------------------------------------------
    public partial class AboutForm : Form
    //-------------------------------------------------------------
    {
        //-------------------------------------------------------------
        public AboutForm()
        //-------------------------------------------------------------
        {
            InitializeComponent();

            //-------------------------------------------------------------
            //Set version
            //-------------------------------------------------------------
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            DateTime buildDate = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
            string displayableVersion = $"{version} ({buildDate.ToString("yyyy-MM-dd")})";
            label_Version_Value.Text = displayableVersion;
        }

        //-------------------------------------------------------------
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //-------------------------------------------------------------
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://github.com/Nosceteipsum/KobberLan");
            Process.Start(sInfo);
        }

        //-------------------------------------------------------------
        private void pictureBox_Paypal_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://paypal.me/mightycodedragon");
            Process.Start(sInfo);
        }

        //-------------------------------------------------------------
        private void pictureBox_DonateBitcoin_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://commerce.coinbase.com/checkout/f7793326-52f9-40c1-8009-0d5a25d9ae01");
            Process.Start(sInfo);
        }
    }
}
