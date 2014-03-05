﻿/*
 * Dissertation CV Wordsearch Solver
 * Image Markup
 * Wordsearch class
 * By Josh Keegan 26/02/2014
 * Last Edit 05/03/2013
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageMarkup.Exceptions;

namespace ImageMarkup
{
    public class Wordsearch
    {
        //Private variables

        //Public variables
        public string Id { get; private set; }
        public char[,] Chars { get; private set; }
        public string[] Words { get; private set; }

        public int Rows
        {
            get
            {
                return Chars.GetLength(1);
            }
        }

        public int Cols
        {
            get
            {
                return Chars.GetLength(0);
            }
        }

        public string StrChars
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                for(int i = 0; i < Chars.GetLength(0); i++)
                {
                    for(int j = 0; j < Chars.GetLength(1); j++)
                    {
                        builder.Append(Chars[i, j]);
                    }

                    //Don't append a newline on the last line
                    if(i != Chars.GetLength(0) - 1)
                    {
                        builder.Append("\n");
                    }
                }
                return builder.ToString();
            }
        }

        //Constructor
        public Wordsearch(string id, char[,] chars, string[] words)
        {
            this.Id = id;
            this.Chars = chars;
            this.Words = words;
        }
    }
}
