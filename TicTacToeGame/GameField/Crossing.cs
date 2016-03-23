using System;
using System.Collections.Generic;

namespace TicTacToeGame.GameField
{
    public static class Crossing
    {
        /// <summary>
        /// класс хранящий координаты последней зачеркнутой клетки, и направление зачеркивающей линии
        /// </summary>
        public class CrossingResult
        {
            public CellCoords coords { get; private set; }
            public Directions direction { get; private set; }

            public CrossingResult(CellCoords _coords, Directions _direction)
            {
                coords = _coords;
                direction = _direction;
            }

        }

        /// <summary>
        /// все возможные направления для проверки
        /// </summary>
        public enum Directions
        {
            Up,
            Down,
            Left,
            Right,
            DiagonalUpToRight,
            DiagonalDownToRight,
            DiagonalUpToLeft,
            DiagonalDownToLeft,
            Fail

        }

        /// <summary>
        /// Словарь хранящий противоположные стороны
        /// </summary>
        private static Dictionary<Directions, Directions> oppositeDirections = new Dictionary<Directions, Directions>()
        {
            [Directions.Up] = Directions.Down,
            [Directions.Down] = Directions.Up,
            [Directions.Left] = Directions.Right,
            [Directions.Right] = Directions.Left,
            [Directions.DiagonalDownToLeft] = Directions.DiagonalUpToRight,
            [Directions.DiagonalUpToRight] = Directions.DiagonalDownToLeft,
            [Directions.DiagonalDownToRight] = Directions.DiagonalUpToLeft,
            [Directions.DiagonalUpToLeft] = Directions.DiagonalDownToRight

        };

        /// <summary>
        /// необходимое количество зачеркнутых символов
        /// </summary>
        private static int pointsToCross;

        /// <summary>
        /// количество зачеркнутых символов для проверки
        /// </summary>
        private static int currentPoints;

        /// <summary>
        /// координаты клетки, с которой начинается проверка
        /// </summary>
        private static CellCoords oldCoords;

        /// <summary>
        /// координаты последней клетки
        /// </summary>
        private static CellCoords endCoords;

        /// <summary>
        /// последнее выбранное направление (используется после смены направления)
        /// </summary>
        private static Directions endDir;

        /// <summary>
        /// проверка на последнюю клетку
        /// </summary>
        private static bool endFlag;

        /// <summary>
        /// проверка на смену направления
        /// </summary>
        private static bool alreadyOpposite;

        /// <summary>
        /// игровое поле, на котором ведется проверка
        /// </summary>
        private static Field field;

        /// <summary>
        /// тип проверяемой клетки
        /// </summary>
        private static CellType type;

        /// <summary>
        /// список координат клеток, которые при выстроении линии заблокируются
        /// </summary>
        private static List<CellCoords> blockedCellList;

        /// <summary>
        /// Заблокировать все выбранные клетки
        /// </summary>
        private static void BlockCells(Field _field)
        {
            foreach (CellCoords blockedCoords in blockedCellList)
            {
                _field.MarkTheFieldPlace(blockedCoords.X, blockedCoords.Y, CellType.Blocked);
            }

        }

        /// <summary>
        /// Проверка выстроенной линии от выбранного символа
        /// </summary>
        public static CrossingResult CheckLines(CellCoords coords, CellType _type, Field _field, int _pointsToCross)
        {
            //сохраняем поле, тип и необходимое количество символов
            field = _field;
            type = _type;
            pointsToCross = _pointsToCross;

            //запоминаем координаты первой клетки
            oldCoords.X = coords.X;
            oldCoords.Y = coords.Y;

            //создание списка с заблокированными клетками
            blockedCellList = new List<CellCoords>();

            //проверка каждого направления
            foreach (Directions direction in Enum.GetValues(typeof(Directions)))
            {
                //не проверять неудачный случай
                if (direction == Directions.Fail)
                    break;

                //очистка списка с координатами заблокированных клеток
                blockedCellList.Clear();

                //сброс зачеркнутых символов
                currentPoints = 0;

                //сброс смены направления
                alreadyOpposite = false;
                endFlag = false;

                if (CheckNextCell(direction, coords))
                {
                    //добавление первой клетки в список заблокированных
                    blockedCellList.Add(oldCoords);
                    //все зачеркнутые клетки получают статус заблокированных
                    BlockCells(_field);

                    return new CrossingResult(endCoords, oppositeDirections[endDir]);
                }
            }

            return new CrossingResult(endCoords, Directions.Fail);
        }

        private static bool CheckNextCell(Directions direction, CellCoords coords)
        {
            bool result = false;

            //проверить соответствие типа клетки
            bool cellTypeRight = field.FieldArray[coords.X, coords.Y].CellType == type;

            //если соответствует символу
            if (cellTypeRight)
            {
                //увеличить счетчик клеток
                currentPoints++;

                //добавить клетку в список блокируемых 
                //если это не первая клетка (иначе при смене направления первая клетка будет занесена в список повторно)
                if (!coords.Equals(oldCoords))
                    blockedCellList.Add(coords);

                //проверить на необходимое количество клеток для зачеркивания
                if (currentPoints == pointsToCross)
                    return true;

                // выбрать следующую клетку согласно направлению
                coords = ChooseCell(direction, coords);

                //проверить на выход за границы поля
                if (CheckOutOfField(coords, field))
                    return false;

                //проверить следующую клетку
                result = CheckNextCell(direction, coords);
            }

            //если еще не было смены направления меняем направление
            if (!alreadyOpposite)
                result = ChangeDirection(direction, cellTypeRight, result);

            if (!endFlag)
            {
                endCoords = coords;
                endDir = direction;
                endFlag = true;
            }
            
            return result;
        }

        private static bool ChangeDirection(Directions direction, bool currentCellTypeRight, bool lineResult)
        {
            bool result = false;

            //если клетка не соответствует типу, либо проверка вышла за границы поля
            if (!currentCellTypeRight || lineResult == false)
            {
                //но в выбранном направлении уже были зачеркнутые клетки,                
                if (currentPoints > 1)
                {
                    //меняем направление проверки на противоположенное
                    direction = oppositeDirections[direction];
                    //меняем флаг отвечающий за проверку смены направления
                    alreadyOpposite = true;
                    //уменьшаем счетчик клеток на единицу, поскольку алгоритм прибавит стартовую клетку  
                    currentPoints--;
                    //начинаем проверку противоположенного напраления с координат первой клетки
                    result = CheckNextCell(direction, oldCoords);

                }
                else
                    return false;                
            }

            return result;
        }

        /// <summary>
        /// Проверка выхода за границы поля
        /// </summary>
        /// <returns>Возвращает true, если координаты выходят за границы</returns>
        private static bool CheckOutOfField(CellCoords coords, Field field)
        {
            return (coords.X < 0)||(coords.Y < 0)||(coords.X >= field.FieldSize)||(coords.Y >= field.FieldSize);

        }

        /// <summary>
        /// выбрать следующую клетку согласно направлению
        /// </summary>
        private static CellCoords ChooseCell(Directions direction, CellCoords coords)
        {
            switch (direction)
            {
                case Directions.Up:
                    coords.Y--;
                    break;
                case Directions.Down:
                    coords.Y++;
                    break;
                case Directions.Left:
                    coords.X--;
                    break;
                case Directions.Right:
                    coords.X++;
                    break;
                case Directions.DiagonalDownToLeft:
                    coords.Y++;
                    coords.X--;
                    break;
                case Directions.DiagonalDownToRight:
                    coords.Y++;
                    coords.X++;
                    break;
                case Directions.DiagonalUpToLeft:
                    coords.Y--;
                    coords.X--;
                    break;
                case Directions.DiagonalUpToRight:
                    coords.Y--;
                    coords.X++;
                    break;
            }

            return coords;
        }
    }
}
