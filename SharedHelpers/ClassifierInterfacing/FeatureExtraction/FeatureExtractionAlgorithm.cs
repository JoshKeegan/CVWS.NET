﻿/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Feature Extraction Algorithm - abstract class 
 * By Josh Keegan 08/03/2014
 * Last Edit 11/03/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.ClassifierInterfacing.FeatureExtraction
{
    public abstract class FeatureExtractionAlgorithm
    {
        //Method is non-static to allow for feature reduction algorithms that learn what features are best to extract on the training data
        //The best features can be stored local to the instance this way
        public abstract double[][] Extract(Bitmap[] charImgs);
    }
}