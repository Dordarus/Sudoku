using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        async Task UpdateFileList()
        {
            var files = await DependencyService.Get<IFileWorker>().GetFilesAsync();
            List<Game> games = new List<Game>();

            foreach (var fileName in files)
            {
                var splited = fileName.Substring(0, fileName.Length - 4).Split('|');
                games.Add(new Game { Time = splited[0], Title = splited[1], GameDuration = $"Game duration {splited[2]}", FullName = fileName });
            }
            Games = games;
            BindingContext = this;
            gamesList.ItemsSource = Games;
            
            gamesList.SelectedItem = null;
        }
    }
}
