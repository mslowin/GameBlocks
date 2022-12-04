using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes.ChessPieces
{
    /// <summary>
    /// Class representing a pawn on Chess board.
    /// </summary>
    public class Pawn : ChessPiece
    {
        /// <summary>
        /// Constructor of a Pawn class.
        /// </summary>
        /// <param name="startCoordinates">Coordinates where the pawn is placed at the beginning.</param>
        /// <param name="color">Color of the pwan.</param>
        public Pawn(Coordinates startCoordinates, string color)
        {
            CurrentCoordinates = startCoordinates;
            Color = color;
            Name = $"{color.Remove(1)}p";
        }

        /// <summary>
        /// Changes Pawns coordinates.
        /// </summary>
        /// <param name="newX">New X coordinate of the Pawn.</param>
        /// <param name="newY">New Y coordinate of the Pawn.</param>
        public void Move(int newX, int newY)
        {
            CurrentCoordinates.X = newX;
            CurrentCoordinates.Y = newY;
        }

        /// <summary>
        /// Checks whether move is legal.
        /// </summary>
        /// <param name="newX">X coordinate to move the pawn to.</param>
        /// <param name="newY">Y coordinate to move the pawn to.</param>
        /// <returns>True if the move is legal, false if illegal</returns>
        public bool IsMovePossible(int newX, int newY)
        {
            if (Color == "white")
            {
                if (newX == CurrentCoordinates.X - 1 && newY == CurrentCoordinates.Y)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (newX == CurrentCoordinates.X + 1 && newY == CurrentCoordinates.Y)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
