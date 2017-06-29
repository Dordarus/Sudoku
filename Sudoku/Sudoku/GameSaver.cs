using System.Linq;
using System.Collections.Generic;
using Sudoku.CustomProperties;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Sudoku
{
    public class LabelInGrid
    {
        public string Text { get; set; }
        public Color BackgroundColor { get; set; }
        public int Index { get; set; }
        public string Tag { get; set; }

        public LabelInGrid(TagLabel label)
        {
            Text = label.Text;
            BackgroundColor = label.BackgroundColor;
            Index = ((Grid)label.Parent).Children.IndexOf(label);
            Tag = label.Tag;
        }
    }

    public class ListClass
    {
        public List<LabelInGrid> Labels { get; set; }
    }

    static class GameSaver
    {
        static List<LabelInGrid> lig = new List<LabelInGrid>();
        static ListClass list = new ListClass();

        public static async void SaveGame(Grid grid, string info)
        {
            foreach (TagLabel label in grid.Children)
            {
                lig.Add(new LabelInGrid(label));
            }

            list.Labels = lig;

            var serialized = Serialize();

            await DependencyService.Get<IFileWorker>().SaveTextAsync($"{info}.dat", serialized);          
        } 

        //TODO: Use this method to load JSON-file and deserialize it into List and restore playground
        public static async Task<Grid> LoadGame()
        {
            var fromFile = await DependencyService.Get<IFileWorker>().LoadTextAsync("some name");
            var labelList = await Deserialize(fromFile);

            Grid playGround = new Grid { ColumnSpacing = 2, RowSpacing=2};

            var j = 0;
            var i = 0;
            foreach (TagLabel label in labelList)
            {
                if (j < 9)
                {
                    playGround.Children.Add(label, i, j++);
                }
                else
                {
                    i++;
                    j = 0;
                }
            }

           return playGround;
        }

        private static string Serialize()
        {
            return JsonConvert.SerializeObject(list);
        }

        private static Task<List<TagLabel>> Deserialize(string serialized)
        {
            var jobject = JObject.Parse(serialized);
            var resultList = jobject.SelectToken("Labels").Select(jt => jt.ToObject<TagLabel>()).ToList();

            return Task.FromResult(resultList);
        }
    }
}
