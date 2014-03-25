/*
 * Dissertation CV Wordsearch Solver
 * Quantitative Evaluation
 * Program Entry Point
 * By Josh Keegan 08/03/2013
 * Last Edit 25/03/2014
 */

using System;
using System.Collections.Generic;
using System.IO;

using ImageMarkup;
using QuantitativeEvaluation.Evaluators;
using SharedHelpers.ClassifierInterfacing;
using SharedHelpers.ClassifierInterfacing.FeatureExtraction;
using System.Drawing;

namespace QuantitativeEvaluation
{
    class Program
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
        internal const string NEURAL_NETWORKS_PATH = CLASSIFIERS_PATH + "NeuralNetworks/";
        internal const string NEURAL_NETWORK_FILE_EXTENSION = ".networkWeights";
        private const bool EVALUATE_NEURAL_NETWORKS = false;
        private const bool EVALUATE_WORDSEARCH_ROTATION_CORRECTION = true;

        static void Main(string[] args)
        {
            //Initialise logging
            bool dirCreated = false;
            if (!Directory.Exists(LOGS_DIR_PATH))
            {
                dirCreated = true;
                System.IO.Directory.CreateDirectory(LOGS_DIR_PATH);
            }

            int logAttempt = 0;
            while(true)
            {
                string logName = String.Format("{0}/Quantitative_Evaluation_Log.{1}.{2:000}.log", LOGS_DIR_PATH, DateTime.Now.ToString("yyyy-MM-dd"), logAttempt);
                if (!System.IO.File.Exists(logName))
                {
                    Log.Initialise(logName, LOG_LEVEL);
                    Log.Info("Log Initialised");
                    if (dirCreated)
                    {
                        Log.Warn("Logs Directory was not found, creating . . .");
                    }
                    break;
                }
                logAttempt++;
            }
            //If the directories for storing Classifiers don't exist, make them now
            if (!Directory.Exists(CLASSIFIERS_PATH))
            {
                Log.Info("Classifiers Directory didn't exist, creating . . .");
                Directory.CreateDirectory(CLASSIFIERS_PATH);
            }
            if (!Directory.Exists(NEURAL_NETWORKS_PATH))
            {
                Log.Info("Neural Networks Path didn't exist, creating . . .");
                Directory.CreateDirectory(NEURAL_NETWORKS_PATH);
            }

            //Load the Wordsearch Database
            Log.Info("Loading Wordsearch Database . . .");
            ImageMarkupDatabase.LoadDatabase();
            Log.Info("Wordsearch Database Loaded");

            //TODO: Change to some pre-determined split rather than splitting at runtime
            //Split the Wordsearch Images into 3 groups: training, cross-validation & evaluation
            Log.Info("Splitting Wordsearch Image data into Training, Cross-Validation & Evaluation data sets");
            List<WordsearchImage> wordsearchImages = ImageMarkupDatabase.GetWordsearchImages();
            List<WordsearchImage> trainingWordsearchImages = new List<WordsearchImage>(wordsearchImages.Count / 3);
            List<WordsearchImage> crossValidationWordsearchImages = new List<WordsearchImage>(wordsearchImages.Count / 3);
            List<WordsearchImage> evaluationWordsearchImages = new List<WordsearchImage>(wordsearchImages.Count / 3);

            for (int i = 0; i < wordsearchImages.Count; i++)
            {
                if (i % 3 == 0)
                {
                    trainingWordsearchImages.Add(wordsearchImages[i]);
                }
                else if (i % 3 == 1)
                {
                    crossValidationWordsearchImages.Add(wordsearchImages[i]);
                }
                else
                {
                    evaluationWordsearchImages.Add(wordsearchImages[i]);
                }
            }
            Log.Info("Data split into Training, Cross-Validation & Evalutaion data");

            //If we're evaluating neural networks
            if (EVALUATE_NEURAL_NETWORKS)
            {
                //Evaluate all of the neural network combo's
                Log.Info("Starting to evaluate all Neural Networks . . .");
                IDictionary<string, NeuralNetworkEvaluator> neuralNetworkEvalResults =
                    EvaluateNeuralNetworks.evaluateNeuralNetworks(trainingWordsearchImages,
                    crossValidationWordsearchImages, evaluationWordsearchImages);
                Log.Info("Evaluation of all Neural Networks completed");

                //Write out the evaluation results
                if (!Directory.Exists(EVALUATION_RESULTS_DIR_PATH))
                {
                    Log.Info("Evaluation Results Directory didn't exist, creating . . .");
                    Directory.CreateDirectory(EVALUATION_RESULTS_DIR_PATH);
                }

                Log.Info("Writing out Neural Network Evaluation Results . . .");
                foreach (KeyValuePair<string, NeuralNetworkEvaluator> pair in neuralNetworkEvalResults)
                {
                    string networkName = pair.Key;
                    ConfusionMatrix cm = pair.Value.ConfusionMatrix;

                    Log.Info(String.Format("Network \"{0}\" misclassified {1}/{2}", networkName,
                        cm.NumMisclassifications, cm.TotalClassifications));

                    try
                    {
                        cm.WriteToCsv(EVALUATION_RESULTS_DIR_PATH + "/" + networkName + ".csv");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Error writing Confusion Matrix to file " + EVALUATION_RESULTS_DIR_PATH + "/" + networkName + ".csv");
                        Console.WriteLine(e);
                    }
                }
                Log.Info("Neural Network Evaluation results written out successfully");
            }
            
            //If we're evaluating Wordsearch rotation correction
            if(EVALUATE_WORDSEARCH_ROTATION_CORRECTION)
            {
                Log.Info("Starting to evaluate Wordsearch Rotation Correction");

                //Get the Feature Reduction Algorithm to be used
                Log.Info("Loading Feature Extraction Algorithm . . .");
                //TODO: Load a pre-trained feature reduction algorithm rather than training on the training data every time
                FeatureExtractionPCA featureExtractionAlgorithm = new FeatureExtractionPCA(); //Full PCA (no dimensionality reduction)
                Dictionary<char, List<Bitmap>> trainingData = CharData.GetCharData(trainingWordsearchImages);
                Bitmap[] trainingCharImgs;
                double[][] trainingOutput;
                CharData.GetNeuralNetworkBitmapsAndOutput(trainingData, out trainingCharImgs, out trainingOutput);
                featureExtractionAlgorithm.Train(trainingCharImgs);
                Log.Info("Feature Extraction Algorithm Loaded");

                //Get the classifier to be used
                Log.Info("Loading Classifier . . .");
                Classifier classifier = new AForgeActivationNeuralNetClassifier(featureExtractionAlgorithm, 
                    NEURAL_NETWORKS_PATH + "SingleLayer Sigmoid BkPropLearn PCAAllFeatures" + NEURAL_NETWORK_FILE_EXTENSION);
                Log.Info("Classifier Loaded");

                //Evaluate the wordsearch Image Rotation Correction
                double rotationCorrectionRate = EvaluateWordsearchRotationCorrection.Evaluate(evaluationWordsearchImages, classifier);
                Log.Info(String.Format("Wordsearch Rotation Correction returned the correct answer {0}% of the time", rotationCorrectionRate * 100));

                Log.Info("Wordsearch Rotation Correction Evaluation complete");
            }
        }
    }
}
