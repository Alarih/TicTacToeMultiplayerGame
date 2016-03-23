using System.Windows.Controls;

namespace TicTacToeGame
{
    /// <summary>
    /// Класс для связи контрола картинки с классом клетки игрового поля
    /// </summary>
    public class TicTacImage : Button
    {
        /// <summary>
        /// координата по Х
        /// </summary>
        public int X;

        /// <summary>
        /// координата по Y
        /// </summary>
        public int Y;

        /// <summary>
        /// Флаг, показывающий состояние картинки (отправлена или нет)
        /// </summary>
        public bool sended = false;

    }
}