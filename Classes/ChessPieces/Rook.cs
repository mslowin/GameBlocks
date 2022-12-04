using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes.ChessPieces
{
    public class Rook : ChessPiece
    {
        /// <summary>
        /// Constructor of a Knight class.
        /// </summary>
        /// <param name="startCoordinates">Coordinates where the pawn is placed at the beginning.</param>
        /// <param name="color">Color of the pwan.</param>
        public Rook(Coordinates startCoordinates, string color)
        {
            CurrentCoordinates = startCoordinates;
            Color = color;
            Name = $"{color.Remove(1)}r";
        }

        /// <summary>
        /// Changes Rooks coordinates.
        /// </summary>
        /// <param name="newX">New X coordinate of the Rook.</param>
        /// <param name="newY">New Y coordinate of the Rook.</param>
        public void Move(int newX, int newY)
        {
            CurrentCoordinates.X = newX;
            CurrentCoordinates.Y = newY;
        }

        /// <summary>
        /// Checks whether move is legal.
        /// </summary>
        /// <param name="newX">X coordinate to move the Rook to.</param>
        /// <param name="newY">Y coordinate to move the Rook to.</param>
        /// <returns>True if the move is legal, false if illegal</returns>
        public bool IsMovePossible(int newX, int newY, string[,] Grid)
        {
            List<Coordinates> possibleCoordinates = new();

            int bottomIterations = Grid.GetLength(0) - 1 - CurrentCoordinates.X;
            int upperIterations = Grid.GetLength(0) - 1 - bottomIterations;
            int rightIterations = Grid.GetLength(1) - 1 - CurrentCoordinates.Y;
            int leftIterations = Grid.GetLength(1) - 1 - rightIterations;

            int multiplier = 1;
            for (int i = 0; i < bottomIterations; i++)
            {
                possibleCoordinates.Add(new(CurrentCoordinates.X + multiplier, CurrentCoordinates.Y));
                multiplier++;
            }
            multiplier = 1;
            for (int i = 0; i < rightIterations; i++)
            {
                possibleCoordinates.Add(new(CurrentCoordinates.X, CurrentCoordinates.Y + multiplier));
                multiplier++;
            }
            multiplier = 1;
            for (int i = 0; i < leftIterations; i++)
            {
                possibleCoordinates.Add(new(CurrentCoordinates.X, CurrentCoordinates.Y - multiplier));
                multiplier++;
            }
            multiplier = 1;
            for (int i = 0; i < upperIterations; i++)
            {
                possibleCoordinates.Add(new(CurrentCoordinates.X - multiplier, CurrentCoordinates.Y));
                multiplier++;
            }

            var matchingCoordinates = possibleCoordinates.Where(p => p.X == newX && p.Y == newY).ToList();
            if (matchingCoordinates.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
