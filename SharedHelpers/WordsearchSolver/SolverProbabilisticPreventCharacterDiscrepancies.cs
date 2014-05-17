/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Wordsearch Solver Algorithm (using the probabilities for each character, and prevent a single
 *  character from being used in two words as two different letters)
 * By Josh Keegan 16/05/2014
 * Last Edit 17/05/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;

namespace SharedHelpers.WordsearchSolver
{
    public class SolverProbabilisticPreventCharacterDiscrepancies : Solver
    {
        //Constants
        private const int MAX_TREE_DEPTH = 3; //inclusive & zero-indexed

        //Private variables
        private LinkedList<Tuple<int, Dictionary<string, int>>> openNodes; //Tree Level => Selected Positions
        private List<Dictionary<string, int>> closedNodes;

        protected override Solution doSolve(double[][][] wordsearch, string[] words)
        {
            //Get every possible position for each word to be in
            Dictionary<string, List<Tuple<double, WordPosition>>> wordsPositions = getAllPossibleWordPositions(wordsearch, words);

            //Sort the possibilities for each word so that the best possibility is the first item in each list
            sortPossibleWordsearchPositionsDesc(wordsPositions);

            //Perform a breadth-firt search of the possible combinations of WordPositions
            //  Note that the starting point for this is the best position for each word placed irrespective of all others
            //  and that due to the size of the search tree it will aim to find a solution as quickly as possible from that
            //  starting point, and never evaluate the whole tree (or even a large portion of it)
            int numCols = wordsearch.Length;
            int numRows = wordsearch[0].Length;
            Solution solution = findSolution(wordsPositions, numCols, numRows);

            //Clean up (not really necessary but due to the size they can become make sure the objects in them get freed up for GC ASAP)
            openNodes = null;
            closedNodes = null;

            return solution;
        }

        /*
         * Private Helpers
         */

        private static Dictionary<string, List<Tuple<double, WordPosition>>> getAllPossibleWordPositions(double[][][] wordsearch, string[] words)
        {
            //Initialise data structure to be returned
            Dictionary<string, List<Tuple<double, WordPosition>>> wordsPositions = new Dictionary<string, List<Tuple<double, WordPosition>>>();
            foreach(string word in words)
            {
                wordsPositions.Add(word, new List<Tuple<double, WordPosition>>());
            }

            //Loop through each character in the wordsearch
            for(int i = 0; i < wordsearch.Length; i++) //Cols
            {
                for(int j = 0; j < wordsearch[i].Length; j++) //Rows
                {
                    //Loop over each word to be found
                    foreach(string word in words)
                    {
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
                        if (canGoUp)
                        {
                            double score = 0;
                            for (int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i][j - l][thisCharIdx];
                            }

                            //Add this word position to the list of possible positions, along with it's score
                            wordsPositions[word].Add(Tuple.Create(score, new WordPosition(i, j, i, finishUp)));
                        }

                        //Down
                        if (canGoDown)
                        {
                            double score = 0;
                            for (int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i][j + l][thisCharIdx];
                            }

                            //Add this word position to the list of possible positions, along with it's score
                            wordsPositions[word].Add(Tuple.Create(score, new WordPosition(i, j, i, finishDown)));
                        }

                        //Left
                        if (canGoLeft)
                        {
                            double score = 0;
                            for (int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i - l][j][thisCharIdx];
                            }

                            //Add this word position to the list of possible positions, along with it's score
                            wordsPositions[word].Add(Tuple.Create(score, new WordPosition(i, j, finishLeft, j)));
                        }

                        //Right
                        if (canGoRight)
                        {
                            double score = 0;
                            for (int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i + l][j][thisCharIdx];
                            }

                            //Add this word position to the list of possible positions, along with it's score
                            wordsPositions[word].Add(Tuple.Create(score, new WordPosition(i, j, finishRight, j)));
                        }

                        //Up & Left
                        if (canGoUp && canGoLeft)
                        {
                            double score = 0;
                            for (int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i - l][j - l][thisCharIdx];
                            }

                            //Add this word position to the list of possible positions, along with it's score
                            wordsPositions[word].Add(Tuple.Create(score, new WordPosition(i, j, finishLeft, finishUp)));
                        }

                        //Up & Right
                        if (canGoUp && canGoRight)
                        {
                            double score = 0;
                            for (int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i + l][j - l][thisCharIdx];
                            }

                            //Add this word position to the list of possible positions, along with it's score
                            wordsPositions[word].Add(Tuple.Create(score, new WordPosition(i, j, finishRight, finishUp)));
                        }

                        //Down & Right
                        if (canGoDown && canGoRight)
                        {
                            double score = 0;
                            for (int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i + l][j + l][thisCharIdx];
                            }

                            //Add this word position to the list of possible positions, along with it's score
                            wordsPositions[word].Add(Tuple.Create(score, new WordPosition(i, j, finishRight, finishDown)));
                        }

                        //Down & Left
                        if (canGoDown && canGoLeft)
                        {
                            double score = 0;
                            for (int l = 0; l < word.Length; l++)
                            {
                                int thisCharIdx = word[l] - 'A';
                                score += wordsearch[i - l][j + l][thisCharIdx];
                            }

                            //Add this word position to the list of possible positions, along with it's score
                            wordsPositions[word].Add(Tuple.Create(score, new WordPosition(i, j, finishLeft, finishDown)));
                        }
                    }
                }
            }
            return wordsPositions;
        }

        private static void sortPossibleWordsearchPositionsDesc(Dictionary<string, List<Tuple<double, WordPosition>>> wordsPositions)
        {
            foreach(string word in wordsPositions.Keys.ToArray()) //Convert the words to an array in order to prevent a CollectionModifiedExceptions
            {
                wordsPositions[word] = wordsPositions[word].OrderByDescending(wordPosition => wordPosition.Item1).ToList();
            }
        }

        private Solution findSolution(Dictionary<string, List<Tuple<double, WordPosition>>> wordsPositions, int cols, int rows)
        {
            //Start by selecting the top solution for each word
            Dictionary<string, int> selectedPositions = new Dictionary<string, int>();
            foreach(string word in wordsPositions.Keys)
            {
                selectedPositions.Add(word, 0);
            }

            //Initialise any variables needed to perform the search
            openNodes = new LinkedList<Tuple<int, Dictionary<string, int>>>();
            closedNodes = new List<Dictionary<string, int>>();

            //Store the positions we selected as an open node (on tree level 0)
            openNodes.AddFirst(Tuple.Create(0, selectedPositions));

            //While there are open nodes
            int level = 0;
            List<Tuple<double, Solution>> validSolutions = new List<Tuple<double, Solution>>(); //Potential solutions at this level
            while(openNodes.Any())
            {
                //Pop the first value off & put it onlt closed nodes
                Tuple<int, Dictionary<string, int>> node = openNodes.First.Value;
                openNodes.RemoveFirst();
                closedNodes.Add(node.Item2);

                int nodeLevel = node.Item1;
                Dictionary<string, int> nodePotentialSolution = node.Item2;

                //If we're entering a new level of the tree
                if(nodeLevel != level)
                {
                    //Check to see if there were solutions on the current level
                    if(validSolutions.Any())
                    {
                        //Find the best solution at this level and use that
                        int bestIdx = 0;
                        for(int i = 1; i < validSolutions.Count; i++)
                        {
                            if(validSolutions[i].Item1 > validSolutions[bestIdx].Item1)
                            {
                                bestIdx = i;
                            }
                        }
                        return validSolutions[bestIdx].Item2;
                    }
                    else //Otherwise there weren't any solutions on the previous level, start on this next level
                    {
                        level = nodeLevel;

                        //If we've gone beyond the Max Depth
                        if(level > MAX_TREE_DEPTH)
                        {
                            break; //Break out of this loop in order to return the default solution
                        }
                    }
                }
                
                //Evaluate the next node. If it gives a valid Solution, add it to the list of solutions at this level
                if(evaluateNode(node, wordsPositions, cols, rows))
                {
                    Solution solution = generateSolution(nodePotentialSolution, wordsPositions);

                    //Score the Solution & store it
                    double score = scoreSolution(nodePotentialSolution, wordsPositions);
                    validSolutions.Add(Tuple.Create(score, solution));
                }
            }

            //We ran out of nodes to evaluate without finding a solution 
            //  (either because the root node is a valid solution, or there were no valid nodes before exceeding the Maximum Tree Depth)
            //  return the first position for every word as the solution
            return generateSolution(selectedPositions, wordsPositions);
        }

        //Generate a Solution object from some selected word positions
        private static Solution generateSolution(Dictionary<string, int> selectedPositions,
            Dictionary<string, List<Tuple<double, WordPosition>>> wordsPositions)
        {
            //Get the word positions that are selected
            Solution solution = new Solution();
            foreach (KeyValuePair<string, int> kvp in selectedPositions)
            {
                string word = kvp.Key;
                int positionIdx = kvp.Value;
                WordPosition position = wordsPositions[word][positionIdx].Item2;

                solution.Add(word, position);
            }
            return solution;
        }

        //Returns whether this node gives a valid Solution (i.e. one where there are no collisions)
        private bool evaluateNode(Tuple<int, Dictionary<string, int>> node, 
            Dictionary<string, List<Tuple<double, WordPosition>>> wordsPositions, int cols, int rows)
        {
            int level = node.Item1;
            Dictionary<string, int> selectedPositions = node.Item2;

            //Get the word positions that are selected
            Solution candidateSolution = generateSolution(selectedPositions, wordsPositions);

            //Generate a collision table for this candidate solution
            Dictionary<string, char>[,] collisionTable = generateCollisionTable(candidateSolution, cols, rows);

            //Find any collisions between words
            List<IntPoint> collisions = findCollisions(collisionTable);

            //If there are collisions
            if(collisions.Any())
            {
                //Generate the successors to this node
                List<Tuple<int, Dictionary<string, int>>> successors = new List<Tuple<int, Dictionary<string, int>>>();

                //At each place there is a collision
                foreach(IntPoint collisionPoint in collisions)
                {
                    //Try moving each word involved in that collision to every combination of their positions
                    //  up until one higher than the current position
                    foreach(string word in collisionTable[collisionPoint.X, collisionPoint.Y].Keys)
                    {
                        //The Word Position index currently being used for this word
                        int currentPositionIdx = selectedPositions[word];

                        for(int newPositionIdx = 0; newPositionIdx <= currentPositionIdx + 1; newPositionIdx++)
                        {
                            //Don't repeat the current position index. Saves some work for vet successors
                            if(newPositionIdx != currentPositionIdx)
                            {
                                //Clone the node
                                Dictionary<string, int> nextSelectedPositions = new Dictionary<string, int>(selectedPositions);

                                //Update the selected position for this word
                                nextSelectedPositions[word] = newPositionIdx;

                                //Add this node to the successors
                                successors.Add(Tuple.Create(level + 1, nextSelectedPositions));
                            }
                        }
                    }
                }

                //Vet the successors
                foreach(Tuple<int, Dictionary<string, int>> successorNode in successors)
                {
                    Dictionary<string, int> successorSelectedPositions = successorNode.Item2;

                    //If this is already on the list of open or closed nodes, don't add it to open to be processed *again*
                    bool foundClosed = false;
                    foreach(Dictionary<string, int> closedNode in closedNodes)
                    {
                        //Check to see if this node is the one we're vetting
                        if(areSelectedPositionsEqual(closedNode, successorSelectedPositions))
                        {
                            foundClosed = true;
                            break;
                        }
                    }

                    bool foundOpen = false;
                    //Don't bother searching open if we already found this node on closed
                    if(!foundClosed)
                    {
                        foreach (Tuple<int, Dictionary<string, int>> openNode in openNodes)
                        {
                            Dictionary<string, int> openNodeSelectedPositions = openNode.Item2;

                            //Check to see if this node is the one we're vetting
                            if (areSelectedPositionsEqual(openNodeSelectedPositions, successorSelectedPositions))
                            {
                                foundOpen = true;
                                break;
                            }
                        }
                    }
                    
                    //If we haven't found this node to already be on either open or closed, add it to open
                    if(!foundOpen && !foundClosed)
                    {
                        openNodes.AddLast(successorNode);
                    }
                }

                //This node didn't yield a valid solution, report that back
                return false;
            }
            else //Otherwise there are no collisions
            {
                return true;
            }
        }

        private static bool areSelectedPositionsEqual(Dictionary<string, int> a, Dictionary<string, int> b)
        {
            foreach (KeyValuePair<string, int> kvp in a)
            {
                string word = kvp.Key;
                
                if(kvp.Value != b[word])
                {
                    return false;
                }
            }
            //Don't bother checking if all of the items in each dictionary are present in the other, 
            //  assume they are due to the context of this function
            return true;
        }

        //Score a (valid) candidate solution (actually done with word positions instead of the Solution as 
        //  we've already pre-computed the score for each word in each position)
        private static double scoreSolution(Dictionary<string, int> selectedWordPositions, 
            Dictionary<string, List<Tuple<double, WordPosition>>> wordsPositions)
        {
            //Sum the scores for the individually selected positions to give the score of the overall solution
            double score = 0;
            foreach(KeyValuePair<string, int> kvp in selectedWordPositions)
            {
                string word = kvp.Key;
                int positionIdx = kvp.Value;

                score += wordsPositions[word][positionIdx].Item1;
            }
            return score;
        }

        //Generates a collision table (a lookup table for each cell in the wordsearch that shows each word using it and what character
        //  that word uses it as)
        private static Dictionary<string, char>[,] generateCollisionTable(Dictionary<string, WordPosition> wordPositions, int cols, int rows)
        {
            //Build up a representation of the Wordsearch and what characters each word uses
            Dictionary<string, char>[,] collisionTable = new Dictionary<string, char>[cols, rows];
            for(int i = 0; i < collisionTable.GetLength(0); i++)
            {
                for(int j = 0; j < collisionTable.GetLength(1); j++)
                {
                    collisionTable[i, j] = new Dictionary<string, char>();
                }
            }

            //Iterate over each word, populating the wordsearch
            foreach(KeyValuePair<string, WordPosition> kvp in wordPositions)
            {
                string word = kvp.Key;
                WordPosition position = kvp.Value;

                //Foreach character the word goes through
                int charIdx = 0;
                foreach(IntPoint charPosition in position.CharIndices)
                {
                    //Store that this word goes through this character
                    collisionTable[charPosition.X, charPosition.Y].Add(word, word[charIdx]);

                    charIdx++;
                }
            }

            return collisionTable;
        }

        //Find collisions (where two words use the same grid position as two different characters)
        private static List<IntPoint> findCollisions(Dictionary<string, char>[,] collisionTable)
        {
            List<IntPoint> collisions = new List<IntPoint>();

            for(int i = 0; i < collisionTable.GetLength(0); i++) //Cols
            {
                for(int j = 0; j < collisionTable.GetLength(1); j++) //Rows
                {
                    Dictionary<string, char> words = collisionTable[i, j];

                    //If there is more than one word making use of this character, check that they are all using it as the same character
                    if (words.Count > 1)
                    {
                        char? firstChar = null;
                        foreach (char c in words.Values)
                        {
                            //If first iter, store char
                            if (firstChar == null)
                            {
                                firstChar = c;
                            }
                            else //Otherwise this is a later iteration, check that the char this word is using this position as is the same as previous words
                            {
                                //If this word is using this charcter differently to how the first word used it, there's a collision
                                if (c != firstChar)
                                {
                                    collisions.Add(new IntPoint(i, j));
                                }
                            }
                        }
                    }
                }
            }
            return collisions;
        }
    }
}
