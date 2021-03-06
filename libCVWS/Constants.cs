﻿/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS
 * Constants Class - for storing constants that will be used in many places in the program and have no obvious place to be stored
 * By Josh Keegan 25/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS
{
    public static class Constants
    {
        public const int CHAR_WITH_WHITESPACE_WIDTH = 20;
        public const int CHAR_WITH_WHITESPACE_HEIGHT = 25;

        internal const int PRIME1 = 23;
        internal const int PRIME2 = 41;
    }
}
