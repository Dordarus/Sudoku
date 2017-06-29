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
    public partial class OldGamesPage : ContentPage
    {
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
            string filename = (string)((MenuItem)sender).BindingContext;
            
            await DependencyService.Get<IFileWorker>().DeleteAsync(filename);           
            await UpdateFileList();
        }

        async Task UpdateFileList()
        {           
            filesList.ItemsSource = await DependencyService.Get<IFileWorker>().GetFilesAsync();
            filesList.SelectedItem = null;
        }
    }
}