#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;

namespace Ecs
{
    /// <summary>
    /// A Transform is a Component that will attach to every GameObject. It stores
    /// the location in the window and keeps track of children Transforms. This allows
    /// for a hierarchy of GameObject's like a tree.
    /// </summary>
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
        
        /// <summary>
        /// Set the parent of this Transform.
        /// </summary>
        /// <param name="parent">The parent to set this Transform to.</param>
        public void SetParent(Transform parent)
        {
            // Make a child reference for easier readability.
            Transform child = this;

            // Don't allow parent and child to be the same reference.
            if (parent == child) { return; }
            

            child.Parent = parent;

            // Don't allow the parent to have multiple of the same child.
            if (!parent.children.Contains(child))
            {
                parent.children.Add(child);
            }
            return;
        }

        /// <summary>
        /// Move this Transform.
        /// </summary>
        /// <param name="dx">Amount to move in x.</param>
        /// <param name="dy">Amount to move in y.</param>
        public void Translate(int dx, int dy)
        {
            TranslateOnChildren(this, dx, dy);
            return;
        }

        /// <summary>
        /// Move this Transform and all its children helper function.
        /// </summary>
        /// <param name="root">The root Transform to move.</param>
        /// <param name="dx">Amount to move in x.</param>
        /// <param name="dy">Amount to move in y.</param>
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

        /// <summary>
        /// Count the number of children this Transform has.
        /// </summary>
        /// <returns>
        /// The number of children on this Transform.
        /// </returns>
        public int ChildCount()
        {
            return children.Count;
        }

        public IEnumerator GetEnumerator()
        {
            return new InnerEnumerator(children.ToArray());
        }
        public void DestroyParentReference()
        {
            if (this.Parent != null)
            {
                this.Parent.children.Remove(this);
            }
        }
    }
    
}
