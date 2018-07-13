//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DungeonMaker
{
    public enum CellType { Wall, Room, Passage, Door, LockedDoor, KeyMonster, Monster, Boss, Start }

    public class Cell
    {
        public CellType type = CellType.Wall;
        public int group = 0;

        /// <summary>
        /// A representation of one square on the map, which can be one of several types.
        /// </summary>
        public Cell()
        {
        }

        /// <summary>
        /// Checks whether a cell is solid wall or not.
        /// </summary>
        /// <returns>Returns true if the cell is a room passage or door.</returns>
        public bool IsOpen()
        {
            return this.type == CellType.Room || this.type == CellType.Passage || this.type == CellType.Door;
        }

        /// <summary>
        /// Gets a string representation of the cell, depending on its type.
        /// </summary>
        /// <returns>Returns a one-character representation.</returns>
        public String ToChar()
        {
            switch (this.type)
            {
                case CellType.Room:
                    return "r";
                case CellType.Passage:
                    return "p";
                case CellType.Door:
                    return "d";
                case CellType.LockedDoor:
                    return "l";
                case CellType.Wall:
                    return "w";
                case CellType.KeyMonster:
                    return "k";
                case CellType.Monster:
                    return "m";
                case CellType.Boss:
                    return "b";
                case CellType.Start:
                    return "s";
            }
            return " ";
        }
    }
}
