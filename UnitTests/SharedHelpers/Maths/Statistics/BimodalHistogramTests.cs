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
         * Test Threshold Bin Index
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

        [TestMethod]
        public void TestThreshold2()
        {
            //Test finding threshold in data similar to two gaussians not quite fully separated
            double[] data = { 1, 2, 2, 3, 3, 3, 3, 4, 4, 5, 5, 5, 5, 6, 6, 7 };
            BimodalHistogram hist = new BimodalHistogram(data, 1, 7, 7);

            //Check bins are as expected
            uint[] expectedBins = { 1, 2, 4, 2, 4, 2, 1 };
            CollectionAssert.AreEqual(expectedBins, hist.Bins);

            //Check the threshold index selected
            int expected = 3;
            Assert.AreEqual(expected, hist.ThresholdBinIdx);
        }

        /*
         * Test Threshold Value
         */
        [TestMethod]
        public void TestThresholdValue1()
        {
            //Test finding the value of the threshold in data where this is trivial
            double[] data = { 1, 3 };
            BimodalHistogram hist = new BimodalHistogram(data, 1, 4, 3);

            //Check bins are as expected
            uint[] expectedBins = { 1, 0, 1 };
            CollectionAssert.AreEqual(expectedBins, hist.Bins);

            //Check the threshold value selected 
            double expected = 2;
            Assert.AreEqual(expected, hist.ThresholdValue);
        }
    }
}
