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
            this.lblChars = new System.Windows.Forms.Label();
            this.lblWords = new System.Windows.Forms.Label();
            this.rtbWords = new System.Windows.Forms.RichTextBox();
            this.lblWordsearchId = new System.Windows.Forms.Label();
            this.btnSaveWordsearch = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxWordsearchImage)).BeginInit();
            this.SuspendLayout();
            // 
            // rtbChars
            // 
            this.rtbChars.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbChars.Location = new System.Drawing.Point(15, 473);
            this.rtbChars.Name = "rtbChars";
            this.rtbChars.Size = new System.Drawing.Size(317, 330);
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
            this.picBoxWordsearchImage.Location = new System.Drawing.Point(12, 29);
            this.picBoxWordsearchImage.Name = "picBoxWordsearchImage";
            this.picBoxWordsearchImage.Size = new System.Drawing.Size(661, 421);
            this.picBoxWordsearchImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxWordsearchImage.TabIndex = 0;
            this.picBoxWordsearchImage.TabStop = false;
            // 
            // lblChars
            // 
            this.lblChars.AutoSize = true;
            this.lblChars.Location = new System.Drawing.Point(12, 453);
            this.lblChars.Name = "lblChars";
            this.lblChars.Size = new System.Drawing.Size(58, 13);
            this.lblChars.TabIndex = 4;
            this.lblChars.Text = "Characters";
            // 
            // lblWords
            // 
            this.lblWords.AutoSize = true;
            this.lblWords.Location = new System.Drawing.Point(339, 457);
            this.lblWords.Name = "lblWords";
            this.lblWords.Size = new System.Drawing.Size(73, 13);
            this.lblWords.TabIndex = 5;
            this.lblWords.Text = "Words to Find";
            // 
            // rtbWords
            // 
            this.rtbWords.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbWords.Location = new System.Drawing.Point(339, 474);
            this.rtbWords.Name = "rtbWords";
            this.rtbWords.Size = new System.Drawing.Size(191, 329);
            this.rtbWords.TabIndex = 6;
            this.rtbWords.Text = "";
            // 
            // lblWordsearchId
            // 
            this.lblWordsearchId.AutoSize = true;
            this.lblWordsearchId.Location = new System.Drawing.Point(11, 12);
            this.lblWordsearchId.Name = "lblWordsearchId";
            this.lblWordsearchId.Size = new System.Drawing.Size(82, 13);
            this.lblWordsearchId.TabIndex = 7;
            this.lblWordsearchId.Text = "Wordsearch ID ";
            // 
            // btnSaveWordsearch
            // 
            this.btnSaveWordsearch.Image = global::DataEntryGUI.Properties.Resources.save;
            this.btnSaveWordsearch.Location = new System.Drawing.Point(684, 106);
            this.btnSaveWordsearch.Name = "btnSaveWordsearch";
            this.btnSaveWordsearch.Size = new System.Drawing.Size(139, 71);
            this.btnSaveWordsearch.TabIndex = 31;
            this.btnSaveWordsearch.Text = "Save Wordsearch to Database";
            this.btnSaveWordsearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveWordsearch.UseVisualStyleBackColor = true;
            this.btnSaveWordsearch.Click += new System.EventHandler(this.btnSaveWordsearch_Click);
            // 
            // WordsearchMarkupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 971);
            this.Controls.Add(this.btnSaveWordsearch);
            this.Controls.Add(this.lblWordsearchId);
            this.Controls.Add(this.rtbWords);
            this.Controls.Add(this.lblWords);
            this.Controls.Add(this.lblChars);
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
        private System.Windows.Forms.Label lblChars;
        private System.Windows.Forms.Label lblWords;
        private System.Windows.Forms.RichTextBox rtbWords;
        private System.Windows.Forms.Label lblWordsearchId;
        private System.Windows.Forms.Button btnSaveWordsearch;
    }
}