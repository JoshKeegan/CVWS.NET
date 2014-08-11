/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Quantitative Evaluation
 * Evaluate System from Segmentation until Solver (the entire system except Wordsearch Detection)
 * By Josh Keegan 19/05/2014
 * Last Edit 20/05/2014
 */

using libCVWS.ImageAnalysis.WordsearchSegmentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageMarkup;
using QuantitativeEvaluation.Evaluators;

namespace QuantitativeEvaluation
{
    internal static class EvaluateSegmentationToSolver
    {
        internal static Dictionary<string, double> Evaluate(List<WordsearchImage> wordsearchImages)
        {
            Log.Info("Starting to Evaluate System stages from Segmentation to Solver . . .");

            //Register an interest in the bitmaps of all the WordsearchImages (so that they remain in memory throughout)
            foreach(WordsearchImage wordsearchImage in wordsearchImages)
            {
                wordsearchImage.RegisterInterestInBitmap();
            }

            Dictionary<string, double> scores = new Dictionary<string, double>();

            //TODO: Remove any duplicate algorithms caused by this evaluation not using the DetectionSegmentationAlgorithm (or DetectionSegmentationRemoveSmallRowsAndCols)
            Dictionary<string, AlgorithmCombination> algorithmsToEvaluate = EvaluateFullSystem.GetAlgorithmsToEvaluate();
            foreach(KeyValuePair<string, AlgorithmCombination> kvp in algorithmsToEvaluate)
            {
                string description = kvp.Key;
                AlgorithmCombination algorithms = kvp.Value;

                scores.Add(description, Evaluate(wordsearchImages, algorithms));
            }


            //Deregister an interest in all of the Wordsearch Images
            foreach(WordsearchImage wordsearchImage in wordsearchImages)
            {
                wordsearchImage.DeregisterInterestInBitmap();
            }

            Log.Info("Completed evaluate of System stages from Segmentation to Solver");

            return scores;
        }

        private static double Evaluate(List<WordsearchImage> wordsearchImages, AlgorithmCombination algorithms)
        {
            Log.Info("Evaluating System Stages from Segmentation to Solver . . .");

            int numCorrect = 0;
            List<WordsearchSolutionEvaluator> evaluators = new List<WordsearchSolutionEvaluator>();

            foreach(WordsearchImage wordsearchImage in wordsearchImages)
            {
                //Register an interest in the Bitmap of the image
                wordsearchImage.RegisterInterestInBitmap();

                //Perform the evaluation (happens in a method from EvaluateFullSystem)
                WordsearchSolutionEvaluator evaluator = EvaluateFullSystem.EvaluateWordsearchBitmap(wordsearchImage.Bitmap,
                    wordsearchImage.Wordsearch.Words, wordsearchImage.Wordsearch.Solutions, algorithms);

                //Log Evaluation
                evaluators.Add(evaluator);

                Log.Info(evaluator.ToString());

                if (evaluator.Correct)
                {
                    numCorrect++;
                }

                //Clean up
                wordsearchImage.DeregisterInterestInBitmap();
            }

            Log.Info(String.Format("System found all words correctly for {0} / {1} Wordsearch Images correctly", numCorrect, wordsearchImages.Count));

            //Calculate some extra statistics
            int numWordsearchesNoWordsFound = 0;
            double fMeasureSum = 0;
            int numValidFMeasures = 0;

            foreach (WordsearchSolutionEvaluator evaluator in evaluators)
            {
                //If no words were found correctly
                if (evaluator.TruePositive == 0)
                {
                    numWordsearchesNoWordsFound++;
                }

                //If there was a valid F-Measure
                if (!double.IsNaN(evaluator.FMeasure))
                {
                    fMeasureSum += evaluator.FMeasure;
                    numValidFMeasures++;
                }
            }

            Log.Info(String.Format("In {0} wordsearches no words were found correctly at all", numWordsearchesNoWordsFound));
            Log.Info(String.Format("Average F-Measure (when not NaN): {0}", fMeasureSum / numValidFMeasures));

            Log.Info("Segmentation to Solver Evaluation Completed");

            return (double)numCorrect / wordsearchImages.Count;
        }
    }
}
