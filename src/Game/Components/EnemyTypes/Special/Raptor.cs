//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;

using Game.Interfaces.Markers;

namespace Game.Components.EnemyTypes
{
    class Raptor : Enemy, IDoorOpener, IXRayVision
    {
        public bool HasVision { get; } = true;
        public Raptor(Random rand, int level, bool isShiny)
            : base(
                  "Raptor",                     //Enemy's name
                  "Allen!",                     //Enemy's description
                  level,                        //Level of the enemy
                  4 * level,                    //Equation for the enemy's health.
                  level ,                       //Equation for the enemy's armor.
                  2 + level,                    //Equation for the enemy's attack. 
                  15 * level,                    //xp given by beating this enemy.
                  isShiny,
                  rand
                  )
        {
        }

        public override void Start()
        {
            base.Start();
            mapTile.character = 'r';                    //Enemy's model
            if (isShiny)                            //Color
            {
                mapTile.color.Set(255, 215, 0);
            }
            else
            {
                mapTile.color.Set(180, 0, 0);
            }
            ai.SetRate(0.75f / ((isShiny) ? 2 : 1));                          //Time between each move.
            healthRegen.SetHealthRegen(12.0f / ((isShiny) ? 2 : 1));          //Health regen (seconds for 1 health regen).
        }

    }
}
