using System.Linq;
using System.Collections.Generic;
using Sudoku.CustomProperties;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

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

        public static void SaveGame(Grid grid)
        {
            foreach (TagLabel label in grid.Children)
            {
                lig.Add(new LabelInGrid(label));
            }

            list.Labels = lig;

            var serialized = Serialize();
            Deserialize(serialized);
        }

        private static string Serialize()
        {
            return JsonConvert.SerializeObject(list);
        }

        // TODO: Use resultList to restore playGround
        private static void Deserialize(string serialized)
        {
            var jobject = JObject.Parse(serialized);
            var resultList = jobject.SelectToken("Labels").Select(jt => jt.ToObject<TagLabel>()).ToList();

           
        }
    }
}
