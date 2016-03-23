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
    /// Interaction logic for LeaderboardTab.xaml
    /// </summary>
    public partial class LeaderboardTab : TabItem
    {
        public LeaderboardTab()
        {
            InitializeComponent();

            CollectionViewSource itemCollectionViewSource;
            itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
            itemCollectionViewSource.Source =  Statistics.leaderboard.leaderTable;
        }

        private void menuBtn_Click(object sender, RoutedEventArgs e)
        {
            TabControler.Change(GameTabs.MainMenu);
        }

    }
}
