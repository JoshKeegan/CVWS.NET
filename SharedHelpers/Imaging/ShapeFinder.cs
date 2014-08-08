/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Shared Helpers
 * Shape Finder class - methods to find the positions of shapes in an image
 * By Josh Keegan 22/04/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;
using AForge.Imaging;
using AForge.Math.Geometry;

namespace SharedHelpers.Imaging
{
    public static class ShapeFinder
    {
        public static List<List<IntPoint>> Quadrilaterals(Bitmap img, int minWidth, int minHeight)
        {
            Bitmap thresholdedImg = FilterCombinations.AdaptiveThreshold(img);

            //Use blob recognition on the image to find all blobs meeting the specified minimum width & height
            BlobCounter blobCounter = new BlobCounter();

            //Filter out small blobs
            blobCounter.MinWidth = minWidth;
            blobCounter.MinHeight = minHeight;
            blobCounter.FilterBlobs = true;

            //Order the blobs by size (desc), as since we're looking for quads of a minimum size, it's likely we'll be more interested in the laregr ones
            blobCounter.ObjectsOrder = ObjectsOrder.Size;

            //Check if each blob is approximately a quadriateral, and if so store the 4 corners
            List<List<IntPoint>> foundQuads = new List<List<IntPoint>>();
            //Shape checker to be used to test if a blob is a quadrilateral
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();

            //Find the blobs
            blobCounter.ProcessImage(thresholdedImg);
            Blob[] blobs = blobCounter.GetObjectsInformation();

            foreach(Blob b in blobs)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(b);
                List<IntPoint> corners = null;

                //Is this blob approximately a quadrilateral?
                if(shapeChecker.IsQuadrilateral(edgePoints, out corners))
                {
                    //Store the Quad's corners
                    foundQuads.Add(corners);
                }
            }

            return foundQuads;
        }
    }
}
