using Xamarin.Forms;

namespace Sudoku.CustomProperties
{
    public class TagLabel : Label
    {
        public static readonly BindableProperty TagProperty =
            BindableProperty.Create("Tag", typeof(string), typeof(TagLabel), "0");

        public string Tag
        {
            set
            {
                SetValue(TagProperty, value);
            }
            get
            {
                return (string)GetValue(TagProperty);
            }
        }
    }
}
