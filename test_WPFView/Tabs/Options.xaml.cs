using System;
using System.Collections.Generic;
using System.Linq;
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
    using System.Windows.Threading;
    using TicTacToeGame;
    using TicTacToeGame.GameField;
    using TicTacToeGame.GameMatch;

    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : TabItem
    {
        public Options()
        {
            InitializeComponent();

            foreach (int item in ViewData.crossCount.Keys)
            {
                fieldSizeBox.Items.Add(item);
            }

        }

        private void fieldSizeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fieldSizeBox.SelectedValue != null)
            {
                crossCountBox.Items.Clear();
                crossCountBox.Text = "";
                winBox.Items.Clear();
                winBox.Text = "";

                var item = fieldSizeBox.SelectedValue;
                List<int> crossList = ViewData.crossCount[Convert.ToInt32(item)];

                foreach (int crossCount in crossList)
                {
                    crossCountBox.Items.Add(crossCount);
                }

                List<int> winList = ViewData.winCount[Convert.ToInt32(item)];

                foreach (int winCount in winList)
                {
                    winBox.Items.Add(winCount);
                }

                createGameBtn.IsEnabled = false;
            }

            crossCountBox.IsEnabled = true;

        }

        private void crossCountBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (winBox.Text != "" && serverPortBox.Text != "")
            {
                createGameBtn.IsEnabled = true;
            }

        }

        private void winBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (crossCountBox.Text != "" && serverPortBox.Text != "")
            {
                createGameBtn.IsEnabled = true;
            }
        }

        private void createGameBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = ((MainWindow)Application.Current.MainWindow);

            int fieldSize = Convert.ToInt32(fieldSizeBox.Text);
            int crossCount = Convert.ToInt32(crossCountBox.Text);
            int winCount = Convert.ToInt32(winBox.Text);
            
            Network.StartServerSetProps(Convert.ToInt32(serverPortBox.Text), fieldSize, crossCount, winCount);

            netMsgWindow netWindow = new netMsgWindow("Ожидание подключения второго игрока...", true);
            netWindow.ShowDialog();       
        }

        //swap ports
        private void swap_Click(object sender, RoutedEventArgs e)
        {
            string temp = clientPortBox.Text;
            clientPortBox.Text = serverPortBox.Text;
            serverPortBox.Text = temp;
        }

        private void menuBtn_Click(object sender, RoutedEventArgs e)
        {
            TabControler.Change(GameTabs.MainMenu);
        }

        private void myIpBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Network.GetLocalIPAddress());
        }

        private void serverPortBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (serverPortBox.Text != "" && fieldSizeBox.Text != "" && crossCountBox.Text != "" && winBox.Text != "")
            {
                createGameBtn.IsEnabled = true;
            }

            ushort tryPort = 0;
            bool isPortInt16 = ushort.TryParse(serverPortBox.Text, out tryPort);
            
            if (!isPortInt16) { serverPortBox.Clear(); }

            if (serverPortBox.Text == "") createGameBtn.IsEnabled = false;
        }
    }
}
