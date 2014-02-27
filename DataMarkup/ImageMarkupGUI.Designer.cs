namespace DataEntryGUI
{
    partial class ImageMarkupGUI
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
            this.picBoxWordsearchImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxWordsearchImage)).BeginInit();
            this.SuspendLayout();
            // 
            // picBoxWordsearchImage
            // 
            this.picBoxWordsearchImage.Location = new System.Drawing.Point(13, 13);
            this.picBoxWordsearchImage.Name = "picBoxWordsearchImage";
            this.picBoxWordsearchImage.Size = new System.Drawing.Size(1465, 840);
            this.picBoxWordsearchImage.TabIndex = 0;
            this.picBoxWordsearchImage.TabStop = false;
            this.picBoxWordsearchImage.Click += new System.EventHandler(this.picBoxWordsearchImage_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1503, 877);
            this.Controls.Add(this.picBoxWordsearchImage);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picBoxWordsearchImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picBoxWordsearchImage;
    }
}

