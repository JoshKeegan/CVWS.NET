/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Segmentation - class to hold the indices that split rows & cols
 * By Josh Keegan 02/04/2014
 * Last Edit 03/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedHelpers.Exceptions;

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
        public Segmentation(int numRows, int numCols, int width, int height)
        {
            //Validation: numRows & numCols must be >= 1
            if(numRows < 1 || numCols < 1)
            {
                throw new InvalidRowsAndColsException("Rows and cols must be >= 1");
            }
            //Validation: width & height must be >= 0
            if(width < 0 || height < 0)
            {
                throw new InvalidImageDimensionsException("Image dimensions must be positive");
            }

            //Comvert num. of rows & cols to incices
            //Cols
            double colWidth = (double)width / numCols;
            int[] cols = new int[numCols - 1];
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i] = (int)(colWidth * (i + 1));
            }

            //Rows
            double rowHeight = (double)height / numRows;
            int[] rows = new int[numRows - 1];
            for(int i = 0; i < rows.Length; i++)
            {
                rows[i] = (int)(rowHeight * (i + 1));
            }

            this.Rows = rows;
            this.Cols = cols;
        }

        //Construct from the start & end indices of each row & col
        public Segmentation(int[,] rows, int[,] cols, int width, int height)
        {
            //TODO: Convert row/col start & end indices into indices splitting them by finding the mid-point between the end of one & start of the next
        }
    }
}
