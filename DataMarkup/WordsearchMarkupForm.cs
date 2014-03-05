/*
 * Dissertation CV Wordsearch Solver
 * Data Entry GUI
 * Wordsearch Markup Form class
 * By Josh Keegan 05/03/2014
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AForge;
using AForge.Imaging;

using ImageMarkup;
using ImageMarkup.Exceptions;
using SharedHelpers;
using SharedHelpers.Imaging;
using SharedHelpers.Exceptions;

namespace DataEntryGUI
{
    public partial class WordsearchMarkupForm : Form
    {
        //Constants
        private static Color HIGHLIGHT_CHAR_COLOUR = Color.Blue;
        private const int WORDSEARCH_IMAGE_BITMAP_WIDTH = 400;
        private const int WORDSEARCH_IMAGE_BITMAP_HEIGHT = 400;

        //Private vars
        private string defaultLblToProcessLength;
        private Queue<string> toProcess;
        private string currentWordsearchId = null;
        private WordsearchImage currentWordsearchImage = null;
        private Bitmap currentBitmapWithGrid = null;

        public WordsearchMarkupForm()
        {
            InitializeComponent();

            //Store the lblToProcessLength default label so the number can be prepended
            defaultLblToProcessLength = lblToProcessLength.Text;

            //If the Image Markup Database hasn't already been loaded
            if (!ImageMarkupDatabase.Loaded)
            {
                //Load in the meta data for images that are already processed
                ImageMarkupDatabase.LoadDatabase();
            }

            //Get the ID's for wordsearches that haven't been marked up
            HashSet<string> liWordsearchIDsToBeMarkedUp = ImageMarkupDatabase.GetWordsearchIDsToBeMarkedUp();
            toProcess = new Queue<string>(liWordsearchIDsToBeMarkedUp);

            updateLblToProcess();
        }

        private void btnNextWordsearch_Click(object sender, EventArgs e)
        {
            //If there is a Bitmap to dispose of
            if (currentWordsearchImage != null)
            {
                //Dispose of bitmap(s) derived from WordsearchImage bitmap
                if(picBoxWordsearchImage.Image != currentBitmapWithGrid)
                {
                    picBoxWordsearchImage.Image.Dispose();
                }
                currentBitmapWithGrid.Dispose();
            }

            //If there is another wordsearch to mark up
            if (toProcess.Count > 0)
            {
                //Get the next wordsearch ID
                currentWordsearchId = toProcess.Dequeue();

                //Get the clearest image of this wordsearch
                currentWordsearchImage = ImageMarkupDatabase.GetClearestWordsearchImage(currentWordsearchId);

                //Get the bitmap of this wordsearch image and draw the grid on it
                currentBitmapWithGrid = currentWordsearchImage.GetBitmapCustomResolution(
                    WORDSEARCH_IMAGE_BITMAP_WIDTH, WORDSEARCH_IMAGE_BITMAP_HEIGHT);
                DrawGrid.GridInPlace(currentBitmapWithGrid, currentWordsearchImage.Rows, currentWordsearchImage.Cols);

                //Highlight 0, 0 (current index) & display the image
                highlightCharacter(0, 0);

                //Update the label showing how many wordsearches are left to be processed
                updateLblToProcess();
            }
            else //Otherwise there are no more wordsearches to mark up
            {
                picBoxWordsearchImage.Visible = false;
            }
        }

        private void rtbChars_TextChanged(object sender, EventArgs e)
        {
            string[] lines = rtbChars.Text.Split('\n');
            

            //TODO: Check that the text entered hasn't gone out of bounds


            //Get the row & col number of the caret
            Tuple<int, int> rowColPos = getRowsAndColsFromCaretPos(rtbChars.SelectionStart);
            int rowIdx = rowColPos.Item1;
            int colIdx = rowColPos.Item2;

            //If the user has typed in the last character of a row, and there are more rows to be entered, add a new line
            if(colIdx >= currentWordsearchImage.Cols)
            {
                rtbChars.Text += "\n";
                rtbChars.Focus(); //Give the RTB focus to ensure it's safe to move the caret
                rtbChars.SelectionStart = rtbChars.Text.Length;

                //Update the indexes we're using
                rowIdx++;
                colIdx = 0;
            }

            //Highlight the next character for the user to enter
            highlightCharacter(rowIdx, colIdx);
        }

        /*
         * Private Helpers
         */
        private void updateLblToProcess()
        {
            lblToProcessLength.Text = toProcess.Count.ToString() + defaultLblToProcessLength;
        }

        private Tuple<int, int> getRowsAndColsFromCaretPos(int caretPos)
        {
            int rowIdx = caretPos / ((int)currentWordsearchImage.Cols + 1); //+1 to account for newline characters
            int colIdx = caretPos % ((int)currentWordsearchImage.Cols + 1);

            return new Tuple<int, int>(rowIdx, colIdx);
        }

        private void highlightCharacter(int row, int col)
        {
            if(row < 0 || col < 0)
            {
                throw new InvalidRowsAndColsException("Rows or Cols must not be negative");
            }

            IntPoint[] charPoints = DrawGrid.GetImageCoordinatesForChar(currentBitmapWithGrid.Width, currentBitmapWithGrid.Height,
                currentWordsearchImage.Rows, currentWordsearchImage.Cols, (uint)row, (uint)col);

            Bitmap highlighted = DrawShapes.Polygon(currentBitmapWithGrid, new List<IntPoint>(charPoints), HIGHLIGHT_CHAR_COLOUR);

            //If there is a highlighted image currently being show, dispose of it
            if(picBoxWordsearchImage.Image != null &&
                picBoxWordsearchImage.Image != currentBitmapWithGrid)
            {
                picBoxWordsearchImage.Image.Dispose();
            }

            picBoxWordsearchImage.Image = highlighted;
        }
    }
}
