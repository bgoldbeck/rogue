#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using Ecs;
using Game.Interfaces;
using Game.Interfaces.Markers;

namespace Game.Components
{
    class Wall : Component, IInteractable
    {
        private bool bedRock = false;

        public Wall(bool isBedRock)
        {
            bedRock = isBedRock;
        }

        public override void Start()
        {
            return;
        }

        public override void Update()
        {
            return;
        }

        public override void Render()
        {
            return;
        }

        public void OnInteract(GameObject objectInteracting, object interactorType)
        {
            if(objectInteracting == null) { return; }
            if(!(interactorType is IRage)) { return; }

            IRage ragingEnemy = (IRage)interactorType;

            if (ragingEnemy.isRaging)
            {
                MapManager.CurrentMap().PopObject(transform.position.x, transform.position.y);
                MapManager.CurrentNavigationMap().RemoveObject(transform.position);
                GameObject.Destroy(gameObject);
            }
        }

        public bool IsInteractable
        {
            get{
                return !bedRock;
            }
        }

    }
}