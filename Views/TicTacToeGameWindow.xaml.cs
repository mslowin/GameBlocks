using GameBlocks.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Logika interakcji dla klasy TicTacToeGameWindow.xaml
    /// </summary>
    public partial class TicTacToeGameWindow : Window
    {
        /// <summary>
        /// Token usd to stop background task from another task.
        /// </summary>
        private CancellationTokenSource ts = new();

        public static bool _didOpponentMove { get; set; } = false;
        public static Coordinates? _opponentsMoveCoordinates { get; set; }
        public static List<Coordinates> _allMoves { get; set; } = new();
        public string _gameKey { get; set; }
        public bool _wasThisUserFirst { get; set; }
        public TicTacToe _gameTicTacToe { get; set; }




        public TicTacToeGameWindow(string gameKey, bool wasThisUserFirst)
        {
            InitializeComponent();
            _gameKey = gameKey;
            _wasThisUserFirst = wasThisUserFirst;
            _gameTicTacToe = StartGameTicTacToe(this, gameKey, wasThisUserFirst);

            CancellationToken ct = ts.Token;

            Task.Factory.StartNew(() => _gameTicTacToe.UpdateGame(ct), ct).ContinueWith((tsk) =>
            {
                // Tu coś do zmiany bedzie
                Close();
            }, TaskScheduler.FromCurrentSynchronizationContext());

            Task.Factory.StartNew(() => WaitForYourTurn(ct), ct).ContinueWith((tsk) =>
            {
                // Tu coś do zmiany bedzie
                Close();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void WaitForYourTurn(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (_didOpponentMove == true)
                {
                    this.Dispatcher.Invoke(() => { MoveTextBox.IsEnabled = true; });
                    this.Dispatcher.Invoke(() => { SubmitButton.IsEnabled = true; });
                }
                else if (_didOpponentMove == false)
                {
                    this.Dispatcher.Invoke(() => { MoveTextBox.IsEnabled = false; });
                    this.Dispatcher.Invoke(() => { SubmitButton.IsEnabled = false; });
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        public TicTacToe StartGameTicTacToe(TicTacToeGameWindow ticTacToeGameWindow, string gameKey, bool wasThisUserFirst)
        {
            if (wasThisUserFirst)
            {
                TicTacToe gameTicTacToe = new(ticTacToeGameWindow, gameKey, GlobalVariables.UserAccount!.Login);
                return gameTicTacToe;
            }
            else
            {
                TicTacToe gameTicTacToe = new(ticTacToeGameWindow, gameKey, GlobalVariables.UserAccount!.Login, TicTacToe.DrawSymbol(gameKey));
                return gameTicTacToe;
            }
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (MoveTextBox.Text == "")
            {
                // Coordinates needed
            }
            else
            {
                string coordinates = MoveTextBox.Text;
                int x = int.Parse(coordinates.Remove(1, 2));  // 1,2 -> 1
                var test = coordinates.Remove(0, 2);
                int y = int.Parse(coordinates.Remove(0, 2));  // 1,2 -> 2
                _gameTicTacToe.PublishMove(new(x, y));
            }
        }
    }
}
