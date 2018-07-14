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
            Assert.True(child.parent == parent);

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

    }
}
