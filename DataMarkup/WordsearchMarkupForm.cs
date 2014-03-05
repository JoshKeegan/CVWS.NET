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
using System.Text.RegularExpressions;
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
        private string defaultLblWordsearchId;
        private Queue<string> toProcess;
        private string currentWordsearchId = null;
        private WordsearchImage currentWordsearchImage = null;
        private Bitmap currentBitmapWithGrid = null;

        public WordsearchMarkupForm()
        {
            InitializeComponent();

            //Store any label defaults
            defaultLblToProcessLength = lblToProcessLength.Text;
            defaultLblWordsearchId = lblWordsearchId.Text;

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
                currentBitmapWithGrid = null;

                currentWordsearchId = null;
                currentWordsearchImage = null;
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

                //Update the label showing the wordsearch ID
                lblWordsearchId.Text = defaultLblWordsearchId + currentWordsearchId;

                //Reset all fields
                resetFields();
            }
            else //Otherwise there are no more wordsearches to mark up
            {
                picBoxWordsearchImage.Visible = false;
                lblWordsearchId.Text = defaultLblWordsearchId;
            }
        }

        private void rtbChars_TextChanged(object sender, EventArgs e)
        {
            //If we're currently marking up a wordsearch
            if(currentBitmapWithGrid != null)
            {
                string[] lines = rtbChars.Text.ToUpper().Split('\n');

                //Check that the text entered hasn't gone out of bounds
                validateCharsStr(lines);

                //Get the row & col number of the caret
                Tuple<int, int> rowColPos = getRowsAndColsFromCaretPos(rtbChars.SelectionStart);
                int rowIdx = rowColPos.Item1;
                int colIdx = rowColPos.Item2;

                //If the user has typed in the last character of a row, and there are more rows to be entered, add a new line
                if (colIdx >= currentWordsearchImage.Cols
                    && rowIdx < currentWordsearchImage.Rows - 1)
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
        }

        private void btnSaveWordsearch_Click(object sender, EventArgs e)
        {
            //check that we currently have a Wordsearch on screen
            if(currentWordsearchId != null)
            {
                //Validate the characters entered
                string[] lines = rtbChars.Text.ToUpper().Split('\n');

                bool validChars = validateCharsStr(lines);

                if(validChars)
                {
                    char[,] chars = getChars(lines);
                    string[] words = getWords();

                    //Make a Wordsearch object
                    Wordsearch wordsearch = new Wordsearch(currentWordsearchId, chars, words);

                    //If this wordsearch is already in the database, remove it
                    if(ImageMarkupDatabase.ContainsWordsearch(currentWordsearchId))
                    {
                        ImageMarkupDatabase.RemoveWordsearch(currentWordsearchId);
                    }

                    //Store the Wordsearch object and write it out
                    ImageMarkupDatabase.AddWordsearch(wordsearch);
                    ImageMarkupDatabase.WriteDatabase();
                }
                else
                {
                    MessageBox.Show("Please check the chars entered");
                }
            }
            else //Otherwise there is no wordsearch to save data for
            {
                MessageBox.Show("No wordsearch currently on screen to save data for");
            }
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

        private bool validateCharsStr(string[] lines)
        {
            bool toRet = true;

            //Check that the text entered hasn't gone out of bounds
            if (lines.Length > currentWordsearchImage.Rows)
            {
                MessageBox.Show("Too many Rows");
                toRet = false;
            }

            foreach (string line in lines)
            {
                if (line.Length > currentWordsearchImage.Cols)
                {
                    MessageBox.Show("Too many Cols");
                    toRet = false;
                    break; //Only show the error once
                }
            }

            //Only allow A-Z
            foreach(string line in lines)
            {
                foreach(char c in line)
                {
                    if(c < 'A' || c > 'Z')
                    {
                        MessageBox.Show("Chars must only be A-Z");
                        toRet = false;
                        break; //Only show the error once
                    }
                }
            }

            //Check the number of rows
            if(lines.Length != currentWordsearchImage.Rows)
            {
                toRet = false;
            }

            //Check the number of cols
            foreach(string line in lines)
            {
                if(line.Length != currentWordsearchImage.Cols)
                {
                    toRet = false;
                }
            }

            return toRet;
        }

        private char[,] getChars(string[] lines)
        {
            char[,] chars = new char[currentWordsearchImage.Cols, currentWordsearchImage.Rows];

            for(int row = 0; row < chars.GetLength(1); row++)
            {
                for(int col = 0; col < chars.GetLength(0); col++)
                {
                    chars[col, row] = lines[row][col];
                }
            }

            return chars;
        }

        //Get the words entered. Strip non-alphabetic characters as these will not be in the wordsearch
        private string[] getWords()
        {
            string[] lines = rtbWords.Text.ToUpper().Split('\n');

            List<string> words = new List<string>(lines.Length);

            foreach(string line in lines)
            {
                //Ignore non-alphabetic characters
                string word = Regex.Replace(line, @"[^A-Z]+", "");

                //Ignore empty lines
                if(word != "")
                {
                    words.Add(word);
                }
            }

            return words.ToArray();
        }

        private void resetFields()
        {
            rtbChars.Text = "";
            rtbWords.Text = "";
        }
    }
}
