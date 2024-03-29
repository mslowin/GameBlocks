﻿using GameBlocks.Classes;
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
    public partial class TicTacToeGameWindow
    {
        /// <summary>
        /// Indicates whether the opponent has made a move.
        /// </summary>
        public static bool DidOpponentMove { get; set; } = false;

        /// <summary>
        /// Coordinates where opponent placed it's symbol.
        /// </summary>
        public static Coordinates? OpponentsMoveCoordinates { get; set; }

        /// <summary>
        /// List with all moves from the begining of the game.
        /// </summary>
        public static List<Coordinates> AllMoves { get; set; } = new();

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
        public TicTacToe GameTicTacToe { get; set; }

        /// <summary>
        /// Token used to stop background task from another thread.
        /// </summary>
        private CancellationTokenSource _ts = new();

        /// <summary>
        /// TicTacToeGameWindow constructor.
        /// </summary>
        /// <param name="gameKey">Key consisting of players' logins to publish data with.</param>
        /// <param name="wasThisUserFirst">Indicates if the user created or joind a waiting room.</param>
        public TicTacToeGameWindow(string gameKey, bool wasThisUserFirst)
        {
            InitializeComponent();
            GameKey = gameKey;
            WasThisUserFirst = wasThisUserFirst;
            GameTicTacToe = StartGameTicTacToe(this, gameKey, wasThisUserFirst);
            YourSymbolTextBox.Text = GameTicTacToe.Symbol;

            CancellationToken ct = _ts.Token;

            Task<bool>.Factory.StartNew(() => GameTicTacToe.UpdateGame(this, ct), ct).ContinueWith((tsk) =>
            {
                // displaying results window:
                var didUserWin = tsk.Result;
                ResultsWindow resultsWindow = new(didUserWin);
                resultsWindow.ShowDialog();
                Close();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Creates TicTacToe class object.
        /// </summary>
        /// <param name="ticTacToeGameWindow">TicTacToeGameWindow object to modify window's propherties.</param>
        /// <param name="gameKey">Key consisting of players' logins to publish data with.</param>
        /// <param name="wasThisUserFirst">Indicates if the user created or joind a waiting room.</param>
        /// <returns>New TicTacToe class object.</returns>
        public static TicTacToe StartGameTicTacToe(TicTacToeGameWindow ticTacToeGameWindow, string gameKey, bool wasThisUserFirst)
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
                int y = int.Parse(coordinates.Remove(0, 2));
                int x = int.Parse(coordinates.Remove(1, 2));
                GameTicTacToe.PublishMove(new(x, y));
            }
        }
    }
}
