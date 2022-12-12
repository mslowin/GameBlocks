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
        public List<Coordinates> PossibleMoves { get; set; }

        private string opponentsColor { get; set; }

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
            if (Color == "white") { opponentsColor = "black"; }
            else { opponentsColor = "white"; }
        }

        /// <summary>
        /// Changes Queens coordinates.
        /// </summary>
        /// <param name="newX">New X coordinate of the Queen.</param>
        /// <param name="newY">New Y coordinate of the Queen.</param>
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
        /// <param name="Grid">Grid with all pieces on it.</param>
        /// <returns>True if the move is legal, false if illegal</returns>
        public bool IsMovePossible(int newX, int newY, string[,] Grid)
        {
            List<Coordinates> possibleCoordinates = new();
            string opponentsKingName = opponentsColor.First() + "K";

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
                var xCoord = CurrentCoordinates.X + multiplier;
                var yCoord = CurrentCoordinates.Y;

                if (Grid[xCoord, yCoord] != " " && Grid[xCoord, yCoord] != opponentsKingName)
                {
                    possibleCoordinates.Add(new(xCoord, yCoord));
                    break;
                }
                possibleCoordinates.Add(new(xCoord, yCoord));
                multiplier++;
            }
            multiplier = 1;
            for (int i = 0; i < rightIterations; i++)
            {
                var xCoord = CurrentCoordinates.X;
                var yCoord = CurrentCoordinates.Y + multiplier;

                if (Grid[xCoord, yCoord] != " " && Grid[xCoord, yCoord] != opponentsKingName)
                {
                    possibleCoordinates.Add(new(xCoord, yCoord));
                    break;
                }
                possibleCoordinates.Add(new(xCoord, yCoord));
                multiplier++;
            }
            multiplier = 1;
            for (int i = 0; i < leftIterations; i++)
            {
                var xCoord = CurrentCoordinates.X;
                var yCoord = CurrentCoordinates.Y - multiplier;

                if (Grid[xCoord, yCoord] != " " && Grid[xCoord, yCoord] != opponentsKingName)
                {
                    possibleCoordinates.Add(new(xCoord, yCoord));
                    break;
                }
                possibleCoordinates.Add(new(xCoord, yCoord));
                multiplier++;
            }
            multiplier = 1;
            for (int i = 0; i < upperIterations; i++)
            {
                var xCoord = CurrentCoordinates.X - multiplier;
                var yCoord = CurrentCoordinates.Y;

                if (Grid[xCoord, yCoord] != " " && Grid[xCoord, yCoord] != opponentsKingName)
                {
                    possibleCoordinates.Add(new(xCoord, yCoord));
                    break;
                }
                possibleCoordinates.Add(new(xCoord, yCoord));
                multiplier++;
            }
            multiplier = 1;
            for (int i = 0; i < bottomRightIterations; i++)
            {
                var xCoord = CurrentCoordinates.X + multiplier;
                var yCoord = CurrentCoordinates.Y + multiplier;

                if (Grid[xCoord, yCoord] != " " && Grid[xCoord, yCoord] != opponentsKingName)
                {
                    possibleCoordinates.Add(new(xCoord, yCoord));
                    break;
                }
                possibleCoordinates.Add(new(xCoord, yCoord));
                multiplier++;
            }
            multiplier = 1;
            for (int i = 0; i < bottomLeftIterations; i++)
            {
                var xCoord = CurrentCoordinates.X + multiplier;
                var yCoord = CurrentCoordinates.Y - multiplier;

                if (Grid[xCoord, yCoord] != " " && Grid[xCoord, yCoord] != opponentsKingName)
                {
                    possibleCoordinates.Add(new(xCoord, yCoord));
                    break;
                }
                possibleCoordinates.Add(new(xCoord, yCoord));
                multiplier++;
            }
            multiplier = 1;
            for (int i = 0; i < upperRightIterations; i++)
            {
                var xCoord = CurrentCoordinates.X - multiplier;
                var yCoord = CurrentCoordinates.Y + multiplier;

                if (Grid[xCoord, yCoord] != " " && Grid[xCoord, yCoord] != opponentsKingName)
                {
                    possibleCoordinates.Add(new(xCoord, yCoord));
                    break;
                }
                possibleCoordinates.Add(new(xCoord, yCoord));
                multiplier++;
            }
            multiplier = 1;
            for (int i = 0; i < upperLeftIterations; i++)
            {
                var xCoord = CurrentCoordinates.X - multiplier;
                var yCoord = CurrentCoordinates.Y - multiplier;

                if (Grid[xCoord, yCoord] != " " && Grid[xCoord, yCoord] != opponentsKingName)
                {
                    possibleCoordinates.Add(new(xCoord, yCoord));
                    break;
                }
                possibleCoordinates.Add(new(xCoord, yCoord));
                multiplier++;
            }

            var matchingCoordinates = possibleCoordinates.Where(p => p.X == newX && p.Y == newY).ToList();
            PossibleMoves = possibleCoordinates;
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
