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
    /// Interaction logic for ChessGameWindow.xaml class.
    /// </summary>
    public partial class ChessGameWindow
    {
        /// <summary>
        /// Indicates whether the opponent has made a move.
        /// </summary>
        public static bool DidOpponentMove { get; set; } = false;

        /// <summary>
        /// Coordinates where opponent placed it's symbol.
        /// </summary>
        public static ChessCoordinates? OpponentsMoveCoordinates { get; set; }

        /// <summary>
        /// List with all moves from the begining of the game.
        /// </summary>
        public static List<ChessCoordinates> AllMoves { get; set; } = new();

        /// <summary>
        /// Key consisting of players' logins to publish data with (e.g. login_vs_login_5).
        /// </summary>
        public string GameKey { get; set; }

        /// <summary>
        /// Indicates if the user created or joind a waiting room.
        /// </summary>
        public bool WasThisUserFirst { get; set; }

        /// <summary>
        /// TicTacToe class object to perform operations on.
        /// </summary>
        public Chess GameChess { get; set; }

        /// <summary>
        /// Token used to stop background task from another thread.
        /// </summary>
        private CancellationTokenSource _ts = new();

        /// <summary>
        /// ChessGameWindow constructor.
        /// </summary>
        /// <param name="gameKey">Key consisting of players' logins to publish data with.</param>
        /// <param name="wasThisUserFirst">Indicates if the user created or joind a waiting room.</param>
        public ChessGameWindow(string gameKey, bool wasThisUserFirst)
        {
            InitializeComponent();
            GameKey = gameKey;
            WasThisUserFirst = wasThisUserFirst;
            GameChess = StartGameChess(this, gameKey, wasThisUserFirst);
            YourColorTextBox.Text = GameChess.Color;

            CancellationToken ct = _ts.Token;

            Task<bool>.Factory.StartNew(() => GameChess.UpdateGame(this, ct), ct).ContinueWith((tsk) =>
            {
                // displaying results window:
                var didUserWin = tsk.Result;
                ResultsWindow resultsWindow = new(didUserWin);
                resultsWindow.ShowDialog();
                Close();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Clears information textbox after changing destination text box.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void ToTexTBox_TextChanged(object sender, RoutedEventArgs e)
        {
            InformationTextBlock.Text = "";
        }

        /// <summary>
        /// Clears information textbox after changing starting coordinates text box.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void FromTexTBox_TextChanged(object sender, RoutedEventArgs e)
        {
            InformationTextBlock.Text = "";
        }

        /// <summary>
        /// Creates Chess class object.
        /// </summary>
        /// <param name="chessGameWindow">ChessGameWindow object to modify window's propherties.</param>
        /// <param name="gameKey">Key consisting of players' logins to publish data with.</param>
        /// <param name="wasThisUserFirst">Indicates if the user created or joind a waiting room.</param>
        /// <returns>New Chess class object.</returns>
        private Chess StartGameChess(ChessGameWindow chessGameWindow, string gameKey, bool wasThisUserFirst)
        {
            if (wasThisUserFirst)
            {
                Chess gameChess = new(chessGameWindow, gameKey, GlobalVariables.UserAccount!.Login);
                return gameChess;
            }
            else
            {
                Chess gameChess = new(chessGameWindow, gameKey, GlobalVariables.UserAccount!.Login, Chess.DrawColor(gameKey));
                return gameChess;
            }
        }

        /// <summary>
        /// Submits user's move.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (FromTextBox.Text == "" || ToTextBox.Text == "")
            {
                InformationTextBlock.Text = "Coordinates needed";
            }
            else
            {
                string fromCoordinates = FromTextBox.Text;
                string ToCoordinates = ToTextBox.Text;
                int oldY = int.Parse(fromCoordinates.Remove(0, 2));
                int oldX = int.Parse(fromCoordinates.Remove(1, 2));
                int newY = int.Parse(ToCoordinates.Remove(0, 2));
                int newX = int.Parse(ToCoordinates.Remove(1, 2));
                GameChess.PublishMove(new(oldX, oldY, newX, newY));
            }
        }
    }
}
