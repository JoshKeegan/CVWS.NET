/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Segmentation Algorithm splitting using separate start & end thresholds based on percentile values of dark pixels per row/col
 * By Josh Keegan 03/04/2014
 * Last Edit 05/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedHelpers.Imaging;
using SharedHelpers.ImageAnalysis.WordsearchSegmentation;
using SharedHelpers.Maths.Statistics;
using BaseObjectExtensions;

namespace SharedHelpers.ImageAnalysis.WordsearchSegmentation.VariedRowColSize
{
    public class SegmentByPercentileTwoThresholds : SegmentationAlgorithmByStartEndIndices, ISegmentationAlgorithmOnBoolArr
    {
        //Constants
        private const double CHAR_START_THRESHOLD_PERCENTILE = 60; //TODO: Maybe have these as defaults and have a constructor that can override them??
        private const double CHAR_END_THRESHOLD_PERCENTILE = 40;

        protected override void doSegment(System.Drawing.Bitmap image, out int[,] rows, out int[,] cols)
        {
            DoSegment(Converters.BitmapToBoolArray(image), out rows, out cols);
        }

        public void DoSegment(bool[,] image, out int[,] rows, out int[,] cols)
        {
            //Count the number of dark pixels per row and column
            uint[] colDarkPixelCounts = SegmentationAlgorithmHelpers.CountNumDarkPixelsPerCol(image);
            uint[] rowDarkPixelCounts = SegmentationAlgorithmHelpers.CountNumDarkPixelsPerRow(image);

            //Calculate the percentile-based char start/end thresholds
            Percentile colPercentile = new Percentile(colDarkPixelCounts.ToDoubleArr());
            Percentile rowPercentile = new Percentile(rowDarkPixelCounts.ToDoubleArr());
            double colCharStartThreshold = colPercentile.CalculatePercentile(CHAR_START_THRESHOLD_PERCENTILE);
            double colCharEndThreshold = colPercentile.CalculatePercentile(CHAR_END_THRESHOLD_PERCENTILE);
            double rowCharStartThreshold = rowPercentile.CalculatePercentile(CHAR_START_THRESHOLD_PERCENTILE);
            double rowCharEndThreshold = rowPercentile.CalculatePercentile(CHAR_END_THRESHOLD_PERCENTILE);

            //Determine the start & end indices of all the rows & cols, using the percentile thresholds to determine entry & exit
            uint[,] colChars = SegmentationAlgorithmHelpers.FindCharIndices(colDarkPixelCounts, colCharStartThreshold, colCharEndThreshold);
            uint[,] rowChars = SegmentationAlgorithmHelpers.FindCharIndices(rowDarkPixelCounts, rowCharStartThreshold, rowCharEndThreshold);

            rows = rowChars.ToIntArr();
            cols = colChars.ToIntArr();
        }
    }
}
