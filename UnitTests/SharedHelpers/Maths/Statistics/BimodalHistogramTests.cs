/*
 * Dissertation CV Wordsearch Solver
 * Unit Tests
 * SharedHelpers.Maths.Statistics.BimodalHistogram Tests
 * By Josh Keegan 04/04/2014
 */

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedHelpers.Maths.Statistics;

namespace UnitTests.SharedHelpers.Maths.Statistics
{
    [TestClass]
    public class BimodalHistogramTests
    {
        /*
         * Test Threshold
         */
        [TestMethod]
        public void TestThreshold1()
        {
            //Test finding the threshold in data where this is trivial
            double[] data = { 1, 3 };
            BimodalHistogram hist = new BimodalHistogram(data, 1, 3, 3);

            //Check bins are as expected
            uint[] expectedBins = { 1, 0, 1 };
            CollectionAssert.AreEqual(expectedBins, hist.Bins);

            //Check the threshold index selected 
            int expected = 1;
            Assert.AreEqual(expected, hist.ThresholdBinIdx);
        }
    }
}
