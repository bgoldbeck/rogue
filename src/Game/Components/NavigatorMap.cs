using System;
using System.Collections.Generic;
using System.Text;

using DataStructures;
using Ecs;

namespace Game.Components
{
    class NavigatorMap : Component
    {
        private static Graph<Vec2i> graph = null;

        public override void Start()
        {

            graph = new Graph<Vec2i>();
            Map map = Map.CacheInstance();
            if (map != null)
            {
                // Building the graph from the map.
                for (int x = 0; x < map.width; ++x)
                {
                    for (int y = 0; y < map.height; ++y)
                    {
                        Vec2i from = new Vec2i(x, y);
                        AddNeighbors(from, map);
                    }
                }
            }
            return;
        }

        public override void Update()
        {
            /*graph = new Graph<Vec2i>();
            Map map = Map.CacheInstance();
            if (map != null)
            {
                // Building the graph from the map.
                for (int x = 0; x < map.width; ++x)
                {
                    for (int y = 0; y < map.height; ++y)
                    {
                        Vec2i from = new Vec2i(x, y);
                        AddNeighbors(from, map);
                    }
                }
            }*/
        }

        public static Graph<Vec2i> CacheInstance()
        {
            return graph;
        }

        public static void UpdatePositions(params Vec2i[] positionList)
        {
            Map map = Map.CacheInstance();
            if (graph == null || map == null)
            {
                return;
            }

            foreach (Vec2i current in positionList)
            {
                RemoveNeighbors(current);
                AddNeighbors(current, map);
            }
        }

        private static void AddNeighbors(Vec2i from, Map map)
        {
            if (graph == null)
            {
                return;
            }
            Vec2i[] neighbors = new Vec2i[]
            {
                        new Vec2i(from.x + 1, from.y), new Vec2i(from.x - 1, from.y), new Vec2i(from.x, from.y + 1), new Vec2i(from.x, from.y - 1)
            };
            foreach (Vec2i currentNeighbor in neighbors)
            {
                // TODO. need to block going off the maps edges.
                //if (map.PeekObject(x,y) != null) { continue; }
                if (map.PeekObject(currentNeighbor) == null || map.PeekObject(currentNeighbor).GetComponent<Door>() != null)
                {
                    graph.AddEdge(from, currentNeighbor);
                }
            }
        }
        private static void RemoveNeighbors(Vec2i from)
        {
            if (graph == null)
            {
                return;
            }
            List<Vec2i> neighbors = graph.GetEdges(from);

            //if(neighbors.Count)
            while(neighbors.Count > 0)
            {
                graph.RemoveEdge(from, neighbors[0]);
            }
        }

        public static void RemoveObject(Vec2i from)
        {
            Map map = Map.CacheInstance();
            if (graph == null || map == null || from == null)
            {
                return;
            }

            AddNeighbors(from, map);
        }
    }
}
