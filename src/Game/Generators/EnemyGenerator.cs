#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using System;

using Game.Components.EnemyTypes;
using Ecs;

namespace Game.Generators
{
    /// <summary>
    /// Manages dealing with randomly generating monsters from the implemented monsters.
    /// </summary>
    class EnemyGenerator
    {
        /// <summary>
        /// Takes an inputted GameObject and adds an enemy component to it.
        /// </summary>
        /// <param name="rand">The random seed used to generate the enemy.</param>
        /// <param name="level">The level of the enemy generated.</param>
        /// <param name="slot">The GameObject that this component is being added to.</param>
        /// <param name="oneInNShiny">The odds of a shiny monster (one out of...).</param>
        static public void Fill(Random rand,int level, GameObject slot, int oneInNShiny)
        {
            bool isShiny = (rand.Next() % oneInNShiny == 0);
            switch (rand.Next() % 5)
            {
                case 0:
                    slot.AddComponent(new Snake(rand, level, isShiny));
                    break;
                case 1:
                    slot.AddComponent(new Goblin(rand, level, isShiny));
                    break;
                case 2:
                    slot.AddComponent(new Raptor(rand, level, isShiny));
                    break;
                case 3:
                    slot.AddComponent(new Ninja(rand, level, isShiny));
                    break;
                case 4:
                    slot.AddComponent(new Zombie(rand, level, isShiny));
                    break;
            }
        }
    }
}