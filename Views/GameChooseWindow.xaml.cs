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
    /// Logika interakcji dla klasy GameChooseWindow.xaml
    /// </summary>
    public partial class GameChooseWindow : Window
    {
        /// <summary>
        /// Indicator which game is currently selected (0 -> TicTacToe, 1 -> Checkers)
        /// </summary>
        public int _SelectedGame = 0;

        /// <summary>
        /// Indicator which game is currently selected (0 -> TicTacToe, 1 -> Checkers)
        /// </summary>
        public readonly List<string> _AvailableGames = new() { "TicTacToe", "Checkers" };

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
            bool IsGameStart = ExtensionsMethods.CreateOrJoinWaitingRoom(_AvailableGames[_SelectedGame]);
        }

        /// <summary>
        /// Selects next available game.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void Right_Arrow_Button_Click(object sender, RoutedEventArgs e)
        {
            _SelectedGame++;
            (_SelectedGame, GameNameTextBlock.Text) = ExtensionsMethods.ChooseGameTitle(_SelectedGame, _AvailableGames);
            string resourcePath = $"Views/{GameNameTextBlock.Text}_Icon.jpg";
            GameImage.Source = new BitmapImage(ExtensionsMethods.GetFullImageSource(resourcePath));
        }


        /// <summary>
        /// Selects previous available game.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void Left_Arrow_Button_Click(object sender, RoutedEventArgs e)
        {
            _SelectedGame--;
            (_SelectedGame, GameNameTextBlock.Text) = ExtensionsMethods.ChooseGameTitle(_SelectedGame, _AvailableGames);
            string resourcePath = $"Views/{GameNameTextBlock.Text}_Icon.jpg";
            GameImage.Source = new BitmapImage(ExtensionsMethods.GetFullImageSource(resourcePath));
        }

        /// <summary>
        /// Handling the window closing event.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = CustomMessageBox.Show("Do you really want to exit?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
            // Stopping a Node
            //MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, "stop");
            ExtensionsMethods.ExitApplication();
        }
    }
}
