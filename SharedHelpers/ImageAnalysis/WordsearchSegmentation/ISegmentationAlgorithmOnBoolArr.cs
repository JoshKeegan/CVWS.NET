/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Segmentation Algorithm Interface for Algorithms that operate on bool[,] instead of Bitmaps
 * By Josh Keegan 03/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.ImageAnalysis.WordsearchSegmentation
{
    public interface ISegmentationAlgorithmOnBoolArr
    {
        //Method to extract the segmentation indices from a boolean array representation of a wordsearch image
        public Segmentation Segment(bool[,] image);
    }
}
