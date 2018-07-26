//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

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
