using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using static Sudoku.Loader;

namespace Sudoku
{
    public class GameInfo
    {
        public string Title { get; set; }
        public string Time { get; set; }
        public string GameDuration { get; set; }
        public string FullName { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OldGamesPage : ContentPage
    {
        public List<GameInfo> Games { get; set; }


        public OldGamesPage()
        {
            InitializeComponent();   
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await UpdateFileList();
        }

        async void Delete(object sender, EventArgs args)
        {
            var bindableObject = (BindableObject)sender;
            var context = ((GameInfo)bindableObject.BindingContext);
            var name = context.FullName;

            await DependencyService.Get<IFileWorker>().DeleteAsync(name);
            await UpdateFileList();
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var game = (GameInfo)e.Item;
            var fileName = game.FullName;           
            var gameDuration = game.GameDuration.Substring(14);

            var splitted = game.Title.Split();
            var name = splitted[0];
            var dif = splitted[1];
            var savingDate = game.Time;

            var playground = await LoadGame(fileName);
            await Navigation.PushAsync(new GamePage(name, dif, gameDuration, playground, IndexOfRedLabel, savingDate));
        }

        async Task UpdateFileList()
        {
            var files = await DependencyService.Get<IFileWorker>().GetFilesAsync();
            List<GameInfo> games = new List<GameInfo>();

            foreach (var fileName in files)
            {
                var splitted = fileName.Substring(0, fileName.Length - 4).Split('|');
                if (splitted.Length < 2)
                {
                    continue;
                }
                games.Add(new GameInfo { Time = splitted[0], Title = splitted[1], GameDuration = $"Game duration {splitted[2]}", FullName = fileName });
            }
            Games = games;
            BindingContext = this;
            gamesList.ItemsSource = Games;
            
            gamesList.SelectedItem = null;
        }
    }
}