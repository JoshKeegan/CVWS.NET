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

using AForge;

using libCVWS.BaseObjectExtensions;

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

        // connection angle criteria
        private const double MIN_ANGLE_BETWEEN_CONNECTIONS_TO_THE_SAME_ELEMENT = Math.PI / 4;

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

        public LatticeConstructionSettings Settings;

        #endregion

        #region Constructors

        public BlobLatticeElement(BlobMeta blob, LatticeConstructionSettings settings)
        {
            Blob = blob;
            Settings = settings;
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
            return vetConnectionDistance(proposedConnection, out tooFar) && 
                   vetConnectionAngle(proposedConnection) &&
                   vetConnectionDimensions(proposedConnection);
        }

        private bool vetConnectionDistance(BlobLatticeElement proposedConnection, out bool tooFar)
        {
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
            // If this vetting method is disabled, skip it
            if (!Settings.ElementConnectionVettingByDimensions)
            {
                return true;
            }

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

        private bool vetConnectionAngle(BlobLatticeElement proposedConnection)
        {
            // If this vetting method is disabled, skip it
            if (!Settings.ElementConnectionVettingByAngle)
            {
                return true;
            }

            // If there are no existing connections from this element, check them
            if (ConnectedTo.Count > 0)
            {
                // The point at the centre of these connections (this one)
                Point currentPoint = Blob.Blob.CenterOfGravity;

                // Calculate the angle to the proposed connection
                double proposedConnectionAngle = currentPoint.AngleTo(proposedConnection.Blob.Blob.CenterOfGravity);

                // Check the angle to each existing connection, making sure the proposed one isn't too close 
                //  to an existing connection
                foreach (BlobLatticeElement le in ConnectedTo)
                {
                    // Calculate angle of existing connection
                    double existingConnectionAngle = currentPoint.AngleTo(le.Blob.Blob.CenterOfGravity);

                    // Calculate difference between this connections angle and the proposed one
                    double angleBetweenConnections = Math.Abs(existingConnectionAngle - proposedConnectionAngle);

                    // Check if they're too close
                    if (angleBetweenConnections < MIN_ANGLE_BETWEEN_CONNECTIONS_TO_THE_SAME_ELEMENT)
                    {
                        return false;
                    }
                }
            }

            // If there are existing connections from the proposed element, check them
            // Disabled for consistency with other vetting & doesn't appear to be helpful. Perhaps make a setting flag for this?
            /*if (proposedConnection.ConnectedTo.Count > 0)
            {
                // The point at the centre of these connections (the proposed one)
                Point currentPoint = proposedConnection.Blob.Blob.CenterOfGravity;

                // Calculate the angle to the proposed connection
                double proposedConnectionAngle = currentPoint.AngleTo(Blob.Blob.CenterOfGravity);

                // Check the angle to each existing connection, making sure that the proposed one isn't too close
                //  to an existing connection to the proposed point
                foreach (BlobLatticeElement le in ConnectedTo)
                {
                    // Calculate angle of existing connection
                    double existingConnectionAngle = currentPoint.AngleTo(le.Blob.Blob.CenterOfGravity);

                    // Calculate difference between this connections angle and the proposed one
                    double angleBetweenConnections = Math.Abs(existingConnectionAngle - proposedConnectionAngle);

                    // Check if they're too close
                    if (angleBetweenConnections < MIN_ANGLE_BETWEEN_CONNECTIONS_TO_THE_SAME_ELEMENT)
                    {
                        return false;
                    }
                }
            }*/

            // If we get this far, then none of the angles were too close, so it's passed this stage of vetting
            return true;
        }

        #endregion
    }
}
