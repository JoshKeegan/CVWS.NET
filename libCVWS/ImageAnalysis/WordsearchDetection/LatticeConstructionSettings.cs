/*
 * CVWS.NET
 * libCvws
 * Lattice Construction Settings
 * Authors:
 *  Josh Keegan 17/04/2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.ImageAnalysis.WordsearchDetection
{
    public class LatticeConstructionSettings
    {
        #region Blob Recognition

        /// <summary>
        /// Whether to discard very noisy regions of the image.
        /// This is done by resizing the image to a very low resolution & performing blob recognition on it.
        /// Very noisy areas of the high-res image should blur together into blobs in the low-res one.
        /// Any large blobs in the low res image have blobs in the corresponding area of the full-res image
        /// discarded.
        /// </summary>
        public bool RemoveVeryNoisyBlobs = true;

        #endregion

        #region Blob Lattice Connection Vetting

        /// <summary>
        /// Whether to vet proposed new connections between elements by checking whether the proposed
        /// element already has the target number of connections.
        /// If enabled, no element can possibly have more than the target number of connections.
        /// </summary>
        public bool ElementsConnectionVettingByProposedAlreadySaturated = false;

        /// <summary>
        /// Whether to vet proposed new connections between elements by checking the angle
        /// between the two elements isn't too similar to the angle of another existing connection.
        /// Relies on the fact that each connected character in a wordsearch is 90 degrees apart
        /// </summary>
        public bool ElementConnectionVettingByAngle = false;

        /// <summary>
        /// Whether to vet proposed new connections between elements by checking whether the proposed
        /// blob's dimensions are similar enough to that of the current blob.
        /// </summary>
        public bool ElementConnectionVettingByDimensions = true;

        #endregion

        #region Vetting of produced overall lattice

        /// <summary>
        /// Whether to vet the overall produced lattice has an expected number of elements (for
        /// what you'd expect of a wordsearch).
        /// Intended as a computational optimisation (by removing obviously wrong candidate solutions 
        /// before further heavy processing is done to them), not to improve the end result.
        /// </summary>
        public bool LatticeVettingByNumElements = true;

        #endregion
    }
}
