/*
 * Dissertation CV Wordsearch Solver
 * Quatitative Evaluation
 * Char Data class - static methods for handling character image data
 * By Josh Keegan 11/03/2014
 * Last Edit 25/03/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageMarkup;
using SharedHelpers.Imaging;
using SharedHelpers.Exceptions;
using SharedHelpers.ClassifierInterfacing;

namespace QuantitativeEvaluation
{
    public static class CharData
    {
        //Public Methods
        public static Dictionary<char, List<Bitmap>> GetCharData(List<WordsearchImage> wordsearchImages)
        {
            //Construct data structure to be returned
            Dictionary<char, List<Bitmap>> data = new Dictionary<char, List<Bitmap>>();

            //Make a blank entry for each valid char
            for (int i = (int)'A'; i <= (int)'Z'; i++)
            {
                char c = (char)i;
                List<Bitmap> charImgs = new List<Bitmap>();
                data.Add(c, charImgs);
            }

            foreach (WordsearchImage wordsearchImage in wordsearchImages)
            {
                Bitmap[,] rawCharImages = wordsearchImage.GetCharBitmaps();

                for (int i = 0; i < rawCharImages.GetLength(0); i++)
                {
                    for (int j = 0; j < rawCharImages.GetLength(1); j++)
                    {
                        //Get a Bitmap of just the character (without whitespace)
                        Bitmap extractedChar = CharImgExtractor.Extract(rawCharImages[i, j]);

                        //Get the char that this image is of
                        char c = wordsearchImage.Wordsearch.Chars[i, j];

                        //Check the char is valid
                        if (c < 'A' || c > 'Z')
                        {
                            throw new UnexpectedClassifierOutputException("Chars must be in range A-Z. Found " + c);
                        }

                        //Store
                        data[c].Add(extractedChar);

                        //Clean up
                        rawCharImages[i, j].Dispose();
                    }
                }
            }

            return data;
        }

        public static char[] GetCharLabels(Dictionary<char, List<Bitmap>> data)
        {
            int numInputs = 0;
            foreach (List<Bitmap> arr in data.Values)
            {
                numInputs += arr.Count;
            }

            char[] labels = new char[numInputs];
            int idx = 0;
            foreach (KeyValuePair<char, List<Bitmap>> entry in data)
            {
                char c = entry.Key;

                for (int i = 0; i < entry.Value.Count; i++)
                {
                    labels[idx] = c;
                    idx++;
                }
            }

            return labels;
        }

        public static void GetNeuralNetworkBitmapsAndOutput(Dictionary<char, List<Bitmap>> data,
            out Bitmap[] bitmaps, out double[][] output)
        {
            int numInputs = 0;
            foreach(List<Bitmap> list in data.Values)
            {
                numInputs += list.Count;
            }

            bitmaps = new Bitmap[numInputs];
            output = new double[numInputs][];
            int idx = 0;
            foreach(KeyValuePair<char, List<Bitmap>> entry in data)
            {
                char c = entry.Key;
                List<Bitmap> images = entry.Value;

                double[] thisCharOutput = NeuralNetworkHelpers.GetDesiredOutputForChar(c);

                foreach(Bitmap image in images)
                {
                    bitmaps[idx] = image;
                    output[idx] = thisCharOutput;
                    idx++;
                }
            }
        }
    }
}
