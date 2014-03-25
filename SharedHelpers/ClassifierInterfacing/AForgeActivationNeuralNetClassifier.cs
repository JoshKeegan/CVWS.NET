/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * AForge Activation Neural Network Classifier Encapsulation class
 * By Josh Keegan 25/03/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Neuro;

using SharedHelpers.ClassifierInterfacing.FeatureExtraction;

namespace SharedHelpers.ClassifierInterfacing
{
    public class AForgeActivationNeuralNetClassifier : Classifier
    {
        //Private Variables
        private ActivationNetwork network;

        //Constuctor
        public AForgeActivationNeuralNetClassifier(FeatureExtractionAlgorithm featureExtractionAlgorithm, string networkPath)
            : base(featureExtractionAlgorithm)
        {
            Load(networkPath);
        }

        //Public Methods
        public override void Load(string path)
        {
            network = ActivationNetwork.Load(path) as ActivationNetwork;
        }

        public override double[] Classify(double[] charData)
        {
            return network.Compute(charData);
        }
    }
}
