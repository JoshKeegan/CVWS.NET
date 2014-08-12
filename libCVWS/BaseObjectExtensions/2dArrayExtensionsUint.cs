/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS Base Object Extensions
 * partial 2D Array Extensions for Uint
 * By Josh Keegan 03/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.BaseObjectExtensions
{
    public static partial class _2dArrayExtensions
    {
        public static int[,] ToIntArr(this uint[,] uintArr)
        {
            int[,] intArr = new int[uintArr.GetLength(0), uintArr.GetLength(1)];

            for(int i = 0; i < uintArr.GetLength(0); i++)
            {
                for(int j = 0; j < uintArr.GetLength(1); j++)
                {
                    intArr[i, j] = checked((int)uintArr[i, j]); //throws OverflowException if val > Int32.MaxValue
                }
            }
            return intArr;
        }
    }
}
