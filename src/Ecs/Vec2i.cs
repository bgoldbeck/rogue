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
        public int x = 0;
        public int y = 0;

        /// <summary>
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>A new vector representing the sum of vectors (a+b)</returns>
        public static Vec2i operator +(Vec2i a, Vec2i b)
        {
            Vec2i vec = new Vec2i
            {
                x = a.x + b.x,
                y = a.y + b.y
            };
            return vec;
        }

        /// <summary> 
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>A new vector representing the difference of vectors (a-b)</returns>
        public static Vec2i operator -(Vec2i a, Vec2i b)
        {
            Vec2i vec = new Vec2i
            {
                x = a.x - b.x,
                y = a.y - b.y
            };
            return vec;
        }

        /// <summary>
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The absolute distance between to vectors</returns>
        public static int Distance(Vec2i a, Vec2i b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

    }

}
