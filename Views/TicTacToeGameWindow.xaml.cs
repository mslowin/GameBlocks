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
    /// Interaction logic for TicTacToeGameWindow.xaml class.
    /// </summary>
    public partial class TicTacToeGameWindow : Window
    {
        /// <summary>
        /// Indicates whether the opponent has made a move.
        /// </summary>
        public static bool _didOpponentMove { get; set; } = false;

        /// <summary>
        /// Coordinates where opponent placed it's symbol.
        /// </summary>
        public static Coordinates? _opponentsMoveCoordinates { get; set; }

        /// <summary>
        /// List with all moves from the begining of the game.
        /// </summary>
        public static List<Coordinates> _allMoves { get; set; } = new();

        /// <summary>
        /// Key consisting of players' logins to publish data with (e.g. login_vs_login_5).
        /// </summary>
        public string _gameKey { get; set; }

        /// <summary>
        /// Indicates if the user created or joind a waiting room.
        /// </summary>
        public bool _wasThisUserFirst { get; set; }

        /// <summary>
        /// TicTacToe class object to perform operations on.
        /// </summary>
        public TicTacToe _gameTicTacToe { get; set; }

        /// <summary>
        /// Token used to stop background task from another thread.
        /// </summary>
        private CancellationTokenSource ts = new();

        /// <summary>
        /// TicTacToeGameWindow constructor.
        /// </summary>
        /// <param name="gameKey">Key consisting of players' logins to publish data with.</param>
        /// <param name="wasThisUserFirst">Indicates if the user created or joind a waiting room.</param>
        public TicTacToeGameWindow(string gameKey, bool wasThisUserFirst)
        {
            InitializeComponent();
            _gameKey = gameKey;
            _wasThisUserFirst = wasThisUserFirst;
            _gameTicTacToe = StartGameTicTacToe(this, gameKey, wasThisUserFirst);

            CancellationToken ct = ts.Token;

            Task.Factory.StartNew(() => _gameTicTacToe.UpdateGame(this, ct), ct).ContinueWith((tsk) =>
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

        /// <summary>
        /// In a loop checks if the opponent has made a move (_didOpponentMove variable).
        /// </summary>
        /// <param name="cancellationToken">Token used to stop background task from another thread.</param>
        private void WaitForYourTurn(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (_didOpponentMove)  // when opponent moved
                {
                    Dispatcher.Invoke(() => { MoveTextBox.IsEnabled = true; });
                    Dispatcher.Invoke(() => { SubmitButton.IsEnabled = true; });
                }
                else if (!_didOpponentMove)  // when opponent didnt move
                {
                    Dispatcher.Invoke(() => { MoveTextBox.IsEnabled = false; });
                    Dispatcher.Invoke(() => { SubmitButton.IsEnabled = false; });
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Creates TicTacToe class object.
        /// </summary>
        /// <param name="ticTacToeGameWindow">TicTacToeGameWindow object to modify window's propherties.</param>
        /// <param name="gameKey">Key consisting of players' logins to publish data with.</param>
        /// <param name="wasThisUserFirst">Indicates if the user created or joind a waiting room.</param>
        /// <returns>New TicTacToe class object.</returns>
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

        /// <summary>
        /// Submits user's move.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
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
                int y = int.Parse(coordinates.Remove(0, 2));  // 1,2 -> 2
                _gameTicTacToe.PublishMove(new(x, y));
            }
        }
    }
}
