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
        public MainWindow()
        {
            InitializeComponent();
            GlobalVariables.ChainName = ExtensionsMethods.ReadSetupFile();
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            // Starting a Node
            MultiChainClient.RunCommand("multichaind", GlobalVariables.ChainName, "-daemon");
            liststreamsButton.IsEnabled = true;
        }

        private void Liststreams_Button_Click(object sender, RoutedEventArgs e)
        {
            // Using multi chain command (The node must be already running)
            string output, err;
            (output, err) = MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, "liststreams");
            textBox1.Text = output;
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            // Stopping a Node
            MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, "stop");
            liststreamsButton.IsEnabled = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = CustomMessageBox.Show("Do you really want to exit?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Stopping a Node
            MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, "stop");
        }
    }
}
