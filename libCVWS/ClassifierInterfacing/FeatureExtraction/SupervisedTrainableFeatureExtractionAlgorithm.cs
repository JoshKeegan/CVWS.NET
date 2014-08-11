/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS
 * Supervised Trainable Feature Extraction Algorithm - abstract class
 * By Josh Keegan 12/03/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using libCVWS.Exceptions;

namespace libCVWS.ClassifierInterfacing.FeatureExtraction
{
    public abstract class SupervisedTrainableFeatureExtractionAlgorithm : FeatureExtractionAlgorithm
    {
        private bool trained = false;

        //Use the public methods here to perform checks about the training before then passing the call on to the child class in the protected DoBlah methods
        public void Train(Bitmap[] charImgs, char[] labels)
        {
            if (trained)
            {
                throw new TrainableFeatureExtractionAlgorithmException("Trainable feature extraction algorithm has already been trained");
            }
            trained = true;

            DoTrain(charImgs, labels);
        }

        protected abstract void DoTrain(Bitmap[] charImgs, char[] labels);

        public override double[][] Extract(Bitmap[] charImgs)
        {
            if (!trained)
            {
                throw new TrainableFeatureExtractionAlgorithmException("Trainable feature extraction algorithms must be trained before extracting data");
            }

            return DoExtract(charImgs);
        }

        protected abstract double[][] DoExtract(Bitmap[] charImgs);
    }
}
