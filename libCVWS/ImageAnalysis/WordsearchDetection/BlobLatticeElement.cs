/*
 * CVWS.NET
 * BlobLatticeElement
 * Authors:
 *  Josh Keegan 16/04/2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.ImageAnalysis.WordsearchDetection
{
    internal class BlobLatticeElement
    {
        #region Constants

        private const int SEARCH_FOR_NUM_LATTICE_CONNECTIONS = 4;

        // connected blob size criteria
        private const double CONNECTION_MIN_BLOB_DIMENSIONS_MULTIPLIER = 0.8;
        private const double CONNECTION_MAX_BLOB_DIMENSIONS_MULTIPLIER = 1.2;

        // connection distance criteria
        private const float CONNECTION_MAX_DISTANCE_MULTIPLIER = 1.25f;
        private const float CONNECTION_MIN_DISTANCE_MULTIPLIER = 0.8f;

        #endregion

        #region Public Variables

        public readonly BlobMeta Blob;
        public readonly List<BlobLatticeElement> ConnectedTo = new List<BlobLatticeElement>(SEARCH_FOR_NUM_LATTICE_CONNECTIONS);
        /// <summary>
        /// Whether this Lattice Element has already been queued for search.
        /// Doesn't mean it's still queued, it could already have been processed & this could still be true
        /// </summary>
        public bool QueuedForSearch = false;
        public bool Processed = false;

        #endregion

        #region Constructors

        public BlobLatticeElement(BlobMeta blob)
        {
            Blob = blob;
        }

        #endregion

        #region Public Methods

        public void FindConnections(BlobLatticeElement[] latticeElements)
        {
            // Mark as processed
            Processed = true;

            // Mark this blob as being used
            Blob.Used = true;

            // If we already have enough connected elements, don't look any further
            if (ConnectedTo.Count >= SEARCH_FOR_NUM_LATTICE_CONNECTIONS)
            {
                return;
            }

            // Exclude this element and any it's already connected to as options
            IEnumerable<BlobLatticeElement> possibleElements =
                latticeElements.Where(le => le != this && !ConnectedTo.Contains(le));

            // Order the other lattice elements by their distance from this one
            IEnumerable<BlobLatticeElement> nearest =
                possibleElements.OrderBy(le => le.Blob.Blob.CenterOfGravity.DistanceTo(Blob.Blob.CenterOfGravity));

            foreach (BlobLatticeElement le in nearest)
            {
                // vet this lattice element to see if it could be a match for a connection to this one
                bool tooFar;
                if (vetConnection(le, out tooFar))
                {
                    ConnectedTo.Add(le);
                    le.ConnectedTo.Add(this);

                    // If we've now found enough connections, stop looking
                    if (ConnectedTo.Count >= SEARCH_FOR_NUM_LATTICE_CONNECTIONS)
                    {
                        break;
                    }
                }

                // If we've now started trying blobs that are too far away to be candidates, stop
                if (tooFar)
                {
                    break;
                }
            }
        }


        #endregion

        #region Private Methods

        private bool vetConnection(BlobLatticeElement proposedConnection, out bool tooFar)
        {
            // TODO: Should one of the criteria be the number of connections the proposed connection already has?
            return vetConnectionDistance(proposedConnection, out tooFar) && vetConnectionDimensions(proposedConnection);
        }

        private bool vetConnectionDistance(BlobLatticeElement proposedConnection, out bool tooFar)
        {
            // DEBUG: DISABLE METHOD
            //tooFar = false;
            //return true;

            // If there are no existing connections, we can't vet based on the distance
            if (ConnectedTo.Count == 0)
            {
                tooFar = false;
                return true;
            }

            // Get the current max & min distances for current connections
            float currentMinDistance = ConnectedTo[0].Blob.Blob.CenterOfGravity.DistanceTo(Blob.Blob.CenterOfGravity);
            float currentMaxDistance = currentMinDistance;
            for (int i = 1; i < ConnectedTo.Count; i++)
            {
                float distance = ConnectedTo[i].Blob.Blob.CenterOfGravity.DistanceTo(Blob.Blob.CenterOfGravity);

                if (distance < currentMinDistance)
                {
                    currentMinDistance = distance;
                }
                else if (distance > currentMaxDistance)
                {
                    currentMaxDistance = distance;
                }
            }

            // Calculate what the allowed range band is
            float minDistance = currentMinDistance * CONNECTION_MIN_DISTANCE_MULTIPLIER;
            float maxDistance = currentMaxDistance * CONNECTION_MAX_DISTANCE_MULTIPLIER;

            // Check whether the proposed connection is within the allowed range band
            float proposedDistance = proposedConnection.Blob.Blob.CenterOfGravity.DistanceTo(Blob.Blob.CenterOfGravity);
            tooFar = proposedDistance > maxDistance;
            return !tooFar && proposedDistance >= minDistance;
        }

        private bool vetConnectionDimensions(BlobLatticeElement proposedConnection)
        {
            // Get this blob's dimensions
            int thisWidth = Blob.Blob.Rectangle.Width;
            int thisHeight = Blob.Blob.Rectangle.Height;

            // Calculate allowable dimensions range
            int minWidth = (int) (thisWidth * CONNECTION_MIN_BLOB_DIMENSIONS_MULTIPLIER);
            int minHeight = (int) (thisHeight * CONNECTION_MIN_BLOB_DIMENSIONS_MULTIPLIER);
            int maxWidth = (int) (thisWidth * CONNECTION_MAX_BLOB_DIMENSIONS_MULTIPLIER);
            int maxHeight = (int) (thisHeight * CONNECTION_MAX_BLOB_DIMENSIONS_MULTIPLIER);

            // Get the blob for the proposed connection's dimensions
            int targetWidth = proposedConnection.Blob.Blob.Rectangle.Width;
            int targetHeight = proposedConnection.Blob.Blob.Rectangle.Height;
            
            // Check if the target blob's dimensions are acceptable.
            //  Only require one dimension to be in the allowed range
            return (targetWidth >= minWidth && targetWidth <= maxWidth) ||
                   (targetHeight >= minHeight && targetHeight <= maxHeight);
        }

        #endregion
    }
}
