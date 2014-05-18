/*
 * Dissertation CV Wordsearch Solver
 * Regenerate Image Hashes - for when a new image hashing system has lead to the 
 *  hashes stored for images in the ImageMarkupDatabase needing to be updated
 * Program Entry Point
 * By Josh Keegan 18/05/2014
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageMarkup;
using SharedHelpers;

namespace RegenImageHashes
{
    class Program
    {
        //Constants
        private const string RAW_IMAGE_DIR = ImageMarkupDatabase.DATA_DIRECTORY_PATH + "images";

        static void Main(string[] args)
        {
            //Convert the Raw Image Directory Path to an absoulte path
            string absoluteRawImageDirectory = Path.GetFullPath(RAW_IMAGE_DIR);

            //Load the Wordsearch Database
            ImageMarkupDatabase.LoadDatabase();

            //Load the new image hashes
            Dictionary<string, string> imageHashes = ImageLoader.GetAllImageHashesInSubdirs(absoluteRawImageDirectory);

            foreach(KeyValuePair<string, string> kvp in imageHashes)
            {
                string hash = kvp.Key;
                string absolutePath = kvp.Value;
                //Get the path relative to the Raw Image Directory (what wil be stored in the database)
                string relativePath = Paths.GenerateRelativePath(absoluteRawImageDirectory, absolutePath);

                //Find the image
                Image image = ImageMarkupDatabase.GetImageByPath(relativePath);

                //Update the hash
                image.Hash = hash;
            }

            //Save the Wordsearch Database
            ImageMarkupDatabase.WriteDatabase();
        }
    }
}
