//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;

namespace Ecs
{
    /// <summary>
    /// Class for storing a location (x, y) coordinate.
    /// </summary>
    public class Vec2i
    {
        public readonly int x = 0;
        public readonly int y = 0;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Vec2i()
        {
        }

        /// <summary>
        /// Constructor to create a new Vec2i(x, y)
        /// </summary>
        /// <param name="x">The x location.</param>
        /// <param name="y">The y location</param>
        public Vec2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        /// <summary>
        /// Create a new Veci from another Vec2i instance.
        /// </summary>
        /// <param name="referenceToVector">The other Vec2i instance to copy.</param>
        public Vec2i(Vec2i referenceToVector)
        {
            if(referenceToVector == null)
            {
                throw new ArgumentNullException("Null Vec2i passed as parameter into the constructor.");
            }
            x = referenceToVector.x;
            y = referenceToVector.y;
        }

        /// <summary>
        /// Add two Vec2i objects together using linear algebra.
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>A new vector representing the sum of vectors (a+b)</returns>
        public static Vec2i operator +(Vec2i a, Vec2i b)
        {
            if (a == null || b == null)
            {
                throw new ArgumentNullException("Attempted addition operator on null Vec2i.");
            }
            return new Vec2i(a.x + b.x, a.y + b.y);
        }

        /// <summary> 
        /// Subtract two Vec2i objects together using linear algebra.
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>A new vector representing the difference of vectors (a-b)</returns>
        public static Vec2i operator -(Vec2i a, Vec2i b)
        {
            if (a == null || b == null)
            {
                throw new ArgumentNullException("Attempted subtraction operator on null Vec2i.");
            }
            return new Vec2i(a.x - b.x, a.y - b.y);
        }

        /// <summary>
        /// Determine if two Vec2i objects are equivalent.
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True, if this object equals obj</returns>
        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            Vec2i v = (Vec2i)obj;
            return (x == v.x) && (y == v.y);
        }

        /// <summary>
        /// Determine if two Vec2i objects are equivalent.
        /// </summary>
        /// <param name="a">The first Vec2i object.</param>
        /// <param name="b">The second Vec2i object.</param>
        /// <returns>True, if a equal b</returns>
        public static bool operator ==(Vec2i a, Vec2i b)
        {
            bool isEqual;
            if ((object)a == null)
            {
                isEqual = ((object)b == null);
            }
            else
            {
                isEqual = a.Equals(b);
            }
            return isEqual;
        }

        /// <summary>
        /// Determine if two Vec2i objets are not equivalent.
        /// </summary>
        /// <param name="a">The first Vec2i object.</param>
        /// <param name="b">The second Vec2i object.</param>
        /// <returns>True, if a does not equal b</returns>
        public static bool operator !=(Vec2i a, Vec2i b)
        {
            return !(a == b);
        }

        /// <summary>
        /// The hash code for this object.
        /// </summary>
        /// <returns>The hashcode for this object.</returns>
        public override int GetHashCode()
        {
            return x ^ y;
        }

        /// <summary>
        /// Determine the distance between two Vec2i objects.
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The absolute distance between to vectors</returns>
        public static double Distance(Vec2i a, Vec2i b)
        {
            if (a == null || b == null)
            {
                throw new ArgumentNullException("Attempted to calculate the distance using a null Vec2i.");
            }
            return Math.Sqrt(Math.Pow((double)(b.x - a.x), 2.0) + Math.Pow((double)(b.y - a.y), 2.0));
        }

        /// <summary>
        /// See https://en.wikipedia.org/wiki/Taxicab_geometry
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The manhattan distance between two vectors</returns>
        public static int Heuristic(Vec2i a, Vec2i b)
        {
            if (a == null || b == null)
            {
                throw new ArgumentNullException("Attempted to calculate the heuristic using a null Vec2i.");
            }
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }
      
    }

}
