/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers Imaging Exceptions
 * UnexpectedPixelFormatException
 * By Josh Keegan 06/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.Imaging.Exceptions
{
    public class UnexpectedPixelFormatException : Exception
    {
        //Constructors
        public UnexpectedPixelFormatException() { }

        public UnexpectedPixelFormatException(string message) : base(message) { }

        public UnexpectedPixelFormatException(string message, Exception inner) : base(message, inner) { }
    }
}
