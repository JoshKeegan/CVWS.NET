/*
 * Dissertation CV Wordsearch Solver
 * Unit Tests
 * SharedHelpers.Maths.Geometry Tests
 * By Josh Keegan 05/03/2014
 */

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AForge;

using SharedHelpers.Maths;
using SharedHelpers.Maths.Exceptions;

namespace UnitTests.SharedHelpers.Maths
{
    [TestClass]
    public class GeometryTests
    {
        /*
         * Euclidean Distance Function Tests
         */
        [TestMethod]
        //Basic test
        public void TestEuclideanDistance1()
        {
            IntPoint a = new IntPoint(1, 1);
            IntPoint b = new IntPoint(2, 2);

            double distance = Geometry.EuclideanDistance(a, b);
            Assert.AreEqual(Math.Sqrt(2), distance);
        }

        [TestMethod]
        //Parameter order doesn't matter
        public void TestEuclideanDistance2()
        {
            IntPoint a = new IntPoint(2, 2);
            IntPoint b = new IntPoint(1, 1);

            double distance = Geometry.EuclideanDistance(a, b);
            Assert.AreEqual(Math.Sqrt(2), distance);
        }

        [TestMethod]
        //Non-unit lengths test
        public void TestEuclideanDistance3()
        {
            IntPoint a = new IntPoint(324, 12);
            IntPoint b = new IntPoint(12, 123);

            double distance = Geometry.EuclideanDistance(a, b);
            Assert.AreEqual(3 * Math.Sqrt(12185), distance);
        }

        [TestMethod]
        //Negative coordinates
        public void TestEuclideanDistance4()
        {
            IntPoint a = new IntPoint(-1, -1);
            IntPoint b = new IntPoint(-2, -2);

            double distance = Geometry.EuclideanDistance(a, b);
            Assert.AreEqual(Math.Sqrt(2), distance);
        }

        [TestMethod]
        //0 distance / points equal
        public void TestQuclideanDistance5()
        {
            IntPoint a = new IntPoint(0, 0);
            IntPoint b = new IntPoint(0, 0);

            double distance = Geometry.EuclideanDistance(a, b);
            Assert.AreEqual(0, distance);
        }

        /*
         * IsQuadrilateral Tests
         */
        [TestMethod]
        //Basic test
        public void TestIsQuadrilateral1()
        {
            Assert.IsTrue(Geometry.IsQuadrilateral(getValidQuadrilateral()));
        }

        [TestMethod]
        //Test with a non-array collection
        public void TestIsQuadrilateral2()
        {
            List<IntPoint> points = new List<IntPoint>(getValidQuadrilateral());
            Assert.IsTrue(Geometry.IsQuadrilateral(points));
        }

        [TestMethod]
        //Test with too many points
        public void TestIsQuadrilateral3()
        {
            Assert.IsFalse(Geometry.IsQuadrilateral(getValidPentagon()));
        }

        [TestMethod]
        //Test with too few points
        public void TestIsQuadrilateral4()
        {
            Assert.IsFalse(Geometry.IsQuadrilateral(getValidTriangle()));
        }

        [TestMethod]
        //Test errors with null array
        public void TestIsQuadrilateral5()
        {
            IntPoint[] arr = null;
            try
            {
                Geometry.IsQuadrilateral(arr);

                //No error
                Assert.Fail();
            }
            catch { }
        }

        /*
         * TriangleAngle Tests
         */
        [TestMethod]
        //Basic test
        public void TestTriangleAngle1()
        {
            double a = 8;
            double b = 6;
            double c = 7;

            double B = Geometry.TriangleAngle(a, b, c);

            Assert.AreEqual(0.81, Math.Round(B, 2));
        }

        /*
         * AreaQuadrilateral Tests
         */
        [TestMethod]
        //Basic test
        public void TestAreaQuadrilateral1()
        {
            IntPoint[] points = getValidQuadrilateral();
            double expected = VALID_QUADRILATERAL_AREA;

            double actual = Geometry.AreaQuadrilateral(points);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        //Test with a non-array collection
        public void TestAreaQuadrilatera2()
        {
            List<IntPoint> points = new List<IntPoint>(getValidQuadrilateral());
            double expected = VALID_QUADRILATERAL_AREA;

            double actual = Geometry.AreaQuadrilateral(points);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        //Test with invalid Quadrilateral
        public void TestAreaQuadrilateral3()
        {
            IntPoint[] points = getValidPentagon();

            try
            {
                Geometry.AreaQuadrilateral(points);
            }
            catch(InvalidShapeException) //Check the type of exception returned too
            {
                return; //Got the expected exception
            }
            Assert.Fail(); //Didn't get the expected exception, fail
        }

        /*
         * Area Tests
         */
        [TestMethod]
        //Test with a unit rectangle
        public void TestArea1()
        {
            IntPoint topLeft = new IntPoint(0, 1);
            IntPoint topRight = new IntPoint(1, 1);
            IntPoint bottomRight = new IntPoint(1, 0);
            IntPoint bottomLeft = new IntPoint(0, 0);

            Assert.AreEqual(1, Geometry.Area(topLeft, topRight, bottomRight, bottomLeft));
        }

        [TestMethod]
        //Test with a non-unit rectangle
        public void TestArea2()
        {
            IntPoint topLeft = new IntPoint(0, 2);
            IntPoint topRight = new IntPoint(2, 2);
            IntPoint bottomRight = new IntPoint(2, 0);
            IntPoint bottomLeft = new IntPoint(0, 0);

            Assert.AreEqual(4, Geometry.Area(topLeft, topRight, bottomRight, bottomLeft));
        }

        [TestMethod]
        //Test with 
        public void TestArea3()
        {
            IntPoint topLeft = new IntPoint(12, 123);
            IntPoint topRight = new IntPoint(136, 203);
            IntPoint bottomRight = new IntPoint(324, 12);
            IntPoint bottomLeft = new IntPoint(5, 42);

            double actual = Geometry.Area(topLeft, topRight, bottomRight, bottomLeft);

            double expected = 32386.5; //Calculated by hand with the shoelace formula

            Assert.AreEqual(expected, actual);
        }


        /*
         * Private Helpers
         */
        private static IntPoint[] getValidTriangle()
        {
            IntPoint[] toRet = new IntPoint[3];

            toRet[0] = new IntPoint(0, 0);
            toRet[1] = new IntPoint(1, 1);
            toRet[2] = new IntPoint(1, 0);

            return toRet;
        }

        private const double VALID_QUADRILATERAL_AREA = 1;
        private static IntPoint[] getValidQuadrilateral()
        {
            IntPoint[] toRet = new IntPoint[4];

            toRet[0] = new IntPoint(0, 1);
            toRet[1] = new IntPoint(1, 1);
            toRet[2] = new IntPoint(1, 0);
            toRet[3] = new IntPoint(0, 0);

            return toRet;
        }

        private static IntPoint[] getValidPentagon()
        {
            IntPoint[] toRet = new IntPoint[5];

            toRet[0] = new IntPoint(0, 1);
            toRet[1] = new IntPoint(1, 2);
            toRet[2] = new IntPoint(2, 1);
            toRet[3] = new IntPoint(2, 0);
            toRet[4] = new IntPoint(0, 0);

            return toRet;
        }
    }
}
