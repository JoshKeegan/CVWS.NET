/*
 * Dissertation CV Wordsearch Solver
 * Quantitative Evaluation
 * Program Entry Point
 * By Josh Keegan 08/03/2013
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
using SharedHelpers.Exceptions;
using SharedHelpers.Imaging;
using QuantitativeEvaluation.Evaluators;
using QuantitativeEvaluation.FeatureReduction;

namespace QuantitativeEvaluation
{
    class Program
    {
        //Constants
        private const string LOGS_DIR_PATH = "logs";
        private const LogLevel LOG_LEVEL = LogLevel.All;

        //Constants used by the evaluation of the Neural network (to be moved elsewhere)
        private const int CHAR_WITH_WHITESPACE_WIDTH = 20;
        private const int CHAR_WITH_WHITESPACE_HEIGHT = 25;
        private const int CHAR_WITHOUT_WHITESPACE_WIDTH = 12;
        private const int CHAR_WITHOUT_WHITESPACE_HEIGHT = 16;
        private const int NUM_INPUT_VALUES = CHAR_WITHOUT_WHITESPACE_WIDTH * CHAR_WITHOUT_WHITESPACE_HEIGHT;
        private const double LEARNING_RATE = 0.5;
        private const double LEARNED_AT_ERROR = 0.5; //The error returned by the neural network. When less than this class as learned
        private const int MAX_LEARNING_ITERATIONS = 100; //The maximum number of iterations to train the network for
        private const int ITERATIONS_PER_PROGRESS_UPDATE = 25;
        private const string MISCLASSIFIED_IMAGES_DIR_PATH = "Misclassified";

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

            //Evaluate a neural network
            evaluateNeuralNetwork(trainingWordsearchImages, crossValidationWordsearchImages, evaluationWordsearchImages);
        }

        //Example usage: evaluate a Neural Network
        private static void evaluateNeuralNetwork(List<WordsearchImage> trainingWordsearchImages, 
            List<WordsearchImage> crossValidationWordsearchImages, List<WordsearchImage> evaluationWordsearchImages)
        {
            //TODO: Evaluate for each feature reduction algorithm automatically?

            //Get the training data for the Neural Network
            Log.Info("Loading & processing the character training data");
            Dictionary<char, List<double[]>> trainingData = getCharData(trainingWordsearchImages);
            Log.Info("Loaded training character data");

            //Convert the training data into a format the Neural network accepts
            Log.Info("Converting data to format for Neural Network . . .");
            double[][] input;
            double[][] output;
            convertDataToNeuralNetworkFormat(trainingData, out input, out output);
            Log.Info("Conversion Complete");
            Log.Info(String.Format("There are {0} training input character samples", input.Length));

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
            int iterNum = 0;
            do
            {
                error = teacher.RunEpoch(input, output);

                //Progress update
                if (iterNum % ITERATIONS_PER_PROGRESS_UPDATE == 0)
                {
                    Log.Info(String.Format("Learned for {0} iterations. Error: {1}", iterNum, error));
                }

                if (iterNum >= MAX_LEARNING_ITERATIONS)
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
            Dictionary<char, List<double[]>> evaluationData = getCharData(evaluationWordsearchImages);
            Log.Info("Loaded evaluation character data");

            //Convert the evaluation data into a format the Neural network accepts
            Log.Info("Converting data to format for Neural Network . . .");
            double[][] evalInput;
            double[][] evalOutput;
            convertDataToNeuralNetworkFormat(evaluationData, out evalInput, out evalOutput);
            Log.Info("Conversion Complete");
            Log.Info(String.Format("There are {0} evaluation input character samples", evalInput.Length));

            //Evaluate the trained network on the evaluation data
            NeuralNetworkEvaluator evaluator = new NeuralNetworkEvaluator(neuralNet);
            char[] evaluationDataLabels = getEvaluationDataLabels(evaluationData);
            evaluator.Evaluate(evalInput, evaluationDataLabels);

            Log.Info(String.Format("{0} / {1} characters from the evaluation data were misclassified",
                evaluator.ConfusionMatrix.NumMisclassifications, evaluator.ConfusionMatrix.TotalClassifications));

            //Write out the Confusion Matrix so that we can inspect the reults in greater detail
            evaluator.ConfusionMatrix.WriteToCsv("neuralNetwork.csv");
        }

        //TODO: Refactor. Complete rewrite needed, should be multiple methods, clearer & elsewhere
        private static Dictionary<char, List<double[]>> getCharData(List<WordsearchImage> wordsearchImages)
        {
            //Make some objects now for reuse later
            BradleyLocalThresholding bradleyLocalThreshold = new BradleyLocalThresholding();
            ExtractBiggestBlob extractBiggestBlob = new ExtractBiggestBlob();
            ResizeNearestNeighbor resize = new ResizeNearestNeighbor(CHAR_WITHOUT_WHITESPACE_WIDTH, CHAR_WITHOUT_WHITESPACE_HEIGHT);
            Invert invert = new Invert();

            //Construct Data Structure to be returned
            Dictionary<char, List<double[]>> data = new Dictionary<char, List<double[]>>();

            //Make a blank entry for each valid char
            for (int i = (int)'A'; i <= (int)'Z'; i++)
            {
                char c = (char)i;
                List<double[]> charImgs = new List<double[]>();
                data.Add(c, charImgs);
            }

            foreach (WordsearchImage wordsearchImage in wordsearchImages)
            {
                Bitmap[,] rawCharImages = wordsearchImage.GetCharBitmaps(CHAR_WITH_WHITESPACE_WIDTH, CHAR_WITH_WHITESPACE_HEIGHT);

                for (int i = 0; i < rawCharImages.GetLength(0); i++)
                {
                    for (int j = 0; j < rawCharImages.GetLength(1); j++)
                    {
                        //Greyscale
                        Bitmap greyImg = Grayscale.CommonAlgorithms.BT709.Apply(rawCharImages[i, j]); //Use the BT709 (HDTV spec) for RBG weights

                        //Bradley Local Thresholding
                        bradleyLocalThreshold.ApplyInPlace(greyImg);

                        //Invert the image (required for blob detection)
                        invert.ApplyInPlace(greyImg);

                        //TODO: The biggest blob might be something else (when in a corner of a bounded wordsearch it's often the 2 lines making a square). Account for this
                        //Extract the largest blob (the character)
                        Bitmap charWithoutWhitespace = extractBiggestBlob.Apply(greyImg);

                        //Resize the image of the char without whitespace to some normalised size (using nearest neighbour because we've already thresholded)
                        Bitmap normalisedCharWithoutWhitespace = resize.Apply(charWithoutWhitespace);

                        //Convert Bitmap to double[,] (+-0.5) (what's used to train the neural network)
                        double[] doubleImg = Converters.ThresholdedBitmapToDoubleArray(normalisedCharWithoutWhitespace);
                        char charImg = wordsearchImage.Wordsearch.Chars[i, j];

                        //Check the char is valid
                        if (charImg < 'A' || charImg > 'Z')
                        {
                            throw new UnexpectedClassifierOutputException("Chars must be in range A-Z. Found " + charImg);
                        }

                        data[charImg].Add(doubleImg);

                        //Clean up
                        rawCharImages[i, j].Dispose();
                        greyImg.Dispose();
                        charWithoutWhitespace.Dispose();
                        normalisedCharWithoutWhitespace.Dispose();
                    }
                }
            }

            return data;
        }

        private static char[] getEvaluationDataLabels(Dictionary<char, List<double[]>> data)
        {
            int numInputs = 0;
            foreach (List<double[]> arr in data.Values)
            {
                numInputs += arr.Count;
            }

            char[] labels = new char[numInputs];
            int idx = 0;
            foreach (KeyValuePair<char, List<double[]>> entry in data)
            {
                char c = entry.Key;

                for (int i = 0; i < entry.Value.Count; i++)
                {
                    labels[idx] = c;
                    idx++;
                }
            }

            return labels;
        }

        private static void convertDataToNeuralNetworkFormat(Dictionary<char, List<double[]>> data, out double[][] input, out double[][] output)
        {
            int numInputs = 0;
            foreach (List<double[]> arr in data.Values)
            {
                numInputs += arr.Count;
            }

            input = new double[numInputs][];
            output = new double[numInputs][];
            int idx = 0;
            foreach (KeyValuePair<char, List<double[]>> entry in data)
            {
                char c = entry.Key;
                List<double[]> images = entry.Value;

                double[] thisCharOutput = NeuralNetworkHelpers.GetDesiredOutputForChar(c);

                foreach (double[] image in images)
                {
                    input[idx] = image;
                    output[idx] = thisCharOutput;
                    idx++;
                }
            }
        }
    }
}
