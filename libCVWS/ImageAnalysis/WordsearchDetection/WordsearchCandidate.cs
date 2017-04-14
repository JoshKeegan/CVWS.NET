/*
 * CVWS.NET
 * libCvws
 * WordsearchCandidate - a region extracted from an image that could potentially be a wordsearch
 * Authors:
 *  Josh Keegan 14/04/2017
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;

namespace libCVWS.ImageAnalysis.WordsearchDetection
{
    public struct WordsearchCandidate : IDisposable
    {
        #region Public Variables

        public readonly List<IntPoint> OriginalImageCoords;
        public readonly Bitmap Bitmap;

        #endregion

        #region Constructors

        public WordsearchCandidate(List<IntPoint> originalImageCoords, Bitmap bitmap)
        {
            OriginalImageCoords = originalImageCoords;
            Bitmap = bitmap;
        }

        #endregion

        #region Implement IDisposable

        public void Dispose()
        {
            Bitmap?.Dispose();
        }

        #endregion
    }
}
