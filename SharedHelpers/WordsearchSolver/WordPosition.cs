/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Shared Helpers
 * Word Position class
 * By Josh Keegan 26/04/2014
 * Last Edit 16/05/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;

using SharedHelpers.Exceptions;

namespace SharedHelpers.WordsearchSolver
{
    public class WordPosition
    {
        //Private variables
        private HorizontalWordDirection? horizontalDirection = null;
        private VerticalWordDirection? verticalDirection = null;
        private WordDirection? wordDirection = null;


        private HorizontalWordDirection HorizontalDirection
        {
            get
            {
                //If the Horizontal Direction is unknown, calculate it
                if (horizontalDirection == null)
                {
                    if (StartCol < EndCol)
                    {
                        horizontalDirection = HorizontalWordDirection.Right;
                    }
                    else if (StartCol == EndCol)
                    {
                        horizontalDirection = HorizontalWordDirection.None;
                    }
                    else
                    {
                        horizontalDirection = HorizontalWordDirection.Left;
                    }
                }
                return horizontalDirection.GetValueOrDefault(); //Won't ever return default due to above if statement
            }
        }

        private VerticalWordDirection VerticalDirection
        {
            get
            {
                //If Vertical Direction is unknown, calculate it
                if(verticalDirection == null)
                {
                    if (StartRow < EndRow)
                    {
                        verticalDirection = VerticalWordDirection.Down;
                    }
                    else if (StartRow == EndRow)
                    {
                        verticalDirection = VerticalWordDirection.None;
                    }
                    else
                    {
                        verticalDirection = VerticalWordDirection.Up;
                    }
                }
                return verticalDirection.GetValueOrDefault(); //Won't ever return default due to above if statement
            }
        }

        //Public variables
        public int StartCol { get; private set; }
        public int StartRow { get; private set; }
        public int EndCol { get; private set; }
        public int EndRow { get; private set; }

        public WordDirection Direction
        {
            get
            {
                //If the Word Direction is unknown, calculate it
                if (wordDirection == null)
                {
                    wordDirection = WordDirectionHelpers.GetWordDirection(HorizontalDirection, VerticalDirection);
                }
                return wordDirection.GetValueOrDefault(); //Won't ever return default due to above if statement
            }
        }

        public IEnumerable<IntPoint> CharIndices
        {
            get
            {
                int x = StartCol;
                int y = StartRow;

                while(true)
                {
                    //Yield this position
                    yield return new IntPoint(x, y);

                    //Check to see if we're at the end of the word
                    if(x == EndCol && y == EndRow)
                    {
                        yield break;
                    }

                    //Increment the indices to those of the next character
                    switch(HorizontalDirection)
                    {
                        case HorizontalWordDirection.Left:
                            x--;
                            break;
                        case HorizontalWordDirection.Right:
                            x++;
                            break;
                    }
                    switch(VerticalDirection)
                    {
                        case VerticalWordDirection.Up:
                            y--;
                            break;
                        case VerticalWordDirection.Down:
                            y++;
                            break;
                    }
                }
            }
        }

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

        public bool Equals(WordPosition wordPosition)
        {
            return StartCol == wordPosition.StartCol &&
                StartRow == wordPosition.StartRow &&
                EndCol == wordPosition.EndCol &&
                EndRow == wordPosition.EndRow;
        }
    }
}
