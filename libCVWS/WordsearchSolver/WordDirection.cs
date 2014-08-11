/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS
 * WordDirection enum
 * By Josh Keegan 16/05/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.WordsearchSolver
{
    public enum WordDirection
    {
        Up, Right, Down, Left,
        UpRight, DownRight, DownLeft, UpLeft,
        None //Represents words of length 1
    }

    /*
     * Internal enums used during the calculation of the overall WordDirection
     */
    internal enum HorizontalWordDirection
    {
        Left, Right, None
    }

    internal enum VerticalWordDirection
    {
        Up, Down, None
    }

    internal static class WordDirectionHelpers
    {
        internal static WordDirection GetWordDirection(HorizontalWordDirection horizontalDirection, VerticalWordDirection verticalDirection)
        {
            WordDirection wordDirection;
            switch(horizontalDirection)
            {
                case HorizontalWordDirection.Right:
                    switch(verticalDirection)
                    {
                        case VerticalWordDirection.Down:
                            wordDirection = WordDirection.DownRight;
                            break;
                        case VerticalWordDirection.Up:
                            wordDirection = WordDirection.UpRight;
                            break;
                        default:
                            wordDirection = WordDirection.Right;
                            break;
                    }
                    break;
                case HorizontalWordDirection.Left:
                    switch (verticalDirection)
                    {
                        case VerticalWordDirection.Down:
                            wordDirection = WordDirection.DownLeft;
                            break;
                        case VerticalWordDirection.Up:
                            wordDirection = WordDirection.UpLeft;
                            break;
                        default:
                            wordDirection = WordDirection.Left;
                            break;
                    }
                    break;
                default:
                    switch (verticalDirection)
                    {
                        case VerticalWordDirection.Down:
                            wordDirection = WordDirection.Down;
                            break;
                        case VerticalWordDirection.Up:
                            wordDirection = WordDirection.Up;
                            break;
                        default:
                            wordDirection = WordDirection.None; //Special case of a single length word
                            break;
                    }
                    break;                            
            }
            return wordDirection;
        }
    }
}
