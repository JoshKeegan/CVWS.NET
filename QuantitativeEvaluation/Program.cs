﻿/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Quantitative Evaluation
 * Program Entry Point
 * Authors:
 *  Josh Keegan 08/03/2013
 */

using System;
using System.Collections.Generic;
using System.IO;

using KLog;

using ImageMarkup;
using QuantitativeEvaluation.Evaluators;
using libCVWS.ClassifierInterfacing;
using libCVWS.ClassifierInterfacing.FeatureExtraction;

namespace QuantitativeEvaluation
{
    public static class Program
    {
        //Constants
        private const string LOGS_DIR_PATH = "logs";
#if DEBUG
        private const LogLevel LOG_LEVEL = LogLevel.All;
#endif

#if !DEBUG
        private const LogLevel LOG_LEVEL = LogLevel.Info | LogLevel.Warning | LogLevel.Error;
#endif
        private const string EVALUATION_RESULTS_DIR_PATH = "EvaluationResults";
        private const string CLASSIFIERS_PATH = ImageMarkup.ImageMarkupDatabase.DATA_DIRECTORY_PATH + "Classifiers/";
        internal const string FEATURE_EXTRACTORS_PATH = ImageMarkup.ImageMarkupDatabase.DATA_DIRECTORY_PATH + "FeatureExtractors/";
        internal const string NEURAL_NETWORKS_PATH = CLASSIFIERS_PATH + "NeuralNetworks/";
        internal const string NEURAL_NETWORK_FILE_EXTENSION = ".networkWeights";
        internal const string FEATURE_EXTRACTORS_FILE_EXTENSION = ".featureExtraction";
        internal const string PCA_ALL_FEATURES_FILE_NAME = "pcaAllFeatures";
        internal const string PCA_TOP_FEATURES_FILE_NAME = "pcaTopFeatures";
        private const string OLD_IMAGES_PATH = @"images\2014.02.20";
        private const bool EVALUATE_NEURAL_NETWORKS = false;
        private const bool EVALUATE_WORDSEARCH_ROTATION_CORRECTION = false;
        private const bool EVALUATE_WORDSEARCH_SEGMENTATION = false;
        private const bool EVALUATE_OVERALL_WORDSEARCH_DETECTION = true;
        private const bool EVALUATE_FULL_SYSTEM = false;
        private const bool EVALUATE_STAGES_SEGMENTATION_TO_SOLVER = false;

        public static void Main(string[] args)
        {
            //Initialise logging to the console &  a file
            Log consoleLog = new ConsoleLog(LOG_LEVEL);
            Log fileLog;
            bool dirCreated = false;
            if (!Directory.Exists(LOGS_DIR_PATH))
            {
                dirCreated = true;
                System.IO.Directory.CreateDirectory(LOGS_DIR_PATH);
            }

            int logAttempt = 0;
            while(true)
            {
                string logName = String.Format("{0}/Quantitative_Evaluation_DefaultLog.{1}.{2:000}.log", LOGS_DIR_PATH, DateTime.Now.ToString("yyyy-MM-dd"), logAttempt);
                if (!System.IO.File.Exists(logName))
                {
                    fileLog = new FileLog(logName, LOG_LEVEL);
                    DefaultLog.Info("Log Initialised");
                    break;
                }
                logAttempt++;
            }
            DefaultLog.Log = new CompoundLog(consoleLog, fileLog);
            if (dirCreated)
            {
                DefaultLog.Warn("Logs Directory was not found, creating . . .");
            }

            //If the directories for storing Classifiers don't exist, make them now
            if (!Directory.Exists(CLASSIFIERS_PATH))
            {
                DefaultLog.Info("Classifiers Directory didn't exist, creating . . .");
                Directory.CreateDirectory(CLASSIFIERS_PATH);
            }
            if (!Directory.Exists(NEURAL_NETWORKS_PATH))
            {
                DefaultLog.Info("Neural Networks Path didn't exist, creating . . .");
                Directory.CreateDirectory(NEURAL_NETWORKS_PATH);
            }

            //Load the Wordsearch Database
            DefaultLog.Info("Loading Wordsearch Database . . .");
            ImageMarkupDatabase.LoadDatabase();
            DefaultLog.Info("Wordsearch Database Loaded");

            //TODO: Change to some pre-determined split rather than splitting at runtime
            //Split the Wordsearch Images into 3 groups: training, cross-validation & evaluation
            DefaultLog.Info("Splitting Wordsearch Image data into Training, Cross-Validation & Evaluation data sets");
            List<WordsearchImage> wordsearchImages = ImageMarkupDatabase.GetWordsearchImages();

            List<WordsearchImage> trainingWordsearchImages = new List<WordsearchImage>();
            List<WordsearchImage> crossValidationWordsearchImages = new List<WordsearchImage>();
            List<WordsearchImage> evaluationWordsearchImages = new List<WordsearchImage>();

            //Split the images from 2014.02.20 into the three groups & add all of the 2014.05.18 images to the evaluation data set
            int oldImagesNum = 0;
            foreach(WordsearchImage wordsearchImage in wordsearchImages)
            {
                string imgPath = wordsearchImage.FromImage.Path;

                //If this image is in the old image directory (the one being split amongst the 3 data sets)
                if(imgPath.Substring(0, OLD_IMAGES_PATH.Length) == OLD_IMAGES_PATH)
                {
                    //Determine which data set to put the image in
                    if (oldImagesNum % 3 == 0)
                    {
                        trainingWordsearchImages.Add(wordsearchImage);
                    }
                    else if(oldImagesNum % 3 == 1)
                    {
                        crossValidationWordsearchImages.Add(wordsearchImage);
                    }
                    else
                    {
                        evaluationWordsearchImages.Add(wordsearchImage);
                    }
                    oldImagesNum++;
                }
                else //Otherwise this image in in the new image directory and should be put in the evaluation data set
                {
                    evaluationWordsearchImages.Add(wordsearchImage);
                }
            }

            DefaultLog.Info("Data split into Training, Cross-Validation & Evalutaion data");

            //If we're evaluating neural networks
            if (EVALUATE_NEURAL_NETWORKS)
            {
                //Evaluate all of the neural network combo's
                DefaultLog.Info("Starting to evaluate all Neural Networks . . .");
                IDictionary<string, NeuralNetworkEvaluator> neuralNetworkEvalResults =
                    EvaluateNeuralNetworks.evaluateNeuralNetworks(trainingWordsearchImages,
                    crossValidationWordsearchImages, evaluationWordsearchImages);
                DefaultLog.Info("Evaluation of all Neural Networks completed");

                //Write out the evaluation results
                if (!Directory.Exists(EVALUATION_RESULTS_DIR_PATH))
                {
                    DefaultLog.Info("Evaluation Results Directory didn't exist, creating . . .");
                    Directory.CreateDirectory(EVALUATION_RESULTS_DIR_PATH);
                }

                DefaultLog.Info("Writing out Neural Network Evaluation Results . . .");
                foreach (KeyValuePair<string, NeuralNetworkEvaluator> pair in neuralNetworkEvalResults)
                {
                    string networkName = pair.Key;
                    ConfusionMatrix cm = pair.Value.ConfusionMatrix;

                    DefaultLog.Info("Network \"{0}\" misclassified {1}/{2}", networkName,
                        cm.NumMisclassifications, cm.TotalClassifications);

                    try
                    {
                        cm.WriteToCsv(EVALUATION_RESULTS_DIR_PATH + "/" + networkName + ".csv");
                    }
                    catch (Exception e)
                    {
                        DefaultLog.Error("Error writing Confusion Matrix to file " + EVALUATION_RESULTS_DIR_PATH + "/" + networkName + ".csv");
                        Console.WriteLine(e);
                    }
                }
                DefaultLog.Info("Neural Network Evaluation results written out successfully");
            }
            
            //If we're evaluating Wordsearch rotation correction
            if(EVALUATE_WORDSEARCH_ROTATION_CORRECTION)
            {
                DefaultLog.Info("Starting to evaluate Wordsearch Rotation Correction");

                //Get the Feature Reduction Algorithm to be used
                DefaultLog.Info("Loading Feature Extraction Algorithm . . .");
                //Load a pre-trained feature reduction algorithm rather than training on the training data every time
                FeatureExtractionPCA featureExtractionAlgorithm = (FeatureExtractionPCA)TrainableFeatureExtractionAlgorithm.Load(
                    FEATURE_EXTRACTORS_PATH + PCA_ALL_FEATURES_FILE_NAME + FEATURE_EXTRACTORS_FILE_EXTENSION);
                DefaultLog.Info("Feature Extraction Algorithm Loaded");

                //Get the classifier to be used
                DefaultLog.Info("Loading Classifier . . .");
                Classifier classifier = new AForgeActivationNeuralNetClassifier(featureExtractionAlgorithm, 
                    NEURAL_NETWORKS_PATH + "SingleLayer Sigmoid BkPropLearn PCAAllFeatures" + NEURAL_NETWORK_FILE_EXTENSION);
                DefaultLog.Info("Classifier Loaded");

                //Evaluate the wordsearch Image Rotation Correction
                double rotationCorrectionRate = EvaluateWordsearchRotationCorrection.Evaluate(evaluationWordsearchImages, classifier);
                DefaultLog.Info("Wordsearch Rotation Correction returned the correct answer {0}% of the time", rotationCorrectionRate * 100);

                DefaultLog.Info("Wordsearch Rotation Correction Evaluation complete");
            }

            //If we're evaluating Wordsearch Segmentation
            if(EVALUATE_WORDSEARCH_SEGMENTATION)
            {
                /*
                 * Note that here all Wordsearch Images are used for evaluation (essentially they are all in the evaluation data set)
                 * This is because the training & cross validation data haven't been used to develop the algorithms, meaning that they 
                 * are fresh data and can be used for evaluation
                 */

                DefaultLog.Info("Starting to evaluate Wordsearch Segmentation");

                //Evaluate by the number of rows and cols returned
                Dictionary<string, double> scoresByNumRowsCols = EvaluateWordsearchSegmentation.EvaluateByNumRowsAndCols(wordsearchImages);

                //Print out scores
                DefaultLog.Info("Scores for Evaluation by Number of Rows and Cols");
                printScores(scoresByNumRowsCols);

                DefaultLog.Info("Wordsearch Segmentation Evaluation complete");
            }

            //If we're evaluating overall Wordsearch Detection (Candidates Detection & Candidate Vetting combined)
            if (EVALUATE_OVERALL_WORDSEARCH_DETECTION)
            {
                /*
                 * Note that here all Images are used for evaluation (essentially they are all in the evaluation data set)
                 * This is because the training & cross validation data haven't been used to develop the algorithms, meaning
                 * that they are fresh data and can be used for evaluation
                 */

                DefaultLog.Info("Starting to evaluate Overall Wordsearch Detection");

                Dictionary<string, double> scores = EvaluateOverallWordsearchDetection.EvaluateReturnsWordsearch(ImageMarkupDatabase.GetImages());

                //Print out scores
                DefaultLog.Info("Scores for Evaluation based on a single wordsearch returned (the best candidate)");
                printScores(scores);

                DefaultLog.Info("Overall Wordsearch Detection Evaluation Complete");
            }

            //If we're evaluating the Full System
            if(EVALUATE_FULL_SYSTEM)
            {
                /*
                 * This evaluation stage uses Images rather than WordsearchImages, and the Images used must not contain any WordsearchImages 
                 * from the training or cross-validation data sets
                 */

                DefaultLog.Info("Building the collection of Evaluation Images . . .");

                //Compile a list of the hashes of the Images that contain each WordsearchImage in the training & cross-validation data sets
                HashSet<string> usedImageHashes = new HashSet<string>();
                List<WordsearchImage> usedWordsearchImages = new List<WordsearchImage>(trainingWordsearchImages);
                usedWordsearchImages.AddRange(crossValidationWordsearchImages);
                foreach(WordsearchImage wordsearchImage in usedWordsearchImages)
                {
                    usedImageHashes.Add(wordsearchImage.FromImageHash);
                }

                //Now build a list of Images whose hash aren't present in the set of used hashes
                List<Image> allImages = ImageMarkupDatabase.GetImages();
                List<Image> evaluationImages = new List<Image>();
                foreach(Image image in allImages)
                {
                    if(!usedImageHashes.Contains(image.Hash))
                    {
                        evaluationImages.Add(image);
                    }
                }

                DefaultLog.Info("Collection of Evaluation Images built");

                DefaultLog.Info("Starting to Evaluate the Full System");

                Dictionary<string, double> scores = EvaluateFullSystem.Evaluate(evaluationImages);

                //Print out scores
                DefaultLog.Info("Scores for Evaluation of the Full System");
                printScores(scores);

                DefaultLog.Info("Full System Evaluation Complete");
            }

            //If we're evaluating the stages after Wordsearch Detection (Segmentation to Solver)
            if(EVALUATE_STAGES_SEGMENTATION_TO_SOLVER)
            {
                DefaultLog.Info("Starting to Evaluate the stages from Segmentation until Solving");

                Dictionary<string, double> scores = EvaluateSegmentationToSolver.Evaluate(evaluationWordsearchImages);

                //Print out scores
                DefaultLog.Info("Scores for Evaluation of the stages Segmentation to Solver");
                printScores(scores);

                DefaultLog.Info("Evaluation of stages Segmentation to Solving complete");
            }
        }

        private static void printScores(IDictionary<string, double> scores)
        {
            foreach(KeyValuePair<string, double> kvp in scores)
            {
                string algorithm = kvp.Key;
                double score = kvp.Value;

                DefaultLog.Info("Algorithm \"{0}\" returned the correct answer {1}% of the time", algorithm, score * 100);
            }
        }
    }
}
