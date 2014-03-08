namespace DataEntryGUI
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
            this.btnImageMarkup = new System.Windows.Forms.Button();
            this.btnWordsearchMarkup = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnImageMarkup
            // 
            this.btnImageMarkup.Location = new System.Drawing.Point(13, 13);
            this.btnImageMarkup.Name = "btnImageMarkup";
            this.btnImageMarkup.Size = new System.Drawing.Size(77, 54);
            this.btnImageMarkup.TabIndex = 0;
            this.btnImageMarkup.Text = "Image Markup";
            this.btnImageMarkup.UseVisualStyleBackColor = true;
            this.btnImageMarkup.Click += new System.EventHandler(this.btnImageMarkup_Click);
            // 
            // btnWordsearchMarkup
            // 
            this.btnWordsearchMarkup.Location = new System.Drawing.Point(96, 13);
            this.btnWordsearchMarkup.Name = "btnWordsearchMarkup";
            this.btnWordsearchMarkup.Size = new System.Drawing.Size(77, 54);
            this.btnWordsearchMarkup.TabIndex = 1;
            this.btnWordsearchMarkup.Text = "Word search Markup";
            this.btnWordsearchMarkup.UseVisualStyleBackColor = true;
            this.btnWordsearchMarkup.Click += new System.EventHandler(this.btnWordsearchMarkup_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(186, 77);
            this.Controls.Add(this.btnWordsearchMarkup);
            this.Controls.Add(this.btnImageMarkup);
            this.Name = "MainForm";
            this.Text = "Data Entry";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImageMarkup;
        private System.Windows.Forms.Button btnWordsearchMarkup;
    }
}