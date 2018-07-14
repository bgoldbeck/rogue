//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecs
{
    public class Transform : Component
    {
        public Transform parent = null;
        public List<Transform> children = new List<Transform>();
        public Vec2i position = new Vec2i();



        public void SetParent(Transform transform)
        {
            this.parent = transform;
            this.parent.children.Add(this);
            return;
        }

        public void Translate(int dx, int dy)
        {
            this.position = new Vec2i(this.position.x + dx, this.position.y + dy);
            return;
        }


    }
    
}
