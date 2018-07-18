using System;
using System.Collections.Generic;
using System.Text;

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
            mapTile = (MapTile)this.gameObject.AddComponent(new MapTile());
            return;
        }

        public override void Update()
        {
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
            trapVisible = true;
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
