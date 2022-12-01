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

        public bool Move(int newX, int newY)
        {
            CurrentCoordinates.X = newX;
            CurrentCoordinates.Y = newY;

            return true;
        }

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
