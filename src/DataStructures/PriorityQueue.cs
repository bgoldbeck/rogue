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
            T min = default(T);
            if (data.Min != null)
            {
                min = data.Min.Item2;
            }
            return min;
        }

        public T Dequeue()
        {
            T min = Peek();

            if (min != null)
            { 
                data.Remove(data.Min);
            }
            return min;
        }

        public int Count()
        {
            return data.Count;
        }
    }
}
