/*
 * Computer Vision Wordsearch Solver
 * Unit Tests
 * BaseObjectExtensions.CollectionExtensions Tests for the Uint operators
 * By Josh Keegan 26/03/2014
 * Last Edit 05/04/2014
 */

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BaseObjectExtensions;

namespace UnitTests.BaseObjectExtensions
{
    [TestClass]
    public class CollectionExtensionsUintTests
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
    }
}
