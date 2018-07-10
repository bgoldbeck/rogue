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

            GameObject go = GameObject.FindWithTag("Map");
            Map map = (Map)go.GetComponent(typeof(Map));
            map.PopObject(transform.position.x, transform.position.y);
            GameObject.Destroy(gameObject);
        }
    }
}
