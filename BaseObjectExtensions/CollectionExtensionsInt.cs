/*
 * Computer Vision Wordsearch Solver
 * Base Object Extensions
 * partial Collection Extensions class - functions working on ints
 * By Josh Keegan 22/04/2014
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
        public static double[] ToDoubleArr(this ICollection<int> collection)
        {
            double[] toRet = new double[collection.Count];

            int i = 0;
            foreach(int num in collection)
            {
                toRet[i] = (double)num;

                i++;
            }
            return toRet;
        }
    }
}
