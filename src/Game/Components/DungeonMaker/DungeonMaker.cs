using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public class DungeonMaker
    {
        const int roomAddAttempts = 100;
        const int minRoomDimension = 2;
        const int maxRoomDimension = 8;
        const int deadEndsToLeave = 8;

        private List<List<Cell>> cells;
        private List<Room> roomList;
        private int height;
        private int width;
        private Random rand;
        private int groupCounter;

        public DungeonMaker(int width, int height, int seed)
        {
            this.width = width;
            this.height = height;
            this.rand = new Random(seed);
            this.groupCounter = 0;
            this.cells = new List<List<Cell>>();
            for (int x = 0; x < width; ++x)
            {
                List<Cell> row = new List<Cell>();
                for (int y = 0; y < height; ++y)
                {
                    row.Add(new Cell());
                }
                this.cells.Add(row);
            }
            this.roomList = new List<Room>();
        }

        private void AddRooms()
        {
            int attempts = 0;
            while(attempts < roomAddAttempts)
            {
                if (!TryAddingRoom())
                    attempts += 1;
            }
        }

        private bool TryAddingRoom()
        {
            int roomWidth = rand.Next(minRoomDimension, maxRoomDimension + 1);
            int roomHeight = rand.Next(minRoomDimension, maxRoomDimension + 1);
            int x = rand.Next(1, this.width - roomWidth);
            int y = rand.Next(1, this.height - roomHeight);
            Room room = new Room(roomWidth, roomHeight, x, y);
            if (RoomCanBePlaced(room))
            {
                Carve(room);
                this.roomList.Add(room);
                return true;
            }
            return false;
        }

        private bool RoomCanBePlaced(Room room)
        {
            for (int x = room.x - 1; x <= room.x + room.width; ++x)
            {
                for (int y = room.y - 1; y <= room.y + room.height; ++y)
                {
                    if (cells[x][y].IsOpen())
                        return false;
                }
            }
            return true;
        }

        private void Carve(Room room)
        {
            for (int x = room.x; x < room.x + room.width; ++x)
            {
                for (int y = room.y; y < room.y + room.height; ++y)
                {
                    cells[x][y].type = CellType.Room;
                    cells[x][y].group = this.groupCounter;
                }
            }
            ++this.groupCounter;
        }

        private void AddPassages()
        {
            bool adding = true;
            while(adding)
            {
                List<Coord> locations = FindStartingPoints();
                if (locations.Count() == 0)
                    adding = false;
                else
                    AddPassage(locations[this.rand.Next(0, locations.Count())]);
            }
        }

        private void AddPassage(Coord start)
        {
            Stack<Coord> recursionStack = new Stack<Coord>();
            recursionStack.Push(start);
            CarvePassage(start.x, start.y);
            bool done = false;
            do
            {
                // Growing the path forward until a dead end
                while (true)
                {
                    //Console.WriteLine("Stack size = {0}.", recursionStack.Count());
                    if (recursionStack.Count() == 0)
                        break;
                    Coord current = recursionStack.Peek();
                    //Console.WriteLine("Current position: {0}, {1}", current.x, current.y);
                    List<Coord> possibleCells = new List<Coord>();
                    if (current.x > 1 && CanGrowLeft(current))
                        possibleCells.Add(new Coord(current.x - 1, current.y));
                    if (current.y > 1 && CanGrowUp(current))
                        possibleCells.Add(new Coord(current.x, current.y - 1));
                    if (current.x < this.width-2  && CanGrowRight(current))
                        possibleCells.Add(new Coord(current.x + 1, current.y));
                    if (current.y < this.height-2 && CanGrowDown(current))
                        possibleCells.Add(new Coord(current.x, current.y + 1));
                    if (possibleCells.Count() == 0)
                        break;
                    Coord choice = possibleCells[this.rand.Next(0, possibleCells.Count())];
                    //Console.WriteLine("Growing path to {0}, {1}.", choice.x, choice.y);
                    recursionStack.Push(choice);
                    CarvePassage(choice.x, choice.y);
                }

                // Backing up a step
                recursionStack.Pop();
                if (recursionStack.Count() == 0)
                    done = true;
            } while (!done);
            ++this.groupCounter;
        }

        private List<Coord> FindStartingPoints()
        {
            List<Coord> points = new List<Coord>();
            for(int x = 1; x < this.width-1; ++x)
            {
                for(int y = 1; y < this.height-1; ++y)
                {
                    if (!cells[x][y].IsOpen() && CellSurroundedBySolid(x, y))
                    {
                        points.Add(new Coord(x, y));
                    }
                }
            }

            return points;
        }

        private bool CanGrowLeft(Coord c)
        {
            int x = c.x-1;
            int y = c.y;
            if (!cells[x][y].IsOpen() &&
                !cells[x-1][y-1].IsOpen() &&
                !cells[x-1][y].IsOpen() &&
                !cells[x-1][y+1].IsOpen() &&
                !cells[x][y-1].IsOpen() &&
                !cells[x][y+1].IsOpen())
                return true;
            return false;
        }

        private bool CanGrowUp(Coord c)
        {
            int x = c.x;
            int y = c.y-1;
            if (!cells[x][y].IsOpen() &&
                !cells[x-1][y-1].IsOpen() &&
                !cells[x][y-1].IsOpen() &&
                !cells[x+1][y-1].IsOpen() &&
                !cells[x-1][y].IsOpen() &&
                !cells[x+1][y].IsOpen())
                return true;
            return false;
        }

        private bool CanGrowRight(Coord c)
        {
            int x = c.x+1;
            int y = c.y;
            if (!cells[x][y].IsOpen() &&
                !cells[x+1][y-1].IsOpen() &&
                !cells[x+1][y].IsOpen() &&
                !cells[x+1][y+1].IsOpen() &&
                !cells[x][y-1].IsOpen() &&
                !cells[x][y+1].IsOpen())
                return true;
            return false;
        }

        private bool CanGrowDown(Coord c)
        {
            int x = c.x;
            int y = c.y+1;
            if (!cells[x][y].IsOpen() &&
                !cells[x-1][y+1].IsOpen() &&
                !cells[x][y+1].IsOpen() &&
                !cells[x+1][y+1].IsOpen() &&
                !cells[x-1][y].IsOpen() &&
                !cells[x+1][y].IsOpen())
                return true;
            return false;
        }

        private bool CellSurroundedBySolid(int x, int y)
        {
            for (int i = x - 1; i <= x + 1; ++i)
            {
                for (int j = y - 1; j <= y + 1; ++j)
                {
                    if (cells[i][j].IsOpen())
                        return false;
                }
            }
            return true;
        }
         
        private void CarveDoor(int x, int y)
        {
            cells[x][y].type = CellType.Door;
        }

        private void CarvePassage(int x, int y)
        {
            cells[x][y].type = CellType.Passage;
            cells[x][y].group = this.groupCounter;
        }

        private void ConnectAreas()
        {
            bool searching = true;
            while (searching)
            {
                List<Coord> points = FindConnectingWalls();
                if (points.Count() == 0)
                    searching = false;
                else
                {
                    Coord choice = points[this.rand.Next(0, points.Count())];
                    int x = choice.x;
                    int y = choice.y;
                    bool verticalWall = cells[x - 1][y].IsOpen();
                    if (verticalWall)
                        JoinGroups(new Coord(x - 1, y), new Coord(x + 1, y));
                    else
                        JoinGroups(new Coord(x, y - 1), new Coord(x, y + 1));
                    CarveDoor(x, y);
                }
            }
        }

        private List<Coord> FindConnectingWalls()
        {
            List<Coord> results = new List<Coord>();
            for(int x = 2; x < this.width-2; ++x)
            {
                for(int y = 2; y < this.height-2; ++y)
                {
                    if (cells[x][y].type == CellType.Wall)
                    {
                        if (
                            (
                                cells[x - 1][y].type != CellType.Wall &&
                                cells[x + 1][y].type != CellType.Wall &&
                                cells[x - 1][y].type != CellType.Door &&
                                cells[x + 1][y].type != CellType.Door &&
                                cells[x - 1][y].group != cells[x + 1][y].group
                            )
                            ||
                            (
                                cells[x][y - 1].type != CellType.Wall &&
                                cells[x][y + 1].type != CellType.Wall &&
                                cells[x][y - 1].type != CellType.Door &&
                                cells[x][y + 1].type != CellType.Door &&
                                cells[x][y - 1].group != cells[x][y + 1].group
                            )
                        )
                            results.Add(new Coord(x, y));
                    }
                }
            }
            return results;
        }

        private void JoinGroups(Coord first, Coord second)
        {
            int firstGroup = cells[first.x][first.y].group;
            int secondGroup = cells[second.x][second.y].group;
            for(int x = 0; x < this.width; ++x)
            {
                for(int y = 0; y < this.height; ++y)
                {
                    if (cells[x][y].group == secondGroup)
                        cells[x][y].group = firstGroup;
                }
            }
        }

        private void FillInDeadEnds()
        {
            List<Coord> deadEnds = FindDeadEnds();
            while(deadEnds.Count() > deadEndsToLeave)
            {
                for(int i = 0; i < deadEnds.Count(); ++i)
                {
                    int x = deadEnds[i].x;
                    int y = deadEnds[i].y;
                    cells[x][y].type = CellType.Wall;
                    cells[x][y].group = 0;
                }
                deadEnds = FindDeadEnds();
            }
        }

        private List<Coord> FindDeadEnds()
        {
            List<Coord> results = new List<Coord>();
            for(int x = 1; x < this.width-1; ++x)
            {
                for(int y = 0; y < this.height-1; ++y)
                {
                    if (IsDeadEnd(x, y))
                        results.Add(new Coord(x, y));
                }
            }
            return results;
        }

        private bool IsDeadEnd(int x, int y)
        {
            if (cells[x][y].type != CellType.Passage)
                return false;
            if (cells[x-1][y].type == CellType.Door ||
                cells[x][y-1].type == CellType.Door ||
                cells[x+1][y].type == CellType.Door ||
                cells[x][y+1].type == CellType.Door)
                return false;
            int passageCount = 0;
            if (cells[x - 1][y].type == CellType.Passage)
                ++passageCount;
            if (cells[x][y - 1].type == CellType.Passage)
                ++passageCount;
            if (cells[x + 1][y].type == CellType.Passage)
                ++passageCount;
            if (cells[x][y + 1].type == CellType.Passage)
                ++passageCount;
            return passageCount == 1;
        }

        public void Generate()
        {
            AddRooms();
            AddPassages();
            ConnectAreas();
            FillInDeadEnds();
        }

        public void Draw()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < this.height; ++y)
            {
                for(int x = 0; x < this.width; ++x)
                {

                    sb.Append(cells[x][y].ToChar());
                }
                sb.Append("\n");
            }
            Console.Write(sb.ToString());
        }

        public List<String> Stringify()
        {
            List<String> result = new List<string>();

            for (int y = 0; y < this.height; ++y)
            {
                StringBuilder sb = new StringBuilder();
                for (int x = 0; x < this.width; ++x)
                {
                    sb.Append(cells[x][y].ToChar());
                }
                result.Add(sb.ToString());
            }

            return result;
        }
    }
}
