/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Segmentation - class to hold the indices that split rows & cols
 * By Josh Keegan 02/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.ImageAnalysis.WordsearchSegmentation
{
    public class Segmentation
    {
        //Public vars
        public int[] Rows { get; private set; } // The indices splitting rows/cols
        public int[] Cols { get; private set; }

        //Constructors
        public Segmentation(int[] rows, int[] cols)
        {
            this.Cols = cols;
            this.Rows = rows;
        }

        //Construct from number of rows & cols
        public Segmentation(int rows, int cols, int width, int height)
        {
            //TODO: Comvert rows & cols to incices splitting them
        }

        //Construct from the start & end indices of each row & col
        public Segmentation(int[,] rows, int[,] cols, int width, int height)
        {
            //TODO: Convert row/col start & end indices into indices splitting them by finding the mid-point between the end of one & start of the next
        }
    }
}
