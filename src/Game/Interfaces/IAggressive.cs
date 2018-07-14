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
