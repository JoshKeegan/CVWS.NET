/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS
 * Percentile class
 * By Josh Keegan 05/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.Maths.Statistics
{
    public class Percentile
    {
        //Public vars
        public double[] Data { get; private set; }

        //Constructor
        public Percentile(ICollection<double> values)
        {
            //Validation
            if (values == null || values.Count == 0)
            {
                throw new ArgumentOutOfRangeException("Collection must contain value(s)");
            }

            //Convert the collection to an array to make percentile related calculations easier
            this.Data = values.ToArray();
            Array.Sort(this.Data);
        }

        //Public Methods
        public double CalculatePercentile(double percentile)
        {
            if (percentile < 0 || percentile > 100)
            {
                throw new ArgumentOutOfRangeException("Percentile must be in the range 0..100 to be valid");
            }

            //Determine the real (floating point) "index" to access.
            //explanation: Get position to lookup between 0..1, then multiply it by the number 
            //of elements in the array to get it into the range 0..arr.Length
            //This leaves the range being 1 larger than the array length, which is correct
            //(think of there being 4 values in an array and you trying to find the three quartile values,
            //the quartiles need to slot between the values rather than on the indices)
            //Finally, subtract 1/2 to slot the index into place (e.g. between the values in the quartiles example)
            double realIndex = ((percentile / 100) * Data.Length) - 0.5;

            //Keep the real index within valid array index bounds, the algorithm need not account for anything outside the known range of values
            realIndex = Math.Max(0, realIndex);
            realIndex = Math.Min(realIndex, Data.Length - 1);

            //Use linear interpolation between indices to return a better approximation of the percentile
            int lowerIndex = (int)Math.Floor(realIndex);
            int upperIndex = (int)Math.Ceiling(realIndex);
            double pointBetweenIndexes = realIndex % 1;

            return linearlyInterpolate(Data[lowerIndex], Data[upperIndex], pointBetweenIndexes);
        }

        public double CalculateRank(double value)
        {
            //Calculate rank based on the standard formula here http://en.wikipedia.org/wiki/Percentile_rank
            
            //Count the num values less than this & the same as the supplied value
            int numLess = 0;
            int numSame = 0;
            foreach(double d in Data)
            {
                if(d < value)
                {
                    numLess++;
                }
                else if(d == value)
                {
                    numSame++;
                }
                else //Larger, since the array is ordered there's no need to look any further
                {
                    break;
                }
            }

            return ((numLess + (numSame * 0.5d)) / Data.Length) * 100;
        }

        /*
         * Helpers
         */
        //Linearly interpolate between two values, given a point between them to find (in range 0..1)
        private static double linearlyInterpolate(double a, double b, double point)
        {
            //Let a be the small and b be the big number
            if (a > b)
            {
                //Swap the numbers
                double c = a;
                a = b;
                b = c;
            }

            double diff = b - a;

            double interVal = diff * point;
            return a + interVal;
        }
    }
}
