/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Shasred Helpers
 * Wordsearch Segmentation Algorithm splitting by extracting character sized blobs from an Image of a wordsearch
 *  and then estimating row & col positions from the blob positions & sizes
 * By Josh Keegan 04/04/2014
 * Last Edit 22/04/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedHelpers.Imaging;
using BaseObjectExtensions;

using AForge.Imaging;
using AForge.Imaging.Filters;

namespace SharedHelpers.ImageAnalysis.WordsearchSegmentation.VariedRowColSize
{
    public class SegmentByBlobRecognition : SegmentationAlgorithmByStartEndIndices
    {
        //Constants
        private const double BLOB_FILTER_MIN_DIMENSION_IMAGE_PERCENTAGE = 1; //i.e. a blob must be at least 1% of the width of the image & at most 15%
        private const double BLOB_FILTER_MAX_DIMENSION_IMAGE_PERCENTAGE = 15;

        private const double BLOB_FILTER_MEAN_MULTIPLIER_MINIMUM = 0.75;
        private const double BLOB_FILTER_MEAN_MULTIPLIER_MAXIMUM = 1.25;

        protected override void doSegment(Bitmap image, out int[,] rows, out int[,] cols)
        {
            //Adaptively threshold the image to give us a binary result
            Bitmap thresholded = FilterCombinations.AdaptiveThreshold(image);

            //Invert the image, to give white foreground and black background (as is required by the AForge.NET Blob Detection system)
            Invert invertFilter = new Invert();
            invertFilter.ApplyInPlace(thresholded);

            //Perform Blob Recognition
            Blob[] blobs = blobRecognition(thresholded);

            //Get character blobs (filter out the non-characters)
            List<Blob> blobChars = filterBlobs(blobs);

            //Determine the row & col positions from the char blobs
            findRowsAndCols(blobChars, out rows, out cols);

            //Clean up
            thresholded.Dispose();
        }

        private static Blob[] blobRecognition(Bitmap thresholdedInverted)
        {
            //Use the AForge.NET BlobCounter to perform Blob Recognition / Connected Region Analysis
            BlobCounter blobRecognition = new BlobCounter();

            //Filter out blobs that are abviously the wrong size to be characters
            blobRecognition.MinWidth = (int)(thresholdedInverted.Width * (BLOB_FILTER_MIN_DIMENSION_IMAGE_PERCENTAGE / 100));
            blobRecognition.MinHeight = (int)(thresholdedInverted.Height * (BLOB_FILTER_MIN_DIMENSION_IMAGE_PERCENTAGE / 100));
            blobRecognition.MaxWidth = (int)(thresholdedInverted.Width * (BLOB_FILTER_MAX_DIMENSION_IMAGE_PERCENTAGE / 100));
            blobRecognition.MaxHeight = (int)(thresholdedInverted.Height * (BLOB_FILTER_MAX_DIMENSION_IMAGE_PERCENTAGE / 100));
            blobRecognition.FilterBlobs = true;

            blobRecognition.ProcessImage(thresholdedInverted);
            Blob[] blobs = blobRecognition.GetObjectsInformation();

            return blobs;
        }

        //Filter out non-character blobs
        private static List<Blob> filterBlobs(Blob[] blobs)
        {
            uint[] widths = new uint[blobs.Length];
            uint[] heights = new uint[blobs.Length];

            for(int i = 0; i < blobs.Length; i++)
            {
                widths[i] = checked((uint)blobs[i].Rectangle.Width); //Throws OverflowException if val < 0
                heights[i] = checked((uint)blobs[i].Rectangle.Height);
            }

            //Filter out blobs that are too big or too small
            //Only remove blobs that are too big or too small in both dimensions, to allow for thin chars like I's
            double meanWidth = widths.Mean();
            double minWidth = meanWidth * BLOB_FILTER_MEAN_MULTIPLIER_MINIMUM;
            double maxWidth = meanWidth * BLOB_FILTER_MEAN_MULTIPLIER_MAXIMUM;
            double meanHeight = heights.Mean();
            double minHeight = meanHeight * BLOB_FILTER_MEAN_MULTIPLIER_MINIMUM;
            double maxHeight = meanHeight * BLOB_FILTER_MEAN_MULTIPLIER_MAXIMUM;

            List<Blob> chars = new List<Blob>();
            foreach(Blob b in blobs)
            {
                int width = b.Rectangle.Width;
                int height = b.Rectangle.Height;

                //Is this Blob allowable
                if((width <= maxWidth && width >= minWidth) || //Width check
                    (height <= maxHeight && height >= minHeight)) //Height check
                {
                    chars.Add(b);
                }
            }
            return chars;
        }

        private static void findRowsAndCols(List<Blob> blobs, out int[,] rows, out int[,] cols)
        {
            //Handle the case where no Blobs were found
            if(blobs.Count == 0)
            {
                rows = new int[0, 0];
                cols = new int[0, 0];
                return;
            }

            //Find cols
            //Sort blobs left to right (by left edge co-ord)
            Blob[] sorted = blobs.OrderBy(b => b.Rectangle.X).ToArray();

            //Now find cols in the sorted blobs by looking for overlapping blobs
            List<Tuple<int, int>> liCols = new List<Tuple<int, int>>();
            int colStart = sorted[0].Rectangle.X;
            int colEnd = sorted[0].Rectangle.X + sorted[0].Rectangle.Width - 1; //-1 because start & end indices used for segmentation should be inclusive & the width makes it exclusive of the end index
            for(int i = 1; i < sorted.Length; i++)
            {
                int start = sorted[i].Rectangle.X;
                int end = start + sorted[i].Rectangle.Width - 1; //-1 because start & end indices used for segmentation should be inclusive & the width makes it exclusive of the end index

                //If we have reached the start of a new col
                if(start > colEnd)
                {
                    //Add the current col to list of cols
                    liCols.Add(Tuple.Create(colStart, colEnd));

                    //Set the column start & end values to that of this character
                    colStart = start;
                    colEnd = end;
                }
                else //Otherwise we're still in the same col
                {
                    //Update the column end index if this character goes further to the right
                    if(end > colEnd)
                    {
                        colEnd = end;
                    }
                }
            }
            //We'll have finished in a column, add this col to the found cols
            liCols.Add(Tuple.Create(colStart, colEnd));


            //Convert list of tuples to 2D array
            cols = new int[liCols.Count, 2];
            for(int i = 0; i < liCols.Count; i++)
            {
                cols[i, 0] = liCols[i].Item1;
                cols[i, 1] = liCols[i].Item2;
            }

            //Find rows
            //Sort blobs top to bottom (by top edge co-ord)
            sorted = blobs.OrderBy(b => b.Rectangle.Y).ToArray();

            //Now find rows in the sorted blobs by looking for overlapping blobs
            List<Tuple<int, int>> liRows = new List<Tuple<int, int>>();
            int rowStart = sorted[0].Rectangle.Y;
            int rowEnd = sorted[0].Rectangle.Y + sorted[0].Rectangle.Height - 1; //-1 because start & end indices used for segmentation should be inclusive & the width makes it exclusive of the end index
            for(int i = 1; i < sorted.Length; i++)
            {
                int start = sorted[i].Rectangle.Y;
                int end = start + sorted[i].Rectangle.Height - 1; //-1 because start & end indices used for segmentation should be inclusive & the width makes it exclusive of the end index

                //If we have reached the start of a new row
                if(start > rowEnd)
                {
                    //Add the current row to list of rows
                    liRows.Add(Tuple.Create(rowStart, rowEnd));

                    //Set the row start & end values to that of this character
                    rowStart = start;
                    rowEnd = end;
                }
                else //Otherwise we're still in the same row
                {
                    //Update the row end index if this character goes further down the image
                    if(end > rowEnd)
                    {
                        colEnd = end;
                    }
                }
            }
            //We'll ahve finished in a row, add this row to the found rows
            liRows.Add(Tuple.Create(rowStart, rowEnd));

            //Convert list of tuples to 2D array
            rows = new int[liRows.Count, 2];
            for(int i = 0; i < liRows.Count; i++)
            {
                rows[i, 0] = liRows[i].Item1;
                rows[i, 1] = liRows[i].Item2;
            }
        }
    }
}
