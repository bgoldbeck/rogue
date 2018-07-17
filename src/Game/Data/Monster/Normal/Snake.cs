using System;
using System.Collections.Generic;
using System.Text;

using Ecs;
using Game.Components;

namespace Game.Data.Monster
{
    class Snake : Enemy
    {
        public Snake(Random rand, int level) 
            :base(      
                  "Snake",               //Monster's name
                  "Snake? SNAKE!!!!",    //Monster's description
                  level,                 //Level of the monster
                  2 + (3 * level),       //Equation for the monster's health.
                  level,                 //Equation for the monster's armor.
                  level + 1              //Equation for the monster's attack.
                  )
        {
        }

        public override void Start()
        {
            base.Start();
            mapTile.character = 's';                //Monster's model
            mapTile.color.Set(255, 80, 80);         //Color
            ai.SetRate(2);                          //Time between each move.
        }                           
    }
}
