/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Segmentation Algorithm splitting based on mean no. of dark pixels per row/col
 * By Josh Keegan 03/04/2014
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
    public class SegmentByMeanDarkPixels : SegmentationAlgorithmByStartEndIndices, ISegmentationAlgorithmOnBoolArr
    {
        protected override void doSegment(Bitmap image, out int[,] rows, out int[,] cols)
        {
            DoSegment(Converters.BitmapToBoolArray(image), out rows, out cols);
        }

        public void DoSegment(bool[,] image, out int[,] rows, out int[,] cols)
        {
            //Count the number of dark pixels per row and column
            uint[] colDarkPixelCounts = SegmentationAlgorithmHelpers.CountNumDarkPixelsPerCol(image);
            uint[] rowDarkPixelCounts = SegmentationAlgorithmHelpers.CountNumDarkPixelsPerRow(image);

            //Determine for each row/col of pixels whether you are in a character, using the mean as the threshold to determine entry & exit
            uint[,] colChars = SegmentationAlgorithmHelpers.FindCharIndices(colDarkPixelCounts, colDarkPixelCounts.Mean());
            uint[,] rowChars = SegmentationAlgorithmHelpers.FindCharIndices(rowDarkPixelCounts, rowDarkPixelCounts.Mean());

            rows = rowChars.ToIntArr();
            cols = colChars.ToIntArr();
        }
    }
}
