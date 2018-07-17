//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Text;


namespace Game.Components.EnemyTypes
{
    class Goblin : Enemy
    {
        public Goblin(Random rand, int level)
            : base(
                  "Goblin",                  //Monster name
                  "Just a normal Goblin",    //Monster description
                  level,                     //Level of the monster
                  5 * level,                 //Equation for the monster's health.
                  (level > 1) ? level - 1 : 0,//Equation for the monster's armor.
                  3 + level                  //Equation for the monster's attack.                        
                  )
        {
        }

        public override void Start()
        {
            base.Start();
            mapTile.character = 'g';                //Monster's model
            mapTile.color.Set(0, 180, 0);         //Color
            ai.SetRate(3);                          //Time between each move.
        }
    }
}
