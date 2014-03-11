/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Feature Extraction Discrete Cosine Transform - return the DCT values given bitmaps
 * By Josh Keegan 11/03/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Accord.Math;

using BaseObjectExtensions;
using SharedHelpers.Imaging;

namespace SharedHelpers.ClassifierInterfacing.FeatureExtraction
{
    public class FeatureExtractionDCT : FeatureExtractionAlgorithm
    {
        public override double[][] Extract(Bitmap[] charImgs)
        {
            double[][] charFeatures = new double[charImgs.Length][];

            for(int i = 0; i < charImgs.Length; i++)
            {
                double[,] doubleImg2d = Converters.ThresholdedBitmapTo2DDoubleArray(charImgs[i]);

                //Apply DCT
                CosineTransform.DCT(doubleImg2d);

                //Convert the 2D array to a flat one to be used as inputs to the neural network
                double[] doubleImg = doubleImg2d.ToSingleDimension();
                charFeatures[i] = doubleImg;
            }

            return charFeatures;
        }
    }
}
