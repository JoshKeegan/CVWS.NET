/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers Exceptions
 * InvalidImageDimensionsException
 * By Josh Keegan 03/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.Exceptions
{
    public class InvalidImageDimensionsException : Exception
    {
        //Constructors
        public InvalidImageDimensionsException() { }

        public InvalidImageDimensionsException(string message) : base(message) { }

        public InvalidImageDimensionsException(string message, Exception inner) : base(message, inner) { }
    }
}
