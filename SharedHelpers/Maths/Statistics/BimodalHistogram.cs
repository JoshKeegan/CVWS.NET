/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Bimodal Histogram class
 * By Josh Keegan 04/04/2014
 * Last Edit 05/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BaseObjectExtensions;

namespace SharedHelpers.Maths.Statistics
{
    public class BimodalHistogram : Histogram
    {
        //Public Vars
        public int ThresholdBinIdx { get; private set; }
        public double ThresholdValue //The Value of the Bin that it's best to threshold at
        {
            get
            {
                return MinBin + (ThresholdBinIdx * BinWidth);
            }
        }

        //Constructors
        public BimodalHistogram(ICollection<double> values)
            : base(values) { }

        public BimodalHistogram(ICollection<double> values, double min, double max)
            : base(values, min, max) { }

        public BimodalHistogram(ICollection<double> values, int numBins)
            : base(values, numBins) { }

        public BimodalHistogram(ICollection<double> values, double min, double max, int numBins)
            : base(values, min, max, numBins) { }

        /*
         * Helper Methods
         */
        protected override void update()
        {
            //Update the Histogram
            base.update();

            //Update the Threshold Bin Index
            ThresholdBinIdx = calculateThresholdBinIdx();
        }

        //Implementation of Otsu's Method as he proposed it (non-optimised) but based on his version maximising inter class variance
        //Based on the maths (NOT the optimised algorithm) from http://en.wikipedia.org/wiki/Otsu%27s_method#Method 
        //and http://web-ext.u-aizu.ac.jp/course/bmclass/documents/otsu1979.pdf
        private int calculateThresholdBinIdx()
        {
            double bestThresholdInterClassVariance = double.MinValue;
            int bestThreshold = -1;

            //Check all thresholds, searching for the max inter class variance
            for (int i = 0; i < Bins.Length; i++)
            {
                uint[] class1 = Bins.Take(i).ToArray();
                uint[] class2 = new uint[Bins.Length - i];
                for (int j = 0; j < class2.Length; j++)
                {
                    class2[j] = Bins[i + j];
                }

                double class1Probability = (double)class1.Sum(a => a) / NumValues;
                double class2Probability = (double)class2.Sum(a => a) / NumValues;

                double class1Mean = 0;
                for (int j = 0; j < class1.Length; j++)
                {
                    class1Mean += ((double)class1[j] / NumValues) * (((j + 0.5d) * BinWidth) + MinBin);
                }
                class1Mean /= class1Probability;

                double class2Mean = 0;
                for (int j = 0; j < class2.Length; j++)
                {
                    class2Mean += ((double)class2[j] / NumValues) * (((j + i + 0.5d) * BinWidth) + MinBin);
                }
                class2Mean /= class2Probability;

                double interClassVariance = class1Probability * class2Probability * Math.Pow((class1Mean - class2Mean), 2);

                if (interClassVariance > bestThresholdInterClassVariance)
                {
                    bestThresholdInterClassVariance = interClassVariance;
                    bestThreshold = i;
                }
            }
            return bestThreshold;
        }

        /*
         * This Optimised version of the function appears to be broken
         * TODO: Fix & Write Unit tests to identify problem to prevent it happening again
         */
        //Calculate the best threshold index by maximising the inter class variance
        //Idea based on Otsu's Method http://en.wikipedia.org/wiki/Otsu%27s_method 
        //and more efficient implementations of it's thresholding (so you don't need to 
        //compute means & variances of each class at each threshold)
        /*private int calculateThresholdBinIdxOptimised()
        {
            double bestThresholdBetweenClassVariance = double.MinValue;
            int bestThreshold = -1;

            //Calculate the mean value stored in this histogram (normalised to lie between 0..1) - used later so you 
            //  don't have to recalculate the means on each iteration
            double meanHistValue = 0;
            for(int i = 0; i < Bins.Length; i++)
            {
                meanHistValue += (i * BinWidth) * (Bins[i] / (double)NumValues); //Normalise to be in range 0..1
            }

            //Initial class probabilities
            double class1Probability = 0;
            double class2Probability = 1;

            //Initial class 1 mean value
            double class1Mean = 0;

            //Check all thresholds
            for(int i = 0; i < Bins.Length; i++)
            {
                //Calculate class 2 mean for this candidate threshold
                double class2Mean = (meanHistValue - (class1Mean * class1Probability)) / class2Probability;

                //Calculate the between class variance
                double betweenClassVariance = class1Probability * (1 - class1Probability) * Math.Pow(class1Mean - class2Mean, 2);

                //Check if we found a new threshold candidate
                if(betweenClassVariance > bestThresholdBetweenClassVariance)
                {
                    bestThresholdBetweenClassVariance = betweenClassVariance;
                    bestThreshold = i;
                }

                //Update the class probabilities & class 1 mean
                class1Mean *= class1Probability;
                
                class1Probability += Bins[i] / (double)NumValues; //Normalise by the no. of items in the histogram to get the amount the probability has lowered by
                class2Probability -= Bins[i] / (double)NumValues;

                class1Mean += i * (Bins[i] / (double)NumValues);

                if(class1Probability != 0)
                {
                    class1Mean /= class1Probability;
                }
            }

            return bestThreshold;
        }*/
    }
}
