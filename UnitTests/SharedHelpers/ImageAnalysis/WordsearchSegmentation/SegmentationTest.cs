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

        [TestMethod]
        public void TestConstructor8()
        {
            //Test constructor when it's supplied with row & col start & end indices
            int width = 10;
            int height = 5;
            int[,] rows = new int[,] { { 1, 2 }, { 2, 4 } };
            int[,] cols = new int[,] { {0, 3}, {5, 6}, {6, 8} };

            Segmentation segmentation = new Segmentation(rows, cols, width, height);

            int[] rowsSeg = { 2 };
            int[] colsSeg = { 4, 6 };

            CollectionAssert.AreEqual(rowsSeg, segmentation.Rows);
            CollectionAssert.AreEqual(colsSeg, segmentation.Cols);
        }

        [TestMethod]
        public void TestConstructor9()
        {
            //Test constructor when it's supplied with row & col start & end indices whose segmentations will require rounding
            int width = 10;
            int height = 5;
            int[,] rows = new int[,] { { 0, 1 }, { 2, 4 } };
            int[,] cols = new int[,] { { 0, 3 }, { 4, 6 }, { 7, 8 } };

            Segmentation segmentation = new Segmentation(rows, cols, width, height);

            int[] rowsSeg = { 1 };
            int[] colsSeg = { 3, 6 };

            CollectionAssert.AreEqual(rowsSeg, segmentation.Rows);
            CollectionAssert.AreEqual(colsSeg, segmentation.Cols);
        }

        [TestMethod]
        public void TestConstructor10()
        {
            //Test constructor when it's supplied with invalid width
            int width = -1;
            int height = 5;
            int[,] rows = new int[,] { { 0, 1 }, { 2, 4 } };
            int[,] cols = new int[,] { { 0, 3 }, { 4, 6 }, { 7, 8 } };

            try
            {
                Segmentation s = new Segmentation(rows, cols, width, height);
                Assert.Fail(); //Threw no exception, fail
            }
            catch(InvalidImageDimensionsException)
            {
                //Threw correct exception, Pass
            }
            catch
            {
                //Threw wrong exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor11()
        {
            //Test constructor when it's supplied with invalid height
            int width = 10;
            int height = -1;
            int[,] rows = new int[,] { { 0, 1 }, { 2, 4 } };
            int[,] cols = new int[,] { { 0, 3 }, { 4, 6 }, { 7, 8 } };

            try
            {
                Segmentation s = new Segmentation(rows, cols, width, height);
                Assert.Fail(); //Threw no exception, fail
            }
            catch (InvalidImageDimensionsException)
            {
                //Threw correct exception, Pass
            }
            catch
            {
                //Threw wrong exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor12()
        {
            //Test constructor when it's supplied with invalid number of rows
            int width = 10;
            int height = 5;
            int[,] rows = new int[,] {  };
            int[,] cols = new int[,] { { 0, 3 }, { 4, 6 }, { 7, 8 } };

            try
            {
                Segmentation s = new Segmentation(rows, cols, width, height);
                Assert.Fail(); //Threw no exception, fail
            }
            catch (InvalidRowsAndColsException)
            {
                //Threw correct exception, Pass
            }
            catch
            {
                //Threw wrong exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor13()
        {
            //Test constructor when it's supplied with invalid number of cols
            int width = 10;
            int height = 5;
            int[,] rows = new int[,] { { 0, 1 }, { 2, 4 } };
            int[,] cols = new int[,] {  };

            try
            {
                Segmentation s = new Segmentation(rows, cols, width, height);
                Assert.Fail(); //Threw no exception, fail
            }
            catch (InvalidRowsAndColsException)
            {
                //Threw correct exception, Pass
            }
            catch
            {
                //Threw wrong exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor14()
        {
            //Test constructor when it's supplied with invalid row indices (<0)
            int width = 10;
            int height = 5;
            int[,] rows = new int[,] { { 0, 1 }, { -2, 4 } };
            int[,] cols = new int[,] { { 0, 3 }, { 4, 6 }, { 7, 8 } };

            try
            {
                Segmentation s = new Segmentation(rows, cols, width, height);
                Assert.Fail(); //Threw no exception, fail
            }
            catch (InvalidRowsAndColsException)
            {
                //Threw correct exception, Pass
            }
            catch
            {
                //Threw wrong exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor15()
        {
            //Test constructor when it's supplied with invalid row indices (>= height)
            int width = 10;
            int height = 5;
            int[,] rows = new int[,] { { 0, 1 }, { 5, 4 } };
            int[,] cols = new int[,] { { 0, 3 }, { 4, 6 }, { 7, 8 } };

            try
            {
                Segmentation s = new Segmentation(rows, cols, width, height);
                Assert.Fail(); //Threw no exception, fail
            }
            catch (InvalidRowsAndColsException)
            {
                //Threw correct exception, Pass
            }
            catch
            {
                //Threw wrong exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor16()
        {
            //Test constructor when it's supplied with invalid col indices (<0)
            int width = 10;
            int height = 5;
            int[,] rows = new int[,] { { 0, 1 }, { 2, 4 } };
            int[,] cols = new int[,] { { 0, -3 }, { 4, 6 }, { 7, 8 } };

            try
            {
                Segmentation s = new Segmentation(rows, cols, width, height);
                Assert.Fail(); //Threw no exception, fail
            }
            catch (InvalidRowsAndColsException)
            {
                //Threw correct exception, Pass
            }
            catch
            {
                //Threw wrong exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor17()
        {
            //Test constructor when it's supplied with invalid col indices (>=width)
            int width = 10;
            int height = 5;
            int[,] rows = new int[,] { { 0, 1 }, { 2, 4 } };
            int[,] cols = new int[,] { { 0, 3 }, { 4, 6 }, { 7, 42 } };

            try
            {
                Segmentation s = new Segmentation(rows, cols, width, height);
                Assert.Fail(); //Threw no exception, fail
            }
            catch (InvalidRowsAndColsException)
            {
                //Threw correct exception, Pass
            }
            catch
            {
                //Threw wrong exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor18()
        {
            //Test constructor when it's supplied with invalid row array dimensions
            int width = 10;
            int height = 5;
            int[,] rows = new int[,] { { 0, 1, 2 }, { 2, 4, 3 } };
            int[,] cols = new int[,] { { 0, 3 }, { 4, 6 }, { 7, 8 } };

            try
            {
                Segmentation s = new Segmentation(rows, cols, width, height);
                Assert.Fail(); //Threw no exception, fail
            }
            catch (UnexpectedArrayDimensionsException)
            {
                //Threw correct exception, Pass
            }
            catch
            {
                //Threw wrong exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor19()
        {
            //Test constructor when it's supplied with invalid col array dimensions
            int width = 10;
            int height = 5;
            int[,] rows = new int[,] { { 0, 1 }, { 2, 4 } };
            int[,] cols = new int[,] { { 0, 3, 3 }, { 4, 6, 6 }, { 7, 8, 8 } };

            try
            {
                Segmentation s = new Segmentation(rows, cols, width, height);
                Assert.Fail(); //Threw no exception, fail
            }
            catch (UnexpectedArrayDimensionsException)
            {
                //Threw correct exception, Pass
            }
            catch
            {
                //Threw wrong exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor20()
        {
            //Test constructor when it's supplied with overlapping rows
            int width = 10;
            int height = 5;
            int[,] rows = new int[,] { { 0, 1 }, { 0, 4 } };
            int[,] cols = new int[,] { { 0, 3 }, { 4, 6 }, { 7, 8 } };

            try
            {
                Segmentation s = new Segmentation(rows, cols, width, height);
                Assert.Fail(); //Threw no exception, fail
            }
            catch (InvalidRowsAndColsException)
            {
                //Threw correct exception, Pass
            }
            catch
            {
                //Threw wrong exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor21()
        {
            //Test constructor when it's supplied with overlapping cols
            int width = 10;
            int height = 5;
            int[,] rows = new int[,] { { 0, 1 }, { 2, 4 } };
            int[,] cols = new int[,] { { 0, 3 }, { 4, 6 }, { 5, 8 } };

            try
            {
                Segmentation s = new Segmentation(rows, cols, width, height);
                Assert.Fail(); //Threw no exception, fail
            }
            catch (InvalidRowsAndColsException)
            {
                //Threw correct exception, Pass
            }
            catch
            {
                //Threw wrong exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor22()
        {
            //Test constructor when it's supplied with unordered rows
            int width = 10;
            int height = 5;
            int[,] rows = new int[,] { { 2, 4 }, { 0, 1 } };
            int[,] cols = new int[,] { { 0, 3 }, { 4, 6 }, { 7, 8 } };

            try
            {
                Segmentation s = new Segmentation(rows, cols, width, height);
                Assert.Fail(); //Threw no exception, fail
            }
            catch (InvalidRowsAndColsException)
            {
                //Threw correct exception, Pass
            }
            catch
            {
                //Threw wrong exception, fail
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConstructor23()
        {
            //Test constructor when it's supplied with unordered cols
            int width = 10;
            int height = 5;
            int[,] rows = new int[,] { { 0, 1 }, { 2, 4 } };
            int[,] cols = new int[,] { { 0, 3 }, { 7, 8 }, { 4, 6 } };

            try
            {
                Segmentation s = new Segmentation(rows, cols, width, height);
                Assert.Fail(); //Threw no exception, fail
            }
            catch (InvalidRowsAndColsException)
            {
                //Threw correct exception, Pass
            }
            catch
            {
                //Threw wrong exception, fail
                Assert.Fail();
            }
        }

        /*
         * Test NumRows Accessor
         */
        [TestMethod]
        public void TestNumRows1()
        {
            int[] rows = { };
            int[] cols = { 1 };

            Segmentation s = new Segmentation(rows, cols);

            Assert.AreEqual(1, s.NumRows);
        }

        /*
         * Test NumCols Accessor
         */
        [TestMethod]
        public void TestNumCols1()
        {
            int[] rows = { };
            int[] cols = { 1 };

            Segmentation s = new Segmentation(rows, cols);

            Assert.AreEqual(2, s.NumCols);
        }
    }
}
