/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Image Extensions Tests
 * By Josh Keegan 14/05/2014
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

using libCVWS.BaseObjectExtensions;

namespace UnitTests.libCVWS.BaseObjectExtensions
{
    [TestClass]
    public class ImageExtensionsTests
    {
        /*
         * Test IsDisposed
         */
        [TestMethod]
        public void TestIsDisposed1()
        {
            //Test with a Bitmap that has been disposed of
            Bitmap b = Helpers.getArbitraryBitmap();
            b.Dispose();

            Assert.IsTrue(b.IsDisposed());
        }

        [TestMethod]
        public void TestIsDisposed2()
        {
            //Test with a Bitmap that hasn't been disposed of
            Bitmap b = Helpers.getArbitraryBitmap();

            Assert.IsFalse(b.IsDisposed());

            //Clean up
            b.Dispose();
        }
    }
}
