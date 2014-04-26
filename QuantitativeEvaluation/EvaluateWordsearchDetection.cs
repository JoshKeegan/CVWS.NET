/*
 * Dissertation CV Wordsearch Solver
 * Quantitative Evaluation
 * Evaluate Wordsearch Detection
 * By Josh Keegan 22/04/2014
 * Last Edit 26/04/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bitmap = System.Drawing.Bitmap; //Bitmap only, else there will be a clash on Image w/ ImageMarkup.Image

using AForge;

using ImageMarkup;
using SharedHelpers.ImageAnalysis.WordsearchDetection;
using SharedHelpers.ImageAnalysis.WordsearchSegmentation;
using SharedHelpers.ImageAnalysis.WordsearchSegmentation.VariedRowColSize;
using SharedHelpers.Maths;

namespace QuantitativeEvaluation
{
    internal static class EvaluateWordsearchDetection
    {
        //Evaluate the wordsearch recognition system by checking that for each image it returns one of the wordsearches contained within
        //  (as some images may contain more than one wordsearch)
        internal static Dictionary<string, double> EvaluateReturnsWordsearch(List<Image> images)
        {
            Log.Info("Starting to Evaluate Wordsearch Recognition By Quadrilateral Detection and different methods of Wordsearch Segmentation");

            //Register an interest in the bitmaps of all the images (so that they remain in memory throughout)
            foreach(Image image in images)
            {
                image.RegisterInterestInBitmap();
            }

            Dictionary<string, double> scores = new Dictionary<string, double>();

            //Get the score for each segmentation algorithm (currently only ones that use row/col start & end points are supported, although this is actually all of them ATM)
            scores.Add("MeanDarkPixels", EvaluateReturnsWordsearch(images, new SegmentByMeanDarkPixels()));
            scores.Add("MedianDarkPixels", EvaluateReturnsWordsearch(images, new SegmentByMedianDarkPixels()));
            scores.Add("PercentileTwoThresholds", EvaluateReturnsWordsearch(images, new SegmentByPercentileTwoThresholds()));
            scores.Add("BlobRecognition", EvaluateReturnsWordsearch(images, new SegmentByBlobRecognition())); //TODO: Bug here prevents running (bug spotted when whole image was always being returned as blob)
            scores.Add("HistogramThresholdDarkPixels", EvaluateReturnsWordsearch(images, new SegmentByHistogramThresholdDarkPixels()));
            scores.Add("ThresholdDarkPixels", EvaluateReturnsWordsearch(images, new SegmentByThresholdDarkPixels()));
            scores.Add("HistogramThresholdPercentileRankTwoThresholds", EvaluateReturnsWordsearch(images, new SegmentByHistogramThresholdPercentileRankTwoThreshold()));

            //Deregsiter an interest in all of the images
            foreach(Image image in images)
            {
                image.DeregisterInterestInBitmap();
            }

            Log.Info("Completed evaluation of Wordsearch Recognition by Quadrilateral detection and different methods of Wordsearch Segmentation");

            return scores;        
        }

        private static double EvaluateReturnsWordsearch(List<Image> images, SegmentationAlgorithm segAlgorithm)
        {
            Log.Info("Evaluating Wordsearch Recognition by best wordsearch returned . . .");

            int numCorrect = 0;

            //Test the algorithm on each Image
            foreach (Image image in images)
            {
                //Register an interest in the Bitmap of the Image
                image.RegisterInterestInBitmap();

                Tuple<List<IntPoint>, Bitmap> bestCandidate = DetectionAlgorithm.ExtractBestWordsearch(image.Bitmap, segAlgorithm);
                
                //If we found a valid best candidate
                if (bestCandidate != null)
                {
                    //Check if the best candidate is a wordsearch in this image
                    List<IntPoint> points = bestCandidate.Item1;

                    if(IsWordsearch(points, image))
                    {
                        numCorrect++;
                    }
                    else
                    {
                        //Console.WriteLine("Incorrect"); //Debug Point (for viewing incorrect Bitmaps)
                    }
                }
                else //Otherwise we couldn't find anything that resembeled a quadrilateral (and could therefore be a wordsearch)
                {
                    
                }

                //Clean up
                bestCandidate.Item2.Dispose();
                image.DeregisterInterestInBitmap();
            }

            Log.Info(String.Format("Found a Wordsearch for {0} / {1} Images correctly", numCorrect, images.Count));
            Log.Info("Wordsearch Recognition Evaluation Completed");

            return (double)numCorrect / images.Count;
        }

        //Check if some candidate list of points is a wordsearch in a marked up image
        private static bool IsWordsearch(List<IntPoint> candidate, Image image)
        {
            foreach (WordsearchImage wordsearchImage in image.WordsearchImages)
            {
                IntPoint[] wordsearchPoints = wordsearchImage.Coordinates;

                //Does the candidate match this wordsearch (allowing for some margin of error so the points needn't be pixel perfect)

                //Margin for error should be an average cell dimension for a wordsearch grid in a circle around each corner of the wordsearch
                double widthTop = Geometry.EuclideanDistance(wordsearchImage.TopLeft, wordsearchImage.TopRight);
                double widthBottom = Geometry.EuclideanDistance(wordsearchImage.BottomLeft, wordsearchImage.BottomRight);
                double avgWidth = (widthTop + widthBottom) / 2;
                double avgCellWidth = avgWidth / wordsearchImage.Cols;

                double heightLeft = Geometry.EuclideanDistance(wordsearchImage.TopLeft, wordsearchImage.BottomLeft);
                double heightRight = Geometry.EuclideanDistance(wordsearchImage.TopRight, wordsearchImage.BottomRight);
                double avgHeight = (heightLeft + heightRight) / 2;
                double avgCellHeight = avgHeight / wordsearchImage.Rows;


                double errorMarginRadius = (avgCellWidth + avgCellHeight) / 2;

                //CandidatePoint => WordsearchPoint
                List<List<int>> matchedPoints = new List<List<int>>(candidate.Count);
                for (int i = 0; i < candidate.Count; i++)
                {
                    matchedPoints.Add(new List<int>());
                }

                //For each candidate point, attempt to match each actual wordsearch point, storing the matches
                bool brokeEarly = false;
                for (int i = 0; i < candidate.Count; i++)
                {
                    for(int j = 0; j < wordsearchPoints.Length; j++)
                    {
                        IntPoint candidatePoint = candidate[i];
                        IntPoint wordsearchPoint = wordsearchPoints[j];

                        double pointDistance = Geometry.EuclideanDistance(candidatePoint, wordsearchPoint);

                        if(pointDistance <= errorMarginRadius)
                        {
                            matchedPoints[i].Add(j);
                        }
                    }

                    //If there were no wordsearch points that could be matched to this candidate point, fail for this wordsearch
                    if(matchedPoints[i].Count == 0)
                    {
                        brokeEarly = true;
                        break;
                    }
                }

                //If we already know for sure that this isn't a match, continue to the next wordsearch 
                if(brokeEarly)
                {
                    continue;
                }

                //Each point matched to another, now to check that there is a way for each point to mapped onto a distinct point in the other set
                List<List<int>> pointMatchCandidates = new List<List<int>>();

                //Initialise the search with the first possible matches
                foreach(int n in matchedPoints[0])
                {
                    List<int> li = new List<int>();
                    li.Add(n);

                    pointMatchCandidates.Add(li);
                }

                for(int i = 1; i < matchedPoints.Count; i++)
                {
                    List<List<int>> newCandidates = new List<List<int>>();
                    foreach(int wordsearchPoint in matchedPoints[i])
                    {
                        for(int j = 0; j < pointMatchCandidates.Count; j++)
                        {
                            //If this branch of the tree doesn't already contain this point, add it as it is a possible next branch to a solution
                            if(!pointMatchCandidates[j].Contains(wordsearchPoint))
                            {
                                List<int> copied = copy(pointMatchCandidates[j]);
                                copied.Add(wordsearchPoint);
                                newCandidates.Add(copied);
                            }
                        }
                    }
                    //Update the list of candidates
                    pointMatchCandidates = newCandidates;
                }

                //If we are left with any solutions, then the candidate matches the wordsearch
                if(pointMatchCandidates.Count > 0)
                {
                    return true;
                }
            }
            //Didn't match any wordsearches
            return false;
        }

        private static List<int> copy(List<int> list)
        {
            List<int> toRet = new List<int>(list.Count);
            foreach(int n in list)
            {
                toRet.Add(n);
            }
            return toRet;
        }
    }
}
