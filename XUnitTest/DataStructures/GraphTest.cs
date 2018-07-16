using System;
using Xunit;
using DataStructures;
using Ecs;
using System.Collections.Generic;

namespace XUnitTests.DataStructures
{
    public class GraphTest
    {
        [Fact]
        public void AddThreeEdges_EdgeCountThree()
        {
            Graph<Vec2i> graph = new Graph<Vec2i>();
            Vec2i a = new Vec2i(0, 0);
            Vec2i b = new Vec2i(1, 1);
            Vec2i c = new Vec2i(2, 2);
            Vec2i d = new Vec2i(3, 3);

            graph.AddEdge(a, b);
            graph.AddEdge(a, c);
            graph.AddEdge(a, d);

            graph.Edges.TryGetValue(a, out List<Vec2i> neighbors);

            Assert.True(neighbors != null && neighbors.Count == 3);
        }

        [Fact]
        public void AddThreeEdgesRemoveTwo_EdgeCountOne()
        {
            Graph<Vec2i> graph = new Graph<Vec2i>();
            Vec2i a = new Vec2i(0, 0);
            Vec2i b = new Vec2i(1, 1);
            Vec2i c = new Vec2i(2, 2);
            Vec2i d = new Vec2i(3, 3);

            graph.AddEdge(a, b);
            graph.AddEdge(a, c);
            graph.AddEdge(a, d);

            graph.RemoveEdge(a, c);
            graph.RemoveEdge(a, d);
            graph.Edges.TryGetValue(a, out List<Vec2i> neighbors);
            

            // Also assert the last edge is 'b'.
            Assert.True(neighbors != null && neighbors.Count == 1 && 
                neighbors[0] == b && 
                neighbors[0] != c && neighbors[0] != d);
        }
    }
}
