//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;

using Ecs;
using Game.Generators;
using IO;

namespace Game.Components
{
    class SpawnManager : Component
    {
        //private Tuple<string, string, int, int>[] enemyData =
        //{ Tuple.Create<string, string, int, int>("s", "Snake", 2, 2 ) };
        private static Random rand = new Random();
        private static int oneInNShiny = 100; //On average, in how many enemies a shiny is spawned.

        public SpawnManager()
        {
        }

        public override void Start()
        {
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

        public static GameObject CreateBoss(int x, int y, int level)
        {
            GameObject go = GameObject.Instantiate("Boss");
            //MonsterGenerator.Fill(rand, level, go);
            BossGenerator.Fill(rand, level, go, oneInNShiny);
            go.transform.position = new Vec2i(x, y);
            //go.transform.position.x = x;
            //go.transform.position.y = y;
            Inventory i = (Inventory)go.AddComponent(new Inventory());
            i.Add(new Item("The Ultimate Thing"));
            MapTile mt = (MapTile)go.GetComponent<MapTile>();
            //mt.character = 'ß';

            return go;
        }

        public static GameObject CreateEnemy(int x, int y, int level, bool hasKey = false)
        {
            GameObject go = GameObject.Instantiate("Monster");
            MonsterGenerator.Fill(rand, level, go, oneInNShiny);
            go.transform.position = new Vec2i(x, y);
            //go.transform.position.x = x;
            //go.transform.position.y = y;
            Inventory i = (Inventory)go.AddComponent(new Inventory());
            if (hasKey)
            {
                i.Add(new Item("Key"));
                MapTile m = (MapTile)go.GetComponent<MapTile>();
                //m.character = 'k';
            }

            return go;
        }

        public static GameObject CreateDoor(int x, int y)
        {
            GameObject go = GameObject.Instantiate("Door");
            go.AddComponent(new Door());
            go.transform.position = new Vec2i(x, y);
            //go.transform.position.x = x;
            //go.transform.position.y = y;
            go.AddComponent(new MapTile('d', new Color(210, 105, 30)));
            go.AddComponent(new Sound());
            return go;
        }

        public static GameObject CreateLockedDoor(int x, int y)
        {
            GameObject go = GameObject.Instantiate("Locked Door");
            go.AddComponent(new Door(true));
            go.transform.position = new Vec2i(x, y);
            //go.transform.position.x = x;
            //go.transform.position.y = y;
            go.AddComponent(new MapTile('l', new Color(210, 105, 30)));

            return go;
        }

        public static GameObject CreateWall(int x, int y, bool bedRock)
        {
            GameObject go = GameObject.Instantiate("Wall");
            go.AddComponent(new Wall(bedRock));
            go.transform.position = new Vec2i(x, y);
            //go.transform.position.x = x;
            //go.transform.position.y = y;
            int value = rand.Next(80, 180);
            go.AddComponent(new MapTile('█', new Color(value, value, value)));

            return go;
        }
    }
}
