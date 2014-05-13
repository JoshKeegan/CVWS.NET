namespace DemoGUI
{
    partial class MainForm
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentDirectoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stretchImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.algorithmsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordsearchDetectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.candidateSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quadrilateralsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.candidateRankingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordsearchDetectionSegmentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordsearchSegmentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotationCorrectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.probabilisticClassificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotationCorrectionFeatureExtractionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotationCorrectionClassificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.characterExtractionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.biggestBlobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.featureExtractionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.classificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordsearchSolverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nonProbabilisticToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.probabilisticToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewFiles = new System.Windows.Forms.ListView();
            this.columnHeaderFileNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.listViewImageLog = new System.Windows.Forms.ListView();
            this.columnHeaderImageLog = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
            this.splitContainerRight = new System.Windows.Forms.SplitContainer();
            this.splitContainerRightTop = new System.Windows.Forms.SplitContainer();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnStartProcessing = new System.Windows.Forms.Button();
            this.checkListProcessingStages = new System.Windows.Forms.CheckedListBox();
            this.splitContainerRightBottom = new System.Windows.Forms.SplitContainer();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.txtWordsToFind = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).BeginInit();
            this.splitContainerLeft.Panel1.SuspendLayout();
            this.splitContainerLeft.Panel2.SuspendLayout();
            this.splitContainerLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).BeginInit();
            this.splitContainerRight.Panel1.SuspendLayout();
            this.splitContainerRight.Panel2.SuspendLayout();
            this.splitContainerRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRightTop)).BeginInit();
            this.splitContainerRightTop.Panel1.SuspendLayout();
            this.splitContainerRightTop.Panel2.SuspendLayout();
            this.splitContainerRightTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRightBottom)).BeginInit();
            this.splitContainerRightBottom.Panel1.SuspendLayout();
            this.splitContainerRightBottom.Panel2.SuspendLayout();
            this.splitContainerRightBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(984, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDirectoryToolStripMenuItem,
            this.recentDirectoriesToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openDirectoryToolStripMenuItem
            // 
            this.openDirectoryToolStripMenuItem.Name = "openDirectoryToolStripMenuItem";
            this.openDirectoryToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openDirectoryToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.openDirectoryToolStripMenuItem.Text = "&Open Directory";
            this.openDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openDirectoryToolStripMenuItem_Click);
            // 
            // recentDirectoriesToolStripMenuItem
            // 
            this.recentDirectoriesToolStripMenuItem.Name = "recentDirectoriesToolStripMenuItem";
            this.recentDirectoriesToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.recentDirectoriesToolStripMenuItem.Text = "&Recent Directories";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageViewToolStripMenuItem,
            this.algorithmsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "&Settings";
            // 
            // imageViewToolStripMenuItem
            // 
            this.imageViewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomToolStripMenuItem,
            this.normalToolStripMenuItem,
            this.centreToolStripMenuItem,
            this.autoSizeToolStripMenuItem,
            this.stretchImageToolStripMenuItem});
            this.imageViewToolStripMenuItem.Name = "imageViewToolStripMenuItem";
            this.imageViewToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.imageViewToolStripMenuItem.Text = "Image &View";
            // 
            // zoomToolStripMenuItem
            // 
            this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            this.zoomToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.zoomToolStripMenuItem.Text = "&Zoom";
            this.zoomToolStripMenuItem.Click += new System.EventHandler(this.zoomToolStripMenuItem_Click);
            // 
            // normalToolStripMenuItem
            // 
            this.normalToolStripMenuItem.Name = "normalToolStripMenuItem";
            this.normalToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.normalToolStripMenuItem.Text = "&Normal";
            this.normalToolStripMenuItem.Click += new System.EventHandler(this.normalToolStripMenuItem_Click);
            // 
            // centreToolStripMenuItem
            // 
            this.centreToolStripMenuItem.Name = "centreToolStripMenuItem";
            this.centreToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.centreToolStripMenuItem.Text = "&Centre";
            this.centreToolStripMenuItem.Click += new System.EventHandler(this.centreToolStripMenuItem_Click);
            // 
            // autoSizeToolStripMenuItem
            // 
            this.autoSizeToolStripMenuItem.Name = "autoSizeToolStripMenuItem";
            this.autoSizeToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.autoSizeToolStripMenuItem.Text = "&Auto Size";
            this.autoSizeToolStripMenuItem.Click += new System.EventHandler(this.autoSizeToolStripMenuItem_Click);
            // 
            // stretchImageToolStripMenuItem
            // 
            this.stretchImageToolStripMenuItem.Name = "stretchImageToolStripMenuItem";
            this.stretchImageToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.stretchImageToolStripMenuItem.Text = "&Stretch Image";
            this.stretchImageToolStripMenuItem.Click += new System.EventHandler(this.stretchImageToolStripMenuItem_Click);
            // 
            // algorithmsToolStripMenuItem
            // 
            this.algorithmsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wordsearchDetectionToolStripMenuItem,
            this.wordsearchSegmentationToolStripMenuItem,
            this.rotationCorrectionToolStripMenuItem,
            this.characterExtractionToolStripMenuItem,
            this.featureExtractionToolStripMenuItem,
            this.classificationToolStripMenuItem,
            this.wordsearchSolverToolStripMenuItem});
            this.algorithmsToolStripMenuItem.Name = "algorithmsToolStripMenuItem";
            this.algorithmsToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.algorithmsToolStripMenuItem.Text = "&Algorithms";
            // 
            // wordsearchDetectionToolStripMenuItem
            // 
            this.wordsearchDetectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.candidateSelectionToolStripMenuItem,
            this.candidateRankingToolStripMenuItem});
            this.wordsearchDetectionToolStripMenuItem.Name = "wordsearchDetectionToolStripMenuItem";
            this.wordsearchDetectionToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.wordsearchDetectionToolStripMenuItem.Text = "Wordsearch Detection";
            // 
            // candidateSelectionToolStripMenuItem
            // 
            this.candidateSelectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quadrilateralsToolStripMenuItem});
            this.candidateSelectionToolStripMenuItem.Name = "candidateSelectionToolStripMenuItem";
            this.candidateSelectionToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.candidateSelectionToolStripMenuItem.Text = "Candidate Selection";
            // 
            // quadrilateralsToolStripMenuItem
            // 
            this.quadrilateralsToolStripMenuItem.Checked = true;
            this.quadrilateralsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.quadrilateralsToolStripMenuItem.Name = "quadrilateralsToolStripMenuItem";
            this.quadrilateralsToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.quadrilateralsToolStripMenuItem.Text = "Quadrilaterals";
            // 
            // candidateRankingToolStripMenuItem
            // 
            this.candidateRankingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wordsearchDetectionSegmentationToolStripMenuItem});
            this.candidateRankingToolStripMenuItem.Name = "candidateRankingToolStripMenuItem";
            this.candidateRankingToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.candidateRankingToolStripMenuItem.Text = "Candidate Ranking";
            // 
            // wordsearchDetectionSegmentationToolStripMenuItem
            // 
            this.wordsearchDetectionSegmentationToolStripMenuItem.Checked = true;
            this.wordsearchDetectionSegmentationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.wordsearchDetectionSegmentationToolStripMenuItem.Name = "wordsearchDetectionSegmentationToolStripMenuItem";
            this.wordsearchDetectionSegmentationToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.wordsearchDetectionSegmentationToolStripMenuItem.Text = "Wordsearch Segmentation";
            // 
            // wordsearchSegmentationToolStripMenuItem
            // 
            this.wordsearchSegmentationToolStripMenuItem.Name = "wordsearchSegmentationToolStripMenuItem";
            this.wordsearchSegmentationToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.wordsearchSegmentationToolStripMenuItem.Text = "Wordsearch Segmentation";
            // 
            // rotationCorrectionToolStripMenuItem
            // 
            this.rotationCorrectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.probabilisticClassificationToolStripMenuItem});
            this.rotationCorrectionToolStripMenuItem.Name = "rotationCorrectionToolStripMenuItem";
            this.rotationCorrectionToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.rotationCorrectionToolStripMenuItem.Text = "Rotation Correction";
            // 
            // probabilisticClassificationToolStripMenuItem
            // 
            this.probabilisticClassificationToolStripMenuItem.Checked = true;
            this.probabilisticClassificationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.probabilisticClassificationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rotationCorrectionFeatureExtractionToolStripMenuItem,
            this.rotationCorrectionClassificationToolStripMenuItem});
            this.probabilisticClassificationToolStripMenuItem.Name = "probabilisticClassificationToolStripMenuItem";
            this.probabilisticClassificationToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.probabilisticClassificationToolStripMenuItem.Text = "Probabilistic Classification";
            // 
            // rotationCorrectionFeatureExtractionToolStripMenuItem
            // 
            this.rotationCorrectionFeatureExtractionToolStripMenuItem.Name = "rotationCorrectionFeatureExtractionToolStripMenuItem";
            this.rotationCorrectionFeatureExtractionToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.rotationCorrectionFeatureExtractionToolStripMenuItem.Text = "Feature Extraction";
            // 
            // rotationCorrectionClassificationToolStripMenuItem
            // 
            this.rotationCorrectionClassificationToolStripMenuItem.Name = "rotationCorrectionClassificationToolStripMenuItem";
            this.rotationCorrectionClassificationToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.rotationCorrectionClassificationToolStripMenuItem.Text = "Classification";
            // 
            // characterExtractionToolStripMenuItem
            // 
            this.characterExtractionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.biggestBlobToolStripMenuItem});
            this.characterExtractionToolStripMenuItem.Name = "characterExtractionToolStripMenuItem";
            this.characterExtractionToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.characterExtractionToolStripMenuItem.Text = "Character Image Extraction";
            // 
            // biggestBlobToolStripMenuItem
            // 
            this.biggestBlobToolStripMenuItem.Checked = true;
            this.biggestBlobToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.biggestBlobToolStripMenuItem.Name = "biggestBlobToolStripMenuItem";
            this.biggestBlobToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.biggestBlobToolStripMenuItem.Text = "Biggest Blob";
            // 
            // featureExtractionToolStripMenuItem
            // 
            this.featureExtractionToolStripMenuItem.Name = "featureExtractionToolStripMenuItem";
            this.featureExtractionToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.featureExtractionToolStripMenuItem.Text = "Feature Extraction";
            // 
            // classificationToolStripMenuItem
            // 
            this.classificationToolStripMenuItem.Name = "classificationToolStripMenuItem";
            this.classificationToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.classificationToolStripMenuItem.Text = "Classification";
            // 
            // wordsearchSolverToolStripMenuItem
            // 
            this.wordsearchSolverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nonProbabilisticToolStripMenuItem,
            this.probabilisticToolStripMenuItem});
            this.wordsearchSolverToolStripMenuItem.Name = "wordsearchSolverToolStripMenuItem";
            this.wordsearchSolverToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.wordsearchSolverToolStripMenuItem.Text = "Wordsearch Solver";
            // 
            // nonProbabilisticToolStripMenuItem
            // 
            this.nonProbabilisticToolStripMenuItem.Name = "nonProbabilisticToolStripMenuItem";
            this.nonProbabilisticToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.nonProbabilisticToolStripMenuItem.Text = "Non-Probabilistic";
            // 
            // probabilisticToolStripMenuItem
            // 
            this.probabilisticToolStripMenuItem.Checked = true;
            this.probabilisticToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.probabilisticToolStripMenuItem.Name = "probabilisticToolStripMenuItem";
            this.probabilisticToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.probabilisticToolStripMenuItem.Text = "Probabilistic";
            // 
            // listViewFiles
            // 
            this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFileNames});
            this.listViewFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewFiles.FullRowSelect = true;
            this.listViewFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewFiles.HideSelection = false;
            this.listViewFiles.Location = new System.Drawing.Point(0, 0);
            this.listViewFiles.MultiSelect = false;
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.Size = new System.Drawing.Size(177, 241);
            this.listViewFiles.TabIndex = 1;
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.View = System.Windows.Forms.View.Details;
            this.listViewFiles.SelectedIndexChanged += new System.EventHandler(this.listViewFiles_SelectedIndexChanged);
            // 
            // columnHeaderFileNames
            // 
            this.columnHeaderFileNames.Tag = "";
            this.columnHeaderFileNames.Text = "File Names";
            this.columnHeaderFileNames.Width = 131;
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 503);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(984, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // listViewImageLog
            // 
            this.listViewImageLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderImageLog});
            this.listViewImageLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewImageLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewImageLog.HideSelection = false;
            this.listViewImageLog.Location = new System.Drawing.Point(0, 0);
            this.listViewImageLog.MultiSelect = false;
            this.listViewImageLog.Name = "listViewImageLog";
            this.listViewImageLog.Size = new System.Drawing.Size(177, 234);
            this.listViewImageLog.TabIndex = 3;
            this.listViewImageLog.UseCompatibleStateImageBehavior = false;
            this.listViewImageLog.View = System.Windows.Forms.View.Details;
            this.listViewImageLog.SelectedIndexChanged += new System.EventHandler(this.listViewImageLog_SelectedIndexChanged);
            // 
            // columnHeaderImageLog
            // 
            this.columnHeaderImageLog.Text = "Image Log";
            this.columnHeaderImageLog.Width = 84;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 24);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerLeft);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerRight);
            this.splitContainerMain.Size = new System.Drawing.Size(984, 479);
            this.splitContainerMain.SplitterDistance = 177;
            this.splitContainerMain.TabIndex = 5;
            // 
            // splitContainerLeft
            // 
            this.splitContainerLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLeft.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLeft.Name = "splitContainerLeft";
            this.splitContainerLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLeft.Panel1
            // 
            this.splitContainerLeft.Panel1.Controls.Add(this.listViewFiles);
            // 
            // splitContainerLeft.Panel2
            // 
            this.splitContainerLeft.Panel2.Controls.Add(this.listViewImageLog);
            this.splitContainerLeft.Size = new System.Drawing.Size(177, 479);
            this.splitContainerLeft.SplitterDistance = 241;
            this.splitContainerLeft.TabIndex = 0;
            // 
            // splitContainerRight
            // 
            this.splitContainerRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRight.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRight.Name = "splitContainerRight";
            this.splitContainerRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerRight.Panel1
            // 
            this.splitContainerRight.Panel1.Controls.Add(this.splitContainerRightTop);
            // 
            // splitContainerRight.Panel2
            // 
            this.splitContainerRight.Panel2.Controls.Add(this.splitContainerRightBottom);
            this.splitContainerRight.Size = new System.Drawing.Size(803, 479);
            this.splitContainerRight.SplitterDistance = 378;
            this.splitContainerRight.TabIndex = 0;
            // 
            // splitContainerRightTop
            // 
            this.splitContainerRightTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRightTop.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerRightTop.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRightTop.Name = "splitContainerRightTop";
            // 
            // splitContainerRightTop.Panel1
            // 
            this.splitContainerRightTop.Panel1.Controls.Add(this.pictureBox);
            // 
            // splitContainerRightTop.Panel2
            // 
            this.splitContainerRightTop.Panel2.Controls.Add(this.btnStartProcessing);
            this.splitContainerRightTop.Panel2.Controls.Add(this.checkListProcessingStages);
            this.splitContainerRightTop.Size = new System.Drawing.Size(803, 378);
            this.splitContainerRightTop.SplitterDistance = 638;
            this.splitContainerRightTop.TabIndex = 0;
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(638, 378);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // btnStartProcessing
            // 
            this.btnStartProcessing.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnStartProcessing.Location = new System.Drawing.Point(0, 0);
            this.btnStartProcessing.Name = "btnStartProcessing";
            this.btnStartProcessing.Size = new System.Drawing.Size(161, 23);
            this.btnStartProcessing.TabIndex = 1;
            this.btnStartProcessing.Text = "Start";
            this.btnStartProcessing.UseVisualStyleBackColor = true;
            this.btnStartProcessing.Click += new System.EventHandler(this.btnStartProcessing_Click);
            // 
            // checkListProcessingStages
            // 
            this.checkListProcessingStages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkListProcessingStages.FormattingEnabled = true;
            this.checkListProcessingStages.Items.AddRange(new object[] {
            "Wordsearch Detection",
            "Wordsearch Segmentation",
            "Rotation Correction",
            "Character Image Extraction",
            "Feature Extraction",
            "Classification",
            "Wordsearch Solver"});
            this.checkListProcessingStages.Location = new System.Drawing.Point(0, 26);
            this.checkListProcessingStages.Name = "checkListProcessingStages";
            this.checkListProcessingStages.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkListProcessingStages.Size = new System.Drawing.Size(158, 349);
            this.checkListProcessingStages.TabIndex = 0;
            // 
            // splitContainerRightBottom
            // 
            this.splitContainerRightBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRightBottom.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRightBottom.Name = "splitContainerRightBottom";
            // 
            // splitContainerRightBottom.Panel1
            // 
            this.splitContainerRightBottom.Panel1.Controls.Add(this.txtLog);
            // 
            // splitContainerRightBottom.Panel2
            // 
            this.splitContainerRightBottom.Panel2.Controls.Add(this.txtWordsToFind);
            this.splitContainerRightBottom.Size = new System.Drawing.Size(803, 97);
            this.splitContainerRightBottom.SplitterDistance = 519;
            this.splitContainerRightBottom.TabIndex = 0;
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(519, 97);
            this.txtLog.TabIndex = 0;
            // 
            // txtWordsToFind
            // 
            this.txtWordsToFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWordsToFind.Location = new System.Drawing.Point(0, 0);
            this.txtWordsToFind.Multiline = true;
            this.txtWordsToFind.Name = "txtWordsToFind";
            this.txtWordsToFind.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtWordsToFind.Size = new System.Drawing.Size(280, 97);
            this.txtWordsToFind.TabIndex = 0;
            this.txtWordsToFind.Text = "Enter words to find here . . .\r\nPlease enter one word per line";
            this.txtWordsToFind.Enter += new System.EventHandler(this.txtWordsToFind_Enter);
            this.txtWordsToFind.Leave += new System.EventHandler(this.txtWordToFind_Leave);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 525);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Computer Vision Wordsearch Solver Demo GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerLeft.Panel1.ResumeLayout(false);
            this.splitContainerLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).EndInit();
            this.splitContainerLeft.ResumeLayout(false);
            this.splitContainerRight.Panel1.ResumeLayout(false);
            this.splitContainerRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).EndInit();
            this.splitContainerRight.ResumeLayout(false);
            this.splitContainerRightTop.Panel1.ResumeLayout(false);
            this.splitContainerRightTop.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRightTop)).EndInit();
            this.splitContainerRightTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.splitContainerRightBottom.Panel1.ResumeLayout(false);
            this.splitContainerRightBottom.Panel1.PerformLayout();
            this.splitContainerRightBottom.Panel2.ResumeLayout(false);
            this.splitContainerRightBottom.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRightBottom)).EndInit();
            this.splitContainerRightBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ListView listViewFiles;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ListView listViewImageLog;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainerLeft;
        private System.Windows.Forms.SplitContainer splitContainerRight;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ColumnHeader columnHeaderFileNames;
        private System.Windows.Forms.ColumnHeader columnHeaderImageLog;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentDirectoriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stretchImageToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainerRightTop;
        private System.Windows.Forms.SplitContainer splitContainerRightBottom;
        private System.Windows.Forms.TextBox txtWordsToFind;
        private System.Windows.Forms.CheckedListBox checkListProcessingStages;
        private System.Windows.Forms.Button btnStartProcessing;
        private System.Windows.Forms.ToolStripMenuItem algorithmsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wordsearchDetectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem candidateSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quadrilateralsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem candidateRankingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wordsearchDetectionSegmentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotationCorrectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wordsearchSegmentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem probabilisticClassificationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem characterExtractionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem biggestBlobToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem classificationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotationCorrectionFeatureExtractionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotationCorrectionClassificationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem featureExtractionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wordsearchSolverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nonProbabilisticToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem probabilisticToolStripMenuItem;
    }
}

