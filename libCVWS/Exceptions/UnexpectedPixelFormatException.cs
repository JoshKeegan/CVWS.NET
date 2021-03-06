﻿/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS Exceptions
 * UnexpectedPixelFormatException
 * By Josh Keegan 06/03/2014
 * Last Edit 08/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.Exceptions
{
    public class UnexpectedPixelFormatException : Exception
    {
        //Constructors
        public UnexpectedPixelFormatException() { }

        public UnexpectedPixelFormatException(string message) : base(message) { }

        public UnexpectedPixelFormatException(string message, Exception inner) : base(message, inner) { }
    }
}
