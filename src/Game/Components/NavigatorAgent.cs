﻿using System;
using System.Collections.Generic;
using System.Text;

using Ecs;
using DataStructures;
using IO;

namespace Game.Components
{
    public class NavigatorAgent : Component
    {
        public override void Start()
        {

            return;
        }



        public override void LateUpdate()
        {
            Double defaultPriority = 0.0;

            Graph<Vec2i> graph = new Graph<Vec2i>();

            Map map = Map.CacheInstance();
            Vec2i current = null;

            // Building the graph from the map.
            for (int x = 0; x < map.width; ++x)
            {
                for (int y = 0; y < map.height; ++y)
                {
                    // TODO. need to block going off the maps edges.
                    //if (map.PeekObject(x,y) != null) { continue; }
                    Vec2i from = new Vec2i(x, y);

                    if (map.PeekObject(x + 1, y) == null || map.PeekObject(x + 1, y).GetComponent<Door>() != null)
                    {
                        graph.AddEdge(from, new Vec2i(x + 1, y));
                    }
                    if (map.PeekObject(x - 1, y) == null || map.PeekObject(x - 1, y).GetComponent<Door>() != null)
                    {
                        graph.AddEdge(from, new Vec2i(x - 1, y));
                    }
                    if (map.PeekObject(x, y + 1) == null || map.PeekObject(x, y + 1).GetComponent<Door>() != null)
                    {
                        graph.AddEdge(from, new Vec2i(x, y + 1));
                    }
                    if (map.PeekObject(x, y - 1) == null || map.PeekObject(x, y - 1).GetComponent<Door>() != null)
                    {
                        graph.AddEdge(from, new Vec2i(x, y - 1));
                    }
                }
            }

            Vec2i goal = null;

            Vec2i start = this.transform.position;

            // Get the goal. For testing purposes. this navigator will set it's goal to a nearby enemy.
            for (int x = 0; x < map.width; ++x)
            {
                for (int y = 0; y < map.height; ++y)
                {
                    if (map.PeekObject(x, y) != null && map.PeekObject(x, y).GetComponent<Enemy>() != null)
                    {
                        if (Vec2i.Heuristic(new Vec2i(x, y), start) < 15)
                        {
                            goal = new Vec2i(x, y);
                            break;
                        }
                    }
                }
                if (goal != null) { break; }
            }

            //frontier = Queue()
            //frontier.put(start)
            //visited = { }
            //visited[start] = True

            //while not frontier.empty():
            //current = frontier.get()
            //for next in graph.neighbors(current):
            //if next not in visited:
            //frontier.put(next)
            //visited[next] = True
            PriorityQueue<Vec2i> frontier = new PriorityQueue<Vec2i>();
            frontier.Enqueue(start, defaultPriority);
            
            Dictionary<Vec2i, Vec2i> cameFrom = new Dictionary<Vec2i, Vec2i>();

            while (frontier.Count() != 0)
            {
                current = frontier.Dequeue();

                if (current == goal) { break; }

                if (graph.GetEdges(current) != null)
                {
                    foreach (Vec2i next in graph.GetEdges(current))
                    {
                        if (next == null) { continue; }

                        if (map.PeekObject(next.x, next.y) == null || next == goal)
                        {
                            if (!cameFrom.ContainsValue(next))
                            { 
                                frontier.Enqueue(next, 1.0);
                                cameFrom[next] = current;
                            }
                        }
                    }
                }
            }

            // To construct the path
            //current = goal
            //path = []
            //while current != start: 
            //path.append(current)
            //current = came_from[current]
            //path.append(start) # optional

            bool isPathGood = true;
       

            if (goal != null)
            { 
                current = goal;
                List<Vec2i> path = new List<Vec2i>();
                while (current != start)
                {
                    path.Add(current);
                    
                    if (!cameFrom.ContainsKey(current))
                    {
                        isPathGood = false;
                        break;
                    }
                    else
                    { 
                        current = cameFrom[current];
                    }
                    if (current != start && current != goal && (map.PeekObject(current.x, current.y) != null && map.PeekObject(current.x, current.y).GetComponent<Door>() == null))
                    {
                        isPathGood = false;
                        break;
                    }
                }
                path.Add(start); //path.append(start) # optional

                if (isPathGood)
                {
                    Camera camera = Camera.CacheInstance();
                    int halfWidth = camera.width / 2;
                    int halfHeight = camera.height / 2;

                    Player player = Player.MainPlayer();
                    int playerX = player.transform.position.x;
                    int playerY = player.transform.position.y;

                    foreach (Vec2i v in path)
                    {
                        int x = v.x - playerX + halfWidth;
                        int y = v.y - playerY + halfHeight;
                        if (x < camera.width && y < camera.height)
                          ConsoleUI.Write(x, y, ".", new Color(255, 0, 255));
                    }
                }
            }
            return;
        }
    }
}
