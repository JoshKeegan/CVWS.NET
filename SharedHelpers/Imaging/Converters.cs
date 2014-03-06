/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Converters class - convert Images to other formats
 * By Josh Keegan 06/03/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedHelpers.Imaging.Exceptions;

namespace SharedHelpers.Imaging
{
    public static class Converters
    {
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
    }
}
