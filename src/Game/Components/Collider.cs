//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using IO;
using Game.Interfaces;
using Game.DataStructures;

namespace Game.Components
{
    class Collider : Component
    {
        public override void Start()
        {
            return;
        }

        public override void Update()
        {
            if (gameObject.GetComponent<Player>() != null)
            {

            }

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
            //Grabs the map that is stored in the global GameObject.
            GameObject test = GameObject.FindWithTag("Map");
            if (test == null)
            {
                Debug.LogError("Could not find 'Map' GameObject from the Collider.");
                return CollisionTypes.None;
            }

            //Grabs the map component in the game object.
            Map area = (Map)test.GetComponent(typeof(Map));
            if(area == null)
            {
                Debug.LogError("Map wasn't found.");
                return CollisionTypes.None;
            }

            //Checks if the map cell is open.
            GameObject go = area.PeekObject(this.transform.position.x + dx, this.transform.position.y + dy);
            if (go == null)
            {
                return CollisionTypes.None;
            }
            else  //If not, an object is present.
            {
                return CollisionTypes.ActiveObject;
            }
        }
    }
}
