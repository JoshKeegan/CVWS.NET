/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS
 * Wordsearch Segmentation Algorithm - abstract class
 * By Josh Keegan 02/04/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.ImageAnalysis.WordsearchSegmentation
{
    public abstract class SegmentationAlgorithm
    {
        //Method to extract the segmentation points from a Bitmap of a wordsearch
        public abstract Segmentation Segment(Bitmap image);
    }
}
