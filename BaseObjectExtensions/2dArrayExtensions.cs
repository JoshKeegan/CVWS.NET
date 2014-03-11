﻿/*
 * Dissertation CV Wordsearch Solver
 * Base Object Extensions
 * 2D Array Extensions
 * By Josh Keegan 11/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseObjectExtensions
{
    public static class _2dArrayExtensions
    {
        public static T[] ToSingleDimension<T>(this T[,] arr2d)
        {
            T[] arr1d = new T[arr2d.Length];

            for(int i = 0; i < arr2d.GetLength(0); i++)
            {
                for(int j = 0; j < arr2d.GetLength(1); j++)
                {
                    arr1d[(j * arr2d.GetLength(0)) + i] = arr2d[i, j];
                }
            }
            return arr1d;
        }

        //TODO: Could implement optimised versions for some base types that get used by this. As in http://stackoverflow.com/a/797383
    }
}
