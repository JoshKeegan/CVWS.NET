/*
 * Dissertation Cv Wordsearch Solver
 * Shared Helpers
 * Feature Extraction Multiclass (>2) Linear Discriminant Analysis
 * By Josh Keegan 12/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Accord.Statistics;
using Accord.Statistics.Analysis;

using SharedHelpers.Imaging;

namespace SharedHelpers.ClassifierInterfacing.FeatureExtraction
{
    public class FeatureExtractionLDA : SupervisedTrainableFeatureExtractionAlgorithm
    {
        //Private variables
        private LinearDiscriminantAnalysis lda;
        private int? numDimensions = null;

        //Constructors
        public FeatureExtractionLDA() { }

        //Dimensionality Reduction constuctor - return top n dimensions/features when doing feature extraction
        public FeatureExtractionLDA(int numDimensions)
        {
            this.numDimensions = numDimensions;
        }

        //Protected methods (called by the public methods in parent class)
        protected override void DoTrain(System.Drawing.Bitmap[] charImgs, char[] chars)
        {
            double[][] input = charImgs.Select(img => Converters.ThresholdedBitmapToDoubleArray(img)).ToArray();
            int[] labels = getClassLabels(chars);

            lda = new LinearDiscriminantAnalysis(input, labels);
            lda.Compute();
        }

        protected override double[][] DoExtract(System.Drawing.Bitmap[] charImgs)
        {
            double[][] input = charImgs.Select(img => Converters.ThresholdedBitmapToDoubleArray(img)).ToArray();

            double[][] components;

            //If returning all dimensions
            if (numDimensions == null)
            {
                components = lda.Transform(input);
            }
            else
            {
                components = lda.Transform(input, (int)numDimensions);
            }

            return components;
        }

        //Private Helpers
        private static int[] getClassLabels(char[] chars)
        {
            return chars.Select(c => (int)(c - 'A')).ToArray();
        }
    }
}
