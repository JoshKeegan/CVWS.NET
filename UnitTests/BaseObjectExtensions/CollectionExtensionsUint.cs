/*
 * Dissertation CV Wordsearch Solver
 * Unit Tests
 * BaseObjectExtensions.CollectionExtensions Tests for the Uint operators
 * By Josh Keegan 26/03/2014
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
    }
}
