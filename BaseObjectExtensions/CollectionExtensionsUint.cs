/*
 * Dissertation CV Wordsearch Solver
 * Base Object Extensions
 * partial Collection Extensions class - functions working on uints
 * By Josh Keegan 10/03/2014
 * Last Edit 04/04/2014
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
        public static double[] ToDoubleArr(this ICollection<uint> collection)
        {
            double[] toRet = new double[collection.Count];

            int i = 0;
            foreach(uint num in collection)
            {
                toRet[i] = (double)num;

                i++;
            }
            return toRet;
        }

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

        public static double Median(this ICollection<uint> collection)
        {
            //Convert the collection to an array to be worked on
            uint[] arr = collection.ToArray();

            //Sort the array
            Array.Sort(arr);

            //If there is an odd number of elements, pick the middle one
            if(arr.Length % 2 == 1)
            {
                return arr[arr.Length / 2];
            }
            //Otherwise there is an even number of elements, return the mean of the middle 2
            else
            {
                uint a = arr[(arr.Length / 2) - 1];
                uint b = arr[arr.Length / 2];

                return (double)(a + b) / 2;
            }
        }

        public static double Percentile(this ICollection<uint> collection, double percentile)
        {
            //Validation
            if(collection.Count == 0)
            {
                throw new ArgumentOutOfRangeException("Collection must contain value(s)");
            }
            if(percentile < 0 || percentile > 100)
            {
                throw new ArgumentOutOfRangeException("Percentile must be in the range 0..100 to be valid");
            }

            //Convert the collection to an array to be worked on
            uint[] arr = collection.ToArray();

            //Sort the array
            Array.Sort(arr);

            //Determine the real (floating point) "index" to access.
            //explanation: Get position to lookup between 0..1, then multiply it by the number 
            //of elements in the array to get it into the range 0..arr.Length
            //This leaves the range being 1 larger than the array length, which is correct
            //(think of there being 4 values in an array and you trying to find the three quartile values,
            //the quartiles need to slot between the values rather than on the indices)
            //Finally, subtract 1/2 to slot the index into place (e.g. between the values in the quartiles example)
            double realIndex = ((percentile / 100) * arr.Length) - 0.5;

            //Keep the real index within valid array index bounds, the algorithm need not account for anything outside the known range of values
            realIndex = Math.Max(0, realIndex);
            realIndex = Math.Min(realIndex, arr.Length - 1);

            //Use linear interpolation between indices to return a better approximation of the percentile
            int lowerIndex = (int)Math.Floor(realIndex);
            int upperIndex = (int)Math.Ceiling(realIndex);
            double pointBetweenIndexes = realIndex % 1;

            return linearlyInterpolate(arr[lowerIndex], arr[upperIndex], pointBetweenIndexes);
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
