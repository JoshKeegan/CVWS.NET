/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS
 * Draw Shapes class - various methods for drawing shapes (filling in gaps in AForge.NET)
 * Authors: 
 *  Josh Keegan 04/03/2014
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

using libCVWS.BaseObjectExtensions;

namespace libCVWS.Imaging
{
    public static class DrawShapes
    {
        #region Polygon

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

        #endregion

        #region Polygons

        public static Bitmap Polygons(Bitmap origImg, IEnumerable<List<IntPoint>> polygons)
        {
            Bitmap img = origImg.DeepCopy();
            PolygonsInPlace(img, polygons);
            return img;
        }

        public static void PolygonsInPlace(Bitmap img, IEnumerable<List<IntPoint>> polygons)
        {
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadWrite, img.PixelFormat);

            foreach (List<IntPoint> polygon in polygons)
            {
                Drawing.Polygon(imgData, polygon, DrawDefaults.DEFAULT_COLOUR);
            }

            img.UnlockBits(imgData);
        }

        #endregion
    }
}
