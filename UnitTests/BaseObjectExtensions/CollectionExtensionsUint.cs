/*
 * Dissertation CV Wordsearch Solver
 * Unit Tests
 * BaseObjectExtensions.CollectionExtensions Tests for the Uint operators
 * By Josh Keegan 26/03/2014
 * Last Edit 03/04/2014
 */

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BaseObjectExtensions;

namespace UnitTests.BaseObjectExtensions
{
    [TestClass]
    public class CollectionExtensionsUint
    {
        /*
         * Mean Function Tests
         */
        [TestMethod]
        public void TestMean1()
        {
            uint[] uints = { 1, 2, 3 };
            double expected = 2;
            double actual = uints.Mean();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMean2()
        {
            uint[] uints = { 4, 36, 45, 50, 75 };
            double expected = 42;
            double actual = uints.Mean();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMean3()
        {
            //Test result with decimal point
            uint[] uints = { 75, 2 };
            double expected = 38.5;
            double actual = uints.Mean();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMean4()
        {
            //Test Queue
            uint[] arrUints = { 75, 2 };
            Queue<uint> uints = new Queue<uint>(arrUints);
            double expected = arrUints.Mean();
            double actual = uints.Mean();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMean5()
        {
            //Test Stack
            uint[] arrUints = { 75, 2 };
            Stack<uint> uints = new Stack<uint>(arrUints);
            double expected = arrUints.Mean();
            double actual = uints.Mean();

            Assert.AreEqual(expected, actual);
        }

        /*
         * Median Function Tests
         */
        [TestMethod]
        public void TestMedian1()
        {
            //Test odd arr
            uint[] arr = { 0, 2, 3 };

            Assert.AreEqual(2, arr.Median());
        }

        [TestMethod]
        public void TestMedian2()
        {
            //Test even arr
            uint[] arr = { 0, 1 };

            Assert.AreEqual(0.5, arr.Median());
        }

        [TestMethod]
        public void TestMedian3()
        {
            //Test arr not sorted
            uint[] arr = { 1, 5, 5, 1, 1, 3, 6 };

            Assert.AreEqual(3, arr.Median());
        }

        [TestMethod]
        public void TestMedian4()
        {
            //Test arr doesn't get changed
            uint[] arr = { 2, 1 };
            uint[] orig = { 2, 1 };

            arr.Median();

            CollectionAssert.AreEqual(orig, arr);
        }

        /*
         * Percentile Tests
         */
        [TestMethod]
        public void TestPercentile1()
        {
            //Test odd arr 50th percentile
            uint[] arr = { 0, 2, 3 };

            Assert.AreEqual(2, arr.Percentile(50));
        }

        [TestMethod]
        public void TestPercentile2()
        {
            //Test even arr 50th percentile
            uint[] arr = { 0, 1 };

            Assert.AreEqual(0.5, arr.Percentile(50));
        }

        [TestMethod]
        public void TestPercentile3()
        {
            //Test unordered
            uint[] arr = { 2, 3, 0 };

            Assert.AreEqual(2, arr.Percentile(50));
        }

        [TestMethod]
        public void TestPercentile4()
        {
            //Test Quartiles of data trivially split into quartiles
            uint[] arr = { 1, 2, 3, 4 };

            Assert.AreEqual(1.5, arr.Percentile(25));
            Assert.AreEqual(2.5, arr.Percentile(50));
            Assert.AreEqual(3.5, arr.Percentile(75));
        }

        [TestMethod]
        public void TestPercentile5()
        {
            //Test Quartiles of random data
            uint[] arr = { 10, 20, 42, 36, 102, 12, 34 };

            Assert.AreEqual(14, arr.Percentile(25));
            Assert.AreEqual(34, arr.Percentile(50));
            Assert.AreEqual(40.5, arr.Percentile(75));
        }

        [TestMethod]
        public void TestPercentile6()
        {
            //Test array with just one element
            uint[] arr = { 1 };

            Assert.AreEqual(1, arr.Percentile(23));
        }

        [TestMethod]
        public void TestPercentile7()
        {
            //Test array with no elements
            uint[] arr = new uint[0];

            try
            {
                arr.Percentile(50);
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
        public void TestPercentile8()
        {
            //Test percentile < 0
            uint[] arr = { 1, 2 };

            try
            {
                arr.Percentile(-1);
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

        [TestMethod]
        public void TestPercentile9()
        {
            //Test percentile > 100
            uint[] arr = { 1, 2 };

            try
            {
                arr.Percentile(100.1);
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
