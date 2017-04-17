/*
 * CVWS.NET
 * libCvws
 * DrawBlobLattice
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
using libCVWS.ImageAnalysis.WordsearchDetection;

namespace libCVWS.Imaging
{
    internal static class DrawBlobLattice
    {
        #region Constants

        private static readonly Color CONNECTION_COLOUR = DrawDefaults.DEFAULT_COLOUR;

        private static readonly Color[] BLOB_COLOURS = DrawDefaults.MULTIPLE_COLOURS
            .Where(c => c != CONNECTION_COLOUR)
            .ToArray();

        #endregion

        #region Public Methods

        /// <summary>
        /// Draws the blob lattice on to a blank bitmap
        /// </summary>
        public static Bitmap Draw(BlobCounter blobCounter, BlobLatticeElement[] lattice, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            DrawInPlace(bitmap, blobCounter, lattice);
            return bitmap;
        }

        public static Bitmap Draw(Bitmap imgIn, BlobCounter blobCounter, BlobLatticeElement[] lattice)
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

            DrawInPlace(imgOut, blobCounter, lattice);
            return imgOut;
        }

        public static void DrawInPlace(Bitmap img, BlobCounter blobCounter, BlobLatticeElement[] lattice)
        {
            // Labelling requires an RGB image
            if (img.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                throw new ArgumentException("Image pixel format must support colours", nameof(img));
            }

            // Get object labels. Each blob get a unique label &this is an array for each pixel
            //  in the image with a label saying which blob it belongs to, or 0 if no blob.
            int[] labels = blobCounter.ObjectLabels;

            // Construct a dictionary of which blob IDs to draw => index in lattice array
            //  This is necessary because not all blobs in the blob counter will be in this lattice
            //  and we want the index because we want to use as much of the colour pallette as possible
            //  before re-using a colour
            Dictionary<int, int> blobsToDraw = new Dictionary<int, int>();
            for (int i = 0; i < lattice.Length; i++)
            {
                blobsToDraw.Add(lattice[i].Blob.Blob.ID, i);
            }

            // Lock image for read write so we can alter it
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadWrite, img.PixelFormat);
            UnmanagedImage unmanaged = new UnmanagedImage(imgData);

            int extraBytesPerRow = unmanaged.Stride - (img.Width * 3);

            unsafe
            {
                byte* ptr = (byte*) unmanaged.ImageData.ToPointer();

                // for each row
                for (int y = 0, p = 0; y < img.Height; y++)
                {
                    // for each pixel
                    for (int x = 0; x < img.Width; x++, ptr += 3, p++)
                    {
                        if (labels[p] != 0 && blobsToDraw.ContainsKey(labels[p]))
                        {
                            Color c = BLOB_COLOURS[blobsToDraw[labels[p]] % BLOB_COLOURS.Length];

                            // Uses AForge.NET defined constants to set each byte of the colour
                            ptr[RGB.R] = c.R;
                            ptr[RGB.G] = c.G;
                            ptr[RGB.B] = c.B;
                        }
                    }
                    ptr += extraBytesPerRow;
                }
            }

            // Draw connections between lattice elements
            //  Note that each line will be drawn twice. This could be avoided by tracking which connections we've drawn if this ever needed optimising
            foreach (BlobLatticeElement from in lattice)
            {
                foreach (BlobLatticeElement to in from.ConnectedTo)
                {
                    Drawing.Line(unmanaged, from.Blob.Blob.CenterOfGravity.Round(),
                        to.Blob.Blob.CenterOfGravity.Round(), CONNECTION_COLOUR);
                }
            }

            // Unlock the image data again
            img.UnlockBits(imgData);
        }

        #endregion
    }
}
