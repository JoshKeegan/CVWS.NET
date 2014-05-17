/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Feature Extraction Principal Components Analysis - return the PCA values given bitmaps
 * By Josh Keegan 11/03/2014
 * Last Edit 17/05/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Accord.Statistics;
using Accord.Statistics.Analysis;

using SharedHelpers.Imaging;

namespace SharedHelpers.ClassifierInterfacing.FeatureExtraction
{
    [Serializable]
    public class FeatureExtractionPCA : TrainableFeatureExtractionAlgorithm, ISerializable
    {
        //Private variables
        private PrincipalComponentAnalysis pca;
        private int? numDimensions = null;

        //Constuctors
        public FeatureExtractionPCA() { }

        //Dimensionality reduction constructor - return top n dimensions/features when doing feature extraction
        public FeatureExtractionPCA(int numDimensions)
        {
            this.numDimensions = numDimensions;
        }

        //Deserialization constructor
        public FeatureExtractionPCA(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            pca = (PrincipalComponentAnalysis)info.GetValue("pca", typeof(PrincipalComponentAnalysis));
            numDimensions = (int?)info.GetValue("numDimensions", typeof(int?));
        }

        //Protected methods (called by the public methods in parent class)
        protected override void DoTrain(Bitmap[] charImgs)
        {
            double[][] input = convertDataToPCAInputFormat(charImgs);

            pca = new PrincipalComponentAnalysis(input, AnalysisMethod.Center);
            pca.Compute();
        }

        protected override double[][] DoExtract(Bitmap[] charImgs)
        {
            double[][] input = convertDataToPCAInputFormat(charImgs);

            double[][] components;

            //If returning all dimensions
            if(numDimensions == null)
            {
                components = pca.Transform(input);
            }
            else
            {
                components = pca.Transform(input, (int)numDimensions);
            }

            return components;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("pca", pca);
            info.AddValue("numDimensions", numDimensions);

            //Save out any data on the base object
            base.GetObjectData(info, context);
        }

        //Private Helpers
        private static double[][] convertDataToPCAInputFormat(Bitmap[] charImgs)
        {
            //Convert each Bitmap to a double[] to be used as input to the PCA algorithm
            double[][] pcaInput = new double[charImgs.Length][];

            for (int i = 0; i < charImgs.Length; i++)
            {
                pcaInput[i] = Converters.ThresholdedBitmapToDoubleArray(charImgs[i]);
            }

            return pcaInput;
        }
    }
}