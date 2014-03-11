/*
 * Dissertation CV Wordsearch Solver
 * Quantitative Evaluation
 * Program Entry Point
 * By Josh Keegan 08/03/2013
 * Last Edit 11/03/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using AForge.Imaging.Filters;
using AForge.Neuro;
using AForge.Neuro.Learning;

using ImageMarkup;
using SharedHelpers;
using SharedHelpers.ClassifierInterfacing;
using SharedHelpers.ClassifierInterfacing.FeatureExtraction;
using SharedHelpers.Exceptions;
using SharedHelpers.Imaging;
using QuantitativeEvaluation.Evaluators;
using BaseObjectExtensions;

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

        //Constants used by the evaluation of the Neural network (to be moved elsewhere)
        private const int CHAR_WITH_WHITESPACE_WIDTH = 20;
        private const int CHAR_WITH_WHITESPACE_HEIGHT = 25;
        private const int CHAR_WITHOUT_WHITESPACE_WIDTH = 12;
        private const int CHAR_WITHOUT_WHITESPACE_HEIGHT = 16;
        private const int NUM_INPUT_VALUES = CHAR_WITHOUT_WHITESPACE_WIDTH * CHAR_WITHOUT_WHITESPACE_HEIGHT;
        private const double LEARNING_RATE = 0.5;
        private const double LEARNED_AT_ERROR = 0.5; //The error returned by the neural network. When less than this class as learned
        private const int MAX_LEARNING_ITERATIONS = 1000; //The maximum number of iterations to train the network for
        private const int ITERATIONS_PER_PROGRESS_UPDATE = 25;
        private const string MISCLASSIFIED_IMAGES_DIR_PATH = "Misclassified";
        private const int NUM_ITERATIONS_EQUAL_IMPLIES_PLATEAU = 50; //The number of previous iterations to record when evaluating a network in training on the cross-validation data
                                                                     //Used for detecting a plateau and detecting over-learning of data

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

            //Evaluate a neural network with the inputs as the pixel values
            FeatureExtractionAlgorithm rawPixelFeatureExtraction = new FeatureExtractionPixelValues();
            evaluateNeuralNetwork(trainingWordsearchImages, crossValidationWordsearchImages, 
                evaluationWordsearchImages, rawPixelFeatureExtraction);
        }

        //Example usage: evaluate a Neural Network
        private static void evaluateNeuralNetwork(List<WordsearchImage> trainingWordsearchImages, 
            List<WordsearchImage> crossValidationWordsearchImages, List<WordsearchImage> evaluationWordsearchImages,
            FeatureExtractionAlgorithm featureExtraction)
        {
            //TODO: Evaluate for each feature reduction algorithm automatically?

            //Get the training data for the Neural Network
            Log.Info("Loading & processing the character training data");
            Dictionary<char, List<Bitmap>> trainingData = CharData.GetCharData(trainingWordsearchImages);
            Log.Info("Loaded training character data");

            //Convert the training data into a format the Neural network accepts
            Log.Info("Converting data to format for Neural Network . . .");
            double[][] input;
            Bitmap[] trainingCharImgs;
            double[][] output;
            CharData.GetNeuralNetworkBitmapsAndOutput(trainingData, out trainingCharImgs, out output);
            input = featureExtraction.Extract(trainingCharImgs);
            trainingCharImgs.DisposeAll(); //Dispose of all the training Bitmaps, freeing up memory
            Log.Info("Conversion Complete");
            Log.Info(String.Format("There are {0} training input character samples", input.Length));

            //Load the cross-validation data for the neural network
            Log.Info("Loading & processing the character data (cross-validation)");
            Dictionary<char, List<Bitmap>> crossValidationData = CharData.GetCharData(crossValidationWordsearchImages);
            Log.Info("Loaded cross-validation character data");

            //Convert the evaluation data into a format the Neural network accepts
            Log.Info("Converting data to format for Neural Network . . .");
            double[][] crossValInput;
            Bitmap[] crossValCharImgs;
            double[][] crossValOutput;
            CharData.GetNeuralNetworkBitmapsAndOutput(crossValidationData, out crossValCharImgs, out crossValOutput);
            crossValInput = featureExtraction.Extract(crossValCharImgs);
            crossValCharImgs.DisposeAll(); //Dispose of all the cross-validation Bitmaps, freeing up memory
            Log.Info("Conversion Complete");
            Log.Info(String.Format("There are {0} cross-validation input character samples", crossValInput.Length));
            char[] crossValidationDataLabels = CharData.GetCharLabels(crossValidationData);

            //TODO: Make N Neural networks here, randomise each ones weights and train them all, then select the one that performs best on 
            //  the cross-validation data??

            //Create the neural network
            BipolarSigmoidFunction sigmoidFunction = new BipolarSigmoidFunction(2.0f);
            ActivationNetwork neuralNet = new ActivationNetwork(sigmoidFunction, NUM_INPUT_VALUES, ClassifierHelpers.NUM_CHAR_CLASSES);

            //Randomise the networks initial weights
            neuralNet.Randomize();

            //Create teacher that the network will use to learn the data (Back Propogation Learning technique used here)
            BackPropagationLearning teacher = new BackPropagationLearning(neuralNet);
            teacher.LearningRate = LEARNING_RATE;

            //Make the network learn the data
            Log.Info("Training the neural network . . .");
            double error;

            //TODO: Store the previous NUM_ITERATIONS_EQUAL_IMPLIES_PLATEAU networks so in the event of over-learning, we can return to the best one
            //Use the cross-validation data to notice if the network starts to over-learn the data.
            //Store the previous network (before training) and check if the performance drops on the cross-validation data
            MemoryStream prevNetworkStream = new MemoryStream();
            uint prevNetworkNumMisclassified = uint.MaxValue;
            Queue<uint> prevNetworksNumMisclassified = new Queue<uint>(NUM_ITERATIONS_EQUAL_IMPLIES_PLATEAU);
            //Initialise the queue to be full of uint.MaxValue
            for(int i = 0; i < NUM_ITERATIONS_EQUAL_IMPLIES_PLATEAU; i++)
            {
                prevNetworksNumMisclassified.Enqueue(prevNetworkNumMisclassified);
            }

            int iterNum = 1;
            do
            {
                //Perform an iteration of training (calls teacher.Run() for each item in the array of inputs/outputs provided)
                error = teacher.RunEpoch(input, output);

                //Progress update
                if (iterNum % ITERATIONS_PER_PROGRESS_UPDATE == 0)
                {
                    Log.Info(String.Format("Learned for {0} iterations. Error: {1}", iterNum, error));
                }

                //Evaluate this network on the cross-validation data
                //Clear the Memory Stream storing the previous network
                prevNetworkStream.SetLength(0);
                //Store this network & the number of characters it misclassified on the cross-validation data
                neuralNet.Save(prevNetworkStream);
                NeuralNetworkEvaluator crossValidationEvaluator = new NeuralNetworkEvaluator(neuralNet);
                crossValidationEvaluator.Evaluate(crossValInput, crossValidationDataLabels);
                uint networkNumMisclassified = crossValidationEvaluator.ConfusionMatrix.NumMisclassifications;
                Log.Debug(String.Format("Network misclassified {0} / {1} on the cross-validation data set", networkNumMisclassified, 
                    crossValidationEvaluator.ConfusionMatrix.TotalClassifications));


                //Check if we've overlearned the data and performance on the cross-valiadtion data has dropped off
                if(networkNumMisclassified > prevNetworksNumMisclassified.Mean()) //Use the mean of the number of misclassification, as the actual number can move around a bit
                {
                    //Cross-Validation performance has dropped, reinstate the previous network & break
                    Log.Info(String.Format("Network has started to overlearn the training data on iteration {0}. Using previous classifier.", iterNum));
                    prevNetworkStream.Position = 0; //Set head to start of stream
                    neuralNet = ActivationNetwork.Load(prevNetworkStream) as ActivationNetwork; //Read in the network
                    break;
                }

                //This is now the previous network, update the number it misclassified
                prevNetworkNumMisclassified = networkNumMisclassified;
                prevNetworksNumMisclassified.Dequeue();
                prevNetworksNumMisclassified.Enqueue(prevNetworkNumMisclassified);

                //Check if the performance has plateaued
                if(prevNetworksNumMisclassified.Distinct().Count() == 1) //Allow for slight movement in performance here??
                {
                    //Cross-Validation performance has plateaued, use this network as the final one & break
                    Log.Info(String.Format("Network performance on cross-validation data has plateaued on iteration {0}.", iterNum));
                    break;
                }

                //Check if we've performed the max number of iterations
                if (iterNum > MAX_LEARNING_ITERATIONS)
                {
                    Log.Info(String.Format("Reached the maximum number of learning iterations ({0}), with error {1}", MAX_LEARNING_ITERATIONS, error));
                    break;
                }
                iterNum++;
            }
            while (error > LEARNED_AT_ERROR);

            Log.Info(String.Format("Data learned to an error of {0}", error));

            //Load the evaluation data for the neural network
            Log.Info("Loading & processing the character data (evaluation)");
            Dictionary<char, List<Bitmap>> evaluationData = CharData.GetCharData(evaluationWordsearchImages);
            Log.Info("Loaded evaluation character data");

            //Convert the evaluation data into a format the Neural network accepts
            Log.Info("Converting data to format for Neural Network . . .");
            double[][] evalInput;
            Bitmap[] evalCharImgs;
            double[][] evalOutput;
            CharData.GetNeuralNetworkBitmapsAndOutput(evaluationData, out evalCharImgs, out evalOutput);
            evalInput = featureExtraction.Extract(evalCharImgs);
            evalCharImgs.DisposeAll(); //Dispose of all the bitmaps, freeing up memory
            Log.Info("Conversion Complete");
            Log.Info(String.Format("There are {0} evaluation input character samples", evalInput.Length));

            //Evaluate the trained network on the evaluation data
            NeuralNetworkEvaluator evaluator = new NeuralNetworkEvaluator(neuralNet);
            char[] evaluationDataLabels = CharData.GetCharLabels(evaluationData);
            evaluator.Evaluate(evalInput, evaluationDataLabels);

            Log.Info(String.Format("{0} / {1} characters from the evaluation data were misclassified",
                evaluator.ConfusionMatrix.NumMisclassifications, evaluator.ConfusionMatrix.TotalClassifications));

            //Write out the Confusion Matrix so that we can inspect the reults in greater detail
            evaluator.ConfusionMatrix.WriteToCsv("neuralNetwork.csv");
        }
    }
}
