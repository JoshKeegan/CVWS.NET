/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Draw Grid class - various methods to draw grids on images
 * By Josh Keegan 03/03/2014
 * Last Edit 05/03/2014
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

using SharedHelpers.Exceptions;

namespace SharedHelpers.Imaging
{
    public static class DrawGrid
    {
        public static Bitmap Grid(Bitmap img, int rows, int cols)
        {
            return Grid(img, rows, cols, DrawDefaults.DEFAULT_COLOUR);
        }

        public static Bitmap Grid(Bitmap img, uint rows, uint cols)
        {
            return Grid(img, rows, cols, DrawDefaults.DEFAULT_COLOUR);
        }

        public static Bitmap Grid(Bitmap imgOrig, int rows, int cols, Color colour)
        {
            Bitmap img = new Bitmap(imgOrig);
            GridInPlace(img, rows, cols, colour);
            return img;
        }

        public static Bitmap Grid(Bitmap imgOrig, uint rows, uint cols, Color colour)
        {
            Bitmap img = new Bitmap(imgOrig);
            GridInPlace(img, rows, cols, colour);
            return img;
        }

        public static void GridInPlace(Bitmap img, int rows, int cols)
        {
            GridInPlace(img, rows, cols, DrawDefaults.DEFAULT_COLOUR);
        }

        public static void GridInPlace(Bitmap img, uint rows, uint cols)
        {
            GridInPlace(img, rows, cols, DrawDefaults.DEFAULT_COLOUR);
        }

        public static void GridInPlace(Bitmap img, int rows, int cols, Color colour)
        {
            if(rows < 0 || cols < 0)
            {
                throw new InvalidRowsAndColsException("Rows or Cols must not be negative");
            }

            GridInPlace(img, (uint)rows, (uint)cols, colour);
        }

        public static void GridInPlace(Bitmap img, uint rows, uint cols, Color colour)
        {
            //Lock image for read write so we can alter it
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadWrite, img.PixelFormat);

            //Draw lines at regular intervals along the image both vertically & horizontally
            //TODO: Fix bug where sometimes draws a line at the end, other times doesn't (because of double precision)
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

        public static IntPoint[] GetImageCoordinatesForChar(int width, int height, int rows, int cols, int rowNum, int colNum)
        {
            if (rows < 0 || cols < 0 || rowNum < 0 || colNum < 0)
            {
                throw new InvalidRowsAndColsException("Rows or Cols must not be negative");
            }

            return GetImageCoordinatesForChar(width, height, (uint)rows, (uint)cols, (uint)rowNum, (uint)colNum);
        }

        public static IntPoint[] GetImageCoordinatesForChar(int width, int height, uint rows, uint cols, uint rowNum, uint colNum)
        {
            double colWidth = width / (double)cols;
            double rowHeight = height / (double)rows;

            int leftX = (int)(colWidth * colNum);
            int rightX = (int)(colWidth * (colNum + 1));

            int topY = (int)(rowHeight * rowNum);
            int bottomY = (int)(rowHeight * (rowNum + 1));

            IntPoint topLeft = new IntPoint(leftX, topY);
            IntPoint topRight = new IntPoint(rightX, topY);
            IntPoint bottomRight = new IntPoint(rightX, bottomY);
            IntPoint bottomLeft = new IntPoint(leftX, bottomY);

            IntPoint[] toRet = new IntPoint[4];
            toRet[0] = topLeft;
            toRet[1] = topRight;
            toRet[2] = bottomRight;
            toRet[3] = bottomLeft;

            return toRet;
        }

        public static Bitmap Grid(Bitmap img, uint[,] rows, uint[,] cols)
        {
            return Grid(img, rows, cols, DrawDefaults.DEFAULT_START_COLOUR, DrawDefaults.DEFAULT_END_COLOUR);
        }

        public static Bitmap Grid(Bitmap imgOrig, uint[,] rows, uint[,] cols, Color startColour, Color endColour)
        {
            Bitmap img = new Bitmap(imgOrig);
            GridInPlace(img, rows, cols, startColour, endColour);
            return img;
        }

        public static void GridInPlace(Bitmap img, uint[,] rows, uint[,] cols)
        {
            GridInPlace(img, rows, cols, DrawDefaults.DEFAULT_START_COLOUR, DrawDefaults.DEFAULT_END_COLOUR);
        }

        public static void GridInPlace(Bitmap img, uint[,] rows, uint[,] cols, Color startColour, Color endColour)
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
