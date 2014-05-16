/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Combine Images Class - methods for combining multiple images into a single larger image
 * By Josh Keegan 13/05/2014
 * Last Edit 16/05/2014
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
        public static Bitmap Grid(Bitmap[,] charImgs)
        {
            //Validation: Check that there is at least one image
            if(charImgs.Length == 0)
            {
                throw new ArgumentException("Collection \"charImgs\" must have at least one element");
            }

            //Validation: Check that all images are in the same PixelFormat
            PixelFormat pixelFormat = charImgs[0, 0].PixelFormat;
            foreach(Bitmap charImg in charImgs)
            {
                if(charImg.PixelFormat != pixelFormat)
                {
                    throw new UnexpectedPixelFormatException("All images must have the same PixelFormat");
                }
            }

            //Validation: Check that all images in each column have the same width
            for(int i = 0; i < charImgs.GetLength(0); i++) //Cols
            {
                int colWidth = charImgs[i, 0].Width;

                for(int j = 1; j < charImgs.GetLength(1); j++) //Rows
                {
                    if(charImgs[i, j].Width != colWidth)
                    {
                        throw new InvalidImageDimensionsException("All images in each column must have the same Width in order to form a grid");
                    }
                }
            }

            //Validation: Check that all images in each row have the same height
            for(int i = 0; i < charImgs.GetLength(1); i++) //Row
            {
                int rowHeight = charImgs[0, i].Height;

                for(int j = 1; j < charImgs.GetLength(0); j++) //Col
                {
                    if(charImgs[j, i].Height != rowHeight)
                    {
                        throw new InvalidImageDimensionsException("All images in each row must have the same Height in order to form a grid");
                    }
                }
            }

            //Determine the number of bytes per pixel
            int bytesPerPixel = charImgs[0, 0].GetBitsPerPixel() / 8;

            //Calculate the total image Width & Height
            int imgWidth = 0;
            for(int i = 0; i < charImgs.GetLength(0); i++) //Cols
            {
                imgWidth += charImgs[i, 0].Width;
            }

            int imgHeight = 0;
            for(int i = 0; i < charImgs.GetLength(1); i++) //Rows
            {
                imgHeight += charImgs[0, i].Height;
            }

            Bitmap img = new Bitmap(imgWidth, imgHeight, pixelFormat);

            //Lock image for write so we can access it with pointers
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.WriteOnly, img.PixelFormat);

            unsafe
            {
                byte* ptrImgData = (byte*)imgData.Scan0;

                //Keep track of how many pixels in from the left we are to the left of this col
                int colLeftIdx = 0;

                for(int col = 0; col < charImgs.GetLength(0); col++)
                {
                    //Keep track of how many pixels in from the top we are to the top of this row
                    int rowTopIdx = 0;

                    for(int row = 0; row < charImgs.GetLength(1); row++)
                    {
                        //Lock this char for read so we can copy it across to the new image
                        BitmapData charData = charImgs[col, row].LockBits(new Rectangle(0, 0, 
                            charImgs[col, row].Width, charImgs[col, row].Height), ImageLockMode.ReadOnly, pixelFormat);

                        byte* ptrCharData = (byte*)charData.Scan0;

                        //Loop over each pixel in this char img to be copied across
                        for (int i = 0; i < charImgs[col, row].Width; i++) //Cols
                        {
                            for(int j = 0; j < charImgs[col, row].Height; j++) //Rows
                            {
                                int imgOffset = (((rowTopIdx) + j) * imgData.Stride) + ((colLeftIdx + i) * bytesPerPixel);
                                int charImgOffset = (j * charData.Stride) + (i * bytesPerPixel);

                                //Copy the bytes for this pixel
                                for(int k = 0; k < bytesPerPixel; k++)
                                {
                                    ptrImgData[imgOffset + k] = ptrCharData[charImgOffset + k];
                                }
                            }
                        }

                        charImgs[col, row].UnlockBits(charData);

                        //Update the rowLeftIdx for the next iter
                        rowTopIdx += charImgs[col, row].Height;
                    }

                    //Update the colLeftIdx for the next iter
                    colLeftIdx += charImgs[col, 0].Width;
                }
            }

            img.UnlockBits(imgData);

            return img;
        }
    }
}
