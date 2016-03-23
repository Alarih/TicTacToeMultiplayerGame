using System;

namespace TicTacToeGame.GameField
{
    /// <summary>
    /// Поле, состоящее из клеток на котором происходит игровой процесс.
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Размер поля
        /// </summary>
        public int FieldSize { get; private set; }

        /// <summary>
        /// Множество клеток игрового поля, которые могут иметь одно из нескольких состояний
        /// </summary>
        public Cell[,] FieldArray { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Field(int fieldSize)
        {
            FieldSize = fieldSize;

            FieldArray = new Cell[FieldSize, FieldSize];

            for (int i = 0; i < FieldSize; i++)
            {
                for (int k = 0; k < FieldSize; k++)
                {
                    FieldArray[i, k] = new Cell();
                }
            }

        }

        /// <summary>
        /// Попробовать заполнить клетку на поле. 
        /// Если указаны неверные координаты, либо поле уже занято, метод вернет false
        /// </summary>
        public bool MarkTheFieldPlace(int x, int y, CellType type)
        {
            bool res = false;

            try
            {
                res = FieldArray[x, y].SetCellType(type);
            }
            catch (NullReferenceException)
            {
                return false;
            }

            return res;
        }

        /// <summary>
        /// Очистка поля
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < FieldSize; i++)
            {
                for (int k = 0; k < FieldSize; k++)
                {
                    FieldArray[i, k].Clear();
                }
            }

        }

    }
}
