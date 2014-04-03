/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Segmentation Algorithm Helpers
 * By Josh Keegan 03/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.ImageAnalysis.WordsearchSegmentation
{
    internal static class SegmentationAlgorithmHelpers
    {
        internal static uint[] CountNumDarkPixelsPerCol(bool[,] img)
        {
            uint[] colPixelCounts = new uint[img.GetLength(0)];
            for (int i = 0; i < img.GetLength(0); i++)
            {
                colPixelCounts[i] = 0;
                for (int j = 0; j < img.GetLength(1); j++)
                {
                    if (img[i, j])
                    {
                        colPixelCounts[i]++;
                    }
                }
            }
            return colPixelCounts;
        }

        internal static uint[] CountNumDarkPixelsPerRow(bool[,] img)
        {
            uint[] rowPixelCounts = new uint[img.GetLength(1)];
            for (int i = 0; i < img.GetLength(1); i++)
            {
                rowPixelCounts[i] = 0;
                for (int j = 0; j < img.GetLength(0); j++)
                {
                    if (img[j, i])
                    {
                        rowPixelCounts[i]++;
                    }
                }
            }
            return rowPixelCounts;
        }

        //Determine for each row/col whether it is in a char given some threshold to determine when you enter/exit a char
        internal static uint[,] FindCharIndices(uint[] darkPixelCounts, double enterExitThreshold)
        {
            return FindCharIndices(darkPixelCounts, enterExitThreshold, enterExitThreshold);
        }

        //Determine for each row/col whether it is in a char given some threshold to 
        //  enter a char (value must go above threshold), and some threshold to exit a char (value must go below threshold)
        internal static uint[,] FindCharIndices(uint[] darkPixelCounts, double enterThreshold, double exitThreshold)
        {
            List<uint[]> chars = new List<uint[]>();

            //Keep track of whether we've in a char
            uint charStartIdx = 0;
            bool inChar = false;

            for(uint i = 0; i < darkPixelCounts.Length; i++)
            {
                //If we're in a character then we're searching for an end position
                if (inChar)
                {
                    //If we have reached the end of the character that we were in
                    if(darkPixelCounts[i] < exitThreshold)
                    {
                        //Store this chars position
                        uint[] thisChar = { charStartIdx, i };
                        chars.Add(thisChar);

                        //Update inChar
                        inChar = false;
                    }
                }
                //Otherwise we aren't in a character, but this line is dark enough for us to enter one
                else if(darkPixelCounts[i] > enterThreshold)
                {
                    charStartIdx = i;
                    inChar = true;
                }
            }

            //If we end in a character, make it's end position the last character
            if(inChar && charStartIdx != (uint)darkPixelCounts.Length - 1) //Also Prevent rows/cols of 0 width
            {
                uint[] thisChar = { charStartIdx, (uint)darkPixelCounts.Length - 1 };
                chars.Add(thisChar);
            }

            //Convert to uint[,] (to return)
            uint[,] toRet = new uint[chars.Count, 2];
            for(int i = 0; i < chars.Count; i++)
            {
                toRet[i, 0] = chars[i][0];
                toRet[i, 1] = chars[i][1];
            }
            return toRet;
        }
    }
}
