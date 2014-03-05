/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Invalid Shape Exception class
 * By Josh Keegan 05/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.Maths.Exceptions
{
    public class InvalidShapeException : Exception
    {
        //Constructors
        public InvalidShapeException() { }

        public InvalidShapeException(string message) : base(message) { }
    }
}
