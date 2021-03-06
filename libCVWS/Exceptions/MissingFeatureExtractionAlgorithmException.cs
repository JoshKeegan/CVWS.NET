﻿/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS Exceptions
 * MissingFeatureExtractionAlgorithmException
 * By Josh Keegan 25/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.Exceptions
{
    public class MissingFeatureExtractionAlgorithmException : Exception
    {
        //Constructors
        public MissingFeatureExtractionAlgorithmException() { }

        public MissingFeatureExtractionAlgorithmException(string message) : base(message) { }

        public MissingFeatureExtractionAlgorithmException(string message, Exception inner) : base(message, inner) { }
    }
}
