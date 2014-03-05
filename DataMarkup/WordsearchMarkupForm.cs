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

using ImageMarkup;
using ImageMarkup.Exceptions;
using SharedHelpers;
using SharedHelpers.Imaging;

namespace DataEntryGUI
{
    public partial class WordsearchMarkupForm : Form
    {
        //Private vars
        private string defaultLblToProcessLength;
        private Queue<string> toProcess;
        private string currentWordsearchId = null;
        private WordsearchImage currentWordsearchImage = null;

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

        /*
         * Private Helpers
         */
        private void updateLblToProcess()
        {
            lblToProcessLength.Text = toProcess.Count.ToString() + defaultLblToProcessLength;
        }

        private void btnNextWordsearch_Click(object sender, EventArgs e)
        {
            //If there is a Bitmap to dispose of
            if(currentWordsearchImage != null)
            {
                //TODO: Dispose of bitmap(s) derived from WordsearchImage bitmap

                //TODO: Deregister interest in the wordsearch image bitmap

            }

            //If there is another wordsearch to mark up
            if(toProcess.Count > 0)
            {
                //Get the next wordsearch ID
                currentWordsearchId = toProcess.Dequeue();

                //Get the clearest image of this wordsearch
                currentWordsearchImage = ImageMarkupDatabase.GetClearestWordsearchImage(currentWordsearchId);

                //TODO: Register interest in the bitmap, & fetch it

                //TODO: Draw the full grid & store for further drawing

                //TODO: Highlight 0, 0 (current index)

                //TODO: Display wordsearch image

            }
            else //Otherwise there are no more wordsearches to mark up
            {
                picBoxWordsearchImage.Image = new Bitmap(1, 1); //Set visible to false instead??
            }
            
        }
    }
}
