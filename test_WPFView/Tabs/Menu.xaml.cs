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

using TicTacToeGame.Statistics;

namespace test_WPFView.Tabs
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : TabItem
    {
        public Menu()
        {
            InitializeComponent();

            //загрузить таблицу рекордов при старте игры
            Statistics.Load();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            TabControler.Change(GameTabs.CreateGameMenu);
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

            //сохранить таблицу рекордов при выходе
            Statistics.Save();
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            TabControler.Change(GameTabs.ConnectMenu);                      
        }

        private void profileButton_Click(object sender, RoutedEventArgs e)
        {
            TabControler.Change(GameTabs.Profile);

           // ProfileTab.UpdateScoreLabels();
        }

        private void leaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            TabControler.Change(GameTabs.Leaderboard);

        }

        //private void connectButton_Click(object sender, RoutedEventArgs e)
        //{
        //    TabControler.Change(GameTabs.ConnectMenu);                      
        //}
    }
}
