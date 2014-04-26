/*
 * Dissertation CV Wordsearch Solver
 * Quantitative Evaluation
 * Wordsearch Solution Evaluator
 * By Josh Keegan 26/04/2014
 * 
 * Note that this doesn't currently extend Evaluator because it would require some changes to the structure of Evaluator.
 *  If ever Evaluator gets used more it may be worth spending the time reworking these classes so this is a child of Evaluator
 *  with as much as possible of the code from here going into Evaluator.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedHelpers.WordsearchSolver;

namespace QuantitativeEvaluation.Evaluators
{
    public class WordsearchSolutionEvaluator
    {
        //Private variables

        //Note that the following definitions are altered slightly to apply to the context of finding only a single solution for each word, rather than the full set of 
        //  solutions contained in the gold standard data set
        private int numTruePositive; // Words found in correct positions
        private int numFalsePositive; // Words found in the wrong position
        private int numFalseNegative; // Words not found but are in the wordsearch
        private int numTrueNegative; // Words not in wordsearch

        //Public Variables
        public double Precision
        {
            get
            {
                return (double)numTruePositive / (numTruePositive + numFalsePositive);
            }
        }

        public double Recall
        {
            get
            {
                return (double)numTruePositive / (numTruePositive + numFalseNegative);
            }
        }

        public double FMeasure
        {
            get
            {
                return 2 * ((Precision * Recall) / (Precision + Recall));
            }
        }

        public bool Correct
        {
            get
            {
                return numFalsePositive == 0 && numFalseNegative == 0;
            }
        }

        //Constructor
        public WordsearchSolutionEvaluator(Solution proposedSolution, Dictionary<string, List<WordPosition>> correctSolutions)
        {
            numTruePositive = 0;
            numFalsePositive = 0;
            numFalseNegative = 0;
            numTrueNegative = 0;

            //For each word there was to look for
            foreach(KeyValuePair<string, List<WordPosition>> kvp in correctSolutions)
            {
                string word = kvp.Key;
                List<WordPosition> correctPositions = kvp.Value;

                //Check this word is in the wordsearch
                if(correctPositions.Count > 0)
                {
                    //Did the proposed solution give a position for this word
                    if(proposedSolution.ContainsKey(word))
                    {
                        //Did the proposed solution contain this word in the correct position
                        bool inCorrectPosition = false;
                        foreach(WordPosition correctPosition in correctPositions)
                        {
                            if(correctPosition.Equals(proposedSolution[word]))
                            {
                                inCorrectPosition = true;
                                break;
                            }
                        }

                        if(inCorrectPosition)
                        {
                            numTruePositive++;
                        }
                        else //Otherwise the proposed solution contains the word in the wrong position (false positive)
                        {
                            numFalsePositive++;
                        }
                    }
                    else //Otherwise the proposed solution didn't find this word (false negative)
                    {
                        numFalseNegative++;
                    }
                }
                else //Otherwise this word was not in the wordsearch
                {
                    //Did the proposed solution give a position for this word (false positive)
                    if(proposedSolution.ContainsKey(word))
                    {
                        numFalsePositive++;
                    }
                    else //Otherwise the proposed solution recognised that this word isn't actually there (true negative)
                    {
                        numTrueNegative++;
                    }
                }
            }
        }

        public override string ToString()
        {
            if(Correct)
            {
                return "Solution Correct";
            }
            else
            {
                return String.Format("Solution Incorrect\nTrue Positives: {0}\nTrue Negatives: {1}\n"
                    + "False Positives: {2}\nFalse Negatives: {3}\nPrecision: {4}\nRecall: {5}\nF-Measure: {6}",
                    numTruePositive, numTrueNegative, numFalsePositive, numFalseNegative, Precision, Recall, FMeasure);
            }
        }
    }
}
