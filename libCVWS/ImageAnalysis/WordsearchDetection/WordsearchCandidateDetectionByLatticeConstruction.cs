/*
 * CVWS.NET
 * WordsearchCandidateDetectionByLatticeConstruction
 * Authors:
 *  Josh Keegan 16/04/2017
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;

using libCVWS.Imaging;
using libCVWS.IntermediateImageLogging;

namespace libCVWS.ImageAnalysis.WordsearchDetection
{
    public class WordsearchCandidateDetectionByLatticeConstruction : IWordsearchCandidatesDetectionAlgorithm
    {
        #region Constants

        // Blob recognition constants
        private const double BLOB_MIN_DIMENSIONS_MULTIPLIER = 0.005;
        private const double BLOB_MAX_DIMENSIONS_MULTIPLIER = 0.05;

        // Lattice vetting constants
        private const int MIN_LATTICE_ELEMENTS = 25; // a lattice must have 25 elements (i.e. a 5x5 wordsearch with all chars found)

        #endregion

        #region Implement IWordsearchCandidatesDetectionAlgorithm

        public WordsearchCandidate[] FindCandidates(Bitmap img, IntermediateImageLog imageLog = null)
        {
            using (Bitmap thresholdedImg = FilterCombinations.AdaptiveThreshold(img))
            {
                // Blob recognition requires blobs to be white & background to be black
                Invert invertFilter = new Invert();
                invertFilter.ApplyInPlace(thresholdedImg);

                // Use blob recognition on the image to find all blobs meeting the allowed dimensions range
                BlobCounter blobCounter = new BlobCounter()
                {
                    FilterBlobs = true,
                    MinWidth = (int) (BLOB_MIN_DIMENSIONS_MULTIPLIER * img.Width),
                    MinHeight = (int) (BLOB_MIN_DIMENSIONS_MULTIPLIER * img.Height),
                    MaxWidth = (int) (BLOB_MAX_DIMENSIONS_MULTIPLIER * img.Width),
                    MaxHeight = (int) (BLOB_MAX_DIMENSIONS_MULTIPLIER * img.Height),
                    // Order by size
                    ObjectsOrder = ObjectsOrder.Size
                };
                blobCounter.ProcessImage(thresholdedImg);

                // Log a visualisation of the blob recognition
                if (imageLog != null)
                {
                    Bitmap blobRecognitionVis = DrawBlobRecognition.Draw(blobCounter, img.Width, img.Height);
                    imageLog.Log(blobRecognitionVis, "Candidate Detection: Blob Recognition");
                }

                BlobMeta[] blobs = blobCounter.GetObjectsInformation().Select(b => new BlobMeta(b)).ToArray();
                // TODO: exclude blobs by doing blob recognition on a low-res version of the image (e.g. 100x100).
                //  Regions of background noise will blur together in the low-res image, so any large blobs there
                //  will be lots of small blobs in the full-res one. Those hundreds of small blobs can be excluded because
                //  they overlap with the large blob in the low-res image, greatly reducing the search space

                // Now try and find some lattices
                List<BlobLatticeElement[]> lattices = new List<BlobLatticeElement[]>();
                while (true)
                {
                    // Find blobs that we haven't already used as part of another lattice
                    BlobMeta[] unused = blobs.Where(b => !b.Used).ToArray();

                    // If there are no unused blobs, stop
                    if (unused.Length == 0)
                    {
                        break;
                    }

                    // Blobs are ordered by size. Use the middle one as a starting point
                    BlobMeta startingBlob = unused[unused.Length / 2];

                    // TODO: Would location by a map that we spiralled out pixel by pixel be more efficient??
                    // Construct a lattice
                    BlobLatticeElement[] lattice = constructLattice(blobs, startingBlob);

                    // Check that this lattice meets overall lattice vetting criteria
                    if (vetLattice(lattice))
                    {
                        lattices.Add(lattice);
                    }
                }

                // Generate WordsearchCandidates from the lattices found
                return generateWordsearchCandidates(lattices, img, blobCounter);
            }
        }

        #endregion

        #region Private Methods

        private static BlobLatticeElement[] constructLattice(BlobMeta[] blobs, BlobMeta startingBlob)
        {
            // Generate lattice elements for each blob
            BlobLatticeElement[] possibleElements = blobs.Select(b => new BlobLatticeElement(b)).ToArray();

            // Find the lattice element for the starting blob
            BlobLatticeElement startingElement = possibleElements.First(ble => ble.Blob.Equals(startingBlob));

            // Keep track of whice lattice elements we need to find connections from (doing this here so we can work
            //  iteratively rather than recursively)
            LinkedList<BlobLatticeElement> toFindConnections = new LinkedList<BlobLatticeElement>();
            toFindConnections.AddLast(startingElement);
            startingElement.QueuedForSearch = true;

            LinkedListNode<BlobLatticeElement> node = toFindConnections.First;
            while (node != null)
            {
                // Process this node
                BlobLatticeElement thisLatticeElement = node.Value;
                thisLatticeElement.FindConnections(possibleElements);

                // Queue any connected elements that aren't already queued
                foreach (BlobLatticeElement le in thisLatticeElement.ConnectedTo)
                {
                    if (!le.QueuedForSearch)
                    {
                        le.QueuedForSearch = true;
                        toFindConnections.AddLast(le);
                    }
                }

                // Get the next node
                node = node.Next;
            }

            // We haven't been removing lattice elements from the linked list as we go, so this is now all
            //  elements in the lattice
            return toFindConnections.ToArray();
        }

        private static bool vetLattice(BlobLatticeElement[] lattice)
        {
            return lattice.Length >= MIN_LATTICE_ELEMENTS;
        }

        private static WordsearchCandidate[] generateWordsearchCandidates(IEnumerable<BlobLatticeElement[]> lattices, Bitmap originalImage, BlobCounter blobCounter)
        {
            List<WordsearchCandidate> candidates = new List<WordsearchCandidate>();

            foreach (BlobLatticeElement[] lattice in lattices)
            {
                // Get all of the points for each blob making up the lattice in one list (required for graham scan)
                List<IntPoint> points = lattice.SelectMany(le => blobCounter.GetBlobsEdgePoints(le.Blob.Blob)).ToList();

                // Calculate the convex hull for this lattice using a Graham Scan
                GrahamConvexHull grahamScan = new GrahamConvexHull();
                List<IntPoint> convexHull = grahamScan.FindHull(points);

                // Check to see if it's quadrilateral
                SimpleShapeChecker simpleShapeChecker = new SimpleShapeChecker();
                List<IntPoint> corners;
                if (simpleShapeChecker.IsQuadrilateral(convexHull, out corners))
                {
                    candidates.Add(new WordsearchCandidate(corners,
                        new QuadrilateralTransformation(corners).Apply(originalImage)));
                }
            }

            return candidates.ToArray();
        }

        #endregion
    }
}
