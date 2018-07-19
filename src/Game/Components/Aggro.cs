//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;

using Ecs;
using Game.Interfaces;
using Game.Interfaces.Markers;
using IO;

namespace Game.Components
{
    class Aggro :Component
    {
        private bool seenTarget = false; //If a target has been seen.
        private int aggroRange = 10;     //The max range that is searched for a target.
        private int lastSeenPlayer = 0;  //How many turns since the last time the player has been seen.
        private int turnsTillEnemyGivesUp = 3; //How many turns before the target is forgotten.

        public override void Start()
        {
            base.Start();
            return;
        }

        public override void Update()
        {
            base.Update();

            TargetSearch();
            return;
        }


        /// <summary>
        /// Searches for a target player that is within range and is in the line of sight.
        /// </summary>
        public void TargetSearch()
        {
            Enemy searchee = (Enemy)base.gameObject.GetComponent<Enemy>();
            bool targetUpdated = false;

            if(searchee == null)
            {
                Debug.LogError("Aggro component needs an an enemy object.");
                return;
            }

            //If the enemy is close enough to the player, it saves the location it has seen the
            //player at and set the boolean that is has seen the player.
            Player player = Player.MainPlayer();

            if (Vec2i.Distance(player.transform.position, transform.position) < aggroRange)
            {
                if ( searchee is IXRayVision || CheckLine(transform.position, player.transform.position))
                {
                    // OnAggro.
                    gameObject.SendMessage<IAggressive>("OnAggro", new object[] { player.gameObject });
                    //searchee.target = player.transform;
                    targetUpdated = true;
                    seenTarget = true;
                }
            }
            //If the enemy can't see the player but has seen the player before. It checks how long
            //since the last time it has seen the player. If it has been too long, it sets the boolean
            //to false.
            if (seenTarget && !targetUpdated)
            {
                if (++lastSeenPlayer > turnsTillEnemyGivesUp)
                {
                    // OnDegggro
                    //searchee.target = null;
                    gameObject.SendMessage<IAggressive>("OnDeaggro");
                    seenTarget = false;
                }
            }
            return;
        }

        /// <summary>
        /// Checks if there exists a linear line between the target and this objects current position.
        /// </summary>
        /// <param name="targeterLocation"> The location of the targeter.</param>
        /// <param name="targetLocation"> The location of the target.</param>
        /// <returns></returns>
        private bool CheckLine(Vec2i targeterLocation, Vec2i targetLocation)
        {
            Map map = Map.CacheInstance();
            bool hitObject = false;

            if (map == null)
            {
                Debug.LogWarning("Map not found.");
                return false;
            }

            /*If the distance from the target on the X-axis is larger then the Y-axis, is uses
            //the X-axis as the indepent variable and generates the slope of Y in terms of the
            //change in X. It then increments a block at a time towards the target checking at each increment
            //if there is an object blocking the line of sight.*/
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
            //Does the same thing as above but the Y-Axis is the indepent variable.
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
