namespace ModelContertApp
{
    partial class Main
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
            this.buttonSelectInputFile = new System.Windows.Forms.Button();
            this.buttonSaveFileLocation = new System.Windows.Forms.Button();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSaveFileLocation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.checkBoxFlipYZ = new System.Windows.Forms.CheckBox();
            this.checkBoxFlipTriangles = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxModelUnits = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // buttonSelectInputFile
            // 
            this.buttonSelectInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectInputFile.Location = new System.Drawing.Point(445, 12);
            this.buttonSelectInputFile.Name = "buttonSelectInputFile";
            this.buttonSelectInputFile.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectInputFile.TabIndex = 0;
            this.buttonSelectInputFile.Text = "...";
            this.buttonSelectInputFile.UseVisualStyleBackColor = true;
            this.buttonSelectInputFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // buttonSaveFileLocation
            // 
            this.buttonSaveFileLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveFileLocation.Location = new System.Drawing.Point(445, 41);
            this.buttonSaveFileLocation.Name = "buttonSaveFileLocation";
            this.buttonSaveFileLocation.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveFileLocation.TabIndex = 1;
            this.buttonSaveFileLocation.Text = "...";
            this.buttonSaveFileLocation.UseVisualStyleBackColor = true;
            this.buttonSaveFileLocation.Click += new System.EventHandler(this.buttonSaveFile_Click);
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFileName.Location = new System.Drawing.Point(82, 14);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.ReadOnly = true;
            this.textBoxFileName.Size = new System.Drawing.Size(357, 20);
            this.textBoxFileName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Input File: ";
            // 
            // textBoxSaveFileLocation
            // 
            this.textBoxSaveFileLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSaveFileLocation.Location = new System.Drawing.Point(82, 43);
            this.textBoxSaveFileLocation.Name = "textBoxSaveFileLocation";
            this.textBoxSaveFileLocation.Size = new System.Drawing.Size(357, 20);
            this.textBoxSaveFileLocation.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Output File: ";
            // 
            // buttonConvert
            // 
            this.buttonConvert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConvert.Location = new System.Drawing.Point(12, 116);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(508, 28);
            this.buttonConvert.TabIndex = 8;
            this.buttonConvert.Text = "Convert";
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
            // 
            // checkBoxFlipYZ
            // 
            this.checkBoxFlipYZ.AutoSize = true;
            this.checkBoxFlipYZ.Location = new System.Drawing.Point(82, 80);
            this.checkBoxFlipYZ.Name = "checkBoxFlipYZ";
            this.checkBoxFlipYZ.Size = new System.Drawing.Size(56, 17);
            this.checkBoxFlipYZ.TabIndex = 9;
            this.checkBoxFlipYZ.Text = "FlipYZ";
            this.checkBoxFlipYZ.UseVisualStyleBackColor = true;
            // 
            // checkBoxFlipTriangles
            // 
            this.checkBoxFlipTriangles.AutoSize = true;
            this.checkBoxFlipTriangles.Location = new System.Drawing.Point(153, 80);
            this.checkBoxFlipTriangles.Name = "checkBoxFlipTriangles";
            this.checkBoxFlipTriangles.Size = new System.Drawing.Size(85, 17);
            this.checkBoxFlipTriangles.TabIndex = 10;
            this.checkBoxFlipTriangles.Text = "FlipTriangles";
            this.checkBoxFlipTriangles.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(255, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Model Units: ";
            // 
            // comboBoxModelUnits
            // 
            this.comboBoxModelUnits.FormattingEnabled = true;
            this.comboBoxModelUnits.Location = new System.Drawing.Point(330, 78);
            this.comboBoxModelUnits.Name = "comboBoxModelUnits";
            this.comboBoxModelUnits.Size = new System.Drawing.Size(109, 21);
            this.comboBoxModelUnits.TabIndex = 12;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 156);
            this.Controls.Add(this.comboBoxModelUnits);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBoxFlipTriangles);
            this.Controls.Add(this.checkBoxFlipYZ);
            this.Controls.Add(this.buttonConvert);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxSaveFileLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.buttonSaveFileLocation);
            this.Controls.Add(this.buttonSelectInputFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Model Converter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectInputFile;
        private System.Windows.Forms.Button buttonSaveFileLocation;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSaveFileLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonConvert;
        private System.Windows.Forms.CheckBox checkBoxFlipYZ;
        private System.Windows.Forms.CheckBox checkBoxFlipTriangles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxModelUnits;
    }
}

