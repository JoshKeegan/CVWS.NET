/*
 * Dissertation CV Wordsearch Solver
 * Unit Tests
 * SharedHelpers.ImageAnalysis.WordsearchSegmentation.WordsearchSegmentation Tests
 * By Josh Keegan 02/04/2014
 * Last Edit 03/04/2014
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedHelpers.ImageAnalysis.WordsearchSegmentation;
using SharedHelpers.Exceptions;

namespace UnitTests.SharedHelpers.ImageAnalysis.WordsearchSegmentation
{
    [TestClass]
    public class SegmentationTest
    {
        /*
         * Constructor Tests
         */
        [TestMethod]
        public void TestConstructor1()
        {
            //Basic Tests
            int[] rows = { 1, 2, 3, 67, 12 };
            int[] cols = { 89, 5, 4 };
            Segmentation segmentation = new Segmentation(rows, cols);
            CollectionAssert.AreEqual(rows, segmentation.Rows);
            CollectionAssert.AreEqual(cols, segmentation.Cols);
        }

        [TestMethod]
        public void TestConstructor2()
        {
            //Test constructor when it's supplied with number of rows and cols
            int width = 10;
            int height = 5;
            int numRows = 5;
            int numCols = 2;

            Segmentation segmentation = new Segmentation(numRows, numCols, width, height);

            int[] rows = { 1, 2, 3, 4 };
            int[] cols = { 5 }; // 0-5, 5-10

            CollectionAssert.AreEqual(rows, segmentation.Rows);
            CollectionAssert.AreEqual(cols, segmentation.Cols);
        }

        [TestMethod]
        public void TestConstructor3()
        {
            //Test constructor when it's supplied with an invalid number of rows
            int width = 10;
            int height = 5;
            int numRows = 0;
            int numCols = 2;

            try
            {
                Segmentation segmentation = new Segmentation(numRows, numCols, width, height);
                Assert.Fail();
            }
            catch(InvalidRowsAndColsException)
            {
                //Threw the correct exception, passed
            }
            catch
            {
                //Threw the wrong exception, failed
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor4()
        {
            //Test constructor when it's supplied with an invalid number of cols
            int width = 10;
            int height = 5;
            int numRows = 5;
            int numCols = 0;

            try
            {
                Segmentation segmentation = new Segmentation(numRows, numCols, width, height);
                Assert.Fail();
            }
            catch (InvalidRowsAndColsException)
            {
                //Threw the correct exception, passed
            }
            catch
            {
                //Threw the wrong exception, failed
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor5()
        {
            //Test constructor when it's supplied with an invalid width
            int width = -1;
            int height = 5;
            int numRows = 5;
            int numCols = 2;

            try
            {
                Segmentation segmentation = new Segmentation(numRows, numCols, width, height);
                Assert.Fail();
            }
            catch (InvalidImageDimensionsException)
            {
                //Threw the correct exception, passed
            }
            catch
            {
                //Threw the wrong exception, failed
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor6()
        {
            //Test constructor when it's supplied with an invalid height
            int width = 10;
            int height = -1;
            int numRows = 5;
            int numCols = 2;

            try
            {
                Segmentation segmentation = new Segmentation(numRows, numCols, width, height);
                Assert.Fail();
            }
            catch (InvalidImageDimensionsException)
            {
                //Threw the correct exception, passed
            }
            catch
            {
                //Threw the wrong exception, failed
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor7()
        {
            //Test constructor when it's supplied with number of rows and cols that don't divide without a remainder into the width & height
            int width = 10;
            int height = 5;
            int numRows = 2;
            int numCols = 3;

            Segmentation segmentation = new Segmentation(numRows, numCols, width, height);

            int[] rows = { 2 }; //Expect the floor to be returned
            int[] cols = { 3, 6 };

            CollectionAssert.AreEqual(rows, segmentation.Rows);
            CollectionAssert.AreEqual(cols, segmentation.Cols);
        }

        /*
         * TODO: More Unit Tests
         */
    }
}
