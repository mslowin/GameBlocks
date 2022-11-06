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
        /// x coordinate (columns).
        /// </summary>
        public int _x { get; set; }

        /// <summary>
        /// y coordinate (rows).
        /// </summary>
        public int _y { get; set; }

        /// <summary>
        /// Constructor of Coordinates class.
        /// </summary>
        /// <param name="x">x coordinate (columns).</param>
        /// <param name="y">y coordinate (rows).</param>
        public Coordinates(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}
