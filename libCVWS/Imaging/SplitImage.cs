/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS
 * Split Image class - methods for splitting an image up into a collection of smaller images
 * By Josh Keegan 06/03/2014
 * Last Edit 28/04/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Imaging;

using libCVWS.BaseObjectExtensions;
using libCVWS.Exceptions;
using libCVWS.ImageAnalysis.WordsearchSegmentation;

namespace libCVWS.Imaging
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

        public static Bitmap[,] Segment(Bitmap img, Segmentation segmentation)
        {
            //Validation: Check that the Bitmap has the dimensions listed in the Segmentation
            if(img.Width != segmentation.Width || img.Height != segmentation.Height)
            {
                throw new ArgumentException("Bitmap dimensions must match Segmentation dimensions");
            }

            //Determine the number of Bytes Per pixel in this image
            int bytesPerPixel = img.GetBitsPerPixel() / 8;

            Bitmap[,] chars = new Bitmap[segmentation.NumCols, segmentation.NumRows];

            //Lock the image for read so we can access it with pointers
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadOnly, img.PixelFormat);

            unsafe
            {
                byte* ptrImageData = (byte*)imgData.Scan0;

                for (int col = 0; col < chars.GetLength(0); col++)
                {
                    for(int row = 0; row < chars.GetLength(1); row++)
                    {
                        //Get the start & end indices of this char based on the segmentation points
                        int colStart = col == 0 ? 0 : segmentation.Cols[col - 1]; //If this is the first col, it starts at zero. Otherwise where the previous col ended
                        int colEnd = col == chars.GetLength(0) - 1 ? segmentation.Width - 1 : segmentation.Cols[col]; //If this is the last col, ends at width. Otherwise at the segmentation index

                        int rowStart = row == 0 ? 0 : segmentation.Rows[row - 1]; //If this is the first row, it starts at zero. Otherwise where the previous row ended
                        int rowEnd = row == chars.GetLength(1) - 1 ? segmentation.Height - 1 : segmentation.Rows[row]; //If this is the last row, ends at height. Otherwise at the segmentation index

                        //Make a new Bitmap to hold the char
                        Bitmap charBitmap = new Bitmap(colEnd + 1 - colStart, rowEnd + 1 - rowStart, img.PixelFormat);

                        //Lock image for write so we can write to it
                        BitmapData charData = charBitmap.LockBits(new Rectangle(0, 0, charBitmap.Width, charBitmap.Height),
                            ImageLockMode.WriteOnly, charBitmap.PixelFormat);

                        byte* ptrCharData = (byte*)charData.Scan0;

                        //Loop over each pixel in this char to be extracted
                        for(int pxCol = colStart; pxCol <= colEnd; pxCol++)
                        {
                            for(int pxRow = rowStart; pxRow <= rowEnd; pxRow++)
                            {
                                int imgOffset = (pxRow * imgData.Stride) + (pxCol * bytesPerPixel);
                                int charOffset = ((pxRow - rowStart) * charData.Stride) + ((pxCol - colStart) * bytesPerPixel);

                                //Copy the bytes for this pixel
                                for(int i = 0; i < bytesPerPixel; i++)
                                {
                                    ptrCharData[charOffset + i] = ptrImageData[imgOffset + i];
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
