﻿/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS Exceptions
 * TrainableFeatureExtractionAlgorithmException
 * By Josh Keegan 11/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.Exceptions
{
    public class TrainableFeatureExtractionAlgorithmException : Exception
    {
        //Constructors
        public TrainableFeatureExtractionAlgorithmException() { }

        public TrainableFeatureExtractionAlgorithmException(string message) : base(message) { }

        public TrainableFeatureExtractionAlgorithmException(string message, Exception inner) : base(message, inner) { }
    }
}
