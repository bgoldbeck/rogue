using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DungeonMaker
{
    public class Coord
    {
        public int x;
        public int y;

        /// <summary>
        /// Creates a x/y coordinate pair. Mainly for use in Lists.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
