using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes.ChessPieces
{
    public class Knight : ChessPiece
    {
        /// <summary>
        /// Constructor of a Knight class.
        /// </summary>
        /// <param name="startCoordinates">Coordinates where the pawn is placed at the beginning.</param>
        /// <param name="color">Color of the pwan.</param>
        public Knight(Coordinates startCoordinates, string color)
        {
            CurrentCoordinates = startCoordinates;
            Color = color;
            Name = $"{color.Remove(1)}k";
        }

        /// <summary>
        /// Changes Knights coordinates.
        /// </summary>
        /// <param name="newX">New X coordinate of the Knight.</param>
        /// <param name="newY">New Y coordinate of the Knight.</param>
        public void Move(int newX, int newY)
        {
            CurrentCoordinates.X = newX;
            CurrentCoordinates.Y = newY;
        }

        /// <summary>
        /// Checks whether move is legal.
        /// </summary>
        /// <param name="newX">X coordinate to move the Knight to.</param>
        /// <param name="newY">Y coordinate to move the Knight to.</param>
        /// <returns>True if the move is legal, false if illegal</returns>
        public bool IsMovePossible(int newX, int newY, string[,] Grid)
        {
            if (newX == CurrentCoordinates.X - 2 && (newY == CurrentCoordinates.Y + 1 || newY == CurrentCoordinates.Y - 1))
            {
                return true;
            }
            if (newX == CurrentCoordinates.X + 2 && (newY == CurrentCoordinates.Y + 1 || newY == CurrentCoordinates.Y - 1))
            {
                return true;
            }

            if ((newX == CurrentCoordinates.X +1 || newX == CurrentCoordinates.X - 1) && newY == CurrentCoordinates.Y - 2)
            {
                return true;
            }
            if ((newX == CurrentCoordinates.X + 1 || newX == CurrentCoordinates.X - 1) && newY == CurrentCoordinates.Y + 2)
            {
                return true;
            }

            return false;

        }
    }
}
