namespace DataEntryGUI
{
    partial class ImageMarkupForm
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
            this.btnAddWordsearchImage = new System.Windows.Forms.Button();
            this.lblTopLeft = new System.Windows.Forms.Label();
            this.lblCoordinates = new System.Windows.Forms.Label();
            this.txtTopLeftX = new System.Windows.Forms.TextBox();
            this.txtTopLeftY = new System.Windows.Forms.TextBox();
            this.txtTopRightY = new System.Windows.Forms.TextBox();
            this.txtTopRightX = new System.Windows.Forms.TextBox();
            this.lblTopRight = new System.Windows.Forms.Label();
            this.txtBottomRightY = new System.Windows.Forms.TextBox();
            this.txtBottomRightX = new System.Windows.Forms.TextBox();
            this.lblBottomRight = new System.Windows.Forms.Label();
            this.txtBottomLeftY = new System.Windows.Forms.TextBox();
            this.txtBottomLeftX = new System.Windows.Forms.TextBox();
            this.lblBottomLeft = new System.Windows.Forms.Label();
            this.lblNumRows = new System.Windows.Forms.Label();
            this.lblNumCols = new System.Windows.Forms.Label();
            this.txtNumRows = new System.Windows.Forms.TextBox();
            this.txtNumCols = new System.Windows.Forms.TextBox();
            this.lblWordsearchId = new System.Windows.Forms.Label();
            this.txtWordsearchId = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxWordsearchImage)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picBoxWordsearchImage
            // 
            this.picBoxWordsearchImage.Location = new System.Drawing.Point(3, 3);
            this.picBoxWordsearchImage.Name = "picBoxWordsearchImage";
            this.picBoxWordsearchImage.Size = new System.Drawing.Size(1483, 927);
            this.picBoxWordsearchImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picBoxWordsearchImage.TabIndex = 0;
            this.picBoxWordsearchImage.TabStop = false;
            this.picBoxWordsearchImage.Click += new System.EventHandler(this.picBoxWordsearchImage_Click);
            // 
            // lblToPorcessLength
            // 
            this.lblToPorcessLength.AutoSize = true;
            this.lblToPorcessLength.Location = new System.Drawing.Point(1504, 6);
            this.lblToPorcessLength.Name = "lblToPorcessLength";
            this.lblToPorcessLength.Size = new System.Drawing.Size(64, 13);
            this.lblToPorcessLength.TabIndex = 1;
            this.lblToPorcessLength.Text = " To Process";
            // 
            // btnNextImage
            // 
            this.btnNextImage.Image = global::DataEntryGUI.Properties.Resources.right;
            this.btnNextImage.Location = new System.Drawing.Point(1507, 23);
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
            this.panel1.Size = new System.Drawing.Size(1489, 933);
            this.panel1.TabIndex = 3;
            // 
            // btnAddWordsearchImage
            // 
            this.btnAddWordsearchImage.Location = new System.Drawing.Point(1507, 783);
            this.btnAddWordsearchImage.Name = "btnAddWordsearchImage";
            this.btnAddWordsearchImage.Size = new System.Drawing.Size(75, 48);
            this.btnAddWordsearchImage.TabIndex = 4;
            this.btnAddWordsearchImage.Text = "Add Word search Image";
            this.btnAddWordsearchImage.UseVisualStyleBackColor = true;
            this.btnAddWordsearchImage.Click += new System.EventHandler(this.btnAddWordsearchImage_Click);
            // 
            // lblTopLeft
            // 
            this.lblTopLeft.AutoSize = true;
            this.lblTopLeft.Location = new System.Drawing.Point(1507, 495);
            this.lblTopLeft.Name = "lblTopLeft";
            this.lblTopLeft.Size = new System.Drawing.Size(50, 13);
            this.lblTopLeft.TabIndex = 6;
            this.lblTopLeft.Text = "Top Left:";
            // 
            // lblCoordinates
            // 
            this.lblCoordinates.AutoSize = true;
            this.lblCoordinates.Location = new System.Drawing.Point(1507, 473);
            this.lblCoordinates.Name = "lblCoordinates";
            this.lblCoordinates.Size = new System.Drawing.Size(66, 13);
            this.lblCoordinates.TabIndex = 7;
            this.lblCoordinates.Text = "Coordinates:";
            // 
            // txtTopLeftX
            // 
            this.txtTopLeftX.Location = new System.Drawing.Point(1580, 492);
            this.txtTopLeftX.Name = "txtTopLeftX";
            this.txtTopLeftX.Size = new System.Drawing.Size(32, 20);
            this.txtTopLeftX.TabIndex = 8;
            // 
            // txtTopLeftY
            // 
            this.txtTopLeftY.Location = new System.Drawing.Point(1618, 492);
            this.txtTopLeftY.Name = "txtTopLeftY";
            this.txtTopLeftY.Size = new System.Drawing.Size(32, 20);
            this.txtTopLeftY.TabIndex = 9;
            // 
            // txtTopRightY
            // 
            this.txtTopRightY.Location = new System.Drawing.Point(1618, 518);
            this.txtTopRightY.Name = "txtTopRightY";
            this.txtTopRightY.Size = new System.Drawing.Size(32, 20);
            this.txtTopRightY.TabIndex = 12;
            // 
            // txtTopRightX
            // 
            this.txtTopRightX.Location = new System.Drawing.Point(1580, 518);
            this.txtTopRightX.Name = "txtTopRightX";
            this.txtTopRightX.Size = new System.Drawing.Size(32, 20);
            this.txtTopRightX.TabIndex = 11;
            // 
            // lblTopRight
            // 
            this.lblTopRight.AutoSize = true;
            this.lblTopRight.Location = new System.Drawing.Point(1507, 521);
            this.lblTopRight.Name = "lblTopRight";
            this.lblTopRight.Size = new System.Drawing.Size(57, 13);
            this.lblTopRight.TabIndex = 10;
            this.lblTopRight.Text = "Top Right:";
            // 
            // txtBottomRightY
            // 
            this.txtBottomRightY.Location = new System.Drawing.Point(1618, 544);
            this.txtBottomRightY.Name = "txtBottomRightY";
            this.txtBottomRightY.Size = new System.Drawing.Size(32, 20);
            this.txtBottomRightY.TabIndex = 15;
            // 
            // txtBottomRightX
            // 
            this.txtBottomRightX.Location = new System.Drawing.Point(1580, 544);
            this.txtBottomRightX.Name = "txtBottomRightX";
            this.txtBottomRightX.Size = new System.Drawing.Size(32, 20);
            this.txtBottomRightX.TabIndex = 14;
            // 
            // lblBottomRight
            // 
            this.lblBottomRight.AutoSize = true;
            this.lblBottomRight.Location = new System.Drawing.Point(1507, 547);
            this.lblBottomRight.Name = "lblBottomRight";
            this.lblBottomRight.Size = new System.Drawing.Size(71, 13);
            this.lblBottomRight.TabIndex = 13;
            this.lblBottomRight.Text = "Bottom Right:";
            // 
            // txtBottomLeftY
            // 
            this.txtBottomLeftY.Location = new System.Drawing.Point(1618, 570);
            this.txtBottomLeftY.Name = "txtBottomLeftY";
            this.txtBottomLeftY.Size = new System.Drawing.Size(32, 20);
            this.txtBottomLeftY.TabIndex = 18;
            // 
            // txtBottomLeftX
            // 
            this.txtBottomLeftX.Location = new System.Drawing.Point(1580, 570);
            this.txtBottomLeftX.Name = "txtBottomLeftX";
            this.txtBottomLeftX.Size = new System.Drawing.Size(32, 20);
            this.txtBottomLeftX.TabIndex = 17;
            // 
            // lblBottomLeft
            // 
            this.lblBottomLeft.AutoSize = true;
            this.lblBottomLeft.Location = new System.Drawing.Point(1507, 573);
            this.lblBottomLeft.Name = "lblBottomLeft";
            this.lblBottomLeft.Size = new System.Drawing.Size(64, 13);
            this.lblBottomLeft.TabIndex = 16;
            this.lblBottomLeft.Text = "Bottom Left:";
            // 
            // lblNumRows
            // 
            this.lblNumRows.AutoSize = true;
            this.lblNumRows.Location = new System.Drawing.Point(1513, 615);
            this.lblNumRows.Name = "lblNumRows";
            this.lblNumRows.Size = new System.Drawing.Size(37, 13);
            this.lblNumRows.TabIndex = 19;
            this.lblNumRows.Text = "Rows:";
            // 
            // lblNumCols
            // 
            this.lblNumCols.AutoSize = true;
            this.lblNumCols.Location = new System.Drawing.Point(1513, 641);
            this.lblNumCols.Name = "lblNumCols";
            this.lblNumCols.Size = new System.Drawing.Size(50, 13);
            this.lblNumCols.TabIndex = 20;
            this.lblNumCols.Text = "Columns:";
            // 
            // txtNumRows
            // 
            this.txtNumRows.Location = new System.Drawing.Point(1580, 612);
            this.txtNumRows.Name = "txtNumRows";
            this.txtNumRows.Size = new System.Drawing.Size(32, 20);
            this.txtNumRows.TabIndex = 21;
            this.txtNumRows.TextChanged += new System.EventHandler(this.txtNumRows_TextChanged);
            // 
            // txtNumCols
            // 
            this.txtNumCols.Location = new System.Drawing.Point(1580, 638);
            this.txtNumCols.Name = "txtNumCols";
            this.txtNumCols.Size = new System.Drawing.Size(32, 20);
            this.txtNumCols.TabIndex = 22;
            this.txtNumCols.TextChanged += new System.EventHandler(this.txtNumCols_TextChanged);
            // 
            // lblWordsearchId
            // 
            this.lblWordsearchId.AutoSize = true;
            this.lblWordsearchId.Location = new System.Drawing.Point(1527, 693);
            this.lblWordsearchId.Name = "lblWordsearchId";
            this.lblWordsearchId.Size = new System.Drawing.Size(85, 13);
            this.lblWordsearchId.TabIndex = 23;
            this.lblWordsearchId.Text = "Word search ID:";
            // 
            // txtWordsearchId
            // 
            this.txtWordsearchId.Location = new System.Drawing.Point(1618, 690);
            this.txtWordsearchId.Name = "txtWordsearchId";
            this.txtWordsearchId.Size = new System.Drawing.Size(100, 20);
            this.txtWordsearchId.TabIndex = 24;
            // 
            // ImageMarkupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 952);
            this.Controls.Add(this.txtWordsearchId);
            this.Controls.Add(this.lblWordsearchId);
            this.Controls.Add(this.txtNumCols);
            this.Controls.Add(this.txtNumRows);
            this.Controls.Add(this.lblNumCols);
            this.Controls.Add(this.lblNumRows);
            this.Controls.Add(this.txtBottomLeftY);
            this.Controls.Add(this.txtBottomLeftX);
            this.Controls.Add(this.lblBottomLeft);
            this.Controls.Add(this.txtBottomRightY);
            this.Controls.Add(this.txtBottomRightX);
            this.Controls.Add(this.lblBottomRight);
            this.Controls.Add(this.txtTopRightY);
            this.Controls.Add(this.txtTopRightX);
            this.Controls.Add(this.lblTopRight);
            this.Controls.Add(this.txtTopLeftY);
            this.Controls.Add(this.txtTopLeftX);
            this.Controls.Add(this.lblCoordinates);
            this.Controls.Add(this.lblTopLeft);
            this.Controls.Add(this.btnAddWordsearchImage);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnNextImage);
            this.Controls.Add(this.lblToPorcessLength);
            this.Name = "ImageMarkupForm";
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
        private System.Windows.Forms.Button btnAddWordsearchImage;
        private System.Windows.Forms.Label lblTopLeft;
        private System.Windows.Forms.Label lblCoordinates;
        private System.Windows.Forms.TextBox txtTopLeftX;
        private System.Windows.Forms.TextBox txtTopLeftY;
        private System.Windows.Forms.TextBox txtTopRightY;
        private System.Windows.Forms.TextBox txtTopRightX;
        private System.Windows.Forms.Label lblTopRight;
        private System.Windows.Forms.TextBox txtBottomRightY;
        private System.Windows.Forms.TextBox txtBottomRightX;
        private System.Windows.Forms.Label lblBottomRight;
        private System.Windows.Forms.TextBox txtBottomLeftY;
        private System.Windows.Forms.TextBox txtBottomLeftX;
        private System.Windows.Forms.Label lblBottomLeft;
        private System.Windows.Forms.Label lblNumRows;
        private System.Windows.Forms.Label lblNumCols;
        private System.Windows.Forms.TextBox txtNumRows;
        private System.Windows.Forms.TextBox txtNumCols;
        private System.Windows.Forms.Label lblWordsearchId;
        private System.Windows.Forms.TextBox txtWordsearchId;
    }
}

