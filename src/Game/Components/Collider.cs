using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using Game.Interfaces;
using Game.Components;
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

        public CollisionTypes handleCollision(int dx, int dy, GameObject found)
        {
            Map area = gameObject.GetComponent("Map");
            if(area.cellGrid[this.position.y + dy][this.position.x + dx] == Blocked)
            {
                return Wall;
            }
            if(area.objectGrid[this.position.y + dy][this.position.x + dx] != null)
            {
                found = area.objectGrid[this.position.y + dy][this.position.x + dx];
                return Object;
            }
            return None;
        }
    }
}
