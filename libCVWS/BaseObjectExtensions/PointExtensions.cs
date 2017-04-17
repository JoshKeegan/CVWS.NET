/*
 * CVWS.NET
 * libCvws
 * AForge.NET Point Extension Methods
 * Authors:
 *  Josh Keegan 17/04/2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;

namespace libCVWS.BaseObjectExtensions
{
    public static class PointExtensions
    {
        /// <summary>
        /// Calculates the angle to another Point.
        /// Angle is from the positive x axis, from the positive Y side & angle is measured in radians
        /// </summary>
        public static double AngleTo(this Point a, Point b)
        {
            float opp = Math.Abs(b.X - a.X);
            float adj = Math.Abs(b.Y - a.Y);

            double theta = Math.Atan(adj / opp);

            // If b is to the left of a (b.X < a.X) then the angle we've calculated is actually to the negative x axis
            //  So we must correct for that
            if (b.X < a.X)
            {
                theta = Math.PI - theta;
            }
            // If b is below a (b.Y < a.Y) then the angle we've calculated is to the positive x axis, but from the 
            //  negative Y side, so correct for that too
            if (b.Y < a.Y)
            {
                theta = (Math.PI * 2) - theta;
            }

            return theta;
        }
    }
}
