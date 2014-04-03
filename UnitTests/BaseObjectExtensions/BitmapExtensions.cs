/*
 * Dissertation CV Wordsearch Solver
 * Bitmap Extensions Tests
 * By Josh Keegan 03/04/2014
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AForge.Imaging.Filters;

using BaseObjectExtensions;

namespace UnitTests.BaseObjectExtensions
{
    [TestClass]
    public class BitmapExtensions
    {
        [TestMethod]
        public void TestDataEquals1()
        {
            //Test that a bitmap is the same as itself returns true
            Bitmap b = getArbitraryBitmap();

            Assert.IsTrue(b.DataEquals(b));

            //Clean Ups
            b.Dispose();
        }

        [TestMethod]
        public void TestDataEquals2()
        {
            //Test that two bitmaps that are exactly the same returns true
            Bitmap a = getArbitraryBitmap();
            Bitmap b = getArbitraryBitmap();

            Assert.IsTrue(a.DataEquals(b));

            //Clean Up
            a.Dispose();
            b.Dispose();
        }

        [TestMethod]
        public void TestDataEquals3()
        {
            //Test that two 8bpp bitmaps return true
            Bitmap a = getGreyscaleBitmap();
            Bitmap b = getGreyscaleBitmap();

            Assert.IsTrue(a.DataEquals(b));

            //Clean Up
            a.Dispose();
            b.Dispose();
        }

        [TestMethod]
        public void TestDataEquals4()
        {
            //Test that two Bitmaps of different widths return false
            Bitmap a = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
            Bitmap b = new Bitmap(2, 1, PixelFormat.Format8bppIndexed);

            Assert.IsFalse(a.DataEquals(b));

            //Clean Up
            a.Dispose();
            b.Dispose();
        }

        [TestMethod]
        public void TestDataEquals5()
        {
            //Test that two Bitmaps of different heights return false
            Bitmap a = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
            Bitmap b = new Bitmap(1, 2, PixelFormat.Format8bppIndexed);

            Assert.IsFalse(a.DataEquals(b));

            //Clean Up
            a.Dispose();
            b.Dispose();
        }

        [TestMethod]
        public void TestDataEquals6()
        {
            //Test that two Bitmaps of different Pixel Formats return false
            Bitmap a = new Bitmap(1, 1, PixelFormat.Format64bppPArgb);
            Bitmap b = new Bitmap(1, 1, PixelFormat.Format64bppArgb);

            Assert.IsFalse(a.DataEquals(b));

            //Clean Up
            a.Dispose();
            b.Dispose();
        }

        /*
         * Private Helpers
         */
        private static Bitmap getArbitraryBitmap()
        {
            Bitmap b = new Bitmap(2, 2, PixelFormat.Format32bppArgb);
            b.SetPixel(0, 0, Color.Blue);
            b.SetPixel(0, 1, Color.BurlyWood);
            b.SetPixel(1, 0, Color.Lime);
            b.SetPixel(1, 1, Color.Teal);
            return b;
        }

        private static Bitmap getGreyscaleBitmap()
        {
            //Cannot use SetPixel on Indexed bitmaps (which 8bpp must be), so make a colour one and then greyscale it
            Bitmap colourBitmap = new Bitmap(2, 1, PixelFormat.Format32bppArgb);
            colourBitmap.SetPixel(0, 0, Color.Black);
            colourBitmap.SetPixel(1, 0, Color.White);

            Bitmap greyBitmap = Grayscale.CommonAlgorithms.BT709.Apply(colourBitmap);

            colourBitmap.Dispose();

            return greyBitmap;
        }
    }
}
