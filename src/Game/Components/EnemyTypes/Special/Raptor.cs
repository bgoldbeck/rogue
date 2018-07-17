using System;
using System.Collections.Generic;
using System.Text;

using Game.Interfaces;

namespace Game.Components.EnemyTypes
{
    class Raptor : Enemy, IDoorOpener, IXRayVision
    {
        public Raptor(Random rand, int level)
            : base(
                  "Raptor",                  //Monster name
                  "Allen!",                  //Monster description
                  level,                     //Level of the monster
                  4 * level,                 //Equation for the monster's health.
                 (level > 1) ? level - 1 : 0,//Equation for the monster's armor.
                  2 + level                  //Equation for the monster's attack.                        
                  )
        {
        }
        public override void Start()
        {
            base.Start();
            mapTile.character = 'r';                    //Monster's model
            mapTile.color.Set(180, 0, 0);               //Color
            ai.SetRate(2);                              //Time between each move.
        }
    }
}
