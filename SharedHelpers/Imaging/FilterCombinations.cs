/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Filter Combinations class - commonly used combinations of Image Filters 
 *  applied one after the other routinely
 * By Josh Keegan 04/04/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Imaging.Filters;

namespace SharedHelpers.Imaging
{
    public static class FilterCombinations
    {
        //Adaptively threshold a Bitmap (using Bradley Local Thresholding)
        public static Bitmap AdaptiveThreshold(Bitmap img)
        {
            Bitmap greyImg;

            //If the image is 8bpp
            if (img.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                greyImg = img;
            }
            else //Otherwise the image needs converting to greyscale before any further processing
            {
                greyImg = Grayscale.CommonAlgorithms.BT709.Apply(img);
            }

            //Threshold the image
            BradleyLocalThresholding bradleyLocalThreshold = new BradleyLocalThresholding();
            Bitmap bradleyLocalImg = bradleyLocalThreshold.Apply(greyImg);

            //Clean up
            if(greyImg != img) //If we had to greyscale the input, dispose the greyscale Bitmap
            {
                greyImg.Dispose();
            }

            return bradleyLocalImg;
        }
    }
}
