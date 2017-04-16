/*
 * CVWS.NET
 * libCvws
 * IWordsearchCandidatesDetectionAlgorithm - Interface for algorithms detecting regions of an image that could be a wordsearch.
 * Authors:
 *  Josh Keegan 14/04/2017
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using libCVWS.IntermediateImageLogging;

namespace libCVWS.ImageAnalysis.WordsearchDetection
{
    public interface IWordsearchCandidatesDetectionAlgorithm
    {
        /// <summary>
        /// Finds all regions of an image that could be wordsearches
        /// </summary>
        WordsearchCandidate[] FindCandidates(Bitmap image, IntermediateImageLog intermediateImageLog = null);
    }
}
