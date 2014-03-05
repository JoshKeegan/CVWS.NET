/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers Exceptions
 * RegisteredInterestException class
 * By Josh Keegan 05/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.Exceptions
{
    public class RegisteredInterestException : Exception
    {
        //Constructors
        public RegisteredInterestException() { }

        public RegisteredInterestException(string message) : base(message) { }
    }
}
