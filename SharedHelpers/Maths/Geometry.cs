/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Maths.Geometry class
 * By Josh Keegan 05/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;

using SharedHelpers.Maths.Exceptions;

namespace SharedHelpers.Maths
{
    public static class Geometry
    {
        public static double EuclideanDistance(IntPoint a, IntPoint b)
        {
            int width = b.X - a.X;
            int height = b.Y - a.Y;
            return Math.Sqrt((width * width) + (height * height));
        }

        public static double AreaQuadrilateral(ICollection<IntPoint> points)
        {
            IntPoint[] arrPoints = points.ToArray();
            return AreaQuadrilateral(arrPoints);
        }

        public static double AreaQuadrilateral(IntPoint[] points)
        {
            if(!IsQuadrilateral(points))
            {
                throw new InvalidShapeException("Expected Quadrilateral");
            }

            return Area(points[0], points[1], points[2], points[3]);
        }

        //Implements general equation from http://www.wikihow.com/Find-the-Area-of-a-Quadrilateral. Might be better to change to more general implementation for any polygon at some point (e.g Shoelace formula)
        public static double Area(IntPoint topLeft, IntPoint topRight, IntPoint bottomRight, IntPoint bottomLeft)
        {
            double a = EuclideanDistance(topLeft, bottomLeft);
            double b = EuclideanDistance(bottomLeft, bottomRight);
            double c = EuclideanDistance(bottomRight, topRight);
            double d = EuclideanDistance(topRight, topLeft);

            double bottomLeftToTopRight = EuclideanDistance(bottomLeft, topRight);
            //double topLeftToBottomRight = EuclideanDistance(topLeft, bottomRight);

            double A = TriangleAngle(a, bottomLeftToTopRight, d);
            double C = TriangleAngle(c, bottomLeftToTopRight, c);

            double topLeftTriangleArea = (a * d * Math.Sin(A)) / 2;
            double bottomRightTriangleArea = (b * c * Math.Sin(C)) / 2;

            return topLeftTriangleArea + bottomRightTriangleArea;
        }

        //get the angle between side a & c (i.e. angle B) in any triangle where all 3 sides are known
        public static double TriangleAngle(double a, double b, double c)
        {
            return Math.Acos((c * c + a * a - b * b) / (2 * c * a)); //law of cosines (for angle B)
        }

        public static bool IsQuadrilateral(ICollection<IntPoint> points)
        {
            IntPoint[] arrPoints = points.ToArray();
            return IsQuadrilateral(arrPoints);
        }

        public static bool IsQuadrilateral(IntPoint[] points)
        {
            return points.Length == 4;
        }
    }
}
