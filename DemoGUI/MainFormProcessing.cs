/*
 * Dissertation CV Wordsearch Solver
 * Demo GUI
 * Main Form (partial). Code to do all of the Image Processing
 * By Josh Keegan 12/05/2014
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
using SharedHelpers.ImageAnalysis.WordsearchSegmentation;
using SharedHelpers.ImageAnalysis.WordsearchSegmentation.VariedRowColSize;

namespace DemoGUI
{
    public partial class MainForm : Form
    {
        //Private variables
        private Dictionary<string, SegmentationAlgorithm> wordsearchSegmentationAlgorithms = new Dictionary<string, SegmentationAlgorithm>()
        {
            { "Blob Recognition", new SegmentByBlobRecognition() },
            { "Histogram Single Threshold", new SegmentByHistogramThresholdDarkPixels() },
            { "Histogram Two Thresholds from Percentile Rank", new SegmentByHistogramThresholdPercentileRankTwoThresholds() },
            { "Mean Dark Pixels", new SegmentByMeanDarkPixels() },
            { "Median Dark Pixels", new SegmentByMedianDarkPixels() },
            { "Percentile Two Thresholds", new SegmentByPercentileTwoThresholds() },
            { "Preselected Dark Pixel Threshold", new SegmentByThresholdDarkPixels() }
        };

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
                foreach(string txt in wordsearchSegmentationAlgorithms.Keys)
                {
                    //Make the tool strip item
                    ToolStripItem subMenuItem = menuItem.DropDownItems.Add(txt);
                }
            }

            //TODO: Same process for other menus to be populated

            //TODO: Set the default options as checked
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
