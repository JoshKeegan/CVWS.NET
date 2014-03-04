/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Draw Grid class - various methods to draw grids on images
 * By Josh Keegan 03/03/2014
 * Last Edit 04/03/2014
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

namespace SharedHelpers.Imaging
{
    public static class Draw
    {
        public static Bitmap DrawGrid(Bitmap img, int rows, int cols)
        {
            return DrawGrid(img, rows, cols, DrawDefaults.DEFAULT_COLOUR);
        }

        public static Bitmap DrawGrid(Bitmap imgOrig, int rows, int cols, Color colour)
        {
            Bitmap img = new Bitmap(imgOrig);
            DrawGridInPlace(img, rows, cols, colour);
            return img;
        }

        public static void DrawGridInPlace(Bitmap img, int rows, int cols)
        {
            DrawGridInPlace(img, rows, cols, DrawDefaults.DEFAULT_COLOUR);
        }

        public static void DrawGridInPlace(Bitmap img, int rows, int cols, Color colour)
        {
            //Lock image for read write so we can alter it
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadWrite, img.PixelFormat);

            //Draw lines at regular intervals along the image both vertically & horizontally
            double colWidth = img.Width / (double)cols;
            for (double i = colWidth; i < img.Width; i += colWidth)
            {
                IntPoint p1 = new IntPoint((int)i, 0);
                IntPoint p2 = new IntPoint((int)i, img.Height);
                Drawing.Line(imgData, p1, p2, colour);
            }

            double rowHeight = img.Height / (double)rows;
            for (double i = rowHeight; i < img.Height; i += rowHeight)
            {
                IntPoint p1 = new IntPoint(0, (int)i);
                IntPoint p2 = new IntPoint(img.Width, (int)i);
                Drawing.Line(imgData, p1, p2, colour);
            }

            img.UnlockBits(imgData);
        }

        public static Bitmap DrawGrid(Bitmap img, uint[,] rows, uint[,] cols)
        {
            return DrawGrid(img, rows, cols, DrawDefaults.DEFAULT_START_COLOUR, DrawDefaults.DEFAULT_END_COLOUR);
        }

        public static Bitmap DrawGrid(Bitmap imgOrig, uint[,] rows, uint[,] cols, Color startColour, Color endColour)
        {
            Bitmap img = new Bitmap(imgOrig);
            DrawGridInPlace(img, rows, cols, startColour, endColour);
            return img;
        }

        public static void DrawGridInPlace(Bitmap img, uint[,] rows, uint[,] cols)
        {
            DrawGridInPlace(img, rows, cols, DrawDefaults.DEFAULT_START_COLOUR, DrawDefaults.DEFAULT_END_COLOUR);
        }

        public static void DrawGridInPlace(Bitmap img, uint[,] rows, uint[,] cols, Color startColour, Color endColour)
        {
            //Lock image for read write so we can alter it
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadWrite, img.PixelFormat);

            //Draw lines at the start & end of each character for both rows and cols
            for (int i = 0; i < cols.GetLength(0); i++) //cols
            {
                uint start = cols[i, 0];
                uint end = cols[i, 1];

                //Start Line
                IntPoint p1 = new IntPoint((int)start, 0);
                IntPoint p2 = new IntPoint((int)start, img.Height);
                Drawing.Line(imgData, p1, p2, startColour);

                //End Line
                IntPoint p3 = new IntPoint((int)end, 0);
                IntPoint p4 = new IntPoint((int)end, img.Height);
                Drawing.Line(imgData, p3, p4, endColour);
            }

            //Rows
            for (int i = 0; i < rows.GetLength(0); i++)
            {
                uint start = rows[i, 0];
                uint end = rows[i, 1];

                //Start Line
                IntPoint p1 = new IntPoint(0, (int)start);
                IntPoint p2 = new IntPoint(img.Width, (int)start);
                Drawing.Line(imgData, p1, p2, startColour);

                //End Line
                IntPoint p3 = new IntPoint(0, (int)end);
                IntPoint p4 = new IntPoint(img.Width, (int)end);
                Drawing.Line(imgData, p3, p4, endColour);
            }

            img.UnlockBits(imgData);
        }
    }
}
