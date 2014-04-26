/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Solution class
 * By Josh Keegan 26/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.WordsearchSolver
{
    public class Solution : Dictionary<string, WordPosition>
    {
        public void Add(string word, int startCol, int startRow, int endCol, int endRow)
        {
            this.Add(word, new WordPosition(startCol, startRow, endCol, endRow));
        }
    }
}
