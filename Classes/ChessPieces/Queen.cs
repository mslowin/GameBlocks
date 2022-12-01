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

        public void Move(int newX, int newY)
        {
            CurrentCoordinates.X = newX;
            CurrentCoordinates.Y = newY;
        }
    }
}
