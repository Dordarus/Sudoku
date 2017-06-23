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
        Grid playGround;
        public GamePage(string name, string dif)
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

            DisplayTime(name, dif);
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
        private async void DisplayTime(string name, string dif)
        {
            var dt = DateTime.Parse("00:00");
            while (alive)
            {
                dt = dt.AddSeconds(1);
                Title = $"{name} - {dif} - Your time: {dt.ToString("mm:ss")}";
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

            if (label.Tag == "play")
            {
                if (parentGrid.Children.IndexOf(button) != parentGrid.Children.Count - 1)
                {
                    label.FontAttributes = FontAttributes.None;
                    label.Text = button.Text;

                    //var isCorrect = IsCorrectNumber(lastIndex, label.Text, playGround);

                    //DisplayAlert("Is correct?", $"{isCorrect}", "Cancel");
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