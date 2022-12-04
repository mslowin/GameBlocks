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
        public bool IsMovePossible(int newX, int newY, string[,] Grid)
        {
            if (newX > CurrentCoordinates.X + 1 || newX < CurrentCoordinates.X - 1)
            {
                return false;
            }
            if (newY > CurrentCoordinates.Y + 1 || newY < CurrentCoordinates.Y - 1)
            {
                return false;
            }

            return true;
        }
    }
}
