/*
 * Dissertation CV Wordsearch Solver
 * Image Markup
 * Image class
 * By Josh Keegan 26/02/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageMarkup
{
    public class Image
    {
        //Private variables
        private Bitmap bitmap = null;

        //Public variables
        public string Path { get; private set; }
        public string Hash { get; private set; }
        public WordsearchImage[] WordsearchImages { get; private set; }
        public Dictionary<string, string> MetaData { get; private set; }

        public Bitmap Bitmap
        {
            get
            {
                //If we don't have the bitmap, load it
                if(bitmap == null)
                {
                    bitmap = new Bitmap(Path);
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
    }
}
