/*
 * Dissertation CV Wordsearch Solver
 * Unit Tests
 * SharedHelpers.Maths.Statistics.Histogram Tests
 * By Josh Keegan 04/04/2014
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedHelpers.Maths.Statistics;

namespace UnitTests.SharedHelpers.Maths.Statistics
{
    [TestClass]
    public class HistogramTests
    {
        /*
         * Test Data
         */
        [TestMethod]
        public void TestData1()
        {
            //Test you get the same data back that you put in
            double[] data = { 0, 2.3, 4.7 };
            Histogram hist = new Histogram(data);

            double[] expected = { 0, 2.3, 4.7 };
            CollectionAssert.AreEqual(expected, (ICollection)hist.Data);
        }

        /*
         * Test Bins
         */
        [TestMethod]
        public void TestBins1()
        {
            //Test that data gets put into the correct bins
            double[] data = { 1, 2, 3, 4, 5, 7, 2 };
            Histogram hist = new Histogram(data, 7);

            uint[] expected = { 1, 2, 1, 1, 1, 0, 1 };
            CollectionAssert.AreEqual(expected, hist.Bins);
        }

        [TestMethod]
        public void TestBins2()
        {
            //Test that values below the specified minimum get ignored
            double[] data = { 0, 1, 2, 3, 4, 5, 0, 0, 7, 2 };
            Histogram hist = new Histogram(data, 1, 7, 7);

            uint[] expected = { 1, 2, 1, 1, 1, 0, 1 };
            CollectionAssert.AreEqual(expected, hist.Bins);
        }

        [TestMethod]
        public void TestBins3()
        {
            //Test that values above the sepcified maximum get ignored
            double[] data = { 1, 2, 3, 4, 5, 7, 2 };
            Histogram hist = new Histogram(data, 1, 6, 6);

            uint[] expected = { 1, 2, 1, 1, 1, 0 };
            CollectionAssert.AreEqual(expected, hist.Bins);
        }

        [TestMethod]
        public void TestBins4()
        {
            //Test that values equal to the specified maximum get included in the final bin
            double[] data = { 1, 2, 3, 4, 6, 5, 7, 2 };
            Histogram hist = new Histogram(data, 1, 6, 6);

            uint[] expected = { 1, 2, 1, 1, 1, 1 };
            CollectionAssert.AreEqual(expected, hist.Bins);
        }

        /*
         * Test Num Buns
         */
        [TestMethod]
        public void TestNumBins1()
        {
            //Test that the Number of Bins reported is equal to the actual number of Bins 
            double[] data = { 1, 2, 3, 132412, 124, 124, 12, 32, 13, 412, 645, 76, 7, 2 };
            Histogram hist = new Histogram(data);

            Assert.AreEqual(hist.Bins.Length, hist.NumBins);
        }

        [TestMethod]
        public void TestNumBins2()
        {
            //Test that NumBins can be set and the actual bins will be updated
            double[] data = { 1, 2, 3, 4, 5, 7, 2 };
            Histogram hist = new Histogram(data, 7);

            //Check the initial bins
            uint[] expected = { 1, 2, 1, 1, 1, 0, 1 };
            CollectionAssert.AreEqual(expected, hist.Bins);

            //Update the number of bins
            hist.NumBins = 1;
            uint[] newBinsExpected = { (uint)data.Length };
            CollectionAssert.AreEqual(newBinsExpected, hist.Bins);
        }

        /*
         * Test Num Values
         */
        [TestMethod]
        public void TestNumValues1()
        {
            //Test that Num Values is as expected when all values are in range
            double[] data = { 1, 2, 3 };
            Histogram hist = new Histogram(data);

            Assert.AreEqual((uint)data.Length, hist.NumValues);
        }

        [TestMethod]
        public void TestNumValues2()
        {
            //Test that Num Values is as expected when some values aren't in range
            double[] data = { 1, 2, 3 };
            Histogram hist = new Histogram(data, 1, 2);

            Assert.AreEqual(2u, hist.NumValues);
        }

        [TestMethod]
        public void TestNumValues3()
        {
            //Test Num Values is 0 when all values are outside the specified Histogram range
            double[] data = { 1, 2, 3 };
            Histogram hist = new Histogram(data, -1, 0);

            Assert.AreEqual(0u, hist.NumValues);
        }

        /*
         * Test Bin Width
         */
        [TestMethod]
        public void TestBinWidth1()
        {
            //Test that the Bin width is as expected
            double[] data = { 0, 1, 2, 3, 4, 5, 7, 2 };
            Histogram hist = new Histogram(data, 7);

            double expected = 1;
            Assert.AreEqual(expected, hist.BinWidth);
        }

        [TestMethod]
        public void TestBinWidth2()
        {
            //Test that the Bin width is as expected when it's a non-integer value
            double[] data = { 1, 2, 3, 4, 5, 7, 2 };
            Histogram hist = new Histogram(data, 3, 4, 7);

            double expected = 1d / 7d;
            Assert.AreEqual(expected, hist.BinWidth);
        }

        /*
         * Test Constructor
         */
        [TestMethod]
        public void TestConstructor1()
        {
            //Histogram must be initialised on *some* data
            double[] data = { };

            try
            {
                Histogram hist = new Histogram(data);
                Assert.Fail(); //No exception thrown
            }
            catch
            {
                //Exception - pass
            }
        }

        [TestMethod]
        public void TestConstructor2()
        {
            //Constructor must accept lists
            List<double> data = new List<double>(new double[] { 5, 4, 6 });

            Histogram hist = new Histogram(data);
        }
    }
}
