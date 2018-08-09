#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using System.Collections.Generic;

namespace DataStructures
{
    public class Graph <T>
    {
        private Dictionary<T, List<T>> Data { get; set; } = new Dictionary<T, List<T>>();


        public void AddEdge(T from, T to)
        {
            AddUndirectedEdge(from, to);
            AddUndirectedEdge(to, from);
            return;
        }

        private void AddUndirectedEdge(T from, T to)
        {
            AddVertex(from);
            Data.TryGetValue(from, out List<T> neighbors);
            
            neighbors.Add(to);
            Data[from] = neighbors;
            
            return;
        }

        public void AddVertex(T vertex)
        {
            if (!Data.ContainsKey(vertex))
            { 
                Data.Add(vertex, new List<T>());
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertex"></param>
        public void RemoveVertex(T vertex)
        {
            List<T> neighbors = GetEdges(vertex);

            if (neighbors != null)
            {
                foreach (T neighbor in neighbors)
                {
                    RemoveEdge(vertex, neighbor);
                }
                Data.Remove(vertex);
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void RemoveEdge(T from, T to)
        {
            RemoveDirectedEdge(from, to);
            RemoveDirectedEdge(to, from);
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="remove"></param>
        public void RemoveDirectedEdge(T from, T remove)
        {
            if (Data.TryGetValue(from, out List<T> neighbors))
            {
                neighbors.Remove(remove);
                
                Data.Remove(from);
                
                Data[from] = neighbors;
                
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public List<T> GetEdges(T from)
        {
            Data.TryGetValue(from, out List<T> neighbors);
            return neighbors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public int Degree(T vertex)
        {
            Data.TryGetValue(vertex, out List<T> neighbors);

            return neighbors != null ? neighbors.Count : 0;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int VertexCount()
        {
            return Data.Count;
        }

        public bool ContainsIsland()
        {
            bool found = false;
            foreach (T vertex in Data.Keys)
            {
                if(Data.TryGetValue(vertex, out List<T> neighbors))
                {
                    if (neighbors != null && neighbors.Count == 0)
                    {
                        found = true;
                        break;
                    }
                }
            }
            return found;
        }

    }
}
