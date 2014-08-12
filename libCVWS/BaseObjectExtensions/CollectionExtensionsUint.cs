/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Base Object Extensions
 * partial Collection Extensions class - functions working on uints
 * By Josh Keegan 10/03/2014
 * Last Edit 12/08/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.BaseObjectExtensions
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
    }
}
