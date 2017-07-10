using System;
using Xamarin.Forms;

namespace Sudoku
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void StartButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewGamePage());
        }

        private async void ContinueButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OldGamesPage());
        }
        private async void LeaderboardButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WinnerPage());
        }
    }
}
