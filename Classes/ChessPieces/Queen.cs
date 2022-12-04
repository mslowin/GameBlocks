using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes.ChessPieces
{
    /// <summary>
    /// Class representing a Queen on Chess board.
    /// </summary>
    public class Queen : ChessPiece
    {
        /// <summary>
        /// Constructor of a Queen class.
        /// </summary>
        /// <param name="startCoordinates">Coordinates where the Queen is placed at the beginning.</param>
        /// <param name="color">Color of the Queen.</param>
        public Queen(Coordinates startCoordinates, string color)
        {
            CurrentCoordinates = startCoordinates;
            Color = color;
            Name = $"{color.Remove(1)}Q";
        }

        /// <summary>
        /// Changes Queens coordinates.
        /// </summary>
        /// <param name="newX">New X coordinate of the Queens.</param>
        /// <param name="newY">New Y coordinate of the Queens.</param>
        public void Move(int newX, int newY)
        {
            CurrentCoordinates.X = newX;
            CurrentCoordinates.Y = newY;
        }

        /// <summary>
        /// Checks whether move is legal.
        /// </summary>
        /// <param name="newX">X coordinate to move the Queen to.</param>
        /// <param name="newY">Y coordinate to move the Queen to.</param>
        /// <returns>True if the move is legal, false if illegal</returns>
        public bool IsMovePossible(int newX, int newY, string[,] Grid)
        {
            List<Coordinates> possibleCoordinates = new();

            int bottomIterations = Grid.GetLength(0) - 1 - CurrentCoordinates.X;
            int upperIterations = Grid.GetLength(0) - 1 - bottomIterations;
            int rightIterations = Grid.GetLength(1) - 1 - CurrentCoordinates.Y;
            int leftIterations = Grid.GetLength(1) - 1 - rightIterations;
            int upperRightIterations = Math.Min(upperIterations, rightIterations);
            int upperLeftIterations = Math.Min(upperIterations, leftIterations);
            int bottomRightIterations = Math.Min(bottomIterations, rightIterations);
            int bottomLeftIterations = Math.Min(bottomIterations, leftIterations);

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
            multiplier = 1;
            for (int i = 0; i < bottomRightIterations; i++)
            {
                possibleCoordinates.Add(new(CurrentCoordinates.X + multiplier, CurrentCoordinates.Y + multiplier));
                multiplier++;
            }
            multiplier = 1;
            for (int i = 0; i < bottomLeftIterations; i++)
            {
                possibleCoordinates.Add(new(CurrentCoordinates.X + multiplier, CurrentCoordinates.Y - multiplier));
                multiplier++;
            }
            multiplier = 1;
            for (int i = 0; i < upperRightIterations; i++)
            {
                possibleCoordinates.Add(new(CurrentCoordinates.X - multiplier, CurrentCoordinates.Y + multiplier));
                multiplier++;
            }
            multiplier = 1;
            for (int i = 0; i < upperLeftIterations; i++)
            {
                possibleCoordinates.Add(new(CurrentCoordinates.X - multiplier, CurrentCoordinates.Y - multiplier));
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
