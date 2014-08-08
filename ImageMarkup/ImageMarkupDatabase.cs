/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Image Markup
 * Image Markup Database - Class representing the entire dataset of marked up word searches
 * By Josh Keegan 26/02/2014
 * Last Edit 18/05/2014
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using AForge;

using ImageMarkup.Exceptions;

namespace ImageMarkup
{
    public static class ImageMarkupDatabase
    {
        //Constants
        public const string DATA_DIRECTORY_PATH = "../../../data/";
        private const string XML_DOC_LOCATION = DATA_DIRECTORY_PATH + "markup.xml";

        private const string XML_ROOT_EL = "ImageMarkupData";

        private const string XML_IMAGES_EL = "Images";
        private const string XML_IMAGE_EL = "Image";
        private const string XML_IMAGE_FILE_PATH_EL = "FilePath";
        private const string XML_IMAGE_HASH_EL = "Hash";
        private const string XML_IMAGE_METADATA_EL = "MetaData";

        private const string XML_WORDSEARCH_IMAGES_EL = "WordsearchImages";
        private const string XML_WORDSEARCH_IMAGE_EL = "WordsearchImage";
        private const string XML_WORDSEARCH_IMAGE_COORDINATES_EL = "Coordinates";
        private const string XML_WORDSEARCH_IMAGE_ROWS_EL = "Rows";
        private const string XML_WORDSEARCH_IMAGE_COLS_EL = "Cols";
        private const string XML_WORDSEARCH_IMAGE_WORDSEARCH_ID_EL = "WordsearchId";
        private const string XML_WORDSEARCH_IMAGE_METADATA_EL = "MetaData";

        private const string XML_WORDSEARCHES_EL = "Wordsearches";
        private const string XML_WORDSEARCH_EL = "Wordsearch";
        private const string XML_WORDSEARCH_ID_EL = "ID";
        private const string XML_WORDSEARCH_CHARS_EL = "Chars";
        private const string XML_WORDSEARCH_WORDS_EL = "Words";
        private const string XML_WORDSEARCH_WORD_EL = "Word";

        //Private Variables
        private static Dictionary<string, Image> images = null;
        private static Dictionary<string, Wordsearch> wordsearches = null;

        //Public Variables
        public static bool Loaded //Is the database currently loaded into memory
        {
            get
            {
                return images != null && wordsearches != null;
            }
        }

        //Public Methods
        public static void LoadDatabase()
        {
            //Check if we have a file for the database
            if (File.Exists(XML_DOC_LOCATION))
            {
                loadDatabase();
            }
            else //Otherwise the file doesn't exist, make a blank database
            {
                //Set all of the local variables to empty defaults
                images = new Dictionary<string, Image>();
                wordsearches = new Dictionary<string, Wordsearch>();

                //Write the database to file
                writeDatabase();
            }
        }

        public static void WriteDatabase()
        {
            //If the dataset has been loaded
            if(images != null && wordsearches != null)
            {
                writeDatabase();
            }
            else //Otherwise the data hasn't been loaded
            {
                throw new DatabaseNotInitialisedException();
            }
        }

        public static void AddImage(Image image)
        {
            AddImage(image.Hash, image);
        }

        public static void AddImage(string hash, Image image)
        {
            //If the dataset has been loaded
            if (images != null)
            {
                images.Add(hash, image);
            }
            else //Otherwise the data hasn't been loaded
            {
                throw new DatabaseNotInitialisedException();
            }

        }

        public static List<Image> GetImages()
        {
            //If the dataset has been loaded
            if(images != null)
            {
                return images.Values.ToList();
            }
            else //Otherwise the data hasn't been loaded 
            {
                throw new DatabaseNotInitialisedException();
            }
        }

        public static Image GetImage(string hash)
        {
            //If the dataset has been loaded
            if (images != null)
            {
                //Try and get the image being searched for
                try
                {
                    Image toRet = images[hash];
                    return toRet;
                }
                catch
                {
                    throw new DataNotFoundException(String.Format("Image with hash \"{0}\" not found", hash));
                }
            }
            else //Otherwise the data hasn't been loaded
            {
                throw new DatabaseNotInitialisedException();
            }
        }

        public static Image GetImageByPath(string relativePath)
        {
            //If the dataset has been loaded
            if (images != null)
            {
                //Find the image being searched for
                foreach (Image image in images.Values)
                {
                    if (image.Path == relativePath)
                    {
                        return image;
                    }
                }

                throw new DataNotFoundException(String.Format("Image at path \"{0}\" not found in database", relativePath));
            }
            else //Otherwise the data hasn't been loaded
            {
                throw new DatabaseNotInitialisedException();
            }
        }

        public static bool ContainsImage(string hash)
        {
            //If the dataset has been loaded
            if(images != null)
            {
                return images.ContainsKey(hash);
            }
            else //Otherwise the data hasn't been loaded
            {
                throw new DatabaseNotInitialisedException();
            }
        }

        public static void RemoveImage(string hash)
        {
            //If the dataset has been loaded
            if(images != null)
            {
                images.Remove(hash);
            }
            else //Otherwise the data hasn't been loaded
            {
                throw new DatabaseNotInitialisedException();
            }
        }

        public static void AddWordsearch(Wordsearch wordsearch)
        {
            AddWordsearch(wordsearch.Id, wordsearch);
        }

        public static void AddWordsearch(string wordsearchId, Wordsearch wordsearch)
        {
            //If the dataset has been loaded
            if(wordsearches != null)
            {
                wordsearches.Add(wordsearchId, wordsearch);
            }
            else //Otherwise the data hasn't been loaded
            {
                throw new DatabaseNotInitialisedException();
            }
        }

        public static Wordsearch GetWordsearch(string wordsearchId)
        {
            //If the dataset has been loaded
            if (wordsearches != null)
            {
                //try and get the wordsearch being searched for
                try
                {
                    return wordsearches[wordsearchId];
                }
                catch
                {
                    throw new DataNotFoundException(String.Format("Wordsearch with ID \"{0}\" not found", wordsearchId));
                }
            }
            else //Otherwise the data hasn't been loaded
            {
                throw new DatabaseNotInitialisedException();
            }
        }

        public static bool ContainsWordsearch(string wordsearchId)
        {
            //If the dataset has been loaded
            if (wordsearches != null)
            {
                return wordsearches.ContainsKey(wordsearchId);
            }
            else //Otherwise the data hasn't been loaded
            {
                throw new DatabaseNotInitialisedException();
            }
        }

        public static void RemoveWordsearch(string wordsearchId)
        {
            //If the dataset has been loaded
            if(wordsearches != null)
            {
                wordsearches.Remove(wordsearchId);
            }
            else //Otherwise the data hasn't been loaded
            {
                throw new DatabaseNotInitialisedException();
            }
        }

        public static List<WordsearchImage> GetWordsearchImages()
        {
            //If the dataaset has been loaded
            if(images != null)
            {
                List<WordsearchImage> wordsearchImages = new List<WordsearchImage>();

                foreach (Image image in images.Values)
                {
                    wordsearchImages.AddRange(image.WordsearchImages);
                }

                return wordsearchImages;
            }
            else //Otherwise the data hasn't been loaded
            {
                throw new DatabaseNotInitialisedException();
            }
        }

        //Note: because of data structures will be considerably slower than other lookups available. If extensive usage of this is required
        //consider an extra data structure for mapping wordsearchIds => WordsearchImages
        public static List<WordsearchImage> GetWordsearchImages(string wordsearchId)
        {
            //If the dataset has been loaded
            if(images != null)
            {
                List<WordsearchImage> toRet = new List<WordsearchImage>();

                foreach (Image image in images.Values)
                {
                    foreach (WordsearchImage wordsearchImage in image.WordsearchImages)
                    {
                        try
                        {
                            if (wordsearchImage.WordsearchId == wordsearchId)
                            {
                                toRet.Add(wordsearchImage);
                            }
                        }
                        catch { }
                    }
                }

                return toRet;
            }
            else //Otherwise the data hasn't been loaded
            {
                throw new DatabaseNotInitialisedException();
            }
        }

        //Get the first WordsearchImage of the specified wordsearch
        public static WordsearchImage GetWordsearchImage(string wordsearchId)
        {
            //If the dataset has been loaded
            if(images != null)
            {
                foreach (Image image in images.Values)
                {
                    foreach (WordsearchImage wordsearchImage in image.WordsearchImages)
                    {
                        try
                        {
                            if (wordsearchImage.WordsearchId == wordsearchId)
                            {
                                return wordsearchImage;
                            }
                        }
                        catch { }
                    }
                }

                throw new DataNotFoundException(String.Format("WordsearchImages with WordsearchID {0} not found", wordsearchId));
            }
            else //Otherwise the data hasn't been loaded
            {
                throw new DatabaseNotInitialisedException();
            }
        }

        //Get the best WordsearchImage of the specified wordsearch (currently based on area, extra metrics could be added later, e.g. checks for metadata about lighting etc...)
        public static WordsearchImage GetClearestWordsearchImage(string wordsearchId)
        {
            List<WordsearchImage> wordsearchImages = GetWordsearchImages(wordsearchId);

            //check there are some WordsearchImages with this wordsearch ID
            if(wordsearchImages.Count == 0)
            {
                throw new DataNotFoundException(String.Format("WordsearchImages with WordsearchID {0} not found", wordsearchId));
            }

            //Find the image with the largest screen space area
            double largest = wordsearchImages[0].ScreenSpaceArea;
            int largestIdx = 0;

            for(int i = 1; i < wordsearchImages.Count; i++)
            {
                double area = wordsearchImages[i].ScreenSpaceArea;
                if(wordsearchImages[i].ScreenSpaceArea > largest)
                {
                    largest = area;
                    largestIdx = i;
                }
            }

            return wordsearchImages[largestIdx];
        }

        //Get a list of wordsearchID's that need marking up
        public static HashSet<string> GetWordsearchIDsToBeMarkedUp()
        {
            HashSet<string> toRet = new HashSet<string>();

            foreach(Image image in images.Values)
            {
                foreach(WordsearchImage wordsearchImage in image.WordsearchImages)
                {
                    if(!ContainsWordsearch(wordsearchImage.WordsearchId)
                        && !toRet.Contains(wordsearchImage.WordsearchId))
                    {
                        toRet.Add(wordsearchImage.WordsearchId);
                    }
                }
            }

            return toRet;
        }

        //Private Methods
        private static void loadDatabase()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XML_DOC_LOCATION);

            XmlNode rootNode = xmlDoc.SelectSingleNode(XML_ROOT_EL);
            
            //Images
            XmlNode imagesNode = rootNode.SelectSingleNode(XML_IMAGES_EL);
            XmlNodeList imageNodes = imagesNode.SelectNodes(XML_IMAGE_EL);

            images = new Dictionary<string, Image>();
            foreach(XmlNode imageNode in imageNodes)
            {
                string path = imageNode.SelectSingleNode(XML_IMAGE_FILE_PATH_EL).InnerText;
                string hash = imageNode.SelectSingleNode(XML_IMAGE_HASH_EL).InnerText;

                //Wordsearch Images
                XmlNode wordsearchImagesNode = imageNode.SelectSingleNode(XML_WORDSEARCH_IMAGES_EL);
                XmlNodeList wordsearcheImageNodes = wordsearchImagesNode.SelectNodes(XML_WORDSEARCH_IMAGE_EL);
                List<WordsearchImage> wordsearchImages = new List<WordsearchImage>();
                foreach(XmlNode wiNode in wordsearcheImageNodes)
                {
                    //Coordinates
                    string strCoords = wiNode.SelectSingleNode(XML_WORDSEARCH_IMAGE_COORDINATES_EL).InnerText;
                    string[] pairs = strCoords.Trim().Split('.');

                    char[] trimChars = new char[] { '(', ')', ' ' };
                    
                    string[] parts = pairs[0].Split(',');
                    string x = parts[0].Trim(trimChars);
                    string y = parts[1].Trim(trimChars);
                    IntPoint topLeft = new IntPoint(int.Parse(x), int.Parse(y));

                    parts = pairs[1].Split(',');
                    x = parts[0].Trim(trimChars);
                    y = parts[1].Trim(trimChars);
                    IntPoint topRight = new IntPoint(int.Parse(x), int.Parse(y));

                    parts = pairs[2].Split(',');
                    x = parts[0].Trim(trimChars);
                    y = parts[1].Trim(trimChars);
                    IntPoint bottomRight = new IntPoint(int.Parse(x), int.Parse(y));

                    parts = pairs[3].Split(',');
                    x = parts[0].Trim(trimChars);
                    y = parts[1].Trim(trimChars);
                    IntPoint bottomLeft = new IntPoint(int.Parse(x), int.Parse(y));

                    uint rows = uint.Parse(wiNode.SelectSingleNode(XML_WORDSEARCH_IMAGE_ROWS_EL).InnerText);
                    uint cols = uint.Parse(wiNode.SelectSingleNode(XML_WORDSEARCH_IMAGE_COLS_EL).InnerText);

                    //Meta Data
                    XmlNode metaDataNode = wiNode.SelectSingleNode(XML_WORDSEARCH_IMAGE_METADATA_EL);
                    XmlNodeList metaDataNodes = metaDataNode.ChildNodes;
                    Dictionary<string, string> metaData = new Dictionary<string, string>();
                    foreach(XmlNode metaDataEntryNode in metaDataNodes)
                    {
                        metaData.Add(metaDataEntryNode.Name, metaDataEntryNode.InnerText);
                    }

                    //Optional WordsearchID field
                    string wordsearchId = null;
                    try
                    {
                        wordsearchId = wiNode.SelectSingleNode(XML_WORDSEARCH_IMAGE_WORDSEARCH_ID_EL).InnerText;
                    }
                    catch { }

                    //If there was a Wordsearch ID, pass that to the constructor
                    WordsearchImage wordsearchImage;
                    if(wordsearchId != null)
                    {
                        wordsearchImage = new WordsearchImage(topLeft, topRight, bottomRight,
                        bottomLeft, rows, cols, metaData, hash, wordsearchId);
                    }
                    else //Otherwise there is no Wordsearch ID
                    {
                        wordsearchImage = new WordsearchImage(topLeft, topRight, bottomRight, 
                        bottomLeft, rows, cols, metaData, hash);
                    }
                    wordsearchImages.Add(wordsearchImage);
                }

                //Meta Data
                XmlNodeList imageMetaDataNodes = imageNode.SelectSingleNode(XML_IMAGE_METADATA_EL).ChildNodes;
                Dictionary<string, string> imageMetaData = new Dictionary<string, string>();
                foreach(XmlNode metaDataEntryNode in imageMetaDataNodes)
                {
                    imageMetaData.Add(metaDataEntryNode.Name, metaDataEntryNode.InnerText);
                }

                Image image = new Image(path, hash, wordsearchImages.ToArray(), imageMetaData);
                images.Add(hash, image);
            }

            //Wordsearches
            XmlNode wordsearchesNode = rootNode.SelectSingleNode(XML_WORDSEARCHES_EL);
            XmlNodeList wordsearchNodes = wordsearchesNode.SelectNodes(XML_WORDSEARCH_EL);
            wordsearches = new Dictionary<string, Wordsearch>();
            foreach(XmlNode wordsearchNode in wordsearchNodes)
            {
                string wordsearchId = wordsearchNode.SelectSingleNode(XML_WORDSEARCH_ID_EL).InnerText;

                //Chars
                string strChars = wordsearchNode.SelectSingleNode(XML_WORDSEARCH_CHARS_EL).InnerText.Replace("\r", ""); //Strip carriage returns
                string[] strCharsRows = strChars.Split('\n');
                char[,] chars = new char[strCharsRows[0].Length, strCharsRows.Length];
                for(int i = 0; i < strCharsRows.Length; i++)
                {
                    for(int j = 0; j < strCharsRows[i].Length; j++)
                    {
                        chars[j, i] = strCharsRows[i][j];
                    }
                }

                //Words
                XmlNode wordsNode = wordsearchNode.SelectSingleNode(XML_WORDSEARCH_WORDS_EL);
                XmlNodeList wordNodes = wordsNode.SelectNodes(XML_WORDSEARCH_WORD_EL);
                List<string> words = new List<string>();
                foreach(XmlNode wordNode in wordNodes)
                {
                    words.Add(wordNode.InnerText);
                }

                Wordsearch wordsearch = new Wordsearch(wordsearchId, chars, words.ToArray());
                wordsearches.Add(wordsearchId, wordsearch);
            }
        }

        private static void writeDatabase()
        {
            //If there is a database file already, delete it
            if(File.Exists(XML_DOC_LOCATION))
            {
                File.Delete(XML_DOC_LOCATION);
            }

            //Pretty-print (indent & new lines) the XML doc
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.NewLineOnAttributes = true;

            //Write out the database contents from memory
            XmlWriter writer = XmlWriter.Create(XML_DOC_LOCATION, xmlWriterSettings);

            writer.WriteStartDocument();
            writer.WriteStartElement(XML_ROOT_EL);

            //Images
            writer.WriteStartElement(XML_IMAGES_EL);
            foreach(Image image in images.Values)
            {
                writer.WriteStartElement(XML_IMAGE_EL);

                writer.WriteElementString(XML_IMAGE_FILE_PATH_EL, image.Path);
                writer.WriteElementString(XML_IMAGE_HASH_EL, image.Hash);

                //Wordsearch Images
                writer.WriteStartElement(XML_WORDSEARCH_IMAGES_EL);
                foreach(WordsearchImage wordsearchImage in image.WordsearchImages)
                {
                    //Write the Wordsearch Image to XML doc
                    writer.WriteStartElement(XML_WORDSEARCH_IMAGE_EL);

                    writer.WriteElementString(XML_WORDSEARCH_IMAGE_COORDINATES_EL, String.Format("({0},{1}).({2},{3}).({4},{5}).({6},{7})", 
                        wordsearchImage.TopLeft.X, wordsearchImage.TopLeft.Y, wordsearchImage.TopRight.X, wordsearchImage.TopRight.Y, 
                        wordsearchImage.BottomRight.X, wordsearchImage.BottomRight.Y, wordsearchImage.BottomLeft.X, 
                        wordsearchImage.BottomLeft.Y));
                    writer.WriteElementString(XML_WORDSEARCH_IMAGE_ROWS_EL, wordsearchImage.Rows.ToString());
                    writer.WriteElementString(XML_WORDSEARCH_IMAGE_COLS_EL, wordsearchImage.Cols.ToString());

                    //Wordsearch ID is an optional field
                    try
                    {
                        writer.WriteElementString(XML_WORDSEARCH_IMAGE_WORDSEARCH_ID_EL, wordsearchImage.WordsearchId);
                    }
                    catch { }

                    //Meta Data
                    writer.WriteStartElement(XML_WORDSEARCH_IMAGE_METADATA_EL);
                    foreach(KeyValuePair<string, string> metaData in wordsearchImage.MetaData)
                    {
                        writer.WriteElementString(metaData.Key, metaData.Value);
                    }
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                //Meta Data
                writer.WriteStartElement(XML_IMAGE_METADATA_EL);
                foreach(KeyValuePair<string, string> metaData in image.MetaData)
                {
                    writer.WriteElementString(metaData.Key, metaData.Value);
                }
                writer.WriteEndElement();
                

                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            //Wordsearches
            writer.WriteStartElement(XML_WORDSEARCHES_EL);
            foreach(Wordsearch wordsearch in wordsearches.Values)
            {
                writer.WriteStartElement(XML_WORDSEARCH_EL);

                writer.WriteElementString(XML_WORDSEARCH_ID_EL, wordsearch.Id);
                writer.WriteElementString(XML_WORDSEARCH_CHARS_EL, wordsearch.StrChars);

                //Words to find
                writer.WriteStartElement(XML_WORDSEARCH_WORDS_EL);
                foreach(string word in wordsearch.Words)
                {
                    writer.WriteElementString(XML_WORDSEARCH_WORD_EL, word);
                }
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Close();
        }
    }
}
