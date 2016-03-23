using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame.Statistics
{
    using TicTacToeGame.Player;

    public enum MatchResult
    {
        Win,
        Draw,
        Lose
    }

    /// <summary>
    /// Класс представляющий место в таблице рекордов
    /// </summary>
    [Serializable]
    public class LeaderboardPlace
    {
        public string Name { get; set; }
        public int WinGamesCounter { get; set; }
        public int LoseGamesCounter { get; set; }
        public int DrawGamesCounter { get; set; }

        public LeaderboardPlace(string _name, int _winGamesCounter, int _loseGamesCounter, int _drawGamesCounter)
        {
            Name = _name;
            WinGamesCounter = _winGamesCounter;
            LoseGamesCounter = _loseGamesCounter;
            DrawGamesCounter = _drawGamesCounter;

        }

        public LeaderboardPlace()
        {

        }
    }

    /// <summary>
    /// Класс представляющий таблицу рекордов
    /// </summary>
    [Serializable]
    public class Leaderboard : INotifyPropertyChanged
    {     
        public string profileName { get; set; }

        private int profileWins_;
        private int profileLoses_;
        private int profileDraws_;

        public int profileWins { get { return profileWins_; } private set { profileWins_ = value; OnPropertyChanged("profileWins"); } }
        public int profileLoses { get { return profileLoses_; } private set { profileLoses_ = value; OnPropertyChanged("ProfileLoses"); } }
        public int profileDraws { get { return profileDraws_; } private set { profileDraws_ = value; OnPropertyChanged("ProfileDraws"); } }

        public ObservableCollection<LeaderboardPlace> leaderTable { get; private set; } = new ObservableCollection<LeaderboardPlace>();
        //public ObservableCollection<LeaderboardPlace> leaderTable { get { return leaderTable_; } private set { leaderTable_ = value; OnPropertyChanged("leaderTable"); } }


        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }

        }       

        public void AddOrUpdate(LeaderboardPlace _place, MatchResult _res)
        {
            LeaderboardPlace sameNamePlace = leaderTable.FirstOrDefault(x => x.Name == _place.Name);

            if (sameNamePlace == null)
            {
                leaderTable.Add(_place);
            }
            else
            {
                switch (_res)
                {
                    case MatchResult.Win:
                        sameNamePlace.WinGamesCounter++;
                        break;
                    case MatchResult.Draw:
                        sameNamePlace.DrawGamesCounter++;
                        break;
                    case MatchResult.Lose:
                        sameNamePlace.LoseGamesCounter++;
                        break;
                    default:
                        break;
                }

                //OnPropertyChanged("leaderTable");

            }
        }

        /// <summary>
        /// Обновление очков в профиле игрока
        /// </summary>
        public void UpdateProfileScores(int _win, int _lose, int _draw)
        {
            profileWins += _win;
            profileLoses += _lose;
            profileDraws += _draw;

        }

        public Leaderboard(string _profileName)
        {
            leaderTable = new ObservableCollection<LeaderboardPlace>();
            profileName = _profileName;

            profileWins = 0;
            profileLoses = 0;
            profileDraws = 0;

        }
    }

    public static class Statistics
    {
        private const string DEFAULT_PLAYER_NAME = "Player";

        private const string SCOREBOARD_NAME = "/scores.bin";

        /// <summary>
        /// Объект таблицы рекордов
        /// </summary>
        public static Leaderboard leaderboard { get; private set; }

        /// <summary>
        /// Сохранить очки и имя игрока в таблицу рекордов
        /// </summary>
        public static void AddOrUpdate(Player _player, MatchResult _res)
        {
            if (leaderboard != null)
            {
                LeaderboardPlace place = new LeaderboardPlace(_player.Name, _player.WinGamesCounter, _player.LoseGamesCounter, _player.DrawGamesCounter);
                leaderboard.AddOrUpdate(place, _res);
            }
        }

        /// <summary>
        /// Сохранение текущей таблицы рекордов в файл
        /// </summary>
        public static void Save()
        {
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + SCOREBOARD_NAME;

            using (Stream stream = File.Open(filePath, FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, leaderboard);
            }
        }

        /// <summary>
        /// Загрузка текущей таблицы рекордов из файла
        /// </summary>
        public static void Load()
        {
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + SCOREBOARD_NAME;

            try
            {
                using (Stream stream = File.Open(filePath, FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    leaderboard = (Leaderboard)binaryFormatter.Deserialize(stream);
                }
            }
            catch (FileNotFoundException)
            {
                //если файл с таблицей рекордов не найден, создается новая, пустая таблица рекордов и случайное имя игрока
                Random r = new Random();
                leaderboard = new Leaderboard(DEFAULT_PLAYER_NAME + r.Next(1994, 2116).ToString());
            }

        }
    }
}
