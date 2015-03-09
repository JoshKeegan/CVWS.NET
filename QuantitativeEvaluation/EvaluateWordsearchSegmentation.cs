/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Quantitative Evaluation
 * Evaluate Wordsearch Segmentation
 * By Josh Keegan 03/04/2014
 * Last Edit 09/03/2015
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KLogNet;

using ImageMarkup;
using libCVWS.ImageAnalysis.WordsearchSegmentation;
using libCVWS.ImageAnalysis.WordsearchSegmentation.VariedRowColSize;

namespace QuantitativeEvaluation
{
    internal static class EvaluateWordsearchSegmentation
    {
        internal static readonly Dictionary<String, SegmentationAlgorithm> SEGMENTATION_ALGORITHMS =
            new Dictionary<string, SegmentationAlgorithm>()
        {
            { "MeanDarkPixels", new SegmentByMeanDarkPixels() },
            { "MedianDarkPixels", new SegmentByMedianDarkPixels() },
            { "PercentileTwoThresholds", new SegmentByPercentileTwoThresholds() },
            { "BlobRecognition", new SegmentByBlobRecognition() },
            { "HistogramThresholdDarkPixels", new SegmentByHistogramThresholdDarkPixels() },
            { "ThresholdDarkPixels", new SegmentByThresholdDarkPixels() },
            { "HistogramThresholdPercentileRankTwoThresholds", new SegmentByHistogramThresholdPercentileRankTwoThresholds() }
        };

        internal static Dictionary<string, double> EvaluateByNumRowsAndCols(List<WordsearchImage> wordsearchImages)
        {
            DefaultLog.Info("Starting to Evaluate all Wordsearch Image Segmentation Algorithms based on the number of rows & cols they return");

            Dictionary<string, double> scores = new Dictionary<string, double>();

            //Get the score for each segmentation algorithm
            foreach(KeyValuePair<string, SegmentationAlgorithm> kvp in SEGMENTATION_ALGORITHMS)
            {
                string name = kvp.Key;
                SegmentationAlgorithm algorithm = kvp.Value;

                Tuple<double, double?> algorithmScores = EvaluateByNumRowsAndCols(wordsearchImages, algorithm);
                scores.Add(name, algorithmScores.Item1);
                
                //If a score was returned for having removed small rows and cols, log that too
                if(algorithmScores.Item2 != null)
                {
                    scores.Add(name + " (RemoveSmallRowsAndCols)", algorithmScores.Item2.GetValueOrDefault());
                }
            }

            DefaultLog.Info("Completed evaluation of all Wordsearch Image Segmentation Algorithms based on the number of rows and cols they return");

            return scores;
        }

        //returns score & score after removing erroenously small rows and cols (null if segmentation algorithm doesn't support this)
        private static Tuple<double, double?> EvaluateByNumRowsAndCols(List<WordsearchImage> wordsearchImages, SegmentationAlgorithm segAlgorithm)
        {
            DefaultLog.Info("Evaluating Wordsearch Image Segmentation by number of rows and cols returned . . .");

            int numCorrect = 0;
            int numCorrectRemoveSmallRowsAndCols = 0;

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

                //If this segmentation algorithm performs its segmentation with start & end character indices & can therefore have
                //  small rows and cols removed, do it
                if(segAlgorithm is SegmentationAlgorithmByStartEndIndices)
                {
                    Segmentation segRemoveSmallRowsAndCols = proposedSegmentation.RemoveSmallRowsAndCols();

                    if(segRemoveSmallRowsAndCols.NumRows == wordsearchImage.Rows &&
                        segRemoveSmallRowsAndCols.NumCols == wordsearchImage.Cols)
                    {
                        numCorrectRemoveSmallRowsAndCols++;
                    }
                }

                //Clean Up
                wordsearchImage.DeregisterInterestInBitmap();
            }

            DefaultLog.Info("Returned {0}/{1} Wordsearch Segmentations Correctly", numCorrect, wordsearchImages.Count);

            double score = (double)numCorrect / wordsearchImages.Count;
            double? scoreRemoveSmallRowsAndCols = null;
            if(segAlgorithm is SegmentationAlgorithmByStartEndIndices)
            {
                scoreRemoveSmallRowsAndCols = (double)numCorrectRemoveSmallRowsAndCols / wordsearchImages.Count;
                DefaultLog.Info("Returned {0}/{1} Wordsearch Segmentations Correctly after removing small rows and cols", 
                    numCorrectRemoveSmallRowsAndCols, wordsearchImages.Count);
            }

            DefaultLog.Info("Wordsearch Image Segmentation Evaluation Completed");

            return Tuple.Create(score, scoreRemoveSmallRowsAndCols);
        }
    }
}
