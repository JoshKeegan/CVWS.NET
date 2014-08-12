/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS Base Object Extensions
 * partial Collection Extensions class - functions working on IDisposable
 * By Josh Keegan 11/03/2014
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
        //Dispose of all resources in a collection of disposable resources
        public static void DisposeAll(this ICollection<IDisposable> collection)
        {
            foreach(IDisposable d in collection)
            {
                d.Dispose();
            }
        }
    }
}
