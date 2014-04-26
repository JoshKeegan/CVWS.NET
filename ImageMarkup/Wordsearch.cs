/*
 * Dissertation CV Wordsearch Solver
 * Image Markup
 * Wordsearch class
 * By Josh Keegan 26/02/2014
 * Last Edit 26/04/2013
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageMarkup.Exceptions;
using SharedHelpers.WordsearchSolver;

namespace ImageMarkup
{
    public class Wordsearch
    {
        //Private variables
        private Dictionary<string, List<WordPosition>> solutions = null;

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
                for(int i = 0; i < Chars.GetLength(1); i++)
                {
                    for(int j = 0; j < Chars.GetLength(0); j++)
                    {
                        builder.Append(Chars[j, i]);
                    }

                    //Don't append a newline on the last line
                    if(i != Chars.GetLength(1) - 1)
                    {
                        builder.Append("\n");
                    }
                }
                return builder.ToString();
            }
        }

        public Dictionary<string, List<WordPosition>> Solutions
        {
            get
            {
                //If we haven't already found all possible positions for each word, find them
                if (this.solutions == null)
                {
                    //Initialise the data structure
                    solutions = new Dictionary<string,List<WordPosition>>();

                    foreach(string word in Words)
                    {
                        solutions.Add(word, new List<WordPosition>());
                    }

                    //Loop through each character in the wordsearch, trying each word in each possible direction
                    for(int i = 0; i < Chars.GetLength(0); i++) //Col num (left to right)
                    {
                        for(int j = 0; j < Chars.GetLength(1); j++) //Row num (top to bottom)
                        {
                            foreach(string word in Words)
                            {
                                //If this character is the same as the starting character of the word then the word could start here
                                if(word[0] == Chars[i, j])
                                {
                                    //Calculate the indices it would finish at in each direction
                                    int finishUp = j - word.Length + 1;
                                    int finishDown = j + word.Length - 1;
                                    int finishLeft = i - word.Length + 1;
                                    int finishRight = i + word.Length - 1;

                                    //Calculate whether the word could fit in each direction
                                    bool canGoUp = finishUp >= 0;
                                    bool canGoDown = finishDown < Chars.GetLength(1);
                                    bool canGoLeft = finishLeft >= 0;
                                    bool canGoRight = finishRight < Chars.GetLength(0);

                                    //Check if there is a potential solution here in each direction
                                    //Up
                                    if(canGoUp)
                                    {
                                        bool found = true;
                                        for(int k = 1; k < word.Length; k++)
                                        {
                                            if(word[k] != Chars[i, j - k])
                                            {
                                                found = false;
                                                break;
                                            }
                                        }
                                        if(found)
                                        {
                                            WordPosition position = new WordPosition(i, j, i, finishUp);
                                            solutions[word].Add(position);
                                        }
                                    }

                                    //Down
                                    if(canGoDown)
                                    {
                                        bool found = true;
                                        for(int k = 1; k < word.Length; k++)
                                        {
                                            if(word[k] != Chars[i, j + k])
                                            {
                                                found = false;
                                                break;
                                            }
                                        }
                                        if(found)
                                        {
                                            WordPosition position = new WordPosition(i, j, i, finishDown);
                                            solutions[word].Add(position);
                                        }
                                    }

                                    //Left
                                    if(canGoLeft)
                                    {
                                        bool found = true;
                                        for(int k = 1; k < word.Length; k++)
                                        {
                                            if(word[k] != Chars[i - k, j])
                                            {
                                                found = false;
                                                break;
                                            }
                                        }
                                        if(found)
                                        {
                                            WordPosition position = new WordPosition(i, j, finishLeft, j);
                                            solutions[word].Add(position);
                                        }
                                    }

                                    //Right
                                    if(canGoRight)
                                    {
                                        bool found = true;
                                        for(int k = 1; k < word.Length; k++)
                                        {
                                            if(word[k] != Chars[i + k, j])
                                            {
                                                found = false;
                                                break;
                                            }
                                        }
                                        if(found)
                                        {
                                            WordPosition position = new WordPosition(i, j, finishRight, j);
                                            solutions[word].Add(position);
                                        }
                                    }

                                    //Up & Left
                                    if(canGoUp && canGoLeft)
                                    {
                                        bool found = true;
                                        for(int k = 1; k < word.Length; k++)
                                        {
                                            if(word[k] != Chars[i - k, j - k])
                                            {
                                                found = false;
                                                break;
                                            }
                                        }
                                        if(found)
                                        {
                                            WordPosition position = new WordPosition(i, j, finishLeft, finishUp);
                                            solutions[word].Add(position);
                                        }
                                    }

                                    //Up & Right
                                    if(canGoUp && canGoRight)
                                    {
                                        bool found = true;
                                        for (int k = 1; k < word.Length; k++)
                                        {
                                            if (word[k] != Chars[i + k, j - k])
                                            {
                                                found = false;
                                                break;
                                            }
                                        }
                                        if (found)
                                        {
                                            WordPosition position = new WordPosition(i, j, finishRight, finishUp);
                                            solutions[word].Add(position);
                                        }
                                    }

                                    //Down & Right
                                    if(canGoDown && canGoRight)
                                    {
                                        bool found = true;
                                        for (int k = 1; k < word.Length; k++)
                                        {
                                            if (word[k] != Chars[i + k, j + k])
                                            {
                                                found = false;
                                                break;
                                            }
                                        }
                                        if (found)
                                        {
                                            WordPosition position = new WordPosition(i, j, finishRight, finishDown);
                                            solutions[word].Add(position);
                                        }
                                    }

                                    //Down && Left
                                    if(canGoDown && canGoLeft)
                                    {
                                        bool found = true;
                                        for (int k = 1; k < word.Length; k++)
                                        {
                                            if (word[k] != Chars[i - k, j + k])
                                            {
                                                found = false;
                                                break;
                                            }
                                        }
                                        if (found)
                                        {
                                            WordPosition position = new WordPosition(i, j, finishLeft, finishDown);
                                            solutions[word].Add(position);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return this.solutions;
            }
        }

        //Constructor
        public Wordsearch(string id, char[,] chars, string[] words)
        {
            this.Id = id;
            this.Chars = chars;
            this.Words = words;

            //Remove words of length 0 (empty string) in order to prevent problems later where words are assumed to be of length >= 1
            if(Words.Contains(""))
            {
                List<string> nonEmptyWords = new List<string>();
                foreach(string s in this.Words)
                {
                    if(s != "")
                    {
                        nonEmptyWords.Add(s);
                    }
                }
                this.Words = nonEmptyWords.ToArray();
            }
        }
    }
}
