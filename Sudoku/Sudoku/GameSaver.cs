using System.Collections.Generic;
using System;

using Sudoku.CustomProperties;

using Newtonsoft.Json;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace Sudoku
{
    static class GameSaver
    {
        static ListClass list = new ListClass();

        public static async void SaveGame(Grid grid, string currentInfo, string startInfo)
        {
            List<MyLabel> ml = new List<MyLabel>();
            List<Color> colors = new List<Color>();

            foreach (TagLabel label in grid.Children)
            {
                var attributeName = Enum.GetName(typeof(FontAttributes), label.FontAttributes);
                ml.Add(new MyLabel { Text = label.Text, Tag = label.Tag, FontAttribute = attributeName });
                colors.Add(label.BackgroundColor);
            }

            list.Labels = ml;
            list.Colors = colors;

            var serialized = await Serialize();

            if (startInfo != "")
            {
                await DependencyService.Get<IFileWorker>().DeleteAsync($"{startInfo}.dat");
                await DependencyService.Get<IFileWorker>().SaveTextAsync($"{currentInfo}.dat", serialized);
            }               
            else
                await DependencyService.Get<IFileWorker>().SaveTextAsync($"{currentInfo}.dat", serialized);
        }
        
        private static Task<string> Serialize()
        {
            return Task.Run(() => JsonConvert.SerializeObject(list));
        }        
    }
}