/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Bitmap Extensions Tests
 * By Josh Keegan 03/04/2014
 * Last Edit 14/05/2014
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AForge.Imaging.Filters;

using libCVWS.BaseObjectExtensions;

namespace UnitTests.libCVWS.BaseObjectExtensions
{
    [TestClass]
    public class BitmapExtensionsTests
    {
        /*
         * Test DataEquals
         */
        [TestMethod]
        public void TestDataEquals1()
        {
            //Test that a bitmap is the same as itself returns true
            Bitmap b = Helpers.getArbitraryBitmap();

            Assert.IsTrue(b.DataEquals(b));

            //Clean Up
            b.Dispose();
        }

        [TestMethod]
        public void TestDataEquals2()
        {
            //Test that two bitmaps that are exactly the same returns true
            Bitmap a = Helpers.getArbitraryBitmap();
            Bitmap b = Helpers.getArbitraryBitmap();

            Assert.IsTrue(a.DataEquals(b));

            //Clean Up
            a.Dispose();
            b.Dispose();
        }

        [TestMethod]
        public void TestDataEquals3()
        {
            //Test that two 8bpp bitmaps return true
            Bitmap a = Helpers.getGreyscaleBitmap();
            Bitmap b = Helpers.getGreyscaleBitmap();

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
    }
}
