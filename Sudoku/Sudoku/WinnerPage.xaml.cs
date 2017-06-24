using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sudoku
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WinnerPage : ContentPage
    {
        private string name;
        private string dif;
        private string gameDuration;

        public WinnerPage(string name, string dif, string gameDuration)
        {
            InitializeComponent();

            this.name = name;
            this.dif = dif;
            this.gameDuration = gameDuration;

            Label label = new Label
            {
                Text = $"{name} your score :/nDifficult:{dif}/nTime: {gameDuration}",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Content = label;
        }
    }
}