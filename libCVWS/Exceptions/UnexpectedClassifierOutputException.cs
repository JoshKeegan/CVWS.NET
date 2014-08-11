/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS Exceptions
 * UnexpectedClassifierOutputException
 * By Josh Keegan 08/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.Exceptions
{
    public class UnexpectedClassifierOutputException : Exception
    {
        //Constructors
        public UnexpectedClassifierOutputException() { }

        public UnexpectedClassifierOutputException(string message) : base(message) { }

        public UnexpectedClassifierOutputException(string message, Exception inner) 
            : base(message, inner) { }
    }
}
