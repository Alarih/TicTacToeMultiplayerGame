using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;

namespace test_WPFView
{
    public enum GameTabs
    {
        MainMenu = 2,
        CreateGameMenu = 1,
        ConnectMenu = 3,
        Game = 0,
        Profile = 4,
        Leaderboard = 5,

    }

    public static class TabControler
    {
        /// <summary>
        /// Изменение закладки TabControl'а
        /// </summary>
        public static void Change(GameTabs _tab)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(delegate() {
                ((MainWindow)Application.Current.MainWindow).tabControl.SelectedIndex = (int)_tab;
            }));
            
        }

    }
}
