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
    public class TicTacToe
    {
        public static string _streamName { get; } = "GameTicTacToe";
        public static TicTacToeGameWindow? _ticTacToeGameWindow { get; set;  }
        public string _gameKey { get; set; }
        public string _login { get; set; }
        public string _symbol { get; set; }
        public string _opponentsSymbol { get; set; }
        public string[,] _grid { get; set; } = new string[3,3];

        // x,y
        // 0,0 | 0,1 | 0,2
        // 1,0 | 1,1 | 1,2
        // 2,0 | 2,1 | 2,2

        public TicTacToe(TicTacToeGameWindow ticTacToeGameWindow, string gameKey, string login, string symbol)
        {
            _ticTacToeGameWindow = ticTacToeGameWindow;
            _gameKey = gameKey;
            _login = login;
            _symbol = symbol;
            if (_symbol == "X") _opponentsSymbol = "o";
            else _opponentsSymbol = "X";
        }

        public TicTacToe(TicTacToeGameWindow ticTacToeGameWindow, string gameKey, string login)
        {
            _ticTacToeGameWindow = ticTacToeGameWindow;
            _gameKey = gameKey;
            _login = login;
            _symbol = CheckSymbol();
            if (_symbol == "X") _opponentsSymbol = "o";
            else _opponentsSymbol = "X";
        }

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

        private void UpdateGrid(string symbol, Coordinates coordinates)
        {
            _grid[coordinates._x, coordinates._y] = symbol;
        }

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

        private bool IsGameOver()
        {
            return false;
        }

        public void PublishMove(Coordinates coords)
        {
            MultiChainClient.PublishToStream(_streamName, _gameKey,
                $"{{\"\"\"json\"\"\":{{" +
                $"\"\"\"login\"\"\":\"\"\"{GlobalVariables.UserAccount!.Login}\"\"\"," +
                $"\"\"\"symbol\"\"\":\"\"\"{_symbol}\"\"\"," +
                $"\"\"\"place\"\"\":\"\"\"{coords._x},{coords._y}\"\"\"}}}}");
        }

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

        private string CheckSymbol()
        {
            (string output, _) = MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, $"liststreamkeyitems {_streamName} {_gameKey}");
            List<string> opponentsSymbol = ExtensionsMethods.SearchInJson(output, "drawn");
            return opponentsSymbol.Last();
        }
    }
}
