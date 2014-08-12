/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS
 * Stats class
 * By Josh Keegan 22/04/2014
 * Last Edit 12/08/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BaseObjectExtensions;

namespace libCVWS.Maths.Statistics
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

        //Function to calculate the mean of a Collection of numbers
        public static double Mean(ICollection<uint> collection)
        {
            ulong sum = 0;
            foreach (uint x in collection)
            {
                sum += x;
            }
            return sum / (double)collection.Count;
        }

        //Stacks and Queues don't implement ICollection, so have seperate methods for them
        public static double Mean(Queue<uint> queue)
        {
            return Mean(queue.ToArray());
        }

        public static double Mean(Stack<uint> stack)
        {
            return Mean(stack.ToArray());
        }

        public static double Median(ICollection<uint> collection)
        {
            //Convert the collection to an array to be worked on
            uint[] arr = collection.ToArray();

            //Sort the array
            Array.Sort(arr);

            //If there is an odd number of elements, pick the middle one
            if (arr.Length % 2 == 1)
            {
                return arr[arr.Length / 2];
            }
            //Otherwise there is an even number of elements, return the mean of the middle 2
            else
            {
                uint a = arr[(arr.Length / 2) - 1];
                uint b = arr[arr.Length / 2];

                return (double)(a + b) / 2;
            }
        }
    }
}
