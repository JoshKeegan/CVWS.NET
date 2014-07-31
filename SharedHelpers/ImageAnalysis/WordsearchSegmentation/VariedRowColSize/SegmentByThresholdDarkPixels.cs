/*
 * Computer Vision Wordsearch Solver
 * Shared Helpers
 * Wordsearch Segmentation Algorithm splitting based on a preselected threshold 
 *  (from looking at a histogram of the no. of dark pixels per row and col)
 * By Josh Keegan 05/04/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedHelpers.ImageAnalysis.WordsearchSegmentation;
using SharedHelpers.Imaging;
using BaseObjectExtensions;

namespace SharedHelpers.ImageAnalysis.WordsearchSegmentation.VariedRowColSize
{
    public class SegmentByThresholdDarkPixels : SegmentationAlgorithmByStartEndIndices, ISegmentationAlgorithmOnBoolArr
    {
        private const double THRESHOLD_DIMENSION_PERCENTAGE = 7.5; //Selected by looking at a histogram of col pixel counts

        protected override void doSegment(Bitmap image, out int[,] rows, out int[,] cols)
        {
            DoSegment(Converters.BitmapToBoolArray(image), out rows, out cols);
        }

        public void DoSegment(bool[,] image, out int[,] rows, out int[,] cols)
        {
            //Count the number of dark pixels per row and column
            uint[] colDarkPixelCounts = SegmentationAlgorithmHelpers.CountNumDarkPixelsPerCol(image);
            uint[] rowDarkPixelCounts = SegmentationAlgorithmHelpers.CountNumDarkPixelsPerRow(image);

            //Determine the start & end indices of all the rows & cols, using the mean as the threshold to determine entry & exit
            uint[,] colChars = SegmentationAlgorithmHelpers.FindCharIndices(colDarkPixelCounts, (THRESHOLD_DIMENSION_PERCENTAGE / 100) * rowDarkPixelCounts.Length);
            uint[,] rowChars = SegmentationAlgorithmHelpers.FindCharIndices(rowDarkPixelCounts, (THRESHOLD_DIMENSION_PERCENTAGE / 100) * colDarkPixelCounts.Length);

            rows = rowChars.ToIntArr();
            cols = colChars.ToIntArr();
        }
    }
}
