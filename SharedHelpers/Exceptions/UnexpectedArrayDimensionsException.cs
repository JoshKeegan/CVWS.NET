﻿/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Shared Helpers Exceptions
 * UnexpectedArrayDiensionsException
 * By Josh Keegan 03/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.Exceptions
{
    public class UnexpectedArrayDimensionsException : Exception
    {
        //Constructors
        public UnexpectedArrayDimensionsException() { }

        public UnexpectedArrayDimensionsException(string message) : base(message) { }

        public UnexpectedArrayDimensionsException(string message, Exception inner) : base(message, inner) { }
    }
}
