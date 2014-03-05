/*
 * Dissertation Cv Wordsearch Solver
 * Shared Helpers Exceptions
 * InvalidRowsAndColsException
 * By Josh Keegan 05/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.Exceptions
{
    public class InvalidRowsAndColsException : Exception
    {
        //Constructors
        public InvalidRowsAndColsException() { }

        public InvalidRowsAndColsException(string message) : base(message) { }

        public InvalidRowsAndColsException(string message, Exception inner) : base(message, inner) { }
    }
}
