using System;
using Xunit;
using Ecs;
using System.Collections.Generic;

namespace XUnitTestProject
{
    public class TransformTest
    {
        [Fact]
        public void SetParentOfChildAndHasParent()
        {
            Transform child = new Transform();
            Transform parent = new Transform();
            child.SetParent(parent);
            Assert.True(child.Parent == parent);

        }

        [Fact]
        public void TranslateByTwoAddsTwoToPosition()
        {
            Transform transform = new Transform();
            transform.position = new Vec2i(0, 0);
            transform.Translate(2, 2);
            Assert.True(transform.position.x == 2 && transform.position.y == 2);
        }

        [Fact]
        public void TranslateByTwoAddsTwoToPositionAndChild()
        {
            Transform child = new Transform();
            Transform parent = new Transform();
            child.position = new Vec2i(2, 2);
            parent.position = new Vec2i(4, 4);
            child.SetParent(parent);

            parent.Translate(2, 2);

            Assert.True(child.position.x == 4 && child.position.y == 4);
            Assert.True(parent.position.x == 6 && parent.position.y == 6);
        }


        [Fact]
        public void TransformIterateOverChildrenByEnumeratorIterator()
        {
            Transform child1 = new Transform();
            Transform child2 = new Transform();
            Transform child3 = new Transform();
            Transform parent = new Transform();
            child1.SetParent(parent);
            child2.SetParent(parent);
            child3.SetParent(parent);
            int i = 0;
            foreach (Transform transform in parent)
            {
                ++i;
            }
            Assert.True(i == 3);
        }

        [Fact]
        public void SetParentToParent()
        {
            Transform child1 = new Transform();
            Transform child2 = new Transform();
            Transform parent = new Transform();
            child1.SetParent(parent);
            child2.SetParent(parent);
            parent.SetParent(parent);

            int i = 0;
            foreach (Transform transform in parent)
            {
                ++i;
            }
            Assert.True(i == 2);
        }

        [Fact]
        public void SetParentMultipleChildrenOneIsSame()
        {
            Transform child1 = new Transform();
            Transform child2 = new Transform();
            Transform parent = new Transform();
            child1.SetParent(parent);
            child2.SetParent(parent);
            child2.SetParent(parent);

            int i = 0;
            foreach (Transform transform in parent)
            {
                ++i;
            }
            Assert.True(i == 2);
        }
    }
}

