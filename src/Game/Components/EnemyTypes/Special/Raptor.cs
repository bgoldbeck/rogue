//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

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
                  "Raptor",                     //Enemy's name
                  "Allen!",                     //Enemy's description
                  level,                        //Level of the enemy
                  4 * level,                    //Equation for the enemy's health.
                  level ,                       //Equation for the enemy's armor.
                  2 + level,                    //Equation for the enemy's attack. 
                  15 * level                    //xp given by beating this enemy.
                  )
        {
        }
        public override void Start()
        {
            base.Start();
            mapTile.character = 'r';                    //Enemy's model
            mapTile.color.Set(180, 0, 0);               //Color
            ai.SetRate(2);                              //Time between each move.
        }
    }
}
