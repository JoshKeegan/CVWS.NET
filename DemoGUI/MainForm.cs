/*
 * Dissertation CV Wordsearch Solver
 * Demo GUI
 * Main Form, Business Logic
 * By Josh Keegan 09/05/2014
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

namespace DemoGUI
{
    public partial class MainForm : Form
    {
        //Private Variables
        private string currentDirectory = null;
        private Bitmap currentBitmap = null;
        private Dictionary<string, Bitmap> imageLog = null;

        public MainForm()
        {
            InitializeComponent();
        }

        #region Tool Strip Click Event Handlers
        /*
         * File
         */
        //Open Directory
        private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Ask the user which directory to use
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string dirPath = folderBrowserDialog.SelectedPath;

                //If this directory opens without error
                if(openDirectory(dirPath))
                {
                    //Store this as the current directory
                    currentDirectory = dirPath;

                    //TODO: When recent directories is implemented, remember this dir having been opened
                }
            }
        }

        //Recent Directories
        private void recentDirectoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: Yet to implement this functionality
        }

        //Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region List View Click Event Handlers
        //A new file has been selected for processing
        private void listViewFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            openSelectedImageFile();
        }
        
        //A new image from the image log has been selected for viewing
        private void listViewImageLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            showSelectedImageLogEntry();
        }
        #endregion

        #region Helper Methods
        //Open a directory
        private bool openDirectory(string dirPath)
        {
            bool success = true;

            try
            {
                //Find all of the File Names of Images in this directory
                List<string> imageFileNames = ImageLoader.FindAllImageFileNamesInDir(dirPath);

                //Clear the interface ready to have the images for this dir displayed
                clearInterface();

                //Show the file list for this Directory
                listViewFiles.Items.AddRange(imageFileNames.Select(str => new ListViewItem(str)).ToArray());
            }
            catch
            {
                success = false;
                MessageBox.Show(String.Format("Could not open directory \"{0}\"", dirPath), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return success;
        }

        private void openSelectedImageFile()
        {
            //Dispose of the currently selected image
            if(currentBitmap != null)
            {
                currentBitmap.Dispose();
                currentBitmap = null;
            }

            //Clear everthing that is set on a per-file basis
            clearImageLog();
            //TODO: Add more fields to be cleared here as they get added/used

            //If there is a selected file & a selected directory
            if(listViewFiles.SelectedItems.Count == 1 && currentDirectory != null)
            {
                //Try in case of IO problems
                try
                {
                    //Load the new image as the currently selected Bitmap
                    string imagePath = Path.Combine(currentDirectory, listViewFiles.SelectedItems[0].Text);

                    currentBitmap = (Bitmap)Bitmap.FromFile(imagePath);
                }
                catch //Something went wrong loading the image
                {
                    MessageBox.Show("Could not load file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //If the Bitmap was loaded successfully
            if(currentBitmap != null)
            {
                //Set up the Image Log (with this being the first entry)
                imageLog = new Dictionary<string, Bitmap>();
                imageLogAdd("Input", currentBitmap);

                //Select this item in the image log so it gets displayed in the picture box
                listViewImageLog.Items[0].Selected = true;
            }
        }

        private void showSelectedImageLogEntry()
        {
            //If there is a selected image log entry
            if(listViewImageLog.SelectedItems.Count == 1)
            {
                //Show this Bitmap in the picture box
                pictureBox.Image = imageLog[listViewImageLog.SelectedItems[0].Text];
            }
        }

        private void imageLogAdd(string txt, Bitmap img)
        {
            imageLog.Add(txt, img);

            //Add this on the screen
            listViewImageLog.Items.Add(txt);
        }

        //Clear the Interface of everything loaded (disposing of all images etc...)
        //Note: Doesn't dispose of the current Bitmap so that it won't have to be loaded again if we're doing more work on it
        private void clearInterface()
        {
            //TODO: Check all things are being cleared & keep adding to this as new things get added

            //Clear the currently displayed image
            pictureBox.Image = null;

            //Clear everything related to the image log
            clearImageLog();

            //Clear the File List View
            listViewFiles.Items.Clear();

            //Clear the Text Log
            txtLog.Text = "";

            //TODO: Clear the status strip
        }

        private void clearImageLog()
        {
            //Dispose of all of the Bitmaps on the Image Log (to prevent memory leaks). 
            //  Do not dispose of the current Bitmap (so that it doesn't have to be loaded again
            //  if we're going to run another algorithm on it)
            if (imageLog != null)
            {
                foreach (Bitmap img in imageLog.Values)
                {
                    if (img != currentBitmap)
                    {
                        img.Dispose();
                    }
                }
            }
            imageLog = null;

            //Clear the List View
            listViewImageLog.Items.Clear();
        }
        #endregion
    }
}
