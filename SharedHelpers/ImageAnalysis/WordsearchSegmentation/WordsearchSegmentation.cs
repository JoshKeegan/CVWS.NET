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
    public class WordsearchSegmentation
    {
        //Public vars
        public int[] Rows { get; private set; } // The indices splitting rows/cols
        public int[] Cols { get; private set; }

        //Constructors
        public WordsearchSegmentation(int[] rows, int[] cols)
        {
            this.Cols = cols;
            this.Rows = rows;
        }

        //Constrct from number of rows & cols
        public WordsearchSegmentation(int rows, int cols)
        {
            //TODO: Comvert rows & cols to incices splitting them
        }

        //Construct from the start & end indices of each row & col
        public WordsearchSegmentation(int[,] rows, int[,] cols)
        {
            //TODO: Convert row/col start & end indices into indices splitting them by finding the mid-point between the end of one & start of the next
        }
    }
}
