using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TicTacToeGame.Network
{
    using TicTacToeGame.Statistics;

    /// <summary>
    /// Список сетевых сообщений
    /// </summary>
    public enum NetMessages
    {
        /// <summary>
        /// заполнить клетку
        /// </summary>
        MarkCell,
        /// <summary>
        ///  сообщение о присоединеннии к игре от клиента
        /// </summary>
        Connect,
        /// <summary>
        ///  сообщение об отключении от игры от клиента либо от сервера
        /// </summary>
        Disconnect,
        /// <summary>
        ///  ответ от сервера клиенту, отправляется вместе с параметрами игры
        /// </summary>
        ServerResponse,
        /// <summary>
        /// Предложение о повторной игре
        /// </summary>
        PlayAgain,
        /// <summary>
        /// Отказ от повторной игры
        /// </summary>
        PlayAgainRefuse,
        /// <summary>
        /// Сообщение с именем профиля
        /// </summary>
        ClientName

    }

    /// <summary>
    /// Класс, осуществляющий запуск клиентского и серверного потоков, а также сетевое взаимодействие между игроками
    /// </summary>
    public static class Network
    {        
        private static Server server;
        private static Client client;
        
        private static int serverPort = -1;

        /// <summary>
        /// Состояние клиента
        /// </summary>
        public static bool clientOn { get { return client == null ? false : true; } }

        public static bool iAmHost { get; set; } = false;

        private static int fieldSize = 0;
        private static int crossCount = 0;
        private static int pointsToWin = 0;

        public static bool? replayInvite { get; set; } = null;
        public static string clientName { get; set; } = "myEnemy";

        /// <summary>
        /// Отправить сетевое сообщение оппоненту
        /// </summary>
        private static void SendMessage(NetMessages _message)
        {
            SendMessage(_message, "");
        }

        /// <summary>
        /// Отправить сетевое сообщение с дополнительной информацией оппоненту
        /// </summary>
        private static void SendMessage(NetMessages _message, string _extra)
        {
            if (client != null)
            {
                Thread clientSendThread = new Thread(new ParameterizedThreadStart(client.ThreadSend));
                clientSendThread.IsBackground = true;
                clientSendThread.Start(((int)_message).ToString() + _extra);
            } else
            {
                MessageBox.Show( $"Ошибка отправки сообщения [{_message.ToString() + _extra}]. Клиент не запущен. ");
            }

        }

        /// <summary>
        /// Остановить работу сервера (и завершение серверного потока)
        /// </summary>
        public static void StopServer()
        {
            if (server != null)
            {
                server.Stop();
            }
        }

        /// <summary>
        /// отсылка сообщения о нажатии на поле от клиента серверу
        /// </summary>
        public static void SendCoords(int _x, int _y)
        {
            SendMessage(NetMessages.MarkCell, _x.ToString() + "," + _y.ToString());

        }

        /// <summary>
        /// Отправка имени профиля оппоненту
        /// </summary>
        public static void SendName()
        {
            SendMessage(NetMessages.ClientName, Statistics.leaderboard.profileName);

        }

        /// <summary>
        /// Отправка сообщения игрока-клиента игроку-серверу о подключении (вместе с IP клиента)
        /// </summary>
        /// <param name="_IP">IP</param>
        public static void SendClientIP(string _IP)
        {
            SendMessage(NetMessages.Connect, _IP);

        }

        /// <summary>
        /// Отправка оппоненту предложения повторить игру
        /// </summary>
        public static void SendPlayAgain()
        {
            SendMessage(NetMessages.PlayAgain);

        }

        /// <summary>
        /// Отправка оппоненту отказа играть снова
        /// </summary>
        public static void SendPlayAgainRefuse()
        {
            SendMessage(NetMessages.PlayAgainRefuse);

        }

        /// <summary>
        /// Отправить настройки от игрока-сервера игроку-клиенту
        /// </summary>
        public static void SendServerSettings()
        {
            if (fieldSize == 0 || crossCount == 0 || pointsToWin == 0)
            {
                MessageBox.Show("sendServerSettings() Параметры игры не заданы");
            }
            else
            {
                SendMessage(NetMessages.ServerResponse, fieldSize.ToString() + ';' + crossCount.ToString() + ';' + pointsToWin.ToString() + ';');
            }
        }

        /// <summary>
        /// Отправка сообщения партнеру по игре об отсоединении
        /// </summary>
        public static void SendDisconnect()
        {
            SendMessage(NetMessages.Disconnect);
            StopServer();

        }

        // Событие "Закрытие окна ожидания подключения"
        public delegate void closeNetMsgWinContainer();

        public static event closeNetMsgWinContainer onCloseNetMsgWin;

        /// <summary>
        /// Закрыть окно ожидания подключения
        /// </summary>
        public static void CloseNetMsgWindow()
        {
            onCloseNetMsgWin();

        }

        // Событие "Старт игры"
        public delegate void startGameContainer(int _fieldSize, int _crossCount, int _pointsToWin, bool _host);

        public static event startGameContainer onStartGame;

        /// <summary>
        /// Запуск игры с определяемыми параметрами
        /// </summary>
        /// <param name="_fieldSize">Размер поля</param>
        /// <param name="_crossCount">количество клеток для зачеркивания линии</param>
        /// <param name="_pointsToWin">количество очков для победы</param>
        /// <param name="_host">роль игрока (true - сервер; false - клиент)</param>
        public static void StartGame(int _fieldSize, int _crossCount, int _pointsToWin, bool _host)
        {
            Network.replayInvite = null;
            iAmHost = _host;

            if (_fieldSize == 0 || _crossCount == 0 || _pointsToWin == 0)
            {
                throw new Exception("Параметры запуска игры не были определены");
            }

            fieldSize = _fieldSize;
            crossCount = _crossCount;
            pointsToWin = _pointsToWin;

            onStartGame(_fieldSize, _crossCount, _pointsToWin, _host);
            
        }

        /// <summary>
        /// Запуск игры с параметрами, определенными при старте сервера
        /// </summary>
        public static void StartGame(bool _host)
        {
            StartGame(fieldSize, crossCount, pointsToWin, _host);
        }

        /// <summary>
        /// Создание сервера программы
        /// </summary>
        public static void StartServer()
        {
            if (serverPort != -1)
            {
                StartServer(serverPort);
            }
            else
            {
                throw new Exception("Попытка запука сервера без заданного порта!");
            }
        }

        /// <summary>
        /// Запуск сервера (и запуск серверного потока)
        /// </summary>
        public static void StartServer(int _serverPort)
        {
            serverPort = _serverPort;
            server = new Server(_serverPort);

            //создание серверного потока
            Thread serverThread = new Thread(server.Receive);
            serverThread.IsBackground = true;
            serverThread.Start();

        }

        /// <summary>
        /// Запуск сервера с заданием параметров игры
        /// </summary>
        public static void StartServerSetProps(int _serverPort, int _fieldSize, int _crossCount, int _pointsToWin)
        {
            serverPort = _serverPort;
            server = new Server(_serverPort);

            //создание серверного потока
            Thread serverThread = new Thread(server.Receive);
            serverThread.IsBackground = true;
            serverThread.Start();       

            //задать параметры игры
            fieldSize = _fieldSize;
            crossCount = _crossCount;
            pointsToWin = _pointsToWin;

        }

        /// <summary>
        /// создание клиента программы
        /// </summary>
        public static void StartClient(string _IP, int _clientPort)
        {
            client = new Client(_IP, _clientPort);

        }

        /// <summary>
        /// Получение локального IP адреса
        /// </summary>
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        #region Событие "Нажатие на картинку игроком оппонентом" 

        public delegate void clickImageByCoordsContainer(int x, int y);

        public static event clickImageByCoordsContainer onServerImageClick;

        public static void RaiseClickImageByCoordsEvent(int x, int y)
        {
            onServerImageClick(x, y);
        }

        #endregion

        #region Событие "Выйти в главное меню" 

        public delegate void exitToMainMenuContainer();

        public static event exitToMainMenuContainer onExitToMainMenu;

        public static void RaiseExitToMainMenu()
        {
            onExitToMainMenu();
        }

        #endregion

        #region Событие "Добавить очко победы игроку-серверу" 

        public delegate void incServerPointsContainer();

        public static event incServerPointsContainer onIncServerPlayerPoint;

        public static void RaiseIncServerPlayerPoint()
        {
            onIncServerPlayerPoint();
        }

        #endregion

        #region Событие "Добавить очко победы игроку-клиенту" 

        public delegate void incClientPointsContainer();

        public static event incClientPointsContainer onIncClientPlayerPoint;

        public static void RaiseIncClientPlayerPoint()
        {
            onIncClientPlayerPoint();
        }

        #endregion
    }
}
