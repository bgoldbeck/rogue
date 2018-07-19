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

            return;
        }

        public override void Update()
        {
            graph = new Graph<Vec2i>();
            Map map = Map.CacheInstance();
            if(map != null)
            {
                // Building the graph from the map.
                for (int x = 0; x < map.width; ++x)
                {
                    for (int y = 0; y < map.height; ++y)
                    {
                        // TODO. need to block going off the maps edges.
                        //if (map.PeekObject(x,y) != null) { continue; }
                        Vec2i from = new Vec2i(x, y);
                        Vec2i[] neighbors = new Vec2i[]
                        {
                        new Vec2i(x + 1, y), new Vec2i(x - 1, y), new Vec2i(x, y + 1), new Vec2i(x, y - 1)
                        };
                        foreach (Vec2i currentNeighbor in neighbors)
                        {
                            if (map.PeekObject(currentNeighbor) == null || map.PeekObject(currentNeighbor).GetComponent<Door>() != null)
                            {
                                graph.AddEdge(from, currentNeighbor);
                            }
                        }
                    }
                }
            }
        }

        public static Graph<Vec2i> CacheInstance()
        {
            return graph;
        }

    }
}
