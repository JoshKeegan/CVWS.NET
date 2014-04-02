/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Segmentation Algorithm - abstract class
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
    public abstract class WordsearchSegmentationAlgorithm
    {
        //Method to extract the segmentation points from a Bitmap of a wordsearch
        public abstract WordsearchSegmentation Segment(Bitmap image);
    }
}
