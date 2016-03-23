using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public static class ViewData
    {
        /// <summary>
        /// Словарь, содержащий варианты возможного количества зачеркнутых символов для каждой размерности поля 
        /// </summary>
        public static Dictionary<int, List<int>> crossCount = new Dictionary<int, List<int>>()
        {
            [3] = new List<int>() { 3 },
            [5] = new List<int>() { 3 },
            [6] = new List<int>() { 3, 4 },
            [7] = new List<int>() { 3, 4 },
            [8] = new List<int>() { 3, 4, 5 },
            [9] = new List<int>() { 3, 4, 5},
            [10] = new List<int>() { 3, 4, 5, 6}

        };

        /// <summary>
        /// Словарь, содержащий варианты возможного количества зачеркнутых линий для победы для каждой размерности поля 
        /// </summary>
        public static Dictionary<int, List<int>> winCount = new Dictionary<int, List<int>>()
        {
            [3] = new List<int>() { 1 },
            [5] = new List<int>() { 1, 2 },
            [6] = new List<int>() { 1, 2 },
            [7] = new List<int>() { 1, 2, 3 },
            [8] = new List<int>() { 1, 2, 3 },
            [9] = new List<int>() { 1, 2, 3 },
            [10] = new List<int>() { 1, 2, 3, 4 }

        };

        public static string scoreLabel { get; } = "Счет (X:0) - ";
    }
}
