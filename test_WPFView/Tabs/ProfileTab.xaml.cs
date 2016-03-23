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
    /// Interaction logic for ProfileTab.xaml
    /// </summary>
    public partial class ProfileTab : TabItem
    {
        public ProfileTab()
        {
            InitializeComponent();

            //загрузить имя профиля при старте игры
            nameBox.Text = Statistics.leaderboard.profileName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TabControler.Change(GameTabs.MainMenu);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Scoreboard.UpdateProfile(Scoreboard.leaderboard.profileWins + 1, 0, 0); //тест связывания

            if (nameBox.Text != "")
            {
                if (nameBox.Text == Statistics.leaderboard.profileName)
                {
                    MessageBox.Show("Имя профиля осталось прежним");
                    return;
                }

                var mbRes = MessageBox.Show("Вы уверены что хотите сменить имя?", "Смена имени профиля", MessageBoxButton.YesNo);

                if (mbRes == MessageBoxResult.Yes)
                {
                    Statistics.leaderboard.profileName = nameBox.Text;
                    MessageBox.Show($"Вы поменяли имя профиля на {nameBox.Text}");
                }
                else
                {
                    nameBox.Text = Statistics.leaderboard.profileName;
                }

            }
        }

    }
}
