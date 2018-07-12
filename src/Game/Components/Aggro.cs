using System;
using System.Collections.Generic;
using System.Text;

using Ecs;
using IO;

namespace Game.Components
{
    class Aggro :Component
    {
        private int aggroRange = 10;
        private int lastSeenPlayer = 0;
        private int turnsTillEnemyGivesUp = 3;

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


        public void TargetSearch()
        {
            Enemy searchee = (Enemy)base.gameObject.GetComponent<Enemy>();
            bool targetUpdated = false;

            if(searchee == null)
            {
                Debug.LogError("Aggo isn't a component of an enemy object.");
                return;
            }
            //If the enemy is close enough to the player, it saves the location it has seen the
            //player at and set the boolean that is has seen the player.
            Player player = Player.MainPlayer();

            if (Vec2i.Distance(player.transform.position, transform.position) < aggroRange)
            {
                if (CheckLine(transform.position, player.transform.position))
                {
                    searchee.target = player.transform;
                    targetUpdated = true;
                }
            }
            //If the enemy can't see the player but has seen the player before. It checks how long
            //since the last time it has seen the player. If it has been too long, it sets the boolean
            //to false.
            if (searchee.target != null && !targetUpdated)
            {
                if (++lastSeenPlayer > turnsTillEnemyGivesUp)
                {
                    searchee.target = null;
                }
            }
            return;
        }

        private bool CheckLine(Vec2i playerLocation, Vec2i enemyLocation)
        {
            Map map = Map.CacheInstance();
            if (map == null)
            {
                Debug.LogWarning("Map not found.");
                return false;
            }
            if (enemyLocation.x == playerLocation.x)
            {
                int slopeY = (playerLocation.y < enemyLocation.y) ? 1 : -1;
                int currentY = slopeY;
                while (playerLocation.y + currentY != enemyLocation.y)
                {
                    if (map.PeekObject(enemyLocation.x, playerLocation.y + currentY) != null)
                    {
                        return false;
                    }
                    currentY += slopeY;
                }
                return true;
            }
            else
            {
                double slopeY = (enemyLocation.y - playerLocation.y + 0.0) / (enemyLocation.x - playerLocation.x);
                double currentY = slopeY;
                int slopeX = (playerLocation.x < enemyLocation.x) ? 1 : -1;
                int currentX = slopeX;

                while (playerLocation.x + currentX != enemyLocation.x)
                {
                    if (map.PeekObject(playerLocation.x + currentX, playerLocation.y + (int)Math.Round(currentY, 0)) != null)
                    {
                        return false;
                    }
                    currentY += slopeY;
                    currentX += slopeX;
                }
                return true;
            }
        }
    }
}
