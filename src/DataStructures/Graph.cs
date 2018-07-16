using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class Graph <T>
    {
        private Dictionary<T, List<T>> Data { get; set; } = new Dictionary<T, List<T>>();

        public void AddEdge(T from, T to)
        {
            Data.TryGetValue(from, out List<T> neighbors);
            
            if (neighbors == null)
            {
                neighbors = new List<T>();
            }
            neighbors.Add(to);
            Data[from] = neighbors;
            
            return;
        }

        public void RemoveEdge(T from, T remove)
        {
            if (Data.TryGetValue(from, out List<T> neighbors))
            {
                neighbors.Remove(remove);

                Data[from] = neighbors;
            }
            return;
        }

        public List<T> GetEdges(T from)
        {
            Data.TryGetValue(from, out List<T> neighbors);
            return neighbors;
        }

        public int GetEdgeCount(T from)
        {
            Data.TryGetValue(from, out List<T> neighbors);

            return neighbors != null ? neighbors.Count : 0;
        }

        public int VertexCount()
        {
            return Data.Count;
        }

    }
}
