﻿/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS
 * Wordsearch Segmentation Algorithm Returning Start And End Indices - abstract class
 * By Josh Keegan 02/04/2014
 * Last Edit 03/04/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using libCVWS.ImageAnalysis.WordsearchSegmentation;

namespace libCVWS.ImageAnalysis.WordsearchSegmentation.VariedRowColSize
{
    public abstract class SegmentationAlgorithmByStartEndIndices : SegmentationAlgorithm
    {
        public override Segmentation Segment(Bitmap image)
        {
            int[,] rows;
            int[,] cols;
            doSegment(image, out rows, out cols);
            return new Segmentation(rows, cols, image.Width, image.Height);
        }

        protected abstract void doSegment(Bitmap image, out int[,] rows, out int[,] cols);
    }
}
