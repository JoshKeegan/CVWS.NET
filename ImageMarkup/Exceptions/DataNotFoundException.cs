﻿/*
 * Dissertation CV Wordsearch Solver
 * Image Markup
 * Data Not Found Exception Class
 * By Josh Keegan 26/02/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageMarkup.Exceptions
{
    class DataNotFoundException : Exception
    {
        //Constructors
        public DataNotFoundException() {  }

        public DataNotFoundException(string message) : base(message) { }
    }
}
