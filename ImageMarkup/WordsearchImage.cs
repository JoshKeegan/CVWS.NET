/*
 * Dissertation CV Wordsearch Solver
 * Image Markup
 * Wordsearch Image Class
 * By Josh Keegan 26/02/2014
 * Last Edit 25/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using AForge;
using AForge.Imaging.Filters;

using ImageMarkup.Exceptions;
using SharedHelpers;
using SharedHelpers.Maths;
using SharedHelpers.Exceptions;
using SharedHelpers.Imaging;

namespace ImageMarkup
{
    public class WordsearchImage
    {
        //Private variables
        private string wordsearchId;
        private Bitmap bitmap;
        private uint riCountBitmap = 0; //Registered Interest in the Bitmap

        //Public Variables
        public IntPoint TopLeft { get; private set; }
        public IntPoint TopRight { get; private set; }
        public IntPoint BottomRight { get; private set; }
        public IntPoint BottomLeft { get; private set; }

        public IntPoint[] Coordinates
        {
            get
            {
                IntPoint[] toRet = new IntPoint[4];
                toRet[0] = TopLeft; //Winding Order: Clockwise starting in top left
                toRet[1] = TopRight;
                toRet[2] = BottomRight;
                toRet[3] = BottomLeft;

                return toRet;
            }
        }

        public double ScreenSpaceArea
        {
            get
            {
                return Geometry.Area(TopLeft, TopRight, BottomRight, BottomLeft);
            }
        }

        public uint Rows { get; private set; }
        public uint Cols { get; private set; }

        public string FromImageHash { get; private set; } //The hash of the image that this wordsearch image comes from

        public Image FromImage
        {
            get
            {
                return ImageMarkupDatabase.GetImage(FromImageHash);
            }
        }

        public string WordsearchId
        {
            get
            {
                //Check if we have a wordsearch ID
                if(wordsearchId != null)
                {
                    return wordsearchId;
                }
                else //Otherwise we don't have an ID for the Wordsearch contained in this image
                {
                    throw new DataNotFoundException("WordsearchID Not Found");
                }
            }
        }

        public Wordsearch Wordsearch
        {
            get
            {
                return ImageMarkupDatabase.GetWordsearch(WordsearchId);
            }
        }

        public Bitmap Bitmap
        {
            get
            {
                //If we don't have the bitmap, make it
                if(bitmap == null)
                {
                    //Check that an interest has been registered in the bitmap (not necessary but should act as an early indicator rather than letting memory leaks happen)
                    if (riCountBitmap > 0)
                    {
                        //Fetch the bitmap of the main image this wordsearch image is in
                        Image fromImage = FromImage;
                        fromImage.RegisterInterestInBitmap();

                        //Transform the region containing this wordsearch into a new image
                        QuadrilateralTransformation quadTransform = new QuadrilateralTransformation(
                            new List<IntPoint>(Coordinates));

                        bitmap = quadTransform.Apply(fromImage.Bitmap);

                        //Deregister interest in the main Image's Bitmap to allow for it to dispose of it appropriately
                        fromImage.DeregisterInterestInBitmap();
                    }
                    else //Otherwise the caller hasn't registered their interest in the resource before attempting to access it
                    {
                        throw new Exception(String.Format("Caller hasn't registered their interest in WordsearchImage of wordsearchID {0}, Image hash {1} before attempting to access it", wordsearchId, FromImageHash));
                    }
                }
                return bitmap;
            }
        }

        public Dictionary<string, string> MetaData { get; private set; }

        //Constructors
        public WordsearchImage(IntPoint topLeft, IntPoint topRight, IntPoint bottomRight, 
            IntPoint bottomLeft, uint rows, uint cols, Dictionary<string, string> metaData, 
            string fromImageHash)
            : this(topLeft, topRight, bottomRight, bottomLeft, rows, cols, metaData, fromImageHash, null) {  }

        public WordsearchImage(IntPoint topLeft, IntPoint topRight, IntPoint bottomRight,
            IntPoint bottomLeft, uint rows, uint cols, Dictionary<string, string> metaData, 
            string fromImageHash, string wordsearchId)
        {
            this.TopLeft = topLeft;
            this.TopRight = topRight;
            this.BottomRight = bottomRight;
            this.BottomLeft = bottomLeft;
            this.Rows = rows;
            this.Cols = cols;
            this.MetaData = metaData;
            this.FromImageHash = fromImageHash;
            this.wordsearchId = wordsearchId;
        }

        //Public methods
        public void RegisterInterestInBitmap()
        {
            riCountBitmap++;
        }

        public void DeregisterInterestInBitmap()
        {
            //If there is currently a registered interest in the bitmap
            if (riCountBitmap > 0)
            {
                riCountBitmap--;

                //If the bitmap is no longer in use (nobody has a registered interest in it), then dispose of it
                if (riCountBitmap == 0)
                {
                    bitmap.Dispose();
                    bitmap = null;
                }
            }
            else //Otherwise there is no registered in the bitmap object, cannot deregister
            {
                throw new RegisteredInterestException(String.Format("Cannot deregister interest in Bitmap for WordsearchImage of wordsearchID {0}, Image hash {1} as there are currently no registered interests", wordsearchId, FromImageHash));
            }
        }

        public Bitmap GetBitmapCustomResolution(int width, int height)
        {
            //Fetch the bitmap of the main image this wordsearch image is in
            Image fromImage = FromImage;
            fromImage.RegisterInterestInBitmap();

            //Transform the region containing this wordsearch into a new image
            QuadrilateralTransformation quadTransform = new QuadrilateralTransformation(
                new List<IntPoint>(Coordinates), width, height);

            Bitmap customResBitmap = quadTransform.Apply(fromImage.Bitmap);

            //Deregister interest in the main Image's Bitmap to allow for it to dispose of it appropriately
            fromImage.DeregisterInterestInBitmap();

            return customResBitmap;
        }

        //Get the Char Bitmaps using a default image size
        public Bitmap[,] GetCharBitmaps()
        {
            return GetCharBitmaps(Constants.CHAR_WITH_WHITESPACE_WIDTH, Constants.CHAR_WITH_WHITESPACE_HEIGHT);
        }

        public Bitmap[,] GetCharBitmaps(int width, int height)
        {
            int wordsearchWidth = width * (int)Cols;
            int wordsearchHeight = height * (int)Rows;

            //get a bitmap of this wordsearch image, but make the resolution correct for the desired character resolutions
            Bitmap wordsearchImageBitmap = GetBitmapCustomResolution(wordsearchWidth, wordsearchHeight);

            //Split the bitmap up into a 2D array of bitmaps
            Bitmap[,] chars = SplitImage.Grid(wordsearchImageBitmap, Rows, Cols);

            wordsearchImageBitmap.Dispose();

            return chars;
        }
    }
}
