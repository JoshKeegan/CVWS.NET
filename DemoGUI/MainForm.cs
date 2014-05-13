/*
 * Dissertation CV Wordsearch Solver
 * Demo GUI
 * Main Form, Business Logic
 * By Josh Keegan 09/05/2014
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

namespace DemoGUI
{
    public partial class MainForm : Form
    {
        //Constants
        private const string CONFIG_X = "MainFormX";
        private const string CONFIG_Y = "MainFormY";
        private const string CONFIG_WIDTH = "MainFormWidth";
        private const string CONFIG_HEIGHT = "MainFormHeight";
        private const string CONFIG_STATE = "MainFormState";
        private const string CONFIG_MAIN_SPLITTER = "MainFormMainSplitter";
        private const string CONFIG_LEFT_SPLITTER = "MainFormLeftSplitter";
        private const string CONFIG_RIGHT_SPLITTER = "MainFormRightSplitter";
        private const string CONFIG_RIGHT_TOP_SPLITTER = "MainFormRightTopSplitter";
        private const string CONFIG_RIGHT_BOTTOM_SPLITTER = "MainFormRightBottomSplitter";
        private const string CONFIG_RECENT_DIRECTORY = "RecentDirectory";
        private const string CONFIG_PICTURE_BOX_SIZE_MODE = "MainFormPictureBoxSizeMode";

        private const int NUM_RECENT_DIRECTORIES = 8;

        //Private Variables
        private LinkedList<string> recentDirectories = new LinkedList<string>();
        private string currentDirectory = null;
        private Bitmap currentBitmap = null;
        private Dictionary<string, Bitmap> imageLog = null;
        private string defaultTxtWordsToFind;
        private Task processingTask = null;

        #region Object Construction & Form Initialisation
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Store the default value of txtWordsToFind for later use
            defaultTxtWordsToFind = txtWordsToFind.Text;

            //Populate the Algorithms Menu, set it's defaults & place event handlers on anything that you can choose
            populateAlgorithmsMenu();
            //TODO: Set the Algorithms Menu default options
            setupAlgorithmsEventHandlers();

            //If the configuration from a previous run of the program can be loaded, use those settings
            if(Configuration.Load())
            {
                //Attempt to load each setting from the configuration file

                //Load Window Position, Size & State
                int windowX = int.Parse(Configuration.GetConfigurationOption(CONFIG_X));
                int windowY = int.Parse(Configuration.GetConfigurationOption(CONFIG_Y));
                int windowWidth = int.Parse(Configuration.GetConfigurationOption(CONFIG_WIDTH));
                int windowHeight = int.Parse(Configuration.GetConfigurationOption(CONFIG_HEIGHT));

                Size windowSize = new Size(windowWidth, windowHeight);
                Point windowTopLeft = new Point(windowX, windowY);
                Point windowTopRight = new Point(windowX + windowWidth, windowY + windowHeight);

                //Validate window position & size by checking it's (at least partially) within the area covered by a display
                bool validWindowPosition = false;
                foreach (Screen screen in Screen.AllScreens)
                {
                    //If any part of the top of the window lies within a screen, then this is a valid window position
                    if (screen.WorkingArea.Contains(windowTopLeft)
                        || screen.WorkingArea.Contains(windowTopRight))
                    {
                        validWindowPosition = true;
                        break;
                    }
                }

                //Only set the window to the position from the config if it's valid
                if (validWindowPosition)
                {
                    this.Location = windowTopLeft;
                    this.Size = windowSize;

                    this.WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState),
                        Configuration.GetConfigurationOption(CONFIG_STATE));

                    //Load the splitter distances too
                    splitContainerMain.SplitterDistance = int.Parse(Configuration.GetConfigurationOption(CONFIG_MAIN_SPLITTER));
                    splitContainerLeft.SplitterDistance = int.Parse(Configuration.GetConfigurationOption(CONFIG_LEFT_SPLITTER));
                    splitContainerRight.SplitterDistance = int.Parse(Configuration.GetConfigurationOption(CONFIG_RIGHT_SPLITTER));
                    splitContainerRightTop.SplitterDistance = int.Parse(Configuration.GetConfigurationOption(CONFIG_RIGHT_TOP_SPLITTER));
                    splitContainerRightBottom.SplitterDistance = int.Parse(Configuration.GetConfigurationOption(CONFIG_RIGHT_BOTTOM_SPLITTER));
                }

                //Load Recent Directories
                for (int i = 0; i < NUM_RECENT_DIRECTORIES; i++)
                {
                    string dirPath = Configuration.GetConfigurationOption(CONFIG_RECENT_DIRECTORY + i);

                    if (dirPath != null)
                    {
                        recentDirectories.AddLast(dirPath);
                    }
                }
                //Show the loaded list of recent dirs in the menu
                generateRecentDirsList();

                //Load Picture View Mode
                PictureBoxSizeMode pictureBoxSizeMode = (PictureBoxSizeMode)Enum.Parse(typeof(PictureBoxSizeMode),
                    Configuration.GetConfigurationOption(CONFIG_PICTURE_BOX_SIZE_MODE));
                setPictureBoxSizeMode(pictureBoxSizeMode);
            }
            else //Otherwise we couldn't load a configuration file
            {
                // Set any defaults that aren't already set
                setPictureBoxSizeMode(PictureBoxSizeMode.Zoom);
            }
        }
        #endregion

        #region Form Controls
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save Window Position, Size & State
            if(this.WindowState != FormWindowState.Minimized) //Don't save the current position if minimised
            {
                //Don't save the location, width & height if maximised
                if(this.WindowState != FormWindowState.Maximized)
                {
                    Configuration.SetConfigurationOption(CONFIG_X, Location.X.ToString());
                    Configuration.SetConfigurationOption(CONFIG_Y, Location.Y.ToString());
                    Configuration.SetConfigurationOption(CONFIG_WIDTH, Width.ToString());
                    Configuration.SetConfigurationOption(CONFIG_HEIGHT, Height.ToString());
                }

                //Save the Form state
                Configuration.SetConfigurationOption(CONFIG_STATE, WindowState.ToString());

                //Save the Splitter Distances
                Configuration.SetConfigurationOption(CONFIG_MAIN_SPLITTER, splitContainerMain.SplitterDistance.ToString());
                Configuration.SetConfigurationOption(CONFIG_LEFT_SPLITTER, splitContainerLeft.SplitterDistance.ToString());
                Configuration.SetConfigurationOption(CONFIG_RIGHT_SPLITTER, splitContainerRight.SplitterDistance.ToString());
                Configuration.SetConfigurationOption(CONFIG_RIGHT_TOP_SPLITTER, splitContainerRightTop.SplitterDistance.ToString());
                Configuration.SetConfigurationOption(CONFIG_RIGHT_BOTTOM_SPLITTER, splitContainerRightBottom.SplitterDistance.ToString());
            }

            //Save Recent Directories
            int idx = 0;
            //Write out any recent directories stored in memory
            foreach(string dirPath in recentDirectories)
            {
                Configuration.SetConfigurationOption(CONFIG_RECENT_DIRECTORY + idx, dirPath);

                idx++;
            }

            //Null any remaining slots (to overwrite data that may have previously been stored there)
            for(; idx < NUM_RECENT_DIRECTORIES; idx++)
            {
                Configuration.SetConfigurationOption(CONFIG_RECENT_DIRECTORY + idx, null);
            }

            //Save Picture View Mode
            Configuration.SetConfigurationOption(CONFIG_PICTURE_BOX_SIZE_MODE, pictureBox.SizeMode.ToString());

            //Save the config out to file
            try
            {
                Configuration.Save();
            }
            catch
            {
                MessageBox.Show("Could not save Configuration file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

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
                    //Remember this dir having been opened
                    addRecentDir(dirPath);
                    //Refresh the recent directory list to include this new directory (and get rid of one removed to make way for it if applicable)
                    generateRecentDirsList();
                }
            }
        }

        //When one of the recent directories gets selected to be opened
        private void recentDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dirPath = ((ToolStripMenuItem)sender).Text;

            //If this directory opens without error
            if(openDirectory(dirPath))
            {
                //Move this recent directory to the top of the list
                addRecentDir(dirPath);
                //Refresh the recent directory list shown in the menu
                generateRecentDirsList();
            }
        }

        //Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /*
         * Settings
         */
        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setPictureBoxSizeMode(PictureBoxSizeMode.Zoom);
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setPictureBoxSizeMode(PictureBoxSizeMode.Normal);
        }

        private void centreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setPictureBoxSizeMode(PictureBoxSizeMode.CenterImage);
        }

        private void autoSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setPictureBoxSizeMode(PictureBoxSizeMode.AutoSize);
        }

        private void stretchImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setPictureBoxSizeMode(PictureBoxSizeMode.StretchImage);
        }

        //When anything under Algorithms (or any level below that gets clicked), 
        //  call this to mark it as checked
        private void algorithmsMenuItemChild_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItemSender = (ToolStripMenuItem)sender;

            //Find this Menu Item's siblings
            ToolStripMenuItem parent = (ToolStripMenuItem)menuItemSender.OwnerItem;
            ToolStripItemCollection siblings = parent.DropDownItems;

            //If the sender has siblings
            if(siblings.Count > 1)
            {
                //Uncheck all of the siblings
                foreach(ToolStripMenuItem menuItem in siblings)
                {
                    menuItem.Checked = false;
                }

                //Check the menu item that got clicked (the sender)
                menuItemSender.Checked = true;
            }
        }
        #endregion

        #region Other Event Handlers
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

        //txtWordsToFind has become the active control (e.g. been clicked on )
        private void txtWordsToFind_Enter(object sender, EventArgs e)
        {
            //If the text in txtWordsToFind is the default string, clear it so that it can be typed in
            if(txtWordsToFind.Text == defaultTxtWordsToFind)
            {
                txtWordsToFind.Text = "";
            }
        }

        //txtWordsToFind is no longer the active control (e.g. user has clicked elsewhere)
        private void txtWordToFind_Leave(object sender, EventArgs e)
        {
            //If the text in txtWordsToFind is empty, then put the default text back
            if(txtWordsToFind.Text == "")
            {
                txtWordsToFind.Text = defaultTxtWordsToFind;
            }
        }

        //The button to start the processing has been clicked
        private void btnStartProcessing_Click(object sender, EventArgs e)
        {
            //Disable the button so it cannot be clicked again
            btnStartProcessing.Enabled = false;

            //Do the processing asynchronously so that the main thread is still free to recieve events
            processingTask = Task.Factory.StartNew(() =>
            {
                doProcessing();
            }).ContinueWith(taskState =>
            {
                //If the Task completed due to an unhandled exception being thrown, tell the user & log the exception
                if(taskState.IsFaulted)
                {
                    //Put the exception in the processing log for closer inspection
                    log("Fatal Exception: " + taskState.Exception.ToString());

                    //Tell the user that the processing operation couldn't be completed
                    MessageBox.Show("Couldn't complete operation.\nSee Processing log for details", "Processing Failed to Complete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                //Re-enable the button so that it can be clicked again
                btnStartProcessing.Enabled = true;
            });
        }
        #endregion

        #region Helper Methods
        //Log some text
        private void log(string text)
        {
            //If the text box already has content, then add a new line before appending this
            if(txtLog.Text != "")
            {
                txtLog.Text += Environment.NewLine;
            }

            //Append this text to the log
            txtLog.Text += text;
        }

        private void log(Bitmap img, string text)
        {
            //Add to the imageLog dictionary first, so if the key is already present the exception will be 
            //  thrown before the text gets added to the list
            imageLog.Add(text, img);

            //Add the text to the list
            listViewImageLog.Items.Add(text);
        }

        private void generateRecentDirsList()
        {
            //Remove onclick events for the existing list
            foreach(ToolStripItem toolStripItem in recentDirectoriesToolStripMenuItem.DropDownItems)
            {
                toolStripItem.Click -= new EventHandler(recentDirectoryToolStripMenuItem_Click);
            }

            //Remove all items currently in the list
            recentDirectoriesToolStripMenuItem.DropDownItems.Clear();

            //Add new Items to the list & onclick events for them
            foreach(string dirPath in recentDirectories)
            {
                //Make the tool strip item
                ToolStripItem toolStripItem = recentDirectoriesToolStripMenuItem.DropDownItems.Add(dirPath);

                //Add an event handler for it
                toolStripItem.Click += new EventHandler(recentDirectoryToolStripMenuItem_Click);
            }
        }

        private void addRecentDir(string dirPath)
        {
            //If this directory is already in the recent directories list, remove it
            recentDirectories.Remove(dirPath);

            //Insert it as the most recent
            recentDirectories.AddFirst(dirPath);

            //If we now have too many recently accessed directories, remove the one that was accessed the longest time ago
            if(recentDirectories.Count > NUM_RECENT_DIRECTORIES)
            {
                recentDirectories.RemoveLast();
            }
        }

        private void removeRecentDir(string dirPath)
        {
            recentDirectories.Remove(dirPath);
        }

        private void setPictureBoxSizeMode(PictureBoxSizeMode sizeMode)
        {
            //The container for the picture box (will be used for scrolling if necessary)
            SplitterPanel container = splitContainerRight.Panel1;

            //If using AutoSize (where the PictureBox will automatically resize to the size of the image), show scroll bars
            if(sizeMode == PictureBoxSizeMode.AutoSize)
            {
                pictureBox.Dock = DockStyle.None;
                pictureBox.Location = new Point(0, 0);
                container.AutoScroll = true;
            }
            else //Otherwise make the picture box fill all of the available space & hide the scroll bars
            {
                pictureBox.Dock = DockStyle.Fill;
                container.AutoScroll = false;
            }

            pictureBox.SizeMode = sizeMode;

            //Check the newly selected size mode in the menu
            //First uncheck all
            foreach(ToolStripMenuItem menuItem in imageViewToolStripMenuItem.DropDownItems)
            {
                menuItem.Checked = false;
            }

            //Now check the one we've just enabled
            switch(sizeMode)
            {
                case PictureBoxSizeMode.AutoSize:
                    autoSizeToolStripMenuItem.Checked = true;
                    break;
                case PictureBoxSizeMode.CenterImage:
                    centreToolStripMenuItem.Checked = true;
                    break;
                case PictureBoxSizeMode.Normal:
                    normalToolStripMenuItem.Checked = true;
                    break;
                case PictureBoxSizeMode.StretchImage:
                    stretchImageToolStripMenuItem.Checked = true;
                    break;
                case PictureBoxSizeMode.Zoom:
                    zoomToolStripMenuItem.Checked = true;
                    break;
            }
        }

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

                //Dispose of the current Bitmap if there is one stored
                if(currentBitmap != null)
                {
                    currentBitmap.Dispose();
                    currentBitmap = null;
                }

                //Show the file list for this Directory
                listViewFiles.Items.AddRange(imageFileNames.Select(str => new ListViewItem(str)).ToArray());

                //Store this as the current directory
                currentDirectory = dirPath;
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
