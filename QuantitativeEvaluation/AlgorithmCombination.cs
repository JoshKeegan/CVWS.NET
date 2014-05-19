﻿/*
 * Dissertation CV Wordsearch Solver
 * Quantitative Evaluation
 * Algorithm Combination class
 * By Josh Keegan 19/05/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BaseObjectExtensions;
using ImageMarkup;
using QuantitativeEvaluation.Evaluators;
using SharedHelpers;
using SharedHelpers.ClassifierInterfacing;
using SharedHelpers.ClassifierInterfacing.FeatureExtraction;
using SharedHelpers.ImageAnalysis.WordsearchDetection;
using SharedHelpers.ImageAnalysis.WordsearchRotation;
using SharedHelpers.ImageAnalysis.WordsearchSegmentation;
using SharedHelpers.ImageAnalysis.WordsearchSegmentation.VariedRowColSize;
using SharedHelpers.Imaging;
using SharedHelpers.WordsearchSolver;

namespace QuantitativeEvaluation
{
    internal class AlgorithmCombination
    {
        //Public variables
        public SegmentationAlgorithm DetectionSegmentationAlgorithm { get; private set; }
        public SegmentationAlgorithm SegmentationAlgorithm { get; private set; }
        public EvaluateFullSystem.SegmentationMethod SegmentationMethod { get; private set; }
        public Classifier ProbabilisticRotationCorrectionClassifier { get; private set; }
        public Classifier Classifier { get; private set; }
        public Solver WordsearchSolver { get; private set; }

        //Constructors
        public AlgorithmCombination(SegmentationAlgorithm detectionSegmentationAlgorithm,
            SegmentationAlgorithm segmentationAlgorithm, EvaluateFullSystem.SegmentationMethod segmentationMethod,
            Classifier probabilisticRotationCorrectionClassifier, Classifier classifier,
            Solver wordsearchSolver)
        {
            this.DetectionSegmentationAlgorithm = detectionSegmentationAlgorithm;
            this.SegmentationAlgorithm = segmentationAlgorithm;
            this.SegmentationMethod = segmentationMethod;
            this.ProbabilisticRotationCorrectionClassifier = probabilisticRotationCorrectionClassifier;
            this.Classifier = classifier;
            this.WordsearchSolver = wordsearchSolver;
        }
    }
}
