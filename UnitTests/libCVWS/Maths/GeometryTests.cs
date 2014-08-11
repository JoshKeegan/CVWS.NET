/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Unit Tests
 * libCVWS.Maths.Geometry Tests
 * By Josh Keegan 05/03/2014
 * Last Edit 30/04/2014
 */

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AForge;

using libCVWS.Maths;
using libCVWS.Exceptions;

namespace UnitTests.libCVWS.Maths
{
    [TestClass]
    public class GeometryTests
    {
        /*
         * Euclidean Distance Function Tests
         */
        [TestMethod]
        public void TestEuclideanDistance1()
        {
            //Basic test
            IntPoint a = new IntPoint(1, 1);
            IntPoint b = new IntPoint(2, 2);

            double distance = Geometry.EuclideanDistance(a, b);
            Assert.AreEqual(Math.Sqrt(2), distance);
        }

        [TestMethod]
        public void TestEuclideanDistance2()
        {
            //Parameter order doesn't matter
            IntPoint a = new IntPoint(2, 2);
            IntPoint b = new IntPoint(1, 1);

            double distance = Geometry.EuclideanDistance(a, b);
            Assert.AreEqual(Math.Sqrt(2), distance);
        }

        [TestMethod]
        public void TestEuclideanDistance3()
        {
            //Non-unit lengths test
            IntPoint a = new IntPoint(324, 12);
            IntPoint b = new IntPoint(12, 123);

            double distance = Geometry.EuclideanDistance(a, b);
            Assert.AreEqual(3 * Math.Sqrt(12185), distance);
        }

        [TestMethod]
        public void TestEuclideanDistance4()
        {
            //Negative coordinates
            IntPoint a = new IntPoint(-1, -1);
            IntPoint b = new IntPoint(-2, -2);

            double distance = Geometry.EuclideanDistance(a, b);
            Assert.AreEqual(Math.Sqrt(2), distance);
        }

        [TestMethod]
        public void TestEuclideanDistance5()
        {
            //0 distance / points equal
            IntPoint a = new IntPoint(0, 0);
            IntPoint b = new IntPoint(0, 0);

            double distance = Geometry.EuclideanDistance(a, b);
            Assert.AreEqual(0, distance);
        }

        /*
         * IsQuadrilateral Tests
         */
        [TestMethod]
        public void TestIsQuadrilateral1()
        {
            //Basic test
            Assert.IsTrue(Geometry.IsQuadrilateral(getValidQuadrilateral()));
        }

        [TestMethod]
        public void TestIsQuadrilateral2()
        {
            //Test with a non-array collection
            List<IntPoint> points = new List<IntPoint>(getValidQuadrilateral());
            Assert.IsTrue(Geometry.IsQuadrilateral(points));
        }

        [TestMethod]
        public void TestIsQuadrilateral3()
        {
            //Test with too many points
            Assert.IsFalse(Geometry.IsQuadrilateral(getValidPentagon()));
        }

        [TestMethod]
        public void TestIsQuadrilateral4()
        {
            //Test with too few points
            Assert.IsFalse(Geometry.IsQuadrilateral(getValidTriangle()));
        }

        [TestMethod]
        public void TestIsQuadrilateral5()
        {
            //Test errors with null array
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
        public void TestTriangleAngle1()
        {
            //Basic test
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
        public void TestAreaQuadrilateral1()
        {
            //Basic test
            IntPoint[] points = getValidQuadrilateral();
            double expected = VALID_QUADRILATERAL_AREA;

            double actual = Geometry.AreaQuadrilateral(points);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestAreaQuadrilatera2()
        {
            //Test with a non-array collection
            List<IntPoint> points = new List<IntPoint>(getValidQuadrilateral());
            double expected = VALID_QUADRILATERAL_AREA;

            double actual = Geometry.AreaQuadrilateral(points);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestAreaQuadrilateral3()
        {
            //Test with invalid Quadrilateral
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
        public void TestArea1()
        {
            //Test with a unit rectangle
            IntPoint topLeft = new IntPoint(0, 1);
            IntPoint topRight = new IntPoint(1, 1);
            IntPoint bottomRight = new IntPoint(1, 0);
            IntPoint bottomLeft = new IntPoint(0, 0);

            Assert.AreEqual(1, Geometry.Area(topLeft, topRight, bottomRight, bottomLeft));
        }

        [TestMethod]
        public void TestArea2()
        {
            //Test with a non-unit rectangle
            IntPoint topLeft = new IntPoint(0, 2);
            IntPoint topRight = new IntPoint(2, 2);
            IntPoint bottomRight = new IntPoint(2, 0);
            IntPoint bottomLeft = new IntPoint(0, 0);

            Assert.AreEqual(4, Geometry.Area(topLeft, topRight, bottomRight, bottomLeft));
        }

        [TestMethod]
        public void TestArea3()
        {
            //Test with an arbitrary quadrilateral
            IntPoint topLeft = new IntPoint(12, 123);
            IntPoint topRight = new IntPoint(136, 203);
            IntPoint bottomRight = new IntPoint(324, 12);
            IntPoint bottomLeft = new IntPoint(5, 42);

            double actual = Geometry.Area(topLeft, topRight, bottomRight, bottomLeft);

            double expected = 32386.5; //Calculated by hand with the shoelace formula

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestArea4()
        {
            //Test with array
            Assert.AreEqual(VALID_QUADRILATERAL_AREA, Geometry.Area(getValidQuadrilateral()));
        }

        [TestMethod]
        public void TestArea5()
        {
            //Test with a non-array collection
            Assert.AreEqual(VALID_QUADRILATERAL_AREA, Geometry.Area(new List<IntPoint>(getValidQuadrilateral())));
        }

        [TestMethod]
        public void TestArea6()
        {
            //Test with triangle
            Assert.AreEqual(VALID_TRIANGLE_AREA, Geometry.Area(getValidTriangle()));
        }

        [TestMethod]
        public void TestArea7()
        {
            //Test with anticlockwise winding order
            IntPoint[] points = new IntPoint[4];
            points[0] = new IntPoint(12, 123);
            points[1] = new IntPoint(5, 42);
            points[2] = new IntPoint(324, 12);
            points[3] = new IntPoint(136, 203);

            double actual = Geometry.Area(points);

            double expected = 32386.5; //Calculated by hand with the shoelace formula

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestArea8()
        {
            //Test with points out of order
            IntPoint[] points = new IntPoint[4];
            points[0] = new IntPoint(136, 203);
            points[1] = new IntPoint(12, 123);
            points[2] = new IntPoint(324, 12);
            points[3] = new IntPoint(5, 42);

            double actual = Geometry.Area(points);

            double expected = 32386.5; //Calculated by hand with the shoelace formula

            Assert.AreEqual(expected, actual);
        }

        /*
         * SortPointsClockwise Tests
         */
        [TestMethod]
        public void TestSortPointsClockwise1()
        {
            //Standard test returns points already ordered
            IntPoint[] expected = getValidQuadrilateral();
            IntPoint[] actual = Geometry.SortPointsClockwise(expected);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSortPointsClockwise2()
        {
            //Test with points given in anticlockwise order
            IntPoint[] expected = getValidQuadrilateral();
            IntPoint[] input = new IntPoint[expected.Length];
            input[0] = expected[0]; //Keep the starting position
            input[2] = expected[1];
            input[1] = expected[2];

            CollectionAssert.AreEqual(expected, Geometry.SortPointsClockwise(input)); 
        }

        [TestMethod]
        public void TestSortPointsClockwise3()
        {
            //Test with triangle
            IntPoint[] expected = getValidTriangle();
            IntPoint[] actual = Geometry.SortPointsClockwise(expected);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSortPointsClockwise4()
        {
            //Test with pentagon
            IntPoint[] expected = getValidPentagon();
            IntPoint[] actual = Geometry.SortPointsClockwise(expected);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSortPointsClockwise5()
        {
            //Test with points in random order
            IntPoint[] unordered = new IntPoint[4];
            unordered[0] = new IntPoint(136, 203);
            unordered[1] = new IntPoint(12, 123);
            unordered[2] = new IntPoint(324, 12);
            unordered[3] = new IntPoint(5, 42);

            IntPoint[] expected = new IntPoint[4];
            expected[0] = new IntPoint(136, 203);
            expected[1] = new IntPoint(324, 12);
            expected[2] = new IntPoint(5, 42);
            expected[3] = new IntPoint(12, 123);

            IntPoint[] actual = Geometry.SortPointsClockwise(unordered);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSortPointsClockwise6()
        {
            //Test with a single point
            IntPoint[] expected = new IntPoint[] { new IntPoint(0, 0) };
            IntPoint[] actual = Geometry.SortPointsClockwise(expected);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSortPointsClockwise7()
        {
            //Test with no points
            IntPoint[] expected = new IntPoint[0];
            IntPoint[] actual = Geometry.SortPointsClockwise(expected);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSortPointsClockwise8()
        {
            //Test with line (already clockwise)
            IntPoint[] points = new IntPoint[] { new IntPoint(0, 0), new IntPoint(1, 1) };

            CollectionAssert.AreEqual(points, Geometry.SortPointsClockwise(points));
        }

        [TestMethod]
        public void TestSortPointsClockwise9()
        {
            //Test with line (anticlockwise, but should remain anticlockwise due to the first point being maintained)
            IntPoint[] points = new IntPoint[] { new IntPoint(1, 1), new IntPoint(0, 0) };

            CollectionAssert.AreEqual(points, Geometry.SortPointsClockwise(points));
        }


        /*
         * Private Helpers
         */
        private const double VALID_TRIANGLE_AREA = 0.5;
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
