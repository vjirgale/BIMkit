namespace BIMRuleEditor
{
    partial class PropertyCheckForm
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
            this.textBoxStringValue = new System.Windows.Forms.TextBox();
            this.comboBoxOperString = new System.Windows.Forms.ComboBox();
            this.comboBoxStringProp = new System.Windows.Forms.ComboBox();
            this.textBoxNumValue = new System.Windows.Forms.TextBox();
            this.comboBoxNumUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxOperNum = new System.Windows.Forms.ComboBox();
            this.comboBoxNumProp = new System.Windows.Forms.ComboBox();
            this.comboBoxBoolValue = new System.Windows.Forms.ComboBox();
            this.comboBoxOperBool = new System.Windows.Forms.ComboBox();
            this.comboBoxBoolProp = new System.Windows.Forms.ComboBox();
            this.radioButtonPropCheckBool = new System.Windows.Forms.RadioButton();
            this.radioButtonPropCheckNum = new System.Windows.Forms.RadioButton();
            this.radioButtonPropCheckString = new System.Windows.Forms.RadioButton();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxStringValue
            // 
            this.textBoxStringValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStringValue.Location = new System.Drawing.Point(313, 66);
            this.textBoxStringValue.Name = "textBoxStringValue";
            this.textBoxStringValue.Size = new System.Drawing.Size(262, 20);
            this.textBoxStringValue.TabIndex = 133;
            this.textBoxStringValue.TextChanged += new System.EventHandler(this.textBoxStringValue_TextChanged);
            // 
            // comboBoxOperString
            // 
            this.comboBoxOperString.FormattingEnabled = true;
            this.comboBoxOperString.Location = new System.Drawing.Point(159, 66);
            this.comboBoxOperString.Name = "comboBoxOperString";
            this.comboBoxOperString.Size = new System.Drawing.Size(148, 21);
            this.comboBoxOperString.TabIndex = 132;
            this.comboBoxOperString.SelectedIndexChanged += new System.EventHandler(this.comboBoxOperString_SelectedIndexChanged);
            // 
            // comboBoxStringProp
            // 
            this.comboBoxStringProp.FormattingEnabled = true;
            this.comboBoxStringProp.Location = new System.Drawing.Point(32, 66);
            this.comboBoxStringProp.Name = "comboBoxStringProp";
            this.comboBoxStringProp.Size = new System.Drawing.Size(121, 21);
            this.comboBoxStringProp.TabIndex = 131;
            this.comboBoxStringProp.SelectedIndexChanged += new System.EventHandler(this.comboBoxStringProp_SelectedIndexChanged);
            // 
            // textBoxNumValue
            // 
            this.textBoxNumValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNumValue.Location = new System.Drawing.Point(313, 39);
            this.textBoxNumValue.Name = "textBoxNumValue";
            this.textBoxNumValue.Size = new System.Drawing.Size(180, 20);
            this.textBoxNumValue.TabIndex = 130;
            this.textBoxNumValue.TextChanged += new System.EventHandler(this.textBoxNumValue_TextChanged);
            // 
            // comboBoxNumUnit
            // 
            this.comboBoxNumUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxNumUnit.FormattingEnabled = true;
            this.comboBoxNumUnit.Location = new System.Drawing.Point(499, 39);
            this.comboBoxNumUnit.Name = "comboBoxNumUnit";
            this.comboBoxNumUnit.Size = new System.Drawing.Size(76, 21);
            this.comboBoxNumUnit.TabIndex = 129;
            this.comboBoxNumUnit.SelectedIndexChanged += new System.EventHandler(this.comboBoxNumUnit_SelectedIndexChanged);
            // 
            // comboBoxOperNum
            // 
            this.comboBoxOperNum.FormattingEnabled = true;
            this.comboBoxOperNum.Location = new System.Drawing.Point(159, 39);
            this.comboBoxOperNum.Name = "comboBoxOperNum";
            this.comboBoxOperNum.Size = new System.Drawing.Size(148, 21);
            this.comboBoxOperNum.TabIndex = 128;
            this.comboBoxOperNum.SelectedIndexChanged += new System.EventHandler(this.comboBoxOperNum_SelectedIndexChanged);
            // 
            // comboBoxNumProp
            // 
            this.comboBoxNumProp.FormattingEnabled = true;
            this.comboBoxNumProp.Location = new System.Drawing.Point(32, 39);
            this.comboBoxNumProp.Name = "comboBoxNumProp";
            this.comboBoxNumProp.Size = new System.Drawing.Size(121, 21);
            this.comboBoxNumProp.TabIndex = 127;
            this.comboBoxNumProp.SelectedIndexChanged += new System.EventHandler(this.comboBoxNumProp_SelectedIndexChanged);
            // 
            // comboBoxBoolValue
            // 
            this.comboBoxBoolValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBoolValue.FormattingEnabled = true;
            this.comboBoxBoolValue.Location = new System.Drawing.Point(313, 12);
            this.comboBoxBoolValue.Name = "comboBoxBoolValue";
            this.comboBoxBoolValue.Size = new System.Drawing.Size(262, 21);
            this.comboBoxBoolValue.TabIndex = 126;
            this.comboBoxBoolValue.SelectedIndexChanged += new System.EventHandler(this.comboBoxBoolValue_SelectedIndexChanged);
            // 
            // comboBoxOperBool
            // 
            this.comboBoxOperBool.FormattingEnabled = true;
            this.comboBoxOperBool.Location = new System.Drawing.Point(159, 12);
            this.comboBoxOperBool.Name = "comboBoxOperBool";
            this.comboBoxOperBool.Size = new System.Drawing.Size(148, 21);
            this.comboBoxOperBool.TabIndex = 125;
            this.comboBoxOperBool.SelectedIndexChanged += new System.EventHandler(this.comboBoxOperBool_SelectedIndexChanged);
            // 
            // comboBoxBoolProp
            // 
            this.comboBoxBoolProp.FormattingEnabled = true;
            this.comboBoxBoolProp.Location = new System.Drawing.Point(32, 12);
            this.comboBoxBoolProp.Name = "comboBoxBoolProp";
            this.comboBoxBoolProp.Size = new System.Drawing.Size(121, 21);
            this.comboBoxBoolProp.TabIndex = 124;
            this.comboBoxBoolProp.SelectedIndexChanged += new System.EventHandler(this.comboBoxBoolProp_SelectedIndexChanged);
            // 
            // radioButtonPropCheckBool
            // 
            this.radioButtonPropCheckBool.AutoSize = true;
            this.radioButtonPropCheckBool.Location = new System.Drawing.Point(12, 15);
            this.radioButtonPropCheckBool.Name = "radioButtonPropCheckBool";
            this.radioButtonPropCheckBool.Size = new System.Drawing.Size(14, 13);
            this.radioButtonPropCheckBool.TabIndex = 134;
            this.radioButtonPropCheckBool.TabStop = true;
            this.radioButtonPropCheckBool.UseVisualStyleBackColor = true;
            this.radioButtonPropCheckBool.CheckedChanged += new System.EventHandler(this.RadioButtonPropCheck_CheckedChanged);
            // 
            // radioButtonPropCheckNum
            // 
            this.radioButtonPropCheckNum.AutoSize = true;
            this.radioButtonPropCheckNum.Location = new System.Drawing.Point(12, 42);
            this.radioButtonPropCheckNum.Name = "radioButtonPropCheckNum";
            this.radioButtonPropCheckNum.Size = new System.Drawing.Size(14, 13);
            this.radioButtonPropCheckNum.TabIndex = 135;
            this.radioButtonPropCheckNum.TabStop = true;
            this.radioButtonPropCheckNum.UseVisualStyleBackColor = true;
            this.radioButtonPropCheckNum.CheckedChanged += new System.EventHandler(this.RadioButtonPropCheck_CheckedChanged);
            // 
            // radioButtonPropCheckString
            // 
            this.radioButtonPropCheckString.AutoSize = true;
            this.radioButtonPropCheckString.Location = new System.Drawing.Point(12, 69);
            this.radioButtonPropCheckString.Name = "radioButtonPropCheckString";
            this.radioButtonPropCheckString.Size = new System.Drawing.Size(14, 13);
            this.radioButtonPropCheckString.TabIndex = 136;
            this.radioButtonPropCheckString.TabStop = true;
            this.radioButtonPropCheckString.UseVisualStyleBackColor = true;
            this.radioButtonPropCheckString.CheckedChanged += new System.EventHandler(this.RadioButtonPropCheck_CheckedChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(500, 98);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 147;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonDone
            // 
            this.buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDone.Location = new System.Drawing.Point(413, 98);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(75, 23);
            this.buttonDone.TabIndex = 146;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // PropertyCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 133);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.radioButtonPropCheckString);
            this.Controls.Add(this.radioButtonPropCheckNum);
            this.Controls.Add(this.radioButtonPropCheckBool);
            this.Controls.Add(this.textBoxStringValue);
            this.Controls.Add(this.comboBoxOperString);
            this.Controls.Add(this.comboBoxStringProp);
            this.Controls.Add(this.textBoxNumValue);
            this.Controls.Add(this.comboBoxNumUnit);
            this.Controls.Add(this.comboBoxOperNum);
            this.Controls.Add(this.comboBoxNumProp);
            this.Controls.Add(this.comboBoxBoolValue);
            this.Controls.Add(this.comboBoxOperBool);
            this.Controls.Add(this.comboBoxBoolProp);
            this.Name = "PropertyCheckForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Property Check";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxStringValue;
        private System.Windows.Forms.ComboBox comboBoxOperString;
        private System.Windows.Forms.ComboBox comboBoxStringProp;
        private System.Windows.Forms.TextBox textBoxNumValue;
        private System.Windows.Forms.ComboBox comboBoxNumUnit;
        private System.Windows.Forms.ComboBox comboBoxOperNum;
        private System.Windows.Forms.ComboBox comboBoxNumProp;
        private System.Windows.Forms.ComboBox comboBoxBoolValue;
        private System.Windows.Forms.ComboBox comboBoxOperBool;
        private System.Windows.Forms.ComboBox comboBoxBoolProp;
        private System.Windows.Forms.RadioButton radioButtonPropCheckBool;
        private System.Windows.Forms.RadioButton radioButtonPropCheckNum;
        private System.Windows.Forms.RadioButton radioButtonPropCheckString;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonDone;
    }
}