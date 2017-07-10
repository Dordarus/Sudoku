using System;
using System.Collections.Generic;
using Sudoku.CustomProperties;
using System.Threading.Tasks;
using static Sudoku.Generator;
using static Sudoku.Сorrectness;
using static Sudoku.Saver;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sudoku
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        public string name;
        public string dif;
        public string startTime = "";
        public string savingDate = "";
        public string currentTime = "00:00";
        public Grid playGround;

        public GamePage(string name, string dif, Grid grid = null)
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

            this.name = name;
            this.dif = dif;

            if (grid == null)
                playGround = StartGame(dif);
            else
                playGround = grid;

            frame.Content = playGround;

            LabelTapOption();
            Numbers();

            Appearing += GamePage_Appearing;

            CurrentGame.CurrentGamePage = this;
        }

        public GamePage(string name, string dif, string duration, Grid grid, int index, string savingDate) 
            : this(name, dif, grid)
        {
            currentTime = duration;
            startTime = duration;
            this.savingDate = savingDate;

            if (index > 0)
            {
                var redLabel = (TagLabel)playGround.Children[index];
                Highlightning(redLabel, false);
            }          
        }

        private void GamePage_Appearing(object sender, EventArgs e)
        {
            var parentGrid = (Grid)numbers.Children[0];

            var counter = 0;

            foreach (Button b in parentGrid.Children)
            {
                foreach (TagLabel tagLabel in playGround.Children)
                {
                    if (tagLabel.Text == b.Text)
                    {
                        ++counter;
                    }
                }

                if (counter == 9)
                {
                    b.IsEnabled = false;
                }
                counter = 0;
            }
            DisplayTime();

            string dateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            currentInfo = $"{dateTime}|{name} {dif}|{currentTime}";

            if (savingDate != "")
                startInfo = $"{savingDate}|{name} {dif}|{startTime}";

        }

        private void TapGesture_Label(object sender, EventArgs e)
        {
            var label = (TagLabel)sender;
            int index = playGround.Children.IndexOf(label);
            var isCorrectNUmber = IsCorrectNumber(index, label.Text, playGround);
        
            Highlightning((TagLabel)sender, isCorrectNUmber);
        }

        /*Method work with data with send TapGesture_Label event, when 1 of 81 cells was tapped, changing the color of the tapped cell and 
        all labels with same numbers to LightBlue, remembered last tapped cell and all labels with same numbers and change their color on
        base color, when tapped another one*/
        private int lastIndex = -1;
        private Color lastColor;
        private Dictionary<int, Color> lastCells = new Dictionary<int, Color>();
        private void Highlightning(TagLabel tagLabel, bool isCorrectNumber)
        {
            var index = playGround.Children.IndexOf(tagLabel);

            foreach (var i in lastCells)
            {
                playGround.Children[i.Key].BackgroundColor = i.Value;
            }

            if (lastIndex >= 0)
            {
                playGround.Children[lastIndex].BackgroundColor = lastColor;
            }

            lastIndex = playGround.Children.IndexOf(tagLabel);
            lastColor = tagLabel.BackgroundColor;
            if (tagLabel.Text == "")
            {
                tagLabel.BackgroundColor = Color.LightBlue;
            }
            else
            {
                foreach (TagLabel label in playGround.Children)
                {
                    if (label.Text == tagLabel.Text)
                    {
                        var indexForeachLabel = playGround.Children.IndexOf(label);

                        if (lastCells.ContainsKey(indexForeachLabel))
                        {
                            lastCells.Remove(indexForeachLabel);
                        }

                        lastCells.Add(indexForeachLabel, label.BackgroundColor);

                        if (isCorrectNumber)
                            label.BackgroundColor = Color.LightBlue;
                        else
                            label.BackgroundColor = Color.Red;
                    }
                }
            }
        }

        //Create Grid of buttons with numbers from 1 to 9, and 10th button is delete-button
        private void Numbers()
        {
            Grid grid = new Grid { ColumnSpacing = 2 };

            var counter = 1;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i < 9)
                    {
                        grid.Children.Add(new Button
                        {
                            Text = $"{counter++}",
                            BackgroundColor = Color.LightGray,
                        }, j, i);
                    }
                }
            }
            grid.Children.Add(new Button
            {
                Text = "⌫",
                BackgroundColor = Color.LightGray,
            }, 4, 1);

            foreach (Button button in grid.Children)
            {
                button.Clicked += Button_Clicked;
            }
            numbers.Children.Add(grid);
        }

        //Triggered when user clicked hardware back-button call ShowDialog method
        private bool _canClose = true;
        protected override bool OnBackButtonPressed()
        {
            if (_canClose)
            {
                ShowExitDialog();
            }
            return _canClose;
        }


        public string currentInfo;
        public string startInfo; 
        //Display alert which protect user from accidental press of a button
        public async void ShowExitDialog()
        {
            bool saveGame = false;

            var leaveGame = await DisplayAlert("Leave current game!", "Are you sure?", "Yes", "No");

            if (leaveGame)
                saveGame = await DisplayAlert("Saving", "Save this game?", "Yes", "No");

            if (saveGame)
            {
               SaveGame(playGround, currentInfo, startInfo);
            }

            if (leaveGame)
                await Navigation.PopToRootAsync();
        }

        //Display name of player, difficult, time which passed from start of game in title
        public bool alive = true;
        private async void DisplayTime()
        {
            DateTime dt = DateTime.ParseExact(currentTime, "mm:ss", null);
            while (alive)
            {
                dt = dt.AddSeconds(1);
                currentTime = dt.ToString("mm:ss");
                Title = $"{name} - {dif} - Your time: {currentTime}";
                await Task.Delay(1000);
            }
        }

        private TapGestureRecognizer tapGesture = new TapGestureRecognizer();

        //Bind tapGestureRecognaiser whith all labels in playground Grid 
        private void LabelTapOption()
        {
            foreach (TagLabel label in (frame.Content as Grid).Children)
            {
                label.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                label.HorizontalTextAlignment = TextAlignment.Center;
                label.VerticalTextAlignment = TextAlignment.Center;
                label.TextColor = Color.Black;

                if (label.Tag == "play")
                {
                    label.PropertyChanged += Label_TextChanged;
                }

                label.GestureRecognizers.Add(tapGesture);
            }
            tapGesture.Tapped += TapGesture_Label;
        }

        //event whan TextProperty of Label changed, call TapGesture_Label event to show all same numbers on playground
        private void Label_TextChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var label = (TagLabel)sender;

            if (e.PropertyName == "Text" && label.Text != "")
            {
                int index = playGround.Children.IndexOf(label);

                var isCorrectNUmber = IsCorrectNumber(index, label.Text, playGround);

                Highlightning(label, isCorrectNUmber);
            }
        }

        //Event whan user click one of 10th button which contain numbers and delete-button 
        //and set operation options to buttons
        Stack<int> indexLog = new Stack<int>();
        Stack<string> numberLog = new Stack<string>();
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (lastIndex > 0)
            {
                var button = (Button)sender;
                var parentGrid = (Grid)((button).Parent);
                var label = (TagLabel)playGround.Children[lastIndex];
                var hasEmptyCell = true;

                if (label.Tag == "play")
                {
                    indexLog.Push(lastIndex);
                    numberLog.Push(label.Text);

                    if (parentGrid.Children.IndexOf(button) != parentGrid.Children.Count - 1)
                    {
                        var count = 0;

                        label.FontAttributes = FontAttributes.None;
                        label.Text = button.Text;

                        indexLog.Push(lastIndex);
                        numberLog.Push(label.Text);

                        buttonStackLayout.Children[0].IsEnabled = true;

                        var isCorrectNumber = IsCorrectNumber(lastIndex, label.Text, playGround);

                        //Cheking text of all labels, if there are no emty cells and last number is correct number - you are winner
                        foreach (TagLabel tagLabel in playGround.Children)
                        {
                            if (tagLabel.Text == "")
                            {
                                hasEmptyCell = true;
                                break;
                            }
                            else
                            {
                                hasEmptyCell = false;
                                continue;
                            }
                        }

                        foreach (TagLabel tagLabel in playGround.Children)
                        {
                            if (tagLabel.Text == button.Text)
                            {
                                if (++count == 9)
                                {
                                    button.IsEnabled = false;
                                }
                            }
                        }
                        //Show Winner modal page with scores : time of game duration, name and difficult 
                        if (hasEmptyCell == false && isCorrectNumber)
                        {
                            alive = false;
                            Navigation.PushAsync(new WinnerPage(name, dif, currentTime));
                        }
                    }
                    else
                    {
                        label.Text = "";
                        TapGesture_Label(label, new EventArgs());

                        indexLog.Push(lastIndex);
                        numberLog.Push(label.Text);
                    }

                    var counter = 0;

                    foreach (Button b in parentGrid.Children)
                    {
                        if (b.IsEnabled == false)
                        {
                            foreach (TagLabel tagLabel in playGround.Children)
                            {
                                if (tagLabel.Text == b.Text)
                                {
                                    ++counter;
                                }
                            }

                            if (counter < 9)
                            {
                                b.IsEnabled = true;
                            }
                        }
                    }
                }
            }
        }

        //Event whan user cklick Undo-button, call 2 stack collections(numbers and indexes of labels) 
        //which filled when the Button_Clicked event is called
        private void Undo_Clicked(object sender, EventArgs e)
        {
            if (indexLog.Count != 0)
            {
                var index = indexLog.Pop();
                var number = numberLog.Pop();

                var label = (TagLabel)playGround.Children[index];

                if (numberLog.Count != 0)
                {
                    label.Text = numberLog.Pop();
                    indexLog.Pop();

                    TapGesture_Label(label, new EventArgs());
                }               
            }

            if (indexLog.Count == 0)
            {
                ((Button)sender).IsEnabled = false;
            }
        }      

        //Event whan clicked NewGame button
        private void NewGame_Clicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }
    }
}