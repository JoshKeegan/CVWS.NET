namespace DataEntryGUI
{
    partial class WordsearchMarkupForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtbChars = new System.Windows.Forms.RichTextBox();
            this.lblToProcessLength = new System.Windows.Forms.Label();
            this.btnNextWordsearch = new System.Windows.Forms.Button();
            this.picBoxWordsearchImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxWordsearchImage)).BeginInit();
            this.SuspendLayout();
            // 
            // rtbChars
            // 
            this.rtbChars.Location = new System.Drawing.Point(13, 441);
            this.rtbChars.Name = "rtbChars";
            this.rtbChars.Size = new System.Drawing.Size(661, 330);
            this.rtbChars.TabIndex = 1;
            this.rtbChars.Text = "";
            this.rtbChars.TextChanged += new System.EventHandler(this.rtbChars_TextChanged);
            // 
            // lblToProcessLength
            // 
            this.lblToProcessLength.AutoSize = true;
            this.lblToProcessLength.Location = new System.Drawing.Point(681, 13);
            this.lblToProcessLength.Name = "lblToProcessLength";
            this.lblToProcessLength.Size = new System.Drawing.Size(132, 13);
            this.lblToProcessLength.TabIndex = 2;
            this.lblToProcessLength.Text = " Wordsearches to Process";
            // 
            // btnNextWordsearch
            // 
            this.btnNextWordsearch.Image = global::DataEntryGUI.Properties.Resources.right;
            this.btnNextWordsearch.Location = new System.Drawing.Point(684, 29);
            this.btnNextWordsearch.Name = "btnNextWordsearch";
            this.btnNextWordsearch.Size = new System.Drawing.Size(82, 71);
            this.btnNextWordsearch.TabIndex = 3;
            this.btnNextWordsearch.Text = "Next Word search";
            this.btnNextWordsearch.UseVisualStyleBackColor = true;
            this.btnNextWordsearch.Click += new System.EventHandler(this.btnNextWordsearch_Click);
            // 
            // picBoxWordsearchImage
            // 
            this.picBoxWordsearchImage.Location = new System.Drawing.Point(13, 13);
            this.picBoxWordsearchImage.Name = "picBoxWordsearchImage";
            this.picBoxWordsearchImage.Size = new System.Drawing.Size(661, 421);
            this.picBoxWordsearchImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxWordsearchImage.TabIndex = 0;
            this.picBoxWordsearchImage.TabStop = false;
            // 
            // WordsearchMarkupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 971);
            this.Controls.Add(this.btnNextWordsearch);
            this.Controls.Add(this.lblToProcessLength);
            this.Controls.Add(this.rtbChars);
            this.Controls.Add(this.picBoxWordsearchImage);
            this.Name = "WordsearchMarkupForm";
            this.Text = "Word search Markup";
            ((System.ComponentModel.ISupportInitialize)(this.picBoxWordsearchImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picBoxWordsearchImage;
        private System.Windows.Forms.RichTextBox rtbChars;
        private System.Windows.Forms.Label lblToProcessLength;
        private System.Windows.Forms.Button btnNextWordsearch;
    }
}