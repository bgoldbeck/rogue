//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using Ecs;
using IO;

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
            Map map = MapManager.CurrentMap();
            if (map != null)
            {
                if (this.transform != null)
                {
                    // Checks if the map cell is open.
                    found = map.PeekObject(this.transform.position.x + dx, this.transform.position.y + dy);
                }
                else
                {
                    Debug.LogError("Game object is null.");
                }
            }
            else
            {
                Debug.LogError("Map game object wasn't found.");
            }
            // Return either no collision or some active object.
            if (found == null)
                return CollisionTypes.None;
            if (found.GetComponent<Wall>() != null)
                return CollisionTypes.Wall;
            return CollisionTypes.ActiveObject;
        }
    }
}
