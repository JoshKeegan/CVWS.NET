/*
 * Dissertation CV Wordsearch Solver
 * Base Object Extensions
 * Bitmap Extensions
 * By Josh Keegan 27/02/2014
 * Last Edit 25/03/2014
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
    }
}
