/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS
 * Feature Extraction Algorithm - abstract class 
 * By Josh Keegan 08/03/2014
 * Last Edit 25/03/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.ClassifierInterfacing.FeatureExtraction
{
    public abstract class FeatureExtractionAlgorithm
    {
        //Method is non-static to allow for feature reduction algorithms that learn what features are best to extract on the training data
        //The best features can be stored local to the instance this way
        public abstract double[][] Extract(Bitmap[] charImgs);

        //Method to Extract a single Bitmap
        public double[] Extract(Bitmap charImg)
        {
            Bitmap[] charImgs = { charImg };
            double[][] data = Extract(charImgs);
            return data[0];
        }
    }
}
