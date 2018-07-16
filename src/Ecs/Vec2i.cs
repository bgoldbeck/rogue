//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecs
{
    public class Vec2i
    {
        public readonly int x = 0;
        public readonly int y = 0;

        public Vec2i()
        {
        }

        public Vec2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public Vec2i(Vec2i referenceToVector)
        {
            x = referenceToVector.x;
            y = referenceToVector.y;
        }

        /// <summary>
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>A new vector representing the sum of vectors (a+b)</returns>
        public static Vec2i operator +(Vec2i a, Vec2i b)
        {
            return new Vec2i(a.x + b.x, a.y + b.y);
        }

        /// <summary> 
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>A new vector representing the difference of vectors (a-b)</returns>
        public static Vec2i operator -(Vec2i a, Vec2i b)
        {
            return new Vec2i(a.x - b.x, a.y - b.y);
        }

        /// <summary>
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The absolute distance between to vectors</returns>
        public static int Distance(Vec2i a, Vec2i b)
        {
            return (int)Math.Sqrt(Math.Pow((double)(b.x - a.x), 2.0) + Math.Pow((double)(b.y - a.y), 2.0));
        }

        /// <summary>
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The manhattan distance between to vectors, 
        /// see https://en.wikipedia.org/wiki/Taxicab_geometry</returns>
        static public double Heuristic(Vec2i a, Vec2i b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

    }

}
