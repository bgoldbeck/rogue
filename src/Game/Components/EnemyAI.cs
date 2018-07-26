//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck
using System;
using System.Collections.Generic;

using Ecs;
using IO;

namespace Game.Components
{
    class EnemyAI : Component
    {
        private float secondsBetweenMoves = 2.0f;
        private float secondsSinceLastMove = 0;

        public void SetRate(float rate)
        {
            secondsBetweenMoves = rate;
        }
        

        public override void Update()
        {
            DebugDrawPath();
            return;
        }

        //Comment this out to test the TimerTest.
        //-------------------------------------
        public override void LateUpdate()
        {
            secondsSinceLastMove += ((float)Time.deltaMs / 1000.0f);

            if (secondsBetweenMoves < secondsSinceLastMove)
            {
                Think();
               secondsSinceLastMove = 0;
            }
            return;
        }

        private void DebugDrawPath()
        {
            Player player = Player.MainPlayer();
            if (player == null)
            {
                return;
            }
            //Displays the path of the path finder.
#if DEBUG
            NavigatorAgent navigator = (NavigatorAgent)this.gameObject.GetComponent<NavigatorAgent>();
            if (navigator != null)
            {
                List<Vec2i> path = navigator.targetPath;
                if (path != null)
                {
                    Camera camera = Camera.MainCamera();
                    if (camera == null)
                    {
                        return;
                    }
                    int halfWidth = camera.width / 2;
                    int halfHeight = camera.height / 2;

                    int playerX = player.transform.position.x;
                    int playerY = player.transform.position.y;

                    foreach (Vec2i v in path)
                    {
                        int x = v.x - playerX + halfWidth;
                        int y = v.y - playerY + halfHeight;
                        if (x < camera.width && y < camera.height)
                        {
                            ConsoleUI.Write(x, y, ".", new Color(255, 0, 255));
                        }
                    }
                }
            }
#endif
            return;
        }

        public void Think()
        {
            Enemy puppet = (Enemy)base.gameObject.GetComponent<Enemy>();
            NavigatorAgent navigation = (NavigatorAgent)base.gameObject.GetComponent<NavigatorAgent>();
            if (navigation == null)
            {
                Debug.LogError("EnemyAI component needs an navigator agent object");
                return;
            }
            if (puppet == null)
            {
                Debug.LogError("EnemyAI component needs to be a component of an enemy");
                return;
            }
            List<Vec2i> path = navigation.targetPath;
            if (path != null && path.Count != 0)
            {
                SeekMove(path, puppet.OnMove);
            }
            else
            {
                RandomMove(puppet.OnMove);
            }
            
            return;
        }

        /// <summary>
        /// This method moves the enemy towards a known player.
        /// </summary>
        private void SeekMove(List<Vec2i> path, Action<int,int> Move)
        {
                if (path != null && path.Count != 0)
                {
                    Vec2i deltaMove = path[path.Count - 1] - transform.position;
                    Move(deltaMove.x, deltaMove.y);
                }
            /*Random rand = new Random();

            //Figures out which direction on the it has to move to head towards the player.
            Vec2i deltaMove = target.position - transform.position;
            int dx = deltaMove.x;
            int dy = deltaMove.y;

            if (deltaMove.x != 0)
            {
                dx = deltaMove.x / Math.Abs(deltaMove.x);
            }
            if (deltaMove.y != 0)
            {
                dy = deltaMove.y / Math.Abs(deltaMove.y);
            }

            //It randomly decides whether to try to move on the X-axis or Y-axis.
            bool moveOnX = deltaMove.y == 0 ||(rand.Next() % 2 == 0 && deltaMove.x != 0);
            dx = moveOnX ? dx : 0;
            dy = moveOnX ? 0 : dy;

            //HUD.Append(dx + " " + dy);
            Move(dx, dy);*/
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
