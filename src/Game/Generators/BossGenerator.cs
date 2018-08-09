#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using System;

using Game.Components.EnemyTypes;
using Ecs;

namespace Game.Generators
{
    class BossGenerator
    {
        static public void Fill(Random rand, int level, GameObject slot, int oneInNShiny)
        {
            bool isShiny = (rand.Next() % oneInNShiny == 0);
            switch (rand.Next() % 1)
            {
                case 0:
                    slot.AddComponent(new Minotaur(rand, level, isShiny));
                    break;
            }
        }
    }
}
