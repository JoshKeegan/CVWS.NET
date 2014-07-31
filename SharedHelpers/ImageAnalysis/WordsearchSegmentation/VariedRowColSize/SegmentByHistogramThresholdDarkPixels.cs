/*
 * Computer Vision Wordsearch Solver
 * Shared Helpers
 * Wordsearch Segmentation Algorithm splitting based on a threshold determined by
 *  assuming the dark pixels per row/col will form a Bimodal Histogram (as they 
 *  should due to the monospaced fonts used, and line spacing equal to the font width)
 *  and then finding the threshold that best splits that Histogram in 2 and using it
 *  to locate rows & cols
 * By Josh Keegan 04/04/2014
 * Last Edit 05/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedHelpers.ImageAnalysis.WordsearchSegmentation;
using SharedHelpers.Imaging;
using SharedHelpers.Maths.Statistics;
using BaseObjectExtensions;

namespace SharedHelpers.ImageAnalysis.WordsearchSegmentation.VariedRowColSize
{
    public class SegmentByHistogramThresholdDarkPixels : SegmentationAlgorithmByStartEndIndices, ISegmentationAlgorithmOnBoolArr
    {
        //Constants
        private const int HISTOGRAM_NUM_BINS = 20;

        protected override void doSegment(System.Drawing.Bitmap image, out int[,] rows, out int[,] cols)
        {
            DoSegment(Converters.BitmapToBoolArray(image), out rows, out cols);
        }

        public void DoSegment(bool[,] image, out int[,] rows, out int[,] cols)
        {
            //Count the number of dark pixels per row and column
            uint[] colDarkPixelCounts = SegmentationAlgorithmHelpers.CountNumDarkPixelsPerCol(image);
            uint[] rowDarkPixelCounts = SegmentationAlgorithmHelpers.CountNumDarkPixelsPerRow(image);

            //Calculate the thresholds to use
            BimodalHistogram colHist = new BimodalHistogram(colDarkPixelCounts.ToDoubleArr(), HISTOGRAM_NUM_BINS);
            BimodalHistogram rowHist = new BimodalHistogram(rowDarkPixelCounts.ToDoubleArr(), HISTOGRAM_NUM_BINS);

            //Determine the start & end indices of all the rows & cols
            uint[,] colChars = SegmentationAlgorithmHelpers.FindCharIndices(colDarkPixelCounts, colHist.ThresholdValue);
            uint[,] rowChars = SegmentationAlgorithmHelpers.FindCharIndices(rowDarkPixelCounts, rowHist.ThresholdValue);

            rows = rowChars.ToIntArr();
            cols = colChars.ToIntArr();
        }
    }
}
