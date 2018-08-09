#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

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
