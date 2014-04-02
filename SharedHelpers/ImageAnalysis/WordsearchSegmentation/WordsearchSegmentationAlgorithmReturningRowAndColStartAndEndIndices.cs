﻿/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Segmentation Algorithm Returning Start And End Indices - abstract class
 * By Josh Keegan 02/04/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.ImageAnalysis.WordsearchSegmentation
{
    public abstract class WordsearchSegmentationAlgorithmByStartEndIndices : WordsearchSegmentationAlgorithm
    {
        public override WordsearchSegmentation Segment(Bitmap image)
        {
            int[,] rows;
            int[,] cols;
            doSegment(image, out rows, out cols);
            return new WordsearchSegmentation(rows, cols);
        }

        protected abstract void doSegment(Bitmap image, out int[,] rows, out int[,] cols);
    }
}
