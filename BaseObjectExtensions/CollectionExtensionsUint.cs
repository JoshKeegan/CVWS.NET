/*
 * Dissertation CV Wordsearch Solver
 * Base Object Extensions
 * partial Collection Extensions class - functions working on uints
 * By Josh Keegan 10/03/2014
 * Last Edit 11/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseObjectExtensions
{
    public static partial class CollectionExtensions
    {
        //Function to calculate the mean of a Collection of numbers
        public static double Mean(this ICollection<uint> collection)
        {
            ulong sum = 0;
            foreach(uint x in collection)
            {
                sum += x;
            }
            return sum / (double)collection.Count;
        }

        //Stacks and Queues don't implement ICollection, so have seperate methods for them
        public static double Mean(this Queue<uint> queue)
        {
            return Mean(queue.ToArray());
        }

        public static double Mean(this Stack<uint> stack)
        {
            return Mean(stack.ToArray());
        }

        public static double Percentile(this uint[] arr, double percentile)
        {
            //Sort the array
            uint[] sorted = (uint[])arr.Clone();
            Array.Sort(sorted);

            //Use linear interpolation between indexes to return a better approximation of the percentile
            double realIndex = (percentile / 100) * (arr.Length - 1);
            int lowerIndex = (int)Math.Floor(realIndex);
            int upperIndex = (int)Math.Ceiling(realIndex);
            double pointBetweenIndexes = realIndex % 1;

            return linearlyInterpolate(sorted[lowerIndex], sorted[upperIndex], pointBetweenIndexes);
        }

        /*
         * Helpers
         */
        //Linearly interpolate between two values, given a point between them to find (in range 0..1)
        private static double linearlyInterpolate(double a, double b, double point)
        {
            //Let a be the small and b be the big number
            if (a > b)
            {
                //Swap the numbers
                double c = a;
                a = b;
                b = c;
            }

            double diff = b - a;

            double interVal = diff * point;
            return a + interVal;
        }
    }
}
