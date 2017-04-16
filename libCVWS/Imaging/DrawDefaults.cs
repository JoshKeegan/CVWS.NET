/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS
 * Draw Defaults class - default values used by Draw classes
 * Authors:
 *  Josh Keegan 04/03/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libCVWS.Imaging
{
    static class DrawDefaults
    {
        public static readonly Color DEFAULT_COLOUR = Color.Red;
        public static readonly Color DEFAULT_START_COLOUR = Color.Blue;
        public static readonly Color DEFAULT_END_COLOUR = Color.Red;
        public static readonly Color[] MULTIPLE_COLOURS = new Color[]
        {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Yellow,
            Color.Violet,
            Color.Brown,
            Color.Olive,
            Color.Cyan,
            Color.Magenta,
            Color.Gold,
            Color.Indigo,
            Color.Ivory,
            Color.HotPink,
            Color.DarkRed,
            Color.DarkGreen,
            Color.DarkBlue,
            Color.DarkSeaGreen,
            Color.Gray,
            Color.DarkKhaki,
            Color.DarkGray,
            Color.LimeGreen,
            Color.Tomato,
            Color.SteelBlue,
            Color.SkyBlue,
            Color.Silver,
            Color.Salmon,
            Color.SaddleBrown,
            Color.RosyBrown,
            Color.PowderBlue,
            Color.Plum,
            Color.PapayaWhip,
            Color.Orange
        };
    }
}
