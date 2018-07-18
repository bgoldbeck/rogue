//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using Xunit;
using DataStructures;
using Ecs;
using System.Collections.Generic;

namespace XUnitTests.DataStructures
{
    public class PriorityQueueTest
    {
        [Fact]
        public void AddIncrementingValues_DequeueSmallestIsSmallest()
        {
            PriorityQueue<string> queue = new PriorityQueue<string>();

            queue.Enqueue("World", 1.0);
            queue.Enqueue("Hello", -40.0);
            queue.Enqueue("Hello", -40.0);
            queue.Enqueue("!", 2.0);

            Assert.True(queue.Dequeue() == "Hello" && queue.Count() == 2);
        }

        [Fact]
        public void PeekDoesntRemoveItem()
        {
            PriorityQueue<string> queue = new PriorityQueue<string>();

            queue.Enqueue("!", 2.0);
            queue.Enqueue("Hello", 0.0);
            queue.Enqueue("World", 1.0);

            Assert.True(queue.Peek() == "Hello" && queue.Count() == 3);
        }

        [Fact]
        public void DequeueNoItemsReturnsNull()
        {
            PriorityQueue<string> queue = new PriorityQueue<string>();

            queue.Dequeue();

            Assert.True(queue.Peek() == null);
        }

        [Fact]
        public void TestEcs()
        {
            PriorityQueue<Vec2i> queue = new PriorityQueue<Vec2i>();

            queue.Enqueue(new Vec2i(), 0.0);

            Assert.True(queue.Peek() != null);
        }
    }
}
