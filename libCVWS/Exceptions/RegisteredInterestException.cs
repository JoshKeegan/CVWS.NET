/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS Exceptions
 * RegisteredInterestException class
 * By Josh Keegan 05/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.Exceptions
{
    public class RegisteredInterestException : Exception
    {
        //Constructors
        public RegisteredInterestException() { }

        public RegisteredInterestException(string message) : base(message) { }
    }
}
