using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace TicTacToeGame.Network
{
    /// <summary>
    /// Клиентская часть приложения
    /// </summary>
    public class Client
    {
        private int PORT;

        public Client(string ipAddress, int port)
        {
            IpAddress = ipAddress;
            PORT = port;
        }

        public string IpAddress { get; private set; }

        /// <summary>
        ///     Отправляет сообщение в потоке на IP, заданный в контроле IP
        /// </summary>
        /// <param name="Message">Передаваемое сообщение </param>
        public void ThreadSend(object Message)
        {
            try
            {
                //Проверяем входной объект на соответствие строке
                var MessageText = "";
                if (Message is string)
                    MessageText = Message as string;
                else
                    throw new Exception("На вход необходимо подавать строку");
                //Создаем сокет, коннектимся
                var EndPoint = new IPEndPoint(IPAddress.Parse(IpAddress), PORT);
                var Connector = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                Connector.Connect(EndPoint);
                //Отправляем сообщение
                var SendBytes = Encoding.Default.GetBytes(MessageText);
                Connector.Send(SendBytes);
                Connector.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
    }
}