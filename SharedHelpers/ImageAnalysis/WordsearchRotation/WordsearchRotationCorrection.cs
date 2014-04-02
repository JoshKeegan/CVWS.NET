/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Rotation Correction class - takes an image containing just a Wordsearch and returns it in it's correct orientation
 * By Josh Keegan 25/03/2014
 * Last Edit 02/04/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Imaging.Filters;

using BaseObjectExtensions;
using SharedHelpers.ClassifierInterfacing;
using SharedHelpers.Imaging;

namespace SharedHelpers.ImageAnalysis.WordsearchRotation
{
    public static class WordsearchRotationCorrection
    {
        public static WordsearchRotation CorrectOrientation(WordsearchRotation wordsearchRotation, Classifier classifier) //Classifier MUST be probablistic
        {
            //Rotate the Bitmap through the 4 possible rotations
            WordsearchRotation[] rotations = new WordsearchRotation[4];

            for(int i = 0; i < rotations.Length; i++)
            {
                int angle = i * 90;

                rotations[i] = wordsearchRotation.DeepCopy();
                rotations[i].Rotate(angle);
            }

            //Calculate how likely wach rotation is to be correct
            double bestScore = double.MinValue;
            int bestIdx = -1;
            for(int i = 0; i < rotations.Length; i++)
            {
                double score = scoreBitmap(rotations[i].Bitmap, rotations[i].Rows, rotations[i].Cols, classifier);

                if(score > bestScore)
                {
                    bestScore = score;
                    bestIdx = i;
                }
            }

            //Dispose of the Wordsearch Rotations that weren't the best
            for(int i = 0; i < rotations.Length; i++)
            {
                if(i != bestIdx)
                {
                    rotations[i].Bitmap.Dispose();
                }
            }

            return rotations[bestIdx];
        }

        private static double scoreBitmap(Bitmap imgIn, int rows, int cols, Classifier classifier)
        {
            //Extract each charcater in this wordsearch, then run them through the classifier and sum the liklihoods of 
            //  the most probable class to determine an overall score for the image

            //Use standardised width & height for characters (do this by first resizing the image)
            int wordsearchWidth = Constants.CHAR_WITH_WHITESPACE_WIDTH * cols;
            int wordsearchHeight = Constants.CHAR_WITH_WHITESPACE_HEIGHT * rows;

            ResizeBicubic resize = new ResizeBicubic(wordsearchWidth, wordsearchHeight);
            Bitmap resizedImg = resize.Apply(imgIn);

            //Split the bitmap up into a 2D array of bitmaps
            Bitmap[,] chars = SplitImage.Grid(resizedImg, rows, cols);

            double score = 0;
            foreach(Bitmap charImg in chars)
            {
                //Remove all of the whitespace etc... returning an image that can be used for classification
                Bitmap extractedCharImg = CharImgExtractor.Extract(charImg);

                //Classify this bitmap
                double[] charResult = classifier.Classify(extractedCharImg);

                //Get the largest probability from the classifier output and add it to the overall score
                double largest = charResult[0];
                for(int i = 1; i < charResult.Length; i++)
                {
                    if(charResult[i] > largest)
                    {
                        largest = charResult[i];
                    }
                }

                score += largest;

                //Clean up
                extractedCharImg.Dispose();
                charImg.Dispose();
            }

            return score;
        }
    }
}
