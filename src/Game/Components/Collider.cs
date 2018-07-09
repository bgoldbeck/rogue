//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using IO;
using Game.Interfaces;

namespace Game.Components
{
    public class Collider : Component
    {    
        /// <summary>
         /// Used by the Collider component to return whether an object collided with a
         /// wall, object, or nothing at all.
         /// </summary>
        public enum CollisionTypes { None, Wall, ActiveObject };

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

        /// <summary>
        /// Uses an inputted movement and uses the current position and the movement to check
        /// if there location being moved into is empty or not. If there is already a game object
        /// at that spot, it will also return a reference to that game object.
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="found"></param>
        /// <returns></returns>
        public CollisionTypes HandleCollision(int dx, int dy, out GameObject found)
        {
            found = null;

            // Finds the map that is stored in the global GameObject.
            Map map = Map.CacheInstance();
            if (map != null)
            {            
                // Checks if the map cell is open.
                found = map.PeekObject(this.transform.position.x + dx, this.transform.position.y + dy);
            }
            else
            {
                Debug.LogError("Map game object wasn't found.");
            }
            // Return either no collision or some active object.
            return found == null ? CollisionTypes.None : CollisionTypes.ActiveObject;
        }
    }
}
