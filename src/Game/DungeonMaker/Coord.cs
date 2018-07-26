//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

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
