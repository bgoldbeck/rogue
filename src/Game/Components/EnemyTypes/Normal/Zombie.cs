//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;

namespace Game.Components.EnemyTypes
{
    class Zombie : Enemy
    {
        public Zombie(Random rand, int level, bool isShiny)
            : base(
                  "Zombie",                     //Enemy's name
                  "Just a normal undead",       //Enemy's description
                  level,                        //Level of the enemy
                  7 * level,                    //Equation for the enemy's health.
                  (level > 1) ? level - 1 : 0,  //Equation for the enemy's armor.
                  2 * level,                        //Equation for the enemy's attack.  
                  10 * level,                    //xp given by beating this enemy.
                  isShiny,
                  rand
                  )
        {
        }

        public override void Start()
        {
            base.Start();
            mapTile.character = 'z';                //enemy's model
            if (isShiny)                            //Color
            {
                mapTile.color.Set(255, 215, 0);
            }
            else
            {
                mapTile.color.Set(0, 120, 120);
            }
            ai.SetRate(3.0f / ((isShiny) ? 2 : 1));                       //Time between each move.
            healthRegen.SetHealthRegen(2.0f / ((isShiny) ? 2 : 1));       //Health regen (seconds for 1 health regen).
            aggro.SetAggroPatience(5.0f);                                //Seconds before it gives up.
            aggro.SetAggroRange(16);                                     //Distance it can see the player at.
        }
    }
}
