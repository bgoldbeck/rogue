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

        public bool IsInteractable
        {
            get
            {
                return !locked;
            }
        }

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

        public void OnInteract(GameObject objectInteracting)
        {
            // This line makes only actors with the DoorOpener component can open doors.
            if (objectInteracting != null && objectInteracting.GetComponent<DoorOpener>() == null) { return; }

            bool destroyDoor = false;
            if (locked && objectInteracting.GetComponent<Player>() != null)
            {
                Inventory inv = (Inventory)Player.MainPlayer().GetComponent<Inventory>();
                if (inv.Find("Key") != null)
                {
                    inv.Remove("Key");
                    Map.CacheInstance().PopObject(transform.position.x, transform.position.y);
                    HUD.Append(objectInteracting.Name + " unlocked and opened a door.");
                    destroyDoor = true;
                }
                else
                {
                    HUD.Append(objectInteracting.Name + " tried to open door, but is was locked.");
                    Console.Beep(80, 100);
                }
            }
            else
            {
                Map.CacheInstance().PopObject(transform.position.x, transform.position.y);
                if (objectInteracting.GetComponent<Player>() != null)
                {
                    HUD.Append(objectInteracting.Name + " opened a door.");
                    destroyDoor = true;
                }
            }

            if (destroyDoor)
            {
                GameObject.Destroy(gameObject);
                Console.Beep(100, 100);
            }
        }
        
    }
}
