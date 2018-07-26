//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;

namespace Game.Components.EnemyTypes
{
    class Goblin : Enemy
    {
        public Goblin(Random rand, int level, bool isShiny)
            : base(
                  "Goblin",                     //Enemy's name
                  "Just a normal Goblin",       //Enemy's description
                  level,                        //Level of the enemy
                  5 * level,                    //Equation for the enemy's health.
                  (level > 1) ? level - 1 : 0,  //Equation for the enemy's armor.
                  3 + level,                    //Equation for the enemy's attack.  
                  10 * level,                    //xp given by beating this enemy.
                  isShiny,
                  rand
                  )
        {
        }

        public override void Start()
        {
            base.Start();
            mapTile.character = 'g';                //enemy's model
            if (isShiny)                            //Color
            {
                mapTile.color.Set(255, 215, 0);
            }
            else
            {
                mapTile.color.Set(0, 180, 0);
            }
            ai.SetRate(1.5f / ((isShiny) ? 2 : 1)); //Time between each move.
            healthRegen.SetHealthRegen(12.0f / ((isShiny) ? 2 : 1));      //Health regen (seconds for 1 health regen).
        }
    }
}
