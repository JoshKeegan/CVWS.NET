/*
 * CVWWS.NET
 * libCvws
 * DrawBlobRecognition - draws the results of an AForge.NET BlobCounter
 *  Based on AForge.NET ConnectedComponentsLabeling
 * Authors:
 *  Josh Keegan 16/04/2017
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

namespace libCVWS.Imaging
{
    public static class DrawBlobRecognition
    {
        #region Public Methods

        /// <summary>
        /// Draws the blob counter on to a blank bitmap
        /// </summary>
        public static Bitmap Draw(BlobCounter blobCounter, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            DrawInPlace(bitmap, blobCounter);
            return bitmap;
        }

        public static Bitmap Draw(Bitmap imgIn, BlobCounter blobCounter)
        {
            Bitmap imgOut;
            // If the input image pixel format is only 8bpp (so no colour), 
            //  we need to make one with a pixel format that supports colour for the labels.
            if (imgIn.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                imgOut = imgIn.Clone(new Rectangle(0, 0, imgIn.Width, imgIn.Height), PixelFormat.Format24bppRgb);
            }
            //  Otherwise, it already supports colour. Deep copy it (reusing the input pixel format)
            else
            {
                imgOut = imgIn.DeepCopy();
            }
            
            DrawInPlace(imgOut, blobCounter);
            return imgOut;
        }

        public static void DrawInPlace(Bitmap img, BlobCounter blobCounter)
        {
            // Labelling requires an RGB image
            if (img.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                throw new ArgumentException("Image pixel format must support colours", nameof(img));
            }

            // Lock image for read write so we can alter it
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadWrite, img.PixelFormat);
            UnmanagedImage unmanaged = new UnmanagedImage(imgData);

            drawInPlace(unmanaged, blobCounter);

            // Unlock the image data again
            img.UnlockBits(imgData);
        }

        public static Bitmap DrawSpecifiedBlobs(BlobCounter blobCounter, int width, int height, IEnumerable<int> blobIds)
        {
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            DrawSpecifiedBlobsInPlace(bitmap, blobCounter, blobIds);
            return bitmap;
        }

        public static void DrawSpecifiedBlobsInPlace(Bitmap img, BlobCounter blobCounter, IEnumerable<int> blobIds)
        {
            // Labelling requires an RGB image
            if (img.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                throw new ArgumentException("Image pixel format must support colours", nameof(img));
            }

            // Draw the specified blobs, using all colours
            Dictionary<int, Color> blobsToDraw = new Dictionary<int, Color>();
            int i = 0;
            foreach (int blobId in blobIds)
            {
                blobsToDraw.Add(blobId, DrawDefaults.MULTIPLE_COLOURS[i % DrawDefaults.MULTIPLE_COLOURS.Length]);
                i++;
            }

            // Lock image for read write so we can alter it
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadWrite, img.PixelFormat);
            UnmanagedImage unmanaged = new UnmanagedImage(imgData);

            drawInPlace(unmanaged, blobCounter, blobsToDraw);

            // Unlock the image data again
            img.UnlockBits(imgData);
        }

        #endregion

        #region Internal Methods

        internal static void drawInPlace(UnmanagedImage unmanaged, BlobCounter blobCounter)
        {
            // Draw all blobs, using all colours
            Blob[] blobs = blobCounter.GetObjectsInformation();
            Dictionary<int, Color> blobsToDraw = new Dictionary<int, Color>();
            for (int i = 0; i < blobs.Length; i++)
            {
                blobsToDraw.Add(blobs[i].ID, DrawDefaults.MULTIPLE_COLOURS[i % DrawDefaults.MULTIPLE_COLOURS.Length]);
            }

            drawInPlace(unmanaged, blobCounter, blobsToDraw);
        }

        internal static unsafe void drawInPlace(UnmanagedImage unmanaged, BlobCounter blobCounter,
            IDictionary<int, Color> blobsToDraw)
        {
            // Get object labels. Each blob get a unique label & this is an array for each pixel
            //  in the image with a label saying which blob it belongs to, or 0 if no blob.
            int[] labels = blobCounter.ObjectLabels;

            int extraBytesPerCol = unmanaged.Stride - (unmanaged.Width * 3);

            byte* ptr = (byte*) unmanaged.ImageData.ToPointer();

            // for each col
            for (int y = 0, p = 0; y < unmanaged.Height; y++)
            {
                // for each pixel
                for (int x = 0; x < unmanaged.Width; x++, ptr += 3, p++)
                {
                    int blobId = labels[p];
                    if (blobId != 0 && blobsToDraw.ContainsKey(blobId))
                    {
                        Color c = blobsToDraw[blobId];

                        // Uses AForge.NET defined constants to set each byte of the colour
                        ptr[RGB.R] = c.R;
                        ptr[RGB.G] = c.G;
                        ptr[RGB.B] = c.B;
                    }
                }
                ptr += extraBytesPerCol;
            }
        }

        #endregion
    }
}
