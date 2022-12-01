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

        public void Move(int newX, int newY)
        {
            CurrentCoordinates.X = newX;
            CurrentCoordinates.Y = newY;
        }
    }
}
