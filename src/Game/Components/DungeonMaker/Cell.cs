using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public enum CellType { Wall, Room, Passage, Door }

    public class Cell
    {
        public CellType type = CellType.Wall;
        public int group = 0;

        public Cell()
        {
        }

        public bool IsOpen()
        {
            return this.type == CellType.Room || this.type == CellType.Passage || this.type == CellType.Door;
        }

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
