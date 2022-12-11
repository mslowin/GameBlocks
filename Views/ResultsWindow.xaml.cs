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

namespace GameBlocks.Views
{
    /// <summary>
    /// Logika interakcji dla klasy ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow
    {
        public ResultsWindow(bool didUserWin)
        {
            InitializeComponent();
            if (didUserWin)
            {
                ResultTextBlock.Text = "You Win!";
                ResultTextBlock.Foreground = Brushes.Gold;
                PointsTextBlock.Text = "+30 points";
                PointsTextBlock.Foreground = Brushes.Gold;
                Points.AddPoints("30");
            }
            else
            {
                ResultTextBlock.Text = "You Loose :(";
                ResultTextBlock.Foreground = Brushes.Red;
                PointsTextBlock.Text = "-30 points";
                PointsTextBlock.Foreground = Brushes.Red;
                Points.AddPoints("-30");
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
