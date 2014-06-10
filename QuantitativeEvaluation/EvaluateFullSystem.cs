/*
 * Dissertation CV Wordsearch Solver
 * Quantitative Evaluate
 * Evaluate Full System
 * By Josh Keegan 26/04/2014
 * Last Edit 10/06/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bitmap = System.Drawing.Bitmap; //Bitmap only, else there will be a clash on Image w/ ImageMarkup.Image

using AForge;
using AForge.Imaging.Filters;

using BaseObjectExtensions;
using ImageMarkup;
using QuantitativeEvaluation.Evaluators;
using SharedHelpers;
using SharedHelpers.ClassifierInterfacing;
using SharedHelpers.ClassifierInterfacing.FeatureExtraction;
using SharedHelpers.ImageAnalysis.WordsearchDetection;
using SharedHelpers.ImageAnalysis.WordsearchRotation;
using SharedHelpers.ImageAnalysis.WordsearchSegmentation;
using SharedHelpers.ImageAnalysis.WordsearchSegmentation.VariedRowColSize;
using SharedHelpers.Imaging;
using SharedHelpers.WordsearchSolver;

namespace QuantitativeEvaluation
{
    internal static class EvaluateFullSystem
    {
        //Constants
        internal const string PCA_ALL_FEATURES_NEURAL_NETWORK_CLASSIFIER_PATH = Program.NEURAL_NETWORKS_PATH + "SingleLayer Sigmoid BkPropLearn PCAAllFeatures" + Program.NEURAL_NETWORK_FILE_EXTENSION;

        internal enum SegmentationMethod
        {
            FixedWidth,
            VariedWidthNoResize,
            VariedWidthWithResize
        }

        internal static Dictionary<string, AlgorithmCombination> GetAlgorithmsToEvaluate()
        {
            SegmentationAlgorithm detectionSegmentationAlgorithm = new SegmentByHistogramThresholdPercentileRankTwoThresholds();
            SegmentationAlgorithm segmentationAlgorithm = new SegmentByBlobRecognition();

            //Use same classifier & feature extraction for both
            FeatureExtractionPCA featureExtraction = (FeatureExtractionPCA)TrainableFeatureExtractionAlgorithm.Load(
                Program.FEATURE_EXTRACTORS_PATH + Program.PCA_ALL_FEATURES_FILE_NAME + Program.FEATURE_EXTRACTORS_FILE_EXTENSION);

            Classifier classifier = new AForgeActivationNeuralNetClassifier(featureExtraction, PCA_ALL_FEATURES_NEURAL_NETWORK_CLASSIFIER_PATH);

            Solver wordsearchSolver = new SolverNonProbabilistic();
            Solver probabilisticWordsearchSolver = new SolverProbabilistic();

            Dictionary<string, AlgorithmCombination> algorithmsToEvaluate = new Dictionary<string, AlgorithmCombination>()
            {
                //Standard System
                { 
                    "Detection Segmentation: HistogramThresholdPercentileRankTwoThresholds, Segmentation: BlobRecognition, Segmentation Method: Fixed Width, Rotation Correction Classifier: Neural net with PCA (All Features), Classifier: Neural net with PCA (All Features), Wordsearch Solver: Non-Probabilistic",
                    new AlgorithmCombination(detectionSegmentationAlgorithm, false, segmentationAlgorithm, false, SegmentationMethod.FixedWidth, classifier, classifier, wordsearchSolver) 
                },

                //Don't resize characters to constants size after segmentation
                {
                    "Detection Segmentation: HistogramThresholdPercentileRankTwoThresholds, Segmentation: BlobRecognition, Segmentation Method: Varied Width (No Resize), Rotation Correction Classifier: Neural net with PCA (All Features), Classifier: Neural net with PCA (All Features), Wordsearch Solver: Non-Probabilistic",
                    new AlgorithmCombination(detectionSegmentationAlgorithm, false, segmentationAlgorithm, false, SegmentationMethod.VariedWidthNoResize, classifier, classifier, wordsearchSolver)
                },

                //Solve the wordsearch using probabilistic solver
                {
                    "Detection Segmentation: HistogramThresholdPercentileRankTwoThresholds, Segmentation: BlobRecognition, Segmentation Method: Fixed Width, Rotation Correction Classifier: Neural net with PCA (All Features), Classifier: Neural net with PCA (All Features), Wordsearch Solver: Probabilistic",
                    new AlgorithmCombination(detectionSegmentationAlgorithm, false, segmentationAlgorithm, false, SegmentationMethod.FixedWidth, classifier, classifier, probabilisticWordsearchSolver)
                },

                //Solve the wordsearch using probabilistic solver & varied col width/row height segmentation (resize after character extraction)
                {
                    "Detection Segmentation: HistogramThresholdPercentileRankTwoThresholds, Segmentation: BlobRecognition, Segmentation Method: Varied Width (With Resize), Rotation Correction Classifier: Neural net with PCA (All Features), Classifier: Neural net with PCA (All Features), Wordsearch Solver: Probabilistic",
                    new AlgorithmCombination(detectionSegmentationAlgorithm, false, segmentationAlgorithm, false, SegmentationMethod.VariedWidthWithResize, classifier, classifier, probabilisticWordsearchSolver)
                },

                //Don't resize characters to constant size after segmentation & probablistic solver
                {
                    "Detection Segmentation: HistogramThresholdPercentileRankTwoThresholds, Segmentation: BlobRecognition, Segmentation Method: Varied Width (No Resize), Rotation Correction Classifier: Neural net with PCA (All Features), Classifier: Neural net with PCA (All Features), Wordsearch Solver: Probabilistic",
                    new AlgorithmCombination(detectionSegmentationAlgorithm, false, segmentationAlgorithm, false, SegmentationMethod.VariedWidthNoResize, classifier, classifier, probabilisticWordsearchSolver)
                },

                //Don't resize characters to constant size after segmentation & Probabilistic solver that prevents character discrepancies (when
                //  a position is used as one character in on one word, and another character in another word)
                {
                    "Detection Segmentation: HistogramThresholdPercentileRankTwoThresholds, Segmentation: BlobRecognition, Segmentation Method: Varied Width (No Resize), Rotation Correction Classifier: Neural net with PCA (All Features), Classifier: Neural net with PCA (All Features), Wordsearch Solver: Probabilistic Prevent Character Discrepancies",
                    new AlgorithmCombination(detectionSegmentationAlgorithm, false, segmentationAlgorithm, false, SegmentationMethod.VariedWidthNoResize, classifier, classifier, new SolverProbabilisticPreventCharacterDiscrepancies())
                },

                //Wordsearch detection system: Segment by Mean Dark Pixels
                {
                    "Detection Segmentation: MeanDarkPixels, Segmentation: BlobRecognition, Segmentation Method: Varied Width (No Resize), Rotation Correction Classifier: Neural net with PCA (All Features), Classifier: Neural net with PCA (All Features), Wordsearch Solver: Probabilistic Prevent Character Discrepancies",
                    new AlgorithmCombination(new SegmentByMeanDarkPixels(), false, segmentationAlgorithm, false, SegmentationMethod.VariedWidthNoResize, classifier, classifier, new SolverProbabilisticPreventCharacterDiscrepancies())
                },

                //Remove small rows and cols after segmentation
                {
                    "Detection Segmentation: MeanDarkPixels, Segmentation: BlobRecognition, Segmentation Method: Varied Width (No Resize), Rotation Correction Classifier: Neural net with PCA (All Features), Classifier: Neural net with PCA (All Features), Wordsearch Solver: Probabilistic Prevent Character Discrepancies",
                    new AlgorithmCombination(new SegmentByMeanDarkPixels(), false, segmentationAlgorithm, true, SegmentationMethod.VariedWidthNoResize, classifier, classifier, new SolverProbabilisticPreventCharacterDiscrepancies())
                }
            };

            return algorithmsToEvaluate;
        }

        internal static Dictionary<string, double> Evaluate(List<Image> images)
        {
            Log.Info("Starting to Evaluate Full System with different combinations of algorithms . . .");

            //Register an interest in the bitmaps of all the images (so that they remain in memory throughout)
            foreach (Image image in images)
            {
                image.RegisterInterestInBitmap();
            }

            Dictionary<string, double> scores = new Dictionary<string, double>();

            Dictionary<string, AlgorithmCombination> toEvaluate = GetAlgorithmsToEvaluate();
            foreach(KeyValuePair<string, AlgorithmCombination> kvp in toEvaluate)
            {
                string description = kvp.Key;
                AlgorithmCombination algorithms = kvp.Value;

                scores.Add(description, Evaluate(images, algorithms));
            }

            //Deregsiter an interest in all of the images
            foreach (Image image in images)
            {
                image.DeregisterInterestInBitmap();
            }

            Log.Info("Completed evaluation of Full System with different combinations of algorithms");

            return scores;
        }

        private static double Evaluate(List<Image> images, AlgorithmCombination algorithms)
        {
            return Evaluate(images, algorithms.DetectionSegmentationAlgorithm, algorithms.DetectionSegmentationRemoveSmallRowsAndCols,
                algorithms.SegmentationAlgorithm, algorithms.SegmentationRemoveSmallRowsAndCols,
                algorithms.SegmentationMethod, algorithms.ProbabilisticRotationCorrectionClassifier, algorithms.Classifier,
                algorithms.WordsearchSolver);
        }

        private static double Evaluate(List<Image> images, SegmentationAlgorithm detectionSegmentationAlgorithm, 
            bool detectionSegmentationRemoveSmallRowsAndCols, SegmentationAlgorithm segmentationAlgorithm, 
            bool segmentationRemoveSmallRowsAndCols, SegmentationMethod segmentationMethod, 
            Classifier probabilisticRotationCorrectionClassifier, Classifier classifier, Solver wordsearchSolver)
        {
            Log.Info("Evaluating Full System . . .");

            int numCorrect = 0;
            List<WordsearchSolutionEvaluator> evaluators = new List<WordsearchSolutionEvaluator>();

            foreach(Image image in images)
            {
                //Register an interest in the Bitmap of the image
                image.RegisterInterestInBitmap();

                /*
                 * Wordsearch Detection
                 */
                Tuple<List<IntPoint>, Bitmap> wordsearchImageTuple = DetectionAlgorithm.ExtractBestWordsearch(image.Bitmap, detectionSegmentationAlgorithm, detectionSegmentationRemoveSmallRowsAndCols);
                
                //Original wordsearch image is no longer required
                image.DeregisterInterestInBitmap();

                //If the system failed to find anything remotely resembling a wordsearch, fail now
                if(wordsearchImageTuple == null)
                {
                    continue;
                }

                //Get the words to look for later from this image & the correct solutions
                string[] wordsToFind = null; //Requires default, but won't even get used
                Dictionary<string, List<WordPosition>> correctSolutions = null;
                //If the image contains more than one wordsearch, we need to work out which one has been found
                if(image.WordsearchImages.Length > 1)
                {
                    List<IntPoint> coordinates = wordsearchImageTuple.Item1;
                    bool found = false;

                    //Select the wordsearch found using the algorithm for checking if the returned wordsearch is correct in EvaluateWordsearchDetection
                    foreach(WordsearchImage wordsearchImage in image.WordsearchImages)
                    {
                        //If it's this wordsearch
                        if(EvaluateWordsearchDetection.IsWordsearch(coordinates, wordsearchImage))
                        {
                            wordsToFind = wordsearchImage.Wordsearch.Words;
                            correctSolutions = wordsearchImage.Wordsearch.Solutions;
                            found = true;
                            break;
                        }
                    }

                    //If this isn't one of the wordsearches in the image, then fail now 
                    if(!found)
                    {
                        //Clean up
                        wordsearchImageTuple.Item2.Dispose();

                        continue;
                    }
                }
                else //Otherwise just use the one wordsearch that's in the image
                {
                    wordsToFind = image.WordsearchImages[0].Wordsearch.Words;
                    correctSolutions = image.WordsearchImages[0].Wordsearch.Solutions;
                }
                
                Bitmap extractedImage = wordsearchImageTuple.Item2;

                /*
                 * Image Segmentation onwards happen in EvaluateWordsearchBitmap
                 */
                WordsearchSolutionEvaluator evaluator = EvaluateWordsearchBitmap(extractedImage, wordsToFind, correctSolutions,
                    segmentationAlgorithm, segmentationRemoveSmallRowsAndCols, segmentationMethod, 
                    probabilisticRotationCorrectionClassifier, classifier, wordsearchSolver);

                //Clean up
                extractedImage.Dispose();

                //Log Evaluation
                evaluators.Add(evaluator);

                Log.Info(evaluator.ToString());

                if(evaluator.Correct)
                {
                    numCorrect++;
                }
            }

            Log.Info(String.Format("System found all words correctly for {0} / {1} Images correctly", numCorrect, images.Count));

            //Calculate some extra statistics
            int numWordsearchesNoWordsFound = 0;
            int numDidntReachEvaluation = images.Count - evaluators.Count;
            double fMeasureSum = 0;
            int numValidFMeasures = 0;

            foreach (WordsearchSolutionEvaluator evaluator in evaluators)
            {
                //If no words were found correctly
                if(evaluator.TruePositive == 0)
                {
                    numWordsearchesNoWordsFound++;
                }

                //If there was a valid F-Measure
                if(!double.IsNaN(evaluator.FMeasure))
                {
                    fMeasureSum += evaluator.FMeasure;
                    numValidFMeasures++;
                }
            }

            Log.Info(String.Format("In {0} wordsearches no words were found correctly at all", numWordsearchesNoWordsFound));
            Log.Info(String.Format("{0} wordsearch images got discarded before reaching the evaluation stage", numDidntReachEvaluation));
            Log.Info(String.Format("Average F-Measure (when not NaN): {0}", fMeasureSum / numValidFMeasures));

            Log.Info("Full System Evaluation Completed");

            return (double)numCorrect / images.Count;
        }

        internal static WordsearchSolutionEvaluator EvaluateWordsearchBitmap(Bitmap wordsearchBitmap, string[] wordsToFind,
            Dictionary<string, List<WordPosition>> correctSolutions, AlgorithmCombination algorithms)
        {
            return EvaluateWordsearchBitmap(wordsearchBitmap, wordsToFind, correctSolutions,
                algorithms.SegmentationAlgorithm, algorithms.SegmentationRemoveSmallRowsAndCols, 
                algorithms.SegmentationMethod, algorithms.ProbabilisticRotationCorrectionClassifier, 
                algorithms.Classifier, algorithms.WordsearchSolver);
        }

        internal static WordsearchSolutionEvaluator EvaluateWordsearchBitmap(Bitmap wordsearchBitmap, string[] wordsToFind,
            Dictionary<string, List<WordPosition>> correctSolutions, SegmentationAlgorithm segmentationAlgorithm, 
            bool segmentationRemoveSmallRowsAndCols, SegmentationMethod segmentationMethod,
            Classifier probabilisticRotationCorrectionClassifier, Classifier classifier, Solver wordsearchSolver)
        {
            /*
             * Wordsearch Segmentation
             */
            Segmentation segmentation = segmentationAlgorithm.Segment(wordsearchBitmap);

            /*
             * Wordsearch Rotation Correction
             */
            WordsearchRotation originalRotation;

            //If we're using fixed row & col width
            if (segmentationMethod == SegmentationMethod.FixedWidth)
            {
                originalRotation = new WordsearchRotation(wordsearchBitmap, segmentation.NumRows, segmentation.NumCols);
            }
            else //Otherwise we're using varied row/col width segmentation, use the Segmentation object
            {
                originalRotation = new WordsearchRotation(wordsearchBitmap, segmentation);
            }

            WordsearchRotation rotatedWordsearch = WordsearchRotationCorrection.CorrectOrientation(originalRotation, probabilisticRotationCorrectionClassifier);

            Bitmap rotatedImage = rotatedWordsearch.Bitmap;

            //If the wordsearch has been rotated
            if (rotatedImage != wordsearchBitmap)
            {
                //Update the segmentation

                //If the wordsearch rotation won't have been passed a segmentation
                if (segmentationMethod == SegmentationMethod.FixedWidth)
                {
                    //Make a new fixed width segmentation from the WordsearchRotation
                    segmentation = new Segmentation(rotatedWordsearch.Rows, rotatedWordsearch.Cols,
                        rotatedImage.Width, rotatedImage.Height);
                }
                else
                {
                    //Use the rotated segmentation 
                    segmentation = rotatedWordsearch.Segmentation;
                }
            }

            /*
             * Classification
             */

            //Split image up into individual characters
            Bitmap[,] rawCharImgs = null;

            //If we're using fixed row & col width
            if (segmentationMethod == SegmentationMethod.FixedWidth)
            {
                ResizeBicubic resize = new ResizeBicubic(Constants.CHAR_WITH_WHITESPACE_WIDTH * segmentation.NumCols,
                    Constants.CHAR_WITH_WHITESPACE_HEIGHT * segmentation.NumRows);
                Bitmap resizedImage = resize.Apply(rotatedImage);

                rawCharImgs = SplitImage.Grid(resizedImage, segmentation.NumRows, segmentation.NumCols);

                //Resized image no longer required
                resizedImage.Dispose();
            }
            else //Otherwise we're using varied row/col width segmentation
            {
                rawCharImgs = SplitImage.Segment(rotatedImage, segmentation);

                //If the Segmentation Method is to resize the raw char imgs, resize them
                if (segmentationMethod == SegmentationMethod.VariedWidthWithResize)
                {
                    ResizeBicubic resize = new ResizeBicubic(Constants.CHAR_WITH_WHITESPACE_WIDTH, Constants.CHAR_WITH_WHITESPACE_HEIGHT);

                    for (int i = 0; i < rawCharImgs.GetLength(0); i++)
                    {
                        for (int j = 0; j < rawCharImgs.GetLength(1); j++)
                        {
                            //Only do the resize if it isn't already that size
                            if (rawCharImgs[i, j].Width != Constants.CHAR_WITH_WHITESPACE_WIDTH
                                || rawCharImgs[i, j].Height != Constants.CHAR_WITH_WHITESPACE_HEIGHT)
                            {
                                Bitmap orig = rawCharImgs[i, j];

                                rawCharImgs[i, j] = resize.Apply(orig);

                                //Remove the now unnecessary original/not resized image
                                orig.Dispose();
                            }
                        }
                    }
                }
            }

            //Full sized rotated image no longer required
            rotatedImage.Dispose();

            //Get the part of the image that actually contains the character (without any whitespace)
            Bitmap[,] charImgs = CharImgExtractor.ExtractAll(rawCharImgs);

            //Raw char img's are no longer required
            rawCharImgs.ToSingleDimension().DisposeAll();

            //Perform the classification on all of the images (returns probabilities for each possible class)
            double[][][] classifierOutput = classifier.Classify(charImgs);

            //Actual images of the characters are no longer required
            charImgs.ToSingleDimension().DisposeAll();

            /*
             * Solve Wordsearch
             */
            Solution solution = wordsearchSolver.Solve(classifierOutput, wordsToFind);

            /*
             * Evaluate the Proposed Solution
             */
            WordsearchSolutionEvaluator evaluator = new WordsearchSolutionEvaluator(solution, correctSolutions);

            return evaluator;
        }

    }
}
