/*
 * Dissertation CV Wordsearch Solver
 * Image Markup
 * Image class
 * By Josh Keegan 26/02/2014
 * Last Edit 05/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using SharedHelpers.Exceptions;

namespace ImageMarkup
{
    public class Image
    {
        //Private variables
        private Bitmap bitmap = null;
        private uint riCountBitmap = 0; //Registered Interest in the Bitmap

        //Public variables
        public string Path { get; private set; }
        public string Hash { get; private set; }
        public WordsearchImage[] WordsearchImages { get; private set; }
        public Dictionary<string, string> MetaData { get; private set; }

        //Bitmap disposal is managed by a registered interest system: any caller must first register their interest in the Bitmap, and tell the class when it's finished using it.
        //This allows for manual disposal of the underlying data in the Bitmap object, preventing memory leaks
        public Bitmap Bitmap
        {
            get
            {
                //If we don't have the bitmap, load it
                if(bitmap == null)
                {
                    //Check that an interest has been registered in the bitmap (not necessary but should act as an early indicator rather than letting memory leaks happen)
                    if(riCountBitmap > 0)
                    {
                        bitmap = new Bitmap(Path);
                    }
                    else //Otherwise the caller hasn't registered their interest in the resource before attempting to access it
                    {
                        throw new Exception(String.Format("Caller hasn't registered their interest in Image of hash {0} before attempting to access it", Hash));
                    }
                }
                return bitmap;
            }
        }

        //Constructor
        public Image(string path, string hash, WordsearchImage[] wordsearchImages, Dictionary<string, string> metaData)
        {
            this.Path = path;
            this.Hash = hash;
            this.WordsearchImages = wordsearchImages;
            this.MetaData = metaData;
        }

        //Public methods
        public void RegisterInterestInBitmap()
        {
            riCountBitmap++;
        }

        public void DeregisterInterestInBitmap()
        {
            //If there is currently a registered interest in the bitmap
            if(riCountBitmap > 0)
            {
                riCountBitmap--;

                //If the bitmap is no longer in use (nobody has a registered interest in it), then dispose of it
                if(riCountBitmap == 0)
                {
                    bitmap.Dispose();
                    bitmap = null;
                }
            }
            else //Otherwise there is no registered in the bitmap object, cannot deregister
            {
                throw new RegisteredInterestException(String.Format("Cannot deregister interest in Bitmap for Image with hash {0} as there are currently no registered interests", Hash));
            }
        }
    }
}
