using Xamarin.Forms;
using static Sudoku.Saver;

namespace Sudoku
{
    public partial class App : Application
    {
        GamePage currentGame;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override async void OnStart()
        {
            bool exist = await DependencyService.Get<IFileWorker>().ExistsAsync("Leaderboard.dat");
            if(!exist)
            {
                await DependencyService.Get<IFileWorker>().SaveTextAsync("LeaderBoard.dat", "");
            }          
        }

        protected override void OnSleep()
        {
            currentGame = CurrentGame.CurrentGamePage;
            currentGame.alive = false;
            SaveGame(currentGame.playGround,currentGame.currentInfo, currentGame.startInfo);
            currentGame.startInfo = currentGame.currentInfo;
        }

        protected override void OnResume()
        {
            currentGame.alive = true;
        }
    }
}
