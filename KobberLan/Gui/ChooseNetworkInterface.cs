using KobberLan.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KobberLan.Gui
{
    //-------------------------------------------------------------
    public partial class ChooseNetworkInterface : Form
    //-------------------------------------------------------------
    {
        //-------------------------------------------------------------
        public ChooseNetworkInterface()
        //-------------------------------------------------------------
        {
            InitializeComponent();

            //Get all network interfaces
            foreach (NetworkInterface nic in GetActiveNetworkInterfaces())
            {
               foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
               {
                   if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                   {
                        string ipAdress = ip.Address.ToString();
                        //Selected first as default active network interface
                        if(String.IsNullOrEmpty(textBox_ActiveInterface.Text))
                        {
                            Helper.SetHostIP(ip);
                            textBox_ActiveInterface.Text = ipAdress;
                        }
                        listBox_Interfaces.Items.Add(ipAdress);
                   }
               }
            }

        }

        //-------------------------------------------------------------
        public List<NetworkInterface> GetActiveNetworkInterfaces()
        //-------------------------------------------------------------
        {
            return NetworkInterface.GetAllNetworkInterfaces().Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback).ToList();
        }

        //-------------------------------------------------------------
        private void button_ok_Click(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            Close();
        }

        //-------------------------------------------------------------
        private void listBox_Interfaces_SelectedIndexChanged(object sender, EventArgs e)
        //-------------------------------------------------------------
        {
            //Nothing selected
            if (listBox_Interfaces.SelectedItem == null)
                return;

            // Get the currently selected item in the ListBox.
            string curItem = listBox_Interfaces.SelectedItem.ToString();
            textBox_ActiveInterface.Text = curItem;

            //Get all network interfaces
            foreach (NetworkInterface nic in GetActiveNetworkInterfaces())
            {
               foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
               {
                   if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                   {
                        string ipAdress = ip.Address.ToString();
                        if(ipAdress.Equals(curItem))
                        {
                            //Set interface
                            Helper.SetHostIP(ip);
                            return;
                        }
                   }
               }
            }
        }
    }
}
