//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Text;


namespace Game.Components.EnemyTypes
{
    class Snake : Enemy
    {
        public Snake(Random rand, int level) 
            :base(      
                  "Snake",               //enemy's name
                  "Snake? SNAKE!!!!",    //enemy's description
                  level,                 //Level of the enemy
                  2 + (3 * level),       //Equation for the enemy's health.
                  level,                 //Equation for the enemy's armor.
                  level + 1,             //Equation for the enemy's attack.
                  11 * level             //xp given by beating this enemy.
                  )
        {
        }

        public override void Start()
        {
            base.Start();
            mapTile.character = 's';                //enemy's model
            mapTile.color.Set(255, 80, 80);         //Color
            ai.SetRate(2);                          //Time between each move.
        }                           
    }
}
