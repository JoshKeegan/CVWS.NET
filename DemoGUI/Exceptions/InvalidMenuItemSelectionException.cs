/*
 * Computer Vision Wordsearch Solver
 * Demo GUI
 * Invalid Menu Item Selection Exception
 * By Josh Keegan 13/05/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoGUI.Exceptions
{
    public class InvalidMenuItemSelectionException : Exception
    {
        //Constructors
        public InvalidMenuItemSelectionException() { }

        public InvalidMenuItemSelectionException(string message) : base(message) { }

        public InvalidMenuItemSelectionException(string message, Exception inner) : base(message, inner) { }
    }
}
