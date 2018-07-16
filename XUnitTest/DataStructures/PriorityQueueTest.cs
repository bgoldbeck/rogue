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

            queue.Enqueue("Hello", 0.0);
            queue.Enqueue("World", 1.0);
            queue.Enqueue("!", 2.0);

            Assert.True(queue.Dequeue() == "Hello" && queue.Count() == 2);
        }

        [Fact]
        public void PeekDoesntRemoveItem()
        {
            PriorityQueue<string> queue = new PriorityQueue<string>();

            queue.Enqueue("Hello", 0.0);
            queue.Enqueue("World", 1.0);
            queue.Enqueue("!", 2.0);

            Assert.True(queue.Peek() == "Hello" && queue.Count() == 3);
        }
    }
}
