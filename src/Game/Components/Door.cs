//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using Game.Interfaces;

namespace Game.Components
{
    class Door : Component, IInteractable
    {
        private bool locked = false;

        public Door(bool locked = false)
        {
            this.locked = locked;
        }

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

        public void Interact(GameObject objectInteracting)
        {
            // This line makes only the player allowed to open doors.
            if (objectInteracting != null && objectInteracting.GetComponent<Player>() == null) { return; }

            // if (locked)
            // check if player has key before removing door.
            // else
            Map.CacheInstance().PopObject(transform.position.x, transform.position.y);

            /*
            if (locked && playerhasaccess)
            {
                HUD.Append(objectInteracting.Name + " unlocked and opened a door.");
            }
            else if (locked )
            {
                HUD.Append(objectInteracting.Name + " tried to open door, but is was locked.
                
            }
            else
            {
            }
            */

            HUD.Append(objectInteracting.Name + " opened a door.");
            GameObject.Destroy(gameObject);
        }
    }
}
