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

using test_WPFView.Tabs;
using TicTacToeGame.Network;

namespace test_WPFView
{
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            tabControl.SelectedIndex = 2;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (Network.clientOn)           
                Network.SendDisconnect();

            Dispatcher.InvokeShutdown();            
        }

    }
}
