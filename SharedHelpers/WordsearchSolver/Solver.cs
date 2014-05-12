/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Solver Algorithm - abstract class
 * By Josh Keegan 26/04/2014
 * Last Edit 12/05/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedHelpers.Exceptions;

namespace SharedHelpers.WordsearchSolver
{
    public abstract class Solver
    {
        public Solution Solve(double[][][] wordsearch, string[] words)
        {
            //Validation checks

            //Check that the largest word can fit into the wordsearch
            int maxWordLength = 0;
            foreach(string word in words)
            {
                if(word.Length > maxWordLength)
                {
                    maxWordLength = word.Length;
                }
            }

            if(wordsearch.Length < maxWordLength && wordsearch[0].Length < maxWordLength)
            {
                throw new InvalidRowsAndColsException(String.Format("Not all words can fit into the wordsearch. Longest word is {0} characters long & the wordsearch is only {1}x{2}",
                    maxWordLength, wordsearch.Length, wordsearch[0].Length));
            }

            return doSolve(wordsearch, words);
        }

        protected abstract Solution doSolve(double[][][] wordsearch, string[] words);
    }
}
