using System;
using Xamarin.Forms;
using static Sudoku.TrueRandom;
using static System.Math;
using Sudoku.CustomProperties;

namespace Sudoku
{
    static class Generator
    {
        public static int[,] BaseBigMatrix()
        {
            const int n = 3;            // n  - размер маленькой ячейки количество ячеек n*n
            var grid = new int[n * n, n * n];

            for (var i = 0; i < grid.GetLength(0); i++)
            {
                for (var j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = (i * n + i / n + j) % (n * n) + 1;
                }
            }
            return grid;
        }

        internal static int[,] Transposition(int[,] a)
        {
            var n = a.GetLength(0);
            var trans = new int[n, n];

            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    trans[i, j] = a[j, i];
                }
            }
            return trans;
        }

        internal static int[,] SwapRows(int[,] a)
        {
            var k = 0;

            while (k < GetRandomNumber(5, 11))
            {
                var n = Convert.ToInt32(Sqrt(a.GetLength(1)));

                var firstLine = GetRandomNumber(1, n + 1);
                var secondLine = GetRandomNumber(1, n + 1);

                var area = GetRandomNumber(0, n);

                while (secondLine == firstLine)
                {
                    secondLine = GetRandomNumber(1, n + 1);
                }

                var firstRow = area * n + firstLine;
                var secondRow = area * n + secondLine;

                for (var i = 0; i < a.GetLength(0); ++i)
                {
                    var tempCell = a[firstRow - 1, i];
                    a[firstRow - 1, i] = a[secondRow - 1, i];
                    a[secondRow - 1, i] = tempCell;
                }
                k++;
            }
            return a;
        }

        internal static int[,] SwapColumns(int[,] a)
        {
            var trans = Transposition(a);
            SwapRows(trans);
            return Transposition(trans);
        }

        public static int[,] Eraser(int[,] a, int k)
        {
            var n = 0;
            var length = a.GetLength(0);

            while (n < k)
            {
                for (var i = 0; i < length; i++)
                {
                    for (var j = 0; j < length; j++)
                    {
                        if (j == GetRandomNumber(0, length))
                        {
                            if (a[i, j] == 0)
                                a[i, GetRandomNumber(0, length)] = 0;
                            else
                                a[i, j] = 0;
                        }
                    }
                }
                n++;
            }
            return a;
        }

        public static int[,] Swaping(int[,] trans)
        {
            //Перемешиваю строчки в переделах их квадратов 3Х3, Рандомное количество раз
            SwapRows(trans);
            /*
             * Перемешиваю столбцы в 3 шага: 
             * 1.Транспонирую даную матрицу
             * 2.Перемешиваю сторчки
             * 3.Транспонирую второй раз
             */
            return SwapColumns(trans);
        }

        public static Grid Filler(int[,] a)
        {
            var controlGrid = new Grid { ColumnSpacing = 2, RowSpacing = 2 };

            for (int i = 0; i < 9; i++)
            {
                controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            var number = "";
            var tag = "";
            for (int i = 0; i < 9; i++)
            {
                
                for (int j = 0; j < 9; j++)
                {
                    if (a[i, j] == 0)
                    {
                        number = "";
                        tag = "play";
                    }
                    else
                    {
                        number = $"{a[i, j]}";
                        tag = "base";
                    }

                    if (i >= 3 && i <= 5)
                    {

                        if (j >= 3 && j <= 5)
                        {
                            controlGrid.Children.Add(new TagLabel
                            {
                                Text = number,
                                Tag = tag,
                                BackgroundColor = Color.White
                            }, j, i);
                        }
                        else
                        {
                            controlGrid.Children.Add(new TagLabel
                            {
                                Text = number,
                                Tag = tag,
                                BackgroundColor = Color.LightGray
                            }, j, i);
                        }
                    }
                    else
                    {
                        if (j <= 2 || j >= 6)
                        {
                            controlGrid.Children.Add(new TagLabel
                            {
                                Text = number,
                                Tag = tag,
                                BackgroundColor = Color.White
                            }, j, i);
                        }
                        else
                        {
                            controlGrid.Children.Add(new TagLabel
                            {
                                Text = number,
                                Tag = tag,
                                BackgroundColor = Color.LightGray
                            }, j, i);
                        }
                    }
                }
            }
            return controlGrid;
        }

        public static Grid StartGame(string level)
        {
            var baseMatrix = BaseBigMatrix();
            var trans = Transposition(baseMatrix);
            var swaped = Swaping(trans);

            Grid playGround = null;
            switch (level)
            {
                case "Easy":
                    playGround = Filler(Eraser(swaped, 4));
                    break;
                case "Medium":
                    playGround = Filler(Eraser(swaped, 6));
                    break;
                case "Hard":
                    playGround = Filler(Eraser(swaped, 8));
                    break;
            }
            return playGround;
        }
    }
}