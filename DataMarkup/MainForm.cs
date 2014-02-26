/*
 * Dissertation CV Wordsearch Solver
 * Data Markup
 * Main Form Class - Main GUI Window
 * By Josh Keegan 26/02/2014
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

namespace DataMarkup
{
    public partial class MainForm : Form
    {
        //Constants
        private const string RAW_IMAGE_DIR = "images";

        //Private vars
        private Queue<Bitmap> toProcess;

        public MainForm()
        {
            InitializeComponent();

            //Load in the meta data for images that are already processed
            ImageMarkupDatabase.LoadDatabase();

            //Load in the raw images to process
            List<Bitmap> bitmaps = ImageLoader.LoadAllImagesInSubdirs(RAW_IMAGE_DIR);
            toProcess = new Queue<Bitmap>(bitmaps);
        }

        private void picBoxWordsearchImage_Click(object sender, EventArgs e)
        {
            //TODO: Get the co-ords of the click & store them on this picture as wordsearch corner co-ords
        }
    }
}
