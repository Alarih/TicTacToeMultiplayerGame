using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TicTacToeGame.Network;

namespace test_WPFView.Tabs
{
    /// <summary>
    /// Interaction logic for ConnectMenu.xaml
    /// </summary>
    public partial class ConnectMenu : TabItem
    {
        public ConnectMenu()
        {
            InitializeComponent();
        }

        private void menuBtn_Click(object sender, RoutedEventArgs e)
        {
            TabControler.Change(GameTabs.MainMenu);
        }

        private void connectBtn_Click(object sender, RoutedEventArgs e)
        {
            
            byte[] ipByte = new byte[4] { 0, 0, 0, 0 };
            IPAddress tryIP = new IPAddress(ipByte);
            bool isIP = IPAddress.TryParse(IPBox.Text, out tryIP);

            if (!isIP)
            {
                MessageBox.Show("Введите корректный IP адрес");
                IPBox.Clear();
                return;
            }

            Network.StartServer(Convert.ToInt32(portBox.Text));       
            Network.StartClient(IPBox.Text, Convert.ToInt32(portBox.Text));           
            Network.SendClientIP(Network.GetLocalIPAddress());
            Network.SendName();

            netMsgWindow netWindow = new netMsgWindow("Ожидание ответа от сервера...", false);
            netWindow.ShowDialog();
        }

        private void portBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (portBox.Text != "" && connectBtn != null && IPBox.Text != "")
            {
                connectBtn.IsEnabled = true;
            }

            ushort tryPort = 0;
            bool isPortInt16 = ushort.TryParse(portBox.Text, out tryPort);

            if (!isPortInt16) { portBox.Clear(); }

            if (portBox.Text == "" || IPBox.Text == "") connectBtn.IsEnabled = false;
        }

        private void IPBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
