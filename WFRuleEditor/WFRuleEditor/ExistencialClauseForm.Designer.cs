namespace BIMRuleEditor
{
    partial class ExistencialClauseForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxObjectType = new System.Windows.Forms.ComboBox();
            this.comboBoxObjectExistence = new System.Windows.Forms.ComboBox();
            this.textBoxObjectIndex = new System.Windows.Forms.TextBox();
            this.treeViewProperties = new System.Windows.Forms.TreeView();
            this.buttonDeleteObjProperty = new System.Windows.Forms.Button();
            this.buttonAddProperty = new System.Windows.Forms.Button();
            this.buttonEditProperty = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(255, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 113;
            this.label4.Text = "Property Check(s)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 111;
            this.label7.Text = "Object Name";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 152);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 13);
            this.label10.TabIndex = 110;
            this.label10.Text = "Type";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 125);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 13);
            this.label11.TabIndex = 109;
            this.label11.Text = "Existence";
            // 
            // comboBoxObjectType
            // 
            this.comboBoxObjectType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxObjectType.FormattingEnabled = true;
            this.comboBoxObjectType.Location = new System.Drawing.Point(85, 149);
            this.comboBoxObjectType.Name = "comboBoxObjectType";
            this.comboBoxObjectType.Size = new System.Drawing.Size(167, 21);
            this.comboBoxObjectType.TabIndex = 108;
            this.comboBoxObjectType.SelectedIndexChanged += new System.EventHandler(this.comboBoxObjectType_SelectedIndexChanged);
            // 
            // comboBoxObjectExistence
            // 
            this.comboBoxObjectExistence.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxObjectExistence.FormattingEnabled = true;
            this.comboBoxObjectExistence.Location = new System.Drawing.Point(85, 122);
            this.comboBoxObjectExistence.Name = "comboBoxObjectExistence";
            this.comboBoxObjectExistence.Size = new System.Drawing.Size(167, 21);
            this.comboBoxObjectExistence.TabIndex = 107;
            this.comboBoxObjectExistence.SelectedIndexChanged += new System.EventHandler(this.comboBoxObjectExistence_SelectedIndexChanged);
            // 
            // textBoxObjectIndex
            // 
            this.textBoxObjectIndex.Location = new System.Drawing.Point(85, 25);
            this.textBoxObjectIndex.Name = "textBoxObjectIndex";
            this.textBoxObjectIndex.Size = new System.Drawing.Size(167, 20);
            this.textBoxObjectIndex.TabIndex = 106;
            this.textBoxObjectIndex.TextChanged += new System.EventHandler(this.textBoxObjectIndex_TextChanged);
            // 
            // treeViewProperties
            // 
            this.treeViewProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewProperties.Location = new System.Drawing.Point(258, 25);
            this.treeViewProperties.Name = "treeViewProperties";
            this.treeViewProperties.Size = new System.Drawing.Size(220, 145);
            this.treeViewProperties.TabIndex = 114;
            // 
            // buttonDeleteObjProperty
            // 
            this.buttonDeleteObjProperty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteObjProperty.Location = new System.Drawing.Point(484, 147);
            this.buttonDeleteObjProperty.Name = "buttonDeleteObjProperty";
            this.buttonDeleteObjProperty.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteObjProperty.TabIndex = 117;
            this.buttonDeleteObjProperty.Text = "Delete";
            this.buttonDeleteObjProperty.UseVisualStyleBackColor = true;
            this.buttonDeleteObjProperty.Click += new System.EventHandler(this.buttonDeleteObjProperty_Click);
            // 
            // buttonAddProperty
            // 
            this.buttonAddProperty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddProperty.Location = new System.Drawing.Point(484, 25);
            this.buttonAddProperty.Name = "buttonAddProperty";
            this.buttonAddProperty.Size = new System.Drawing.Size(75, 23);
            this.buttonAddProperty.TabIndex = 115;
            this.buttonAddProperty.Text = "Add";
            this.buttonAddProperty.UseVisualStyleBackColor = true;
            this.buttonAddProperty.Click += new System.EventHandler(this.buttonAddProperty_Click);
            // 
            // buttonEditProperty
            // 
            this.buttonEditProperty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEditProperty.Location = new System.Drawing.Point(484, 54);
            this.buttonEditProperty.Name = "buttonEditProperty";
            this.buttonEditProperty.Size = new System.Drawing.Size(75, 23);
            this.buttonEditProperty.TabIndex = 116;
            this.buttonEditProperty.Text = "Edit";
            this.buttonEditProperty.UseVisualStyleBackColor = true;
            this.buttonEditProperty.Click += new System.EventHandler(this.buttonEditProperty_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(484, 176);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 145;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonDone
            // 
            this.buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDone.Location = new System.Drawing.Point(397, 176);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(75, 23);
            this.buttonDone.TabIndex = 144;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // ExistencialClauseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 211);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.buttonDeleteObjProperty);
            this.Controls.Add(this.buttonAddProperty);
            this.Controls.Add(this.buttonEditProperty);
            this.Controls.Add(this.treeViewProperties);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxObjectIndex);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.comboBoxObjectExistence);
            this.Controls.Add(this.comboBoxObjectType);
            this.Name = "ExistencialClauseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ExistencialClause";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxObjectType;
        private System.Windows.Forms.ComboBox comboBoxObjectExistence;
        private System.Windows.Forms.TextBox textBoxObjectIndex;
        private System.Windows.Forms.TreeView treeViewProperties;
        private System.Windows.Forms.Button buttonDeleteObjProperty;
        private System.Windows.Forms.Button buttonAddProperty;
        private System.Windows.Forms.Button buttonEditProperty;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonDone;
    }
}