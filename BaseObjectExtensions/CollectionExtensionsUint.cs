/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Base Object Extensions
 * partial Collection Extensions class - functions working on uints
 * By Josh Keegan 10/03/2014
 * Last Edit 05/04/2014
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
    }
}
