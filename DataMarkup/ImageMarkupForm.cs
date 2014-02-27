/*
 * Dissertation CV Wordsearch Solver
 * Data Entry GUI
 * Main Form Class - Main GUI Window
 * By Josh Keegan 26/02/2014
 * Last Edit 27/02/2014
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ImageMarkup;
using SharedHelpers;
using BaseObjectExtensions;

namespace DataEntryGUI
{
    public partial class ImageMarkupForm : Form
    {
        //Constants
        private const string RAW_IMAGE_DIR = "images";

        //Private vars
        private Queue<KeyValuePair<Bitmap, string>> toProcess; //Store the hash along with the Bitmap so we don't have to recompute it later
        private KeyValuePair<Bitmap, string> currentBitmap;

        public ImageMarkupForm()
        {
            InitializeComponent();

            //Load in the meta data for images that are already processed
            ImageMarkupDatabase.LoadDatabase();

            //Load in the raw images to process
            List<Bitmap> bitmaps = ImageLoader.LoadAllImagesInSubdirs(RAW_IMAGE_DIR);
            toProcess = new Queue<KeyValuePair<Bitmap, string>>();

            //Do not Load Bitmaps that we already have data for
            HashSet<string> processingQueueHashes = new HashSet<string>();
            foreach(Bitmap b in bitmaps)
            {
                string bitmapHash = b.GetDataHashCode();

                //Only add this Bitmap to the processing queue if we don't already hold any data on it (in the dataset or on the processing queue)
               if(!ImageMarkupDatabase.ContainsImage(bitmapHash) && !processingQueueHashes.Contains(bitmapHash))
               {
                   toProcess.Enqueue(new KeyValuePair<Bitmap, string>(b, bitmapHash));
                   processingQueueHashes.Add(bitmapHash);
               }
            }

            updateLblToProcessLength();
        }

        private void picBoxWordsearchImage_Click(object sender, EventArgs e)
        {
            //TODO: Get the co-ords of the click & store them on this picture as wordsearch corner co-ords
        }

        private void btnNextImage_Click(object sender, EventArgs e)
        {
            //TODO: Save the markup on the current image

            //Get the next image
            currentBitmap = toProcess.Dequeue();

            //Resize the picture box to the 
            picBoxWordsearchImage.Image = currentBitmap.Key;

            updateLblToProcessLength();
        }

        /*
         * Private Helpers
         */
        private void updateLblToProcessLength()
        {
            lblToPorcessLength.Text = toProcess.Count.ToString();
        }
    }
}
