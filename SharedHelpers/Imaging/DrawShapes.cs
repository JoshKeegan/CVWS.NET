/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Draw Shapes class - various methods for drawing shapes (filling in gaps in AForge.NET)
 * By Josh Keegan 04/03/2014
 * Last Edit 16/05/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;
using AForge.Imaging;

using BaseObjectExtensions;

namespace SharedHelpers.Imaging
{
    public static class DrawShapes
    {
        public static Bitmap Polygon(Bitmap origImg, List<IntPoint> points)
        {
            return Polygon(origImg, points, DrawDefaults.DEFAULT_COLOUR);
        }

        public static Bitmap Polygon(Bitmap origImg, List<IntPoint> points, Color colour)
        {
            Bitmap img = origImg.DeepCopy();
            PolygonInPlace(img, points, colour);
            return img;
        }

        public static void PolygonInPlace(Bitmap img, List<IntPoint> points)
        {
            PolygonInPlace(img, points, DrawDefaults.DEFAULT_COLOUR);
        }

        public static void PolygonInPlace(Bitmap img, List<IntPoint> points, Color colour)
        {
            //Draw a polygon in place
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadWrite, img.PixelFormat);

            Drawing.Polygon(imgData, points, colour);

            img.UnlockBits(imgData);
        }
    }
}
