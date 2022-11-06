using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes
{
    public class Coordinates
    {
        public int _x { get; set; }
        public int _y { get; set; }

        public Coordinates(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}
