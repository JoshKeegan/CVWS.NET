/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS
 * Converters class - convert Images to other formats
 * By Josh Keegan 06/03/2014
 * Last Edit 04/04/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Imaging.Filters;

using libCVWS.Exceptions;

namespace libCVWS.Imaging
{
    public static class Converters
    {
        public static bool[,] BitmapToBoolArray(Bitmap img)
        {
            Bitmap bradleyLocalImg = FilterCombinations.AdaptiveThreshold(img);

            //Convert to bool array
            bool[,] toRet = ThresholdedBitmapToBoolArray(bradleyLocalImg);

            //Clean up
            bradleyLocalImg.Dispose();

            return toRet;
        }

        public static unsafe bool[,] ThresholdedBitmapToBoolArray(Bitmap img)
        {
            //Check the image is greyscale/8bpp (required in order to be thresholded)
            if(img.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                throw new UnexpectedPixelFormatException("Bitmap.PixelFormat must be 8bpp (greyscale) in order for it to be thresholded");
            }

            bool[,] boolImg = new bool[img.Width, img.Height];

            BitmapData data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadOnly, img.PixelFormat);

            byte* ptrData = (byte*)data.Scan0;

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    int offset = j * data.Stride + i; // If the image wasn't 8bpp would need to multiply i by the number of BYTES per pixel (as the offset is in bytes not bits)
                    byte px = ptrData[offset];

                    //0 = on, 255 = off
                    if(px == 0)
                    {
                        boolImg[i, j] = true;
                    }
                    else if(px == 255)
                    {
                        boolImg[i, j] = false;
                    }
                    else
                    {
                        throw new UnexpectedPixelFormatException("Bitmap must be thresholded! Contains pixel value " + px);
                    }
                }
            }

            img.UnlockBits(data);

            return boolImg;
        }

        //Convert a thresholded bitmap to a double[] (numbers are around 0, used to improve performance in neural networks)
        public static double[] ThresholdedBitmapToDoubleArray(Bitmap img)
        {
            bool[,] boolImg = ThresholdedBitmapToBoolArray(img);

            double[] dblImg = new double[boolImg.GetLength(0) * boolImg.GetLength(1)];

            for(int i = 0; i < boolImg.GetLength(0); i++)
            {
                for(int j = 0; j < boolImg.GetLength(1); j++)
                {
                    dblImg[(j * boolImg.GetLength(0)) + i] = boolImg[i, j] ? 0.5 : -0.5;
                }
            }

            return dblImg;
        }

        public static double[,] ThresholdedBitmapTo2DDoubleArray(Bitmap img)
        {
            bool[,] boolImg = ThresholdedBitmapToBoolArray(img);

            double[,] dblImg = new double[boolImg.GetLength(0), boolImg.GetLength(1)];

            for(int i = 0; i < dblImg.GetLength(0); i++)
            {
                for(int j = 0; j < dblImg.GetLength(1); j++)
                {
                    dblImg[i, j] = boolImg[i, j] ? 0.5 : -0.5;
                }
            }

            return dblImg;
        }
    }
}
