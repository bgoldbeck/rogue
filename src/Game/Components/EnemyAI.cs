using System;
using System.Collections.Generic;
using System.Text;

using Ecs;
using IO;

namespace Game.Components
{
    class EnemyAI : Component
    {

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();
        }

        public void MakeMove(Transform target)
        {
            Enemy puppet = (Enemy)base.gameObject.GetComponent<Enemy>();
            if(puppet == null)
            {
                Debug.LogError("EnemyAI isn't a component of an Enemy.");
                return;
            }
            if (target != null)
            {
                SeekMove(target,puppet.Move);
            }
            else
            {
                RandomMove(puppet.Move);
            }
        }

        /// <summary>
        /// This method moves the enemy towards a known player.
        /// </summary>
        private void SeekMove(Transform target, Action<int,int> Move)
        {
            if (target == null) return;

            Random rand = new Random();

            //Figures out which direction on the it has to move to head towards the player.
            Vec2i deltaMove = target.position - transform.position;
            if (deltaMove.x != 0)
            {
                deltaMove.x = deltaMove.x / Math.Abs(deltaMove.x);
            }
            if (deltaMove.y != 0)
            {
                deltaMove.y = deltaMove.y / Math.Abs(deltaMove.y);
            }

            //It randomly decides whether to try to move on the X-axis or Y-axis.
            bool moveOnX = rand.Next() % 2 == 0 && deltaMove.x != 0;
            deltaMove.x = moveOnX ? deltaMove.x : 0;
            deltaMove.y = moveOnX ? 0 : deltaMove.y;

            Move(deltaMove.x, deltaMove.y);
        }

        /// <summary>
        /// This method randomly move the enemy.
        /// </summary>
        private void RandomMove(Action<int, int> Move)
        {
            Random rand = new Random();
            int dx = 0, dy = 0;

            //The enemy can randomly move in 5 ways:left, up, right, down, and not move.
            switch (rand.Next() % 5)
            {
                case 0:
                    dx = 1;
                    break;
                case 1:
                    dx = -1;
                    break;
                case 2:
                    dy = 1;
                    break;
                case 3:
                    dy = -1;
                    break;
                default:
                    break;
            }
            Move(dx, dy);
        }
    }
}
