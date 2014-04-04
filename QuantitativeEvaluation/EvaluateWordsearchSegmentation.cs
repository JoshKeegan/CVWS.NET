/*
 * Dissertation CV Wordsearch Solver
 * Quantitative Evaluation
 * Evaluate Wordsearch Segmentation
 * By Josh Keegan 03/04/2014
 * Last Edit 04/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageMarkup;
using SharedHelpers.ImageAnalysis.WordsearchSegmentation;
using SharedHelpers.ImageAnalysis.WordsearchSegmentation.VariedRowColSize;

namespace QuantitativeEvaluation
{
    internal static class EvaluateWordsearchSegmentation
    {
        internal static Dictionary<string, double> EvaluateByNumRowsAndCols(List<WordsearchImage> wordsearchImages)
        {
            Log.Info("Starting to Evaluate all Wordsearch Image Segmentation Algorithms based on the number of rows & cols they return");

            Dictionary<string, double> scores = new Dictionary<string, double>();

            //Get the score for each segmentation algorithm
            scores.Add("MeanDarkPixels", EvaluateByNumRowsAndCols(wordsearchImages, new SegmentByMeanDarkPixels()));
            scores.Add("MedianDarkPixels", EvaluateByNumRowsAndCols(wordsearchImages, new SegmentByMedianDarkPixels()));
            scores.Add("PercentileTwoThresholds", EvaluateByNumRowsAndCols(wordsearchImages, new SegmentByPercentileTwoThresholds()));
            scores.Add("BlobRecognition", EvaluateByNumRowsAndCols(wordsearchImages, new SegmentByBlobRecognition()));

            Log.Info("Completed evaluation of all Wordsearch Image Segmentation Algorithms based on the number of rows and cols they return");

            return scores;
        }

        private static double EvaluateByNumRowsAndCols(List<WordsearchImage> wordsearchImages, SegmentationAlgorithm segAlgorithm)
        {
            Log.Info("Evaluating Wordsearch Image Segmentation by number of rows and cols returned . . .");

            int numCorrect = 0;

            //Test the algorithm on each Wordsearch Image
            foreach(WordsearchImage wordsearchImage in wordsearchImages)
            {
                //Register an interest in the Bitmap of the Wordsearch Image
                wordsearchImage.RegisterInterestInBitmap();

                Segmentation proposedSegmentation = segAlgorithm.Segment(wordsearchImage.Bitmap);

                //Keep track of the number of correct
                if(proposedSegmentation.NumRows == wordsearchImage.Rows && 
                    proposedSegmentation.NumCols == wordsearchImage.Cols)
                {
                    numCorrect++;
                }

                //Clean Up
                wordsearchImage.DeregisterInterestInBitmap();
            }

            Log.Info(String.Format("Returned {0}/{1} Wordsearch Segmentations Correctly", numCorrect, wordsearchImages.Count));
            Log.Info("Wordsearch Image Segmentation Evaluation Completed");

            return (double)numCorrect / wordsearchImages.Count;
        }
    }
}
