/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Classifer class - abstract class for encapsulating classifiers (allowing for methods to use generic 
 *  classifiers rather than ones specific to a particular framework or of a specific type)
 * By Josh Keegan 25/03/2014
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
