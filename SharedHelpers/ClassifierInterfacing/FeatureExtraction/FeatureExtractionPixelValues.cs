/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Shared Helpers
 * Feature Extraction Pixel Values - return the pixel value given binarised bitmaps
 * By Josh Keegan 08/03/2014
 * Last Edit 11/03/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedHelpers.Imaging;

namespace SharedHelpers.ClassifierInterfacing.FeatureExtraction
{
    public class FeatureExtractionPixelValues : FeatureExtractionAlgorithm
    {
        public override double[][] Extract(Bitmap[] charImgs)
        {
            double[][] doubleCharImgs = new double[charImgs.Length][];

            for(int i = 0; i < charImgs.Length; i++)
            {
                double[] doubleImg = Converters.ThresholdedBitmapToDoubleArray(charImgs[i]);
                doubleCharImgs[i] = doubleImg;
            }

            return doubleCharImgs;
        }
    }
}
