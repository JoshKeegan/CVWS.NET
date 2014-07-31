/*
 * Computer Vision Wordsearch Solver
 * Shared Helpers
 * Histogram class
 * By Josh Keegan 04/04/2014
 * 
 * Note: This class could be made to work with types other than double, perhaps
 *  consider spending time looking at Generic Operators? http://www.yoda.arachsys.com/csharp/miscutil/usage/genericoperators.html
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.Maths.Statistics
{
    public class Histogram
    {
        //Private Vars
        private int numBins;

        //Public Vars
        public ICollection<double> Data { get; private set; }
        public uint[] Bins { get; private set; }
        public double MinBin { get; private set; }
        public double MaxBin { get; private set; }
        public int NumBins
        {
            get
            {
                return numBins;
            }
            set
            {
                //Validate Arg
                if(value < 1)
                {
                    throw new ArgumentOutOfRangeException("Must be at least one bin in a histogram");
                }
                else //Valid
                {
                    numBins = value;
                    update();
                }
            }
        }
        public uint NumValues { get; private set; } //The number of values stored in bins in the histogram
        public double BinWidth
        {
            get
            {
                double range = MaxBin - MinBin;
                return range / Bins.Length;
            }
        }
        
        //Constructors
        public Histogram(ICollection<double> values)
            : this(values, values.Min(), values.Max()) { }

        public Histogram(ICollection<double> values, int numBins)
            : this(values, values.Min(), values.Max(), numBins) { }

        public Histogram(ICollection<double> values, double min, double max)
            : this(values, min, max, (int)Math.Max(1, Math.Sqrt(values.Count))) { } //Select number of bins with simplest method (sqrt(n)) http://en.wikipedia.org/wiki/Histogram#Number_of_bins_and_width

        public Histogram(ICollection<double> values, double min, double max, int numBins)
        {
            this.Data = values;
            this.MinBin = min;
            this.MaxBin = max;
            this.NumBins = numBins;
        }

        /*
         * Helper Methods
         */
        protected virtual void update()
        {
            //Update Bins & NumValues
            Bins = new uint[NumBins];
            for (int i = 0; i < Bins.Length; i++)
            {
                Bins[i] = 0;
            }
            NumValues = 0;

            double binWidth = BinWidth;

            foreach (double d in Data)
            {
                //If the data is inside of the range of data the histogram is specified to work with
                if (d >= MinBin && d <= MaxBin)
                {
                    //Allign the data with the minimum Histogram bin starting at 0
                    double zeroAligned = d - MinBin;
                    int idx = (int)(zeroAligned / binWidth);

                    //If the idx is over the end of the array, then this value is equal to the max value, tag it onto the last bin
                    if (idx == Bins.Length)
                    {
                        idx--;
                    }

                    Bins[idx]++;
                    NumValues++;
                }
            }
        }
    }
}
