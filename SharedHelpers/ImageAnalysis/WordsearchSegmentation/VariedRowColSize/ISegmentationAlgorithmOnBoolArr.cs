/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Shared Helpers
 * Wordsearch Segmentation Algorithm Interface for Algorithms that operate on bool[,] instead of Bitmaps
 * By Josh Keegan 03/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.ImageAnalysis.WordsearchSegmentation.VariedRowColSize
{
    public interface ISegmentationAlgorithmOnBoolArr
    {
        //Method to extract the segmentation indices from a boolean array representation of a wordsearch image
        void DoSegment(bool[,] image, out int[,] rows, out int[,] cols);
    }
}
