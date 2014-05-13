/*
 * Dissertation CV Wordsearch Solver
 * Quantitative Evaluate
 * Evaluate Full System
 * By Josh Keegan 26/04/2014
 * Last Edit 13/05/2014
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
        private const string PCA_ALL_FEATURES_NEURAL_NETWORK_CLASSIFIER_PATH = Program.NEURAL_NETWORKS_PATH + "SingleLayer Sigmoid BkPropLearn PCAAllFeatures" + Program.NEURAL_NETWORK_FILE_EXTENSION;

        private enum SegmentationMethod
        {
            FixedWidth,
            VariedWidthNoResize,
            VariedWidthWithResize
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

            SegmentationAlgorithm detectionSegmentationAlgorithm = new SegmentByHistogramThresholdPercentileRankTwoThresholds();
            SegmentationAlgorithm segmentationAlgorithm = new SegmentByBlobRecognition();

            //Use same classifier & feature extraction for both
            FeatureExtractionPCA featureExtraction = (FeatureExtractionPCA)TrainableFeatureExtractionAlgorithm.Load(
                Program.FEATURE_EXTRACTORS_PATH + Program.PCA_ALL_FEATURES_FILE_NAME + Program.FEATURE_EXTRACTORS_FILE_EXTENSION);

            Classifier classifier = new AForgeActivationNeuralNetClassifier(featureExtraction, PCA_ALL_FEATURES_NEURAL_NETWORK_CLASSIFIER_PATH);

            Solver wordsearchSolver = new SolverNonProbabilistic();
            Solver probabilisticWordsearchSolver = new SolverProbabilistic();

            //Standard System
            scores.Add("Detection Segmentation: HistogramThresholdPercentileRankTwoThresholds, Segmentation: BlobRecognition, Segmentation Method: Fixed Width, Rotation Correction Classifier: Neural net with PCA (All Features), Classifier: Neural net with PCA (All Features), Wordsearch Solver: Non-Probabilistic", 
                Evaluate(images, detectionSegmentationAlgorithm, segmentationAlgorithm, SegmentationMethod.FixedWidth, classifier, classifier, wordsearchSolver));
            
            //Don't resize characters to constants size after segmentation
            scores.Add("Detection Segmentation: HistogramThresholdPercentileRankTwoThresholds, Segmentation: BlobRecognition, Segmentation Method: Varied Width (No Resize), Rotation Correction Classifier: Neural net with PCA (All Features), Classifier: Neural net with PCA (All Features), Wordsearch Solver: Non-Probabilistic",
                Evaluate(images, detectionSegmentationAlgorithm, segmentationAlgorithm, SegmentationMethod.VariedWidthNoResize, classifier, classifier, wordsearchSolver));

            //Solve the wordsearch using probabilistic solver
            scores.Add("Detection Segmentation: HistogramThresholdPercentileRankTwoThresholds, Segmentation: BlobRecognition, Segmentation Method: Fixed Width, Rotation Correction Classifier: Neural net with PCA (All Features), Classifier: Neural net with PCA (All Features), Wordsearch Solver: Probabilistic",
                Evaluate(images, detectionSegmentationAlgorithm, segmentationAlgorithm, SegmentationMethod.FixedWidth, classifier, classifier, probabilisticWordsearchSolver));

            //Deregsiter an interest in all of the images
            foreach (Image image in images)
            {
                image.DeregisterInterestInBitmap();
            }

            Log.Info("Completed evaluation of Full System with different combinations of algorithms");

            return scores;
        }

        private static double Evaluate(List<Image> images, SegmentationAlgorithm detectionSegmentationAlgorithm, 
            SegmentationAlgorithm segmentationAlgorithm, SegmentationMethod segmentationMethod, 
            Classifier probablisticRotationCorrectionClassifier, Classifier classifier, Solver wordsearchSolver)
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
                Tuple<List<IntPoint>, Bitmap> wordsearchImageTuple = DetectionAlgorithm.ExtractBestWordsearch(image.Bitmap, detectionSegmentationAlgorithm);
                
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
                 * Wordsearch Segmentation
                 */
                Segmentation segmentation = segmentationAlgorithm.Segment(extractedImage);

                /*
                 * Wordsearch Rotation Correction
                 */
                WordsearchRotation originalRotation;

                //If we're using fixed row & col width
                if(segmentationMethod == SegmentationMethod.FixedWidth)
                {
                    originalRotation = new WordsearchRotation(extractedImage, segmentation.NumRows, segmentation.NumCols);
                }
                else //Otherwise we're using varied row/col width segmentation, use the Segmentation object
                {
                    originalRotation = new WordsearchRotation(extractedImage, segmentation);
                }

                WordsearchRotation rotatedWordsearch = WordsearchRotationCorrection.CorrectOrientation(originalRotation, probablisticRotationCorrectionClassifier);

                Bitmap rotatedImage = rotatedWordsearch.Bitmap;

                //If the wordsearch has been rotated
                if(rotatedImage != extractedImage)
                {
                    //Update the segmentation

                    //If the wordsearch rotation won't have been passed a segmentation
                    if(segmentationMethod == SegmentationMethod.FixedWidth)
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

                    //Clean up the old Bitmap
                    extractedImage.Dispose();
                }

                /*
                 * Classification
                 */

                //Split image up into individual characters
                Bitmap[,] rawCharImgs = null;

                //If we're using fixed row & col width
                if(segmentationMethod == SegmentationMethod.FixedWidth)
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
                    if(segmentationMethod == SegmentationMethod.VariedWidthWithResize)
                    {
                        ResizeBicubic resize = new ResizeBicubic(Constants.CHAR_WITH_WHITESPACE_WIDTH, Constants.CHAR_WITH_WHITESPACE_HEIGHT);

                        for (int i = 0; i < rawCharImgs.GetLength(0); i++)
                        {
                            for (int j = 0; j < rawCharImgs.GetLength(1); j++)
                            {
                                //Only do the resize if it isn't already that size
                                if(rawCharImgs[i, j].Width != Constants.CHAR_WITH_WHITESPACE_WIDTH
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
            Log.Info(String.Format("Average F-Measure (when not NaN): {0}", fMeasureSum / numValidFMeasures));

            Log.Info("Full System Evaluation Completed");

            return (double)numCorrect / images.Count;
        }
    }
}
