#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

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
            aggro.SetAggroPatience(6.0f);                                     //Seconds before it gives up.
            aggro.SetAggroRange(10);                                          //Distance it can see the player at.
        }

    }
}
