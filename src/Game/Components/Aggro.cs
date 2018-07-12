using System;
using System.Collections.Generic;
using System.Text;

using Ecs;

namespace Game.Components
{
    class Aggro :Component
    {
        private int aggroRange = 10;
        private int lastSeenPlayer = 0;
        private int determination = 3;

        public override void Start()
        {
            base.Start();
            return;
        }

        public override void Update()
        {
            base.Update();
            return;
        }


        public void TargetSearch(ref Transform target)
        {
            //If the enemy is close enough to the player, it saves the location it has seen the
            //player at and set the boolean that is has seen the player.
            Player player = Player.MainPlayer();

            if (Vec2i.Distance(player.transform.position, transform.position) < aggroRange)
            {
                target = player.transform;
            }
            //If the enemy can't see the player but has seen the player before. It checks how long
            //since the last time it has seen the player. If it has been too long, it sets the boolean
            //to false.
            else if (target != null)
            {
                if (++lastSeenPlayer > determination)
                {
                    target = null;
                }
            }
            return;
        }
    }
}
