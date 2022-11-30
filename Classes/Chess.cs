using GameBlocks.Classes.ChessPieces;
using GameBlocks.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameBlocks.Classes
{
    /// <summary>
    /// Class representing a Chess game.
    /// </summary>
    public class Chess
    {
        /// <summary>
        /// Name of a stream where game data is published.
        /// </summary>
        public static string StreamName { get; } = "GameChess";

        /// <summary>
        /// ChessGameWindow object to modify window's 
        /// propherties (e.g. to change TextBlocks on the window).
        /// </summary>
        public static ChessGameWindow? ChessGameWindow { get; set; }

        /// <summary>
        /// Key consisting of players' logins to publish data with.
        /// </summary>
        public string GameKey { get; set; }

        /// <summary>
        /// User's login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User's color (ether "black" or "white").
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Opponent's color (ether "black" or "white").
        /// </summary>
        public string OpponentsColor { get; set; }

        /// <summary>
        /// List of pawns.
        /// </summary>
        public List<Pawn> Pawns { get; set; }

        /// <summary>
        /// A two dimentional array forming game grid.
        /// x,y
        /// 0,0 | 0,1 | 0,2 | 0,3 | 0,4 | 0,5 | 0,6 | 0,7
        /// 1,0 | 1,1 | 1,2 | 1,3 | 1,4 | 1,5 | 1,6 | 1,7
        /// 2,0 | 2,1 | 2,2 | 2,3 | 2,4 | 2,5 | 2,6 | 2,7
        /// 3,0 | 3,1 | 3,2 | 3,3 | 3,4 | 3,5 | 3,6 | 3,7
        /// 4,0 | 4,1 | 4,2 | 4,3 | 4,4 | 4,5 | 4,6 | 4,7
        /// 5,0 | 5,1 | 5,2 | 5,3 | 5,4 | 5,5 | 5,6 | 5,7
        /// 6,0 | 6,1 | 6,2 | 6,3 | 6,4 | 6,5 | 6,6 | 6,7
        /// 7,0 | 7,1 | 7,2 | 7,3 | 7,4 | 7,5 | 7,6 | 7,7
        /// </summary>
        public string[,] Grid { get; set; } = new string[8, 8] { { " ", " ", " ", " ", " ", " ", " ", " " }, 
                                                                 { " ", " ", " ", " ", " ", " ", " ", " " }, 
                                                                 { " ", " ", " ", " ", " ", " ", " ", " " }, 
                                                                 { " ", " ", " ", " ", " ", " ", " ", " " }, 
                                                                 { " ", " ", " ", " ", " ", " ", " ", " " }, 
                                                                 { " ", " ", " ", " ", " ", " ", " ", " " }, 
                                                                 { " ", " ", " ", " ", " ", " ", " ", " " }, 
                                                                 { " ", " ", " ", " ", " ", " ", " ", " " }, };

        /// <summary>
        /// Constructor of a Chess class when symbol has already been drawn.
        /// </summary>
        /// <param name="chessGameWindow">ChessGameWindow object to modify window's propherties.</param>
        /// <param name="gameKey">Key consisting of players' logins to publish data with.</param>
        /// <param name="login">User's login.</param>
        /// <param name="color">User's color (ether "black" or "white").</param>
        public Chess(ChessGameWindow chessGameWindow, string gameKey, string login, string color)
        {
            ChessGameWindow = chessGameWindow;
            GameKey = gameKey;
            Login = login; 
            Pawns = InitiatePawns();
            Color = color;
            InitiateGrid();
            chessGameWindow.Dispatcher.Invoke(() => { chessGameWindow.GameAreaTextBlock.Text = DisplayGrid(); });
            if (Color == "black")
            {
                OpponentsColor = "white";
                ChessGameWindow.Dispatcher.Invoke(() => { ChessGameWindow.FromTextBox.IsEnabled = false; });
                ChessGameWindow.Dispatcher.Invoke(() => { ChessGameWindow.ToTextBox.IsEnabled = false; });
                ChessGameWindow.Dispatcher.Invoke(() => { ChessGameWindow.SubmitButton.IsEnabled = false; });
            }
            else
            {
                OpponentsColor = "black";
            }
        }

        /// <summary>
        /// Constructor of a Chess class but color ("black" or "white") is determined by opponents color.
        /// </summary>
        /// <param name="chessGameWindow">ChessGameWindow object to modify window's propherties.</param>
        /// <param name="gameKey">Key consisting of players' logins to publish data with.</param>
        /// <param name="login">User's login.</param>
        public Chess(ChessGameWindow chessGameWindow, string gameKey, string login)
        {
            ChessGameWindow = chessGameWindow;
            GameKey = gameKey;
            Login = login;
            Pawns = InitiatePawns();
            OpponentsColor = CheckColor();
            InitiateGrid();
            chessGameWindow.Dispatcher.Invoke(() => { chessGameWindow.GameAreaTextBlock.Text = DisplayGrid(); });
            if (OpponentsColor == "black")
            {
                Color = "white";
            }
            else
            {
                Color = "black";
                chessGameWindow.Dispatcher.Invoke(() => { chessGameWindow.FromTextBox.IsEnabled = false; });
                chessGameWindow.Dispatcher.Invoke(() => { chessGameWindow.ToTextBox.IsEnabled = false; });
                chessGameWindow.Dispatcher.Invoke(() => { chessGameWindow.SubmitButton.IsEnabled = false; });
            }
        }

        /// <summary>
        /// Initializes Pawns with their coordinates and colors.
        /// </summary>
        /// <returns>List of pawns.</returns>
        public List<Pawn> InitiatePawns()
        {
            List<Pawn> pawns = new();
            for (int i = 0; i < 8; i++)
            {
                pawns.Add(new(startCoordinates:new(1, i), color:"black"));
                pawns.Add(new(startCoordinates:new(6, i), color:"white"));
            }
            return pawns;
        }

        /// <summary>
        /// Runns multichain command and checks if opponent has made a move.
        /// </summary>
        /// <param name="chessGameWindow">ChessGameWindow object to modify window's propherties.</param>
        /// <param name="cancellationToken">Token that can be used by other threads to cancel the loop.</param>
        public bool UpdateGame(ChessGameWindow chessGameWindow, CancellationToken cancellationToken)
        {
            while (true)
            {
                (string output, _) = MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, $"liststreamkeyitems {StreamName} {GameKey}");
                List<string> pieces = ExtensionsMethods.SearchInJson(output, "piece");
                List<string> movesFrom = ExtensionsMethods.SearchInJson(output, "from");
                List<string> movesTo = ExtensionsMethods.SearchInJson(output, "to");
                List<string> colors = ExtensionsMethods.SearchInJson(output, "color");

                if (colors.Count > 0 && colors.Last() == OpponentsColor)  // if opponent moved
                {
                    int oldX = int.Parse(movesFrom.Last().Remove(1, 2));  // 1,2 -> 1
                    int oldY = int.Parse(movesFrom.Last().Remove(0, 2));  // 1,2 -> 2
                    int newX = int.Parse(movesTo.Last().Remove(1, 2));  // 1,2 -> 1
                    int newY = int.Parse(movesTo.Last().Remove(0, 2));  // 1,2 -> 2

                    ChessCoordinates coordinates = new(oldX, oldY, newX, newY);
                    UpdateGrid(pieces.Last(), coordinates);
                    chessGameWindow.Dispatcher.Invoke(() => { chessGameWindow.GameAreaTextBlock.Text = DisplayGrid(); });
                    //if (IsGameOver())
                    //{
                    //    MultiChainClient.PublishToStream(StreamName, GameKey,
                    //        $"{{\"\"\"json\"\"\":{{" +
                    //        $"\"\"\"login\"\"\":\"\"\"{GlobalVariables.UserAccount!.Login}\"\"\"," +
                    //        $"\"\"\"symbol\"\"\":\"\"\"{Symbol}\"\"\"," +
                    //        $"\"\"\"satus\"\"\":\"\"\"WYGRANA\"\"\"}}}}");
                    //    return false;  // returning false as the opponen won
                    //}

                    ChessGameWindow.AllMoves.Add(coordinates);
                    ChessGameWindow.OpponentsMoveCoordinates = coordinates;
                    ChessGameWindow.DidOpponentMove = true;
                    chessGameWindow.Dispatcher.Invoke(() => { chessGameWindow.FromTextBox.IsEnabled = true; });
                    chessGameWindow.Dispatcher.Invoke(() => { chessGameWindow.ToTextBox.IsEnabled = true; });
                    chessGameWindow.Dispatcher.Invoke(() => { chessGameWindow.SubmitButton.IsEnabled = true; });

                }
                else if (colors.Count > 0 && colors.Last() == Color) // if the user moved
                {
                    int oldX = int.Parse(movesFrom.Last().Remove(1, 2));  // 1,2 -> 1
                    int oldY = int.Parse(movesFrom.Last().Remove(0, 2));  // 1,2 -> 2
                    int newX = int.Parse(movesTo.Last().Remove(1, 2));  // 1,2 -> 1
                    int newY = int.Parse(movesTo.Last().Remove(0, 2));  // 1,2 -> 2

                    ChessCoordinates coordinates = new(oldX, oldY, newX, newY);
                    UpdateGrid(pieces.Last(), coordinates);
                    chessGameWindow.Dispatcher.Invoke(() => { chessGameWindow.GameAreaTextBlock.Text = DisplayGrid(); });
                    //if (IsGameOver())
                    //{
                    //    MultiChainClient.PublishToStream(StreamName, GameKey,
                    //        $"{{\"\"\"json\"\"\":{{" +
                    //        $"\"\"\"login\"\"\":\"\"\"{GlobalVariables.UserAccount!.Login}\"\"\"," +
                    //        $"\"\"\"symbol\"\"\":\"\"\"{Symbol}\"\"\"," +
                    //        $"\"\"\"satus\"\"\":\"\"\"WYGRANA\"\"\"}}}}");
                    //    return true;  // returning true as the player won
                    //}

                    ChessGameWindow.AllMoves.Add(coordinates);
                    ChessGameWindow.DidOpponentMove = false;
                    chessGameWindow.Dispatcher.Invoke(() => { chessGameWindow.FromTextBox.IsEnabled = false; });
                    chessGameWindow.Dispatcher.Invoke(() => { chessGameWindow.ToTextBox.IsEnabled = false; });
                    chessGameWindow.Dispatcher.Invoke(() => { chessGameWindow.SubmitButton.IsEnabled = false; });
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
        /// Initieates the grid with pawns at the start.
        /// </summary>
        public void InitiateGrid()
        {
            foreach (var pawn in Pawns)
            {
                Grid[pawn.CurrentCoordinates.X, pawn.CurrentCoordinates.Y] = pawn.Name;
            }
        }

        /// <summary>
        /// Updates game grid (specified piece travels by ChessCordinates).
        /// </summary>
        /// <param name="pieceName">Name of the piece (e.g. black pawn = bp).</param>
        /// <param name="coordinates">ChessCoordinates defying piece old positon and destination.</param>
        private void UpdateGrid(string pieceName, ChessCoordinates coordinates)
        {
            Grid[coordinates.OldX, coordinates.OldY] = " ";
            Grid[coordinates.NewX, coordinates.NewY] = pieceName;
        }

        /// <summary>
        /// Displays Chess grid in txt fotmat.
        /// </summary>
        /// <returns>Chess grid in txt format.</returns>
        public string DisplayGrid()
        {
            string output = "";
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                output += "|";
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    if (Grid[i, j] == " ")
                    {
                        output += $"\u00A0\u00A0\u00A0|";
                    }
                    else
                    {
                        output += $"{Grid[i, j]}\u00A0|";
                    }
                }
                output += Environment.NewLine;
                output += "+---+---+---+---+---+---+---+---+";
                output += Environment.NewLine;
            }
            return output;
        }

        /// <summary>
        /// Publishes a move to the chain.
        /// </summary>
        /// <param name="coords">Coordinates on the grid where symbol was put.</param>
        public void PublishMove(ChessCoordinates coords)
        {
            string pieceName = "";
            foreach(var pawn in Pawns)
            {
                if (pawn.CurrentCoordinates.X == coords.OldX && pawn.CurrentCoordinates.Y == coords.OldY)
                {
                    pieceName = pawn.Name;
                }
            }
            if (pieceName == "")
            {
                Trace.WriteLine("There was no pieces here.");
                return;
            }
            MultiChainClient.PublishToStream(StreamName, GameKey,
                $"{{\"\"\"json\"\"\":{{" +
                $"\"\"\"login\"\"\":\"\"\"{GlobalVariables.UserAccount!.Login}\"\"\"," +
                $"\"\"\"color\"\"\":\"\"\"{Color}\"\"\"," +
                $"\"\"\"piece\"\"\":\"\"\"{pieceName}\"\"\"," +
                $"\"\"\"from\"\"\":\"\"\"{coords.OldX},{coords.OldY}\"\"\"," +
                $"\"\"\"to\"\"\":\"\"\"{coords.NewX},{coords.NewY}\"\"\"}}}}");
        }
        

        /// <summary>
        /// Randomly draws a symbol (ether "black" or "white") and publishes it to a specified stream.
        /// </summary>
        /// <param name="gameKey">Key consisting of players' logins to publish data with.</param>
        /// <returns>Drawn symbol in string type ("black" or "white").</returns>
        public static string DrawColor(string gameKey)
        {
            // TODO: later add logic to randomly select black or white
            string drawnSymbol = "white";
            MultiChainClient.PublishToStream(StreamName, gameKey,
                $"{{\"\"\"json\"\"\":{{" +
                $"\"\"\"login\"\"\":\"\"\"{GlobalVariables.UserAccount!.Login}\"\"\"," +
                $"\"\"\"drawn\"\"\":\"\"\"{drawnSymbol}\"\"\"}}}}");
            return drawnSymbol;
        }

        /// <summary>
        /// Checks (within the stream) which color has been drawn by the opponent.
        /// </summary>
        /// <returns>Opponents color in string format ("black" or "white").</returns>
        private string CheckColor()
        {
            Task.Delay(5000);  // Gives time for the first person to publish it's symbol
            (string output, _) = MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, $"liststreamkeyitems {StreamName} {GameKey}");
            List<string> opponentsColor = ExtensionsMethods.SearchInJson(output, "drawn");
            return opponentsColor.Last();
        }
    }
}
