using System;
using Sudoku.CustomProperties;
using System.Threading.Tasks;
using static Sudoku.Generator;
using static Sudoku.Сorrectness;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sudoku
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        private string name;
        private string dif;

        Grid playGround;
        public GamePage(string name, string dif)
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

            this.name = name;
            this.dif = dif;

            DisplayTime();
            playGround = StartGame(dif);
            frame.Content = playGround;
            LabelTapOption();
            Numbers();
        }

        // Event when 1 of 81 cells tapped, change the color of the tapped cell to LightBlue 
        //remember last tapped cell and change it color on its base color, when tapped another one 
        int lastIndex = 0;
        int counter = 0;
        Color lastColor;
        private void TapGesture_Label(object sender, EventArgs e)
        {
            var label = sender as TagLabel;
            var parentGrid = (Grid)label.Parent;
            var index = parentGrid.Children.IndexOf(label);

            if (counter != 0)
            {
                parentGrid.Children[lastIndex].BackgroundColor = lastColor;
            }

            lastIndex = parentGrid.Children.IndexOf(label);
            lastColor = label.BackgroundColor;

            label.BackgroundColor = Color.LightBlue;
            counter++;         
        }


        //Create Grid of buttons with numbers from 1 to 9, and 10th button is delete-button
        public void Numbers()
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
        bool _canClose = true;
        protected override bool OnBackButtonPressed()
        {
            if (_canClose)
            {
                ShowExitDialog();
            }
            return _canClose;
        }

        //Display alert which protect user from accidental press of a button
        private async void ShowExitDialog()
        {
            var answer = await DisplayAlert("Leave current game!", "Are you sure?", "Yes", "No");
            if (answer)
            {
                await Navigation.PopToRootAsync();
            }
        }

        //Display name of player, difficult, time which passed from start of game in title
        bool alive = true;
        DateTime dt = DateTime.Parse("00:00");
        string currentTime = "";
        private async void DisplayTime()
        {
            while (alive)
            {
                dt = dt.AddSeconds(1);
                currentTime = dt.ToString("mm:ss");
                Title = $"{name} - {dif} - Your time: {currentTime}";
                await Task.Delay(1000);
            }
        }

        TapGestureRecognizer tapGesture = new TapGestureRecognizer
        {
            NumberOfTapsRequired = 1,
        };

        //Bind tapGestureRecognaiser whith all labels in playground Grid 
        public void LabelTapOption()
        {
            foreach (TagLabel label in (frame.Content as Grid).Children)
            {
                label.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                label.FontAttributes = FontAttributes.Bold;
                label.HorizontalTextAlignment = TextAlignment.Center;
                label.VerticalTextAlignment = TextAlignment.Center;
                label.TextColor = Color.Black;


                label.GestureRecognizers.Add(tapGesture);
            }
            tapGesture.Tapped += TapGesture_Label;
        }
       
        //Event whan user click one of 10th button which contain numbers and delete-button 
        //and set text of button into current activ cell
        private void Button_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var parentGrid = (Grid)((button).Parent);
            var label = (TagLabel)playGround.Children[lastIndex];
            var hasEmptyCell = true;

            if (label.Tag == "play")
            {
                if (parentGrid.Children.IndexOf(button) != parentGrid.Children.Count - 1)
                {
                    label.FontAttributes = FontAttributes.None;
                    label.Text = button.Text;

                    var isCorrectNumber = IsCorrectNumber(lastIndex, label.Text, playGround);

                    //DisplayAlert("Is correct?", $"{isCorrectNumber}", "Cancel");

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

                    //Show Winner modal page with scores : time of game duration, name and difficult 
                    if (hasEmptyCell == false && isCorrectNumber)
                    {
                        Navigation.PushModalAsync(new WinnerPage(name, dif, currentTime));
                    }
                }
                else
                {
                    label.Text = "";
                }
            }
        }


        private void Undo_Clicked(object sender, EventArgs e)
        {

        }

        //Event whan clicked NewGame button
        private void NewGame_Clicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }
    }
}