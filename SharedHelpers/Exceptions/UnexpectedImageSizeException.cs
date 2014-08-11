/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS Exceptions
 * UnexpectedImageSizeException
 * By Josh Keegan 06/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.Exceptions
{
    public class UnexpectedImageSizeException : Exception
    {
        //Constructors
        public UnexpectedImageSizeException() { }

        public UnexpectedImageSizeException(string message) : base(message) { }

        public UnexpectedImageSizeException(string message, Exception inner) : base(message, inner) { }
    }
}
