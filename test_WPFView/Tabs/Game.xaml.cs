using System;
using System.Collections.Generic;
using System.Threading;
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
using TicTacToeGame.Network;

namespace test_WPFView.Tabs
{
    using System.Windows.Controls.Primitives;
    using System.Windows.Threading;
    using TicTacToeGame;
    using TicTacToeGame.GameField;
    using TicTacToeGame.GameMatch;

    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : TabItem
    {
        public static GameMatch gameMatch;

        public static volatile Grid gameFieldGrid;

        private double imgSize;      

        /// <summary>
        /// Окно информации
        /// </summary>
        private InfoWin infoWin = new InfoWin();

        public Game()
        {
            InitializeComponent();

            Canvas.SetZIndex(mainCanvas, 2);

            //Подписка на сетевые события
            Network.onStartGame += Network_onStartGame;
            Network.onServerImageClick += Network_onServerImageClick;
            Network.onExitToMainMenu += Network_onExitToMainMenu;
            Network.onIncServerPlayerPoint += Network_onIncServerPlayerPoint;
            Network.onIncClientPlayerPoint += Network_onIncClientPlayerPoint;
        }

        private void Network_onIncClientPlayerPoint()
        {
            gameMatch.ClientWinGame();
        }

        private void Network_onIncServerPlayerPoint()
        {
            gameMatch.ServerWinGame();
        }

        private void Network_onExitToMainMenu()
        {
            TabControler.Change(GameTabs.MainMenu);
        }

        private void Network_onServerImageClick(int x, int y)
        {
            ClickOnImageByCoords(x, y);
        }

        private void Network_onStartGame(int _fieldSize, int _crossCount, int _pointsToWin, bool _host)
        {
            СreateGame(_fieldSize, _crossCount, _pointsToWin, _host);
            TabControler.Change(GameTabs.Game);
        }

        #region Отрисовка игрового поля

        /// <summary>
        /// добавление столбцов и колонок в созданную сетку
        /// </summary>
        private void placeRowsAndCols(int _fieldSize, Grid _gameFieldGrid)
        {
            ColumnDefinition colDef = new ColumnDefinition();
            colDef.Width = new GridLength(1, GridUnitType.Star);

            _gameFieldGrid.ColumnDefinitions.Add(colDef);


            for (int k = 0; k < _fieldSize; k++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(1, GridUnitType.Star);

                _gameFieldGrid.RowDefinitions.Add(rowDef);
            }

        }

        /// <summary>
        /// создание картинки
        /// </summary>
        private void createImage(int _x, int _y)
        {
            TicTacImage img = new TicTacImage();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"/test_WPFView;component/Images/emptyCell.png", UriKind.Relative);
            bi.EndInit();

            img.Content = new Image()
            {
                Source = bi,
                VerticalAlignment = VerticalAlignment.Stretch,
                Stretch = Stretch.Fill
            };

            img.X = _x;
            img.Y = _y;
            img.Click += ImageClick;

            //связка картинки с сеткой
            Grid.SetColumn(img, _x);
            Grid.SetRow(img, _y);
            gameFieldGrid.Children.Add(img);

        }

        /// <summary>
        /// Отрисовка игрового поля
        /// </summary>
        private void DrawField(int fieldSize)
        {
            CleanRectangles();

            //создание сетки для игрового поля
            gameFieldGrid = new Grid();
            gameFieldGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            gameFieldGrid.VerticalAlignment = VerticalAlignment.Stretch;

            //добавление столбцов и колонок в созданную сетку
            for (int i = 0; i < fieldSize; i++)
            {
                placeRowsAndCols(fieldSize, gameFieldGrid);

            }

            //добавление картинок в каждую клетку сетки для игрового поля
            for (int i = 0; i < fieldSize; i++)
            {
                for (int k = 0; k < fieldSize; k++)
                {
                    createImage(i, k);                    
                }

            }

            //связка главной сетки и созданной игровой сетки
            Grid.SetColumn(gameFieldGrid, 0);
            Grid.SetRow(gameFieldGrid, 0);

            mainGrid.Children.Add(gameFieldGrid);

        }

        #endregion

        private void ImageClick(object sender, RoutedEventArgs e)
        {
            TicTacImage image = (TicTacImage)sender;
            performImageClick(image);

        }

        private void ClickOnImageByCoords(int x, int y)
        {
            System.Collections.Generic.IEnumerable<TicTacImage> resultCollection = null;

            try
            {
                //поиск картинки по координатам
                resultCollection = from image in gameFieldGrid.Children.OfType<TicTacImage>()
                                   where (image.X == x && image.Y == y)
                                   select image;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (resultCollection == null)
            {
                throw new Exception("Кнопка по полученным координатам не найдена :(");
            }
            else
            {
                resultCollection.First().sended = true;
                imgclick(resultCollection.First());
            }

        }

        private void imgclick(TicTacImage img)
        {
            img.RaiseEvent((new RoutedEventArgs(ButtonBase.ClickEvent)));
        }

        public void performImageClick(TicTacImage image)
        {
            //заносить в массив крестик либо нолик
            CellType cellType = CellType.Empty;

            //проверка очередности хода
            if (gameMatch.QueueStatus == PlayerType.Server)
            {
                if (!Network.iAmHost && !image.sended) return;
                cellType = CellType.Cross;
                //Network.ChangeQueue();
            }
            if (gameMatch.QueueStatus == PlayerType.Client)
            {
                if (Network.iAmHost && !image.sended) return;
                cellType = CellType.Zero;
               // Network.ChangeQueue();
            }


            //попробовать отметить клетку в массиве игрового поля
            if (!gameMatch.gameField.MarkTheFieldPlace(image.X, image.Y, cellType))
            {
                infoWin.SetText("Поле уже занято!");
                infoWin.ShowDialog();

            }
            else
            {
                //отобразить символ
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                if (gameMatch.QueueStatus == PlayerType.Server)
                    bi.UriSource = new Uri(@"/test_WPFView;component/Images/crossCell.png", UriKind.Relative);
                else
                    bi.UriSource = new Uri(@"/test_WPFView;component/Images/zeroCell.png", UriKind.Relative);
                bi.EndInit();
                image.Content = new Image()
                {
                    Source = bi,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Stretch = Stretch.Fill
                };

                //уменьшить количество свободных клеток
                gameMatch.freeCellsCount--;

                //проверить зачеркивание                
                CheckCrossing(image, cellType);

                //поменять очередность хода
                ChangeQueue();

                if (image.sended == true)
                {
                    image.sended = false;
                }
                else
                {
                    Network.SendCoords(image.X, image.Y);

                }

                EndGameAnnouncing();
            }
        }

        /// <summary>
        /// Проверка зачеркиваний
        /// </summary>
        private void CheckCrossing(TicTacImage image, CellType cellType)
        {
            Crossing.CrossingResult crossRes = Crossing.CheckLines(new CellCoords() { X = image.X, Y = image.Y }, cellType, gameMatch.gameField, gameMatch.CharactersForLine);
            if (crossRes.direction != Crossing.Directions.Fail)
            {
                //get image size 
                var imgWidth = from img in gameFieldGrid.Children.OfType<TicTacImage>()
                               select img.ActualWidth;
                imgSize = imgWidth.First();

                Rectangle rect = RectDrawer.addRect(imgSize, gameMatch.CharactersForLine, crossRes);
                mainCanvas.Children.Add(rect);

                if (gameMatch.QueueStatus == PlayerType.Server)
                {
                    gameMatch.IncServerScore();
                }
                else
                {
                    gameMatch.IncClientScore();
                }

                scoresLabel.Content = ViewData.scoreLabel + gameMatch.ServerScore + ":" + gameMatch.ClientScore;
            
            }
        }

        /// <summary>
        /// Смена игровой очереди
        /// </summary>
        private void ChangeQueue()
        {          
            gameMatch.ChangeQueueStatus();
            ChangeTurnLabel();

        }

        /// <summary>
        /// Проверка победы
        /// </summary>
        private void EndGameAnnouncing()
        {
            PlayerType? endGame = gameMatch.CheckEndGame();

            if (endGame != null)
            {
                //Проверка победы игрока-сервера
                if (endGame == PlayerType.Server)
                {
                    AnnounceServerWin();
                }

                //Проверка победы игрока-клиента
                if (endGame == PlayerType.Client)
                {
                    AnnounceClientWin();
                }

                //Проверка на ничью
                if (endGame == PlayerType.None)
                {
                    AnnounceStandoff();                    
                }

                ReplayOffer();
            }
            

        }

        /// <summary>
        /// Объявить победу игрока-сервера
        /// </summary>
        private void AnnounceServerWin()
        {
            gameMatch.serverPlayer.IncWinGamesCounter();
            gameMatch.clientPlayer.IncLoseGamesCounter();

            Statistics.AddOrUpdate(gameMatch.serverPlayer, MatchResult.Win);
            Statistics.AddOrUpdate(gameMatch.clientPlayer, MatchResult.Lose);

            infoWin.SetText($"Игрок {gameMatch.serverPlayer.Name} победил!");
            infoWin.ShowDialog();

        }

        /// <summary>
        /// Объявить победу игрока-клиента
        /// </summary>
        private void AnnounceClientWin()
        {
            gameMatch.serverPlayer.IncLoseGamesCounter();
            gameMatch.clientPlayer.IncWinGamesCounter();

            Statistics.AddOrUpdate(gameMatch.serverPlayer, MatchResult.Lose);
            Statistics.AddOrUpdate(gameMatch.clientPlayer, MatchResult.Win);

            infoWin.SetText($"Игрок {gameMatch.clientPlayer.Name} победил!");
            infoWin.ShowDialog();

        }

        /// <summary>
        /// Объявить ничью
        /// </summary>
        private void AnnounceStandoff()
        {
            gameMatch.serverPlayer.IncDrawGamesCounter();
            gameMatch.clientPlayer.IncDrawGamesCounter();

            Statistics.AddOrUpdate(gameMatch.serverPlayer, MatchResult.Draw);
            Statistics.AddOrUpdate(gameMatch.clientPlayer, MatchResult.Draw);

            infoWin.SetText("Ничья!");
            infoWin.ShowDialog();

        }

        /// <summary>
        /// Варианты продолжения игры
        /// </summary>
        private void ReplayOffer()
        {
            MessageBoxResult mbRes = MessageBoxResult.Cancel;

            if (Network.replayInvite == null || Network.replayInvite == true)
            {
                mbRes = MessageBox.Show("Хотите ли вы продолжить игру? Матч перезапустится с предыдущими настройками", "Продолжить игру?", MessageBoxButton.YesNo);

                if (mbRes == MessageBoxResult.No)
                {
                    Network.SendPlayAgainRefuse();
                    Network.StopServer();
                    LeaveGame();
                }

                if (mbRes == MessageBoxResult.Yes)
                {
                    if (Network.replayInvite == false)
                    {
                        CancelReplay();
                        return;
                    }
                    if (Network.replayInvite == true)
                    {
                        Network.SendPlayAgain();
                        Network.StartGame(Network.iAmHost);
                        return;
                    }

                    Network.SendPlayAgain();

                    netMsgWindow netWin = new netMsgWindow("Ожидание решения второго игрока... ", Network.iAmHost);
                    netWin.ShowDialog();

                    if (Network.replayInvite == false)
                    {
                        CancelReplay();
                    }
                    if (Network.replayInvite == true)
                    {
                        Network.StartGame(Network.iAmHost);
                    }
                }

            }

            if (Network.replayInvite == false)
            {
                CancelReplay();
            }
        }

        /// <summary>
        /// Отмена перезапуска игры
        /// </summary>
        private void CancelReplay()
        {
            infoWin.SetText("Второй игрок отказался продолжать игру");
            infoWin.ShowDialog();
            Network.StopServer();
            LeaveGame();

        }

        /// <summary>
        /// Инициализация игрового матча
        /// </summary>
        private void InitGameMatch()
        {
            if (gameMatch == null)
            {
                if (Network.iAmHost)
                    gameMatch = new GameMatch(Statistics.leaderboard.profileName, Network.clientName);
                else
                    gameMatch = new GameMatch(Network.clientName, Statistics.leaderboard.profileName);
            }
            else
            {
                gameMatch.SetDefaultQueueStatus();
            }
            
        }

        /// <summary>
        /// Создание игры
        /// </summary>
        public void СreateGame(int _fieldSize, int _crossCount, int _pointsToWin, bool _host)
        {
            // инициализировать игровой матч
            InitGameMatch();

            //очистить игровое поле (отображение)
            if (gameFieldGrid != null)
                gameFieldGrid.Children.Clear();

            //очистить игровое поле (логическое)
            if (gameMatch.gameField != null)
                gameMatch.gameField.Clear();

            //нарисовать чистое игровое поле
            DrawField(_fieldSize);

            //инициализировать игровое поле (логическое)
            gameMatch.gameField = new Field(_fieldSize);

            //обновить содержание надписей интерфейса
            lastScoreLabel.Content = "Игра до " + _pointsToWin.ToString() + " очков";
            crossLabel.Content = "Зачеркивается " + _crossCount.ToString() + " символа (-ов)";
            scoresLabel.Content = ViewData.scoreLabel;
            ChangeTurnLabel();

            //вычислить количество клеток на поле
            gameMatch.freeCellsCount = _fieldSize * _fieldSize;
            
            //определить оставшиеся параметры игрового матча
            gameMatch.CharactersForLine = _crossCount;
            gameMatch.PointsToWin = _pointsToWin;
            gameMatch.ClearScores();

        }

        /// <summary>
        /// Смена сообщения об очередности хода
        /// </summary>
        public void ChangeTurnLabel()
        {
            if (gameMatch.QueueStatus == PlayerType.Server)
            {
                if (Network.iAmHost == true)
                    turnLabel.Content = "Вы ходите (Крестики)";
                if (Network.iAmHost == false)
                    turnLabel.Content = "Ход оппонента (Крестики)";
            }
            if (gameMatch.QueueStatus == PlayerType.Client)
            {
                if (Network.iAmHost == true)
                    turnLabel.Content = "Ход оппонента (Нолики)";
                if (Network.iAmHost == false)
                    turnLabel.Content = "Вы ходите (Нолики)";
            }
        }

        /// <summary>
        /// Сделать все прямоугольники невидимыми
        /// </summary>
        public void CleanRectangles()
        {
            var rects = from rect in mainCanvas.Children.OfType<Rectangle>()
                        select rect;

            foreach (Rectangle rect in rects)
            {
                rect.Visibility = Visibility.Hidden;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Dispatcher.InvokeShutdown();
        }

        private void LeaveGame()
        {
            if (gameMatch != null)
            {
                if (Network.iAmHost)
                {
                    Statistics.leaderboard.UpdateProfileScores(gameMatch.serverPlayer.WinGamesCounter, gameMatch.serverPlayer.LoseGamesCounter, gameMatch.serverPlayer.DrawGamesCounter);
                }
                else
                {
                    Statistics.leaderboard.UpdateProfileScores(gameMatch.clientPlayer.WinGamesCounter, gameMatch.clientPlayer.LoseGamesCounter, gameMatch.clientPlayer.DrawGamesCounter);
                }

                gameMatch = null;
                TabControler.Change(GameTabs.MainMenu);
            }
        }

        private void disconnectBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Если вы покинете игру сейчас, то вам засчитается поражение", "Выйти из игры?", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                if (Network.iAmHost)
                {
                    gameMatch.serverPlayer.IncLoseGamesCounter();
                    Statistics.AddOrUpdate(gameMatch.serverPlayer, MatchResult.Lose);
                    Statistics.AddOrUpdate(gameMatch.clientPlayer, MatchResult.Win);

                }
                else
                {
                    gameMatch.clientPlayer.IncLoseGamesCounter();
                    Statistics.AddOrUpdate(gameMatch.clientPlayer, MatchResult.Lose);
                    Statistics.AddOrUpdate(gameMatch.serverPlayer, MatchResult.Win);

                }             

                Network.SendDisconnect();
                LeaveGame();
                           

            }
        }
    }
}

