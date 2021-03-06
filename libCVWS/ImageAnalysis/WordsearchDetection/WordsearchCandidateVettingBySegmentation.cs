﻿/*
 * CVWS.NET
 * libCvws
 * Wordsearch Candidate Vetting algorithm that works by attempting to segment a given image as if
 *  it were a wordsearch, and then scoring the segmentation produced.
 * Authors:
 *  Josh Keegan 14/04/2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using libCVWS.Exceptions;
using libCVWS.ImageAnalysis.WordsearchSegmentation;

namespace libCVWS.ImageAnalysis.WordsearchDetection
{
    public class WordsearchCandidateVettingBySegmentation : IWordsearchCandidateVettingAlgorithm
    {
        #region Private Variables

        private readonly SegmentationAlgorithm segAlg;
        private readonly bool removeSmallRowsAndCols;

        #endregion

        #region Implement IWordsearchCandidateVettingAlgorithm

        public double Score(WordsearchCandidate candidate)
        {
            double score;
            //If an InvalidRowsAndCols Exception gets throw by the segmentation 
            //  (due to there being 0 rows/cols in the returned segmentation)
            //  this is obviously not a good wordsearch candidate
            try
            {
                Segmentation segmentation = segAlg.Segment(candidate.Bitmap);

                // If removing erroneously small rows and cols before scoring the segmentation, do so now
                if (removeSmallRowsAndCols)
                {
                    segmentation = segmentation.RemoveSmallRowsAndCols();
                }

                CandidateScorer scorer = new CandidateScorer(segmentation);
                score = scorer.WordsearchRecognitionScore;
            }
            catch (InvalidRowsAndColsException)
            {
                //This is slightly better than the default score of Negative Infinity as any candidate
                //  (even one with no rows or cols found in it) is better than no candidate whatsoever
                score = double.MinValue;
            }

            return score;
        }

        #endregion

        #region Constructors

        public WordsearchCandidateVettingBySegmentation(SegmentationAlgorithm segAlg, bool removeSmallRowsAndCols = false)
        {
            // Validation
            if (segAlg == null)
            {
                throw new ArgumentNullException(nameof(segAlg));
            }

            this.segAlg = segAlg;
            this.removeSmallRowsAndCols = removeSmallRowsAndCols;
        }

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            WordsearchCandidateVettingBySegmentation a = obj as WordsearchCandidateVettingBySegmentation;
            if (a == null)
            {
                return false;
            }

            return a.removeSmallRowsAndCols == removeSmallRowsAndCols && 
                // Just checks the type on Segmentation Algorithm. It's possible for an implementation
                //  of SegmentationAlgorithm to have it's own settings, so should reall check segAlg.Equals(), 
                //  but need to implement Equals on segmentation algorithms then!
                a.segAlg.GetType() == segAlg.GetType();
        }

        public override int GetHashCode()
        {
            int hash = Constants.PRIME1;
            hash *= Constants.PRIME2 + removeSmallRowsAndCols.GetHashCode();
            hash *= Constants.PRIME2 + segAlg.GetHashCode();
            return hash;
        }

        #endregion
    }
}
