/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Solver Algorithm - abstract class
 * By Josh Keegan 26/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.WordsearchSolver
{
    public abstract class Solver
    {
        public abstract Solution Solve(double[][][] wordsearch, string[] words);
    }
}
