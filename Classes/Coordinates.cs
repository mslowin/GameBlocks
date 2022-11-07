using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes
{
    /// <summary>
    /// Class representing coordinates on two dimentional plane.
    /// </summary>
    public class Coordinates
    {
        /// <summary>
        /// x coordinate (rows).
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// y coordinate (columns).
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Constructor of Coordinates class.
        /// </summary>
        /// <param name="X">x coordinate (rows).</param>
        /// <param name="Y">y coordinate (columns).</param>
        public Coordinates(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
