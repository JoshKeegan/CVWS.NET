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

        public static double Area(IntPoint topLeft, IntPoint topRight, IntPoint bottomRight, IntPoint bottomLeft)
        {
            return Area(new IntPoint[] { topLeft, topRight, bottomRight, bottomLeft });
        }

        public static double Area(ICollection<IntPoint> points)
        {
            IntPoint[] arrPoints = points.ToArray();
            return Area(arrPoints);
        }

        //Implements the Shoelace algorithm http://en.wikipedia.org/wiki/Shoelace_formula
        public static double Area(IntPoint[] points)
        {
            //There must be at least 3 points in any shape with an area
            if(points.Length < 3)
            {
                throw new InvalidShapeException("A shape requires at least 3 points in order to have an Area");
            }

            //TODO: Check the order of the points are valid & reorder before calculations??

            ulong sumA = 0;
            ulong sumB = 0;
            for(int i = 0; i < points.Length - 1; i++)
            {
                sumA += (ulong)(points[i].X * points[i + 1].Y);
                sumB += (ulong)(points[i + 1].X * points[i].Y);
            }
            sumA += (ulong)(points[points.Length - 1].X * points[0].Y);
            sumB += (ulong)(points[0].X * points[points.Length - 1].Y);

            ulong diff;
            if(sumA > sumB)
            {
                diff = sumA - sumB;
            }
            else
            {
                diff = sumB - sumA;
            }
            return (double)diff / 2;
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
