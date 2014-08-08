/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Demo GUI
 * Image processing Exception
 * By Josh Keegan 13/05/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoGUI.Exceptions
{
    public class ImageProcessingException : Exception
    {
        //Constructors
        public ImageProcessingException() { }

        public ImageProcessingException(string message) : base(message) { }

        public ImageProcessingException(string message, Exception inner) : base(message, inner) { }
    }
}
