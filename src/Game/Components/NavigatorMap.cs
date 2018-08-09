#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using System.Collections.Generic;

using DataStructures;
using Ecs;

namespace Game.Components
{
    /// <summary>
    /// This class initializes and updates a graph of edges between the open blocks in the
    /// map.
    /// </summary>
    class NavigatorMap : Component
    {
        //The static graph of the current level.
        private Graph<Vec2i> graph = null;

        /// <summary>
        /// When this component starts, it uses the map of the level to fill in all the
        /// initial edges between the empty map squares.
        /// </summary>
        public override void Start()
        {
            graph = new Graph<Vec2i>();
            Map map = MapManager.CurrentMap();
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
            //The graph doesn't update itself.
        }

        /// <summary>
        /// Returns the static graph of the edges.
        /// </summary>
        /// <returns></returns>
        public Graph<Vec2i> CacheInstance()
        {
            return graph;
        }

        /// <summary>
        /// Takes in an array of positions that needs to be updated and updates them.
        /// </summary>
        /// <param name="positionList"></param>
        public void UpdatePositions(params Vec2i[] positionList)
        {
            Map map = MapManager.CurrentMap();
            if (graph == null || map == null)
            {
                return;
            }

            //To each update each position, all the edges related to that position are removed
            //and then readded back into the graph.
            foreach (Vec2i current in positionList)
            {
                RemoveNeighbors(current);
                AddNeighbors(current, map);
            }
        }

        /// <summary>
        /// Takes a position on the map and adds the edges the connect it to its neighbor
        /// locations.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="map"></param>
        private void AddNeighbors(Vec2i from, Map map)
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

        /// <summary>
        /// Takes a position on the map and removes the edges that connect it to its neighbor
        /// locations.
        /// </summary>
        /// <param name="from"></param>
        private void RemoveNeighbors(Vec2i from)
        {
            if (graph == null)
            {
                return;
            }
            List<Vec2i> neighbors = graph.GetEdges(from);

            if (neighbors != null)
            {
                while (neighbors.Count > 0)
                {
                    graph.RemoveEdge(from, neighbors[0]);
                }
            }
        }

        /// <summary>
        /// Takes a position and adds its neighbors to connect the now empty spot.
        /// </summary>
        /// <param name="from"></param>
        public void RemoveObject(Vec2i from)
        {
            Map map = MapManager.CurrentMap();
            if (graph == null || map == null || from == null)
            {
                return;
            }

            AddNeighbors(from, map);
        }
    }
}

