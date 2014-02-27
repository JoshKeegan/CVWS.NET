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
            this.lblToPorcessLength = new System.Windows.Forms.Label();
            this.btnNextImage = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxWordsearchImage)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picBoxWordsearchImage
            // 
            this.picBoxWordsearchImage.Location = new System.Drawing.Point(3, 3);
            this.picBoxWordsearchImage.Name = "picBoxWordsearchImage";
            this.picBoxWordsearchImage.Size = new System.Drawing.Size(1783, 927);
            this.picBoxWordsearchImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picBoxWordsearchImage.TabIndex = 0;
            this.picBoxWordsearchImage.TabStop = false;
            this.picBoxWordsearchImage.Click += new System.EventHandler(this.picBoxWordsearchImage_Click);
            // 
            // lblToPorcessLength
            // 
            this.lblToPorcessLength.AutoSize = true;
            this.lblToPorcessLength.Location = new System.Drawing.Point(1807, 8);
            this.lblToPorcessLength.Name = "lblToPorcessLength";
            this.lblToPorcessLength.Size = new System.Drawing.Size(64, 13);
            this.lblToPorcessLength.TabIndex = 1;
            this.lblToPorcessLength.Text = " To Process";
            // 
            // btnNextImage
            // 
            this.btnNextImage.Image = global::DataEntryGUI.Properties.Resources.right;
            this.btnNextImage.Location = new System.Drawing.Point(1810, 25);
            this.btnNextImage.Name = "btnNextImage";
            this.btnNextImage.Size = new System.Drawing.Size(82, 71);
            this.btnNextImage.TabIndex = 2;
            this.btnNextImage.Text = "Next Image";
            this.btnNextImage.UseVisualStyleBackColor = true;
            this.btnNextImage.Click += new System.EventHandler(this.btnNextImage_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.picBoxWordsearchImage);
            this.panel1.Location = new System.Drawing.Point(12, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1789, 933);
            this.panel1.TabIndex = 3;
            // 
            // ImageMarkupGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 952);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnNextImage);
            this.Controls.Add(this.lblToPorcessLength);
            this.Name = "ImageMarkupGUI";
            this.Text = "Image Markup";
            ((System.ComponentModel.ISupportInitialize)(this.picBoxWordsearchImage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picBoxWordsearchImage;
        private System.Windows.Forms.Label lblToPorcessLength;
        private System.Windows.Forms.Button btnNextImage;
        private System.Windows.Forms.Panel panel1;
    }
}

