/*
 * CVWS.NET
 * libCvws
 * Intermediate Image Log abstract class - Allows an algorithm to log images
 *  as it runs, so that its inner workings can be better visualised and understood.
 * Authors:
 *  Josh Keegan 16/04/2017
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.IntermediateImageLogging
{
    public abstract class IntermediateImageLog
    {
        public abstract void Log(Bitmap image, string name);

        // TODO: Could add a Log method that takes arguments for name formatting via String.Format
    }
}
