/*
 * Computer Vision Wordsearch Solver
 * Shared Helpers
 * Stats class
 * By Josh Keegan 22/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BaseObjectExtensions;

namespace SharedHelpers.Maths.Statistics
{
    public static class Stats
    {
        //Variance. Uses Welfords Method (for std dev) - based on answer at http://stackoverflow.com/questions/895929/how-do-i-determine-the-standard-deviation-stddev-of-a-set-of-values
        public static double Variance(double[] arr)
        {
            double m = 0.0;
            double s = 0.0;
            int k = 1;
            foreach (double x in arr)
            {
                double tmpM = m;
                m += (x - tmpM) / k;
                s += (x - tmpM) * (x - m);
                k++;
            }
            return s / k; //Population variance. Divisor would be k - 1 for sample variance
        }

        public static double Variance(ICollection<double> collection)
        {
            double[] arr = collection.ToArray();
            return Variance(arr);
        }

        public static double Variance(ICollection<uint> collection)
        {
            double[] arr = collection.ToDoubleArr();
            return Variance(arr);
        }

        public static double Variance(ICollection<int> collection)
        {
            double[] arr = collection.ToDoubleArr();
            return Variance(arr);
        }

        public static double StandardDeviation(double[] arr)
        {
            return Math.Sqrt(Variance(arr));
        }

        public static double StandardDeviation(ICollection<double> collection)
        {
            double[] arr = collection.ToArray();
            return StandardDeviation(arr);
        }

        public static double StandardDeviation(ICollection<uint> collection)
        {
            double[] arr = collection.ToDoubleArr();
            return StandardDeviation(arr);
        }

        public static double StandardDeviation(ICollection<int> collection)
        {
            double[] arr = collection.ToDoubleArr();
            return StandardDeviation(arr);
        }
    }
}
