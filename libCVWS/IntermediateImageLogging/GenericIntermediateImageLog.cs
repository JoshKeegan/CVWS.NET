/*
 * CVWS.NET
 * libCvws
 * Generic IntermediateImageLog - a generic implementation of an intermediate image log
 *  using a delegate to offload the logging method to the user.
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
    public class GenericIntermediateImageLog : IntermediateImageLog
    {
        #region Public Variables

        public readonly LogImage Method;

        #endregion

        #region Implement IIntermediateImageLog

        public override void Log(Bitmap image, string name)
        {
            Method(image, name);
        }

        #endregion

        #region Constructors

        public GenericIntermediateImageLog(LogImage method)
        {
            Method = method;
        }

        #endregion

        #region Delegates

        public delegate void LogImage(Bitmap image, string name);

        #endregion
    }
}
