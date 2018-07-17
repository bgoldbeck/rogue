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
        public Monster(GameObject slot,String name,String description, int level,int health,
            int armor,int attack):base(name,description,level,health,armor,attack)
        {
            //Adds a Model component to the game object passed in.
            mapTile = (MapTile)slot.AddComponent(new MapTile());
            ai = (EnemyAI)slot.AddComponent(new EnemyAI());
            slot.AddComponent(new Aggro());
            slot.AddComponent(new Sound());

            mapTile.SetLightLevelAfterDiscovery(0.1f);
        }
    }
}
