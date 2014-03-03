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
            this.components = new System.ComponentModel.Container();
            this.lblToPorcessLength = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picBoxWordsearchImageLarge = new System.Windows.Forms.PictureBox();
            this.picBoxImage = new System.Windows.Forms.PictureBox();
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
            this.dataGridViewWordsearchImageMetaData = new System.Windows.Forms.DataGridView();
            this.wordsearchImageMetaDataColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wordsearchImageMetaDataColValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblWordsearchImageMetaData = new System.Windows.Forms.Label();
            this.lblWordsearchImageData = new System.Windows.Forms.Label();
            this.dataGridViewImageMetaData = new System.Windows.Forms.DataGridView();
            this.imageDataColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageDataColValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblImageMetaData = new System.Windows.Forms.Label();
            this.btnSaveImageMetaData = new System.Windows.Forms.Button();
            this.btnNextImage = new System.Windows.Forms.Button();
            this.picBoxWordsearchImage = new System.Windows.Forms.PictureBox();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxWordsearchImageLarge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWordsearchImageMetaData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImageMetaData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxWordsearchImage)).BeginInit();
            this.SuspendLayout();
            // 
            // lblToPorcessLength
            // 
            this.lblToPorcessLength.AutoSize = true;
            this.lblToPorcessLength.Location = new System.Drawing.Point(1504, 6);
            this.lblToPorcessLength.Name = "lblToPorcessLength";
            this.lblToPorcessLength.Size = new System.Drawing.Size(97, 13);
            this.lblToPorcessLength.TabIndex = 1;
            this.lblToPorcessLength.Text = " Images to Process";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.picBoxWordsearchImageLarge);
            this.panel1.Controls.Add(this.picBoxImage);
            this.panel1.Location = new System.Drawing.Point(12, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1489, 933);
            this.panel1.TabIndex = 3;
            // 
            // picBoxWordsearchImageLarge
            // 
            this.picBoxWordsearchImageLarge.Location = new System.Drawing.Point(3, 3);
            this.picBoxWordsearchImageLarge.Name = "picBoxWordsearchImageLarge";
            this.picBoxWordsearchImageLarge.Size = new System.Drawing.Size(1483, 927);
            this.picBoxWordsearchImageLarge.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxWordsearchImageLarge.TabIndex = 32;
            this.picBoxWordsearchImageLarge.TabStop = false;
            this.toolTips.SetToolTip(this.picBoxWordsearchImageLarge, "Click to return to main Image");
            this.picBoxWordsearchImageLarge.Visible = false;
            this.picBoxWordsearchImageLarge.Click += new System.EventHandler(this.pictureBoxWordsearchImageLarge_Click);
            // 
            // picBoxImage
            // 
            this.picBoxImage.Location = new System.Drawing.Point(3, 3);
            this.picBoxImage.Name = "picBoxImage";
            this.picBoxImage.Size = new System.Drawing.Size(1483, 927);
            this.picBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picBoxImage.TabIndex = 0;
            this.picBoxImage.TabStop = false;
            this.toolTips.SetToolTip(this.picBoxImage, "Click to add wordsearch coordinate. Winding order: top left first, clockwise");
            this.picBoxImage.Click += new System.EventHandler(this.picBoxImage_Click);
            // 
            // btnAddWordsearchImage
            // 
            this.btnAddWordsearchImage.Location = new System.Drawing.Point(1811, 657);
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
            this.lblTopLeft.Location = new System.Drawing.Point(1510, 309);
            this.lblTopLeft.Name = "lblTopLeft";
            this.lblTopLeft.Size = new System.Drawing.Size(50, 13);
            this.lblTopLeft.TabIndex = 6;
            this.lblTopLeft.Text = "Top Left:";
            // 
            // lblCoordinates
            // 
            this.lblCoordinates.AutoSize = true;
            this.lblCoordinates.Location = new System.Drawing.Point(1510, 287);
            this.lblCoordinates.Name = "lblCoordinates";
            this.lblCoordinates.Size = new System.Drawing.Size(66, 13);
            this.lblCoordinates.TabIndex = 7;
            this.lblCoordinates.Text = "Coordinates:";
            // 
            // txtTopLeftX
            // 
            this.txtTopLeftX.Location = new System.Drawing.Point(1583, 306);
            this.txtTopLeftX.Name = "txtTopLeftX";
            this.txtTopLeftX.Size = new System.Drawing.Size(32, 20);
            this.txtTopLeftX.TabIndex = 8;
            // 
            // txtTopLeftY
            // 
            this.txtTopLeftY.Location = new System.Drawing.Point(1621, 306);
            this.txtTopLeftY.Name = "txtTopLeftY";
            this.txtTopLeftY.Size = new System.Drawing.Size(32, 20);
            this.txtTopLeftY.TabIndex = 9;
            // 
            // txtTopRightY
            // 
            this.txtTopRightY.Location = new System.Drawing.Point(1621, 332);
            this.txtTopRightY.Name = "txtTopRightY";
            this.txtTopRightY.Size = new System.Drawing.Size(32, 20);
            this.txtTopRightY.TabIndex = 12;
            // 
            // txtTopRightX
            // 
            this.txtTopRightX.Location = new System.Drawing.Point(1583, 332);
            this.txtTopRightX.Name = "txtTopRightX";
            this.txtTopRightX.Size = new System.Drawing.Size(32, 20);
            this.txtTopRightX.TabIndex = 11;
            // 
            // lblTopRight
            // 
            this.lblTopRight.AutoSize = true;
            this.lblTopRight.Location = new System.Drawing.Point(1510, 335);
            this.lblTopRight.Name = "lblTopRight";
            this.lblTopRight.Size = new System.Drawing.Size(57, 13);
            this.lblTopRight.TabIndex = 10;
            this.lblTopRight.Text = "Top Right:";
            // 
            // txtBottomRightY
            // 
            this.txtBottomRightY.Location = new System.Drawing.Point(1621, 358);
            this.txtBottomRightY.Name = "txtBottomRightY";
            this.txtBottomRightY.Size = new System.Drawing.Size(32, 20);
            this.txtBottomRightY.TabIndex = 15;
            // 
            // txtBottomRightX
            // 
            this.txtBottomRightX.Location = new System.Drawing.Point(1583, 358);
            this.txtBottomRightX.Name = "txtBottomRightX";
            this.txtBottomRightX.Size = new System.Drawing.Size(32, 20);
            this.txtBottomRightX.TabIndex = 14;
            // 
            // lblBottomRight
            // 
            this.lblBottomRight.AutoSize = true;
            this.lblBottomRight.Location = new System.Drawing.Point(1510, 361);
            this.lblBottomRight.Name = "lblBottomRight";
            this.lblBottomRight.Size = new System.Drawing.Size(71, 13);
            this.lblBottomRight.TabIndex = 13;
            this.lblBottomRight.Text = "Bottom Right:";
            // 
            // txtBottomLeftY
            // 
            this.txtBottomLeftY.Location = new System.Drawing.Point(1621, 384);
            this.txtBottomLeftY.Name = "txtBottomLeftY";
            this.txtBottomLeftY.Size = new System.Drawing.Size(32, 20);
            this.txtBottomLeftY.TabIndex = 18;
            // 
            // txtBottomLeftX
            // 
            this.txtBottomLeftX.Location = new System.Drawing.Point(1583, 384);
            this.txtBottomLeftX.Name = "txtBottomLeftX";
            this.txtBottomLeftX.Size = new System.Drawing.Size(32, 20);
            this.txtBottomLeftX.TabIndex = 17;
            // 
            // lblBottomLeft
            // 
            this.lblBottomLeft.AutoSize = true;
            this.lblBottomLeft.Location = new System.Drawing.Point(1510, 387);
            this.lblBottomLeft.Name = "lblBottomLeft";
            this.lblBottomLeft.Size = new System.Drawing.Size(64, 13);
            this.lblBottomLeft.TabIndex = 16;
            this.lblBottomLeft.Text = "Bottom Left:";
            // 
            // lblNumRows
            // 
            this.lblNumRows.AutoSize = true;
            this.lblNumRows.Location = new System.Drawing.Point(1516, 413);
            this.lblNumRows.Name = "lblNumRows";
            this.lblNumRows.Size = new System.Drawing.Size(37, 13);
            this.lblNumRows.TabIndex = 19;
            this.lblNumRows.Text = "Rows:";
            // 
            // lblNumCols
            // 
            this.lblNumCols.AutoSize = true;
            this.lblNumCols.Location = new System.Drawing.Point(1516, 439);
            this.lblNumCols.Name = "lblNumCols";
            this.lblNumCols.Size = new System.Drawing.Size(50, 13);
            this.lblNumCols.TabIndex = 20;
            this.lblNumCols.Text = "Columns:";
            // 
            // txtNumRows
            // 
            this.txtNumRows.Location = new System.Drawing.Point(1583, 410);
            this.txtNumRows.Name = "txtNumRows";
            this.txtNumRows.Size = new System.Drawing.Size(32, 20);
            this.txtNumRows.TabIndex = 21;
            this.txtNumRows.TextChanged += new System.EventHandler(this.txtNumRows_TextChanged);
            // 
            // txtNumCols
            // 
            this.txtNumCols.Location = new System.Drawing.Point(1583, 436);
            this.txtNumCols.Name = "txtNumCols";
            this.txtNumCols.Size = new System.Drawing.Size(32, 20);
            this.txtNumCols.TabIndex = 22;
            this.txtNumCols.TextChanged += new System.EventHandler(this.txtNumCols_TextChanged);
            // 
            // lblWordsearchId
            // 
            this.lblWordsearchId.AutoSize = true;
            this.lblWordsearchId.Location = new System.Drawing.Point(1510, 465);
            this.lblWordsearchId.Name = "lblWordsearchId";
            this.lblWordsearchId.Size = new System.Drawing.Size(85, 13);
            this.lblWordsearchId.TabIndex = 23;
            this.lblWordsearchId.Text = "Word search ID:";
            // 
            // txtWordsearchId
            // 
            this.txtWordsearchId.Location = new System.Drawing.Point(1601, 462);
            this.txtWordsearchId.Name = "txtWordsearchId";
            this.txtWordsearchId.Size = new System.Drawing.Size(100, 20);
            this.txtWordsearchId.TabIndex = 24;
            // 
            // dataGridViewWordsearchImageMetaData
            // 
            this.dataGridViewWordsearchImageMetaData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWordsearchImageMetaData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.wordsearchImageMetaDataColName,
            this.wordsearchImageMetaDataColValue});
            this.dataGridViewWordsearchImageMetaData.Location = new System.Drawing.Point(1510, 505);
            this.dataGridViewWordsearchImageMetaData.Name = "dataGridViewWordsearchImageMetaData";
            this.dataGridViewWordsearchImageMetaData.Size = new System.Drawing.Size(376, 146);
            this.dataGridViewWordsearchImageMetaData.TabIndex = 25;
            // 
            // wordsearchImageMetaDataColName
            // 
            this.wordsearchImageMetaDataColName.HeaderText = "Name";
            this.wordsearchImageMetaDataColName.Name = "wordsearchImageMetaDataColName";
            // 
            // wordsearchImageMetaDataColValue
            // 
            this.wordsearchImageMetaDataColValue.HeaderText = "Value";
            this.wordsearchImageMetaDataColValue.Name = "wordsearchImageMetaDataColValue";
            this.wordsearchImageMetaDataColValue.Width = 230;
            // 
            // lblWordsearchImageMetaData
            // 
            this.lblWordsearchImageMetaData.AutoSize = true;
            this.lblWordsearchImageMetaData.Location = new System.Drawing.Point(1510, 489);
            this.lblWordsearchImageMetaData.Name = "lblWordsearchImageMetaData";
            this.lblWordsearchImageMetaData.Size = new System.Drawing.Size(145, 13);
            this.lblWordsearchImageMetaData.TabIndex = 26;
            this.lblWordsearchImageMetaData.Text = "Wordsearch Image Metadata";
            // 
            // lblWordsearchImageData
            // 
            this.lblWordsearchImageData.AutoSize = true;
            this.lblWordsearchImageData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWordsearchImageData.Location = new System.Drawing.Point(1507, 270);
            this.lblWordsearchImageData.Name = "lblWordsearchImageData";
            this.lblWordsearchImageData.Size = new System.Drawing.Size(165, 17);
            this.lblWordsearchImageData.TabIndex = 27;
            this.lblWordsearchImageData.Text = "Word search Image Data";
            // 
            // dataGridViewImageMetaData
            // 
            this.dataGridViewImageMetaData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewImageMetaData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.imageDataColName,
            this.imageDataColValue});
            this.dataGridViewImageMetaData.Location = new System.Drawing.Point(1510, 113);
            this.dataGridViewImageMetaData.Name = "dataGridViewImageMetaData";
            this.dataGridViewImageMetaData.Size = new System.Drawing.Size(376, 146);
            this.dataGridViewImageMetaData.TabIndex = 28;
            // 
            // imageDataColName
            // 
            this.imageDataColName.HeaderText = "Name";
            this.imageDataColName.Name = "imageDataColName";
            // 
            // imageDataColValue
            // 
            this.imageDataColValue.HeaderText = "Value";
            this.imageDataColValue.Name = "imageDataColValue";
            this.imageDataColValue.Width = 230;
            // 
            // lblImageMetaData
            // 
            this.lblImageMetaData.AutoSize = true;
            this.lblImageMetaData.Location = new System.Drawing.Point(1507, 97);
            this.lblImageMetaData.Name = "lblImageMetaData";
            this.lblImageMetaData.Size = new System.Drawing.Size(89, 13);
            this.lblImageMetaData.TabIndex = 29;
            this.lblImageMetaData.Text = "Image Meta Data";
            // 
            // btnSaveImageMetaData
            // 
            this.btnSaveImageMetaData.Image = global::DataEntryGUI.Properties.Resources.save;
            this.btnSaveImageMetaData.Location = new System.Drawing.Point(1598, 23);
            this.btnSaveImageMetaData.Name = "btnSaveImageMetaData";
            this.btnSaveImageMetaData.Size = new System.Drawing.Size(139, 71);
            this.btnSaveImageMetaData.TabIndex = 30;
            this.btnSaveImageMetaData.Text = "Save Image Meta Data to Database";
            this.btnSaveImageMetaData.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveImageMetaData.UseVisualStyleBackColor = true;
            this.btnSaveImageMetaData.Click += new System.EventHandler(this.btnSaveImageMetaData_Click);
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
            // picBoxWordsearchImage
            // 
            this.picBoxWordsearchImage.Location = new System.Drawing.Point(1519, 717);
            this.picBoxWordsearchImage.Name = "picBoxWordsearchImage";
            this.picBoxWordsearchImage.Size = new System.Drawing.Size(367, 222);
            this.picBoxWordsearchImage.TabIndex = 31;
            this.picBoxWordsearchImage.TabStop = false;
            this.toolTips.SetToolTip(this.picBoxWordsearchImage, "Click to enlarge");
            this.picBoxWordsearchImage.Click += new System.EventHandler(this.picBoxWordsearchImage_Click);
            // 
            // ImageMarkupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 952);
            this.Controls.Add(this.picBoxWordsearchImage);
            this.Controls.Add(this.btnSaveImageMetaData);
            this.Controls.Add(this.lblImageMetaData);
            this.Controls.Add(this.dataGridViewImageMetaData);
            this.Controls.Add(this.lblWordsearchImageData);
            this.Controls.Add(this.lblWordsearchImageMetaData);
            this.Controls.Add(this.dataGridViewWordsearchImageMetaData);
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxWordsearchImageLarge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWordsearchImageMetaData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImageMetaData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxWordsearchImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picBoxImage;
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
        private System.Windows.Forms.DataGridView dataGridViewWordsearchImageMetaData;
        private System.Windows.Forms.Label lblWordsearchImageMetaData;
        private System.Windows.Forms.DataGridViewTextBoxColumn wordsearchImageMetaDataColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn wordsearchImageMetaDataColValue;
        private System.Windows.Forms.Label lblWordsearchImageData;
        private System.Windows.Forms.DataGridView dataGridViewImageMetaData;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageDataColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageDataColValue;
        private System.Windows.Forms.Label lblImageMetaData;
        private System.Windows.Forms.Button btnSaveImageMetaData;
        private System.Windows.Forms.PictureBox picBoxWordsearchImage;
        private System.Windows.Forms.PictureBox picBoxWordsearchImageLarge;
        private System.Windows.Forms.ToolTip toolTips;
    }
}

