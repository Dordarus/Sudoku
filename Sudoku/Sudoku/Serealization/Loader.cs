using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using static Sudoku.Generator;

using Newtonsoft.Json.Linq;

using Xamarin.Forms;


namespace Sudoku
{
    static class Loader
    {
        public static int IndexOfRedLabel { get; set; }
       
        public static async Task<Grid> LoadGame(string fileName)
        {
            var fromFile = await DependencyService.Get<IFileWorker>().LoadTextAsync(fileName);
            var listClass = await DeserializePlayGround(fromFile);

            IndexOfRedLabel = IndexRedLabel(listClass.Colors);

            return Filler(listClass.Labels);
        }

        private static Task<ListClass> DeserializePlayGround(string serialized)
        {
            var jobject = JObject.Parse(serialized);
            var labeltList = jobject.SelectToken("Labels").Select(jt => jt.ToObject<MyLabel>()).ToList();
            var myColorList = jobject.SelectToken("Colors").Select(jt => jt.ToObject<MyColor>()).ToList();

            List<Color> colorList = new List<Color>();

            foreach (var item in myColorList)
            {
                colorList.Add(new Color(item.R, item.G, item.B, item.A));
            }

            ListClass listClass = new ListClass
            {
                Labels = labeltList,
                Colors = colorList
            };

            return Task.FromResult(listClass);
        }

        public static async Task<List<WinnerInfo>> LoadLeaderboard()
        {
            var fromFile = await DependencyService.Get<IFileWorker>().LoadTextAsync("Leaderboard.dat");

            if (fromFile != "")
            {
                var winnerList = await DeserializeLeaderboard(fromFile);
                return winnerList.Winners;
            }
            else
            {
                return new List<WinnerInfo>();
            }
        }

        private static Task<WinnerList> DeserializeLeaderboard(string serialized)
        {
            var jobject = JObject.Parse(serialized);
            var winnersList = jobject.SelectToken("Winners").Select(jt => jt.ToObject<WinnerInfo>()).ToList();

            return Task.FromResult(new WinnerList { Winners = winnersList });
        }


        private static int IndexRedLabel(List<Color> colors)
        {
            foreach (Color color in colors)
            {
                if (color == Color.Red)
                {
                    return colors.IndexOf(color);
                }
            }
            return -1;
        }
    }
}