/*
 * Computer Vision Wordsearch Solver
 * Demo GUI
 * Invalid Words Exception
 * By Josh Keegan 13/05/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoGUI.Exceptions
{
    public class InvalidWordsException : Exception
    {
        //Constructors
        public InvalidWordsException() { }

        public InvalidWordsException(string message) : base(message) { }

        public InvalidWordsException(string message, Exception inner) : base(message, inner) { }
    }
}
