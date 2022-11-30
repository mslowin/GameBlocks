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
    public class Pawn
    {
        /// <summary>
        /// Coordinates where the pawn is located.
        /// </summary>
        public Coordinates CurrentCoordinates { get; set; }

        /// <summary>
        /// Color of the pawn.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Name of the pawn.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Constructor of a Pawn class.
        /// </summary>
        /// <param name="startCoordinates"></param>
        /// <param name="color"></param>
        public Pawn(Coordinates startCoordinates, string color)
        {
            CurrentCoordinates = startCoordinates;
            Color = color;
            Name = $"{color.Remove(1)}p";
        }
    }
}
