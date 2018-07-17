//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Text;
using Ecs;

namespace Game.Interfaces
{
    public interface IAggressive
    {
        void OnAggro(GameObject target);
        void OnDeaggro();
    }
}
