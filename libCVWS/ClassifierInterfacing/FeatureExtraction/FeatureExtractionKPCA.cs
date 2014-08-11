/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Feature Extraction Kernel Principal Component Analysis - return the KPCA values given bitmaps
 * By Josh Keegan 11/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Accord.Statistics;
using Accord.Statistics.Analysis;
using Accord.Statistics.Kernels;

using SharedHelpers.Imaging;

namespace SharedHelpers.ClassifierInterfacing.FeatureExtraction
{
    public class FeatureExtractionKPCA : TrainableFeatureExtractionAlgorithm
    {
        //Private variables
        private KernelPrincipalComponentAnalysis kpca;
        private int? numDimensions = null;
        IKernel kernel;

        //Constructors
        public FeatureExtractionKPCA(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public FeatureExtractionKPCA(IKernel kernel, int numDimensions)
            : this(kernel)
        {
            this.numDimensions = numDimensions;
        }

        //Protected methods (called by the public methods in parent class)
        protected override void DoTrain(System.Drawing.Bitmap[] charImgs)
        {
            double[][] input = charImgs.Select(img => Converters.ThresholdedBitmapToDoubleArray(img)).ToArray();

            kpca = new KernelPrincipalComponentAnalysis(input, kernel, AnalysisMethod.Center);
            kpca.Compute();
        }

        protected override double[][] DoExtract(System.Drawing.Bitmap[] charImgs)
        {
            double[][] input = charImgs.Select(img => Converters.ThresholdedBitmapToDoubleArray(img)).ToArray();

            double[][] components;

            //If returning all dimensions
            if(numDimensions == null)
            {
                components = kpca.Transform(input);
            }
            else
            {
                components = kpca.Transform(input, (int)numDimensions);
            }

            return components;
        }
    }
}
