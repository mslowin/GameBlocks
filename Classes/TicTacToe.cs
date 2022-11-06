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
        public static string _streamName { get; } = "GameTicTacToe";

        /// <summary>
        /// TicTacToeGameWindow object to modify window's 
        /// propherties (e.g. to change TextBlocks on the window).
        /// </summary>
        public static TicTacToeGameWindow? _ticTacToeGameWindow { get; set;  }

        /// <summary>
        /// Key consisting of players' logins to publish data with.
        /// </summary>
        public string _gameKey { get; set; }

        /// <summary>
        /// User's login.
        /// </summary>
        public string _login { get; set; }

        /// <summary>
        /// User's symbol (ether "X" or "O").
        /// </summary>
        public string _symbol { get; set; }

        /// <summary>
        /// Opponent's symbol (ether "X" or "O").
        /// </summary>
        public string _opponentsSymbol { get; set; }

        /// <summary>
        /// A two dimentional array forming game grid.
        /// x,y
        /// 0,0 | 0,1 | 0,2
        /// 1,0 | 1,1 | 1,2
        /// 2,0 | 2,1 | 2,2
        /// </summary>
        public string[,] _grid { get; set; } = new string[3,3];


        /// <summary>
        /// Constructor of a TicTacToe class when symbol has already been drawn.
        /// </summary>
        /// <param name="ticTacToeGameWindow">TicTacToeGameWindow object to modify window's propherties.</param>
        /// <param name="gameKey">Key consisting of players' logins to publish data with.</param>
        /// <param name="login">User's login.</param>
        /// <param name="symbol">User's symbol (ether "X" or "O").</param>
        public TicTacToe(TicTacToeGameWindow ticTacToeGameWindow, string gameKey, string login, string symbol)
        {
            _ticTacToeGameWindow = ticTacToeGameWindow;
            _gameKey = gameKey;
            _login = login;
            _symbol = symbol;
            if (_symbol == "X") _opponentsSymbol = "O";
            else _opponentsSymbol = "X";
        }

        /// <summary>
        /// Constructor of a TicTacToe class but symbol ("X" or "O") is determined by opponents symbol.
        /// </summary>
        /// <param name="ticTacToeGameWindow">TicTacToeGameWindow object to modify window's propherties.</param>
        /// <param name="gameKey">Key consisting of players' logins to publish data with.</param>
        /// <param name="login">User's login.</param>
        public TicTacToe(TicTacToeGameWindow ticTacToeGameWindow, string gameKey, string login)
        {
            _ticTacToeGameWindow = ticTacToeGameWindow;
            _gameKey = gameKey;
            _login = login;
            _symbol = CheckSymbol();
            if (_symbol == "X") _opponentsSymbol = "O";
            else _opponentsSymbol = "X";
        }

        /// <summary>
        /// Runns multichain command and checks if opponent has made a move.
        /// </summary>
        /// <param name="ticTacToeGameWindow">TicTacToeGameWindow object to modify window's propherties.</param>
        /// <param name="cancellationToken">Token that can be used by other threads to cancel the loop.</param>
        public void UpdateGame(TicTacToeGameWindow ticTacToeGameWindow, CancellationToken cancellationToken)
        {
            while (true)
            {
                (string output, _) = MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, $"liststreamkeyitems {_streamName} {_gameKey}");
                List<string> moves = ExtensionsMethods.SearchInJson(output, "place");
                List<string> symbols = ExtensionsMethods.SearchInJson(output, "symbol");

                if (symbols.Count > 0 && symbols.Last() == _opponentsSymbol)  // if opponent moved
                {
                    int x = int.Parse(moves.Last().Remove(1, 2));  // 1,2 -> 1
                    int y = int.Parse(moves.Last().Remove(0, 2));  // 1,2 -> 2
                    Coordinates coordinates = new(x, y);
                    UpdateGrid(symbols.Last(), coordinates);
                    ticTacToeGameWindow.Dispatcher.Invoke(() => { ticTacToeGameWindow.GameAreaTextBlock.Text = DisplayGrid(); });
                    TicTacToeGameWindow._allMoves.Add(coordinates);
                    TicTacToeGameWindow._opponentsMoveCoordinates = coordinates;
                    TicTacToeGameWindow._didOpponentMove = true;
                }
                else if (symbols.Count > 0 && symbols.Last() == _symbol) // if the user moved
                {
                    int x = int.Parse(moves.Last().Remove(1, 2));  // 1,2 -> 1
                    int y = int.Parse(moves.Last().Remove(0, 2));  // 1,2 -> 2
                    Coordinates coordinates = new(x, y);
                    UpdateGrid(symbols.Last(), coordinates);
                    ticTacToeGameWindow.Dispatcher.Invoke(() => { ticTacToeGameWindow.GameAreaTextBlock.Text = DisplayGrid(); });
                    TicTacToeGameWindow._allMoves.Add(coordinates);
                    TicTacToeGameWindow._didOpponentMove = false;
                }
                else  // this will be the first move
                {
                    TicTacToeGameWindow._didOpponentMove = true;
                }
                // TODO: ending the game
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
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
            _grid[coordinates._x, coordinates._y] = symbol;
        }

        /// <summary>
        /// Displays TicTacToe grid in txt fotmat.
        /// </summary>
        /// <returns>TicTacToe grid in txt format.</returns>
        public string DisplayGrid()
        {
            string output = "";
            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                output += "|";
                for (int j = 0; j < _grid.GetLength(1); j++)
                {
                    if (_grid[i,j] == "X" || _grid[i, j] == "o")
                    {
                        output += $" {_grid[i, j]} |";
                    }
                    else
                    {
                        output += $"    |";
                    }
                }
                output += System.Environment.NewLine;
            }
            return output;
        }

        /// <summary>
        /// Checks whether someone won the game.
        /// </summary>
        /// <returns>True of game is over, otherwise false.</returns>
        private bool IsGameOver()
        {
            return false;
        }

        /// <summary>
        /// Publishes a move to the chain.
        /// </summary>
        /// <param name="coords">Coordinates on the grid where symbol was put.</param>
        public void PublishMove(Coordinates coords)
        {
            MultiChainClient.PublishToStream(_streamName, _gameKey,
                $"{{\"\"\"json\"\"\":{{" +
                $"\"\"\"login\"\"\":\"\"\"{GlobalVariables.UserAccount!.Login}\"\"\"," +
                $"\"\"\"symbol\"\"\":\"\"\"{_symbol}\"\"\"," +
                $"\"\"\"place\"\"\":\"\"\"{coords._x},{coords._y}\"\"\"}}}}");
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
            MultiChainClient.PublishToStream(_streamName, gameKey,
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
            (string output, _) = MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, $"liststreamkeyitems {_streamName} {_gameKey}");
            List<string> opponentsSymbol = ExtensionsMethods.SearchInJson(output, "drawn");
            return opponentsSymbol.Last();
        }
    }
}
