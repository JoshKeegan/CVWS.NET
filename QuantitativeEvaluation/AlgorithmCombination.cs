/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Quantitative Evaluation
 * Algorithm Combination class
 * By Josh Keegan 19/05/2014
 * Last Edit 10/06/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using libCVWS.BaseObjectExtensions;
using ImageMarkup;
using QuantitativeEvaluation.Evaluators;
using libCVWS;
using libCVWS.ClassifierInterfacing;
using libCVWS.ClassifierInterfacing.FeatureExtraction;
using libCVWS.ImageAnalysis.WordsearchDetection;
using libCVWS.ImageAnalysis.WordsearchRotation;
using libCVWS.ImageAnalysis.WordsearchSegmentation;
using libCVWS.ImageAnalysis.WordsearchSegmentation.VariedRowColSize;
using libCVWS.Imaging;
using libCVWS.WordsearchSolver;

namespace QuantitativeEvaluation
{
    internal class AlgorithmCombination
    {
        //Public variables
        public SegmentationAlgorithm DetectionSegmentationAlgorithm { get; private set; }
        public bool DetectionSegmentationRemoveSmallRowsAndCols { get; private set; }
        public SegmentationAlgorithm SegmentationAlgorithm { get; private set; }
        public bool SegmentationRemoveSmallRowsAndCols { get; private set; }
        public EvaluateFullSystem.SegmentationMethod SegmentationMethod { get; private set; }
        public Classifier ProbabilisticRotationCorrectionClassifier { get; private set; }
        public Classifier Classifier { get; private set; }
        public Solver WordsearchSolver { get; private set; }

        //Constructors
        public AlgorithmCombination(SegmentationAlgorithm detectionSegmentationAlgorithm,
            bool detectionSegmentationRemoveSmallRowsAndCols, SegmentationAlgorithm segmentationAlgorithm, 
            bool segmentationRemoveSmallRowsAndCols, EvaluateFullSystem.SegmentationMethod segmentationMethod,
            Classifier probabilisticRotationCorrectionClassifier, Classifier classifier,
            Solver wordsearchSolver)
        {
            this.DetectionSegmentationAlgorithm = detectionSegmentationAlgorithm;
            this.DetectionSegmentationRemoveSmallRowsAndCols = detectionSegmentationRemoveSmallRowsAndCols;
            this.SegmentationAlgorithm = segmentationAlgorithm;
            this.SegmentationRemoveSmallRowsAndCols = segmentationRemoveSmallRowsAndCols;
            this.SegmentationMethod = segmentationMethod;
            this.ProbabilisticRotationCorrectionClassifier = probabilisticRotationCorrectionClassifier;
            this.Classifier = classifier;
            this.WordsearchSolver = wordsearchSolver;
        }
    }
}
