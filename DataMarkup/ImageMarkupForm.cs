/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Data Entry GUI
 * Image Markup Form class
 * By Josh Keegan 26/02/2014
 * Last Edit 19/05/2014
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
using AForge.Imaging.Filters;

using ImageMarkup;
using ImageMarkup.Exceptions;
using libCVWS;
using libCVWS.Imaging;
using libCVWS.BaseObjectExtensions;
using System.IO;

namespace DataEntryGUI
{
    public partial class ImageMarkupForm : Form
    {
        //Constants
        private const string RAW_IMAGE_DIR = ImageMarkupDatabase.DATA_DIRECTORY_PATH + "images";

        //Private vars
        private string defaultLblToProcessLength;
        private Queue<KeyValuePair<Bitmap, string>> toProcess; //Store the hash along with the Bitmap so we don't have to recompute it later
        private KeyValuePair<Bitmap, string> currentBitmap;
        private Bitmap bitmapToShow = null;
        private List<WordsearchImage> wordsearchImages; // For the images of word searches in the current image

        public ImageMarkupForm()
        {
            InitializeComponent();

            //Store the lblToProcessLength default label so the number can be prepended
            defaultLblToProcessLength = lblToProcessLength.Text;

            //If the Image Markup Database hasn't already been loaded
            if(!ImageMarkupDatabase.Loaded)
            {
                //Load in the meta data for images that are already processed
                ImageMarkupDatabase.LoadDatabase();
            }

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
               else //Otherwise we don't need this Bitmap any more, dispose of it
               {
                   b.Dispose();
               }
            }

            updateLblToProcessLength();

            //Add Event handlers to the coordinate Text Boxes
            txtTopLeftX.TextChanged += new System.EventHandler(txtCoordinates_TextChanges);
            txtTopLeftY.TextChanged += new System.EventHandler(txtCoordinates_TextChanges);
            txtTopRightX.TextChanged += new System.EventHandler(txtCoordinates_TextChanges);
            txtTopRightY.TextChanged += new System.EventHandler(txtCoordinates_TextChanges);
            txtBottomRightX.TextChanged += new System.EventHandler(txtCoordinates_TextChanges);
            txtBottomRightY.TextChanged += new System.EventHandler(txtCoordinates_TextChanges);
            txtBottomLeftX.TextChanged += new System.EventHandler(txtCoordinates_TextChanges);
            txtBottomLeftY.TextChanged += new System.EventHandler(txtCoordinates_TextChanges);
        }

        private void picBoxImage_Click(object sender, EventArgs e)
        {
            //If we're showing an image to be entering coordinates for
            if (!currentBitmap.Equals(default(KeyValuePair<Bitmap, string>)))
            {
                //Get the co-ords of the click
                MouseEventArgs mousePress = e as MouseEventArgs;
                int x = mousePress.X;
                int y = mousePress.Y;

                //Convert the x & y values to image coordinate space
                int imgX = (int)(x * ((double)bitmapToShow.Width / picBoxImage.Width));
                int imgY = (int)(y * ((double)bitmapToShow.Height / picBoxImage.Height));

                //Put the coordinates in the relevant box
                if (txtTopLeftX.Text == "" && txtTopLeftY.Text == "")
                {
                    txtTopLeftX.Text = imgX.ToString();
                    txtTopLeftY.Text = imgY.ToString();
                }
                else if (txtTopRightX.Text == "" && txtTopRightY.Text == "")
                {
                    txtTopRightX.Text = imgX.ToString();
                    txtTopRightY.Text = imgY.ToString();
                }
                else if (txtBottomRightX.Text == "" && txtBottomRightY.Text == "")
                {
                    txtBottomRightX.Text = imgX.ToString();
                    txtBottomRightY.Text = imgY.ToString();
                }
                else if (txtBottomLeftX.Text == "" && txtBottomLeftY.Text == "")
                {
                    txtBottomLeftX.Text = imgX.ToString();
                    txtBottomLeftY.Text = imgY.ToString();
                }
                else
                {
                    MessageBox.Show("All points have been entered for this Wordsearch Image");
                }
            }
        }

        private void btnNextImage_Click(object sender, EventArgs e)
        {
            //If there is a Bitmap to dispose of, dispose of the current Bitmap
            if(!currentBitmap.Equals(default(KeyValuePair<Bitmap, string>)))
            {
                currentBitmap.Key.Dispose();
            }

            //If we're currently displaying the enlarged wordsearch image, switch back to the main image view
            if(picBoxWordsearchImageLarge.Visible)
            {
                picBoxImage.Visible = true;
                picBoxWordsearchImageLarge.Visible = false;
            }

            //If there is another image to get
            if(toProcess.Count > 0)
            {
                //Get the next image
                currentBitmap = toProcess.Dequeue();

                //Reset any variables specific to the previous image
                wordsearchImages = new List<WordsearchImage>();

                //If there is a bitmapToShow to be disposed of, dispose of it
                if(bitmapToShow != null)
                {
                    bitmapToShow.Dispose();
                }

                //Generate the Bitmap to be shown
                bitmapToShow = drawWordsearchImagesOnCurrentWordsearch();

                //Show the bitmap
                picBoxImage.Image = bitmapToShow;

                updateLblToProcessLength();

                //Reset all fields
                resetFields();
            }
            else //Otherwise there are no more images to be processed
            {
                picBoxImage.Visible = false;
            }
            
        }

        private void txtNumRows_TextChanged(object sender, EventArgs e)
        {
            //Redraw the grid around the currend Wordsearch Image being entered with the newly modified data
            drawRowsAndColsOnCurrentWordsearch();
        }

        private void txtNumCols_TextChanged(object sender, EventArgs e)
        {
            drawRowsAndColsOnCurrentWordsearch();
        }

        private void txtCoordinates_TextChanges(object sender, EventArgs e)
        {
            drawRowsAndColsOnCurrentWordsearch();
        }

        private void txtWordsearchId_TextChanged(object sender, EventArgs e)
        {
            //Check if we already have data for this wordsearch
            if (ImageMarkupDatabase.ContainsWordsearch(txtWordsearchId.Text))
            {
                Wordsearch wordsearch = ImageMarkupDatabase.GetWordsearch(txtWordsearchId.Text);

                //Set the rows and cols
                txtNumRows.Text = wordsearch.Rows.ToString();
                txtNumCols.Text = wordsearch.Cols.ToString();
            }
            //Don't have the wordsearch, check for wordsearch images with the same ID
            else
            {
                try
                {
                    WordsearchImage wordsearchImage = ImageMarkupDatabase.GetWordsearchImage(txtWordsearchId.Text);

                    //Set the rows and cols
                    txtNumRows.Text = wordsearchImage.Rows.ToString();
                    txtNumCols.Text = wordsearchImage.Cols.ToString();
                }
                catch (DataNotFoundException) { }
            }
        }

        private void btnAddWordsearchImage_Click(object sender, EventArgs e)
        {
            //Check that we currently have an image on screen
            bool showingBitmap = !currentBitmap.Equals(default(KeyValuePair<Bitmap, string>));

            if(showingBitmap)
            {
                //Fetch all of the data for the Wordsearch Image & Validate it

                //Coordinates
                IntPoint topLeft = new IntPoint(); //Must have defaults to compile
                IntPoint topRight = new IntPoint();
                IntPoint bottomRight = new IntPoint();
                IntPoint bottomLeft = new IntPoint();
                bool validCoords = false;
                try
                {
                    topLeft = getTopLeftCoordinate();
                    topRight = getTopRightCoordinate();
                    bottomRight = getBottomRightCoordinate();
                    bottomLeft = getBottomLeftCoordinate();

                    //Check that the values we have received are in bounds
                    IntPoint[] coords = new IntPoint[4];
                    coords[0] = topLeft; //Winding Order: Clockwise starting in top left
                    coords[1] = topRight;
                    coords[2] = bottomRight;
                    coords[3] = bottomLeft;

                    foreach (IntPoint p in coords)
                    {
                        if (p.X < 0 || p.Y < 0 ||
                            p.X >= currentBitmap.Key.Width || p.Y >= currentBitmap.Key.Height)
                        {
                            throw new Exception("Point outside of Bitmap Bounds");
                        }
                    }

                    validCoords = true;
                }
                catch
                {
                    MessageBox.Show("Error parsing coordinates: please correct them and try again");
                }

                //If the coordinates are valid, get the next lot of data
                if (validCoords)
                {
                    //Num rows & cols
                    uint rows, cols;
                    bool parsedRows = uint.TryParse(txtNumRows.Text, out rows);
                    bool parsedCols = uint.TryParse(txtNumCols.Text, out cols);
                    bool validRowsAndCols = parsedRows && parsedCols && rows != 0 && cols != 0;

                    if (validRowsAndCols)
                    {
                        //Meta Data
                        Dictionary<string, string> metaData = new Dictionary<string, string>();
                        bool validMetaData = true;
                        for (int i = 0; i < dataGridViewWordsearchImageMetaData.Rows.Count; i++)
                        {
                            DataGridViewCell keyCell = dataGridViewWordsearchImageMetaData.Rows[i].Cells[0];
                            //If this cell has a value (i.e. is not the blank cell auto-inserted at the end)
                            if (keyCell.Value != null)
                            {
                                string key = keyCell.Value.ToString().Trim();

                                //Check the key isn't blank
                                if (key != "")
                                {
                                    //Check the key isn't already taken
                                    if (!metaData.ContainsKey(key))
                                    {
                                        DataGridViewCell valueCell = dataGridViewWordsearchImageMetaData.Rows[i].Cells[1];
                                        //Check that this has a value, if not make it empty
                                        string value;
                                        if(valueCell.Value == null)
                                        {
                                            value = "";
                                        }
                                        else
                                        {
                                            value = valueCell.Value.ToString();
                                        }

                                        metaData.Add(key, value);
                                    }
                                    else //Otherwise the key is already taken
                                    {
                                        validMetaData = false;
                                        MessageBox.Show("Invalid Meta Data: You cannot use the same key twice (" + key + ")");
                                        break;
                                    }
                                }
                                else //Otherwise the key is blank
                                {
                                    validMetaData = false;
                                    MessageBox.Show("Invalid Meta Data: You cannot have a blank Key");
                                    break;
                                }
                            }
                        }

                        //If the Meta Data is valid, we have everything necessary to make a WordsearchImage
                        if (validMetaData)
                        {
                            //Make a WordsearchImage
                            string fromImageHash = currentBitmap.Value;

                            WordsearchImage wordsearchImage;

                            //Check for optional field Wordsearch ID
                            string wordsearchId = txtWordsearchId.Text.Trim();
                            if (wordsearchId != "")
                            {
                                wordsearchImage = new WordsearchImage(topLeft, topRight, bottomRight, bottomLeft, rows, cols, metaData, fromImageHash, wordsearchId);
                            }
                            else //Otherwise the optional Wordsearch ID field hasn't been filled in
                            {
                                wordsearchImage = new WordsearchImage(topLeft, topRight, bottomRight, bottomLeft, rows, cols, metaData, fromImageHash);
                            }
                            wordsearchImages.Add(wordsearchImage);

                            //Reset the fields ready for the next WordsearchImage to be entered
                            resetWordsearchImageFields();

                            //If the image being displayed isn't being used elsewhere, dispose of it
                            if(bitmapToShow != picBoxImage.Image)
                            {
                                picBoxImage.Image.Dispose();
                            }

                            //Draw all of the WordsearchImages for this Image onto the original bitmap and display it
                            bitmapToShow = drawWordsearchImagesOnCurrentWordsearch();
                            picBoxImage.Image = bitmapToShow;

                            //If we were showing an enlarged wordsearch image, switch back to the main image
                            if(picBoxWordsearchImageLarge.Visible)
                            {
                                picBoxImage.Visible = true;
                                picBoxWordsearchImageLarge.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error parsing number of rows and cols: please correct them and try again");
                    }
                }
            }
            else //Otherwise we aren't showing a bitmap
            {
                MessageBox.Show("No Image currently on screen to add a Word search Image to");
            }
        }

        private void btnSaveImageMetaData_Click(object sender, EventArgs e)
        {
            //Check that we currently have an image on screen
            bool showingBitmap = !currentBitmap.Equals(default(KeyValuePair<Bitmap, string>));

            if(showingBitmap)
            {
                //Validate all of the data
                Dictionary<string, string> metaData = new Dictionary<string, string>();
                bool validMetaData = true;
                for (int i = 0; i < dataGridViewImageMetaData.Rows.Count; i++)
                {
                    DataGridViewCell keyCell = dataGridViewImageMetaData.Rows[i].Cells[0];
                    //If this cell has a value (i.e. is not the blank cell auto-inserted at the end)
                    if (keyCell.Value != null)
                    {
                        string key = keyCell.Value.ToString().Trim();

                        //Check the key isn't blank
                        if (key != "")
                        {
                            //Check the key isn't already taken
                            if (!metaData.ContainsKey(key))
                            {
                                DataGridViewCell valueCell = dataGridViewImageMetaData.Rows[i].Cells[1];
                                //Check that this cell has a value, if not make it empty
                                string value;
                                if (valueCell.Value == null)
                                {
                                    value = "";
                                }
                                else
                                {
                                    value = valueCell.Value.ToString();
                                }

                                metaData.Add(key, value);
                            }
                            else //Otherwise the key is already taken
                            {
                                validMetaData = false;
                                MessageBox.Show("Invalid Meta Data: You cannot use the same key twice (" + key + ")");
                                break;
                            }
                        }
                        else //Otherwise the key is blank
                        {
                            validMetaData = false;
                            MessageBox.Show("Invalid Meta Data: You cannot have a blank key");
                            break;
                        }
                    }
                }

                if (validMetaData)
                {
                    //Get the values we already have stored and don't need to be validated (as they aren't from the user)
                    string path = currentBitmap.Key.Tag as string;
                    string hash = currentBitmap.Value;

                    //Make the path relative to the Image Markup Data directory
                    string absolutePath = Path.GetFullPath(path);
                    string absoluteDataPath = Path.GetFullPath(ImageMarkupDatabase.DATA_DIRECTORY_PATH);
                    string pathRelToDataDir = Paths.GenerateRelativePath(absoluteDataPath, absolutePath);

                    ImageMarkup.Image image = new ImageMarkup.Image(pathRelToDataDir, hash, wordsearchImages.ToArray(), metaData);

                    //If this image is already in the database, remove it
                    if(ImageMarkupDatabase.ContainsImage(hash))
                    {
                        ImageMarkupDatabase.RemoveImage(hash);
                    }

                    //Store the Image object in the database and write it out
                    ImageMarkupDatabase.AddImage(hash, image);
                    ImageMarkupDatabase.WriteDatabase();

                    //Show the image of the data that's just been written (so as to avoid the possibility of people forgetting to add the last WordsearchImage)

                    //If the image currently being displayed won't be used elsewhere, the dispose of it
                    if (picBoxImage.Image != bitmapToShow)
                    {
                        picBoxImage.Image.Dispose();
                    }
                    picBoxImage.Image = bitmapToShow;
                }
            }
            else //Otherwise we aren't showing a bitmap
            {
                MessageBox.Show("No Image currently on screen to save data for");
            }
        }
        private void picBoxWordsearchImage_Click(object sender, EventArgs e)
        {
            if(picBoxWordsearchImage.Image != null)
            {
                //Show the wordsearch image in the big box
                picBoxWordsearchImageLarge.Image = picBoxWordsearchImage.Image;
                picBoxWordsearchImageLarge.Visible = true;
                picBoxImage.Visible = false;
            }
        }

        private void pictureBoxWordsearchImageLarge_Click(object sender, EventArgs e)
        {
            //Hide the large wordsearch image box
            picBoxImage.Visible = true;
            picBoxWordsearchImageLarge.Visible = false;
        }

        /*
         * Private Helpers
         */
        private void updateLblToProcessLength()
        {
            lblToProcessLength.Text = toProcess.Count.ToString() + defaultLblToProcessLength;
        }

        private void resetFields()
        {
            resetImageFields();
            resetWordsearchImageFields();
        }

        private void resetImageFields()
        {
            dataGridViewImageMetaData.Rows.Clear();
        }

        private void resetWordsearchImageFields()
        {
            txtTopLeftX.Text = "";
            txtTopLeftY.Text = "";
            txtTopRightX.Text = "";
            txtTopRightY.Text = "";
            txtBottomRightX.Text = "";
            txtBottomRightY.Text = "";
            txtBottomLeftX.Text = "";
            txtBottomLeftY.Text = "";

            txtNumRows.Text = "";
            txtNumCols.Text = "";
            txtWordsearchId.Text = "";

            dataGridViewWordsearchImageMetaData.Rows.Clear();

            if(picBoxWordsearchImage.Image != null)
            {
                picBoxWordsearchImage.Image.Dispose();
                picBoxWordsearchImage.Image = null;
            }
        }

        private Bitmap drawWordsearchImagesOnCurrentWordsearch()
        {
            Bitmap img = currentBitmap.Key.DeepCopy();

            //Lock image for read write so we can alter it
            BitmapData imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadWrite, img.PixelFormat);

            foreach(WordsearchImage wordsearchImage in wordsearchImages)
            {
                Drawing.Polygon(imgData, wordsearchImage.Coordinates.ToList(), Color.Red);
            }

            img.UnlockBits(imgData);
            return img;
        }

        private void drawRowsAndColsOnCurrentWordsearch()
        {
            //If there is a currently loaded bitmap to draw on
            if(bitmapToShow != null)
            {
                //Get Rows & Cols
                uint rows, cols;
                uint.TryParse(txtNumRows.Text, out rows);
                uint.TryParse(txtNumCols.Text, out cols);

                //Check we have all four coordinates of the wordsearch image
                if (txtTopLeftX.Text != "" && txtTopLeftY.Text != "" &&
                    txtTopRightX.Text != "" && txtTopRightY.Text != "" &&
                    txtBottomRightX.Text != "" && txtBottomRightY.Text != "" &&
                    txtBottomLeftX.Text != "" && txtBottomLeftY.Text != "")
                {
                    List<IntPoint> corners = new List<IntPoint>(4);
                    try
                    {
                        corners.Add(getTopLeftCoordinate());
                        corners.Add(getTopRightCoordinate());
                        corners.Add(getBottomRightCoordinate());
                        corners.Add(getBottomLeftCoordinate());
                    }
                    catch (FormatException) 
                    {
                        //There was an error parsing one of the coordinates
                        MessageBox.Show("Unable to parse coordinates as integers, please check they are valid");
                        return;
                    }
                    
                    //Draw the bounding box onto the main image
                    Bitmap drawnOn = bitmapToShow.DeepCopy();
                    DrawShapes.PolygonInPlace(drawnOn, corners, Color.Red);

                    //If there is a Bitmap which will become unused in memory, dispose of it
                    if(picBoxImage.Image != bitmapToShow)
                    {
                        picBoxImage.Image.Dispose();
                    }
                    picBoxImage.Image = drawnOn;

                    //Extract the wordsearch image & draw the grid on it
                    QuadrilateralTransformation quadTransform = new QuadrilateralTransformation(corners, 
                        picBoxWordsearchImage.Width, picBoxWordsearchImage.Height);

                    Bitmap transformed = quadTransform.Apply(currentBitmap.Key);
                    DrawGrid.GridInPlace(transformed, (int)rows, (int)cols);

                    //If there is a Bitmap which will become unused in memory, dispose if it
                    if(picBoxWordsearchImage.Image != null)
                    {
                        picBoxWordsearchImage.Image.Dispose();
                    }

                    picBoxWordsearchImage.Image = transformed;

                    //If we're currently showing the picture box image in the larger display, show it there too
                    if(picBoxWordsearchImageLarge.Visible)
                    {
                        picBoxWordsearchImageLarge.Image = transformed;
                    }
                }
            }
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
