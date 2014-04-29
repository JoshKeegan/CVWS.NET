/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Segmentation - class to hold the indices that split rows & cols
 * By Josh Keegan 02/04/2014
 * Last Edit 29/04/2014
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
        //Protected vars (used for Wordsearch Recognition candidate scoring)
        protected int? numCols = null;
        protected int? numRows = null;

        protected int[,] rowStartEnds = null;
        protected int[,] colStartEnds = null;

        //Public vars
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int[] Rows { get; private set; } // The indices splitting rows/cols
        public int[] Cols { get; private set; }

        public int NumRows
        {
            get
            {
                return Rows.Length + 1;
            }
        }

        public int NumCols
        {
            get
            {
                return Cols.Length + 1;
            }
        }

        //Constructors
        protected Segmentation(Segmentation copy)
        {
            this.Width = copy.Width;
            this.Height = copy.Height;
            this.numCols = copy.numCols;
            this.numRows = copy.numRows;
            this.rowStartEnds = copy.rowStartEnds;
            this.colStartEnds = copy.colStartEnds;
            this.Rows = copy.Rows;
            this.Cols = copy.Cols;
        }

        public Segmentation(int[] rows, int[] cols, int width, int height)
        {
            //Validation: width & height must be >= 0
            if (width < 0 || height < 0)
            {
                throw new InvalidImageDimensionsException("Image dimensions must be positive");
            }

            //Validation: each row & col must be less than height & width respectively (and also positive)
            foreach(int row in rows)
            {
                if(row >= height || row < 0)
                {
                    throw new InvalidRowsAndColsException("Row indices must be positive and less than image height to be valid");
                }
            }
            foreach(int col in cols)
            {
                if(col >= width || col < 0)
                {
                    throw new InvalidRowsAndColsException("Col indices must be positive and less than image width to be valid");
                }
            }

            //Store the raw input to the constructor
            this.Width = width;
            this.Height = height;

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

            //Store the raw input to the constructor
            this.numRows = numRows;
            this.numCols = numCols;
            this.Width = width;
            this.Height = height;

            //Convert num. of rows & cols to incices
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
            //Validation: width & height must be >= 0
            if (width < 0 || height < 0)
            {
                throw new InvalidImageDimensionsException("Image dimensions must be positive");
            }

            //Validation: There must be at least 1 row & col
            if(rows.GetLength(0) < 1 || cols.GetLength(0) < 1)
            {
                throw new InvalidRowsAndColsException("Must be at least 1 Row and Col");
            }

            //Validation: row indices must be in range 0 <=  x < height
            foreach (int imgRowIdx in rows)
            {
                if (imgRowIdx >= height || imgRowIdx < 0)
                {
                    throw new InvalidRowsAndColsException("Row indices must be positive and less than image height to be valid");
                }
            }

            //Validation: col indices must be in range 0 <= x < width
            foreach(int imgColIdx in cols)
            {
                if(imgColIdx >= width || imgColIdx < 0)
                {
                    throw new InvalidRowsAndColsException("Col indices must be positive and less than image width to be valid");
                }
            }

            //Validation: rows/cols 2D arrays must be of size (x, 2)
            if(rows.GetLength(1) != 2 || cols.GetLength(1) != 2)
            {
                throw new UnexpectedArrayDimensionsException("Expected rows and cols arrays to be of size (x, 2)");
            }

            //Validation: row/col start indices must be less than their paired end index
            for(int i = 0; i < rows.GetLength(0); i++)
            {
                int start = rows[i, 0];
                int end = rows[i, 1];

                if(start >= end)
                {
                    throw new InvalidRowsAndColsException("Row start indices must be less than row end indices");
                }
            }
            for(int i = 0; i < cols.GetLength(0); i++)
            {
                int start = cols[i, 0];
                int end = cols[i, 1];

                if(start >= end)
                {
                    throw new InvalidRowsAndColsException("Col start indices must be less than col end indices");
                }
            }

            //Validation: Rows/Cols must be ordered & cannot overlap
            int prevEnd = 0;
            for(int i = 0; i < rows.GetLength(0); i++)
            {
                int start = rows[i, 0];

                if(start < prevEnd)
                {
                    throw new InvalidRowsAndColsException("Rows cannot overlap and must be ordered");
                }

                //Update prev end for next iter
                prevEnd = rows[i, 1];
            }
            prevEnd = 0;
            for(int i = 0; i < cols.GetLength(0); i++)
            {
                int start = cols[i, 0];

                if(start < prevEnd)
                {
                    throw new InvalidRowsAndColsException("Cols cannot overlap and must be ordered");
                }

                //Update prevEnd for next iter
                prevEnd = cols[i, 1];
            }

            //Store the raw input to the constructor
            this.rowStartEnds = rows;
            this.colStartEnds = cols;
            this.Width = width;
            this.Height = height;

            //Convert row/col start & end indices into indices splitting them by finding the mid-point between the end of one & start of the next
            int[] rowSplits = new int[rows.GetLength(0) - 1];
            int[] colSplits = new int[cols.GetLength(0) - 1];

            //Rows
            prevEnd = rows[0, 1];
            for(int i = 1; i < rows.GetLength(0); i++)
            {
                int start = rows[i, 0];

                //The split index will be the average of the previous rows end and this ones start
                rowSplits[i - 1] = (prevEnd + start) / 2;

                //Update prevEnd for next iter
                prevEnd = rows[i, 1];
            }

            //Cols
            prevEnd = cols[0, 1];
            for(int i = 1; i < cols.GetLength(0); i++)
            {
                int start = cols[i, 0];

                //The split index will be the average of the previous cols end and this ones start
                colSplits[i - 1] = (prevEnd + start) / 2;

                //Update prevEnd for next iter
                prevEnd = cols[i, 1];
            }

            this.Rows = rowSplits;
            this.Cols = colSplits;
        }

        //Public Methods
        public void Rotate90()
        {
            //When rotating 90 deg, the col order is maintained, but row order gets reversed:
            int[] reversedRows = new int[Rows.Length];

            for(int i = 0; i < Rows.Length; i++)
            {
                reversedRows[reversedRows.Length - 1 - i] = Height - 1 - Rows[i]; //height - 1 because bitmaps are zero indexed
            }

            //Swap rows & cols
            Rows = Cols;
            Cols = reversedRows;

            //Swap width & height
            int intTemp = Width;
            Width = Height;
            Height = intTemp;

            //If we have the number of rows & number of cols stored explicitly, swap them
            if(numRows != null && numCols != null)
            {
                int? nullIntTemp = numRows;
                numRows = numCols;
                numCols = nullIntTemp;
            }

            //If the have the row & col start & end points stored, swap them
            if(rowStartEnds != null && colStartEnds != null)
            {
                //When rotating 90 deg, the col order is maintained, but row order gets reversed:
                int[,] reversedRowStartEnds = new int[rowStartEnds.GetLength(0), rowStartEnds.GetLength(1)];

                for(int i = 0; i < rowStartEnds.GetLength(0); i++)
                {
                    //Swap the start & end
                    reversedRowStartEnds[reversedRowStartEnds.GetLength(0) - 1 - i, 0] = Height - 1 - rowStartEnds[i, 1];
                    reversedRowStartEnds[reversedRowStartEnds.GetLength(0) - 1 - i, 1] = Height = 1 - rowStartEnds[i, 0];
                }

                //Swap rows & cols
                rowStartEnds = colStartEnds;
                colStartEnds = reversedRowStartEnds;
            }
        }

        public void Rotate(int angleDeg)
        {
            //Validation: Check that we've been asked to rotate through a multiple of 90 deg
            if (angleDeg % 90 != 0)
            {
                throw new ArgumentException("angleDeg must be a multiple of 90");
            }

            int numRotations = (angleDeg % 360) / 90; //Modulo 360 to prevent unnecessary rotations for angles of more than 360 deg
            for(int i = 0; i < numRotations; i++)
            {
                Rotate90();
            }
        }

        public Segmentation DeepCopy()
        {
            //Deep copy of object, but shallow for object values
            Segmentation copy = new Segmentation(this);

            //Deep copy object values
            copy.Rows = (int[])copy.Rows.Clone();
            copy.Cols = (int[])copy.Cols.Clone();

            if(copy.rowStartEnds != null)
            {
                copy.rowStartEnds = (int[,])copy.rowStartEnds.Clone();
            }
            if(copy.colStartEnds != null)
            {
                copy.colStartEnds = (int[,])copy.colStartEnds.Clone();
            }
           

            return copy;
        }
    }
}
