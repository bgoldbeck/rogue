using System;
using System.Collections.Generic;
using System.Text;

using Game.Interfaces;
using Ecs;

namespace Game.Components.EnemyTypes
{
    class Ninja : Enemy, IHidden
    {
        public bool IsHidden { protected set; get; } = true;

        public Ninja(Random rand, int level)
        : base(
              "Ninja",                      //Enemy's name
              "You see nothing...",         //Enemy's description
              level,                        //Level of the enemy
              4 * level,                    //Equation for the enemy's health.
              (level > 1) ? level - 1 : 0,   //Equation for the enemy's armor.
              3 + level,                    //Equation for the enemy's attack. 
              15 * level                    //xp given by beating this enemy.
              )
        {
        }

        public override void Start()
        {
            base.Start();
            mapTile.character = '█';                    //Enemy's model
            mapTile.color.Set(1, 0, 0);                 //Color
            ai.SetRate(2);                              //Time between each move.
        }

        public void Reveal()
        {
            IsHidden = false;
            mapTile.character = 'n';
            mapTile.color.Set(30, 30, 30);
        }

    }
}
