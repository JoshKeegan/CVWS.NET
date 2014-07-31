/*
 * Computer Vision Wordsearch Solver
 * Data Entry GUI
 * Main Form Class - Main GUI Window
 * By Josh Keegan 05/03/2013
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

namespace DataEntryGUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnImageMarkup_Click(object sender, EventArgs e)
        {
            //Start the form for marking up images
            ImageMarkupForm imageMarkupForm = new ImageMarkupForm();
            this.Hide();
            imageMarkupForm.ShowDialog();
            this.Show();
        }

        private void btnWordsearchMarkup_Click(object sender, EventArgs e)
        {
            //TODO: Start the form for marking up wordsearches
            WordsearchMarkupForm wordsearchMarkupForm = new WordsearchMarkupForm();
            this.Hide();
            wordsearchMarkupForm.ShowDialog();
            this.Show();
        }
    }
}
