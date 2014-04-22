/*
 * Dissertation CV Wordsearch Solver
 * Quantitative Evaluation
 * Program Entry Point
 * By Josh Keegan 08/03/2013
 * Last Edit 22/04/2014
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
        private const bool EVALUATE_WORDSEARCH_ROTATION_CORRECTION = false;
        private const bool EVALUATE_WORDSEARCH_SEGMENTATION = false;
        private const bool EVALUATE_WORDSEARCH_RECOGNITION = true;

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

            //If we're evaluating Wordsearch Segmentation
            if(EVALUATE_WORDSEARCH_SEGMENTATION)
            {
                /*
                 * Note that here all Wordsearch Images are used for evaluation (essentially they are all in the evaluation data set)
                 * This is because the training & cross validation data haven't been used to develop the algorithms, meaning that they 
                 * are fresh data and can be used for evaluation
                 */

                Log.Info("Starting to evaluate Wordsearch Segmentation");

                //Evaluate by the number of rows and cols returned
                Dictionary<string, double> scoresByNumRowsCols = EvaluateWordsearchSegmentation.EvaluateByNumRowsAndCols(wordsearchImages);

                //Print out scores
                Log.Info("Scores for Evaluation by Number of Rows and Cols");
                printScores(scoresByNumRowsCols);

                Log.Info("Wordsearch Segmentation Evaluation complete");
            }

            //If we're evaluating Wordsearch Recognition
            if(EVALUATE_WORDSEARCH_RECOGNITION)
            {
                /*
                 * Note that here all Images are used for evaluation (essentially they are all in the evaluation data set)
                 * This is because the training & cross validation data haven't been used to develop the algorithms, meaning
                 * that they are fresh data and can be used for evaluation
                 */

                Log.Info("Starting to evaluate Wordsearch Recognition");

                Dictionary<string, double> scores = EvaluateWordsearchRecognition.EvaluateReturnsWordsearch(ImageMarkupDatabase.GetImages());

                //Print out scores
                Log.Info("Scores for Evaluation based on a single wordsearch returned (the best candidate)");
                printScores(scores);

                Log.Info("Wordsearch Recognition Evaluation Complete");
            }
        }

        private static void printScores(IDictionary<string, double> scores)
        {
            foreach(KeyValuePair<string, double> kvp in scores)
            {
                string algorithm = kvp.Key;
                double score = kvp.Value;

                Log.Info(String.Format("Algorithm \"{0}\" returned the correct answer {1}% of the time", algorithm, score * 100));
            }
        }
    }
}
