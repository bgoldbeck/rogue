#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

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
        private float secondsSinceLastSeen = 0;  //How many seconds since the aggro has seen the player
        private float secondsBeforeGivesUp = 3.0f; //How many seconds before it forgets it has seen the player.
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

            if (player != null && Vec2i.Distance(player.transform.position, transform.position) < aggroRange)
            {
                if ( (searchee is IXRayVision && ((IXRayVision)searchee).HasVision) || CheckLine(transform.position, player.transform.position))
                {
                    // OnAggro.
                    gameObject.SendMessage<IAggressive>("OnAggro", new object[] { player.gameObject });
                    //searchee.target = player.transform;
                    targetUpdated = true;
                    seenTarget = true;
                    secondsSinceLastSeen = 0;
                }
            }
            //If the enemy can't see the player but has seen the player before. It checks how long
            //since the last time it has seen the player. If it has been too long, it sets the boolean
            //to false.
            if (seenTarget && !targetUpdated)
            {
                secondsSinceLastSeen += ((float)Time.deltaMs / 1000.0f);
                if (secondsSinceLastSeen > secondsBeforeGivesUp)
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
            Map map = MapManager.CurrentMap();
            bool hitObject = false;

            if (map == null)
            {
                Debug.LogWarning("Map not found.");
                return false;
            }
            
            if(targeterLocation == targetLocation)
            {
                return true;
            }

            double slopeY = 0;
            double slopeX = 0;
            double currentX = 0;
            double currentY = 0;
            /*If the distance from the target on the X-axis is larger then the Y-axis, is uses
            //the X-axis as the indepent variable and generates the slope of Y in terms of the
            //change in X. It then increments a block at a time towards the target checking at each increment
            //if there is an object blocking the line of sight.*/
            if (Math.Abs(targetLocation.x - targeterLocation.x) > Math.Abs(targetLocation.y - targeterLocation.y))
            {
                slopeX = (targeterLocation.x < targetLocation.x) ? 1 : -1;
                slopeY = (targetLocation.y - targeterLocation.y + 0.0) / (targetLocation.x - targeterLocation.x) * slopeX;
            }
            //Does the same thing as above but the Y-Axis is the indepent variable.
            else
            {
                slopeY = (targeterLocation.y < targetLocation.y) ? 1 : -1;
                slopeX = (targetLocation.x - targeterLocation.x + 0.0) / (targetLocation.y - targeterLocation.y) * slopeY;
            }
            currentX = slopeX;
            currentY = slopeY;

            while (((targeterLocation.x + (int)Math.Round(currentX, 0) != targetLocation.x)
                || (targeterLocation.y + (int)Math.Round(currentY) != targetLocation.y)) && !hitObject)
            {
                if (map.PeekObject(targeterLocation.x + (int)Math.Round(currentX, 0), targeterLocation.y + (int)Math.Round(currentY, 0)) != null)
                {
                    hitObject = true;
                }
                currentY += slopeY;
                currentX += slopeX;
            }
            return !hitObject;
        }

        public void SetAggroRange(int newRange)
        {
            aggroRange = newRange;
        }
        public void SetAggroPatience(float newPatience)
        {
            secondsBeforeGivesUp = newPatience;
        }
    }
}
