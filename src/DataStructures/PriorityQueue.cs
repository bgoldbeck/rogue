using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T>
    {
        private SortedSet<Tuple<double, T>> data = new SortedSet<Tuple<double, T>>();

        public void Enqueue(T item, double priority)
        {
            data.Add(Tuple.Create(priority, item));
            return;
        }

        public T Peek()
        {
            Tuple<double, T> min = data.Min;
            return min.Item2;
        }

        public T Dequeue()
        {
            Tuple<double, T> min = data.Min;

            data.Remove(min);

            return min.Item2;
        }

        public int Count()
        {
            return data.Count;
        }
    }
}
