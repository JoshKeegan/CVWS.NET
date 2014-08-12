/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Base Object Extensions
 * Bitmap Extensions
 * By Josh Keegan 27/02/2014
 * Last Edit 03/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace BaseObjectExtensions
{
    public static class BitmapExtensions
    {
        //Hashing function that works on the actual Bitmap data rather than the object
        public static string GetDataHashCode(this Bitmap bitmap)
        {
            byte[] byteData = bitmap.ToByteArray();

            //MD5 Hash the bytes of the image
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(byteData);
            string strHash = Convert.ToBase64String(hash);
            return strHash;
        }

        public static int GetBitsPerPixel(this Bitmap b)
        {
            return Image.GetPixelFormatSize(b.PixelFormat);
        }

        //Represent a Bitmap as a single dimensional array of bytes. Note that this will include any padding in each row
        public static unsafe byte[] ToByteArray(this Bitmap b)
        {
            BitmapData data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                ImageLockMode.ReadOnly, b.PixelFormat);

            int numBytes = data.Stride * b.Height; //Note that because of the stride, any padding will get included
            byte[] toRet = new byte[numBytes];
            IntPtr ptrData = data.Scan0;

            Marshal.Copy(ptrData, toRet, 0, numBytes);

            //Clean up
            b.UnlockBits(data);

            return toRet;
        }

        public static Bitmap DeepCopy(this Bitmap b)
        {
            return b.Clone(new Rectangle(0, 0, b.Width, b.Height), b.PixelFormat);
        }

        //Would ideally override Bitmap's Equals (becuase it inherits it from Object)
        public static unsafe bool DataEquals(this Bitmap a, Bitmap b)
        {
            //First check that the width, height & pixel formats are the same
            if(a.Width != b.Width || a.Height != b.Height || a.PixelFormat != b.PixelFormat)
            {
                return false;
            }

            //If the objects point to the same data, no need for further checks
            if(a == b)
            {
                return true;
            }

            //Now check that the actual image data is the same
            BitmapData aData = a.LockBits(new Rectangle(0, 0, a.Width, a.Height),
                ImageLockMode.ReadOnly, a.PixelFormat);
            BitmapData bData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), 
                ImageLockMode.ReadOnly, b.PixelFormat);
            try
            {
                byte* ptrA = (byte*)aData.Scan0;
                byte* ptrB = (byte*)bData.Scan0;

                int bytesPerPixel = a.GetBitsPerPixel() / 8;

                for(int i = 0; i < a.Width; i++)
                {
                    for(int j = 0; j < a.Height; j++)
                    {
                        int offsetA = j * aData.Stride + (i * bytesPerPixel);
                        int offsetB = j * bData.Stride + (i * bytesPerPixel);
                    
                        //For each byte in this pixel
                        for (int k = 0; k < bytesPerPixel; k++)
                        {
                            byte byteA = ptrA[offsetA + k];
                            byte byteB = ptrB[offsetB + k];

                            if(byteA != byteB)
                            {
                                return false;
                            }
                        }
                    }
                }

                //Got through the entire image without finding a discrepency
                return true;
            }
            finally //Clean up
            {
                a.UnlockBits(aData);
                b.UnlockBits(bData);
            }
        }
    }
}
