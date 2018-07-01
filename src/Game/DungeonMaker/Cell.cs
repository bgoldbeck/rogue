using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public enum CellType { Wall, Room, Passage, Door }

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
                    return " ";
                case CellType.Passage:
                    return "░";
                case CellType.Door:
                    return "d";
                case CellType.Wall:
                    return "█";
            }
            return " ";
        }
    }
}
