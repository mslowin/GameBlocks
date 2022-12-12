using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes.ChessPieces
{
    /// <summary>
    /// Class representing a King on Chess board.
    /// </summary>
    public class King : ChessPiece
    {

        private string opponentsColor { get; set; }

        /// <summary>
        /// Constructor of a King class.
        /// </summary>
        /// <param name="startCoordinates">Coordinates where the King is placed at the beginning.</param>
        /// <param name="color">Color of the King.</param>
        public King(Coordinates startCoordinates, string color)
        {
            CurrentCoordinates = startCoordinates;
            Color = color;
            Name = $"{color.Remove(1)}K";
            if (Color == "white") { opponentsColor = "black"; }
            else { opponentsColor = "white"; }
        }

        /// <summary>
        /// Changes Kings coordinates.
        /// </summary>
        /// <param name="newX">New X coordinate of the King.</param>
        /// <param name="newY">New Y coordinate of the King.</param>
        public void Move(int newX, int newY)
        {
            CurrentCoordinates.X = newX;
            CurrentCoordinates.Y = newY;
        }

        /// <summary>
        /// Checks whether move is legal.
        /// </summary>
        /// <param name="newX">X coordinate to move the King to.</param>
        /// <param name="newY">Y coordinate to move the King to.</param>
        /// <returns>True if the move is legal, false if illegal</returns>
        public bool IsMovePossible(int newX, int newY, string[,] Grid, Chess chess)
        {
            
            if (newX > CurrentCoordinates.X + 1 || newX < CurrentCoordinates.X - 1)
            {
                return false;
            }
            if (newY > CurrentCoordinates.Y + 1 || newY < CurrentCoordinates.Y - 1)
            {
                return false;
            }

            List<Coordinates> impossibleCoordinats = new();

            // Bishops
            List<Bishop> opponentsBishops = chess.Bishops.Where(b => b.Name.StartsWith(opponentsColor.First())).ToList();
            opponentsBishops.ForEach(b => b.IsMovePossible(0, 0, Grid));
            foreach (var bishop in opponentsBishops)
            {
                foreach (var possibleMove in bishop.PossibleMoves)
                {
                    if (newX == possibleMove.X && newY == possibleMove.Y)
                    {
                        return false;
                    }
                }
            }
            
            // Queens
            List<Queen> opponentsQueens = chess.Queens.Where(q => q.Name.StartsWith(opponentsColor.First())).ToList();
            opponentsQueens.ForEach(q => q.IsMovePossible(0, 0, Grid));
            foreach (var queen in opponentsQueens)
            {
                foreach (var possibleMove in queen.PossibleMoves)
                {
                    if (newX == possibleMove.X && newY == possibleMove.Y)
                    {
                        return false;
                    }
                }
            }

            // Rooks
            List<Rook> opponentsRooks = chess.Rooks.Where(r => r.Name.StartsWith(opponentsColor.First())).ToList();
            opponentsRooks.ForEach(r => r.IsMovePossible(0, 0, Grid));
            foreach (var rook in opponentsRooks)
            {
                foreach (var possibleMove in rook.PossibleMoves)
                {
                    if (newX == possibleMove.X && newY == possibleMove.Y)
                    {
                        return false;
                    }
                }
            }

            // Knights
            List<Knight> opponentsKnights = chess.Knights.Where(r => r.Name.StartsWith(opponentsColor.First())).ToList();
            opponentsKnights.ForEach(r => r.IsMovePossible(0, 0));
            foreach (var knight in opponentsKnights)
            {
                foreach (var possibleMove in knight.PossibleMoves)
                {
                    if (newX == possibleMove.X && newY == possibleMove.Y)
                    {
                        return false;
                    }
                }
            }

            // Pawns
            List<Pawn> opponentsPawns = chess.Pawns.Where(r => r.Name.StartsWith(opponentsColor.First())).ToList();
            opponentsPawns.ForEach(r => r.IsMovePossible(0, 0, Grid));
            foreach (var pawn in opponentsPawns)
            {
                foreach (var possibleMove in pawn.PossibleMoves)
                {
                    if (newX == possibleMove.X && newY == possibleMove.Y)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
