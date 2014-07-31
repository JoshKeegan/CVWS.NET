/*
 * Computer Vision Wordsearch Solver
 * Shared Helpers
 * Char Img Extractor class - extract just the character from an image (removing whitespace)
 * By Josh Keegan 11/03/2014
 * Last Edit 26/04/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace SharedHelpers.Imaging
{
    public static class CharImgExtractor
    {
        //Constants
        public const int EXTRACTED_CHAR_WIDTH = 12;
        public const int EXTRACTED_CHAR_HEIGHT = 16;

        //Private variables
        private static BradleyLocalThresholding bradleyLocalThreshold = new BradleyLocalThresholding();
        private static ExtractBiggestBlob extractBiggestBlob = new ExtractBiggestBlob();
        private static ResizeNearestNeighbor resize = new ResizeNearestNeighbor(EXTRACTED_CHAR_WIDTH, EXTRACTED_CHAR_HEIGHT);
        private static Invert invert = new Invert();

        public static Bitmap[,] ExtractAll(Bitmap[,] charsIn)
        {
            Bitmap[,] toRet = new Bitmap[charsIn.GetLength(0), charsIn.GetLength(1)];

            for(int i = 0; i < charsIn.GetLength(0); i++)
            {
                for(int j = 0; j < charsIn.GetLength(1); j++)
                {
                    toRet[i, j] = Extract(charsIn[i, j]);
                }
            }

            return toRet;
        }

        //Method that simply calls the extraction method that evaluation has shown to perform best
        public static Bitmap Extract(Bitmap charIn)
        {
            return ExtractBiggestBlob(charIn);
        }

        //Actual Methods that can be used for extraction
        public static Bitmap ExtractBiggestBlob(Bitmap charIn)
        {
            //Greyscale
            Bitmap greyImg = Grayscale.CommonAlgorithms.BT709.Apply(charIn); //Use the BT709 (HDTV spec) for RGB weights

            //Adaptive threshold
            bradleyLocalThreshold.ApplyInPlace(greyImg);

            //Extract the largest blob (the character)
            invert.ApplyInPlace(greyImg); //Inversion required for blob detection (must be white object on black background)
            Bitmap charWithoutWhitespace = null;
            try
            {
                charWithoutWhitespace = extractBiggestBlob.Apply(greyImg);
            }
            catch(ArgumentException) //Thrown if no blobs in image, use whole image
            {
                charWithoutWhitespace = greyImg;
            }

            //Resize the image of the char without whitespace to some normalised size (using nearest neighbour because we've already thresholded)
            Bitmap normalisedCharWithoutWhitespace = resize.Apply(charWithoutWhitespace);

            //Clean up
            greyImg.Dispose();
            charWithoutWhitespace.Dispose();

            return normalisedCharWithoutWhitespace;
        }

        //TODO: Attempt to remove things other that the character first, e.g. the line that could be on each side of the image
        // if it's in the corner of a bounded wordsearch??
        //Don't worry about small noise as only blobs bigger than the character can cause problems
    }
}
