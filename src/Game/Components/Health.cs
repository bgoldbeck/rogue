//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Text;

using Ecs;
using Game.Interfaces;

namespace Game.Components
{
    class Health : Component, IRegen
    {
        private bool inBattle = false;
        long timeTillRegen = 0;
        long timeBetweenRegen = 20000;

        public override void Start()
        {
            timeTillRegen = timeBetweenRegen + Time.getCurrentTime();
        }

        public override void Update()
        {
            if(timeTillRegen < Time.getCurrentTime())
            {
                if (inBattle)
                {
                    inBattle = false;
                }
                else
                {
                    gameObject.SendMessage<Actor>("RegenHP");
                }
                timeTillRegen = timeBetweenRegen + Time.getCurrentTime();
            }
        }

        public void InBattle()
        {
            inBattle = true;
        }
    }
}
