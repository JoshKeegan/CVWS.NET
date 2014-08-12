/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Base Object Extensions
 * Image Extensions
 * By Josh Keegan 14/05/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.BaseObjectExtensions
{
    public static class ImageExtensions
    {
        //Check if the Bitmap has been disposed of
        public static bool IsDisposed(this Image img)
        {
            //If we can access the Width property of the Image, it hasn't been disposed of
            try
            {
                int width = img.Width;
                return false;
            }
            catch (ArgumentException)
            {
                return true;
            }
        }
    }
}
