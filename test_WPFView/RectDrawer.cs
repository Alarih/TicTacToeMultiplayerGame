using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TicTacToeGame.GameField;

namespace test_WPFView
{
    public static class RectDrawer
    {
        /// <summary>
        /// Метод возвращает прямоугольник по форме и размеру зачеркнутой линии
        /// </summary>
        public static Rectangle addRect(double imgSize, int charactersForLine, Crossing.CrossingResult crossRes)
        {
            Rectangle rect = new Rectangle
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Fill = new SolidColorBrush(Colors.LightGoldenrodYellow),
                RadiusX = imgSize / 3,
                RadiusY = imgSize / 3
            };

            double rotateWidth;

            //проверка направлений
            switch (crossRes.direction)
            {
                case Crossing.Directions.Down:
                    rect.Width = imgSize / 5;
                    rect.Height = imgSize * (charactersForLine);

                    Canvas.SetLeft(rect, (crossRes.coords.X + 0.5) * imgSize - rect.Width / 2);
                    Canvas.SetTop(rect, (crossRes.coords.Y) * imgSize);
                    break;
                case Crossing.Directions.Up:
                    rect.Width = imgSize / 5;
                    rect.Height = imgSize * (charactersForLine);

                    Canvas.SetLeft(rect, (crossRes.coords.X + 0.5) * imgSize - rect.Width / 2);
                    Canvas.SetTop(rect, (crossRes.coords.Y + 1) * imgSize - rect.Height);
                    break;
                case Crossing.Directions.Left:
                    rect.Width = imgSize * (charactersForLine);
                    rect.Height = imgSize / 5;

                    Canvas.SetLeft(rect, (crossRes.coords.X + 1) * imgSize - rect.Width);
                    Canvas.SetTop(rect, (crossRes.coords.Y + 0.5) * imgSize - rect.Height / 2);
                    break;
                case Crossing.Directions.Right:
                    rect.Width = imgSize * (charactersForLine);
                    rect.Height = imgSize / 5;

                    Canvas.SetLeft(rect, (crossRes.coords.X) * imgSize);
                    Canvas.SetTop(rect, (crossRes.coords.Y + 0.5) * imgSize - rect.Height / 2);
                    break;


                case Crossing.Directions.DiagonalUpToLeft:

                    rotateWidth = imgSize * (charactersForLine) * Math.Sqrt(2);
                    rect.Width = rotateWidth;
                    rect.Height = imgSize / 5;
                    rect.RenderTransform = new RotateTransform(45, rotateWidth, 0);

                    Canvas.SetLeft(rect, (crossRes.coords.X + 1) * imgSize - rect.Width);
                    Canvas.SetTop(rect, (crossRes.coords.Y + 1) * imgSize - rect.Height / 2);
                    break;
                case Crossing.Directions.DiagonalDownToLeft:


                    rotateWidth = imgSize * (charactersForLine) * Math.Sqrt(2);
                    rect.Width = rotateWidth;
                    rect.Height = imgSize / 5;
                    rect.RenderTransform = new RotateTransform(315, rotateWidth, 0);

                    Canvas.SetLeft(rect, (crossRes.coords.X + 1) * imgSize - rect.Width);
                    Canvas.SetTop(rect, (crossRes.coords.Y) * imgSize - rect.Height / 2);
                    break;
                case Crossing.Directions.DiagonalUpToRight:


                    rotateWidth = imgSize * (charactersForLine) * Math.Sqrt(2);
                    rect.Width = rotateWidth;
                    rect.Height = imgSize / 5;
                    rect.RenderTransform = new RotateTransform(-45, 0, 0);

                    Canvas.SetLeft(rect, (crossRes.coords.X) * imgSize);
                    Canvas.SetTop(rect, (crossRes.coords.Y + 1) * imgSize - rect.Height / 2);
                    break;
                case Crossing.Directions.DiagonalDownToRight:


                    rotateWidth = imgSize * (charactersForLine) * Math.Sqrt(2);
                    rect.Width = rotateWidth;
                    rect.Height = imgSize / 5;
                    rect.RenderTransform = new RotateTransform(45, 0, 0);

                    Canvas.SetLeft(rect, (crossRes.coords.X) * imgSize);
                    Canvas.SetTop(rect, (crossRes.coords.Y) * imgSize - rect.Height / 2);
                    break;
            }


            return rect;
            
        }

    }
}
