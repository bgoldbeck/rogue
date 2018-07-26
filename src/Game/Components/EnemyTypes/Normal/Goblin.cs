//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;

namespace Game.Components.EnemyTypes
{
    class Goblin : Enemy
    {
        public Goblin(Random rand, int level)
            : base(
                  "Goblin",                     //Enemy's name
                  "Just a normal Goblin",       //Enemy's description
                  level,                        //Level of the enemy
                  5 * level,                    //Equation for the enemy's health.
                  (level > 1) ? level - 1 : 0,  //Equation for the enemy's armor.
                  3 + level,                    //Equation for the enemy's attack.  
                  10 * level                    //xp given by beating this enemy.
                  )
        {
        }

        public override void Start()
        {
            base.Start();
            mapTile.character = 'g';                //enemy's model
            mapTile.color.Set(0, 180, 0);           //Color
            ai.SetRate(1.5f);                       //Time between each move.
            healthRegen.SetHealthRegen(12.0f);      //Health regen (seconds for 1 health regen).
        }
    }
}
