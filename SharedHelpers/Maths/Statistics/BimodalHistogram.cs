/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Bimodal Histogram class
 * By Josh Keegan 04/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.Maths.Statistics
{
    public class BimodalHistogram : Histogram
    {
        //Public Vars
        public int ThresholdBinIdx { get; private set; }

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

        private int calculateThresholdBinIdx()
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
    }
}
