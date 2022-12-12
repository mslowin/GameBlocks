using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes.ChessPieces
{
    public class Bishop : ChessPiece
    {
        public List<Coordinates> PossibleMoves { get; set; }

        private string opponentsColor { get; set; }

        /// <summary>
        /// Constructor of a Bishop class.
        /// </summary>
        /// <param name="startCoordinates">Coordinates where the Bishop is placed at the beginning.</param>
        /// <param name="color">Color of the pwan.</param>
        public Bishop(Coordinates startCoordinates, string color)
        {
            CurrentCoordinates = startCoordinates;
            Color = color;
            Name = $"{color.Remove(1)}b";
            if (Color == "white") { opponentsColor = "black"; }
            else { opponentsColor = "white"; }
        }

        /// <summary>
        /// Changes Bishops coordinates.
        /// </summary>
        /// <param name="newX">New X coordinate of the Bishop.</param>
        /// <param name="newY">New Y coordinate of the Bishop.</param>
        public void Move(int newX, int newY)
        {
            CurrentCoordinates.X = newX;
            CurrentCoordinates.Y = newY;
        }

        /// <summary>
        /// Checks whether move is legal.
        /// </summary>
        /// <param name="newX">X coordinate to move the Bishop to.</param>
        /// <param name="newY">Y coordinate to move the Bishop to.</param>
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
