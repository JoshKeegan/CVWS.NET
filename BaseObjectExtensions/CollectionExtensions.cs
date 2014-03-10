/*
 * Dissertation CV Wordsearch Solver
 * Base Object Extensions
 * Collection Extensions
 * By Josh Keegan 10/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseObjectExtensions
{
    public static class CollectionExtensions
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
    }
}
