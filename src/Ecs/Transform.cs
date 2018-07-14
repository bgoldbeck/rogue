//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecs
{
    public class Transform : Component, IEnumerable
    {

        public Transform Parent { get; private set; } = null;

        private List<Transform> children = new List<Transform>();
        public Vec2i position = new Vec2i();

        // Private enumerator class mostly referenced from 
        // https://support.microsoft.com/en-us/help/322022/how-to-make-a-visual-c-class-usable-in-a-foreach-statement
        private class InnerEnumerator : IEnumerator
        {
            public Transform[] children = new Transform[] { };
            public int position = -1;

            
            public InnerEnumerator(Transform[] list)
            {
                children = list;
            }

            private IEnumerator GetEnumerator()
            {
                return (IEnumerator)this;
            }

            // IEnumerator
            public bool MoveNext()
            {
                position++;
                return (position < children.Length);
            }

            // IEnumerator
            public void Reset()
            {
                position = -1;
            }

            // IEnumerator
            public object Current
            {
                get
                {
                    try
                    {
                        return children[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }  // end nested class
        
        public void SetParent(Transform transform)
        {
            this.Parent = transform;
            this.Parent.children.Add(this);
            return;
        }

        public void Translate(int dx, int dy)
        {
            TranslateOnChildren(this, dx, dy);
            return;
        }

        private void TranslateOnChildren(Transform root, int dx, int dy)
        {
            if (root == null) { return; }

            foreach (Transform transform in root.children)
            {
                TranslateOnChildren(transform, dx, dy);
            }

            root.position = new Vec2i(root.position.x + dx, root.position.y + dy);
            return;
        }

        public IEnumerator GetEnumerator()
        {
            return new InnerEnumerator(children.ToArray());
        }
    }
    
}
