/*
 * Dissertation CV Wordsearch Solver
 * Quantitative Evaluate
 * Evaluate Full System
 * By Josh Keegan 26/04/2014
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

        //TODO: Remove Training Wordsearch Images dependency when PCA is loaded rather than trained every time
        internal static Dictionary<string, double> Evaluate(List<Image> images, List<WordsearchImage> trainingWordsearchImages)
        {
            Log.Info("Starting to Evaluate Full System with different combinations of algorithms . . .");

            //Register an interest in the bitmaps of all the images (so that they remain in memory throughout)
            foreach (Image image in images)
            {
                image.RegisterInterestInBitmap();
            }

            //TODO: Remove the following once PCA is loaded rather than trained every time
            Dictionary<char, List<Bitmap>> trainingData = CharData.GetCharData(trainingWordsearchImages);

            Bitmap[] pcaTrainingData;
            double[][] output; //Unused
            CharData.GetNeuralNetworkBitmapsAndOutput(trainingData, out pcaTrainingData, out output);

            Dictionary<string, double> scores = new Dictionary<string, double>();

            SegmentationAlgorithm detectionSegmentationAlgorithm = new SegmentByHistogramThresholdPercentileRankTwoThresholds();
            SegmentationAlgorithm segmentationAlgorithm = new SegmentByBlobRecognition();

            //Use same classifier & feature extraction for both
            FeatureExtractionPCA featureExtraction = new FeatureExtractionPCA(); //TODO: Load trained feature extraction algorithm rather than training it again
            featureExtraction.Train(pcaTrainingData);

            Classifier classifier = new AForgeActivationNeuralNetClassifier(featureExtraction, PCA_ALL_FEATURES_NEURAL_NETWORK_CLASSIFIER_PATH);

            Solver wordsearchSolver = new SolverNonProbablistic();

            scores.Add("Detection Segmentation: HistogramThresholdPercentileRankTwoThresholds, Segmentation: BlobRecognition, Rotation Correction Classifier: Neural net with PCA (All Features), Classifier: Neural net with PCA (All Features), Wordsearch Solver: Non-Probablistic", 
                Evaluate(images, detectionSegmentationAlgorithm, segmentationAlgorithm, classifier, classifier, wordsearchSolver));

            //Deregsiter an interest in all of the images
            foreach (Image image in images)
            {
                image.DeregisterInterestInBitmap();
            }

            Log.Info("Completed evaluation of Full System with different combinations of algorithms");

            return scores;
        }

        private static double Evaluate(List<Image> images, SegmentationAlgorithm detectionSegmentationAlgorithm, 
            SegmentationAlgorithm segmentationAlgorithm, Classifier probablisticRotationCorrectionClassifier, 
            Classifier classifier, Solver wordsearchSolver)
        {
            Log.Info("Evaluating Full System . . .");

            int numCorrect = 0;

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
                //TODO: Make WordsearchRotation work with Segmentation rather than just rows & cols. 
                //Note that ATM this will prevent the system from working with uneven segmentations (requires uniform for rotation correction until segmentation is supported)
                WordsearchRotation originalRotation = new WordsearchRotation(extractedImage, segmentation.NumRows, segmentation.NumCols);

                WordsearchRotation rotatedWordsearch = WordsearchRotationCorrection.CorrectOrientation(originalRotation, probablisticRotationCorrectionClassifier);

                Bitmap rotatedImage = rotatedWordsearch.Bitmap;

                //If the wordsearch has been rotated
                if(rotatedImage != extractedImage)
                {
                    //TODO: remove when Segmentation is being used instead of rows & cols
                    segmentation = segmentationAlgorithm.Segment(rotatedWordsearch.Bitmap);

                    //Clean up the old Bitmap
                    extractedImage.Dispose();
                }

                /*
                 * Classification
                 */

                //Split image up into individual characters

                //TODO: Segmentation object Support (so can work with uneven image segmentation)
                ResizeBicubic resize = new ResizeBicubic(Constants.CHAR_WITH_WHITESPACE_WIDTH * segmentation.NumCols, 
                    Constants.CHAR_WITH_WHITESPACE_HEIGHT * segmentation.NumRows);
                Bitmap resizedImage = resize.Apply(rotatedImage);

                //Full sized rotated image no longer required
                rotatedImage.Dispose();

                Bitmap[,] rawCharImgs = SplitImage.Grid(resizedImage, segmentation.NumRows, segmentation.NumCols);

                //Resized image no longer required
                resizedImage.Dispose();

                //Get the part of the image that actually contains the character (without any whitespace)
                Bitmap[,] charImgs = CharImgExtractor.ExtractAll(rawCharImgs);

                //Raw char img's are no longer required
                rawCharImgs.ToSingleDimension().DisposeAll();

                //TODO: Move loops to classifier as this is reuseable
                double[][][] classifierOutput = new double[charImgs.GetLength(0)][][];

                for(int i = 0; i < classifierOutput.Length; i++) //Col
                {
                    classifierOutput[i] = new double[charImgs.GetLength(1)][];

                    for(int j = 0; j < classifierOutput[i].Length; j++) //Row
                    {
                        classifierOutput[i][j] = classifier.Classify(charImgs[i, j]);
                    }
                }

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

                Log.Info(evaluator.ToString());

                if(evaluator.Correct)
                {
                    numCorrect++;
                }
            }

            Log.Info(String.Format("System found all words correctly for {0} / {1} Images correctly", numCorrect, images.Count));
            Log.Info("Full System Evaluation Completed");

            return (double)numCorrect / images.Count;
        }
    }
}
