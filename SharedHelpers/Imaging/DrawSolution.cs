/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Draw Solution class
 * By Josh Keegan 14/05/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;
using AForge.Imaging;

using SharedHelpers.ImageAnalysis.WordsearchSegmentation;
using SharedHelpers.WordsearchSolver;

namespace SharedHelpers.Imaging
{
    public static class DrawSolution
    {
        public static Bitmap Solution(Bitmap img, Segmentation segmentation, Solution solution)
        {
            return Solution(img, segmentation, solution, DrawDefaults.DEFAULT_COLOUR);
        }

        public static Bitmap Solution(Bitmap origImg, Segmentation segmentation, Solution solution, Color colour)
        {
            Bitmap img = new Bitmap(origImg);
            SolutionInPlace(img, segmentation, solution, colour);
            return img;
        }

        public static void SolutionInPlace(Bitmap img, Segmentation segmentation, Solution solution)
        {
            SolutionInPlace(img, segmentation, solution, DrawDefaults.DEFAULT_COLOUR);
        }

        //Draws a solution onto a Bitmap, using a specified Segmentation to split the image into characters.
        //  Draws each line from the centre of each segmentation cell
        public static void SolutionInPlace(Bitmap img, Segmentation segmentation, Solution solution, Color colour)
        {
            //Validation: Check that the image dimensions & segmentation dimensions match
            if (img.Width != segmentation.Width || img.Height != segmentation.Height)
            {
                throw new ArgumentException("Bitmap dimensions do not match Segmentation dimensions");
            }

            //Work out the centre point for each character
            IntPoint[,] charCentres = new IntPoint[segmentation.NumCols, segmentation.NumRows];
            for(int i = 0; i < segmentation.NumCols; i++)
            {
                int colStart = (i == 0) ? 0 : segmentation.Cols[i - 1];
                int colEnd = (i == segmentation.NumCols - 1) ? segmentation.Width : segmentation.Cols[i];
                int colCentre = (int)Math.Round((((double)colEnd - colStart) / 2) + colStart);

                for(int j = 0; j < segmentation.NumRows; j++)
                {
                    int rowStart = (j == 0) ? 0 : segmentation.Rows[j - 1];
                    int rowEnd = (j == segmentation.NumRows - 1) ? segmentation.Height : segmentation.Rows[j];
                    int rowCentre = (int)Math.Round(rowStart + (((double)rowEnd - rowStart) / 2));

                    charCentres[i, j] = new IntPoint(colCentre, rowCentre);
                }
            }

            //Lock the image for write so we can alter it
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.WriteOnly, img.PixelFormat);

            //Draw on the word positions
            foreach (WordPosition position in solution.Values)
            {
                IntPoint startPoint = charCentres[position.StartCol, position.StartRow];
                IntPoint endPoint = charCentres[position.EndCol, position.EndRow];

                Drawing.Line(imgData, startPoint, endPoint, colour);
            }

            img.UnlockBits(imgData);
        }
    }
}
