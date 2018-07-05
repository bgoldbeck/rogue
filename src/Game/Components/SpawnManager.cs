using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using Game.Interfaces;
using Game.Data;

namespace Game.Components
{
    class SpawnManager : Component
    {
        private Tuple<string, string, int, int>[] enemyData =
        { Tuple.Create<string, string, int, int>("s", "Snake", 2, 2 ) };
        private Random rand = new Random();

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

        public GameObject CreateEnemy(int level)
        {
            GameObject go = GameObject.Instantiate("Monster-" + level + "-");
            //go.AddComponent(new Enemy());

            /*Model m = (Model)go.AddComponent(new Model());
            string representation = enemyData[rand.Next(0, enemyData.Length)].Item1;
            m.model.Add(representation);*/
            MonsterGenerator.Fill(rand, level, go);

            return go;
        }

        public GameObject CreateDoor()
        {
            GameObject go = GameObject.Instantiate("Door-");
            go.AddComponent(new Door());

            Model m = (Model)go.AddComponent(new Model());
            m.model.Add("d");

            return go;
        }

    }
}
