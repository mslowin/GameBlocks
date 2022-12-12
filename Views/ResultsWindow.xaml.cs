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
        /// <summary>
        /// Constructor of results window.
        /// </summary>
        /// <param name="didUserWin">Indicates whether the user has won.</param>
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

        /// <summary>
        /// Happens after clicking OK button.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
