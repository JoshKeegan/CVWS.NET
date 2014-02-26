/*
 * Dissertation CV Wordsearch Solver
 * Image Markup
 * Database Not Initialised Exception class
 * By Josh Keegan 26/02/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageMarkup.Exceptions
{
    class DatabaseNotInitialisedException : Exception
    {
        //Constructors
        public DatabaseNotInitialisedException() { }

        public DatabaseNotInitialisedException(string message) : base(message) { }
    }
}
