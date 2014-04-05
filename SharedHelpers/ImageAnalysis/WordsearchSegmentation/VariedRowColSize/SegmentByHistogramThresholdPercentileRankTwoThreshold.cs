/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Segmentation Algorithm splitting using separate start & end thresholds determined by
 *  some percentile either side of a value selected from modelling the number of dark pixels
 *  per row/col as a Bimodal Histogram and finding a threshold for that
 * By Josh Keegan 05/04/2014
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
    public class SegmentByHistogramThresholdPercentileRankTwoThreshold : SegmentationAlgorithmByStartEndIndices, ISegmentationAlgorithmOnBoolArr
    {
        //Constants
        private const double PERCENTILE_EITHER_SIDE = 10;

        protected override void doSegment(System.Drawing.Bitmap image, out int[,] rows, out int[,] cols)
        {
            DoSegment(Converters.BitmapToBoolArray(image), out rows, out cols);
        }

        public void DoSegment(bool[,] image, out int[,] rows, out int[,] cols)
        {
            //Count the number of dark pixels per row and column
            uint[] colDarkPixelCounts = SegmentationAlgorithmHelpers.CountNumDarkPixelsPerCol(image);
            uint[] rowDarkPixelCounts = SegmentationAlgorithmHelpers.CountNumDarkPixelsPerRow(image);

            //Calculate the Histograms to select a threshold
            BimodalHistogram colHist = new BimodalHistogram(colDarkPixelCounts.ToDoubleArr());
            BimodalHistogram rowHist = new BimodalHistogram(rowDarkPixelCounts.ToDoubleArr());

            //Find the percentile rank corresponding to the threshold selected from the Histograms
            Percentile colPercentile = new Percentile(colDarkPixelCounts.ToDoubleArr());
            Percentile rowPercentile = new Percentile(rowDarkPixelCounts.ToDoubleArr());
            double colThresholdRank = colPercentile.CalculateRank(colHist.ThresholdValue);
            double rowThresholdRank = rowPercentile.CalculateRank(rowHist.ThresholdValue);

            //Look up the percentile values for the ranks x either side of the auto-selected ones
            double colStartRank = Math.Min(100, colThresholdRank + PERCENTILE_EITHER_SIDE);
            double colEndRank = Math.Max(0, colThresholdRank - PERCENTILE_EITHER_SIDE);
            double rowStartRank = Math.Min(100, rowThresholdRank + PERCENTILE_EITHER_SIDE);
            double rowEndRank = Math.Max(0, rowThresholdRank - PERCENTILE_EITHER_SIDE);

            double colStartVal = colPercentile.CalculatePercentile(colStartRank);
            double colEndVal = colPercentile.CalculatePercentile(colEndRank);
            double rowStartVal = rowPercentile.CalculatePercentile(rowStartRank);
            double rowEndVal = rowPercentile.CalculatePercentile(rowEndRank);

            //Determine the start & end indices of all the rows & cols, using the calculated start & end threshold vals
            uint[,] colChars = SegmentationAlgorithmHelpers.FindCharIndices(colDarkPixelCounts, colStartVal, colEndVal);
            uint[,] rowChars = SegmentationAlgorithmHelpers.FindCharIndices(rowDarkPixelCounts, rowStartVal, rowEndVal);

            rows = rowChars.ToIntArr();
            cols = colChars.ToIntArr();
        }
    }
}
