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

namespace GameBlocks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// MainWindow constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Setup? setup = ExtensionsMethods.ReadSetupFile();
            GlobalVariables.ChainName = setup.ChainName;
            //Chain chain = MultiChainClient.InitializeChain(GlobalVariables.ChainName);
        }

        /// <summary>
        /// Using multi chain command to start the node.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            // Starting a Node
            MultiChainClient.RunCommand("multichaind", GlobalVariables.ChainName, "-daemon");
            liststreamsButton.IsEnabled = true;
        }

        /// <summary>
        /// Using multi chain command to list streams.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void Liststreams_Button_Click(object sender, RoutedEventArgs e)
        {
            string output, err;
            (output, err) = MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, "liststreams");
            textBox1.Text = output;
        }

        /// <summary>
        /// Using multi chain command to stop the running node.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            // Stopping a Node
            MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, "stop");
            liststreamsButton.IsEnabled = false;
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
