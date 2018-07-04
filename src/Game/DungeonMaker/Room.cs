using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DungeonMaker
{
    public class Room
    {
        public int width;
        public int height;
        public int x;
        public int y;

        /// <summary>
        /// Creates a <c>Room</c> object with a specific width/height at a specific x/y location.
        /// </summary>
        /// <param name="width">The width of the room</param>
        /// <param name="height">The height of the room</param>
        /// <param name="x">The x coordinate of the room's top left corner</param>
        /// <param name="y">The y coordinate of the room's top left corner</param>
        public Room(int width, int height, int x, int y)
        {
            this.width = width;
            this.height = height;
            this.x = x;
            this.y = y;
        }
    }
}
