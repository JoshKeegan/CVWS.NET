/*
 * Dissertation CV Wordsearch Solver
 * Image Markup
 * Wordsearch Image Class
 * By Josh Keegan 26/02/2014
 * Last Edit 05/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;

using ImageMarkup.Exceptions;
using SharedHelpers.Maths;

namespace ImageMarkup
{
    public class WordsearchImage
    {
        //Private variables
        private string wordsearchId;

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
    }
}
