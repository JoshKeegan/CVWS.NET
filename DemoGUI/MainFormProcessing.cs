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

namespace DemoGUI
{
    public partial class MainForm : Form
    {
        //Constants
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
            { "Principal Component Analysis (PCA)", typeof(FeatureExtractionPCA) },
            { "Raw Pixel Values", typeof(FeatureExtractionPixelValues) }
        };

        private static readonly Dictionary<string, Type> CLASSIFIERS = new Dictionary<string,Type>()
        {
            { "Single Layer Neural Network", typeof(AForgeActivationNeuralNetClassifier) }
        };

        private static readonly Type DEFAULT_WORDSEARCH_DETECTION_SEGMENTATION = typeof(SegmentByHistogramThresholdPercentileRankTwoThresholds);
        private static readonly Type DEFAULT_WORDSEARCH_SEGMENTATION = typeof(SegmentByBlobRecognition);
        private static readonly Type DEFAULT_ROTATION_CORRECTION_FEATURE_EXTRACTION = typeof(FeatureExtractionPCA);
        private static readonly Type DEFAULT_ROTATION_CORRECTION_CLASSIFICATION = typeof(AForgeActivationNeuralNetClassifier);
        private static readonly Type DEFAULT_FEATURE_EXTRACTION = typeof(FeatureExtractionPCA);
        private static readonly Type DEFAULT_CLASSIFICATION = typeof(AForgeActivationNeuralNetClassifier);

        //Helper method to do all of the processing to solve a wordsearch
        private void doProcessing()
        {
            //TODO
            throw new NotImplementedException();
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

                //If this menu item represents the type that is set as the default, check this menu item
                if (menuItemType.Equals(DEFAULT_ROTATION_CORRECTION_FEATURE_EXTRACTION))
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

                //If this menu item represents the type that is set as the default, check this menu item
                if (menuItemType.Equals(DEFAULT_FEATURE_EXTRACTION))
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
