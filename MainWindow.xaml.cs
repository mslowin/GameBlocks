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
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            (_, _) = MultiChainClient.RunCommand("multichaind", "chain11", "-daemon");
            //textBox1.Text = output;
        }

        private void Liststreams_Button_Click(object sender, RoutedEventArgs e)
        {
            //Na razie chaina trzeba najpierw uruchomić ręcznie;
            string output, err;
            (output, err) = MultiChainClient.RunCommand("multichain-cli", "chain11", "liststreams");
            textBox1.Text = output;
        }

    }
}
