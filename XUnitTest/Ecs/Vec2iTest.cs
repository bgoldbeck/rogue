using System;
using Xunit;
using Ecs;
using System.Collections.Generic;

namespace XUnitTestProject
{
    public class Vec2iTest
    {
        [Fact]
        public void AddTwoVectors()
        {
            Vec2i v1 = new Vec2i(1, 2);
            Vec2i v2 = new Vec2i(3, 4);
            Vec2i sum = v1 + v2;
            Assert.True(sum.x == 4 && sum.y == 6);
        }

        [Fact]
        public void SubtractTwoVectors()
        {
            Vec2i v1 = new Vec2i(6, 6);
            Vec2i v2 = new Vec2i(4, 4);
            Vec2i sum = v1 - v2;
            Assert.True(sum.x == 2 && sum.y == 2);
        }
    }
}

