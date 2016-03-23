using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace TicTacToeInterface
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Server server;
        Client client;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            server = new Server();
            new Thread(new ParameterizedThreadStart(server.Receive)).Start(grid1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            client = new Client("127.0.0.1");
            new Thread(new ParameterizedThreadStart(client.ThreadSend)).Start("test");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (server != null)
            {
                server.StopReceive();
            }
        }
    }
}
