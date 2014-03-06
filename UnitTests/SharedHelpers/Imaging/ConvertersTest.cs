/*
 * Dissertation CV Wordsearch Solver
 * Unit Tests
 * SharedHelpers.Imaging.Converters Tests
 * By Josh Keegan 06/03/2014
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedHelpers.Imaging;

using AForge.Imaging.Filters;

namespace UnitTests.SharedHelpers.Imaging
{
    [TestClass]
    public class ConvertersTest
    {
        /*
         * ThresholdedBitmapToBoolArray Function Tests
         */
        [TestMethod]
        public void TestThresholdedBitmapToBoolArray1()
        {
            Bitmap bitmap = get8bppConvertedSinglePixelBitmap(Color.Black);

            bool[,] expected = new bool[1, 1];
            expected[0, 0] = true;

            CollectionAssert.AreEqual(expected, Converters.ThresholdedBitmapToBoolArray(bitmap));

            bitmap.Dispose();
        }

        [TestMethod]
        public void TestThresholdedBitmapToBoolArray2()
        {
            Bitmap bitmap = get8bppConvertedSinglePixelBitmap(Color.White);

            bool[,] expected = new bool[1, 1];
            expected[0, 0] = false;

            CollectionAssert.AreEqual(expected, Converters.ThresholdedBitmapToBoolArray(bitmap));

            bitmap.Dispose();
        }

        [TestMethod]
        public void TestThresholdedBitmapToBoolArray3()
        {
            //Test that an exception is thrown when an image's PixelFormat isn't 8bpp
            Bitmap bitmap = new Bitmap(1, 1);
            bitmap.SetPixel(0, 0, Color.Black);

            try
            {
                Converters.ThresholdedBitmapToBoolArray(bitmap);
                Assert.Fail();
            }
            catch
            {
                //Do nothing, passed
            }
            finally
            {
                bitmap.Dispose();
            }
        }

        [TestMethod]
        public void TestThresholdedBitmapToBoolArray4()
        {
            //Test that an exception is thrown when an image isn't thresholded (but is 8bpp)
            Bitmap b = new Bitmap(1, 1);
            b.SetPixel(0, 0, Color.Blue);

            Bitmap greyImg = Grayscale.CommonAlgorithms.BT709.Apply(b);
            b.Dispose();

            try
            {
                Converters.ThresholdedBitmapToBoolArray(greyImg);
                Assert.Fail();
            }
            catch
            {
                //Do nothing, passed
            }
            finally
            {
                greyImg.Dispose();
            }
        }

        /*
         * BitmapToBoolArray Function Tests
         */
        [TestMethod]
        public void TestBitmapToBoolArray1()
        {
            //tests involving this function must have both black and white pixels because of the adaptive thresholding
            Bitmap b = new Bitmap(2, 1);
            b.SetPixel(0, 0, Color.Black);
            b.SetPixel(1, 0, Color.White);

            bool[,] expected = new bool[2, 1];
            expected[0, 0] = true;
            expected[1, 0] = false;

            CollectionAssert.AreEqual(expected, Converters.BitmapToBoolArray(b));

            b.Dispose();
        }

        [TestMethod]
        public void TestBitmapToBoolArray2()
        {
            //Check it works with 8bpp thresholded images
            Bitmap black = get8bppConvertedSinglePixelBitmap(Color.Black);
            Bitmap white = get8bppConvertedSinglePixelBitmap(Color.White);

            bool[,] expectedBlack = new bool[1, 1];
            expectedBlack[0, 0] = true;

            bool[,] expectedWhite = new bool[1, 1];
            expectedBlack[0, 0] = false;

            CollectionAssert.AreEqual(expectedBlack, Converters.BitmapToBoolArray(black));
            CollectionAssert.AreEqual(expectedWhite, Converters.BitmapToBoolArray(white));

            black.Dispose();
            white.Dispose();
        }
        
        [TestMethod]
        public void TestBitmapToBoolArray3()
        {
            //Test works with non-black dark colour & non-white light colour
            Bitmap b = new Bitmap(2, 1);
            b.SetPixel(0, 0, Color.DarkBlue);
            b.SetPixel(1, 0, Color.LightCyan);

            bool[,] expected = new bool[2, 1];
            expected[0, 0] = true;
            expected[1, 0] = false;

            CollectionAssert.AreEqual(expected, Converters.BitmapToBoolArray(b));

            b.Dispose();
        }

        /*
         * ThresholdedBitmapToDoubleArray Function Tests
         */
        [TestMethod]
        public void TestThresholdedBitmapToDoubleArray1()
        {
            Bitmap b = get8bppConvertedSinglePixelBitmap(Color.Black);

            double[] expected = new double[1];
            expected[0] = 0.5;

            CollectionAssert.AreEqual(expected, Converters.ThresholdedBitmapToDoubleArray(b));

            b.Dispose();
        }

        [TestMethod]
        public void TestThresholdedBitmapToDoubleArray2()
        {
            Bitmap b = get8bppConvertedSinglePixelBitmap(Color.White);

            double[] expected = new double[1];
            expected[0] = -0.5;

            CollectionAssert.AreEqual(expected, Converters.ThresholdedBitmapToDoubleArray(b));

            b.Dispose();
        }

        [TestMethod]
        public void TestThresholdedBitmapToDoubleArray3()
        {
            //Test multi-dimensional bitmap
            Bitmap b = new Bitmap(2, 2);
            b.SetPixel(0, 0, Color.Black);
            b.SetPixel(1, 0, Color.White);
            b.SetPixel(0, 1, Color.Black);
            b.SetPixel(1, 1, Color.White);

            Bitmap greyImg = Grayscale.CommonAlgorithms.BT709.Apply(b);
            Threshold threshold = new Threshold(128);
            threshold.ApplyInPlace(greyImg);
            b.Dispose();

            double[] expected = new double[] { 0.5, -0.5, 0.5, -0.5 };

            CollectionAssert.AreEqual(expected, Converters.ThresholdedBitmapToDoubleArray(greyImg));

            greyImg.Dispose();
        }

        /*
         * Private helpers
         */
        private Bitmap get8bppConvertedSinglePixelBitmap(Color colour)
        {
            Bitmap b = new Bitmap(1, 1);
            b.SetPixel(0, 0, colour);

            Bitmap greyImg = Grayscale.CommonAlgorithms.BT709.Apply(b);
            Threshold threshold = new Threshold(128);
            threshold.ApplyInPlace(greyImg);

            b.Dispose();

            return greyImg;
        }
    }
}
