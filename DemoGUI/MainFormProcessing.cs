﻿/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Demo GUI
 * Main Form (partial). Code to do all of the Image Processing
 * Authors:
 *  Josh Keegan 12/05/2014
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AForge;
using AForge.Imaging.Filters;

using libCVWS;
using libCVWS.BaseObjectExtensions;
using libCVWS.ClassifierInterfacing;
using libCVWS.ClassifierInterfacing.FeatureExtraction;
using libCVWS.ImageAnalysis.WordsearchDetection;
using libCVWS.ImageAnalysis.WordsearchRotation;
using libCVWS.ImageAnalysis.WordsearchSegmentation;
using libCVWS.ImageAnalysis.WordsearchSegmentation.VariedRowColSize;
using libCVWS.Imaging;
using libCVWS.WordsearchSolver;

using DemoGUI.Exceptions;

namespace DemoGUI
{
    public partial class MainForm : Form
    {
        private enum ProcessingStage
        {
            WordsearchCandidateDetection,
            WordsearchCandidateVetting,
            WordsearchSegmentation,
            SegmentationPostProcessing,
            RotationCorrection,
            CharacterImageExtraction,
            FeatureExtractionAndClassification,
            WordsearchSolver,
            All
        };

        private enum SegmentationPostProcessing
        {
            None,
            RemoveSmallRowsAndCols
        };

        //Constants
        private static readonly Dictionary<ProcessingStage, string> PROCESSING_STAGE_NAMES = new Dictionary<ProcessingStage, string>()
        {
            //These names should be exactly the same as the ones in checkListProcessingStages
            { ProcessingStage.WordsearchCandidateDetection, "Wordsearch Candidate Detection" },
            { ProcessingStage.WordsearchCandidateVetting, "Wordsearch Candidate Vetting" },
            { ProcessingStage.SegmentationPostProcessing, "Segmentation Post-Processing" },
            { ProcessingStage.WordsearchSegmentation, "Wordsearch Segmentation" },
            { ProcessingStage.RotationCorrection, "Rotation Correction" },
            { ProcessingStage.CharacterImageExtraction, "Character Image Extraction" },
            { ProcessingStage.FeatureExtractionAndClassification, "Feature Extraction & Classification" },
            { ProcessingStage.WordsearchSolver, "Wordsearch Solver" },
            { ProcessingStage.All, "All Processing Stages" }
        };

        private static readonly Dictionary<string, SegmentationPostProcessing> SEGMENTATION_POST_PROCESSING_ALGORITHMS = new Dictionary<string, SegmentationPostProcessing>()
        {
            { "None", SegmentationPostProcessing.None },
            { "Remove Erroneously Small Rows & Cols", SegmentationPostProcessing.RemoveSmallRowsAndCols }
        };

        private const string TRAINED_ALGORITHMS_PATH = "TrainedAlgorithms/";
        private const string TRAINED_FEATURE_EXTRACTORS_PATH = TRAINED_ALGORITHMS_PATH + "FeatureExtractors/";
        private const string TRAINED_CLASSIFIERS_PATH = TRAINED_ALGORITHMS_PATH + "Classifiers/";
        private const string TRAINED_CLASSIFIER_FILE_EXTENSION = ".classifier";
        private const string TRAINED_FEATURE_EXTRACTOR_FILE_EXTENSION = ".featureExtraction";

        private static readonly Dictionary<string, IWordsearchCandidatesDetectionAlgorithm>
            WORDSEARCH_CANDIDATES_DETECTION_ALGORITHMS =
                new Dictionary<string, IWordsearchCandidatesDetectionAlgorithm>()
                {
                    {"Quadrilateral Recognition", new WordsearchCandidateDetectionQuadrilateralRecognition()}
                };

        private static readonly Dictionary<string, IWordsearchCandidateVettingAlgorithm>
            WORDSEARCH_CANDIDATE_VETTING_ALGORITHMS = new Dictionary<string, IWordsearchCandidateVettingAlgorithm>()
            {
                {
                    "Segmentation: Blob Recognition",
                    new WordsearchCandidateVettingBySegmentation(new SegmentByBlobRecognition())
                },
                {
                    "Segmentation: Histogram Single Threshold",
                    new WordsearchCandidateVettingBySegmentation(new SegmentByHistogramThresholdDarkPixels())
                },
                {
                    "Segmentation: Histogram Two Thresholds from Percentile Rank",
                    new WordsearchCandidateVettingBySegmentation(
                        new SegmentByHistogramThresholdPercentileRankTwoThresholds())
                },
                {
                    "Segmentation: Mean Dark Pixels",
                    new WordsearchCandidateVettingBySegmentation(new SegmentByMeanDarkPixels())
                },
                {
                    "Segmentation: Median Dark Pixels",
                    new WordsearchCandidateVettingBySegmentation(new SegmentByMedianDarkPixels())
                },
                {
                    "Segmentation: Percentile Two Thresholds",
                    new WordsearchCandidateVettingBySegmentation(new SegmentByPercentileTwoThresholds())
                },
                {
                    "Segmentation: Preselected Dark Pixel Threshold",
                    new WordsearchCandidateVettingBySegmentation(new SegmentByThresholdDarkPixels())
                }
            };

        private static readonly Dictionary<string, SegmentationAlgorithm> WORDSEARCH_SEGMENTATION_ALGORITHMS = new Dictionary<string, SegmentationAlgorithm>()
        {
            { "Blob Recognition", new SegmentByBlobRecognition() },
            { "Histogram Single Threshold", new SegmentByHistogramThresholdDarkPixels() },
            { "Histogram Two Thresholds from Percentile Rank", new SegmentByHistogramThresholdPercentileRankTwoThresholds() },
            { "Mean Dark Pixels", new SegmentByMeanDarkPixels() },
            { "Median Dark Pixels", new SegmentByMedianDarkPixels() },
            { "Percentile Two Thresholds", new SegmentByPercentileTwoThresholds() },
            { "Preselected Dark Pixel Threshold", new SegmentByThresholdDarkPixels() }
        };

        private static readonly Dictionary<string, Type> FEATURE_EXTRACTORS = new Dictionary<string,Type>()
        {
            { "Discrete Cosine Transform (DCT)", typeof(FeatureExtractionDCT) },
            //{ "Linear Discriminant Analysis (LDA)", typeof(FeatureExtractionLDA) }, //TODO: Implement serialization on LDA if this is to be included in the GUI
            { "Principal Component Analysis (PCA) All Features", typeof(FeatureExtractionPCA) },
            { "Principal Component Analysis (PCA) Top 20 Features", typeof(FeatureExtractionPCA) },
            { "Raw Pixel Values", typeof(FeatureExtractionPixelValues) }
        };

        private static readonly Dictionary<string, Type> CLASSIFIERS = new Dictionary<string,Type>()
        {
            { "Single Layer Neural Network", typeof(AForgeActivationNeuralNetClassifier) }
        };

        private static readonly Type DEFAULT_WORDSEARCH_CANDIDATE_DETECTION = typeof(WordsearchCandidateDetectionQuadrilateralRecognition);
        private static readonly IWordsearchCandidateVettingAlgorithm DEFAULT_WORDSEARCH_CANDIDATE_VETTING = new WordsearchCandidateVettingBySegmentation(new SegmentByMeanDarkPixels());
        private static readonly Type DEFAULT_WORDSEARCH_SEGMENTATION = typeof(SegmentByBlobRecognition);
        private const SegmentationPostProcessing DEFAULT_SEGMENTATION_POST_PROCESSING = SegmentationPostProcessing.None;
        private const string DEFAULT_ROTATION_CORRECTION_FEATURE_EXTRACTION = "Principal Component Analysis (PCA) All Features"; //Feature Extraction as a stirng because there can be more than one opention per type
        private static readonly Type DEFAULT_ROTATION_CORRECTION_CLASSIFICATION = typeof(AForgeActivationNeuralNetClassifier);
        private const string DEFAULT_FEATURE_EXTRACTION = "Principal Component Analysis (PCA) All Features";
        private static readonly Type DEFAULT_CLASSIFICATION = typeof(AForgeActivationNeuralNetClassifier);

        private static readonly Dictionary<string, string> TRAINED_FEATURE_EXTRACTOR_FILE_NAMES = new Dictionary<string, string>()
        {
            { "Discrete Cosine Transform (DCT)", "dct" },
            { "Principal Component Analysis (PCA) All Features", "pcaAllFeatures" },
            { "Principal Component Analysis (PCA) Top 20 Features", "pcaTopFeatures" },
            { "Raw Pixel Values", "rawPixelValues" }
        };

        private static readonly Dictionary<string, string> TRAINED_CLASSIFIER_FILE_NAMES = new Dictionary<string, string>()
        {
            { "Single Layer Neural Network", "neuralNetwork_singleLayer" }
        };

        private static readonly Color DRAW_WORDS_COLOUR = Color.Blue;
        private static readonly Color DRAW_SEGMENTATION_COLOUR = Color.Red;

        //Variables
        private Dictionary<ProcessingStage, Stopwatch> processingStageStopwatches;

        //Helper method to do all of the processing to solve a wordsearch
        private void doProcessing()
        {
            /*
             * Get all of the selected Algorithms to be used for the processing
             */
            log("Loading Selected Algorithms . . .");
            // Wordsearch Candidate Detection Algorithm
            IWordsearchCandidatesDetectionAlgorithm wordsearchCandidateDetectionAlgorithm =
                getSelectedWordsearchCandidatesDetectionAlgorithm(candidateSelectionToolStripMenuItem);

            // Wordsearch Candidate Vetting Algorithm
            // Get the 
            IWordsearchCandidateVettingAlgorithm wordsearchCandidateVettingAlgorithm =
                getSelectedWordsearchCandidateVettingAlgorithm(candidateRankingToolStripMenuItem);

            //Wordsearch Segmentation Algorithm
            SegmentationAlgorithm wordsearchSegmentationAlgorithm =
                getSelectedSegmentationAlgorithm(wordsearchSegmentationToolStripMenuItem);

            //Segmentation Post Processor
            SegmentationPostProcessing segmentationPostProcessor = getSelectedSegmentationPostProcessor();

            //Rotation Correction Classifier object (contains both feature extractor & classifier)
            Classifier rotationCorrectionClassifier = getSelectedClassifier(rotationCorrectionFeatureExtractionToolStripMenuItem,
                rotationCorrectionClassificationToolStripMenuItem);

            //Character Image Extraction: Do we normalise the input (with whitespace) image dimensions before finding the biggest blob?
            //  Note that this is ignored if there is equal segmentation spacing (where resizing will always be used as this is how 
            //  the classifier was trained)
            bool charImageExtractionWithWhitespaceNormaliseDimensions = characterImageExtractionInputDimensionsNormalisedToolStripMenuItem.Checked;

            //Classifier object used for actual classification (contains both feature extractor & classifier)
            Classifier classifier = getSelectedClassifier(featureExtractionToolStripMenuItem, classificationToolStripMenuItem);

            //Wordsearch Solver Algorithm
            Solver wordsearchSolver = getSelectedWordsearchSolver();

            log("Selected Algorithms Loaded Successfully!");

            /*
             * Start Processing
             */
            //Start the timer for all processing
            setProcessingStageState(ProcessingStage.All, CheckState.Indeterminate);

            //Get the input image
            Bitmap img = currentBitmap;

            //Get the words to find
            string[] wordsToFind = getWordsToFind();

            /*
             * Wordsearch Candidate Detection
             */
            // Show that we're starting Wordsearch Candidate Detection
            setProcessingStageState(ProcessingStage.WordsearchCandidateDetection, CheckState.Indeterminate);

            // Detect the candidates
            WordsearchCandidate[] wordsearchCandidates = wordsearchCandidateDetectionAlgorithm.FindCandidates(img);

            log(String.Format("Found {0} wordsearch candidates", wordsearchCandidates.Length));

            // If we failed to find anything remotely resembling a worssearch, fail now
            if (wordsearchCandidates.Length == 0)
            {
                throw new ImageProcessingException("Wordsearch Detection could not find anything . . .");
            }

            log(DrawShapes.Polygons(img, wordsearchCandidates.Select(candidate => candidate.OriginalImageCoords)),
                "Wordsearch Candidates");

            // Mark Wordsearch Candidate Detection as completed
            setProcessingStageState(ProcessingStage.WordsearchCandidateDetection, CheckState.Checked);

            /*
             * Wordsearch Candidate Vetting
             */
             // Show that we're starting Wordsearch Candidate Vetting
            setProcessingStageState(ProcessingStage.WordsearchCandidateVetting, CheckState.Indeterminate);

            Bitmap wordsearchImage = wordsearchCandidates
                .OrderByDescending(candidate => wordsearchCandidateVettingAlgorithm.Score(candidate))
                .First()
                .Bitmap;

            log(wordsearchImage, "Best Wordsearch Image");

            // Clean up wordsearch candidates
            foreach (WordsearchCandidate c in wordsearchCandidates)
            {
                // Don't dispose of the selected wordsearchImage, as we need to use that later
                if (c.Bitmap != wordsearchImage)
                {
                    c.Dispose();
                }
            }

            // Mark Wordsearch Candidate Vetting as completed
            setProcessingStageState(ProcessingStage.WordsearchCandidateVetting, CheckState.Checked);

            /*
             * Wordsearch Segmentation
             */
            //Show that we're starting Wordsearch Segmentation
            setProcessingStageState(ProcessingStage.WordsearchSegmentation, CheckState.Indeterminate);

            Segmentation segmentation = wordsearchSegmentationAlgorithm.Segment(wordsearchImage);
            
            //Log the Segmentation (visually)
            log(DrawGrid.Segmentation(wordsearchImage, segmentation, DRAW_SEGMENTATION_COLOUR), "Segmentation");

            //Mark Wordsearch Segmentation as completed
            setProcessingStageState(ProcessingStage.WordsearchSegmentation, CheckState.Checked);

            /*
             * Wordsearch Segmentation Post-Processing
             */

            //Show that we're starting Segmentation Post-Processing
            setProcessingStageState(ProcessingStage.SegmentationPostProcessing, CheckState.Indeterminate);

            switch(segmentationPostProcessor)
            {
                case SegmentationPostProcessing.RemoveSmallRowsAndCols:
                    segmentation = segmentation.RemoveSmallRowsAndCols();
                    break;
            }

            //If any post-processing was done, log the segmentation (visually)
            if(segmentationPostProcessor != SegmentationPostProcessing.None)
            {
                log(DrawGrid.Segmentation(wordsearchImage, segmentation, DRAW_SEGMENTATION_COLOUR), "Post-Processed Segmentation");
            }

            //Mark Segmentation Post-Processing as complete
            setProcessingStageState(ProcessingStage.SegmentationPostProcessing, CheckState.Checked);

            /*
             * Wordsearch Rotation Correction
             */
            //Show that we're starting Rotation Correction
            setProcessingStageState(ProcessingStage.RotationCorrection, CheckState.Indeterminate);

            WordsearchRotation originalRotation;

            //If the rows & cols in the Segmentation are all equally spaced apart, optimise by using the number of rows & cols
            if(segmentation.IsEquallySpaced)
            {
                originalRotation = new WordsearchRotation(wordsearchImage, segmentation.NumRows, segmentation.NumCols);
            }
            else //Otherwise the Segmentation has varied sized row/col width
            {
                originalRotation = new WordsearchRotation(wordsearchImage, segmentation);
            }

            WordsearchRotation rotatedWordsearch = WordsearchRotationCorrection.CorrectOrientation(
                originalRotation, rotationCorrectionClassifier);

            Bitmap rotatedImage = rotatedWordsearch.Bitmap;

            //If the wordsearch has been rotated
            if(rotatedImage != wordsearchImage)
            {
                //Update the segmentation

                //If the wordsearch rotation won't have passed a segmentation
                if(segmentation.IsEquallySpaced)
                {
                    //Make a new fixed width segmentation from the wordsearchRotation
                    segmentation = new Segmentation(rotatedWordsearch.Rows, rotatedWordsearch.Cols,
                        rotatedImage.Width, rotatedImage.Height);
                }
                else //Otherwise the WordsearchRotation will have been working with a Segmentation
                {
                    //Use the rotated Segmentation object
                    segmentation = rotatedWordsearch.Segmentation;
                }
            }

            //Log the rotated image
            log(rotatedImage, "Rotated Wordsearch");
            log(DrawGrid.Segmentation(rotatedImage, segmentation, DRAW_SEGMENTATION_COLOUR), "Rotated Segmentation");

            //Mark Rotation Correction as completed
            setProcessingStageState(ProcessingStage.RotationCorrection, CheckState.Checked);

            /*
             * Character Image Extraction
             */
            //Show that we're starting Character Image Extraction
            setProcessingStageState(ProcessingStage.CharacterImageExtraction, CheckState.Indeterminate);

            //Split the image up using the Segmentation
            Bitmap[,] rawCharImgs = null;

            //If we're using equally spaced Segmentation
            if(segmentation.IsEquallySpaced)
            {
                //Resize the image first, so that the characters returned when you split the image will already be the correct size
                ResizeBicubic resize = new ResizeBicubic(Constants.CHAR_WITH_WHITESPACE_WIDTH * segmentation.NumCols,
                    Constants.CHAR_WITH_WHITESPACE_HEIGHT * segmentation.NumRows);
                Bitmap resizedImage = resize.Apply(rotatedImage);

                //Split the image using a standard grid based on the number of rows & cols 
                //  (which is correct because of the equally spaced Segmentation)
                rawCharImgs = SplitImage.Grid(resizedImage, segmentation.NumRows, segmentation.NumCols);

                //Resized image no longer required
                resizedImage.Dispose();
            }
            else //Otherwise there is varied spacing between characters
            {
                rawCharImgs = SplitImage.Segment(rotatedImage, segmentation);

                //If we're resizing the raw character images (with whitespace) to consistent dimensions
                if(charImageExtractionWithWhitespaceNormaliseDimensions)
                {
                    //Resize the raw char images so that they're all the same dimensions (gives results that are more consistent with how the 
                    //  classifier was trained: with equally spaced segmentation)
                    ResizeBicubic resize = new ResizeBicubic(Constants.CHAR_WITH_WHITESPACE_WIDTH, Constants.CHAR_WITH_WHITESPACE_HEIGHT);

                    for (int i = 0; i < rawCharImgs.GetLength(0); i++)
                    {
                        for (int j = 0; j < rawCharImgs.GetLength(1); j++)
                        {
                            //Only do the resize if it isn't already that size
                            if (rawCharImgs[i, j].Width != Constants.CHAR_WITH_WHITESPACE_WIDTH
                                || rawCharImgs[i, j].Height != Constants.CHAR_WITH_WHITESPACE_HEIGHT)
                            {
                                Bitmap orig = rawCharImgs[i, j];

                                rawCharImgs[i, j] = resize.Apply(orig);

                                //Remove the now unnecessary original/not resized image
                                orig.Dispose();
                            }
                        }
                    }
                }
            }

            //Log the raw character images (if we've resized them)
            if(segmentation.IsEquallySpaced || charImageExtractionWithWhitespaceNormaliseDimensions)
            {
                log(CombineImages.Grid(rawCharImgs), "Raw Character Images (all chars set to equal width & height)");
            }

            //Get the part of the image that actually contains the character (without any whitespace)
            Bitmap[,] charImgs = CharImgExtractor.ExtractAll(rawCharImgs);

            //Raw char img's are no longer required
            rawCharImgs.ToSingleDimension().DisposeAll();

            //Log the extracted character images
            log(CombineImages.Grid(charImgs), "Extracted Character Images");

            //Mark Character Image Extraction as completed
            setProcessingStageState(ProcessingStage.CharacterImageExtraction, CheckState.Checked);

            /*
             * Feature Extraction & Classification
             */
            //Show that we're starting Feature Extraction & Classification
            setProcessingStageState(ProcessingStage.FeatureExtractionAndClassification, CheckState.Indeterminate);

            double[][][] charProbabilities = classifier.Classify(charImgs);

            //Actual images of the characters are no longer required
            charImgs.ToSingleDimension().DisposeAll();

            //Log the wordsearch as classified
            char[,] classifiedChars = NeuralNetworkHelpers.GetMostLikelyChars(charProbabilities);
            log("Wordsearch as classified (character that was given the highest probability):");
            for (int i = 0; i < classifiedChars.GetLength(1); i++) //Rows
            {
                StringBuilder builder = new StringBuilder();

                //Cols
                for(int j = 0; j < classifiedChars.GetLength(0); j++)
                {
                    builder.Append(classifiedChars[j, i]);
                }

                log(builder.ToString());
            }

            //Mark Feature Extraction & Classification as completed
            setProcessingStageState(ProcessingStage.FeatureExtractionAndClassification, CheckState.Checked);

            /*
             * Solve Wordsearch
             */
            //Show that we're starting to Solve the Wordsearch
            setProcessingStageState(ProcessingStage.WordsearchSolver, CheckState.Indeterminate);

            Solution solution = wordsearchSolver.Solve(charProbabilities, wordsToFind);

            //Log the solution visually
            Bitmap bitmapSolution = DrawSolution.Solution(rotatedImage, segmentation, solution, DRAW_WORDS_COLOUR);
            log(bitmapSolution, "Solution");
            log(DrawGrid.Segmentation(bitmapSolution, segmentation, DRAW_SEGMENTATION_COLOUR), "Solution + Segmentation");

            //Log an Image for the solution to each word so that you can see where it thinks each word is
            foreach(KeyValuePair<string, WordPosition> kvp in solution)
            {
                string word = kvp.Key;
                WordPosition position = kvp.Value;

                //Draw this WordPosition onto the rotated image
                log(DrawSolution.WordPosition(rotatedImage, segmentation, position, DRAW_WORDS_COLOUR),
                    String.Format("Solution for Word: {0}", word));
            }

            //Mark the wordsearch as having been solved
            setProcessingStageState(ProcessingStage.WordsearchSolver, CheckState.Checked);

            //Log the time taken to complete all of the processing
            setProcessingStageState(ProcessingStage.All, CheckState.Checked);
        }

        //Get the words to find
        private string[] getWordsToFind()
        {
            //Check that something has actually been entered in the words text box
            if(txtWordsToFind.Text == defaultTxtWordsToFind || txtWordsToFind.Text == "")
            {
                throw new InvalidWordsException("You must enter some words to be found");
            }

            //Get the lines
            string[] lines = txtWordsToFind.Text.ToUpper() //All chars are handled in upper case
                .Replace("\r", "").Replace("\t", "").Replace(" ", "") //Ignore carriage returns, tabs or spaces
                .Replace("-", "").Replace("'", "") //Ignore punctuation (that could be in the words to find)
                .Split('\n');

            List<string> words = new List<string>(lines.Length);
            foreach(string line in lines)
            {
                //If there is a word on this line
                if(line != "")
                {
                    //Check that this word only contains the characters A-Z
                    foreach(char c in line)
                    {
                        if(c < 'A' || c > 'Z')
                        {
                            throw new InvalidWordsException(String.Format(
                                "Word \"{0}\" contains invalid character '{1}'. Only (A-Z) are allowed", line, c));
                        }
                    }

                    //If we don't already have this word, add it to the list
                    if(!words.Contains(line))
                    {
                        words.Add(line);
                    }
                }
            }
            return words.ToArray();
        }

        //Get the selected Wordsearch Solver
        private Solver getSelectedWordsearchSolver()
        {
            //Check if each menu item is checked in turn
            if(nonProbabilisticWordsearchSolverToolStripMenuItem.Checked)
            {
                return new SolverNonProbabilistic();
            }
            else if(probabilisticWordsearchSolverToolStripMenuItem.Checked)
            {
                return new SolverProbabilistic();
            }
            else if(probabilisticPreventCharacterDiscrepanciesToolStripMenuItem.Checked)
            {
                return new SolverProbabilisticPreventCharacterDiscrepancies();
            }
            else //Otherwise no Menu Item was checked (or we didn't test for the checked one)
            {
                throw new InvalidMenuItemSelectionException();
            }
        }

        //Get the selected Segmentation Post-Processing method
        private SegmentationPostProcessing getSelectedSegmentationPostProcessor()
        {
            //Get the name of the Segmentation Post Processing Algorithm
            string segmentationPostProcessorName = null;
            foreach(ToolStripMenuItem menuItem in segmentationPostProcessingToolStripMenuItem.DropDownItems)
            {
                if(menuItem.Checked)
                {
                    segmentationPostProcessorName = menuItem.Text;
                    break;
                }
            }

            return SEGMENTATION_POST_PROCESSING_ALGORITHMS[segmentationPostProcessorName];
        }

        //Get the Classifier object. This encapsulates the classifier itself & the Feature Extractor,
        //  so we pass two parent menu items, one for each
        private Classifier getSelectedClassifier(ToolStripMenuItem parentFeatureExtractor, ToolStripMenuItem parentClassifier)
        {
            //There is only one classifier at the minute, so just always use it
            string classifierName = "Single Layer Neural Network";

            //Get the Feature Extraction Algorithm
            FeatureExtractionAlgorithm featureExtractor = getSelectedFeatureExtractionAlgorithm(parentFeatureExtractor);

            //Work out where to load the Neural Network from. There are different trained versions of the network
            //  for each different Feature Extraction Algorithm

            //Get the name of the Feature Extractor (don't need to handle case of it not being found as the above call to
            //  getSelectedFeatureExtractionAlgorithm will have already thrown an Exception if no menu item was checked)
            string featureExtractorName = null;
            foreach(ToolStripMenuItem menuItem in parentFeatureExtractor.DropDownItems)
            {
                if(menuItem.Checked)
                {
                    featureExtractorName = menuItem.Text;
                    break;
                }
            }

            //Get the file names for the Feature Extractor & Classifier
            string featureExtractorFileName = TRAINED_FEATURE_EXTRACTOR_FILE_NAMES[featureExtractorName];
            string classifierFileName = TRAINED_CLASSIFIER_FILE_NAMES[classifierName];

            //Combine the feature extractor & classifier file names into one which is what the classifier to be loaded is actually called
            string fileName = classifierFileName + "_" + featureExtractorFileName;

            //Construct the full path for the file
            string filePath = TRAINED_CLASSIFIERS_PATH + fileName + TRAINED_CLASSIFIER_FILE_EXTENSION;

            //Construct the Classifier object (which will load the classifier from the specified file)
            Classifier classifier = new AForgeActivationNeuralNetClassifier(featureExtractor, filePath);

            return classifier;
        }

        //Get the FeatureExtractionAlgorithm that is selected under the specified ToolStripMenuItem
        private FeatureExtractionAlgorithm getSelectedFeatureExtractionAlgorithm(ToolStripMenuItem parent)
        {
            //Find the checked MenuItem
            foreach(ToolStripMenuItem menuItem in parent.DropDownItems)
            {
                if(menuItem.Checked)
                {
                    string featureExtractorName = menuItem.Text;

                    Type featureExtractorType = FEATURE_EXTRACTORS[featureExtractorName];

                    FeatureExtractionAlgorithm featureExtractor;

                    //If the type of feature extractor is trainable (and will therefore need to have data loaded)
                    if(featureExtractorType.IsSubclassOf(typeof(TrainableFeatureExtractionAlgorithm)))
                    {
                        //Work out where this Feature Extractor is stored
                        string featureExtractorFileName = TRAINED_FEATURE_EXTRACTOR_FILE_NAMES[featureExtractorName];
                        string featureExtractorPath = TRAINED_FEATURE_EXTRACTORS_PATH + featureExtractorFileName + TRAINED_FEATURE_EXTRACTOR_FILE_EXTENSION;

                        //Load it
                        featureExtractor = TrainableFeatureExtractionAlgorithm.Load(featureExtractorPath);
                    }
                    else //Otherwise this feature extractor doesn't need to be trained, so we can just construct an object of this type
                    {
                        featureExtractor = (FeatureExtractionAlgorithm)Activator.CreateInstance(featureExtractorType);
                    }

                    return featureExtractor;
                }
            }

            //Throw Exception: couldn't find a checked MenuItem
            throw new InvalidMenuItemSelectionException();
        }

        private IWordsearchCandidatesDetectionAlgorithm getSelectedWordsearchCandidatesDetectionAlgorithm(
            ToolStripMenuItem parent)
        {
            // Find the checked MenuItem
            foreach (ToolStripMenuItem menuItem in parent.DropDownItems)
            {
                if (menuItem.Checked)
                {
                    return WORDSEARCH_CANDIDATES_DETECTION_ALGORITHMS[menuItem.Text];
                }
            }

            // If we get this far, we couldn't find a checked MenuItem
            throw new InvalidMenuItemSelectionException();
        }

        private IWordsearchCandidateVettingAlgorithm getSelectedWordsearchCandidateVettingAlgorithm(
            ToolStripMenuItem parent)
        {
            // Find the checked MenuItem
            foreach (ToolStripMenuItem menuItem in parent.DropDownItems)
            {
                if (menuItem.Checked)
                {
                    return WORDSEARCH_CANDIDATE_VETTING_ALGORITHMS[menuItem.Text];
                }
            }

            // If we get this far, we couldn't find a checked MenuItem
            throw new InvalidMenuItemSelectionException();
        }

        //Get the SegmentationAlgorithm that is selected under the specified ToolStripMenuItem
        private SegmentationAlgorithm getSelectedSegmentationAlgorithm(ToolStripMenuItem parent)
        {
            //Find the checked MenuItem
            foreach(ToolStripMenuItem menuItem in parent.DropDownItems)
            {
                if(menuItem.Checked)
                {
                    SegmentationAlgorithm segmentationAlgorithm = WORDSEARCH_SEGMENTATION_ALGORITHMS[menuItem.Text];
                    return segmentationAlgorithm;
                }
            }

            //Throw Exception: couldn't find a checked MenuItem
            throw new InvalidMenuItemSelectionException();
        }

        private void populateAlgorithmsMenu()
        {
            //Get a list of all of the menu items we start with and (may) need to be populated
            List<ToolStripMenuItem> baseMenuItems = getDescendentsOf(algorithmsToolStripMenuItem);

            /*
             * Populate Wordsearch Candidate Detection
             */
            IEnumerable<ToolStripMenuItem> wordsearchCandidateDetectionMenuItems =
                baseMenuItems.Where(menuItem => menuItem.Text == "Candidate Detection");
            // Add the Wordsearch Candidate Detection options to each Menu Item
            foreach (ToolStripMenuItem menuItem in wordsearchCandidateDetectionMenuItems)
            {
                foreach (string txt in WORDSEARCH_CANDIDATES_DETECTION_ALGORITHMS.Keys)
                {
                    // Make the tool strip item
                    menuItem.DropDownItems.Add(txt);
                }
            }

            /*
             * Populate Wordsearch Candidate Vetting
             */
            IEnumerable<ToolStripMenuItem> wordsearchCandidateVettingMenuItems =
                baseMenuItems.Where(menuItem => menuItem.Text == "Candidate Vetting");
            // Add the Wordsearch Candidate Vetting options to each Menu Item
            foreach (ToolStripMenuItem menuItem in wordsearchCandidateVettingMenuItems)
            {
                foreach (string txt in WORDSEARCH_CANDIDATE_VETTING_ALGORITHMS.Keys)
                {
                    // Make the toop strip item
                    menuItem.DropDownItems.Add(txt);
                }
            }

            /*
             * Populate Wordsearch Segmentation
             */
            //Find all of the Wordsearch Segmentation Menu Items
            IEnumerable<ToolStripMenuItem> wordsearchSegmentationMenuItems = baseMenuItems.Where(
                menuItem => menuItem.Text == "Wordsearch Segmentation");
            //Add the Wordsearch Segmentation options to each Menu Item
            foreach(ToolStripMenuItem menuItem in wordsearchSegmentationMenuItems)
            {
                foreach(string txt in WORDSEARCH_SEGMENTATION_ALGORITHMS.Keys)
                {
                    //Make the tool strip item
                    menuItem.DropDownItems.Add(txt);
                }
            }

            /*
             * Populate Segmentation Post-Processing
             */
            IEnumerable<ToolStripMenuItem> segmentationPostProcessingMenuItems = baseMenuItems.Where(
                menuItem => menuItem.Text == "Segmentation Post-Processing");
            //Add the Segmentation Post-Processing options to the Menu Item
            foreach(ToolStripMenuItem menuItem in segmentationPostProcessingMenuItems)
            {
                foreach(string txt in SEGMENTATION_POST_PROCESSING_ALGORITHMS.Keys)
                {
                    //Make the tool strip item
                    menuItem.DropDownItems.Add(txt);
                }
            }

            /*
             * Populate Feature Extraction
             */
            IEnumerable<ToolStripMenuItem> featureExtractionMenuItems = baseMenuItems.Where(
                menuItem => menuItem.Text == "Feature Extraction");
            //Add the Feature Extraction options to each Menu Item
            foreach (ToolStripMenuItem menuItem in featureExtractionMenuItems)
            {
                foreach (string txt in FEATURE_EXTRACTORS.Keys)
                {
                    //Make the tool strip item
                    menuItem.DropDownItems.Add(txt);
                }
            }

            /*
             * Populate Classification
             */
            IEnumerable<ToolStripMenuItem> classificationMenuItems = baseMenuItems.Where(
                menuItem => menuItem.Text == "Classification");
            //Add the Classification options to each Menu Item
            foreach(ToolStripMenuItem menuItem in classificationMenuItems)
            {
                foreach(string txt in CLASSIFIERS.Keys)
                {
                    //Make the tool strip item
                    menuItem.DropDownItems.Add(txt);
                }
            }

            /*
             * Set the default options to checked
             */
             // Wordsearch Candidate Detection
            foreach (ToolStripMenuItem menuItem in candidateSelectionToolStripMenuItem.DropDownItems)
            {
                // Get the type of the algorithm this menu item represents
                Type menuItemType = WORDSEARCH_CANDIDATES_DETECTION_ALGORITHMS[menuItem.Text].GetType();

                // If this menu item represents the type that is set as the default, check it
                if (menuItemType.Equals(DEFAULT_WORDSEARCH_CANDIDATE_DETECTION))
                {
                    menuItem.Checked = true;
                    break; // Found what we're looking for, look no further
                }
            }

            // Wordsearch Candidate Vetting
            foreach(ToolStripMenuItem menuItem in candidateRankingToolStripMenuItem.DropDownItems)
            {
                IWordsearchCandidateVettingAlgorithm vettingAlg =
                    WORDSEARCH_CANDIDATE_VETTING_ALGORITHMS[menuItem.Text];
                
                //If this menu item represents the type that is set as the default, check this menu item
                if(vettingAlg.Equals(DEFAULT_WORDSEARCH_CANDIDATE_VETTING))
                {
                    menuItem.Checked = true;
                    break; //Found what we were looking for, look no further
                }
            }

            //Wordsearch segmentation
            foreach (ToolStripMenuItem menuItem in wordsearchSegmentationToolStripMenuItem.DropDownItems)
            {
                //Get the type of the SegmentationAlgorithm object that this menu item represents
                Type menuItemType = WORDSEARCH_SEGMENTATION_ALGORITHMS[menuItem.Text].GetType();

                //If this menu item represents the type that is set as the default, check this menu item
                if (menuItemType.Equals(DEFAULT_WORDSEARCH_SEGMENTATION))
                {
                    menuItem.Checked = true;
                    break; //Found what we were looking for, look no further
                }
            }

            //Segmentation Post-Processing
            foreach(ToolStripMenuItem menuItem in segmentationPostProcessingToolStripMenuItem.DropDownItems)
            {
                //Get the Segmentation Post-Processing method that this menu item represents
                SegmentationPostProcessing menuItemSegmentationPostProcessing = SEGMENTATION_POST_PROCESSING_ALGORITHMS[menuItem.Text];

                //If this menu item represents the segmentation post-processing methods that is set as the default, check this menu item
                if(menuItemSegmentationPostProcessing == DEFAULT_SEGMENTATION_POST_PROCESSING)
                {
                    menuItem.Checked = true;
                    break; //Found what we were looking for, look no further
                }
            }

            //Rotation Correction Feature Extraction
            foreach(ToolStripMenuItem menuItem in rotationCorrectionFeatureExtractionToolStripMenuItem.DropDownItems)
            {
                //Get the type of the FeatureExtractionAlgorithm object that this menu item represents
                Type menuItemType = FEATURE_EXTRACTORS[menuItem.Text];

                //Get the type of FeatureExtractionAlgorithm object that is the default
                Type defaultType = FEATURE_EXTRACTORS[DEFAULT_ROTATION_CORRECTION_FEATURE_EXTRACTION];

                //If this menu item represents the type that is set as the default, check this menu item
                if (menuItemType.Equals(defaultType))
                {
                    menuItem.Checked = true;
                    break; //Found what we were looking for, look no further
                }
            }

            //Rotation Correction Classification
            foreach (ToolStripMenuItem menuItem in rotationCorrectionClassificationToolStripMenuItem.DropDownItems)
            {
                //Get the type of the classifier that this menu item represents
                Type menuItemType = CLASSIFIERS[menuItem.Text];

                //If this menu item represents the type that is set as the default, check this menu item
                if (menuItemType.Equals(DEFAULT_ROTATION_CORRECTION_CLASSIFICATION))
                {
                    menuItem.Checked = true;
                    break; //Found what we were looking for, look no further
                }
            }

            //Feature Extraction
            foreach (ToolStripMenuItem menuItem in featureExtractionToolStripMenuItem.DropDownItems)
            {
                //Get the type of the FeatureExtractionAlgorithm object that this menu item represents
                Type menuItemType = FEATURE_EXTRACTORS[menuItem.Text];

                //Get the type of FeatureExtractionAlgorithm object that is the default
                Type defaultType = FEATURE_EXTRACTORS[DEFAULT_FEATURE_EXTRACTION];

                //If this menu item represents the type that is set as the default, check this menu item
                if (menuItemType.Equals(defaultType))
                {
                    menuItem.Checked = true;
                    break; //Found what we were looking for, look no further
                }
            }

            //Classification
            foreach (ToolStripMenuItem menuItem in classificationToolStripMenuItem.DropDownItems)
            {
                //Get the type of the classifier that this menu item represents
                Type menuItemType = CLASSIFIERS[menuItem.Text];

                //If this menu item represents the type that is set as the default, check this menu item
                if (menuItemType.Equals(DEFAULT_CLASSIFICATION))
                {
                    menuItem.Checked = true;
                    break; //Found what we were looking for, look no further
                }
            }
        }

        //Setup event handlers for every sub menu item of algorithms, so that whenever an option gets changed, it is marked as selected
        private void setupAlgorithmsEventHandlers()
        {
            List<ToolStripMenuItem> leaves = getLeavesOf(algorithmsToolStripMenuItem);

            foreach(ToolStripMenuItem menuItem in leaves)
            {
                menuItem.Click += new EventHandler(algorithmsMenuItemChild_Click);
            }
        }

        //Treating the descendents of the supplied ToolStripMenuItem as a tree, return it's leaves
        private List<ToolStripMenuItem> getLeavesOf(ToolStripMenuItem root)
        {
            List<ToolStripMenuItem> leaves = new List<ToolStripMenuItem>();

            foreach(ToolStripMenuItem menuItem in root.DropDownItems)
            {
                //If this is a leaf
                if(menuItem.DropDownItems.Count == 0)
                {
                    //Add this leaf to the collection of leaves
                    leaves.Add(menuItem);
                }
                else //Otherwise go down this branch
                {
                    //Recursive step: find all the leaves of this node
                    leaves.AddRange(getLeavesOf(menuItem));
                }
            }

            return leaves;
        }

        //Get all of the descendents of a ToolStripMenuItem (i.e. children, grandchildren etc...)
        private List<ToolStripMenuItem> getDescendentsOf(ToolStripMenuItem root)
        {
            List<ToolStripMenuItem> descendents = new List<ToolStripMenuItem>();

            //Loop over all of the children
            foreach(ToolStripMenuItem menuItem in root.DropDownItems)
            {
                //Add this child Menu Item
                descendents.Add(menuItem);

                //Recursive step: add all of this Menu Items children
                descendents.AddRange(getDescendentsOf(menuItem));
            }

            return descendents;
        }
    }
}
