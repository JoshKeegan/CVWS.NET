/*
 * CVWS.NET
 * Quantitative Evaluation
 * Evaluate Wordsearch Candidates Detection
 * Authors:
 *  Josh Keegan 18/04/2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageMarkup;

using KLog;

using libCVWS.ImageAnalysis.WordsearchDetection;

namespace QuantitativeEvaluation
{
    internal static class EvaluateWordsearchCandidatesDetection
    {
        #region Constants

        internal static readonly Dictionary<string, IWordsearchCandidatesDetectionAlgorithm>
            CANDIDATES_DETECTION_ALGORITHMS = new Dictionary<string, IWordsearchCandidatesDetectionAlgorithm>()
            {
                { "Quadrilateral Detection", new WordsearchCandidateDetectionQuadrilateralRecognition() },
                { "Lattice Construction (default)", new WordsearchCandidateDetectionByLatticeConstruction() },
                { "Lattice Construction (without lattice vetting by num elements)", new WordsearchCandidateDetectionByLatticeConstruction() },
                { "Lattice Construction (without removing very noisy blobs)", new WordsearchCandidateDetectionByLatticeConstruction(new LatticeConstructionSettings() { RemoveVeryNoisyBlobs = false }) },
                { "Lattice Construction (with angle)", new WordsearchCandidateDetectionByLatticeConstruction(new LatticeConstructionSettings() { ElementConnectionVettingByAngle = true }) },
                { "Lattice Construction (with check for proposed connection already being saturated)", new WordsearchCandidateDetectionByLatticeConstruction(new LatticeConstructionSettings() { ElementConnectionVettingByAngle = true }) },
            };

        #endregion

        /// <summary>
        /// Evaluate whether each algorithm detects the wordsearch in each image.
        /// No measure of precision & recall, just looking for whether we get a true positive for each wordsearch in each image
        /// </summary>
        public static Dictionary<string, double> EvaluateDetectsWordsearch(List<Image> images)
        {
            // Register an interest in the bitmaps of all the images (so that they remain in memory throughout)
            foreach (Image image in images)
            {
                image.RegisterInterestInBitmap();
            }

            Dictionary<string, double> scores = new Dictionary<string, double>();

            // Evaluate each candidates detection algorithm
            foreach (KeyValuePair<string, IWordsearchCandidatesDetectionAlgorithm> kvp in
                CANDIDATES_DETECTION_ALGORITHMS)
            {
                string name = kvp.Key;
                IWordsearchCandidatesDetectionAlgorithm algorithm = kvp.Value;

                DefaultLog.Debug("Evaluating wordsearch candidates detection algorithm: {0}", name);

                double score = evaluateAlgorithm(images, algorithm);
                scores.Add(name, score);
            }

            // Deregsiter an interest in all of the images
            foreach (Image image in images)
            {
                image.DeregisterInterestInBitmap();
            }
            return scores;
        }

        private static double evaluateAlgorithm(List<Image> images, IWordsearchCandidatesDetectionAlgorithm algorithm)
        {
            int numCorrect = 0;
            // Keep track of how many wordsearches we were looking for too, as we could have been looking for multiple in some images
            int numWordsearches = 0; 

            // Test this algorithm on each image
            foreach (Image image in images)
            {
                // Update the number of wordsearches we've looked for by how many are in this image
                numWordsearches += image.WordsearchImages.Length;

                // Detect candidates in this image
                WordsearchCandidate[] candidates = algorithm.FindCandidates(image.Bitmap);

                // See if we found each wordsearch image
                HashSet<int> usedCandidateIndices = new HashSet<int>();
                foreach (WordsearchImage wordsearchImage in image.WordsearchImages)
                {
                    for (int i = 0; i < candidates.Length; i++)
                    {
                        // If we've already used this candidate to match another wordsearch image, we can't use it again
                        if (usedCandidateIndices.Contains(i))
                        {
                            continue;
                        }

                        if (EvaluateOverallWordsearchDetection.IsWordsearch(candidates[i].OriginalImageCoords,
                            wordsearchImage))
                        {
                            usedCandidateIndices.Add(i);
                            numCorrect++;
                            break;
                        }
                    }
                }

                // Clean up
                foreach (WordsearchCandidate c in candidates)
                {
                    c.Dispose();
                }
            }

            return (double) numCorrect / numWordsearches;
        }
    }
}
