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
        private List<List<CellState>> cellGrid;
        private List<List<GameObject>> objectGrid;

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;

            // Initialize cell grid
            cellGrid = new List<List<CellState>>();
            for (int x = 0; x < width; ++x)
            {
                List<CellState> row = new List<CellState>();
                for (int y = 0; y < height; ++y)
                {
                    row.Add(CellState.Open);
                }
                this.cellGrid.Add(row);
            }

            // Initialize object grid
            objectGrid = new List<List<GameObject>>();
            for (int x = 0; x < width; ++x)
            {
                List<GameObject> row = new List<GameObject>();
                for (int y = 0; y < height; ++y)
                {
                    row.Add(null);
                }
                this.objectGrid.Add(row);
            }
        }

        public void Regen(int width, int height)
        {
            this.width = width;
            this.height = height;
            CreateLevel(1);
        }

        public override void Start()
        {
            Model mapModel = (Model)this.gameObject.AddComponent(new Model());
            CreateLevel(1);
            return;
        }

        public override void Update()
        {
            Model mapModel = (Model)gameObject.GetComponent<Model>();
            List<String> updated = new List<String>();

            for (int y = 0; y < height; ++y)
            {
                StringBuilder sb = new StringBuilder();
                for (int x = 0; x < width; ++x)
                {
                    GameObject go = PeekObject(x, y);
                    if (go != null)
                    {
                        Model m = (Model)go.GetComponent(typeof(Model));
                        sb.Append(m.model[0]);
                    }
                    else if (cellGrid[x][y] == CellState.Blocked)
                        sb.Append("█");
                    else
                        sb.Append(" ");
                }
                updated.Add(sb.ToString());
            }
            mapModel.model = updated;
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
                            objectGrid[x][y] = sm.CreateDoor();
                            break;
                        case "m":
                            objectGrid[x][y] = sm.CreateEnemy(level);
                            break;
                        case "w":
                            cellGrid[x][y] = CellState.Blocked;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public CellState GetCellState(int x, int y)
        {
            Debug.Log("GetCellState called with x = " + x + ", y = " + y + ".");
            return cellGrid[x][y];
        }

        public GameObject PeekObject(int x, int y)
        {
            Debug.Log("PeekObject called with x = " + x + ", y = " + y + ".");
            return objectGrid[x][y];
        }

        public GameObject PopObject(int x, int y)
        {
            Debug.Log("PopObject called with x = " + x + ", y = " + y + ".");
            GameObject result = objectGrid[x][y];
            objectGrid[x][y] = null;
            return result;
        }
    }
}