/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Word Position class
 * By Josh Keegan 26/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedHelpers.Exceptions;

namespace SharedHelpers.WordsearchSolver
{
    public class WordPosition
    {
        //Public variables
        public int StartCol { get; private set; }
        public int StartRow { get; private set; }
        public int EndCol { get; private set; }
        public int EndRow { get; private set; }

        //Constructor
        public WordPosition(int startCol, int startRow, int endCol, int endRow)
        {
            //Validation: Check all values are >= 0
            if(startCol < 0 || startRow < 0 || endCol < 0 || endRow < 0)
            {
                throw new InvalidRowsAndColsException("Row and column indices must be >= 0 in word positions");
            }

            this.StartCol = startCol;
            this.StartRow = startRow;
            this.EndCol = endCol;
            this.EndRow = endRow;
        }
    }
}
