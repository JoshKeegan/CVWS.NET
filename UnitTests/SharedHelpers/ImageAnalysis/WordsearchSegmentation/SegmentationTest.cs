/*
 * Dissertation CV Wordsearch Solver
 * Unit Tests
 * SharedHelpers.ImageAnalysis.WordsearchSegmentation.WordsearchSegmentation Tests
 * By Josh Keegan 02/04/2014
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedHelpers.ImageAnalysis.WordsearchSegmentation;

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

        /*
         * TODO: More Unit Tests
         */
    }
}
