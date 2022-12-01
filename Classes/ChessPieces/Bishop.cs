using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes.ChessPieces
{
    public class Bishop : ChessPiece
    {
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
        }

        public void Move(int newX, int newY)
        {
            CurrentCoordinates.X = newX;
            CurrentCoordinates.Y = newY;
        }
    }
}
