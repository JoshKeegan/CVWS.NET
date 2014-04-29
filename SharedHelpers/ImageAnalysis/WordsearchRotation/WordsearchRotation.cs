/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Rotation class - holds a Bitmap of a wordsearch and it's number of rows & cols
 *  When you rotate a wordsearch image 90 deg, the rows and cols will swap
 *  This class exists to encapsulate this behaviour
 * By Josh Keegan 25/03/2014
 * Last Edit 29/04/2014
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
        //Private Variables
        private int? rows = null;
        private int? cols = null;

        //Public Variables
        public Bitmap Bitmap;

        public int Rows
        {
            get
            {
                //If we have the number of rows stored on this object, use that
                if(rows != null)
                {
                    return rows.GetValueOrDefault(); //Will always be value from conditional
                }
                else //Otherwise we must have the segmentation, get the number of rows from there
                {
                    return Segmentation.NumRows;
                }
            }
        }

        public int Cols
        {
            get
            {
                //If we have the number of cols stored on this object, use that
                if(rows != null)
                {
                    return cols.GetValueOrDefault(); //Will always be value from conditional
                }
                else //Otherwise we must have the segmentation, get the number of cols from there
                {
                    return Segmentation.NumCols;
                }
            }
        }

        public Segmentation Segmentation { get; private set; }

        //Constructors
        public WordsearchRotation(Bitmap bitmap, int rows, int cols)
        {
            this.Bitmap = bitmap;
            this.rows = rows;
            this.cols = cols;
            this.Segmentation = null;
        }

        public WordsearchRotation(Bitmap bitmap, Segmentation segmentation)
        {
            this.Bitmap = bitmap;
            this.Segmentation = segmentation;
        }

        //Public Methods

        public void Rotate(int angleDeg)
        {
            //Validation: Check that we've been asked to rotate through a multiple of 90 deg
            if(angleDeg % 90 != 0)
            {
                throw new ArgumentException("angleDeg must be a multiple of 90");
            }

            //Rotate the Bitmap
            //Note: If this is ever to be used to rotate through arbitrary angles (not 90, 180, 270 deg) then it should decide to use
            //  interpolation or nearest neighbour rotation accordingly
            RotateNearestNeighbor rotate = new RotateNearestNeighbor(angleDeg);
            Bitmap oldBitmap = Bitmap;
            Bitmap = rotate.Apply(oldBitmap);

            //If we're working on a Segmentation object, let it handle the rotation
            if(Segmentation != null)
            {
                Segmentation.Rotate(angleDeg);
            }
            else //Otherwise we're just working on the number of rows & cols
            {
                //If the angle being rotated through is not a multiple of 180 deg (e.g. 90, 270), then the rows & cols will swap
                if (angleDeg % 180 != 0)
                {
                    //Swap rows & cols
                    int newRows = Cols;
                    cols = Rows;
                    rows = newRows;
                }
            }
            
            //Clean up
            oldBitmap.Dispose();
        }

        public WordsearchRotation DeepCopy()
        {
            //If this WordsearchRotation works on rows & cols, use them
            if(rows != null && cols != null)
            {
                return new WordsearchRotation(Bitmap.DeepCopy(), Rows, Cols);
            }
            else //Otherwise, it works with a Segmentation object
            {
                return new WordsearchRotation(Bitmap.DeepCopy(), Segmentation.DeepCopy());
            }
        }
    }
}
