/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Detection Algorithm
 * By Josh Keegan 22/04/2014
 * Last Edit 10/06/2014
 * 
 * Note: This is a static class containing the methods to recognise a wordsearch in an image that is bounded by a rectangle.
 *  This will be used in the final dissertation due to time constraints. If any future work were done in this area, then this
 *  WordsearchRecognition namespace would be refactored to work in the same manner as WordsearchSegmentstion and this would
 *  be one of many concrete implementations of the abstract concept of a WordsearchRecognitionAlgorithm
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;
using AForge.Imaging.Filters;

using SharedHelpers.Exceptions;
using SharedHelpers.ImageAnalysis.WordsearchSegmentation;
using SharedHelpers.ImageAnalysis.WordsearchSegmentation.VariedRowColSize;
using SharedHelpers.Imaging;

namespace SharedHelpers.ImageAnalysis.WordsearchDetection
{
    public static class DetectionAlgorithm
    {
        //Constants
        private const double BLOB_MIN_DIMENSION_PERCENTAGE = 10; //min width and height a blob must be in order to be a wordsearch candidate, as a percentage of that dimension of the image

        //Method to extract a Bitmap of the best match for a wordsearch in an image (automatically using 
        //  the Wordsearch Segmentation Algorithm decided as being best in evaluation)
        public static Bitmap ExtractBestWordsearch(Bitmap image)
        {
            //TODO: Change the segmentation algorithm to the one that actually performs best in final evaluation
            SegmentationAlgorithm segAlg = new SegmentByHistogramThresholdPercentileRankTwoThresholds();

            //Just return the Bitmap Image as the coordinates are only really useful during evaluation
            Tuple<List<IntPoint>, Bitmap> bestCandidate = ExtractBestWordsearch(image, segAlg);
            if(bestCandidate != null)
            {
                return bestCandidate.Item2;
            }
            else
            {
                return null;
            }
        }

        public static Tuple<List<IntPoint>, Bitmap> ExtractBestWordsearch(Bitmap image, SegmentationAlgorithm segAlg)
        {
            return ExtractBestWordsearch(image, segAlg, false); //Don't remove the erroneously small rows and cols by default
        }

        //Method to extract a Bitmap of the best match for a wordsearch in an image using a specified Wordsearch Segmentation Algorithm
        //returns null if no Wordsearch candidate could be found in the image
        public static Tuple<List<IntPoint>, Bitmap> ExtractBestWordsearch(Bitmap image, SegmentationAlgorithm segAlg, bool removeSmallRowsAndCols)
        {
            double blobMinDimensionDbl = BLOB_MIN_DIMENSION_PERCENTAGE / 100;
            int minWidth = (int)Math.Ceiling(image.Width * blobMinDimensionDbl); //Round up, so that the integer comaprison minimum will always be correct
            int minHeight = (int)Math.Ceiling(image.Height * blobMinDimensionDbl);

            List<List<IntPoint>> quads = ShapeFinder.Quadrilaterals(image, minWidth, minHeight);

            //Check that there are some quads found to search through for the best wordsearch candidate
            if(quads.Count != 0)
            {
                double bestScore = double.NegativeInfinity;
                List<IntPoint> bestCoords = null;
                Bitmap bestBitmap = null;

                //Search for the Bitmap that yields the best score
                foreach (List<IntPoint> quad in quads)
                {
                    //Extract the Bitmap of this quad
                    QuadrilateralTransformation quadTransform = new QuadrilateralTransformation(quad);
                    Bitmap quadBitmap = quadTransform.Apply(image);

                    //Score this wordsearch candidate
                    double score;
                    //If an InvalidRowsAndCols Exception gets throw by the segmentation 
                    //  (due to there being 0 rows/cols in the returned segmentation)
                    //  this is obviously not a good wordsearch candidate
                    try
                    {
                        Segmentation segmentation = segAlg.Segment(quadBitmap);
                        
                        //If removing erroneously small rows and cols before scoring the segmentation, do so now
                        if(removeSmallRowsAndCols)
                        {
                            segmentation = segmentation.RemoveSmallRowsAndCols();
                        }

                        CandidateScorer scorer = new CandidateScorer(segmentation);
                        score = scorer.WordsearchRecognitionScore;
                    }
                    catch(InvalidRowsAndColsException)
                    {
                        //This is slightly better than the default score of Negative Infinity as any candidate
                        //  (even one with no rows or cols found in it) is better than no candidate whatsoever
                        score = double.MinValue;
                    }

                    //If this score is better than the previous best (don't 
                    //  override equal scores as the list is size ordered and 
                    //  we'll default to the biggest wordsearch as being better)
                    if(score > bestScore)
                    {
                        bestScore = score;
                        bestCoords = quad;

                        //Dispose of the previously best Bitmap resource
                        if(bestBitmap != null)
                        {
                            bestBitmap.Dispose();
                        }
                        //Update the ptr to the new one
                        bestBitmap = quadBitmap;
                    }
                    else
                    {
                        //Clean up
                        quadBitmap.Dispose();
                    }
                }

                return Tuple.Create(bestCoords, bestBitmap);
            }
            else //Otherwise there are no quads to search through
            {
                return null;
            }
        }
    }
}
