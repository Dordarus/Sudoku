using System;
using System.Collections.Generic;

using Sudoku.CustomProperties;
using Xamarin.Forms;

namespace Sudoku
{
    static class SmallSquare
    {
        private static List<string> squares = new List<string>(new string[9]);
        static SmallSquare()
        {
            int[,] first = new int[9, 3];
            int[,] second = new int[9, 3];
            int[,] third = new int[9, 3];

            var counter = 0;
            var k = 0;
            var t = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (j <= 2)
                    {
                        first[i, j] = counter++;
                    }

                    if (j >= 3 && j <= 5)
                    {
                        second[i, k++] = counter++;
                    }

                    if (j >= 6)
                    {
                        third[i, t++] = counter++;
                    }
                }
                k = 0;
                t = 0;
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i <= 2)
                    {
                        squares[0] += first[i, j] + ",";
                        squares[1] += second[i, j] + ",";
                        squares[2] += third[i, j] + ",";
                    }

                    if (i >= 3 && i <= 5)
                    {
                        squares[3] += first[i, j] + ",";
                        squares[4] += second[i, j] + ",";
                        squares[5] += third[i, j] + ",";
                    }

                    if (i >= 6)
                    {
                        squares[6] += first[i, j] + ",";
                        squares[7] += second[i, j] + ",";
                        squares[8] += third[i, j] + ",";
                    }
                }
            }

            for (int i = 0; i < squares.Count; i++)
            {
                squares[i] = squares[i].Trim(',');
            }
        }

        public static bool IsCorrectSquare(int index, string number, Grid grid)
        {
            bool isCorrect = true;

            foreach (string s in squares)
            {
                foreach (string num in s.Split(','))
                {
                    if (num == index.ToString())
                    {
                        foreach (string i in s.Split(','))
                        {
                            var label = grid.Children[Convert.ToInt32(i)] as TagLabel;
                            var labelText = label.Text;
                            var labelIndex = grid.Children.IndexOf(label);

                            if (labelText == number && labelIndex != index)
                            {
                                isCorrect = false;
                                break;
                            }
                        }
                    }
                }
            }           
            return isCorrect;
        }
    }
}
