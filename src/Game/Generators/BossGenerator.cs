using System;
using System.Collections.Generic;
using System.Text;

using Game.Components.EnemyTypes;
using Ecs;

namespace Game.Generators
{
    class BossGenerator
    {
        static public void Fill(Random rand, int level, GameObject slot)
        {
            switch (rand.Next() % 1)
            {
                case 0:
                    slot.AddComponent(new Minotaur(rand, level));
                    break;
            }
        }
    }
}
