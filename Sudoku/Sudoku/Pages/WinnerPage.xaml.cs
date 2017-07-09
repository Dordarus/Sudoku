using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using static Sudoku.Saver;
using static Sudoku.Loader;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sudoku
{
    public class WinnerInfo
    {
        public string Name { get; set; }
        public string Difficult { get; set; }
        public string GameDuration { get; set; }
        public string DateOfGame { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WinnerPage : ContentPage
    {
        WinnerInfo winner = new WinnerInfo();
        WinnerList list = new WinnerList();

        public List<WinnerInfo> Winners { get; set; }

        public WinnerPage(string name, string dif, string gameDuration)
        {
            Winners = new List<WinnerInfo>();
            InitializeComponent();

            winner.Name = name;
            winner.Difficult = dif;
            winner.GameDuration = gameDuration;
            winner.DateOfGame = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        }

        private bool _canClose = true;
        protected override bool OnBackButtonPressed()
        {
            if (_canClose)
            {
                ShowExitDialog();
            }
            return _canClose;
        }


        public async void ShowExitDialog()
        {
            var leaveGame = await DisplayAlert("Back to Main Menu!", "Are you sure?", "Yes", "No");     

            if (leaveGame)
                await Navigation.PopToRootAsync();
        }

        private async Task LoadList()
        {
            var winnersList = await LoadLeaderboard();
            Winners = winnersList;
        }

        void UpdateWinnersList()
        {
            BindingContext = this;
            gamesList.ItemsSource = Winners;

            gamesList.SelectedItem = null;
        }

        void Delete(object sender, EventArgs args)
        {
            var bindableObject = (BindableObject)sender;
            var context = ((WinnerInfo)bindableObject.BindingContext);

            Winners.Remove(context);

            UpdateWinnersList();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadList();

            Winners.Add(winner);

            list.Winners = Winners;
            SaveWinner(list);

            UpdateWinnersList();
        }
    }
}