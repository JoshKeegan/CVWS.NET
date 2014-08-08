/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Shared Helpers
 * Classifer class - abstract class for encapsulating classifiers (allowing for methods to use generic 
 *  classifiers rather than ones specific to a particular framework or of a specific type)
 * By Josh Keegan 25/03/2014
 * Last Edit 13/05/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedHelpers.ClassifierInterfacing.FeatureExtraction;
using SharedHelpers.Exceptions;

namespace SharedHelpers.ClassifierInterfacing
{
    public abstract class Classifier
    {
        protected FeatureExtractionAlgorithm featureExtractionAlgorithm = null;

        //Constructors
        public Classifier(FeatureExtractionAlgorithm featureExtractionAlgorithm)
        {
            this.featureExtractionAlgorithm = featureExtractionAlgorithm;
        }

        //Public Methods
        public abstract void Load(String path);

        //Perform classification on a 2D array of images (returns probabilities for each possible class for each image)
        public double[][][] Classify(Bitmap[,] charImgs)
        {
            double[][][] toRet = new double[charImgs.GetLength(0)][][];

            for (int i = 0; i < toRet.Length; i++) //Col
            {
                toRet[i] = new double[charImgs.GetLength(1)][];

                for (int j = 0; j < toRet[i].Length; j++) //Row
                {
                    toRet[i][j] = this.Classify(charImgs[i, j]);
                }
            }

            return toRet;
        }

        public double[] Classify(Bitmap charImg)
        {
            if(featureExtractionAlgorithm == null)
            {
                throw new MissingFeatureExtractionAlgorithmException("Feature Extraction Algorithm required in order to classify a Bitmap");
            }

            double[] charData = featureExtractionAlgorithm.Extract(charImg);

            return Classify(charData);
        }

        public abstract double[] Classify(double[] charData);
    }
}
