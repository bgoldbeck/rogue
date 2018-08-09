#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using Ecs;

namespace Game.Interfaces
{
    public interface IDamageable
    {
        void ApplyDamage(GameObject source, int damage);

        void OnDeath(GameObject source);
    }
}
