using System.Collections.Generic;

using Sudoku.CustomProperties;

using Newtonsoft.Json;

using Xamarin.Forms;


namespace Sudoku
{
    static class GameSaver
    {
        static ListClass list = new ListClass();
        static List<MyLabel> ml = new List<MyLabel>();
        static List<Color> colors = new List<Color>();

        public static async void SaveGame(Grid grid, string info)
        {
            foreach (TagLabel label in grid.Children)
            {
                ml.Add(new MyLabel { Text = label.Text, Tag = label.Tag});
                colors.Add(label.BackgroundColor);
            }
            
            list.Labels = ml;
            list.Colors = colors;

            var serialized = Serialize();

            await DependencyService.Get<IFileWorker>().SaveTextAsync($"{info}.dat", serialized);          
        }
        
        private static string Serialize()
        {
            return JsonConvert.SerializeObject(list);
        }        
    }
}