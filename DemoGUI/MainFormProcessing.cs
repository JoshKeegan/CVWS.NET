/*
 * Dissertation CV Wordsearch Solver
 * Demo GUI
 * Main Form (partial). Code to do all of the Image Processing
 * By Josh Keegan 12/05/2014
 * Last Edit 13/05/2014
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SharedHelpers;
using SharedHelpers.ClassifierInterfacing;
using SharedHelpers.ClassifierInterfacing.FeatureExtraction;
using SharedHelpers.ImageAnalysis.WordsearchSegmentation;
using SharedHelpers.ImageAnalysis.WordsearchSegmentation.VariedRowColSize;

using DemoGUI.Exceptions;
using SharedHelpers.WordsearchSolver;

namespace DemoGUI
{
    public partial class MainForm : Form
    {
        //Constants
        private const string TRAINED_ALGORITHMS_PATH = "TrainedAlgorithms/";
        private const string TRAINED_FEATURE_EXTRACTORS_PATH = TRAINED_ALGORITHMS_PATH + "FeatureExtractors/";
        private const string TRAINED_CLASSIFIERS_PATH = TRAINED_ALGORITHMS_PATH + "Classifiers/";
        private const string TRAINED_CLASSIFIER_FILE_EXTENSION = ".classifier";
        private const string TRAINED_FEATURE_EXTRACTOR_FILE_EXTENSION = ".featureExtraction";

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

        private static readonly Type DEFAULT_WORDSEARCH_DETECTION_SEGMENTATION = typeof(SegmentByHistogramThresholdPercentileRankTwoThresholds);
        private static readonly Type DEFAULT_WORDSEARCH_SEGMENTATION = typeof(SegmentByBlobRecognition);
        //private static readonly Type DEFAULT_ROTATION_CORRECTION_FEATURE_EXTRACTION = typeof(FeatureExtractionPCA);
        private const string DEFAULT_ROTATION_CORRECTION_FEATURE_EXTRACTION = "Principal Component Analysis (PCA) All Features"; //Feature Extraction as a stirng because there can be more than one opention per type
        private static readonly Type DEFAULT_ROTATION_CORRECTION_CLASSIFICATION = typeof(AForgeActivationNeuralNetClassifier);
        //private static readonly Type DEFAULT_FEATURE_EXTRACTION = typeof(FeatureExtractionPCA );
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

        //Helper method to do all of the processing to solve a wordsearch
        private void doProcessing()
        {
            /*
             * Get all of the selected Algorithms to be used for the processing
             */
            log("Loading Selected Algorithms . . .");
            //Wordsearch Detection Segmentation Algorithm
            SegmentationAlgorithm wordsearchDetectionSegmentationAlgorithm = 
                getSelectedSegmentationAlgorithm(wordsearchDetectionSegmentationToolStripMenuItem);

            //Wordsearch Segmentation Algorithm
            SegmentationAlgorithm wordsearchSegmentationAlgorithm =
                getSelectedSegmentationAlgorithm(wordsearchSegmentationToolStripMenuItem);

            //Rotation Correction Classifier object (contains both feature extractor & classifier)
            Classifier rotationCorrectionClassifier = getSelectedClassifier(rotationCorrectionFeatureExtractionToolStripMenuItem,
                rotationCorrectionClassificationToolStripMenuItem);

            //Classifier object used for actual classification (contains both feature extractor & classifier)
            Classifier classifier = getSelectedClassifier(featureExtractionToolStripMenuItem, classificationToolStripMenuItem);

            //Wordsearch Solver Algorithm
            Solver wordsearchSolver = getSelectedWordsearchSolver();

            log("Selected Algorithms Loaded Successfully!");

            //TODO: Process the Image
        }

        //Get the selected Wordsearch Solver
        private Solver getSelectedWordsearchSolver()
        {
            //Check if each menu item is checked in turn
            if(nonProbabilisticToolStripMenuItem.Checked)
            {
                return new SolverProbabilistic();
            }
            else if(probabilisticClassificationToolStripMenuItem.Checked)
            {
                return new SolverNonProbabilistic();
            }
            else //Otherwise no Menu Item was checked (or we didn't test for the checked one)
            {
                throw new InvalidMenuItemSelectionException();
            }
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
            //Wordsearch Detection Candidate Ranking
            foreach(ToolStripMenuItem menuItem in wordsearchDetectionSegmentationToolStripMenuItem.DropDownItems)
            {
                //Get the type of the SegmentationAlgorithm object that this menu item represents
                Type menuItemType = WORDSEARCH_SEGMENTATION_ALGORITHMS[menuItem.Text].GetType();
                
                //If this menu item represents the type that is set as the default, check this menu item
                if(menuItemType.Equals(DEFAULT_WORDSEARCH_DETECTION_SEGMENTATION))
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
