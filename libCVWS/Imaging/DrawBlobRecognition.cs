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
        #region Constants

        private static readonly Color[] COLOURS = new Color[]
        {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Yellow,
            Color.Violet,
            Color.Brown,
            Color.Olive,
            Color.Cyan,
            Color.Magenta,
            Color.Gold,
            Color.Indigo,
            Color.Ivory,
            Color.HotPink,
            Color.DarkRed,
            Color.DarkGreen,
            Color.DarkBlue,
            Color.DarkSeaGreen,
            Color.Gray,
            Color.DarkKhaki,
            Color.DarkGray,
            Color.LimeGreen,
            Color.Tomato,
            Color.SteelBlue,
            Color.SkyBlue,
            Color.Silver,
            Color.Salmon,
            Color.SaddleBrown,
            Color.RosyBrown,
            Color.PowderBlue,
            Color.Plum,
            Color.PapayaWhip,
            Color.Orange
        };

        #endregion

        #region Public Methods

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

        public static unsafe void DrawInPlace(Bitmap img, BlobCounter blobCounter)
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

            // Get object labels. Each blob get a unique label & this is an array for each pixel
            //  in the image with a label saying which blob it belongs to, or 0 if no blob.
            int[] labels = blobCounter.ObjectLabels;

            int extraBytesPerRow = unmanaged.Stride - (img.Width * 3);

            byte* ptr = (byte*) unmanaged.ImageData.ToPointer();

            // for each row
            for (int y = 0, p = 0; y < img.Height; y++)
            {
                // for each pixel
                for (int x = 0; x < img.Width; x++, ptr += 3, p++)
                {
                    if (labels[p] != 0)
                    {
                        Color c = COLOURS[(labels[p] - 1) % COLOURS.Length];

                        // Uses AForge.NET defined constants to set each byte of the colour
                        ptr[RGB.R] = c.R;
                        ptr[RGB.G] = c.G;
                        ptr[RGB.B] = c.B;
                    }
                }
                ptr += extraBytesPerRow;
            }

            // Unlock the image data again
            img.UnlockBits(imgData);
        }

        #endregion
    }
}
