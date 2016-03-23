using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

using TicTacToeGame;



namespace TicTacToeGame.Network
{
    using TicTacToeGame.Statistics;

    /// <summary>
    /// Серверная часть приложения
    /// </summary>
    public class Server
    {
        //порт для приложения
        private int PORT;

        //слушающий объект
        private TcpListener _listener;
        //делегат для отрисовки
        //private readonly Draw AcceptDelegate = (string message, Button btn) => { btn.Content = message; };

        //флаг для отслеживания работы сервера
        private volatile bool listening = true;

        public Server(int port)
        {
            PORT = port;
        }

        public void Stop()
        {
            if (_listener != null)
            {
                listening = false;
                _listener.Server.Close();  
            }
                   
        }

        public void Receive()
        {
            //System.Windows.Threading.Dispatcher.Run();
            //Создаем Listener на порт "по умолчанию"
            _listener = new TcpListener(PORT);
            //Начинаем прослушку
            _listener.Start();
            //и заведем заранее сокет
            Socket ReceiveSocket;
            

            try
            {
                while (listening)
                {

                    //Пришло сообщение
                    ReceiveSocket = _listener.AcceptSocket();
                    var Receive = new byte[256];
                    //Читать сообщение будем в поток
                    using (var MessageR = new MemoryStream())
                    {
                        //Количество считанных байт
                        int ReceivedBytes;
                        do
                        {
                            //Собственно читаем
                            ReceivedBytes = ReceiveSocket.Receive(Receive, Receive.Length, 0);
                            //и записываем в поток
                            MessageR.Write(Receive, 0, ReceivedBytes);
                            //Читаем до тех пор, пока в очереди не останется данных
                        } while (ReceiveSocket.Available > 0);

                        // получить тип сообщения 
                        string message = Encoding.Default.GetString(MessageR.ToArray());
                        int msgNumber = (int)Char.GetNumericValue(message[0]);

                        if (message.Length > 1)
                            message = message.Substring(1, message.Length - 1);

                        NetMessages netMessage = (NetMessages)msgNumber;
                        // switch типов сообщений
                        switch (netMessage)
                        {
                            case NetMessages.MarkCell:
                               // Network.ChangeQueue();
                                OnMarkCell(message);
                               // Network.ChangeQueue();
                                break;
                            case NetMessages.Connect:
                                OnConnect(message, ((IPEndPoint)_listener.LocalEndpoint).Port);
                                break;
                            case NetMessages.Disconnect:
                                OnDisonnect();
                                break;
                            case NetMessages.ServerResponse:
                                OnServerResponse(message);
                                break;
                            case NetMessages.PlayAgain:
                                OnPlayAgain();
                                break;
                            case NetMessages.PlayAgainRefuse:
                                OnPlayAgainRefuse();
                                break;
                            case NetMessages.ClientName:
                                OnClientName(message);
                                break;
                            default:
                                break;
                        }

                    }
                }

            }
            catch (Exception ex)
            {               
               // MessageBox.Show(ex.Message);
            }
            finally
            {
                _listener.Stop();
            }

            
        }

        private void OnClientName(string _message)
        {
            Network.clientName = _message;
        }

        private void OnPlayAgainRefuse()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                Network.replayInvite = false;
                Network.CloseNetMsgWindow();

            }));
           
            
        }

        private void OnPlayAgain()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                Network.replayInvite = true;
                Network.CloseNetMsgWindow();

            }));
            
        }

        /// <summary>
        /// Обработка сообщения Connect
        /// </summary>
        private void OnConnect(string _ip, int _clientPort)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                //запуск клиентской части приложения
                Network.StartClient(_ip, _clientPort);
                //отправка игровых настроек игрока сервера игроку клиенту
                Network.SendServerSettings();
                Network.SendName();
                //запуск игры на стороне игрока сервера
                Network.StartGame(true);
                //закрытие окна ожидания подключения
                Network.CloseNetMsgWindow();

            }));
            
        }

        private void OnDisonnect()
        {
            var dialogReult = MessageBox.Show("Ваш оппонент покинул игру. Вам засчитана победа.", "Выход соперника", MessageBoxButton.OK);
            
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                ProfileIncPoint();

            }));

            Network.RaiseExitToMainMenu();
            Network.StopServer();

        }

        private void ProfileIncPoint()
        {
            if (Network.iAmHost)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    Network.RaiseIncServerPlayerPoint();
                }));
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    Network.RaiseIncClientPlayerPoint();
                }));
            }
        }

        /// <summary>
        /// Обработка ответа от игрока сервера
        /// </summary>
        private void OnServerResponse(string _message)
        {
            //парсинг сообщения от игрока сервера, получение игровых параметров
            int _fieldSize = Convert.ToInt32(_message.Substring(0, _message.IndexOf(";")));
            _message = _message.Substring(_message.IndexOf(";") + 1);
            int _crossCount = Convert.ToInt32(_message.Substring(0, _message.IndexOf(";")));
            _message = _message.Substring(_message.IndexOf(";") + 1);
            int _pointsToWin = Convert.ToInt32(_message.Substring(0, _message.IndexOf(";")));

            
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                //запуск игры на стороне игрока клиента
                Network.StartGame(_fieldSize, _crossCount, _pointsToWin, false);
            
                //закрытие окна ожидания подключения
                Network.CloseNetMsgWindow();
            }));

        }
      
        /// <summary>
        /// Обработка сообщения об отметке клетки
        /// </summary>
        private void OnMarkCell(string _message)
        {
            //парсим данные на координаты
            string coordinates = _message;

            int x = Convert.ToInt32(coordinates.Substring(0, coordinates.IndexOf(",")));
            coordinates = coordinates.Substring(coordinates.IndexOf(",") + 1);
            int y = Convert.ToInt32(coordinates);

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                Network.RaiseClickImageByCoordsEvent(x, y);

            }));
            
        }      

        private delegate void Draw(string message, Button btn);
    }
}