//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using Ecs;

namespace Game.Interfaces
{
    public interface IDamageable
    {
        void ApplyDamage(GameObject source, int damage);

        void OnDeath(GameObject source);
    }
}
