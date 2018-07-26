//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using Ecs;

namespace Game.Interfaces
{
    public interface IAggressive
    {
        void OnAggro(GameObject target);
        void OnDeaggro();
    }
}
