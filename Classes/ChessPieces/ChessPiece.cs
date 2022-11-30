using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes.ChessPieces
{
    public class ChessPiece
    {
        /// <summary>
        /// Coordinates where the ChessPiece is located.
        /// </summary>
        public Coordinates CurrentCoordinates { get; set; } = new(0, 0);

        /// <summary>
        /// Color of the ChessPiece.
        /// </summary>
        public string Color { get; set; } = "";

        /// <summary>
        /// Name of the ChessPiece.
        /// </summary>
        public string Name { get; set; } = "";
    }
}
