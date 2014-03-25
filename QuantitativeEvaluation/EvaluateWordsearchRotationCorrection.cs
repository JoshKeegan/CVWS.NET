/**
 * Dissertation CV Wordsearch Solver
 * Quantitative Evaluation
 * Evaluate Wordsearch Rotation Correction
 * By Josh Keegan 25/03/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Imaging.Filters;

using BaseObjectExtensions;
using ImageMarkup;
using SharedHelpers.ClassifierInterfacing;
using SharedHelpers.BespokeAlgorithms;

namespace QuantitativeEvaluation
{
    internal static class EvaluateWordsearchRotationCorrection
    {
        internal static double Evaluate(List<WordsearchImage> wordsearchImages, Classifier classifier)
        {
            Log.Info("Evaluating Wordsearch Image Rotation Correction . . .");

            int numCorrect = 0;
            int numTotal = 0;

            //Test the algorithm on each Wordsearch Image
            foreach(WordsearchImage wordsearchImage in wordsearchImages)
            {
                //Register an interest in the Bitmap of the wordsearch Image
                wordsearchImage.RegisterInterestInBitmap();

                //Test the system on each posisble rotation of the wordsearch image
                int[] angles = new int[] { 0, 90, 180, 270 };
                String correctHash = wordsearchImage.Bitmap.GetDataHashCode();

                foreach(int angle in angles)
                {
                    WordsearchRotation rotation = new WordsearchRotation(wordsearchImage.Bitmap.DeepCopy(), (int)wordsearchImage.Rows, (int)wordsearchImage.Cols);
                    rotation.Rotate(angle);

                    WordsearchRotation correctRotation = WordsearchRotationCorrection.CorrectOrientation(rotation, classifier);
                    string proposedSolutionHash = correctRotation.Bitmap.GetDataHashCode();

                    //Keep track of the number correct & total number
                    if(proposedSolutionHash == correctHash)
                    {
                        numCorrect++;
                    }
                    numTotal++;

                    //Clean up
                    rotation.Bitmap.Dispose();
                    correctRotation.Bitmap.Dispose();
                }

                //Clean up
                wordsearchImage.DeregisterInterestInBitmap();
            }

            Log.Info(String.Format("Returned {0}/{1} Wordsearch Rotations Correctly", numCorrect, numTotal));

            Log.Info("Wordsearch Image Rotation Evaluation Completed");

            return (double)numCorrect / numTotal;
        }
    }
}
