/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Quantitative Evaluation
 * Algorithm Combination class
 * Authors:
 *  Josh Keegan 19/05/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using libCVWS.ClassifierInterfacing;
using libCVWS.ImageAnalysis.WordsearchDetection;
using libCVWS.ImageAnalysis.WordsearchSegmentation;
using libCVWS.WordsearchSolver;

namespace QuantitativeEvaluation
{
    internal class AlgorithmCombination
    {
        //Public variables
        public readonly IWordsearchCandidatesDetectionAlgorithm CandidatesDetectionAlgorithm;
        public readonly IWordsearchCandidateVettingAlgorithm CandidateVettingAlgorithm;
        public readonly SegmentationAlgorithm SegmentationAlgorithm;
        public readonly bool SegmentationRemoveSmallRowsAndCols;
        public readonly EvaluateFullSystem.SegmentationMethod SegmentationMethod;
        public readonly Classifier ProbabilisticRotationCorrectionClassifier;
        public readonly Classifier Classifier;
        public readonly Solver WordsearchSolver;

        //Constructors
        public AlgorithmCombination(IWordsearchCandidatesDetectionAlgorithm candidatesDetectionAlgorithm,
            IWordsearchCandidateVettingAlgorithm candidateVettingAlgorithm, SegmentationAlgorithm segmentationAlgorithm,
            bool segmentationRemoveSmallRowsAndCols, EvaluateFullSystem.SegmentationMethod segmentationMethod,
            Classifier probabilisticRotationCorrectionClassifier, Classifier classifier, Solver wordsearchSolver)
        {
            CandidatesDetectionAlgorithm = candidatesDetectionAlgorithm;
            CandidateVettingAlgorithm = candidateVettingAlgorithm;
            SegmentationAlgorithm = segmentationAlgorithm;
            SegmentationRemoveSmallRowsAndCols = segmentationRemoveSmallRowsAndCols;
            SegmentationMethod = segmentationMethod;
            ProbabilisticRotationCorrectionClassifier = probabilisticRotationCorrectionClassifier;
            Classifier = classifier;
            WordsearchSolver = wordsearchSolver;
        }
    }
}
