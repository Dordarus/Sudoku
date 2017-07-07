using Xamarin.Forms;
using static Sudoku.GameSaver;

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
