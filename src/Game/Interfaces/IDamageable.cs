//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecs;

namespace Game.Interfaces
{
    public interface IDamageable
    {
        void ApplyDamage(GameObject source, int damage);

        void OnDeath();
    }
}
