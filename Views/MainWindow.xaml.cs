using GameBlocks.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFCustomMessageBox;

namespace GameBlocks.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml class.
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Indicates whether the window is visible or hidden;
        /// </summary>
        private bool _isVisible = true;

        /// <summary>
        /// MainWindow constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Setup? setup = ExtensionsMethods.ReadSetupFile();
            GlobalVariables.ChainName = setup!.ChainName;
            GlobalVariables.PathToMultichainFolder = setup!.PathToMultichainFolder;
            GlobalVariables.MainChain = MultiChainClient.InitializeChain(GlobalVariables.ChainName); // Starting a Node and gathering infotmation
            currentChainTextBlock.Text = setup.ChainName;
        }

        /// <summary>
        /// Checking the chain for matching information in UsersStream to log into an account.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void LogIn_Button_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text;
            string password = passwordTextBox.Password;
            bool credentialsOk = GlobalVariables.MainChain!.LogIntoChainSuccess(login, password);

            if(credentialsOk)
            {
                GlobalVariables.UserAccount = new Account(login, password);     // creation of an account object
                GameChooseWindow gameChooseWindow = new();
                _isVisible = false;
                Visibility = Visibility.Hidden;
                logInButton.IsEnabled = false;
                gameChooseWindow.Show();
            }
            else
            {
                loginErrorTextBlock.Text = "Invalid login or password";
            }
        }

        /// <summary>
        /// Handlig keyboard buttons clicks.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                LogIn_Button_Click(sender, e);
            }
        }

        /// <summary>
        /// Handling the window closing event.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_isVisible)
            {
                var result = MessageBox.Show("Do you really want to exit?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
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
