/*
 * Dissertation CV Wordsearch Solver
 * Shared Helpers
 * Maths.Geometry class
 * By Josh Keegan 05/03/2014
 * Last Edit 08/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge;

using SharedHelpers.Exceptions;

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

        //Calculates the area of an arbitrary polygon using the Shoelace algorithm http://en.wikipedia.org/wiki/Shoelace_formula
        public static double Area(IntPoint[] points)
        {
            //There must be at least 3 points in any shape with an area
            if(points.Length < 3)
            {
                throw new InvalidShapeException("A shape requires at least 3 points in order to have an Area");
            }

            //Order the points before computing the area (since the algorithm requires them to be ordered)
            IntPoint[] clockwisePoints = SortPointsClockwise(points);

            ulong sumA = 0;
            ulong sumB = 0;
            for(int i = 0; i < clockwisePoints.Length - 1; i++)
            {
                sumA += (ulong)(clockwisePoints[i].X * clockwisePoints[i + 1].Y);
                sumB += (ulong)(clockwisePoints[i + 1].X * clockwisePoints[i].Y);
            }
            sumA += (ulong)(clockwisePoints[clockwisePoints.Length - 1].X * clockwisePoints[0].Y);
            sumB += (ulong)(clockwisePoints[0].X * clockwisePoints[clockwisePoints.Length - 1].Y);

            //Because the points are ordered clockwise, sumB will always be greater than sumA so for half their difference we don't need to check for neg
            return (double)(sumB - sumA) / 2;
        }

        //Sort Points into a clockwise rotation. Maintain the starting point incase it is for a specific winding order (as is used for wordsearch images)
        public static IntPoint[] SortPointsClockwise(IntPoint[] unsorted)
        {
            //Implementation of http://stackoverflow.com/a/6989383 with additional constraint of maintaining the starting coordinate

            //Calculate centre of the polygon (mean of all points)
            double centreX = 0;
            double centreY = 0;
            foreach(IntPoint p in unsorted)
            {
                centreX += p.X;
                centreY += p.Y;
            }
            centreX /= unsorted.Length;
            centreY /= unsorted.Length;

            //Sort the points around 12 noon
            List<IntPoint> points = new List<IntPoint>(unsorted);
            points.Sort((a, b) =>
                {
                    //Compare the 2 points, relative to the centre

                    //If one point is left of the centre and the other is to the right, problem is trivial:
                    if(a.X - centreX >= 0 && b.X - centreX < 0)
                    {
                        return -1;
                    }
                    if(a.X - centreX < 0 && b.X - centreX >= 0)
                    {
                        return 1;
                    }

                    //If both points lie on the centre line
                    if(a.X - centreX == 0 && b.X - centreX == 0)
                    {
                        //Order by points furthest out
                        if(a.Y - centreY >= 0 || b.Y - centreY >=0)
                        {
                            return a.Y > b.Y ? -1 : 1;
                        }
                        return b.Y > a.Y ? -1 : 1;
                    }

                    //Compute the cross product of the vectors (centre -> a) x (centre -> b)
                    double det = (a.X - centreX) * (b.Y - centreY) - (b.X - centreX) * (a.Y - centreY);
                    if(det < 0)
                    {
                        return -1;
                    }
                    if(det > 0)
                    {
                        return 1;
                    }

                    //det = 0 => points are on the same line from the centre, check which point is closer to the centre
                    double dA = ((a.X - centreX) * (a.X - centreX)) + ((a.Y - centreY) * (a.Y - centreY));
                    double dB = ((b.X - centreX) * (b.X - centreX)) + ((b.Y - centreY) * (b.Y - centreY));
                    return dA > dB ? -1 : 1;
                });

            //Maintain the first point
            
            //Locate the first point in the ordered points
            int firstPointIdx = 0;
            for(; firstPointIdx < points.Count; firstPointIdx++)
            {
                //If we've found the position of the first point
                if (points[firstPointIdx].X == unsorted[0].X && points[firstPointIdx].Y == unsorted[0].Y)
                {
                    break;
                }
            }

            //Create a new array of points with the first one back in first position
            IntPoint[] sorted = new IntPoint[unsorted.Length];
            for(int i = 0; i < sorted.Length; i++)
            {
                sorted[i] = points[(i + firstPointIdx) % points.Count];
            }
            return sorted;
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
