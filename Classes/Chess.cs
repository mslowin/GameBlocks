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
        /// List of Kings.
        /// </summary>
        public List<King> Kings { get; set; }

        /// <summary>
        /// List of Queens.
        /// </summary>
        public List<Queen> Queens { get; set; }

        /// <summary>
        /// List of Queens.
        /// </summary>
        public List<Bishop> Bishops { get; set; }

        /// <summary>
        /// List of Queens.
        /// </summary>
        public List<Knight> Knights{ get; set; }

        /// <summary>
        /// List of Queens.
        /// </summary>
        public List<Rook> Rooks { get; set; }

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
            (Pawns, Kings, Queens, Bishops, Knights, Rooks) = InitiatePieces();
            Color = color;
            InitiateGrid();
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
            chessGameWindow.Dispatcher.Invoke(() => { chessGameWindow.GameAreaTextBlock.Text = DisplayGrid(); });
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
            (Pawns, Kings, Queens, Bishops, Knights, Rooks) = InitiatePieces();
            OpponentsColor = CheckColor();
            InitiateGrid();
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
            chessGameWindow.Dispatcher.Invoke(() => { chessGameWindow.GameAreaTextBlock.Text = DisplayGrid(); });
        }

        /// <summary>
        /// Initializes Pawns with their coordinates and colors.
        /// </summary>
        /// <returns>List of pawns.</returns>
        public (List<Pawn>, List<King>, List<Queen>, List<Bishop>, List<Knight>, List<Rook>) InitiatePieces()
        {
            List<Pawn> pawns = new();
            List<King> kings = new();
            List<Queen> queens = new();
            List<Bishop> bishops = new();
            List<Knight> knights = new();
            List<Rook> rooks = new();

            for (int i = 0; i < 8; i++)
            {
                pawns.Add(new(startCoordinates:new(1, i), color:"black"));
                pawns.Add(new(startCoordinates:new(6, i), color:"white"));
            }
            kings.Add(new(startCoordinates: new(0, 4), color: "black"));
            kings.Add(new(startCoordinates: new(7, 4), color: "white"));
            
            queens.Add(new(startCoordinates: new(0, 3), color: "black"));
            queens.Add(new(startCoordinates: new(7, 3), color: "white"));

            bishops.Add(new(startCoordinates: new(0, 2), color: "black"));
            bishops.Add(new(startCoordinates: new(0, 5), color: "black"));
            bishops.Add(new(startCoordinates: new(7, 2), color: "white"));
            bishops.Add(new(startCoordinates: new(7, 5), color: "white"));

            knights.Add(new(startCoordinates: new(0, 1), color: "black"));
            knights.Add(new(startCoordinates: new(0, 6), color: "black"));
            knights.Add(new(startCoordinates: new(7, 1), color: "white"));
            knights.Add(new(startCoordinates: new(7, 6), color: "white"));

            rooks.Add(new(startCoordinates: new(0, 0), color: "black"));
            rooks.Add(new(startCoordinates: new(0, 7), color: "black"));
            rooks.Add(new(startCoordinates: new(7, 0), color: "white"));
            rooks.Add(new(startCoordinates: new(7, 7), color: "white"));

            return (pawns, kings, queens, bishops, knights, rooks);
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
            foreach (var king in Kings)
            {
                Grid[king.CurrentCoordinates.X, king.CurrentCoordinates.Y] = king.Name;
            }
            foreach (var queen in Queens)
            {
                Grid[queen.CurrentCoordinates.X, queen.CurrentCoordinates.Y] = queen.Name;
            }
            foreach (var bishop in Bishops)
            {
                Grid[bishop.CurrentCoordinates.X, bishop.CurrentCoordinates.Y] = bishop.Name;
            }
            foreach (var knight in Knights)
            {
                Grid[knight.CurrentCoordinates.X, knight.CurrentCoordinates.Y] = knight.Name;
            }
            foreach (var rook in Rooks)
            {
                Grid[rook.CurrentCoordinates.X, rook.CurrentCoordinates.Y] = rook.Name;
            }
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
                    UpdatePieces(pieces.Last(), coordinates);
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
                    UpdatePieces(pieces.Last(), coordinates);
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
        /// Updates pieces coordinates based on the grid.
        /// </summary>
        /// <param name="pieceName">Name of the piece to be moved.</param>
        /// <param name="coordinates">Chess coordinates from where to take the piece and where to place it.</param>
        private void UpdatePieces(string pieceName, ChessCoordinates coordinates)
        {
            if (pieceName.EndsWith("p"))
            {
                MovePiece(Pawns, coordinates);
            }
            if (pieceName.EndsWith("K"))
            {
                MovePiece(Kings, coordinates);
            }
            if (pieceName.EndsWith("Q"))
            {
                MovePiece(Queens, coordinates);
            }
            if (pieceName.EndsWith("b"))
            {
                MovePiece(Bishops, coordinates);
            }
            if (pieceName.EndsWith("k"))
            {
                MovePiece(Knights, coordinates);
            }
            if (pieceName.EndsWith("r"))
            {
                MovePiece(Rooks, coordinates);
            }
        }

        /// <summary>
        /// Moves specified Piece based on Chess coordinates.
        /// </summary>
        /// <param name="pieces">Dynamic List of pieces to search for the desired piece.</param>
        /// <param name="coordinates">Chess coordinates from where to take the piece and where to place it.</param>
        public void MovePiece(dynamic pieces, ChessCoordinates coordinates)
        {
            foreach (var piec in pieces)
            {
                if (piec.CurrentCoordinates.X == coordinates.OldX && piec.CurrentCoordinates.Y == coordinates.OldY)
                {
                    piec.Move(coordinates.NewX, coordinates.NewY);
                }
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
            string output = "\u00A0\u00A0\u00A0|";
            if (Color == "white")
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    output += $"\u00A0{j}\u00A0|";
                }
                output += Environment.NewLine;
                output += "---+---+---+---+---+---+---+---+---+";
                output += Environment.NewLine;
                for (int i = 0; i < Grid.GetLength(0); i++)
                {
                    output += $"\u00A0{i}\u00A0|";
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
                    output += "---+---+---+---+---+---+---+---+---+";
                    output += Environment.NewLine;
                }
            }
            else
            {
                for (int j = Grid.GetLength(1) - 1; j >= 0; j--)
                {
                    output += $"\u00A0{j}\u00A0|";
                }
                output += Environment.NewLine;
                output += "---+---+---+---+---+---+---+---+---+";
                output += Environment.NewLine;
                for (int i = Grid.GetLength(0) - 1; i >= 0; i--)
                {
                    output += $"\u00A0{i}\u00A0|";
                    for (int j = Grid.GetLength(1) - 1; j >= 0; j--)
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
                    output += "---+---+---+---+---+---+---+---+---+";
                    output += Environment.NewLine;
                }
            }
            return output;
        }

        /// <summary>
        /// Publishes a move to the chain.
        /// </summary>
        /// <param name="coords">Coordinates on the grid where symbol was put.</param>
        public void PublishMove(ChessCoordinates coords)
        {
            // Checking wheteher there is a users piece in the "From" coordinates
            List<string> pieceNames = new();
            string pieceName = "";
            pieceNames.Add(CheckIfTheCoordinatesContainUsersPiece(Pawns, coords));
            pieceNames.Add(CheckIfTheCoordinatesContainUsersPiece(Kings, coords));
            pieceNames.Add(CheckIfTheCoordinatesContainUsersPiece(Queens, coords));
            pieceNames.Add(CheckIfTheCoordinatesContainUsersPiece(Bishops, coords));
            pieceNames.Add(CheckIfTheCoordinatesContainUsersPiece(Knights, coords));
            pieceNames.Add(CheckIfTheCoordinatesContainUsersPiece(Rooks, coords));

            if (pieceNames.Where(x => x != "").ToList().Count == 0)
            {
                ChessGameWindow!.Dispatcher.Invoke(() => { ChessGameWindow.InformationTextBlock.Text = "There were no users pieces here."; });
                return;
            }
            else
            {
                pieceName = pieceNames.First(x => x != "");
            }

            // Checking whether there aren't any users pieces and no opponents King in the destination field
            King opponentsKing = new(startCoordinates: new(0, 0), OpponentsColor);
            if (Grid[coords.NewX,coords.NewY].StartsWith(Color.First()) || Grid[coords.NewX, coords.NewY] == opponentsKing.Name)
            {
                ChessGameWindow!.Dispatcher.Invoke(() => { ChessGameWindow.InformationTextBlock.Text = $"Piece can't be moved here."; });
                return;

            }
            bool isMovePossible = CheckIfMovePossible(pieceName, coords);

            // Checking if the move is possible (Pieces Laws)
            if (!isMovePossible)
            {
                ChessGameWindow!.Dispatcher.Invoke(() => { ChessGameWindow.InformationTextBlock.Text = $"Impossible move."; });
                return;
            }
            
            ChessGameWindow!.Dispatcher.Invoke(() => { ChessGameWindow.InformationTextBlock.Text = ""; });
            MultiChainClient.PublishToStream(StreamName, GameKey,
                $"{{\"\"\"json\"\"\":{{" +
                $"\"\"\"login\"\"\":\"\"\"{GlobalVariables.UserAccount!.Login}\"\"\"," +
                $"\"\"\"color\"\"\":\"\"\"{Color}\"\"\"," +
                $"\"\"\"piece\"\"\":\"\"\"{pieceName}\"\"\"," +
                $"\"\"\"from\"\"\":\"\"\"{coords.OldX},{coords.OldY}\"\"\"," +
                $"\"\"\"to\"\"\":\"\"\"{coords.NewX},{coords.NewY}\"\"\"}}}}");
        }

        /// <summary>
        /// Checks whether move is possible depending on type of piece.
        /// </summary>
        /// <param name="pieceName">Name of the piece to check if move possible.</param>
        /// <param name="coords">Chess coordinates of the move to be checked.</param>
        /// <returns>true if the move is possible, false if not.</returns>
        private bool CheckIfMovePossible(string pieceName, ChessCoordinates coords)
        {
            if (pieceName.EndsWith("p"))
            {
                var pawn = Pawns.First(p => p.CurrentCoordinates.X == coords.OldX && p.CurrentCoordinates.Y == coords.OldY);
                return pawn.IsMovePossible(coords.NewX, coords.NewY);
            }
            if (pieceName.EndsWith("K"))
            {
                var king = Kings.First(K => K.CurrentCoordinates.X == coords.OldX && K.CurrentCoordinates.Y == coords.OldY);
                return king.IsMovePossible(coords.NewX, coords.NewY, Grid);
            }
            if (pieceName.EndsWith("Q"))
            {
                var queen = Queens.First(Q => Q.CurrentCoordinates.X == coords.OldX && Q.CurrentCoordinates.Y == coords.OldY);
                return queen.IsMovePossible(coords.NewX, coords.NewY, Grid);
            }
            if (pieceName.EndsWith("b"))
            {
                var bishop = Bishops.First(b => b.CurrentCoordinates.X == coords.OldX && b.CurrentCoordinates.Y == coords.OldY);
                return bishop.IsMovePossible(coords.NewX, coords.NewY, Grid);
            }
            if (pieceName.EndsWith("k"))
            {
                var knight = Knights.First(k => k.CurrentCoordinates.X == coords.OldX && k.CurrentCoordinates.Y == coords.OldY);
                return knight.IsMovePossible(coords.NewX, coords.NewY, Grid);
            }
            if (pieceName.EndsWith("r"))
            {
                var rook = Rooks.First(r => r.CurrentCoordinates.X == coords.OldX && r.CurrentCoordinates.Y == coords.OldY);
                return rook.IsMovePossible(coords.NewX, coords.NewY, Grid);
            }
            return false;
        }

        /// <summary>
        /// Checks whether the "From" coordinates of Chess coordinates are taken by users piece.
        /// </summary>
        /// <param name="pieces">Dynamic list of pieces to search for a specific piece.</param>
        /// <param name="coords">Chess coordinates to check.</param>
        /// <returns>Name of the found piece. If no pieces found returns "".</returns>
        public string CheckIfTheCoordinatesContainUsersPiece(dynamic pieces, ChessCoordinates coords)
        {
            foreach (var piece in pieces)
            {
                if (piece.CurrentCoordinates.X == coords.OldX && piece.CurrentCoordinates.Y == coords.OldY && piece.Name.StartsWith(Color.First()))
                {
                    return piece.Name;
                }
            }
            return "";
        }
        

        /// <summary>
        /// Randomly draws a symbol (ether "black" or "white") and publishes it to a specified stream.
        /// </summary>
        /// <param name="gameKey">Key consisting of players' logins to publish data with.</param>
        /// <returns>Drawn symbol in string type ("black" or "white").</returns>
        public static string DrawColor(string gameKey)
        {
            Random randomNumber = new();
            int randomIntiger = randomNumber.Next(0, 2);
            string drawnSymbol;
            if (randomIntiger == 0)
            {
                drawnSymbol = "white";
            }
            else
            {
                drawnSymbol = "black";
            }
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
