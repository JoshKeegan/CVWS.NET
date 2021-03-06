﻿/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS
 * Wordsearch Segmentation Algorithm splitting based on Median no. of dark pixels per row/col
 * By Josh Keegan 03/04/2014
 * Last Edit 12/08/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using libCVWS.ImageAnalysis.WordsearchSegmentation;
using libCVWS.Imaging;
using libCVWS.Maths.Statistics;
using libCVWS.BaseObjectExtensions;

namespace libCVWS.ImageAnalysis.WordsearchSegmentation.VariedRowColSize
{
    public class SegmentByMedianDarkPixels : SegmentationAlgorithmByStartEndIndices, ISegmentationAlgorithmOnBoolArr
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

            //Determine the start & end indices of all the rows & cols, using the mean as the threshold to determine entry & exit
            uint[,] colChars = SegmentationAlgorithmHelpers.FindCharIndices(colDarkPixelCounts, Stats.Median(colDarkPixelCounts));
            uint[,] rowChars = SegmentationAlgorithmHelpers.FindCharIndices(rowDarkPixelCounts, Stats.Median(rowDarkPixelCounts));

            rows = rowChars.ToIntArr();
            cols = colChars.ToIntArr();
        }
    }
}
