//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

//The algorithm for A* pathfinding came from this website:
//https://www.redblobgames.com/pathfinding/a-star/introduction.html

using System;
using System.Collections.Generic;
using System.Text;

using Ecs;
using DataStructures;
using IO;

namespace Game.Components
{
    public class NavigatorAgent : Component
    {
        public List<Vec2i> targetPath { get; } = new List<Vec2i>();
        private Map map = null;
        public Transform Target
        {
            get;
            set;
        }

        public override void Start()
        {
            map = MapManager.CurrentMap();
            return;
        }


        public override void LateUpdate()
        {
            targetPath.Clear();
            if (Target == null) { return; }

            Graph<Vec2i> graph;
            if ((graph = MapManager.CurrentNavigationMap().CacheInstance()) == null) { return; }

            Vec2i goal = Target.position;

            if (Vec2i.Heuristic(goal, transform.position) == 1)
            {
                targetPath.Add(goal);
                return;
            }

            Vec2i current = null;

            Vec2i start = this.transform.position;

            PriorityQueue<Vec2i> frontier = new PriorityQueue<Vec2i>();
            frontier.Enqueue(start, 0.0);
            
            Dictionary<Vec2i, Vec2i> cameFrom = new Dictionary<Vec2i, Vec2i>();
            Dictionary<Vec2i, double> costSoFar = new Dictionary<Vec2i, double>(); // Movement Costs.
            cameFrom[start] = null;
            costSoFar[start] = 0; // Movement Costs.

            while (frontier.Count() != 0)
            {
                current = frontier.Dequeue();

                if (current == goal) { break; }

                if (graph.GetEdges(current) != null)
                {
                    foreach (Vec2i next in graph.GetEdges(current))
                    {
                        if (next == null) { continue; }
                        double newCost = costSoFar[current] + 1; // Movement Costs.

                        //If the cost is greater then 40, then the path is too long already and it continues.
                        if(newCost > 40)
                        {
                            continue;
                        }

                        GameObject nextGo = map.PeekObject(next);
                        if (nextGo == null || next == goal || nextGo.GetComponent<Door>() != null)
                        {
                            if (!cameFrom.ContainsValue(next) || newCost < costSoFar[next])
                            {
                                costSoFar[next] = newCost;    // Movement Costs.

                                double priority = newCost + Vec2i.Heuristic(goal, next);

                                frontier.Enqueue(next, priority);
                                cameFrom[next] = current;
                            }
                        }
                    }
                }
            }


            bool isPathGood = true;
       

            if (goal != null)
            { 
                current = goal;
                //List<Vec2i> path = new List<Vec2i>();
                while (current != start)
                {
                    targetPath.Add(current);
                    
                    if (!cameFrom.ContainsKey(current))
                    {
                        isPathGood = false;
                        break;
                    }
                    else
                    { 
                        current = cameFrom[current];
                    }
                    if (current != start && current != goal && (map.PeekObject(current) != null && map.PeekObject(current).GetComponent<Door>() == null))
                    {
                        isPathGood = false;
                        break;
                    }
                }

                if(!isPathGood)
                {
                    targetPath.Clear();
                }

                /*if (isPathGood)
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
                          ConsoleUI.Write(x, y, ".", Color.Teal);
                    }
                }*/
            }
            return;
        }

    }
}
