/*
 * Computer Vision Wordsearch Solver
 * Quantitative Evaluation
 * Neural Network Evaluator
 * By Josh Keegan 08/03/2014
 * Last Edit 10/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Neuro;
using AForge.Neuro.Learning;

using SharedHelpers.ClassifierInterfacing;

namespace QuantitativeEvaluation.Evaluators
{
    public class NeuralNetworkEvaluator : Evaluator<Network, double[], char> //Evaluates type Network, which accepts data in the form of a double[] and gives output in the form of a char
    {
        //Private variables
        private static string[] classLabels = null;

        //Public variables
        public ConfusionMatrix ConfusionMatrix { get; private set; }

        //Constructor
        public NeuralNetworkEvaluator(Network network)
            : base(network) 
        {
            if(classLabels == null)
            {
                classLabels = ClassifierHelpers.GetCharacterClassifierClassLabels();
            }

            ConfusionMatrix = new ConfusionMatrix(classLabels);
        }

        //Public Methods
        public override void Evaluate(double[][] evaluationData, char[] charEvaluationLabels)
        {
            double[][] desiredOutputs = NeuralNetworkHelpers.GetDesiredOutputForChars(charEvaluationLabels);

            //Perform the evaluation on the network, storing the result of each classification in the confusion matrix
            
            for(int i = 0; i < evaluationData.Length; i++)
            {
                double[] output = toEvaluate.Compute(evaluationData[i]);
                char outputChar = NeuralNetworkHelpers.GetMostLikelyChar(output);

                //Record the outcome of this classification in the confusion matrix
                int actualLabelIdx = outputChar - 'A';
                int predictedLabelIdx = charEvaluationLabels[i] - 'A';
                ConfusionMatrix.Add(actualLabelIdx, predictedLabelIdx);
            }
        }
    }
}
