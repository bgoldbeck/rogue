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

        [Fact]
        public void NullVectorAndNonNullVectorAreNotEqual()
        {
            Vec2i a = new Vec2i(-40, 40);
            Vec2i b = null;

            Assert.True(a != b && b != a);
        }

        [Fact]
        public void TwoNullVectorsAreEqual()
        {
            Vec2i a = null;
            Vec2i b = null;

            Assert.True(a == b && b == a);
        }

        [Fact]
        public void VectorCreatedFromCopyConstructorIsEqual()
        {
            Vec2i a = new Vec2i(-40, 40);
            Vec2i b = new Vec2i(a);

            Assert.True(a == b && b == a);
        }

        [Theory]
        [InlineData(1, 2, 3, 4)]
        [InlineData(4, 3, 2, 1)]
        [InlineData(-4, -3, -2, -1)]
        [InlineData(-1, -2, -3, -4)]
        public void DistanceIsCorrectlyCalculatedFromTwoVectors(int ax, int ay, int bx, int by)
        {
            Vec2i a = new Vec2i(ax, ay);
            Vec2i b = new Vec2i(bx, by);
            double distance = Math.Sqrt(Math.Pow((double)(bx - ax), 2.0) + Math.Pow((double)(by - ay), 2.0));

            Assert.True(Vec2i.Distance(a, b) == distance && Vec2i.Distance(b, a) == distance);
        }

        [Theory]
        [InlineData(1, 2, 3, 4)]
        [InlineData(4, 3, 2, 1)]
        [InlineData(-4, -3, -2, -1)]
        [InlineData(-1, -2, -3, -4)]
        public void HeuristicIsCorrectlyCalculatedFromTwoVectors(int ax, int ay, int bx, int by)
        {
            Vec2i a = new Vec2i(ax, ay);
            Vec2i b = new Vec2i(bx, by);
            double heuristic = Math.Abs(ax - bx) + Math.Abs(ay - by);

            Assert.True(Vec2i.Heuristic(a, b) == heuristic && Vec2i.Heuristic(b, a) == heuristic);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(4, 3)]
        [InlineData(-4, -3)]
        [InlineData(-1, -2)]
        public void HashCodeCorrectlyCreated(int ax, int ay)
        {
            Vec2i a = new Vec2i(ax, ay);
            int hashCode = ax ^ ay;

            Assert.True(a.GetHashCode() == hashCode);
        }

        [Fact]
        public void NullVectorInCopyConstructorCausesException()
        {
            Assert.Throws<ArgumentNullException>(() => new Vec2i(null));
        }

        [Fact]
        public void NullVectorInAdditionOperatorCausesException()
        {
            Vec2i a = new Vec2i(1, 2);
            Assert.Throws<ArgumentNullException>(() => a + null);
            Assert.Throws<ArgumentNullException>(() => null + a);
        }

        [Fact]
        public void NullVectorInSubtractionOperatorCausesException()
        {
            Vec2i a = new Vec2i(1, 2);
            Assert.Throws<ArgumentNullException>(() => a - null);
            Assert.Throws<ArgumentNullException>(() => null - a);
        }

        [Fact]
        public void NullVectorParameterInDistanceOperatorCausesException()
        {
            Vec2i a = new Vec2i(1, 2);
            Assert.Throws<ArgumentNullException>(() => Vec2i.Distance(a,null));
            Assert.Throws<ArgumentNullException>(() => Vec2i.Distance(null, a));
            Assert.Throws<ArgumentNullException>(() => Vec2i.Distance(null, null));
        }

        [Fact]
        public void NullVectorParameterInHeuristicOperatorCausesException()
        {
            Vec2i a = new Vec2i(1, 2);
            Assert.Throws<ArgumentNullException>(() => Vec2i.Heuristic(a, null));
            Assert.Throws<ArgumentNullException>(() => Vec2i.Heuristic(null, a));
            Assert.Throws<ArgumentNullException>(() => Vec2i.Heuristic(null, null));
        }
    }
}

