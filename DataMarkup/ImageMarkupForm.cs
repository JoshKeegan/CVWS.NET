﻿/*
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
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AForge;
using AForge.Imaging;

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
        List<WordsearchImage> wordsearchImages; // For the images of word searches in the current image

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
            //Get the co-ords of the click
            MouseEventArgs mousePress = e as MouseEventArgs;
            int x = mousePress.X;
            int y = mousePress.Y;

            //Put the coordinates in the relevant box
            if(txtTopLeftX.Text == "" && txtTopLeftY.Text == "")
            {
                txtTopLeftX.Text = x.ToString();
                txtTopLeftY.Text = y.ToString();
            }
            else if(txtTopRightX.Text == "" && txtTopRightY.Text == "")
            {
                txtTopRightX.Text = x.ToString();
                txtTopRightY.Text = y.ToString();
            }
            else if(txtBottomRightX.Text == "" && txtBottomRightY.Text == "")
            {
                txtBottomRightX.Text = x.ToString();
                txtBottomRightY.Text = y.ToString();
            }
            else if(txtBottomLeftX.Text == "" && txtBottomLeftY.Text == "")
            {
                txtBottomLeftX.Text = x.ToString();
                txtBottomLeftY.Text = y.ToString();
            }
            else
            {
                MessageBox.Show("All points have been entered for this Wordsearch Image");
            }
        }

        private void btnNextImage_Click(object sender, EventArgs e)
        {
            //TODO: Save the markup on the current image (if we have just processed one)

            //If there is another image to get
            if(toProcess.Count > 0)
            {
                //Get the next image
                currentBitmap = toProcess.Dequeue();

                //Resize the picture box to the 
                picBoxWordsearchImage.Image = currentBitmap.Key;

                updateLblToProcessLength();
            }
            else //Otherwise there are no more images to be processed
            {
                picBoxWordsearchImage.Image = new Bitmap(1, 1);
            }
            
        }

        private void txtNumRows_TextChanged(object sender, EventArgs e)
        {
            drawRowsAndColsOnCurrentWordsearch();
        }

        private void txtNumCols_TextChanged(object sender, EventArgs e)
        {
            drawRowsAndColsOnCurrentWordsearch();
        }

        private void btnAddWordsearchImage_Click(object sender, EventArgs e)
        {

        }

        /*
         * Private Helpers
         */
        private void updateLblToProcessLength()
        {
            lblToPorcessLength.Text = toProcess.Count.ToString();
        }

        private void drawRowsAndColsOnCurrentWordsearch()
        {
            //Get Rows & Cols
            uint rows, cols;
            uint.TryParse(txtNumRows.Text, out rows);
            uint.TryParse(txtNumCols.Text, out cols);

            //Check we have all four coordinates of the wordsearch image
            if(txtTopLeftX.Text != "" && txtTopLeftY.Text != "" &&
                txtTopRightX.Text != "" && txtTopRightY.Text != "" &&
                txtBottomRightX.Text != "" && txtBottomRightY.Text != "" &&
                txtBottomLeftX.Text != "" && txtBottomLeftY.Text != "")
            {
                Bitmap drawnOn = drawRowsAndColsOnImage(currentBitmap.Key, 
                    getTopLeftCoordinate(), getTopRightCoordinate(), 
                    getBottomRightCoordinate(), getBottomLeftCoordinate(), 
                    rows, cols);
                picBoxWordsearchImage.Image = drawnOn;
            }
        }

        private Bitmap drawRowsAndColsOnImage(Bitmap img, IntPoint topLeft, 
            IntPoint topRight, IntPoint bottomRight, IntPoint bottomLeft, 
            uint rows, uint cols)
        {
            return drawRowsAndColsOnImage(img, topLeft, topRight, bottomRight, 
                bottomLeft, rows, cols, Color.Red);
        }

        private Bitmap drawRowsAndColsOnImage(Bitmap imgOrig, IntPoint topLeft, 
            IntPoint topRight, IntPoint bottomRight, IntPoint bottomLeft, 
            uint rows, uint cols, Color colour)
        {
            Bitmap img = new Bitmap(imgOrig);

            //Lock image for read write so we can alter it
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadWrite, img.PixelFormat);

            //Draw Lines at regular intervals for rows
            Drawing.Line(imgData, topLeft, topRight, colour);
            for(int i = 1; i <= rows; i++)
            {
                int leftSideRowBottomX = (int)(((bottomLeft.X - topLeft.X) / (double)rows) * i + topLeft.X);
                int leftSideRowBottomY = (int)(((bottomLeft.Y - topLeft.Y) / (double)rows) * i + topLeft.Y);
                IntPoint leftSideRowBottom = new IntPoint(leftSideRowBottomX, leftSideRowBottomY);

                int rightSideRowBottomX = (int)(((bottomRight.X - topRight.X) / (double)rows) * i + topRight.X);
                int rightSideRowbottomY = (int)(((bottomRight.Y - topRight.Y) / (double)rows) * i + topRight.Y);
                IntPoint rightSideRowBottom = new IntPoint(rightSideRowBottomX, rightSideRowbottomY);

                Drawing.Line(imgData, leftSideRowBottom, rightSideRowBottom, colour);
            }

            //Draw Lines at regular intervals for cols (same as rows, but rotate all of the points around -90 deg)
            Drawing.Line(imgData, bottomLeft, topLeft, colour);
            for (int i = 1; i <= cols; i++)
            {
                int bottomSideColRightX = (int)(((bottomRight.X - bottomLeft.X) / (double)cols) * i + bottomLeft.X);
                int bottomSideColRightY = (int)(((bottomRight.Y - bottomLeft.Y) / (double)cols) * i + bottomLeft.Y);
                IntPoint bottomSideColRight = new IntPoint(bottomSideColRightX, bottomSideColRightY);

                int topSideColRightX = (int)(((topRight.X - topLeft.X) / (double)cols) * i + topLeft.X);
                int topSideColRightY = (int)(((topRight.Y - topLeft.Y) / (double)cols) * i + topLeft.Y);
                IntPoint topSideColRight = new IntPoint(topSideColRightX, topSideColRightY);

                Drawing.Line(imgData, bottomSideColRight, topSideColRight, colour);
            }

            img.UnlockBits(imgData);
            return img;
        }

        private IntPoint getTopLeftCoordinate()
        {
            int x = int.Parse(txtTopLeftX.Text);
            int y = int.Parse(txtTopLeftY.Text);
            return new IntPoint(x, y);
        }

        private IntPoint getTopRightCoordinate()
        {
            int x = int.Parse(txtTopRightX.Text);
            int y = int.Parse(txtTopRightY.Text);
            return new IntPoint(x, y);
        }

        private IntPoint getBottomRightCoordinate()
        {
            int x = int.Parse(txtBottomRightX.Text);
            int y = int.Parse(txtBottomRightY.Text);
            return new IntPoint(x, y);
        }

        private IntPoint getBottomLeftCoordinate()
        {
            int x = int.Parse(txtBottomLeftX.Text);
            int y = int.Parse(txtBottomLeftY.Text);
            return new IntPoint(x, y);
        }
    }
}