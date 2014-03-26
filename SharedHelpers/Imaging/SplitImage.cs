/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Split Image class - methods for splitting an image up into a collection of smaller images
 * By Josh Keegan 06/03/2014
 * Last Edit 26/03/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Imaging;

using BaseObjectExtensions;
using SharedHelpers.Exceptions;

namespace SharedHelpers.Imaging
{
    public static class SplitImage
    {
        public static Bitmap[,] Grid(Bitmap img, int rows, int cols)
        {
            if (rows < 0 || cols < 0)
            {
                throw new InvalidRowsAndColsException("Rows or Cols must not be negative");
            }

            return Grid(img, (uint)rows, (uint)cols);
        }

        public static Bitmap[,] Grid(Bitmap img, uint rows, uint cols)
        {
            //Height must be divisible by rows & width must be divisible by cols, otherwise will require interpolation and will be much slower
            if (img.Height % rows != 0 || img.Width % cols != 0)
            {
                throw new UnexpectedImageSizeException("Image height must be divisible by num rows and Image width must be divisible by num cols");
            }

            //Determine the number of Bytes Per pixel in this image
            int bytesPerPixel = img.GetBitsPerPixel() / 8;

            Bitmap[,] chars = new Bitmap[cols, rows];

            //Lock image for read so we can access it with pointers
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadOnly, img.PixelFormat);

            int colWidth = img.Width / (int)cols;
            int rowHeight = img.Height / (int)rows;

            unsafe
            {
                byte* ptrImgData = (byte*)imgData.Scan0;

                for (int col = 0; col < chars.GetLength(0); col++)
                {
                    for (int row = 0; row < chars.GetLength(1); row++)
                    {
                        //Make a new Bitmap to hold the char
                        Bitmap charBitmap = new Bitmap(colWidth, rowHeight, img.PixelFormat);

                        //Lock image for write so we can write to it
                        BitmapData charData = charBitmap.LockBits(new Rectangle(0, 0, charBitmap.Width, charBitmap.Height),
                            ImageLockMode.WriteOnly, charBitmap.PixelFormat);

                        byte* ptrCharData = (byte*)charData.Scan0;

                        //Loop over each pixel in this char to be extracted
                        for (int i = 0; i < colWidth; i++)
                        {
                            for(int j = 0; j < rowHeight; j++)
                            {
                                int imgOffset = ((row * rowHeight) + j) * imgData.Stride + (((col * colWidth) + i) * bytesPerPixel);
                                int charOffset = (j * charData.Stride) + (i * bytesPerPixel);

                                //Copy the bytes for this pixel
                                for(int k = 0; k < bytesPerPixel; k++)
                                {
                                    ptrCharData[charOffset + k] = ptrImgData[imgOffset + k];
                                }
                            }
                        }

                        charBitmap.UnlockBits(charData);
                        chars[col, row] = charBitmap;
                    }
                }
            }

            img.UnlockBits(imgData);

            return chars;
        }
    }
}
