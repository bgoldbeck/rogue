using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class Graph <T>
    {
        public Dictionary<T, List<T>> Edges { get; private set; } = new Dictionary<T, List<T>>();

        public void AddEdge(T from, T to)
        {
            Edges.TryGetValue(from, out List<T> neighbors);
            
            if (neighbors == null)
            {
                neighbors = new List<T>();
            }
            neighbors.Add(to);
            Edges[from] = neighbors;
            
            return;
        }

        public void RemoveEdge(T from, T remove)
        {
            if (Edges.TryGetValue(from, out List<T> neighbors))
            {
                neighbors.Remove(remove);

                Edges[from] = neighbors;
            }
            return;
        }


    }
}
