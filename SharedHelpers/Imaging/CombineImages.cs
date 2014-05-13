/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Combine Images Class - methods for combining multiple images into a single larger image
 * By Josh Keegan 13/05/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BaseObjectExtensions;
using SharedHelpers.Exceptions;

namespace SharedHelpers.Imaging
{
    public static class CombineImages
    {
        //Takes a 2D array of Bitmaps and makes them into a single Bitmap by putting them together in a grid pattern
        //  Note that this method requires all of the images to be the same dimensions
        public static Bitmap Grid(Bitmap[,] imgs)
        {
            //Validation: Check that there is at least one image
            if(imgs.Length == 0)
            {
                throw new ArgumentException("Collection \"imgs\" must have at least one element");
            }

            //Validation: Check that all of the images are the same width & height. Also check that they are all in the same PixelFormat
            int width = imgs[0, 0].Width;
            int height = imgs[0, 0].Height;
            PixelFormat pixelFormat = imgs[0, 0].PixelFormat;
            foreach(Bitmap image in imgs)
            {
                if(image.Width != width || image.Height != height)
                {
                    throw new InvalidImageDimensionsException("All images must have the same dimensions in order to form a regular grid");
                }

                if(image.PixelFormat != pixelFormat)
                {
                    throw new UnexpectedPixelFormatException("All images must have the same PixelFormat");
                }
            }

            //Determine the number of bytes per pixel 
            int bytesPerPixel = imgs[0, 0].GetBitsPerPixel() / 8;

            Bitmap img = new Bitmap(width * imgs.GetLength(0), 
                height * imgs.GetLength(1), imgs[0, 0].PixelFormat);

            //Lock image for write so we can access it with pointers
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.WriteOnly, img.PixelFormat);

            unsafe
            {
                byte* ptrImgData = (byte*)imgData.Scan0;

                for(int col = 0; col < imgs.GetLength(0); col++)
                {
                    for(int row = 0; row < imgs.GetLength(1); row++)
                    {
                        //Lock this char for read so we can copy it accross to the new image
                        BitmapData charData = imgs[col, row].LockBits(new Rectangle(0, 0, width, height),
                            ImageLockMode.ReadOnly, pixelFormat);

                        byte* ptrCharData = (byte*)charData.Scan0;

                        //Loop over each pixel in this char to be extracted
                        for (int i = 0; i < width; i++)
                        {
                            for(int j = 0; j < height; j++)
                            {
                                int imgOffset = ((row * height) + j) * imgData.Stride + (((col * width) + i) * bytesPerPixel);
                                int charOffset = (j * charData.Stride) + (i * bytesPerPixel);

                                //Copy the bytes for this pixel
                                for(int k = 0; k < bytesPerPixel; k++)
                                {
                                    ptrImgData[imgOffset + k] = ptrCharData[charOffset + k];
                                }
                            }
                        }

                        imgs[col, row].UnlockBits(charData);
                    }
                }
            }

            img.UnlockBits(imgData);

            return img;
        }
    }
}
