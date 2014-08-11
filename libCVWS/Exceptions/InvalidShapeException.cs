/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS Exceptions
 * Invalid Shape Exception class
 * By Josh Keegan 05/03/2014
 * Last Edit 08/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.Exceptions
{
    public class InvalidShapeException : Exception
    {
        //Constructors
        public InvalidShapeException() { }

        public InvalidShapeException(string message) : base(message) { }
    }
}
