using GameBlocks.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Indicator which game is currently selected
        /// </summary>
        private int _SelectedGame = 0;

        /// <summary>
        /// GameChooseWindow constructor.
        /// </summary>
        public GameChooseWindow()
        {
            InitializeComponent();
            PlayerLoginTextBlock.Text = GlobalVariables.UserAccount!.Login;
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            // Method intentionally left empty.

        }

        private void Left_Arrow_Button_Click(object sender, RoutedEventArgs e)
        {
            // Method intentionally left empty.
        }

        private void Right_Arrow_Button_Click(object sender, RoutedEventArgs e)
        {
            // Method intentionally left empty.

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
            MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, "stop");
        }
    }
}
