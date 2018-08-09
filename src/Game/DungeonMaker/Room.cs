#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

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

        /// <summary>
        /// Checks whether a point is within a room or not.
        /// </summary>
        /// <param name="checkX">X coordinate to check.</param>
        /// <param name="checkY">Y coordinate to check.</param>
        /// <returns>Returns true if a point lies within the room.</returns>
        public bool Contains(int checkX, int checkY)
        {
            if (checkX >= x && checkX < x + width && checkY >= y && checkY < y + height)
                return true;
            return false;
        }

        /// <summary>
        /// Checks whether a cell is within one square of a room. Used for adding locked door.
        /// </summary>
        /// <param name="checkX">X coordinate to check.</param>
        /// <param name="checkY">Y coordinate to check.</param>
        /// <returns>Returns true if a point is within one square of the room.</returns>
        public bool IsNextTo(int checkX, int checkY)
        {
            if (checkX >= x - 1 && checkX < x + width + 1 && checkY >= y - 1 && checkY < y + height + 1)
                return true;
            return false;
        }
    }
}
