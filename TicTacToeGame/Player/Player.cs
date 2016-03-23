namespace TicTacToeGame.Player
{
    /// <summary>
    /// Игрок – человек, играющий в игру
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Игровое имя – уникальное имя игрока в игре
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Количество побед
        /// </summary>
        public int WinGamesCounter  { get; private set; }
        /// <summary>
        /// Количество поражений
        /// </summary>
        public int LoseGamesCounter { get; private set; }
        /// <summary>
        /// Количество ничейных игр
        /// </summary>
        public int DrawGamesCounter { get; private set; }

        /// <summary>
        /// Конструктор класса Игрок
        /// </summary>
        public Player(string name)
        {
            Name = name;

            WinGamesCounter = 0;
            LoseGamesCounter = 0;
            DrawGamesCounter = 0;

        }

        /// <summary>
        /// Увеличить счетчик побед
        /// </summary>
        public void IncWinGamesCounter()
        {
            WinGamesCounter++;

        }

        /// <summary>
        /// Увеличить счетчик поражений
        /// </summary>
        public void IncLoseGamesCounter()
        {
            LoseGamesCounter++;

        }

        /// <summary>
        /// Увеличить счетчик ничейных игр
        /// </summary>
        public void IncDrawGamesCounter()
        {
            DrawGamesCounter++;

        }
        
    }
}
