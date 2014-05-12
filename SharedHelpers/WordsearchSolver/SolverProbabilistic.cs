/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Solver Algorithm (using the probabilities for each character)
 * By Josh Keegan 12/05/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedHelpers.WordsearchSolver
{
    public class SolverProbabilistic : Solver
    {
        protected override Solution doSolve(double[][][] wordsearch, string[] words)
        {
            //Try each word in each possible position
            Tuple<double, WordPosition>[] bestPositions = new Tuple<double, WordPosition>[words.Length];
            //Set some default value
            for(int i = 0; i < bestPositions.Length; i++)
            {
                bestPositions[i] = null;
            }

            //Loop through each character in the wordsearch
            for(int i = 0; i < wordsearch.Length; i++) //Col
            {
                for(int j = 0; j < wordsearch[i].Length; j++) //Row
                {
                    //Loop over each word to be found
                    for(int k = 0; k < words.Length; k++)
                    {
                        string word = words[k];

                        //Calculate the indices it would finish at in each direction
                        int finishUp = j - word.Length + 1;
                        int finishDown = j + word.Length - 1;
                        int finishLeft = i - word.Length + 1;
                        int finishRight = i + word.Length - 1;

                        //Calculate whether the word could fit in each direction
                        bool canGoUp = finishUp >= 0;
                        bool canGoDown = finishDown < wordsearch[i].Length;
                        bool canGoLeft = finishLeft >= 0;
                        bool canGoRight = finishRight < wordsearch.Length;

                        //Check if the word could fit in each direction
                        //Up
                        if(canGoUp)
                        {
                            double score = 0;
                            for(int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i][j - l][thisCharIdx];
                            }

                            //If this score is better than the current best then this becomes the new best (or if there isn't currently a best)
                            if(bestPositions[k] == null || bestPositions[k].Item1 < score)
                            {
                                bestPositions[k] = Tuple.Create(score, new WordPosition(i, j, i, finishUp));
                            }
                        }

                        //Down
                        if(canGoDown)
                        {
                            double score = 0;
                            for (int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i][j + l][thisCharIdx];
                            }

                            //If this score is better than the current best then this becomes the new best (or if there isn't currently a best)
                            if (bestPositions[k] == null || bestPositions[k].Item1 < score)
                            {
                                bestPositions[k] = Tuple.Create(score, new WordPosition(i, j, i, finishDown));
                            }
                        }

                        //Left
                        if(canGoLeft)
                        {
                            double score = 0;
                            for (int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i - l][j][thisCharIdx];
                            }

                            //If this score is better than the current best then this becomes the new best (or if there isn't currently a best)
                            if (bestPositions[k] == null || bestPositions[k].Item1 < score)
                            {
                                bestPositions[k] = Tuple.Create(score, new WordPosition(i, j, finishLeft, j));
                            }
                        }

                        //Right
                        if(canGoRight)
                        {
                            double score = 0;
                            for (int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i + l][j][thisCharIdx];
                            }

                            //If this score is better than the current best then this becomes the new best (or if there isn't currently a best)
                            if (bestPositions[k] == null || bestPositions[k].Item1 < score)
                            {
                                bestPositions[k] = Tuple.Create(score, new WordPosition(i, j, finishRight, j));
                            }
                        }

                        //Up & Left
                        if(canGoUp && canGoLeft)
                        {
                            double score = 0;
                            for (int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i - l][j - l][thisCharIdx];
                            }

                            //If this score is better than the current best then this becomes the new best (or if there isn't currently a best)
                            if (bestPositions[k] == null || bestPositions[k].Item1 < score)
                            {
                                bestPositions[k] = Tuple.Create(score, new WordPosition(i, j, finishLeft, finishUp));
                            }
                        }

                        //Up & Right
                        if(canGoUp && canGoRight)
                        {
                            double score = 0;
                            for (int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i + l][j - l][thisCharIdx];
                            }

                            //If this score is better than the current best then this becomes the new best (or if there isn't currently a best)
                            if (bestPositions[k] == null || bestPositions[k].Item1 < score)
                            {
                                bestPositions[k] = Tuple.Create(score, new WordPosition(i, j, finishRight, finishUp));
                            }
                        }

                        //Down & Right
                        if(canGoDown && canGoRight)
                        {
                            double score = 0;
                            for (int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i + l][j + l][thisCharIdx];
                            }

                            //If this score is better than the current best then this becomes the new best (or if there isn't currently a best)
                            if (bestPositions[k] == null || bestPositions[k].Item1 < score)
                            {
                                bestPositions[k] = Tuple.Create(score, new WordPosition(i, j, finishRight, finishDown));
                            }
                        }

                        //Down & Left
                        if(canGoDown && canGoLeft)
                        {
                            double score = 0;
                            for (int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i - l][j + l][thisCharIdx];
                            }

                            //If this score is better than the current best then this becomes the new best (or if there isn't currently a best)
                            if (bestPositions[k] == null || bestPositions[k].Item1 < score)
                            {
                                bestPositions[k] = Tuple.Create(score, new WordPosition(i, j, finishLeft, finishDown));
                            }
                        }
                    }
                }
            }

            //Combine all of the best word positions into a solution
            Solution solution = new Solution();
            for(int i = 0; i < bestPositions.Length; i++)
            {
                solution.Add(words[i], bestPositions[i].Item2);
            }
            return solution;
        }
    }
}
