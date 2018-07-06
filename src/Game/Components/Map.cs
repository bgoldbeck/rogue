//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using Game.DungeonMaker; 
using Game.DataStructures;
using IO;

namespace Game.Components
{
    class Map : Component
    {

        private int width;
        private int height;
        private List<List<GameObject>> objects;
        public int startingX = 0;
        public int startingY = 0;

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;

            // Initialize object storage
            objects = new List<List<GameObject>>();
            for (int x = 0; x < width; ++x)
            {
                List<GameObject> row = new List<GameObject>();
                for (int y = 0; y < height; ++y)
                {
                    row.Add(null);
                }
                objects.Add(row);
            }
        }

        public override void Start()
        {
            CreateLevel(1);
            return;
        }

        public override void Update()
        {
            return;
        }

        public override void Render()
        {
            return;
        }

        private void CreateLevel(int level)
        {
            BasicDungeon dm = new BasicDungeon(this.width, this.height, (int)DateTime.Now.Ticks);
            dm.Generate();

            SpawnManager sm = new SpawnManager();

            List<List<String>> blueprint = dm.Package();
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    switch (blueprint[x][y])
                    {
                        case "d":
                            objects[x][y] = sm.CreateDoor(x, y);
                            break;
                        case "m":
                            objects[x][y] = sm.CreateEnemy(x, y, level);
                            break;
                        case "w":
                            objects[x][y] = sm.CreateWall(x, y);
                            break;
                        case "s":
                            this.startingX = x;
                            this.startingY = y;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public GameObject PeekObject(int x, int y)
        {
            Debug.Log("PeekObject called with x = " + x + ", y = " + y + ".");
            return objects[x][y];
        }

        public GameObject PopObject(int x, int y)
        {
            Debug.Log("PopObject called with x = " + x + ", y = " + y + ".");
            GameObject result = objects[x][y];
            objects[x][y] = null;
            return result;
        }
    }
}