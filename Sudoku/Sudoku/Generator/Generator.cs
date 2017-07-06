using System;
using Xamarin.Forms;
using static Sudoku.TrueRandom;
using static System.Math;
using Sudoku.CustomProperties;
using System.Collections.Generic;

namespace Sudoku
{
    static class Generator
    {
        public static int[,] BaseBigMatrix()
        {
            const int n = 3;
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

            while (k < GetRandomNumber(10, 16))
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
            var rows = SwapRows(trans);
            return Transposition(rows);
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
            var rows = SwapRows(trans);
            return SwapColumns(rows);
        }

        public static Grid Filler(List<MyLabel> labelList)
        {
            var playGround = new Grid { ColumnSpacing = 2, RowSpacing = 2 };

            var j = 0;
            var i = 0;
            var count = 0;
            foreach (MyLabel label in labelList)
            {
                count = playGround.Children.Count;
                //Part that fill grid with digits
                if (j < 9)
                {
                    playGround.Children.Add(new TagLabel
                    {
                        Text = label.Text,
                        Tag = label.Tag,
                        FontAttributes = (FontAttributes)Enum.Parse(typeof(FontAttributes), label.FontAttribute)
                    }, j++, i);
                }
                else
                {
                    i++;
                    j = 0;

                    playGround.Children.Add(new TagLabel
                    {
                        Text = label.Text,
                        Tag = label.Tag,
                        FontAttributes = (FontAttributes)Enum.Parse(typeof(FontAttributes), label.FontAttribute)
                    }, j++, i);
                }

                //Part that assigns colors to cells
                if (i >= 3 && i <= 5)
                {
                    if (j >= 4 && j <= 6)
                    {
                        playGround.Children[count].BackgroundColor = Color.White;
                    }
                    else
                    {
                        playGround.Children[count].BackgroundColor = Color.LightGray;
                    }
                }
                else
                {
                    if (j <= 3 || j >= 7)
                    {
                        playGround.Children[count].BackgroundColor = Color.White;
                    }
                    else
                    {
                        playGround.Children[count].BackgroundColor = Color.LightGray;
                    }
                }
            }
            return playGround;
        }

        public static List<MyLabel> ToLabelList(int[,] numbers)
        {
            var number = "";
            var tag = "";

            List<MyLabel> labelList = new List<MyLabel>();

            foreach (int digit in numbers)
            {
                if (digit == 0)
                {
                    number = "";
                    tag = "play";
                }
                else
                {
                    number = digit.ToString();
                    tag = "base";
                }

                labelList.Add(new MyLabel
                {
                    Text = number,
                    Tag = tag,
                    FontAttribute = "Bold"
                });
            }
            return labelList;
        }

        public static Grid StartGame(string level)
        {
            var baseMatrix = BaseBigMatrix();
            var trans = Transposition(baseMatrix);
            var swaped = Swaping(trans);

            Grid playGround = null;
            List<MyLabel> list = null;
            switch (level)
            {
                case "Easy":
                    list = ToLabelList(Eraser(swaped, 4));
                    playGround = Filler(list);
                    break;
                case "Medium":
                    list = ToLabelList(Eraser(swaped, 5));
                    playGround = Filler(list);
                    break;
                case "Hard":
                    list = ToLabelList(Eraser(swaped, 7));
                    playGround = Filler(list);
                    break;
            }
            return playGround;
        }
    }
}
