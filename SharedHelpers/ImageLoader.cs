/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Shared Helpers
 * Image Loader Class
 * By Josh Keegan 26/02/2014
 * Last Edit 18/05/2014
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

using BaseObjectExtensions;

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

                //If this file has an extension
                if(parts.Length != 0)
                {
                    string extension = parts[parts.Length - 1];

                    //If this file's extension is a recognised Image format that can be loaded by the Bitmap constructor
                    if (BITMAP_FILE_EXTENSIONS.Contains(extension.ToLower()))
                    {
                        Bitmap bitmap = LoadImage(filePath);

                        toRet.Add(bitmap);
                    }
                }
            }
            return toRet;
        }

        //returns a Dictionary of hash => path
        public static Dictionary<string, string> GetAllImageHashesInSubdirs(string dirPath)
        {
            //Check for sub directories
            string[] subDirs = Directory.GetDirectories(dirPath);

            //Recursive step
            Dictionary<string, string> toRet = new Dictionary<string, string>();
            foreach (string subDir in subDirs)
            {
                Dictionary<string, string> subDirHashes = GetAllImageHashesInSubdirs(subDir);
                foreach(KeyValuePair<string, string> kvp in subDirHashes)
                {
                    toRet.Add(kvp.Key, kvp.Value);
                }
            }

            //Add the images from this Directory
            string[] files = Directory.GetFiles(dirPath);

            foreach (string filePath in files)
            {
                string[] parts = filePath.Split('.');

                //If this file has an extension
                if (parts.Length != 0)
                {
                    string extension = parts[parts.Length - 1];

                    //If this file's extension is a recognised Image format that can be loaded by the Bitmap constructor
                    if (BITMAP_FILE_EXTENSIONS.Contains(extension.ToLower()))
                    {
                        Bitmap bitmap = LoadImage(filePath);

                        toRet.Add(bitmap.GetDataHashCode(), filePath);

                        //Clean up
                        bitmap.Dispose();
                    }
                }
            }
            return toRet;
        }

        public static Bitmap LoadImage(string path)
        {
            Bitmap bitmap = new Bitmap(path);
            bitmap.Tag = path; //Store the path the file was loaded from in the Bitmap's Tag field

            return bitmap;
        }

        public static List<string> FindAllImageFileNamesInDir(string dirPath)
        {
            //Get the paths for all of the image files
            List<string> filePaths = FindAllImagePathsInDir(dirPath);

            //Get the file names from the full path
            List<string> fileNames = filePaths.Select(path => Path.GetFileName(path)).ToList();

            return fileNames;
        }

        public static List<string> FindAllImagePathsInDir(string dirPath)
        {
            //Get the paths for of all of the files in this directory
            string[] files = Directory.GetFiles(dirPath);

            List<string> imageFilePaths = new List<string>();

            //Find the image files
            foreach (string filePath in files)
            {
                string[] parts = filePath.Split('.');

                //If this file has an extension
                if(parts.Length != 0)
                {
                    string extension = parts[parts.Length - 1];

                    //If this file's extension is a recognised Image format that can be loaded by the Bitmap constructor
                    if(BITMAP_FILE_EXTENSIONS.Contains(extension.ToLower()))
                    {
                        imageFilePaths.Add(filePath);
                    }
                }
            }
            return imageFilePaths;
        }
    }
}
