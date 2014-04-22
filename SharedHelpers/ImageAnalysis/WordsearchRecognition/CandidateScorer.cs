/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Candidate Scorer class - for scoring a wordsearch candidate so that they can be ranked
 * By Josh Keegan 22/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedHelpers.ImageAnalysis.WordsearchSegmentation;
using SharedHelpers.Maths.Statistics;

namespace SharedHelpers.ImageAnalysis.WordsearchRecognition
{
    internal class CandidateScorer : Segmentation
    {
        private double? wordsearchRecognitionScore = null;
        public double WordsearchRecognitionScore
        {
            get
            {
                //If we haven't calculated the score yet, calculate it
                if(wordsearchRecognitionScore == null)
                {
                    calculateWordsearchRecognitionScore();
                }

                //Will always have value due to preceding if statement
                return wordsearchRecognitionScore.GetValueOrDefault();
            }
        }

        //Constructors
        public CandidateScorer(Segmentation segmentation)
            : base(segmentation) { }

        /*
         * Private Helpers
         */

        //Helper method to automatically work out how to score this type of Segmentation and calculate the score with that method
        private void calculateWordsearchRecognitionScore()
        {
            //If we have the start & end positions of the rows and cols
            if (rowStartEnds != null && colStartEnds != null && width != null && height != null)
            {
                calculateWordsearchRecognitionScoreByStartAndEndPositions();
            }
            //Else if Segmentation was constructed by the number of rows and cols
            else if(numRows != null && numCols != null && width != null && height != null)
            {
                //Not really sure how this scenario could be scored
                throw new NotImplementedException("Don't currently support wordsearch candidate scoring by num rows and cols (and cannot think of a way for this to even be possible)");
            }
            //Otherwise we only have the segmentation points
            else
            {
                calculateWordsearchRecognitionScoreBySegmentationIndices();
            }
        }

        private void calculateWordsearchRecognitionScoreByStartAndEndPositions()
        {
            //Get character widths/heights
            int[] rowHeights = getColWidths(rowStartEnds);
            int[] colWidths = getColWidths(colStartEnds);

            //Get character spacing
            int[] rowGaps = getColSpacing(rowStartEnds);
            int[] colGaps = getColSpacing(colStartEnds);

            //The measure will be based on the standard deviation of the widths of cols/rows and the gaps between them:
            double rowHeightStdDev = Stats.StandardDeviation(rowHeights);
            double colWidthStdDev = Stats.StandardDeviation(colWidths);
            double rowGapsStdDev = Stats.StandardDeviation(rowGaps);
            double colsGapsStdDev = Stats.StandardDeviation(colGaps);

            double avgStdDev = (rowHeightStdDev + colWidthStdDev + rowGapsStdDev + colsGapsStdDev) / 4;

            //Inverse because the lower the std.dev, the more likely to be a wordsearch
            wordsearchRecognitionScore = (1 / avgStdDev);
        }

        private void calculateWordsearchRecognitionScoreBySegmentationIndices()
        {
            throw new NotImplementedException("Don't yet support wordsearch candidate scoring by segmentation indices");
        }

        private static int[] getColSpacing(int[,] cols)
        {
            //Check there are enough cols to be a gap between them
            if (cols.GetLength(0) < 2)
            {
                return new int[0];
            }
            else //There are enough cols
            {
                int[] gaps = new int[cols.GetLength(0) - 1];

                for (int i = 0; i < gaps.Length; i++)
                {
                    gaps[i] = cols[i + 1, 0] - cols[i, 1];
                }
                return gaps;
            }
        }

        private static int[] getColWidths(int[,] cols)
        {
            int[] widths = new int[cols.GetLength(0)];

            for (int i = 0; i < widths.Length; i++)
            {
                widths[i] = cols[i, 1] - cols[i, 0];
            }
            return widths;
        }
    }
}
