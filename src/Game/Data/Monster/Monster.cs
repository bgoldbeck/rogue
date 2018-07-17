using System;
using System.Collections.Generic;
using System.Text;

using Game.Components;
using Ecs;

namespace Game.Data.Monster
{
    class Monster : Enemy
    {
        protected MapTile mapTile;
        protected EnemyAI ai;
        public Monster(String name,String description, int level,int health,
            int armor,int attack):base(name,description,level,health,armor,attack)
        {
        }
        public override void Start()
        {
            base.Start();
            //Adds a Model component to the game object passed in.
            mapTile = (MapTile)this.gameObject.AddComponent(new MapTile());
            ai = (EnemyAI)this.gameObject.AddComponent(new EnemyAI());
            this.gameObject.AddComponent(new Aggro());
            this.gameObject.AddComponent(new Sound());
            mapTile.SetLightLevelAfterDiscovery(0.1f);
        }
    }
}
