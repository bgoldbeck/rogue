using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Components.EnemyTypes
{
    class Zombie : Enemy
    {
        public Zombie(Random rand, int level)
            : base(
                  "Zombie",                     //Enemy's name
                  "Just a normal undead",       //Enemy's description
                  level,                        //Level of the enemy
                  7 * level,                    //Equation for the enemy's health.
                  (level > 1) ? level - 1 : 0,  //Equation for the enemy's armor.
                  2 * level,                        //Equation for the enemy's attack.  
                  10 * level                    //xp given by beating this enemy.
                  )
        {
        }

        public override void Start()
        {
            base.Start();
            mapTile.character = 'z';                //enemy's model
            mapTile.color.Set(0, 120, 120);          //Color
            ai.SetRate(3.0f);                       //Time between each move.
            healthRegen.SetHealthRegen(2.0f);       //Health regen (seconds for 1 health regen).
        }
    }
}
