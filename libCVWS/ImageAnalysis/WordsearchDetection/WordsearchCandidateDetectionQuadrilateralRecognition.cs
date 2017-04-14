/*
 * CVWS.NET
 * libCvws
 * WordsearchCandidateDetectionQuadrilateralRecognition - A wordsearch candidate detection algorithm that works
 *  by finding quadrilaterals in the input image (so will only work on wordsearches with a bounding rectangle).
 * Authors:
 *  Josh Keegan 14/04/2017
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;
using AForge.Imaging.Filters;

using libCVWS.Imaging;

namespace libCVWS.ImageAnalysis.WordsearchDetection
{
    public class WordsearchCandidateDetectionQuadrilateralRecognition : IWordsearchCandidatesDetectionAlgorithm
    {
        #region Constants

        //min width and height a blob must be in order to be a wordsearch candidate, as a percentage of that dimension of the image
        private const double BLOB_MIN_DIMENSION_PERCENTAGE = 10;

        #endregion

        #region Implement IWordsearchCandidatesDetectionAlgorithm

        public WordsearchCandidate[] FindCandidates(Bitmap image)
        {
            double blobMinDimensionDbl = BLOB_MIN_DIMENSION_PERCENTAGE / 100;
            int minWidth = (int) Math.Ceiling(image.Width * blobMinDimensionDbl); // Round up, so that the integer comaprison minimum will always be correct
            int minHeight = (int) Math.Ceiling(image.Height * blobMinDimensionDbl);

            // Find all quadrilaterals in the image
            List<List<IntPoint>> quads = ShapeFinder.Quadrilaterals(image, minWidth, minHeight);

            // Make WordsearchCandidates to return by extracting the target area from the base image
            return quads
                .Select(quad => new WordsearchCandidate(quad, new QuadrilateralTransformation(quad).Apply(image)))
                .ToArray();
        }

        #endregion
    }
}
