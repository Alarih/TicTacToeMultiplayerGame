namespace TicTacToeGame.GameMatch
{
    using Player;
    using GameField;
    using Statistics;

    public enum PlayerType
    {
        Server,
        Client,
        /// <summary>
        /// Используется для проверки ничьи
        /// </summary>
        None
    }

    /// <summary>
    /// Игровой матч - сеанс текущей игры.
    /// </summary>
    public class GameMatch
    {
        #region Константы класса
        private const int DefaultPointsToWin = 3;

        private const int DefaultCharactersForLine = 3;
        #endregion

        /// <summary>
        /// Состояние очереди - текущая очередность хода (по умолчанию ход у игрока-сервера).
        /// </summary>
        public PlayerType QueueStatus { get; private set; } = PlayerType.Server;

        /// <summary>
        /// Количество символов для выстраивания линии (значение по умолчанию - см. "Константы класса").
        /// </summary>
        public int CharactersForLine { get; set; } = DefaultCharactersForLine;

        /// <summary>
        /// Количество очков для победы (значение по умолчанию - см. "Константы класса").
        /// </summary>
        public int PointsToWin { get; set; } = DefaultPointsToWin;

        /// <summary>
        /// Игрок сервер - игрок, создающий игровой сеанс.
        /// </summary>
        public Player serverPlayer { get; private set; }

        /// <summary>
        /// Игрок клиент - игрок, подключающийся к игровому матчу.
        /// </summary>
        public Player clientPlayer { get; private set; }

        /// <summary>
        /// Игровое поле
        /// </summary>
        public Field gameField { get; set; }

        /// <summary>
        /// Количество очков игрока-сервера.
        /// </summary>
        public int ServerScore { get; private set; } = 0;

        /// <summary>
        /// Количество очков игрока-клиента.
        /// </summary>
        public int ClientScore { get; private set; } = 0;

        /// <summary>
        /// Переменная для подсчета оставшихся свободных клеток
        /// </summary>
        public int freeCellsCount;

        /// <summary>
        /// Создает объект GameMatch и два объекта Player с указанием их имен.
        /// </summary>
        public GameMatch(string serverPlayerName, string clientPlayerName)
        {
            InitPlayers(serverPlayerName, clientPlayerName);
        }

        /// <summary>
        /// Создает объект GameMatch с указанием количества символов и очков для победы, также создает два объекта Player с указанием их имен.
        /// </summary>
        public GameMatch(string serverPlayerName, string clientPlayerName, int charactersForLine, int pointsToWin)
        {
            InitPlayers(serverPlayerName, clientPlayerName);

            this.CharactersForLine = charactersForLine;
            this.PointsToWin = pointsToWin;
        }

        /// <summary>
        /// Создание двух объектов, представляющих игроков.
        /// </summary>
        private void InitPlayers(string serverPlayerName, string clientPlayerName)
        {
            this.serverPlayer = new Player(serverPlayerName);
            this.clientPlayer = new Player(clientPlayerName);
        }

        /// <summary>
        /// Метод увеличивает количество очков игрока-сервера
        /// </summary>
        public void IncServerScore()
        {
            this.ServerScore++;
        }

        /// <summary>
        /// Метод увеличивает количество очков игрока-клиента
        /// </summary>
        public void IncClientScore()
        {
            this.ClientScore++;
        }

        /// <summary>
        /// Метод добавляет победу игроку-серверу, и обновляет статистику
        /// </summary>
        public void ServerWinGame()
        {
            serverPlayer.IncWinGamesCounter();
            Statistics.AddOrUpdate(serverPlayer, MatchResult.Win);
            Statistics.leaderboard.UpdateProfileScores(serverPlayer.WinGamesCounter, serverPlayer.LoseGamesCounter, serverPlayer.DrawGamesCounter);

        }

        /// <summary>
        /// Метод добавляет победу игроку-клиенту, и обновляет статистику
        /// </summary>
        public void ClientWinGame()
        {
            clientPlayer.IncWinGamesCounter();
            Statistics.AddOrUpdate(clientPlayer, MatchResult.Win);
            Statistics.leaderboard.UpdateProfileScores(clientPlayer.WinGamesCounter, clientPlayer.LoseGamesCounter, clientPlayer.DrawGamesCounter);

        }

        /// <summary>
        /// Метод изменяет текущую очередность хода.
        /// </summary>
        public void ChangeQueueStatus()
        {
            this.QueueStatus = (this.QueueStatus == PlayerType.Server) ? PlayerType.Client : PlayerType.Server;
        }

        /// <summary>
        /// Задать очередность хода по умолчанию
        /// </summary>
        public void SetDefaultQueueStatus()
        {
            this.QueueStatus = PlayerType.Server;
        }

        /// <summary>
        /// Проверка абсолютной победы в матче
        /// </summary>
        public PlayerType? CheckEndGame()
        {           
            if (freeCellsCount > 0)
            {
                //Проверка победы игрока-сервера
                if (ServerScore == PointsToWin)
                {
                    return PlayerType.Server;

                }

                //Проверка победы игрока-клиента
                if (ClientScore == PointsToWin)
                {
                    return PlayerType.Client;

                }

                //если никто не победил, вернуть null
                return null;
            }
            else //Если клеток не осталось
            {
                return CheckEndGameComparing();
            }
        }

        /// <summary>
        /// Сравнительная проверка победы в матче 
        /// </summary>
        /// <returns>вернет победившего игрока по очкам, либо null, если очки игроков равны</returns>
        private PlayerType? CheckEndGameComparing()
        {
            if (ServerScore > ClientScore)
            {
                //победа игрока-сервера
                return PlayerType.Server;

            }
            else
            if (ServerScore < ClientScore)
            {
                //победа игрока-клиента
                return PlayerType.Client;

            }
            else
            if (ClientScore == ServerScore)
            {
                // ничья
                return PlayerType.None;

            }                           

            return null;
        }

        /// <summary>
        /// очистка очков
        /// </summary>
        public void ClearScores()
        {
            ServerScore = 0;
            ClientScore = 0;

        }

    }
}
