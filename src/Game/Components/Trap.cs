#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using Ecs;
using Game.Interfaces;

namespace Game.Components
{
    class Trap : Component, IInteractable, IHidden
    {
        protected MapTile mapTile;
        private bool trapSet = true;
        private bool trapVisible = false;
        private int trapDamage = 0;

        public Trap(int damage)
        {
            trapDamage = damage;
        }
        public override void Start()
        {
            base.Start();
            mapTile = (MapTile)this.gameObject.AddComponent(new MapTile());

            mapTile.character = 'M';                    //Enemy's model
            mapTile.color.Set(110, 85, 20);             //Color
            return;
        }

        public override void Update()
        {
            base.Update();
            //Can traps reset themself?
            return;
        }

        public override void Render()
        {
            return;
        }

        public void OnInteract(GameObject objectInteracting, object interactorType)
        {
            if (!trapSet || objectInteracting == null) { return; }
            if (!(interactorType is IDamageable)) { return; }

            IDamageable trapVictim = (IDamageable)interactorType;
            trapVictim.ApplyDamage(gameObject,trapDamage);
            trapSet = false;
        }

        public void Reveal()
        {
            if (!trapVisible)
            {
                trapVisible = true;
            }
        }

        public bool IsInteractable
        {
            get
            {
                return trapSet;
            }
        }

        public bool IsHidden
        {
            get
            {
                return trapVisible;
            }
        }
    }
}
