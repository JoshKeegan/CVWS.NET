/*
 * CVWS.NET
 * libCvws
 * Blob
 * Authors:
 *  Josh Keegan 16/04/2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Imaging;

namespace libCVWS.ImageAnalysis.WordsearchDetection
{
    internal class BlobMeta
    {
        // Public Variables
        public readonly Blob Blob;
        public bool Used;

        // Constructors
        public BlobMeta(Blob blob, bool used = false)
        {
            Blob = blob;
            Used = used;
        }

        // Public Methods
        public override bool Equals(object obj)
        {
            BlobMeta bm = obj as BlobMeta;
            if (bm == null)
            {
                return false;
            }

            return bm.Blob.ID == Blob.ID;
        }

        public override int GetHashCode()
        {
            return Blob.ID.GetHashCode();
        }
    }
}
