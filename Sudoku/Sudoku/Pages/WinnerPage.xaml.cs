using System;
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

        public WinnerPage(string name, string dif, string gameDuration) : this()
        {
            NavigationPage.SetHasBackButton(this, false);

            winner.Name = name;
            winner.Difficult = dif;
            winner.GameDuration = gameDuration;
            winner.DateOfGame = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

            Winners.Add(winner);

            SaveChanges();
        }

        void SaveChanges()
        {
            list.Winners = Winners;
            SaveWinner(list);
        }

        public WinnerPage()
        {
            InitializeComponent();
            Winners = new List<WinnerInfo>();
            LoadList();
        }

        bool _canClose = true;
        protected override bool OnBackButtonPressed()
        {
            if (_canClose)
            {
                ToRoot();
            }
            return _canClose;
        }
        
        async void ToRoot()
        {
            await Navigation.PopToRootAsync();
        }

        async void LoadList()
        {
            Winners = await LoadLeaderboard();
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

            List<WinnerInfo> clone = new List<WinnerInfo>(Winners);
            clone.Remove(context);
            Winners = clone;

            SaveChanges();

            UpdateWinnersList();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateWinnersList();
        }
    }
}