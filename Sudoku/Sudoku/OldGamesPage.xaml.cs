using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using static Sudoku.GameSaver;

namespace Sudoku
{
    public class Game
    {
        public string Title { get; set; }
        public string Time { get; set; }
        public string GameDuration { get; set; }
        public string FullName { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OldGamesPage : ContentPage
    {
        public List<Game> Games { get; set; }

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
            var context = ((Game)bindableObject.BindingContext);
            var name = context.FullName;

            await DependencyService.Get<IFileWorker>().DeleteAsync(name);
            await UpdateFileList();
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var game = (Game)e.Item;
            var fileName = game.FullName;           
            var gameDuration = game.GameDuration.Substring(14);

            var splited = game.Title.Split();
            var name = splited[0];
            var dif = splited[1];

            var playground = await LoadGame(fileName);
            await Navigation.PushAsync(new GamePage(name, dif, gameDuration, playground));
        }

        async Task UpdateFileList()
        {
            var files = await DependencyService.Get<IFileWorker>().GetFilesAsync();
            List<Game> games = new List<Game>();

            foreach (var fileName in files)
            {
                var splited = fileName.Substring(0, fileName.Length - 4).Split('|');
                //if (splited.Length<2)
                //{
                //    continue;
                //}
                games.Add(new Game { Time = splited[0], Title = splited[1], GameDuration = $"Game duration {splited[2]}", FullName = fileName });
            }
            Games = games;
            BindingContext = this;
            gamesList.ItemsSource = Games;
            
            gamesList.SelectedItem = null;
        }
    }
}