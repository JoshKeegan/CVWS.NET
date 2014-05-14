/*
 * Dissertation CV Wordsearch Solver
 * Unit Tests Helpers
 * By Josh Keegan 14/05/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Imaging.Filters;

namespace UnitTests
{
    internal static class Helpers
    {
        internal static Bitmap getArbitraryBitmap()
        {
            Bitmap b = new Bitmap(2, 2, PixelFormat.Format32bppArgb);
            b.SetPixel(0, 0, Color.Blue);
            b.SetPixel(0, 1, Color.BurlyWood);
            b.SetPixel(1, 0, Color.Lime);
            b.SetPixel(1, 1, Color.Teal);
            return b;
        }

        internal static Bitmap getGreyscaleBitmap()
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
