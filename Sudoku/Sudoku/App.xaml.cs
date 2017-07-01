using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Sudoku
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            
        }

        //TODO: handle this event if player 
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        //TODO: and this
        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
