/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Solver Algorithm (Non-Probabilistic)
 * By Josh Keegan 26/04/2014
 * Last Edit 12/05/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedHelpers.ClassifierInterfacing;

namespace SharedHelpers.WordsearchSolver
{
    public class SolverNonProbabilistic : Solver
    {
        protected override Solution doSolve(double[][][] wordsearch, string[] words)
        {
            //Convert probablistic input to non-probablistic
            char[,] charWordsearch = NeuralNetworkHelpers.GetMostLikelyChars(wordsearch);

            return solve(charWordsearch, words);
        }

        private Solution solve(char[,] chars, string[] words)
        {
            Solution solution = new Solution();

            //Find the first solution for each word
            foreach(string word in words)
            {
                bool foundWord = false;

                //Loop through each character in the wordsearch, looking for this word
                for (int i = 0; i < chars.GetLength(0); i++) //Col
                {
                    for(int j = 0; j < chars.GetLength(1); j++) //Row
                    {
                        //If this character is the same as the starting character of the word then the word could start here
                        if (word[0] == chars[i, j])
                        {
                            //Calculate the indices it would finish at in each direction
                            int finishUp = j - word.Length + 1;
                            int finishDown = j + word.Length - 1;
                            int finishLeft = i - word.Length + 1;
                            int finishRight = i + word.Length - 1;

                            //Calculate whether the word could fit in each direction
                            bool canGoUp = finishUp >= 0;
                            bool canGoDown = finishDown < chars.GetLength(1);
                            bool canGoLeft = finishLeft >= 0;
                            bool canGoRight = finishRight < chars.GetLength(0);

                            //Check if there is a potential solution here in each direction
                            //Up
                            if (canGoUp)
                            {
                                bool found = true;
                                for (int k = 1; k < word.Length; k++)
                                {
                                    if (word[k] != chars[i, j - k])
                                    {
                                        found = false;
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    foundWord = true;
                                    solution.Add(word, i, j, i, finishUp);
                                    break;
                                }
                            }

                            //Down
                            if (canGoDown)
                            {
                                bool found = true;
                                for (int k = 1; k < word.Length; k++)
                                {
                                    if (word[k] != chars[i, j + k])
                                    {
                                        found = false;
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    foundWord = true;
                                    solution.Add(word, i, j, i, finishDown);
                                    break;
                                }
                            }

                            //Left
                            if (canGoLeft)
                            {
                                bool found = true;
                                for (int k = 1; k < word.Length; k++)
                                {
                                    if (word[k] != chars[i - k, j])
                                    {
                                        found = false;
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    foundWord = true;
                                    solution.Add(word, i, j, finishLeft, j);
                                    break;
                                }
                            }

                            //Right
                            if (canGoRight)
                            {
                                bool found = true;
                                for (int k = 1; k < word.Length; k++)
                                {
                                    if (word[k] != chars[i + k, j])
                                    {
                                        found = false;
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    foundWord = true;
                                    solution.Add(word, i, j, finishRight, j);
                                    break;
                                }
                            }

                            //Up & Left
                            if (canGoUp && canGoLeft)
                            {
                                bool found = true;
                                for (int k = 1; k < word.Length; k++)
                                {
                                    if (word[k] != chars[i - k, j - k])
                                    {
                                        found = false;
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    foundWord = true;
                                    solution.Add(word, i, j, finishLeft, finishUp);
                                    break;
                                }
                            }

                            //Up & Right
                            if (canGoUp && canGoRight)
                            {
                                bool found = true;
                                for (int k = 1; k < word.Length; k++)
                                {
                                    if (word[k] != chars[i + k, j - k])
                                    {
                                        found = false;
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    foundWord = true;
                                    solution.Add(word, i, j, finishRight, finishUp);
                                    break;
                                }
                            }

                            //Down & Right
                            if (canGoDown && canGoRight)
                            {
                                bool found = true;
                                for (int k = 1; k < word.Length; k++)
                                {
                                    if (word[k] != chars[i + k, j + k])
                                    {
                                        found = false;
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    foundWord = true;
                                    solution.Add(word, i, j, finishRight, finishDown);
                                    break;
                                }
                            }

                            //Down & Left
                            if (canGoDown && canGoLeft)
                            {
                                bool found = true;
                                for (int k = 1; k < word.Length; k++)
                                {
                                    if (word[k] != chars[i - k, j + k])
                                    {
                                        found = false;
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    foundWord = true;
                                    solution.Add(word, i, j, finishLeft, finishDown);
                                    break;
                                }
                            }
                        }
                    }

                    //If the word's been found, don't carry on looking
                    if(foundWord)
                    {
                        break;
                    }
                }
            }

            return solution;
        }
    }
}
