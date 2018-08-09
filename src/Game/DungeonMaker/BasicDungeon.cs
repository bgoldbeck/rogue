#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.DungeonMaker
{
    public class BasicDungeon
    {
        const int roomAddAttempts = 200; // The higher this number the more packed rooms will be
        const int minRoomWidth = 6; // Smallest width of a room
        const int minRoomHeight = 3;    // Smallest height of a room
        const int maxRoomWidth = 24;    // Largest width of a room
        const int maxRoomHeight = 12;   // Largest height of a room
        const int deadEndsToLeave = 8; // Should be at least '1', since the player starts in a dead end.
        const int chanceToCarveStraightPassage = 95; // Percentage chance for passage to go straight
        const float monstersPerBlock = .022f; // Determines how many monsters show up in room areas

        private List<List<Cell>> cells;
        private List<Room> roomList;
        private Room lockedRoom;
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
        public BasicDungeon(int width, int height, int seed)
        {
            this.width = width;
            this.height = height;
            this.rand = new Random(seed);
            this.groupCounter = 0;
        }

        /// <summary>
        /// Generates a dungeon based some ideas described by Bob Nystrom in his
        /// wonderful article "Rooms and Mazes".
        /// http://journal.stuffwithstuff.com/2014/12/21/rooms-and-mazes/with
        /// </summary>
        public void Generate()
        {
            do
            {
                Clear();
                AddRooms();
                AddPassages();
                ConnectAreas();
                FillInDeadEnds();
                AddMonsters();
                GiveKeyToMonster();
                AddStartingPoint();
            } while (CountLockedDoors() != 1);
        }

        /// <summary>
        /// Initializes the cells to empty.
        /// </summary>
        private void Clear()
        {
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
            lockedRoom = null;
        }

        /// <summary>
        /// Try to add rooms to random locations until the attempt fails <c>roomAddAttempts</c> number of times.
        /// </summary>
        private void AddRooms()
        {
            // Add one locked room
            AddLockedRoom();

            int attempts = 0;
            while(attempts < roomAddAttempts)
            {
                if (TryAddingRoom())
                    attempts = 0;
                else
                    attempts += 1;
            }
        }

        /// <summary>
        /// Add one 'locked' room, containing a boss monster
        /// </summary>
        private void AddLockedRoom()
        {
            int roomWidth = rand.Next(minRoomWidth, maxRoomWidth + 1);
            int roomHeight = rand.Next(minRoomHeight, maxRoomHeight + 1);
            int x = rand.Next(1, this.width - roomWidth);
            int y = rand.Next(1, this.height - roomHeight);
            Room room = new Room(roomWidth, roomHeight, x, y);
            Carve(room);
            this.lockedRoom = room;
            this.roomList.Add(room);
        }

        /// <summary>
        /// Randomly generate a room of dimensions between <c>minRoomDimension</c> and
        /// <c>maxRoomDimension</c> and try to insert it in a random location.
        /// </summary>
        /// <returns>Returns true if room placement was successful.</returns>
        private bool TryAddingRoom()
        {
            int roomWidth = rand.Next(minRoomWidth, maxRoomWidth + 1);
            int roomHeight = rand.Next(minRoomHeight, maxRoomHeight + 1);
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
        /// For use in tracking the direction as paths grow to try and straighten things out.
        /// </summary>
        private enum Direction { None, Up, Right, Down, Left }

        /// <summary>
        /// Begin a winding passage from the given starting coordinates using the
        /// recursive backtracker maze-generation algorithm with some custom code
        /// to straighten paths based on the constant chanceToCarveStraightPassage.
        /// Recursive backtracker algorithm is based on the one described in the
        /// book "Mazes for Programmers", by Jamis Buck.
        /// </summary>
        /// <param name="start">Coordinates from which to begin the passage.</param>
        private void AddPassage(Coord start)
        {
            Stack<Coord> recursionStack = new Stack<Coord>();
            recursionStack.Push(start);
            CarvePassage(start.x, start.y);
            bool done = false;
            Direction lastDirection = Direction.None;
            do
            {
                // Growing the path forward until a dead end
                while (true)
                {
                    if (recursionStack.Count() == 0)
                        break;
                    Coord current = recursionStack.Peek();
                    
                    //Get possible directions, noting if going straight is a possibility
                    Coord straight = null;
                    List<Tuple<Direction, Coord>> possibleCells = new List<Tuple<Direction, Coord>>();
                    if (current.x > 1 && CanGrowLeft(current))
                    {
                        Coord possibleNext = new Coord(current.x - 1, current.y);
                        if (lastDirection == Direction.Left)
                            straight = possibleNext;
                        possibleCells.Add(Tuple.Create<Direction, Coord>(Direction.Left, possibleNext));
                    }
                    if (current.y > 1 && CanGrowUp(current))
                    {
                        Coord possibleNext = new Coord(current.x, current.y - 1);
                        if (lastDirection == Direction.Up)
                            straight = possibleNext;
                        possibleCells.Add(Tuple.Create<Direction, Coord>(Direction.Up, possibleNext));
                    }
                    if (current.x < this.width - 2 && CanGrowRight(current))
                    {
                        Coord possibleNext = new Coord(current.x + 1, current.y);
                        if (lastDirection == Direction.Right)
                            straight = possibleNext;
                        possibleCells.Add(Tuple.Create<Direction, Coord>(Direction.Right, possibleNext));
                    }
                    if (current.y < this.height - 2 && CanGrowDown(current))
                    {
                        Coord possibleNext = new Coord(current.x, current.y + 1);
                        if (lastDirection == Direction.Down)
                            straight = possibleNext;
                        possibleCells.Add(Tuple.Create<Direction, Coord>(Direction.Down, possibleNext));
                    }

                    // If no directions are possible, end the path
                    if (possibleCells.Count() == 0)
                        break;

                    // If going straight is possible, and rng is within the % chance, continue straight
                    if (straight != null && this.rand.Next(0, 100) < chanceToCarveStraightPassage)
                    {
                        recursionStack.Push(straight);
                        CarvePassage(straight.x, straight.y);
                    }
                    else // Otherwise choose a random direction to carve
                    {
                        Tuple<Direction, Coord> choice = possibleCells[this.rand.Next(0, possibleCells.Count())];
                        lastDirection = choice.Item1;
                        Coord location = choice.Item2;
                        recursionStack.Push(location);
                        CarvePassage(location.x, location.y);
                    }
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
            if (lockedRoom.IsNextTo(x, y))
                 cells[x][y].type = CellType.LockedDoor;
            else
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
        /// This joins disconnected sections of the map with the minimum 
        /// number of doors required.
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
            if (cells[x - 1][y].type == CellType.LockedDoor ||
                cells[x][y - 1].type == CellType.LockedDoor ||
                cells[x + 1][y].type == CellType.LockedDoor ||
                cells[x][y + 1].type == CellType.LockedDoor)
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

        private void AddMonsters()
        {
            int blockCount = 0;
            foreach (Room room in roomList)
            {
                // If it's the locked room, add one boss in the center
                if (room == lockedRoom)
                {
                    cells[room.x + room.width / 2][room.y + room.height / 2].type = CellType.Boss;
                }
                else // Otherwise add some monsters now and then
                {
                    for (int x = 0; x < room.width; ++x)
                    {
                        for (int y = 0; y < room.height; ++y)
                        {
                            if (1 / (float)blockCount++ < monstersPerBlock)
                            {
                                cells[room.x + x][room.y + y].type = CellType.Monster;
                                blockCount = 0;
                            }
                        }
                    }
                }
            }
        }

        private void GiveKeyToMonster()
        {
            // Make list of monster cells
            List<Coord> monsters = new List<Coord>();
            for (int x = 1; x < this.width - 1; ++x)
            {
                for (int y = 1; y < this.height - 1; ++y)
                {
                    if (cells[x][y].type == CellType.Monster)
                    {
                        monsters.Add(new Coord(x, y));
                    }
                }
            }

            // Give key to random monster
            Coord choice = monsters[rand.Next(0, monsters.Count)];
            cells[choice.x][choice.y].type = CellType.KeyMonster;
        }

        private void AddStartingPoint()
        {
            List<Coord> deadEnds = FindDeadEnds();
            Coord choice = deadEnds[rand.Next(0, deadEnds.Count)];
            cells[choice.x][choice.y].type = CellType.Start;
        }

        /// <summary>
        /// Counts the number of locked doors on the map.
        /// Used to determine if the 'boss' room has only one entrance.
        /// </summary>
        /// <returns>Returns the number of locked doors on the map.</returns>
        private int CountLockedDoors()
        {
            int count = 0;
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if (cells[x][y].type == CellType.LockedDoor)
                        ++count;
                }
            }
            return count;
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

        public List<List<String>> Package()
        {
            List<List<String>> result = new List<List<String>>();
            for (int x = 0; x < width; ++x)
            {
                List<String> row = new List<String>();
                for (int y = 0; y < height; ++y)
                {
                    row.Add(cells[x][y].ToChar());
                }
                result.Add(row);
            }

            return result;
        }
    }
}
