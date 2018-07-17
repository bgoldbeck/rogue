using System;
using System.Collections.Generic;
using System.Text;

using Ecs;

namespace Game.Data.Monster
{
    class Goblin : Monster
    {
        public Goblin(Random rand, int level, GameObject slot)
            : base(slot,
                  "Goblin",                  //Monster name
                  "Just a normal Goblin",    //Monster description
                  level,                     //Level of the monster
                  5 * level,                 //Equation for the monster's health.
                  (level > 1) ? level - 1 : 0,//Equation for the monster's armor.
                  3 + level                  //Equation for the monster's attack.                        
                  )
        {
            mapTile.character = 'g';                //Monster's model
            mapTile.color.Set(255, 80, 80);         //Color
            ai.SetRate(2);                          //Time between each move.
        }
    }
}
