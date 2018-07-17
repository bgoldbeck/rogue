//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using Xunit;
using Ecs;
using System.Collections.Generic;

namespace XUnitTestProject
{
    public class Vec2iTest
    {
        [Theory]
        [InlineData(1, 2, 3, 4)]
        [InlineData(4, 3, 2, 1)]
        [InlineData(-4, -3, -2, -1)]
        [InlineData(-1, -2, -3, -4)]
        public void AddTwoVectors(int ax, int ay, int bx, int by)
        {
            Vec2i a = new Vec2i(ax, ay);
            Vec2i b = new Vec2i(bx, by);
            Vec2i sum = a + b;
            Assert.True(sum.x == ax + bx && sum.y == ay + by);
        }

        [Theory]
        [InlineData(1, 2, 3, 4)]
        [InlineData(4, 3, 2, 1)]
        [InlineData(-4, -3, -2, -1)]
        [InlineData(-1, -2, -3, -4)]
        public void SubtractTwoVectors(int ax, int ay, int bx, int by)
        {
            Vec2i a = new Vec2i(ax, ay);
            Vec2i b = new Vec2i(bx, by);
            Vec2i difference = a - b;
            Assert.True(difference.x == ax - bx && difference.y == ay - by);
        }

        [Fact]
        public void TwoEqualVectorsAreEqual()
        {
            Vec2i a = new Vec2i(-45, 40);
            Vec2i b = new Vec2i(-45, 40);
            Assert.True(a == b && b == a);
            Assert.True(a.Equals(b) && b.Equals(a));
        }


        [Fact]
        public void TwoNotEqualVectorsAreNotEqual()
        {
            Vec2i a = new Vec2i(-40, 40);
            Vec2i b = new Vec2i(40, -40);
            Assert.True(a != b && b != a);
        }
    }
}

