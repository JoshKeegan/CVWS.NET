/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Rotation class - holds a Bitmap of a wordsearch and it's number of rows & cols
 *  When you rotate a wordsearch image 90 deg, the rows and cols will swap
 *  This class exists to encapsulate this behaviour
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
using SharedHelpers.ImageAnalysis.WordsearchSegmentation;

namespace SharedHelpers.ImageAnalysis.WordsearchRotation
{
    public class WordsearchRotation
    {
        public Bitmap Bitmap;
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public WordsearchRotation(Bitmap bitmap, int rows, int cols)
        {
            this.Bitmap = bitmap;
            this.Rows = rows;
            this.Cols = cols;
        }

        //TODO: Constructor taking Segmentation so that segmentations more complex than a simple number of rows & columns can be used

        public void Rotate(int angleDeg)
        {
            //Note: If this is ever to be used to rotate through arbitrary angles (not 90, 180, 270 deg) then it should decide to use
            //  interpolation or nearest neighbour rotation accordingly
            RotateNearestNeighbor rotate = new RotateNearestNeighbor(angleDeg);
            Bitmap oldBitmap = Bitmap;
            Bitmap = rotate.Apply(oldBitmap);

            //If the angle being rotated through is 90 or 270 deg, then the rows & cols will swap
            if(angleDeg % 180 != 0)
            {
                //Swap rows & cols
                int newRows = Cols;
                Cols = Rows;
                Rows = newRows;
            }
            
            //Clean up
            oldBitmap.Dispose();
        }

        public WordsearchRotation DeepCopy()
        {
            return new WordsearchRotation(Bitmap.DeepCopy(), Rows, Cols);
        }
    }
}
