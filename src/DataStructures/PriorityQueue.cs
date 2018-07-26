//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T>
    {
        //private SortedSet<Tuple<Double, T>> data = new SortedSet<Tuple<Double, T>>();
        private List<double> priorities = new List<double>();
        private List<T> values = new List<T>();

        //private SortedSet<KeyValuePair<Double, >> data = new SortedSet<KeyValuePair<T, Double>>();

        public void Enqueue(T item, double priority)
        {
            if (values.Contains(item)) { return; }
            priorities.Add(priority);
            values.Add(item);
            return;
        }

        public T Peek()
        {
            T min = default(T);
            if (values == null || values.Count == 0) { return min; }
            return values.ElementAt(priorities.IndexOf(priorities.Min()));
        }

        public T Dequeue()
        {
            T min = Peek();
            if (min != null)
            { 
                int index = values.IndexOf(min);
                priorities.RemoveAt(index);
                values.RemoveAt(index);
            }
            return min;
        }

        public int Count()
        {
            return values.Count;
        }
    }
}
