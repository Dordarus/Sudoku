using Xamarin.Forms;

using static Sudoku.SmallSquare;
using Sudoku.CustomProperties;

namespace Sudoku
{
    static class Сorrectness
    {
        private static TagLabel label;
        private static Grid grid;
        private static string labelText;
        private static int labelIndex;
        private static int index;
        private static string number;

        private static bool IsCorrectRow()
        {
            var isorrectRow = true;

            var rowIndex = Grid.GetRow(grid.Children[index]);

            for (int i = rowIndex * 9; i < (rowIndex * 9) + 9; i++)
            {
                if (labelText == (grid.Children[i] as TagLabel).Text)
                {
                    if (index == i)
                    {
                        continue;
                    }
                    isorrectRow = false;
                    break;
                }
            }
            return isorrectRow;
        }

        private static bool IsCorrectColumn()
        {
            var isorrectColumn = true;

            var columnIndex = Grid.GetColumn(grid.Children[index]);

            for (int i = columnIndex; i < 81; i += 9)
            {
                if (labelText == (grid.Children[i] as TagLabel).Text)
                {
                    if (index == i)
                    {
                        continue;
                    }
                    isorrectColumn = false;
                    break;
                }
            }
            return isorrectColumn;
        }

        public static bool IsCorrectNumber(int _index, string _number, Grid _grid)
        {
            grid = _grid;
            number = _number;
            index = _index;
            label = (TagLabel)grid.Children[index];
            labelText = label.Text;
            labelIndex = grid.Children.IndexOf(label);

            return IsCorrectRow() && IsCorrectColumn() && IsCorrectSquare(index, number, grid);
        }
    }
}
