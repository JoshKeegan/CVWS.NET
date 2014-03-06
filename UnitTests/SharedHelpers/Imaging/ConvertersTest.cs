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
