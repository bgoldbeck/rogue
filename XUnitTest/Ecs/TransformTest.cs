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


    }
}
