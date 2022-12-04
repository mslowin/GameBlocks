using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes
{
    public class ChessCoordinates
    {
        /// <summary>
        /// x coordinate from which the piece was taken.
        /// </summary>
        public int OldX { get; set; }

        /// <summary>
        /// y coordinate from which the piece was taken.
        /// </summary>
        public int OldY { get; set; }

        /// <summary>
        /// x coordinate where the piece was placed.
        /// </summary>
        public int NewX { get; set; }

        /// <summary>
        /// y coordinate where the piece was placed.
        /// </summary>
        public int NewY { get; set; }

        /// <summary>
        /// Constructor of Coordinates class.
        /// </summary>
        /// <param name="oldX">x coordinate from which the piece was taken.</param>
        /// <param name="oldY">y coordinate from which the piece was taken.</param>
        /// <param name="newX">x coordinate where the piece was placed..</param>
        /// <param name="newY">y coordinate where the piece was placed.</param>
        public ChessCoordinates(int oldX, int oldY, int newX, int newY)
        {
            this.OldX = oldX;
            this.OldY = oldY;
            this.NewX = newX;
            this.NewY = newY;
        }
    }
}
