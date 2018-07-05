using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using IO;
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
            Map area = (Map)this.gameObject.GetComponent(typeof(Map));
            if(area == null)
            {
                Debug.LogError("Map wasn't found.");
                return CollisionTypes.None;
            }

            if (area.GetCellState(this.transform.position.y + dy,this.transform.position.x + dx) == CellState.Blocked)
            {
                return CollisionTypes.Wall;
            }
            found = area.PeekObject(this.transform.position.y + dy,this.transform.position.x + dx);
            if(found != null)
            {
                return CollisionTypes.ActiveObject;
            }
            return CollisionTypes.None;
        }
    }
}
