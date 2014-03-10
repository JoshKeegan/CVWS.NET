/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Classifier Helpers
 * By Josh Keegan 08/03/2014
 * Last Edit 10/03/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.ClassifierInterfacing
{
    public static class ClassifierHelpers
    {
        //Constants
        public const int NUM_CHAR_CLASSES = 26;

        //Public Methods
        public static string[] GetCharacterClassifierClassLabels()
        {
            string[] classLabels = new string[NUM_CHAR_CLASSES];

            for(int i = 0; i < classLabels.Length; i++)
            {
                classLabels[i] = ((char)('A' + i)).ToString();
            }
            return classLabels;
        }
    }
}
