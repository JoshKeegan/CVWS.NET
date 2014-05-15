/*
 * Dissertation CV Wordsearch Solver
 * 2D Array Extensions Uint Tests
 * By Josh Keegan 03/04/2014
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BaseObjectExtensions;

namespace UnitTests.BaseObjectExtensions
{
    [TestClass]
    public class _2dArrayExtensionsUnitTests
    {
        [TestMethod]
        public void TestToIntArr1()
        {
            uint[,] uintArr = new uint[,] { { 1, 3 } };
            int[,] expected = new int[,] { { 1, 3 } };
            
            CollectionAssert.AreEqual(expected, uintArr.ToIntArr());
        }

        [TestMethod]
        public void TestToIntArr2()
        {
            uint[,] uintArr = new uint[,] { { (uint)int.MaxValue + 1, 0 } };

            try
            {
                uintArr.ToIntArr();
                //No exception thrown, fail
            }
            catch(OverflowException)
            {
                //Correct exception throws, pass
            }
            catch(Exception)
            {
                //Wrong exception thrown, fail
            }
        }
    }
}
