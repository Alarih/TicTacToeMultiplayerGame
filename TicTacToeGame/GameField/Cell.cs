namespace TicTacToeGame.GameField
{
    /// <summary>
    /// Тип игровой клетки
    /// </summary>
    public enum CellType
    {
        /// <summary>
        /// Крестик
        /// </summary>
        Cross,
        /// <summary>
        /// Нолик
        /// </summary>
        Zero,
        /// <summary>
        /// Клетка зачеркнута
        /// </summary>
        Blocked,
        /// <summary>
        /// Пустая клетка
        /// </summary>
        Empty

    }

    public struct CellCoords
    {
        public int X;
        public int Y;

    }

    /// <summary>
    /// Игровая клетка
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Тип клетки
        /// </summary>
        public CellType CellType { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Cell()
        {
            CellType = CellType.Empty;

        }

        /// <summary>
        /// Задать тип клетки, если клетка пустая
        /// </summary>
        public bool SetCellType(CellType type)
        {
            //Сразу завершить выполнение метода, если попытка заполнить пустым символом
            if (type == CellType.Empty)
                return false;

            if (CellType == CellType.Empty || type == CellType.Blocked)
            {
                CellType = type;
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Очистка клетки
        /// </summary>
        public void Clear()
        {
            CellType = CellType.Empty;

        }

    }
}
