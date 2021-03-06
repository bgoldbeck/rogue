﻿#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using Ecs;
using Game.Interfaces;

namespace Game.Components
{
    public class Sound : Component, IDamageable, IMovable, IInteractable
    {
        public bool IsInteractable { get { return true; } }

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

        public void ApplyDamage(GameObject source, int damage)
        {
            // We only care about the player applying damage to other enemies.
            //if (source.GetComponent<Player>() == null) return;
            // Play a rudimentary hit sound
            //Console.Beep(150, 100);
            return;
        }

        public void OnDeath(GameObject source)
        {
            // If the thing was killed by a player.
            //if (source.GetComponent<Player>() != null)
            //{
                //Console.Beep(75, 100);
            //}

            return;
        }

        public void OnMove(int dx, int dy)
        {
            return;
        }

        public void OnFailedMove()
        {
            // If the thing that couldnt move is not the player, who cares.
            //if (GetComponent<Player>() == null) return;
            
            //Console.Beep(37, 25);
            
            return;
        }

        public void OnInteract(GameObject objectInteracting, object interactorType)
        {

            return;
        }
    }
}
