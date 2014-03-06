/*
 * Dissertation CV Wordsearch Solver
 * Dev Char Classification - Console app written during development of the first character classifier
 * Program Entry Point
 * By Josh Keegan 06/03/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Neuro;
using AForge.Neuro.Learning;

using ImageMarkup;
using SharedHelpers;
using SharedHelpers.Imaging;

namespace DevCharClassification
{
    class Program
    {
        //Constants
        private const int CHAR_WITH_WHITESPACE_WIDTH = 25;
        private const int CHAR_WITH_WHITESPACE_HEIGHT = 25;
        private const int CHAR_WITHOUT_WHITESPACE_WIDTH = 16;
        private const int CHAR_WITHOUT_WHITESPACE_HEIGHT = 16;
        private const int NUM_INPUT_VALUES = CHAR_WITHOUT_WHITESPACE_WIDTH * CHAR_WITHOUT_WHITESPACE_HEIGHT;
        private const int NUM_CLASSES = 26;
        private const double LEARNING_RATE = 0.5;
        private const double LEARNED_AT_ERROR = 0.5; //The error returned by the neural network. When less than this class as learned
        private const int MAX_LEARNING_ITERATIONS = 1000; //The maximum number of iterations to train the network for
        private const int ITERATIONS_PER_PROGRESS_UPDATE = 25;

        static void Main(string[] args)
        {
            Console.WriteLine("Loading Wordsearch Database . . .");
            //If the Image Markup Data base hasn't already been loaded
            if(!ImageMarkupDatabase.Loaded)
            {
                ImageMarkupDatabase.LoadDatabase();
            }
            Console.WriteLine("Wordsearch Database Loaded");

            //Split the data into 2 groups: training & evaluation
            Console.WriteLine("Splitting wordsearch image data into training and evaluation data");
            List<WordsearchImage> wordsearchImages = ImageMarkupDatabase.GetWordsearchImages();
            List<WordsearchImage> trainingWordsearchImages = new List<WordsearchImage>(wordsearchImages.Count / 2);
            List<WordsearchImage> evaluationWordsearchImages = new List<WordsearchImage>(wordsearchImages.Count / 2);

            for(int i = 0; i < wordsearchImages.Count; i++)
            {
                if(i % 2 == 0)
                {
                    trainingWordsearchImages.Add(wordsearchImages[i]);
                }
                else
                {
                    evaluationWordsearchImages.Add(wordsearchImages[i]);
                }
            }
            Console.WriteLine("Data split into training and evalutation groups");

            // Get the training data for the Neural Network
            Console.WriteLine("Loading & processing the character data (training)");
            Dictionary<char, List<double[]>> trainingData = getCharData(trainingWordsearchImages);
            Console.WriteLine("Loaded training character data");

            //Convert the training data into a format the Neural network accepts
            int numInputs = 0;
            foreach(List<double[]> arr in trainingData.Values)
            {
                numInputs += arr.Count;
            }

            Console.WriteLine("There are {0} training input character samples", numInputs);

            Console.WriteLine("Converting data to format for Neural Network . . .");
            double[][] input = new double[numInputs][];
            double[][] output = new double[numInputs][];
            int idx = 0;
            foreach(KeyValuePair<char, List<double[]>> entry in trainingData)
            {
                char c = entry.Key;
                List<double[]> images = entry.Value;

                double[] thisCharOutput = desiredOutputForChar(c);
                
                foreach(double[] image in images)
                {
                    input[idx] = image;
                    output[idx] = thisCharOutput;
                    idx++;
                }
            }
            Console.WriteLine("Conversion Complete");

            //Create the neural network
            BipolarSigmoidFunction sigmoidFunction = new BipolarSigmoidFunction(2.0f);
            ActivationNetwork neuralNet = new ActivationNetwork(sigmoidFunction, NUM_INPUT_VALUES, NUM_CLASSES);

            //Randomise the networks weights
            neuralNet.Randomize();

            //Create teacher that the network will use to learn the data
            BackPropagationLearning teacher = new BackPropagationLearning(neuralNet);
            teacher.LearningRate = LEARNING_RATE;

            //Make the network learn the data
            Console.WriteLine("Training the neural network . . .");
            double error;
            int iterNum = 0;
            do
            {
                error = teacher.RunEpoch(input, output);

                //Progress update
                if(iterNum % ITERATIONS_PER_PROGRESS_UPDATE == 0)
                {
                    Console.WriteLine("Learned for {0} iterations. Error: {1}", iterNum, error);
                }

                if(iterNum >= MAX_LEARNING_ITERATIONS)
                {
                    Console.WriteLine("Reached the maximum number of learning iterations ({0}), with error {1}", MAX_LEARNING_ITERATIONS, error);
                    break;
                }
                iterNum++;
            }
            while (error > LEARNED_AT_ERROR);

            Console.WriteLine("Data learned to an error of {0}", error);

            //Compute the number of characters in the input data that are being misclassified
            int numMisclassified = 0;

            for (int i = 0; i < input.Length; i++)
            {
                double[] actualOutput = neuralNet.Compute(input[i]);
                double[] desiredOutput = output[i];

                //Work out which neuron gave the highest probability and which one should have given the highest probability
                int actualMaxIdx = 0;
                int desiredMaxIdx = 0;

                for (int j = 1; j < actualOutput.Length; j++)
                {
                    if(actualOutput[j] > actualOutput[actualMaxIdx])
                    {
                        actualMaxIdx = j;
                    }
                    if(desiredOutput[j] > desiredOutput[desiredMaxIdx])
                    {
                        desiredMaxIdx = j;
                    }
                }

                //If the highest valued neuron wasn't the one it should have been, add one to the number of characters that would have been misclassified
                if(actualMaxIdx != desiredMaxIdx)
                {
                    numMisclassified++;
                }
            }

            Console.WriteLine("{0} / {1} characters from the training data would have been misclassified", numMisclassified, numInputs);
        }

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
                        if(charImg < 'A' || charImg > 'Z')
                        {
                            throw new Exception("Chars must be in range A-Z. Found " + charImg);
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

        //The output desired from the neural network for the specified character
        private static double[] desiredOutputForChar(char c)
        {
            int cIdx = c - 'A';
            double[] toRet = new double[NUM_CLASSES];

            for (int i = 0; i < toRet.Length; i++)
            {
                toRet[i] = i == cIdx ? 0.5f : -0.5f;
            }
            return toRet;
        }
    }
}
