/*
 * Dissertation CV Wordsearch Solver
 * Dev Char Classification - Console app written during development of the first character classifier
 * Program Entry Point
 * By Josh Keegan 06/03/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Neuro;
using AForge.Neuro.Learning;

using ImageMarkup;
using SharedHelpers;
using SharedHelpers.Imaging;

namespace DevCharClassification
{
    class Program
    {
        //Constants
        private const int CHAR_WITH_WHITESPACE_WIDTH = 25;
        private const int CHAR_WITH_WHITESPACE_HEIGHT = 25;
        private const int CHAR_WITHOUT_WHITESPACE_WIDTH = 16;
        private const int CHAR_WITHOUT_WHITESPACE_HEIGHT = 16;

        static void Main(string[] args)
        {
            Console.WriteLine("Loading Wordsearch Database . . .");
            //If the Image Markup Data base hasn't already been loaded
            if(!ImageMarkupDatabase.Loaded)
            {
                ImageMarkupDatabase.LoadDatabase();
            }
            Console.WriteLine("Wordsearch Database Loaded");

            //Split the data into 2 groups: training & evaluation
            Console.WriteLine("Splitting wordsearch image data into training and evaluation data");
            List<WordsearchImage> wordsearchImages = ImageMarkupDatabase.GetWordsearchImages();
            List<WordsearchImage> trainingWordsearchImages = new List<WordsearchImage>(wordsearchImages.Count / 2);
            List<WordsearchImage> evaluationWordsearchImages = new List<WordsearchImage>(wordsearchImages.Count / 2);

            for(int i = 0; i < wordsearchImages.Count; i++)
            {
                if(i % 2 == 0)
                {
                    trainingWordsearchImages.Add(wordsearchImages[i]);
                }
                else
                {
                    evaluationWordsearchImages.Add(wordsearchImages[i]);
                }
            }
            Console.WriteLine("Data split into training and evalutation groups");

            // Get the training data for the Neural Network
            Console.WriteLine("Loading & processing the character data (training)");
            Dictionary<char, List<float[]>> trainingData = getCharData(trainingWordsearchImages);
            Console.WriteLine("Loaded training character data");
        }

        private static Dictionary<char, List<float[]>> getCharData(List<WordsearchImage> wordsearchImages)
        {
            //Make some objects now for reuse later
            BradleyLocalThresholding bradleyLocalThreshold = new BradleyLocalThresholding();
            ExtractBiggestBlob extractBiggestBlob = new ExtractBiggestBlob();
            ResizeNearestNeighbor resize = new ResizeNearestNeighbor(CHAR_WITHOUT_WHITESPACE_WIDTH, CHAR_WITHOUT_WHITESPACE_HEIGHT);

            //Construct Data Structure to be returned
            Dictionary<char, List<float[]>> data = new Dictionary<char, List<float[]>>();

            //Make a blank entry for each valid char
            for (int i = (int)'A'; i <= (int)'Z'; i++)
            {
                char c = (char)i;
                List<float[]> charImgs = new List<float[]>();
                data.Add(c, charImgs);
            }

            foreach (WordsearchImage wordsearchImage in wordsearchImages)
            {
                Bitmap[,] rawCharImages = wordsearchImage.GetCharBitmaps(CHAR_WITH_WHITESPACE_WIDTH, CHAR_WITH_WHITESPACE_HEIGHT);

                for (int i = 0; i < rawCharImages.GetLength(0); i++)
                {
                    for (int j = 0; j < rawCharImages.GetLength(1); j++)
                    {
                        //Greyscale
                        Bitmap greyImg = Grayscale.CommonAlgorithms.BT709.Apply(rawCharImages[i, j]); //Use the BT709 (HDTV spec) for RBG weights

                        //Bradley Local Thresholding
                        bradleyLocalThreshold.ApplyInPlace(greyImg);

                        //Extract the largest blob (the character)
                        Bitmap charWithoutWhitespace = extractBiggestBlob.Apply(greyImg);

                        //Resize the image of the char without whitespace to some normalised size (using nearest neighbour because we've already thresholded)
                        Bitmap normalisedCharWithoutWhitespace = resize.Apply(charWithoutWhitespace);

                        //Convert Bitmap to float[,] (+-0.5) (what's used to train the neural network)
                        float[] floatImg = Converters.ThresholdedBitmapToFloatArray(normalisedCharWithoutWhitespace);
                        char charImg = wordsearchImage.Wordsearch.Chars[i, j];

                        //Check the char is valid
                        if(charImg < 'A' || charImg > 'Z')
                        {
                            throw new Exception("Chars must be in range A-Z. Found " + charImg);
                        }

                        data[charImg].Add(floatImg);

                        //Clean up
                        rawCharImages[i, j].Dispose();
                        greyImg.Dispose();
                        charWithoutWhitespace.Dispose();
                        normalisedCharWithoutWhitespace.Dispose();
                    }
                }
            }

            return data;
        }
    }
}
