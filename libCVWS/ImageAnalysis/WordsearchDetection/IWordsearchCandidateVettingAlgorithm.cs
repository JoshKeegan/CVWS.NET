/*
 * CVWS.NET
 * libCvws
 * IWordsearchCandidateVettingAlgorithm - scores a wordsearch candidate extracted from an image.
 *  Scores could then be used to select the most likely candidate to work with, or select all likely candidates by applying a threshold.
 * Authors:
 *  Josh Keegan 14/04/2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.ImageAnalysis.WordsearchDetection
{
    public interface IWordsearchCandidateVettingAlgorithm
    {
        double Score(WordsearchCandidate candidate);
    }
}
