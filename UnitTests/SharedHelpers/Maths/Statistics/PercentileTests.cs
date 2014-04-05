/*
 * Dissertation CV Wordsearch Solver
 * Unit Tests
 * SharedHelpers.Maths.Statistics.Percentile Tests
 * By Josh Keegan 05/04/2014
 */

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedHelpers.Maths.Statistics;

using BaseObjectExtensions;

namespace UnitTests.SharedHelpers.Maths.Statistics
{
    [TestClass]
    public class PercentileTests
    {
        /*
         * Test Data
         */
        [TestMethod]
        public void TestData1()
        {
            //Check that the data gets sorted into ascending order
            double[] arr = { 5, 4, 3, 7 };

            Percentile p = new Percentile(arr);

            double[] expected = { 3, 4, 5, 7 };
            CollectionAssert.AreEqual(expected, p.Data);
        }

        /*
         * Test Constructor
         */
        [TestMethod]
        public void TestConstructor1()
        {
            //Test array with no elements
            double[] arr = new double[0];

            try
            {
                new Percentile(arr);

                //No Exception, Fail
                Assert.Fail();
            }
            catch(ArgumentOutOfRangeException)
            {
                //Correct exception, pass
            }
            catch
            {
                //Wrong exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor2()
        {
            //Test null input
            double[] arr = null;

            try
            {
                new Percentile(arr);

                //No Exception, Fail
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                //Correct exception, pass
            }
            catch
            {
                //Wrong exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor3()
        {
            //Test with List
            List<double> list = new List<double>(new double[] { 3, 5, 7 });

            Percentile p = new Percentile(list);

            double[] expected = { 3, 5, 7 };
            CollectionAssert.AreEqual(expected, p.Data);
        }

        /*
         * Test Calculate Percentile
         */
        [TestMethod]
        public void TestCalculatePercentile1()
        {
            //Test odd arr 50th percentile
            uint[] arr = { 0, 2, 3 };

            Percentile p = new Percentile(arr.ToDoubleArr());

            Assert.AreEqual(2, p.CalculatePercentile(50));
        }

        [TestMethod]
        public void TestCalculatePercentile2()
        {
            //Test even arr 50th percentile
            uint[] arr = { 0, 1 };

            Percentile p = new Percentile(arr.ToDoubleArr());

            Assert.AreEqual(0.5, p.CalculatePercentile(50));
        }

        [TestMethod]
        public void TestCalculatePercentile3()
        {
            //Test unordered
            uint[] arr = { 2, 3, 0 };

            Percentile p = new Percentile(arr.ToDoubleArr());

            Assert.AreEqual(2, p.CalculatePercentile(50));
        }

        [TestMethod]
        public void TestCalculatePercentile4()
        {
            //Test Quartiles of data trivially split into quartiles
            uint[] arr = { 1, 2, 3, 4 };

            Percentile p = new Percentile(arr.ToDoubleArr());

            Assert.AreEqual(1.5, p.CalculatePercentile(25));
            Assert.AreEqual(2.5, p.CalculatePercentile(50));
            Assert.AreEqual(3.5, p.CalculatePercentile(75));
        }

        [TestMethod]
        public void TestCalculatePercentile5()
        {
            //Test Quartiles of random data
            uint[] arr = { 10, 20, 42, 36, 102, 12, 34 };

            Percentile p = new Percentile(arr.ToDoubleArr());

            Assert.AreEqual(14, p.CalculatePercentile(25));
            Assert.AreEqual(34, p.CalculatePercentile(50));
            Assert.AreEqual(40.5, p.CalculatePercentile(75));
        }

        [TestMethod]
        public void TestCalculatePercentile6()
        {
            //Test array with just one element
            uint[] arr = { 1 };

            Percentile p = new Percentile(arr.ToDoubleArr());

            Assert.AreEqual(1, p.CalculatePercentile(23));
        }

        [TestMethod]
        public void TestCalculatePercentile7()
        {
            //Test percentile < 0
            double[] arr = { 1, 2 };

            Percentile p = new Percentile(arr);

            try
            {
                p.CalculatePercentile(-1);
                //No Exception, Fail
                Assert.Fail();
            }
            catch(ArgumentOutOfRangeException)
            {
                //Correct exception, pass
            }
            catch
            {
                //Wrong Exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestCalculatePercentile8()
        {
            //Test percentile > 100
            double[] arr = { 1, 2 };

            Percentile p = new Percentile(arr);

            try
            {
                p.CalculatePercentile(100.1);
                //No Exception, Fail
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                //Correct exception, pass
            }
            catch
            {
                //Wrong Exception, fail
                Assert.Fail();
            }
        }
    }
}
