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
    public partial class NewGamePage : ContentPage
    {
        public NewGamePage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GamePage(name.Text, ((Button)sender).Text));
        }


        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (name.Text != string.Empty)
            {
                foreach (var button in grid.Children)
                {
                    button.IsEnabled = true;
                }
            }
            else
            {
                foreach (var button in grid.Children)
                {
                    button.IsEnabled = false;
                }
            }
        }
    }
}
