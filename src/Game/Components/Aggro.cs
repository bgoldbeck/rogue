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

        private bool CheckLine(Vec2i targeterLocation, Vec2i targetLocation)
        {
            Map map = Map.CacheInstance();
            bool hitObject = false;

            if (map == null)
            {
                Debug.LogWarning("Map not found.");
                return false;
            }

            if(Math.Abs(targetLocation.x - targeterLocation.x) > Math.Abs(targetLocation.y - targeterLocation.y))
            {
                double slopeY = (targetLocation.y - targeterLocation.y + 0.0) / (targetLocation.x - targeterLocation.x);
                double currentY = slopeY;
                int slopeX = (targeterLocation.x < targetLocation.x) ? 1 : -1;
                int currentX = slopeX;

                while (targeterLocation.x + currentX != targetLocation.x && !hitObject)
                {
                    if (map.PeekObject(targeterLocation.x + currentX, targeterLocation.y + (int)Math.Round(currentY, 0)) != null)
                    {
                        hitObject = true;
                    }
                    currentY += slopeY;
                    currentX += slopeX;
                }
            }
            else
            {
                double slopeX = (targetLocation.x - targeterLocation.x + 0.0) / (targetLocation.y - targeterLocation.y);
                double currentX = slopeX;
                int slopeY = (targeterLocation.y < targetLocation.y) ? 1 : -1;
                int currentY = slopeY;

                while (targeterLocation.y + currentY != targetLocation.y && !hitObject)
                {
                    if (map.PeekObject(targeterLocation.x + (int)Math.Round(currentX,0), targeterLocation.y + currentY) != null)
                    {
                        hitObject = true;
                    }
                    currentY += slopeY;
                    currentX += slopeX;
                }
            }

            return !hitObject;
        }
    }
}
