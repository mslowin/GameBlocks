using GameBlocks.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFCustomMessageBox;

namespace GameBlocks.Views
{
    /// <summary>
    /// Interaction logic for GameChooseWindow.xaml class.
    /// </summary>
    public partial class GameChooseWindow
    {
        /// <summary>
        /// Indicator which game is currently selected (0 -> TicTacToe, 1 -> Checkers)
        /// </summary>
        private int _selectedGame = 0;

        /// <summary>
        /// Indicator which game is currently selected (0 -> TicTacToe, 1 -> Checkers)
        /// </summary>
        private readonly List<string> _availableGames = new() { "TicTacToe", "Checkers" };

        /// <summary>
        /// GameChooseWindow constructor.
        /// </summary>
        public GameChooseWindow()
        {
            InitializeComponent();
            PlayerLoginTextBlock.Text = GlobalVariables.UserAccount!.Login;
        }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            string gameName = _availableGames[_selectedGame];
            (bool wasGameStarted, bool wasThisUserFirst, string? gameKey) = ExtensionsMethods.CreateOrJoinWaitingRoom(gameName);

            if (wasGameStarted && gameKey != null)
            {
                ExtensionsMethods.StartGame(gameName, gameKey, wasThisUserFirst);
            }
        }

        /// <summary>
        /// Selects next available game.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void Right_Arrow_Button_Click(object sender, RoutedEventArgs e)
        {
            _selectedGame++;
            (_selectedGame, GameNameTextBlock.Text) = ExtensionsMethods.ChooseGameTitle(_selectedGame, _availableGames);
            string resourcePath = $"Views/{GameNameTextBlock.Text}_Icon.png";
            GameImage.Source = new BitmapImage(ExtensionsMethods.GetFullImageSource(resourcePath));
        }


        /// <summary>
        /// Selects previous available game.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void Left_Arrow_Button_Click(object sender, RoutedEventArgs e)
        {
            _selectedGame--;
            (_selectedGame, GameNameTextBlock.Text) = ExtensionsMethods.ChooseGameTitle(_selectedGame, _availableGames);
            string resourcePath = $"Views/{GameNameTextBlock.Text}_Icon.png";
            GameImage.Source = new BitmapImage(ExtensionsMethods.GetFullImageSource(resourcePath));
        }

        /// <summary>
        /// Handling the window closing event.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Do you really want to exit?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Handling an instance where the window was closed.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void Window_Closed(object sender, EventArgs e)
        {
            ExtensionsMethods.ExitApplication();
        }
    }
}
