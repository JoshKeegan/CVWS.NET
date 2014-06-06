/*
 * Dissertation CV Wordsearch Solver
 * Quantitative Evaluation
 * Evaluate Neural Networks
 * By Josh Keegan 11/03/2014
 * Last Edit 06/06/2014
 * 
 * Note that if the data has changed between runs, it won't work with trainiable feature extraction 
 *  techniques as the previous trained system will get overwritten (could be changed in the future 
 *  to keep the previous weights for comparison)
 */

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
    internal static class EvaluateNeuralNetworks
    {
        //Constants
        private const double LEARNING_RATE = 0.5;
        private const double LEARNED_AT_ERROR = 0.5; //The error returned by the neural network. When less than this class as learned
        private const int MAX_LEARNING_ITERATIONS = 1000; //The maximum number of iterations to train the network for
        private const int ITERATIONS_PER_PROGRESS_UPDATE = 25;
        private const int NUM_ITERATIONS_EQUAL_IMPLIES_PLATEAU = 50; //The number of previous iterations to record when evaluating a network in training on the cross-validation data
        //Used for detecting a plateau and detecting over-learning of data
        private const int NUM_NETWORKS_TO_TRAIN_FOR_CROSS_VALIDATION_COMPETITION = 10; //Use as 1000 to determine what is close to the maximum performance each network can give
        private const int PCA_NUM_FEATURES = 20;

        //evaluate all neural networks and feature extraction methods we're interested in & return their evaluation results
        internal static IDictionary<string, NeuralNetworkEvaluator> evaluateNeuralNetworks(List<WordsearchImage> trainingWordsearchImages,
            List<WordsearchImage> crossValidationWordsearchImages, List<WordsearchImage> evaluationWordsearchImages)
        {
            //Construct static feature extraction techniques (ones that don't learn the data) here, so they can be reused
            FeatureExtractionAlgorithm rawPixelFeatureExtraction = new FeatureExtractionPixelValues();
            FeatureExtractionAlgorithm dctFeatureExtraction = new FeatureExtractionDCT();

            /*
             * Load all required data
             */

            //Get the training data for the Neural Network
            Log.Info("Loading & processing the character training data");
            Dictionary<char, List<Bitmap>> trainingData = CharData.GetCharData(trainingWordsearchImages);
            Log.Info("Loaded training character data");

            //Convert the training data into a format the Neural network accepts
            Log.Info("Converting data to format for Neural Network . . .");
            Bitmap[] trainingCharImgs;
            double[][] output;
            CharData.GetNeuralNetworkBitmapsAndOutput(trainingData, out trainingCharImgs, out output);
            double[][] rawPixelValuesInput = rawPixelFeatureExtraction.Extract(trainingCharImgs);
            double[][] dctInput = dctFeatureExtraction.Extract(trainingCharImgs);

            //Create the non-static feature extraction algorithms & train them on the training data
            TrainableFeatureExtractionAlgorithm pcaFeatureExtractionAllFeatures = new FeatureExtractionPCA();
            pcaFeatureExtractionAllFeatures.Train(trainingCharImgs);
            TrainableFeatureExtractionAlgorithm pcaFeatureExtractionTopFeatures = new FeatureExtractionPCA(PCA_NUM_FEATURES);
            pcaFeatureExtractionTopFeatures.Train(trainingCharImgs);

            //Export the trained Feature Extraction Algorithms
            Log.Info("Saving Data for Trainable Feature Extraction Algorithms");

            //If the Trainable Feature Extraction Algorithms directory doesn't exist, create it . . .
            if(!Directory.Exists(Program.FEATURE_EXTRACTORS_PATH))
            {
                Log.Info("Feature Extractors Path didn't exist, creating . . .");
                Directory.CreateDirectory(Program.FEATURE_EXTRACTORS_PATH);
            }

            pcaFeatureExtractionAllFeatures.Save(Program.FEATURE_EXTRACTORS_PATH + Program.PCA_ALL_FEATURES_FILE_NAME + Program.FEATURE_EXTRACTORS_FILE_EXTENSION);
            pcaFeatureExtractionTopFeatures.Save(Program.FEATURE_EXTRACTORS_PATH + Program.PCA_TOP_FEATURES_FILE_NAME + Program.FEATURE_EXTRACTORS_FILE_EXTENSION);
            Log.Info("Data for Trainable Feature Extraction Algorithms saved");

            double[][] pcaAllFeaturesInput = pcaFeatureExtractionAllFeatures.Extract(trainingCharImgs);
            double[][] pcaTopFeaturesInput = pcaFeatureExtractionTopFeatures.Extract(trainingCharImgs);

            trainingCharImgs.DisposeAll(); //Dispose of all the training Bitmaps, freeing up memory
            Log.Info("Conversion Complete");
            Log.Info(String.Format("There are {0} training input character samples", rawPixelValuesInput.Length));

            //Load the cross-validation data for the neural network
            Log.Info("Loading & processing the character data (cross-validation)");
            Dictionary<char, List<Bitmap>> crossValidationData = CharData.GetCharData(crossValidationWordsearchImages);
            Log.Info("Loaded cross-validation character data");

            //Convert the evaluation data into a format the Neural network accepts
            Log.Info("Converting data to format for Neural Network . . .");
            Bitmap[] crossValCharImgs;
            double[][] crossValOutput;
            CharData.GetNeuralNetworkBitmapsAndOutput(crossValidationData, out crossValCharImgs, out crossValOutput);
            double[][] rawPixelValuesCossValInput = rawPixelFeatureExtraction.Extract(crossValCharImgs);
            double[][] dctCrossValInput = dctFeatureExtraction.Extract(crossValCharImgs);
            double[][] pcaAllFeaturesCrossValInput = pcaFeatureExtractionAllFeatures.Extract(crossValCharImgs);
            double[][] pcaTopFeaturesCrossValInput = pcaFeatureExtractionTopFeatures.Extract(crossValCharImgs);
            crossValCharImgs.DisposeAll(); //Dispose of all the cross-validation Bitmaps, freeing up memory
            Log.Info("Conversion Complete");
            Log.Info(String.Format("There are {0} cross-validation input character samples", rawPixelValuesCossValInput.Length));
            char[] crossValidationDataLabels = CharData.GetCharLabels(crossValidationData);

            //Load the evaluation data for the neural network
            Log.Info("Loading & processing the character data (evaluation)");
            Dictionary<char, List<Bitmap>> evaluationData = CharData.GetCharData(evaluationWordsearchImages);
            Log.Info("Loaded evaluation character data");

            //Convert the evaluation data into a format the Neural network accepts
            Log.Info("Converting data to format for Neural Network . . .");
            Bitmap[] evalCharImgs;
            double[][] evalOutput;
            CharData.GetNeuralNetworkBitmapsAndOutput(evaluationData, out evalCharImgs, out evalOutput);
            double[][] rawPixelValuesEvalInput = rawPixelFeatureExtraction.Extract(evalCharImgs);
            double[][] dctEvalInput = dctFeatureExtraction.Extract(evalCharImgs);
            double[][] pcaAllFeaturesEvaluationInput = pcaFeatureExtractionAllFeatures.Extract(evalCharImgs);
            double[][] pcaTopFeaturesEvaluationInput = pcaFeatureExtractionTopFeatures.Extract(evalCharImgs);
            evalCharImgs.DisposeAll(); //Dispose of all the bitmaps, freeing up memory
            Log.Info("Conversion Complete");
            Log.Info(String.Format("There are {0} evaluation input character samples", rawPixelValuesEvalInput.Length));
            char[] evaluationDataLabels = CharData.GetCharLabels(evaluationData);

            /*
             * Evaluate each Neural Network
             */

            //Run each network training process in it's own thread as they can take a while . . .
            ConcurrentDictionary<string, NeuralNetworkEvaluator> concurrentEvaluationResults =
                new ConcurrentDictionary<string, NeuralNetworkEvaluator>();

            ManualResetEvent[] doneEvents = new ManualResetEvent[4]; //Update to the number of algorithms to run in parallel

            Log.Info("Starting worker threads");

            doneEvents[0] = new ManualResetEvent(false);
            Task.Factory.StartNew(() =>
            {
                //Single layer Activation Network, Sigmoid Function, Back Propagation Learning on Raw Pixel Values
                string networkName = "SingleLayer Sigmoid BkPropLearn RawPxlVals";
                NeuralNetworkEvaluator singleLayerActivationSigmoidBackPropagationRawPixelEval =
                    evaluateSingleLayerActivationNetworkWithSigmoidFunctionBackPropagationLearning(
                    rawPixelValuesInput, output, rawPixelValuesCossValInput, crossValidationDataLabels,
                    rawPixelValuesEvalInput, evaluationDataLabels, LEARNING_RATE, networkName); //Use the default learning rate
                concurrentEvaluationResults.TryAdd(networkName,
                    singleLayerActivationSigmoidBackPropagationRawPixelEval);

                //Tell the main thread we're done
                doneEvents[0].Set();
            });

            doneEvents[1] = new ManualResetEvent(false);
            Task.Factory.StartNew(() =>
            {
                //Single layer activation network, Sigmoid Function, Back Propagation Learning on DCT
                string networkName = "SingleLayer Sigmoid BkPropLearn DCT";
                NeuralNetworkEvaluator singleLayerActivationSigmoidBackPropagationDCT =
                    evaluateSingleLayerActivationNetworkWithSigmoidFunctionBackPropagationLearning(
                    dctInput, output, dctCrossValInput, crossValidationDataLabels,
                    dctEvalInput, evaluationDataLabels, LEARNING_RATE, networkName); //Use the default learning rate
                concurrentEvaluationResults.TryAdd(networkName,
                    singleLayerActivationSigmoidBackPropagationDCT);

                //Tell the main thread we're done
                doneEvents[1].Set();
            });

            doneEvents[2] = new ManualResetEvent(false);
            Task.Factory.StartNew(() =>
            {
                //Single layer activation network, Sigmoid Function, Back Propagation Learning on PCA with all Features
                string networkName = "SingleLayer Sigmoid BkPropLearn PCAAllFeatures";
                NeuralNetworkEvaluator singleLayerActivationSigmoidBackPropagationPCAAllFeatures =
                    evaluateSingleLayerActivationNetworkWithSigmoidFunctionBackPropagationLearning(
                    pcaAllFeaturesInput, output, pcaAllFeaturesCrossValInput, crossValidationDataLabels,
                    pcaAllFeaturesEvaluationInput, evaluationDataLabels, LEARNING_RATE, networkName); //Use the default learning rate
                concurrentEvaluationResults.TryAdd(networkName,
                    singleLayerActivationSigmoidBackPropagationPCAAllFeatures);

                //Tell the main thread we're done
                doneEvents[2].Set();
            });

            doneEvents[3] = new ManualResetEvent(false);
            Task.Factory.StartNew(() =>
            {
                //Single layer activation network, Sigmoid Function, Back Propagation Learning on PCA, using onlt the top features
                string networkName = "SingleLayer Sigmoid BkPropLearn PCA" + PCA_NUM_FEATURES + "Features";
                NeuralNetworkEvaluator singleLayerActivationSigmoidBackPropagationPCATopFeatures =
                    evaluateSingleLayerActivationNetworkWithSigmoidFunctionBackPropagationLearning(
                    pcaTopFeaturesInput, output, pcaTopFeaturesCrossValInput, crossValidationDataLabels,
                    pcaTopFeaturesEvaluationInput, evaluationDataLabels, LEARNING_RATE, networkName); //Use the default learning rate
                concurrentEvaluationResults.TryAdd(networkName,
                    singleLayerActivationSigmoidBackPropagationPCATopFeatures);

                //Tell the main thread we're done
                doneEvents[3].Set();
            });

            //Wait for all threads to complete
            WaitHandle.WaitAll(doneEvents);
            Log.Info("All worker threads have completed");

            return concurrentEvaluationResults;
        }

        private static NeuralNetworkEvaluator evaluateSingleLayerActivationNetworkWithSigmoidFunctionBackPropagationLearning(
            double[][] input, double[][] output, double[][] crossValidationInput, char[] crossValidationDataLabels,
            double[][] evaluationInput, char[] evaluationDataLabels, double learningRate, string networkName)
        {
            //Create the neural Network
            BipolarSigmoidFunction sigmoidFunction = new BipolarSigmoidFunction(2.0f);
            ActivationNetwork neuralNet = new ActivationNetwork(sigmoidFunction, input[0].Length, ClassifierHelpers.NUM_CHAR_CLASSES);

            //Randomise the networks initial weights
            neuralNet.Randomize();

            //Create teacher that the network will use to learn the data (Back Propogation Learning technique used here)
            BackPropagationLearning teacher = new BackPropagationLearning(neuralNet);
            teacher.LearningRate = LEARNING_RATE;

            //Train the Network
            //trainNetwork(neuralNet, teacher, input, output, crossValidationInput, crossValidationDataLabels);
            //Train multiple networks, pick the one that performs best on the Cross-Validation data
            neuralNet = trainNetworksCompeteOnCrossValidation(neuralNet, teacher,
                input, output, crossValidationInput, crossValidationDataLabels);

            //Evaluate the network returned on the cross-validation data so it can be compared to the current best
            NeuralNetworkEvaluator crossValEvaluator = new NeuralNetworkEvaluator(neuralNet);
            crossValEvaluator.Evaluate(crossValidationInput, crossValidationDataLabels);

            //See if this network is better than the current best network of it's type
            //Try and load a previous network of this type
            string previousNetworkPath = Program.NEURAL_NETWORKS_PATH + networkName + Program.NEURAL_NETWORK_FILE_EXTENSION;
            string previousNetworkCMPath = Program.NEURAL_NETWORKS_PATH + networkName + ".csv";
            bool newBest = false;
            ActivationNetwork bestNetwork = neuralNet;
            if(File.Exists(previousNetworkPath))
            {
                //Load the previous network & evaluate it
                ActivationNetwork previous = ActivationNetwork.Load(previousNetworkPath) as ActivationNetwork;
                NeuralNetworkEvaluator prevCrossValEval = new NeuralNetworkEvaluator(previous);
                prevCrossValEval.Evaluate(crossValidationInput, crossValidationDataLabels);

                //If this network is better than the previous best, write it out as the new best
                if(prevCrossValEval.ConfusionMatrix.NumMisclassifications > crossValEvaluator.ConfusionMatrix.NumMisclassifications)
                {
                    Log.Info(String.Format("New best cross-validation score for network \"{0}\". Previous was {1}/{2}, new best is {3}/{2}",
                        networkName, prevCrossValEval.ConfusionMatrix.NumMisclassifications, prevCrossValEval.ConfusionMatrix.TotalClassifications,
                        crossValEvaluator.ConfusionMatrix.NumMisclassifications));

                    //Delete the old files
                    File.Delete(previousNetworkPath);
                    File.Delete(previousNetworkCMPath);

                    newBest = true;
                }
                else //The previous network is still the best
                {
                    Log.Info(String.Format("Existing \"{0}\" network performed better than new one. New network scored {1}/{2}, existing scored {3}/{2}",
                        networkName, crossValEvaluator.ConfusionMatrix.NumMisclassifications, crossValEvaluator.ConfusionMatrix.TotalClassifications,
                        prevCrossValEval.ConfusionMatrix.NumMisclassifications));
                    bestNetwork = previous;
                }
            }
            else //Otherwise there isn't a previous best
            {
                Log.Info(String.Format("No previous best record for network \"{0}\" . . .", networkName));
                newBest = true;
            }

            //If there is a new best to write out
            if(newBest)
            {
                Log.Info(String.Format("Writing out net best network of type\"{0}\"", networkName));
                neuralNet.Save(previousNetworkPath);
                crossValEvaluator.ConfusionMatrix.WriteToCsv(previousNetworkCMPath);
                Log.Info(String.Format("Finished writing out network \"{0}\"", networkName));
            }

            //Evaluate the best system on the evaluation data
            NeuralNetworkEvaluator evaluator = new NeuralNetworkEvaluator(bestNetwork);
            evaluator.Evaluate(evaluationInput, evaluationDataLabels);
            return evaluator;
        }

        //Train the network many times, with different initial values, evaluate them on the cross valiadtion data and select the best one
        private static ActivationNetwork trainNetworksCompeteOnCrossValidation(ActivationNetwork neuralNet, ISupervisedLearning teacher,
            double[][] input, double[][] output, double[][] crossValidationInput, char[] crossValidationDataLabels)
        {
            Log.Info(String.Format("Training {0} neural networks & picking the one that performs best on the cross-validation data . . .",
                NUM_NETWORKS_TO_TRAIN_FOR_CROSS_VALIDATION_COMPETITION));

            MemoryStream bestNetworkStream = new MemoryStream();
            uint bestNetworkNumMisclassified = uint.MaxValue;

            for (int i = 0; i < NUM_NETWORKS_TO_TRAIN_FOR_CROSS_VALIDATION_COMPETITION; i++)
            {
                Log.Info(String.Format("Training network {0}/{1}", (i + 1), NUM_NETWORKS_TO_TRAIN_FOR_CROSS_VALIDATION_COMPETITION));
                //Train a new network
                neuralNet.Randomize(); //Reset the weights to random values
                trainNetwork(neuralNet, teacher, input, output, crossValidationInput, crossValidationDataLabels);

                //Compare this new networks performance to our current best network
                NeuralNetworkEvaluator evaluator = new NeuralNetworkEvaluator(neuralNet);
                evaluator.Evaluate(crossValidationInput, crossValidationDataLabels);
                uint numMisclassified = evaluator.ConfusionMatrix.NumMisclassifications;

                if (numMisclassified < bestNetworkNumMisclassified)
                {
                    //This network performed better than out current best network, make this the new best

                    //Clear the Memory Stream storing the current best network
                    bestNetworkStream.SetLength(0);
                    //Save the network & update the best numMisclassified
                    neuralNet.Save(bestNetworkStream);
                    bestNetworkNumMisclassified = numMisclassified;
                }
            }

            Log.Info("Trained all networks and selected the best one");

            //Load up the network that performed best
            bestNetworkStream.Position = 0; //Read from the start of the stream
            ActivationNetwork bestNetwork = ActivationNetwork.Load(bestNetworkStream) as ActivationNetwork;
            return bestNetwork;
        }

        private static void trainNetwork(ActivationNetwork neuralNet, ISupervisedLearning teacher,
            double[][] input, double[][] output, double[][] crossValidationInput, char[] crossValidationDataLabels)
        {
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
            for (int i = 0; i < NUM_ITERATIONS_EQUAL_IMPLIES_PLATEAU; i++)
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
                    Log.Debug(String.Format("Learned for {0} iterations. Error: {1}", iterNum, error));
                }

                //Evaluate this network on the cross-validation data
                NeuralNetworkEvaluator crossValidationEvaluator = new NeuralNetworkEvaluator(neuralNet);
                crossValidationEvaluator.Evaluate(crossValidationInput, crossValidationDataLabels);
                uint networkNumMisclassified = crossValidationEvaluator.ConfusionMatrix.NumMisclassifications;
                Log.Debug(String.Format("Network misclassified {0} / {1} on the cross-validation data set", networkNumMisclassified,
                    crossValidationEvaluator.ConfusionMatrix.TotalClassifications));

                //Check if we've overlearned the data and performance on the cross-valiadtion data has dropped off
                if (networkNumMisclassified > prevNetworksNumMisclassified.Mean()) //Use the mean of the number of misclassification, as the actual number can move around a bit
                {
                    //Cross-Validation performance has dropped, reinstate the previous network & break
                    Log.Debug(String.Format("Network has started to overlearn the training data on iteration {0}. Using previous classifier.", iterNum));
                    prevNetworkStream.Position = 0; //Set head to start of stream
                    neuralNet = ActivationNetwork.Load(prevNetworkStream) as ActivationNetwork; //Read in the network
                    break;
                }

                //Clear the Memory Stream storing the previous network
                prevNetworkStream.SetLength(0);
                //Store this network & the number of characters it misclassified on the cross-validation data
                neuralNet.Save(prevNetworkStream);

                //This is now the previous network, update the number it misclassified
                prevNetworkNumMisclassified = networkNumMisclassified;
                prevNetworksNumMisclassified.Dequeue();
                prevNetworksNumMisclassified.Enqueue(prevNetworkNumMisclassified);

                //Check if the performance has plateaued
                if (prevNetworksNumMisclassified.Distinct().Count() == 1) //Allow for slight movement in performance here??
                {
                    //Cross-Validation performance has plateaued, use this network as the final one & break
                    Log.Debug(String.Format("Network performance on cross-validation data has plateaued on iteration {0}.", iterNum));
                    break;
                }

                //Check if we've performed the max number of iterations
                if (iterNum > MAX_LEARNING_ITERATIONS)
                {
                    Log.Debug(String.Format("Reached the maximum number of learning iterations ({0}), with error {1}", MAX_LEARNING_ITERATIONS, error));
                    break;
                }
                iterNum++;
            }
            while (error > LEARNED_AT_ERROR);

            Log.Info(String.Format("Data learned to an error of {0}", error));
        }
    }
}