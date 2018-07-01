using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
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

        /// <summary>
        /// Creates a new empty dungeon of width and height dimensions using the seed provided.
        /// </summary>
        /// <param name="width">Width of the dungeon</param>
        /// <param name="height">Height of the dungeon</param>
        /// <param name="seed">Unique seed used to generate and place rooms.
        /// Generation is also dependent on height/width combination.</param>
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

        /// <summary>
        /// Try to add rooms to random locations until the attempt fails <c>roomAddAttempts</c> number of times.
        /// </summary>
        private void AddRooms()
        {
            int attempts = 0;
            while(attempts < roomAddAttempts)
            {
                if (!TryAddingRoom())
                    attempts += 1;
            }
        }

        /// <summary>
        /// Randomly generate a room of dimensions between <c>minRoomDimension</c> and
        /// <c>maxRoomDimension</c> and try to insert it in a random location.
        /// </summary>
        /// <returns>Returns true if room placement was successful.</returns>
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

        /// <summary>
        /// Check if a room can be placed in the current map.
        /// </summary>
        /// <param name="room">Room object to try placing</param>
        /// <returns>True if placement is possible.</returns>
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

        /// <summary>
        /// Carves the room's cells out of the current map.
        /// </summary>
        /// <param name="room">The room to carve</param>
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

        /// <summary>
        /// Finds all possible passage starting points and randomly adds passages beginning
        /// from one of them until all locations are filled.
        /// </summary>
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

        /// <summary>
        /// Begin a winding passage from the given starting coordinates using the
        /// recursive backtracker maze-generation algorithm. The result can be very
        /// windy, and could possibly use some tweakin to create straighter paths.
        /// </summary>
        /// <param name="start">Coordinates from which to begin the passage.</param>
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

        /// <summary>
        /// Scan entire map for valid passage starting points.
        /// </summary>
        /// <returns>Returns a list of possible starting coordinates.</returns>
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

        /// <summary>
        /// Checks if a cell could grow a passage to the left.
        /// </summary>
        /// <param name="c">Coordinates of the cell to grow from.</param>
        /// <returns>Returns true if left is a viable direction.</returns>
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

        /// <summary>
        /// Checks if a cell could grow a passage upwards.
        /// </summary>
        /// <param name="c">Coordinates of the cell to grow from.</param>
        /// <returns>Returns true if up is a viable direction.</returns>
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

        /// <summary>
        /// Checks if a cell could grow a passage to the right.
        /// </summary>
        /// <param name="c">Coordinates of the cell to grow from.</param>
        /// <returns>Returns true if right is a viable direction.</returns>
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

        /// <summary>
        /// Checks if a cell could grow a passage downward.
        /// </summary>
        /// <param name="c">Coordinates of the cell to grow from.</param>
        /// <returns>Returns true if down is a viable direction.</returns>
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

        /// <summary>
        /// Checks if a given location is surrounded by solid wall.
        /// </summary>
        /// <param name="x">X coordinate to check.</param>
        /// <param name="y">Y coordinate to check.</param>
        /// <returns>Returns true if cell is surrounded.</returns>
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
         
        /// <summary>
        /// Creates a door at the given location.
        /// </summary>
        /// <param name="x">X coordinate of cell to carve.</param>
        /// <param name="y">Y coordinate of cell to carve.</param>
        private void CarveDoor(int x, int y)
        {
            cells[x][y].type = CellType.Door;
        }

        /// <summary>
        /// Carves a passage at the given location.
        /// </summary>
        /// <param name="x">X coordinate of cell to carve.</param>
        /// <param name="y">Y coordinate of cell to carve.</param>
        private void CarvePassage(int x, int y)
        {
            cells[x][y].type = CellType.Passage;
            cells[x][y].group = this.groupCounter;
        }

        /// <summary>
        /// This joins disconnected sections of the map with doors using
        /// Dijkstra's algorithm (not really).
        /// </summary>
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

        /// <summary>
        /// Scans entire map for 1-cell-thick walls separating regions of cells with different
        /// group numbers, meaning they are not yet connected. These are basically valid door
        /// locations. The giant if statement could probably be rewritten for readability.
        /// </summary>
        /// <returns>Returns a list of possible door locations that would connect 
        /// presently disconnected regions of the map.</returns>
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

        /// <summary>
        /// Changes the group value of cells currently in the second group so
        /// that they join the first group. Used to "connect" regions of the map
        /// when generating doors, etc.
        /// </summary>
        /// <param name="first">Group no. to change to.</param>
        /// <param name="second">Group no. of cells to change.</param>
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

        /// <summary>
        /// This fills in dead-end paths until only <c>deadEndsToLeave</c> number remain.
        /// The lower the number, the easier the maze should be.
        /// </summary>
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

        /// <summary>
        /// Scans the map for all dead ends (passage cells with only one other passage leading away).
        /// </summary>
        /// <returns>Returns a list of coordinates of all dead end cells.</returns>
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

        /// <summary>
        /// Checks if the cell at the given coordinates is a dead end. A passage
        /// ending in a door does not count as a dead end.
        /// </summary>
        /// <param name="x">X coordinate of cell to check.</param>
        /// <param name="y">Y coordinate of cell to check.</param>
        /// <returns>Returns true if the cell is a dead end.</returns>
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

        /// <summary>
        /// Generates a typical dungeon with default settings.
        /// </summary>
        public void Generate()
        {
            AddRooms();
            AddPassages();
            ConnectAreas();
            FillInDeadEnds();
        }

        /// <summary>
        /// Debug function to draw the dungeon to the screen.
        /// Note that the output may not fit on the screen.
        /// </summary>
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

        /// <summary>
        /// Converts the dungeon into a list of string rows for use in the <c>Model</c> class.
        /// </summary>
        /// <returns></returns>
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
