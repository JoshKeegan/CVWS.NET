/*
 * CVWS.NET
 * Unit Tests
 * PointExtensions Tests
 * Authors:
 *  Josh Keegan 17/04/2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AForge;

using libCVWS.BaseObjectExtensions;

namespace UnitTests.libCVWS.BaseObjectExtensions
{
    [TestClass]
    public class PointExtensionsTests
    {
        [TestMethod]
        public void TestHalfPi()
        {
            Point a = new Point(0, 0);
            Point b = new Point(0, 1);
            double expected = Math.PI / 2;
            double actual = a.AngleTo(b);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestHalfPiNotOrigin()
        {
            Point a = new Point(12, 12);
            Point b = new Point(12, 13);
            double expected = Math.PI / 2;
            double actual = a.AngleTo(b);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestQuaterPi()
        {
            Point a = new Point(0, 0);
            Point b = new Point(1, 1);
            double expected = Math.PI / 4;
            double actual = a.AngleTo(b);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestThreeQuatersPi()
        {
            Point a = new Point(0, 0);
            Point b = new Point(-1, 1);
            double expected = 3 * Math.PI / 4;
            double actual = a.AngleTo(b);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPi()
        {
            Point a = new Point(0, 0);
            Point b = new Point(-1, 0);
            double expected = Math.PI;
            double actual = a.AngleTo(b);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestOneAndAHalfPi()
        {
            Point a = new Point(0, 0);
            Point b = new Point(0, -1);
            double expected = Math.PI * 1.5;
            double actual = a.AngleTo(b);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestOneAndThreeQuatersPi()
        {
            Point a = new Point(0, 0);
            Point b = new Point(1, -1);
            double expected = Math.PI * 1.75;
            double actual = a.AngleTo(b);

            Assert.AreEqual(expected, actual);
        }
    }
}
