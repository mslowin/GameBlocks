using GameBlocks.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GameBlocks.Classes
{
    /// <summary>
    /// Class representing a TicTacToe game.
    /// </summary>
    public class TicTacToe
    {
        /// <summary>
        /// Name of a stream where game data is published.
        /// </summary>
        public static string StreamName { get; } = "GameTicTacToe";

        /// <summary>
        /// TicTacToeGameWindow object to modify window's 
        /// propherties (e.g. to change TextBlocks on the window).
        /// </summary>
        public static TicTacToeGameWindow? TicTacToeGameWindow { get; set;  }

        /// <summary>
        /// Key consisting of players' logins to publish data with.
        /// </summary>
        public string GameKey { get; set; }

        /// <summary>
        /// User's login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User's symbol (ether "X" or "O").
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Opponent's symbol (ether "X" or "O").
        /// </summary>
        public string OpponentsSymbol { get; set; }

        /// <summary>
        /// A two dimentional array forming game grid.
        /// x,y
        /// 0,0 | 0,1 | 0,2
        /// 1,0 | 1,1 | 1,2
        /// 2,0 | 2,1 | 2,2
        /// </summary>
        public string[,] Grid { get; set; } = new string[3,3];


        /// <summary>
        /// Constructor of a TicTacToe class when symbol has already been drawn.
        /// </summary>
        /// <param name="ticTacToeGameWindow">TicTacToeGameWindow object to modify window's propherties.</param>
        /// <param name="gameKey">Key consisting of players' logins to publish data with.</param>
        /// <param name="login">User's login.</param>
        /// <param name="symbol">User's symbol (ether "X" or "O").</param>
        public TicTacToe(TicTacToeGameWindow ticTacToeGameWindow, string gameKey, string login, string symbol)
        {
            TicTacToeGameWindow = ticTacToeGameWindow;
            GameKey = gameKey;
            Login = login;
            Symbol = symbol;
            if (Symbol == "X")
            {
                OpponentsSymbol = "O";
            }
            else
            {
                OpponentsSymbol = "X";
                ticTacToeGameWindow.Dispatcher.Invoke(() => { ticTacToeGameWindow.MoveTextBox.IsEnabled = false; });
                ticTacToeGameWindow.Dispatcher.Invoke(() => { ticTacToeGameWindow.SubmitButton.IsEnabled = false; });
            }
        }

        /// <summary>
        /// Constructor of a TicTacToe class but symbol ("X" or "O") is determined by opponents symbol.
        /// </summary>
        /// <param name="ticTacToeGameWindow">TicTacToeGameWindow object to modify window's propherties.</param>
        /// <param name="gameKey">Key consisting of players' logins to publish data with.</param>
        /// <param name="login">User's login.</param>
        public TicTacToe(TicTacToeGameWindow ticTacToeGameWindow, string gameKey, string login)
        {
            TicTacToeGameWindow = ticTacToeGameWindow;
            GameKey = gameKey;
            Login = login;
            OpponentsSymbol = CheckSymbol();
            if (OpponentsSymbol == "X")
            {
                Symbol = "O";
                ticTacToeGameWindow.Dispatcher.Invoke(() => { ticTacToeGameWindow.MoveTextBox.IsEnabled = false; });
                ticTacToeGameWindow.Dispatcher.Invoke(() => { ticTacToeGameWindow.SubmitButton.IsEnabled = false; });
            }
            else
            { 
                Symbol = "X";
            }
        }

        /// <summary>
        /// Runns multichain command and checks if opponent has made a move.
        /// </summary>
        /// <param name="ticTacToeGameWindow">TicTacToeGameWindow object to modify window's propherties.</param>
        /// <param name="cancellationToken">Token that can be used by other threads to cancel the loop.</param>
        public bool UpdateGame(TicTacToeGameWindow ticTacToeGameWindow, CancellationToken cancellationToken)
        {
            while (true)
            {
                (string output, _) = MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, $"liststreamkeyitems {StreamName} {GameKey}");
                List<string> moves = ExtensionsMethods.SearchInJson(output, "place");
                List<string> symbols = ExtensionsMethods.SearchInJson(output, "symbol");

                if (symbols.Count > 0 && symbols.Last() == OpponentsSymbol)  // if opponent moved
                {
                    int x = int.Parse(moves.Last().Remove(1, 2));  // 1,2 -> 1
                    int y = int.Parse(moves.Last().Remove(0, 2));  // 1,2 -> 2
                    Coordinates coordinates = new(x, y);
                    UpdateGrid(symbols.Last(), coordinates);
                    ticTacToeGameWindow.Dispatcher.Invoke(() => { ticTacToeGameWindow.GameAreaTextBlock.Text = DisplayGrid(); });
                    if (IsGameOver())
                    {
                        MultiChainClient.PublishToStream(StreamName, GameKey,
                            $"{{\"\"\"json\"\"\":{{" +
                            $"\"\"\"login\"\"\":\"\"\"{GlobalVariables.UserAccount!.Login}\"\"\"," +
                            $"\"\"\"symbol\"\"\":\"\"\"{Symbol}\"\"\"," +
                            $"\"\"\"satus\"\"\":\"\"\"WYGRANA\"\"\"}}}}");
                        return false;  // returning false as the opponen won
                    }

                    TicTacToeGameWindow.AllMoves.Add(coordinates);
                    TicTacToeGameWindow.OpponentsMoveCoordinates = coordinates;
                    TicTacToeGameWindow.DidOpponentMove = true;
                    ticTacToeGameWindow.Dispatcher.Invoke(() => { ticTacToeGameWindow.MoveTextBox.IsEnabled = true; });
                    ticTacToeGameWindow.Dispatcher.Invoke(() => { ticTacToeGameWindow.SubmitButton.IsEnabled = true; });

                }
                else if (symbols.Count > 0 && symbols.Last() == Symbol) // if the user moved
                {
                    int x = int.Parse(moves.Last().Remove(1, 2));  // 1,2 -> 1
                    int y = int.Parse(moves.Last().Remove(0, 2));  // 1,2 -> 2
                    Coordinates coordinates = new(x, y);
                    UpdateGrid(symbols.Last(), coordinates);
                    ticTacToeGameWindow.Dispatcher.Invoke(() => { ticTacToeGameWindow.GameAreaTextBlock.Text = DisplayGrid(); });
                    if (IsGameOver())
                    {
                        MultiChainClient.PublishToStream(StreamName, GameKey,
                            $"{{\"\"\"json\"\"\":{{" +
                            $"\"\"\"login\"\"\":\"\"\"{GlobalVariables.UserAccount!.Login}\"\"\"," +
                            $"\"\"\"symbol\"\"\":\"\"\"{Symbol}\"\"\"," +
                            $"\"\"\"satus\"\"\":\"\"\"WYGRANA\"\"\"}}}}");
                        return true;  // returning true as the player won
                    }

                    TicTacToeGameWindow.AllMoves.Add(coordinates);
                    TicTacToeGameWindow.DidOpponentMove = false;
                    ticTacToeGameWindow.Dispatcher.Invoke(() => { ticTacToeGameWindow.MoveTextBox.IsEnabled = false; });
                    ticTacToeGameWindow.Dispatcher.Invoke(() => { ticTacToeGameWindow.SubmitButton.IsEnabled = false; });
                }
                else  // this will be the first move
                {
                    TicTacToeGameWindow.DidOpponentMove = true;
                }
                // TODO: ending the game with X button
                if (cancellationToken.IsCancellationRequested)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Updates game grid with a symbol on specified coordinates.
        /// </summary>
        /// <param name="symbol">Symbol to put into the grid.</param>
        /// <param name="coordinates">Coordinates where to put the symbol in the grid</param>
        private void UpdateGrid(string symbol, Coordinates coordinates)
        {
            Grid[coordinates.X, coordinates.Y] = symbol;
        }

        /// <summary>
        /// Checks whether someone has won the game.
        /// </summary>
        /// <returns>True if game is over, otherwise false.</returns>
        private bool IsGameOver()
        {
            // Checking whether the user had won:
            // Checking rows:
            if (Grid[0, 0] == Symbol && Grid[0, 1] == Symbol && Grid[0, 2] == Symbol) { return true; }
            if (Grid[1, 0] == Symbol && Grid[1, 1] == Symbol && Grid[1, 2] == Symbol) { return true; }
            if (Grid[2, 0] == Symbol && Grid[2, 1] == Symbol && Grid[2, 2] == Symbol) { return true; }

            // Checking columns:
            if (Grid[0, 0] == Symbol && Grid[1, 0] == Symbol && Grid[2, 0] == Symbol) { return true; }
            if (Grid[0, 1] == Symbol && Grid[1, 1] == Symbol && Grid[2, 1] == Symbol) { return true; }
            if (Grid[0, 2] == Symbol && Grid[1, 2] == Symbol && Grid[2, 2] == Symbol) { return true; }

            // Checking diagonals:
            if (Grid[0, 0] == Symbol && Grid[1, 1] == Symbol && Grid[2, 2] == Symbol) { return true; }
            if (Grid[2, 0] == Symbol && Grid[1, 1] == Symbol && Grid[0, 2] == Symbol) { return true; }

            // And checking whether opponent had won:
            // Checking rows:
            if (Grid[0, 0] == OpponentsSymbol && Grid[0, 1] == OpponentsSymbol && Grid[0, 2] == OpponentsSymbol) { return true; }
            if (Grid[1, 0] == OpponentsSymbol && Grid[1, 1] == OpponentsSymbol && Grid[1, 2] == OpponentsSymbol) { return true; }
            if (Grid[2, 0] == OpponentsSymbol && Grid[2, 1] == OpponentsSymbol && Grid[2, 2] == OpponentsSymbol) { return true; }

            // Checking columns:
            if (Grid[0, 0] == OpponentsSymbol && Grid[1, 0] == OpponentsSymbol && Grid[2, 0] == OpponentsSymbol) { return true; }
            if (Grid[0, 1] == OpponentsSymbol && Grid[1, 1] == OpponentsSymbol && Grid[2, 1] == OpponentsSymbol) { return true; }
            if (Grid[0, 2] == OpponentsSymbol && Grid[1, 2] == OpponentsSymbol && Grid[2, 2] == OpponentsSymbol) { return true; }

            // Checking diagonals:
            if (Grid[0, 0] == OpponentsSymbol && Grid[1, 1] == OpponentsSymbol && Grid[2, 2] == OpponentsSymbol) { return true; }
            if (Grid[2, 0] == OpponentsSymbol && Grid[1, 1] == OpponentsSymbol && Grid[0, 2] == OpponentsSymbol) { return true; }

            return false;
        }

        /// <summary>
        /// Displays TicTacToe grid in txt fotmat.
        /// </summary>
        /// <returns>TicTacToe grid in txt format.</returns>
        public string DisplayGrid()
        {
            string output = "";
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                output += "|";
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    if (Grid[i,j] == "X" || Grid[i, j] == "O")
                    {
                        output += $" {Grid[i, j]} |";
                    }
                    else
                    {
                        output += $"    |";
                    }
                }
                output += Environment.NewLine;
            }
            return output;
        }

        /// <summary>
        /// Publishes a move to the chain.
        /// </summary>
        /// <param name="coords">Coordinates on the grid where symbol was put.</param>
        public void PublishMove(Coordinates coords)
        {
            MultiChainClient.PublishToStream(StreamName, GameKey,
                $"{{\"\"\"json\"\"\":{{" +
                $"\"\"\"login\"\"\":\"\"\"{GlobalVariables.UserAccount!.Login}\"\"\"," +
                $"\"\"\"symbol\"\"\":\"\"\"{Symbol}\"\"\"," +
                $"\"\"\"place\"\"\":\"\"\"{coords.X},{coords.Y}\"\"\"}}}}");
        }

        /// <summary>
        /// Randomly draws a symbol (ether "X" or "O") and publishes it to a specified stream.
        /// </summary>
        /// <param name="gameKey">Key consisting of players' logins to publish data with.</param>
        /// <returns>Drawn symbol in string type ("X" or "O").</returns>
        public static string DrawSymbol(string gameKey)
        {
            // TODO: later add logic to randomly select X or O
            string drawnSymbol = "X";
            MultiChainClient.PublishToStream(StreamName, gameKey,
                $"{{\"\"\"json\"\"\":{{" +
                $"\"\"\"login\"\"\":\"\"\"{GlobalVariables.UserAccount!.Login}\"\"\"," +
                $"\"\"\"drawn\"\"\":\"\"\"{drawnSymbol}\"\"\"}}}}");
            return drawnSymbol;
        }

        /// <summary>
        /// Checks (within the stream) which symbol has been drawn by the opponent.
        /// </summary>
        /// <returns>Opponents symbol in string format ("X" or "O").</returns>
        private string CheckSymbol()
        {
            Task.Delay(5000);  // Gives time for the firs person to publish it's symbol
            (string output, _) = MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, $"liststreamkeyitems {StreamName} {GameKey}");
            List<string> opponentsSymbol = ExtensionsMethods.SearchInJson(output, "drawn");
            return opponentsSymbol.Last();
        }
    }
}
