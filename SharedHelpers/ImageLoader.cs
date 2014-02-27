/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Image Loader Class
 * By Josh Keegan 26/02/2014
 * Last Edit 27/02/2014
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace SharedHelpers
{
    public class ImageLoader
    {
        //Constants
        private static string[] BITMAP_FILE_EXTENSIONS = { "bmp", "gif", "jpg", "jpeg", "exif", "png", "tiff" };

        //Public Methods
        public static List<Bitmap> LoadAllImagesInSubdirs(string dirPath)
        {
            //Check for sub directories
            string[] subDirs = Directory.GetDirectories(dirPath);

            //Recursive step
            List<Bitmap> toRet = new List<Bitmap>();
            foreach(string subDir in subDirs)
            {
                toRet.AddRange(LoadAllImagesInSubdirs(subDir));
            }

            //Add the images from this Directory
            string[] files = Directory.GetFiles(dirPath);

            foreach(string filePath in files)
            {
                string[] parts = filePath.Split('.');
                string extension = parts[parts.Length - 1];

                //If this file's extension is a recognised Image format that can be loaded by the Bitmap constructor
                if (BITMAP_FILE_EXTENSIONS.Contains(extension.ToLower()))
                {
                    Bitmap bitmap = new Bitmap(new Bitmap(filePath));
                    bitmap.Tag = filePath; //Store the path the file was loaded from in the Bitmap's Tag field

                    toRet.Add(bitmap);
                }
            }
            return toRet;
        }
    }
}
