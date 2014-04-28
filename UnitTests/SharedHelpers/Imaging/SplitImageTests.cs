/*
 * Dissertation CV Wordsearch Solver
 * Unit Tests
 * SharedHelpers.Imaging.SplitImage Tests
 * By Josh Keegan 28/04/2014
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BaseObjectExtensions;
using SharedHelpers.ImageAnalysis.WordsearchSegmentation;
using SharedHelpers.Imaging;

namespace UnitTests.SharedHelpers.Imaging
{
    [TestClass]
    public class SplitImageTests
    {
        /*
         * Segment Tests
         */
        [TestMethod]
        public void TestSegment1()
        {
            //Basic Test
            Segmentation s = new Segmentation(2, 2, 2, 2);

            Bitmap b = new Bitmap(2, 2, PixelFormat.Format32bppRgb);
            b.SetPixel(0, 0, Color.Red);
            b.SetPixel(1, 0, Color.Green);
            b.SetPixel(0, 1, Color.Blue);
            b.SetPixel(1, 1, Color.Magenta);

            Bitmap[,] chars = SplitImage.Segment(b, s);

            Assert.AreEqual(Color.Red.ToArgb(), chars[0, 0].GetPixel(0, 0).ToArgb());
            Assert.AreEqual(Color.Green.ToArgb(), chars[1, 0].GetPixel(0, 0).ToArgb());
            Assert.AreEqual(Color.Blue.ToArgb(), chars[0, 1].GetPixel(0, 0).ToArgb());
            Assert.AreEqual(Color.Magenta.ToArgb(), chars[1, 1].GetPixel(0, 0).ToArgb());


            //Clean up
            b.Dispose();
            chars.ToSingleDimension().DisposeAll();
        }
    }
}
