using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using TicTacToeGame.Network;

namespace test_WPFView
{
    /// <summary>
    /// Interaction logic for netMsgWindow.xaml
    /// </summary>
    public partial class netMsgWindow : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public bool wait = true;
        private bool host = false;
        public bool stopServer;

        public netMsgWindow()
        {
            InitializeComponent();

            Network.onCloseNetMsgWin += Network_onCloseNetMsgWin;
        }

        private void Network_onCloseNetMsgWin()
        {
            wait = false;
            stopServer = false;
            CloseWin();

        }

        public netMsgWindow(string _message, bool _host)
        {            
            InitializeComponent();
            messageLabel.Content = _message;
            host = _host;
            stopServer = true;

            Network.onCloseNetMsgWin += Network_onCloseNetMsgWin;
        }

        private void CloseWin()
        {
            wait = false;

            if (((string)messageLabel.Content).Equals("Ожидание решения второго игрока... "))
            {
                if (stopServer)
                {
                    Network.SendPlayAgainRefuse();
                    Network.StopServer();

                }
                TabControler.Change(GameTabs.MainMenu);

            }

            if (stopServer)
                Network.StopServer();

            this.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            CloseWin();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (wait)
            e.Cancel = true;
        }
    }
}
