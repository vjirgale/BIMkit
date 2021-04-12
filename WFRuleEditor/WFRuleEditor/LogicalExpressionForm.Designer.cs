namespace BIMRuleEditor
{
    partial class LogicalExpressionForm
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
            this.groupBoxNewLE = new System.Windows.Forms.GroupBox();
            this.radioButtonXOR = new System.Windows.Forms.RadioButton();
            this.radioButtonOR = new System.Windows.Forms.RadioButton();
            this.radioButtonAND = new System.Windows.Forms.RadioButton();
            this.radioButtonNewRC = new System.Windows.Forms.RadioButton();
            this.radioButtonNewOC = new System.Windows.Forms.RadioButton();
            this.radioButtonNewLE = new System.Windows.Forms.RadioButton();
            this.groupBoxNewOC = new System.Windows.Forms.GroupBox();
            this.comboBoxNegOC = new System.Windows.Forms.ComboBox();
            this.buttonEditPropertyOC = new System.Windows.Forms.Button();
            this.treeViewPropertiesOC = new System.Windows.Forms.TreeView();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxObjIndexOC = new System.Windows.Forms.ComboBox();
            this.groupBoxNewRC = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxObjIndex2RC = new System.Windows.Forms.ComboBox();
            this.comboBoxNegRC = new System.Windows.Forms.ComboBox();
            this.buttonEditPropertyRC = new System.Windows.Forms.Button();
            this.treeViewPropertiesRC = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxObjIndex1RC = new System.Windows.Forms.ComboBox();
            this.buttonDone = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxNewLE.SuspendLayout();
            this.groupBoxNewOC.SuspendLayout();
            this.groupBoxNewRC.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxNewLE
            // 
            this.groupBoxNewLE.Controls.Add(this.radioButtonXOR);
            this.groupBoxNewLE.Controls.Add(this.radioButtonOR);
            this.groupBoxNewLE.Controls.Add(this.radioButtonAND);
            this.groupBoxNewLE.Location = new System.Drawing.Point(32, 12);
            this.groupBoxNewLE.Name = "groupBoxNewLE";
            this.groupBoxNewLE.Size = new System.Drawing.Size(164, 47);
            this.groupBoxNewLE.TabIndex = 0;
            this.groupBoxNewLE.TabStop = false;
            this.groupBoxNewLE.Text = "New Logical Expression";
            // 
            // radioButtonXOR
            // 
            this.radioButtonXOR.AutoSize = true;
            this.radioButtonXOR.Location = new System.Drawing.Point(107, 19);
            this.radioButtonXOR.Name = "radioButtonXOR";
            this.radioButtonXOR.Size = new System.Drawing.Size(48, 17);
            this.radioButtonXOR.TabIndex = 2;
            this.radioButtonXOR.TabStop = true;
            this.radioButtonXOR.Text = "XOR";
            this.radioButtonXOR.UseVisualStyleBackColor = true;
            this.radioButtonXOR.CheckedChanged += new System.EventHandler(this.RadioButtonLogicOp_CheckedChanged);
            // 
            // radioButtonOR
            // 
            this.radioButtonOR.AutoSize = true;
            this.radioButtonOR.Location = new System.Drawing.Point(60, 19);
            this.radioButtonOR.Name = "radioButtonOR";
            this.radioButtonOR.Size = new System.Drawing.Size(41, 17);
            this.radioButtonOR.TabIndex = 1;
            this.radioButtonOR.TabStop = true;
            this.radioButtonOR.Text = "OR";
            this.radioButtonOR.UseVisualStyleBackColor = true;
            this.radioButtonOR.CheckedChanged += new System.EventHandler(this.RadioButtonLogicOp_CheckedChanged);
            // 
            // radioButtonAND
            // 
            this.radioButtonAND.AutoSize = true;
            this.radioButtonAND.Location = new System.Drawing.Point(6, 19);
            this.radioButtonAND.Name = "radioButtonAND";
            this.radioButtonAND.Size = new System.Drawing.Size(48, 17);
            this.radioButtonAND.TabIndex = 0;
            this.radioButtonAND.TabStop = true;
            this.radioButtonAND.Text = "AND";
            this.radioButtonAND.UseVisualStyleBackColor = true;
            this.radioButtonAND.CheckedChanged += new System.EventHandler(this.RadioButtonLogicOp_CheckedChanged);
            // 
            // radioButtonNewRC
            // 
            this.radioButtonNewRC.AutoSize = true;
            this.radioButtonNewRC.Location = new System.Drawing.Point(12, 157);
            this.radioButtonNewRC.Name = "radioButtonNewRC";
            this.radioButtonNewRC.Size = new System.Drawing.Size(14, 13);
            this.radioButtonNewRC.TabIndex = 139;
            this.radioButtonNewRC.TabStop = true;
            this.radioButtonNewRC.UseVisualStyleBackColor = true;
            this.radioButtonNewRC.CheckedChanged += new System.EventHandler(this.RadioButtonNewLE_OC_RC_CheckedChanged);
            // 
            // radioButtonNewOC
            // 
            this.radioButtonNewOC.AutoSize = true;
            this.radioButtonNewOC.Location = new System.Drawing.Point(12, 65);
            this.radioButtonNewOC.Name = "radioButtonNewOC";
            this.radioButtonNewOC.Size = new System.Drawing.Size(14, 13);
            this.radioButtonNewOC.TabIndex = 138;
            this.radioButtonNewOC.TabStop = true;
            this.radioButtonNewOC.UseVisualStyleBackColor = true;
            this.radioButtonNewOC.CheckedChanged += new System.EventHandler(this.RadioButtonNewLE_OC_RC_CheckedChanged);
            // 
            // radioButtonNewLE
            // 
            this.radioButtonNewLE.AutoSize = true;
            this.radioButtonNewLE.Location = new System.Drawing.Point(12, 12);
            this.radioButtonNewLE.Name = "radioButtonNewLE";
            this.radioButtonNewLE.Size = new System.Drawing.Size(14, 13);
            this.radioButtonNewLE.TabIndex = 137;
            this.radioButtonNewLE.TabStop = true;
            this.radioButtonNewLE.UseVisualStyleBackColor = true;
            this.radioButtonNewLE.CheckedChanged += new System.EventHandler(this.RadioButtonNewLE_OC_RC_CheckedChanged);
            // 
            // groupBoxNewOC
            // 
            this.groupBoxNewOC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxNewOC.Controls.Add(this.comboBoxNegOC);
            this.groupBoxNewOC.Controls.Add(this.buttonEditPropertyOC);
            this.groupBoxNewOC.Controls.Add(this.treeViewPropertiesOC);
            this.groupBoxNewOC.Controls.Add(this.label4);
            this.groupBoxNewOC.Controls.Add(this.label7);
            this.groupBoxNewOC.Controls.Add(this.label11);
            this.groupBoxNewOC.Controls.Add(this.comboBoxObjIndexOC);
            this.groupBoxNewOC.Location = new System.Drawing.Point(32, 65);
            this.groupBoxNewOC.Name = "groupBoxNewOC";
            this.groupBoxNewOC.Size = new System.Drawing.Size(523, 86);
            this.groupBoxNewOC.TabIndex = 140;
            this.groupBoxNewOC.TabStop = false;
            this.groupBoxNewOC.Text = "New Object Check";
            // 
            // comboBoxNegOC
            // 
            this.comboBoxNegOC.FormattingEnabled = true;
            this.comboBoxNegOC.Location = new System.Drawing.Point(79, 59);
            this.comboBoxNegOC.Name = "comboBoxNegOC";
            this.comboBoxNegOC.Size = new System.Drawing.Size(158, 21);
            this.comboBoxNegOC.TabIndex = 129;
            this.comboBoxNegOC.SelectedIndexChanged += new System.EventHandler(this.comboBoxNegOC_SelectedIndexChanged);
            // 
            // buttonEditPropertyOC
            // 
            this.buttonEditPropertyOC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEditPropertyOC.Location = new System.Drawing.Point(442, 32);
            this.buttonEditPropertyOC.Name = "buttonEditPropertyOC";
            this.buttonEditPropertyOC.Size = new System.Drawing.Size(75, 23);
            this.buttonEditPropertyOC.TabIndex = 127;
            this.buttonEditPropertyOC.Text = "Edit";
            this.buttonEditPropertyOC.UseVisualStyleBackColor = true;
            this.buttonEditPropertyOC.Click += new System.EventHandler(this.buttonEditPropertyOC_Click);
            // 
            // treeViewPropertiesOC
            // 
            this.treeViewPropertiesOC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewPropertiesOC.Location = new System.Drawing.Point(243, 32);
            this.treeViewPropertiesOC.Name = "treeViewPropertiesOC";
            this.treeViewPropertiesOC.Size = new System.Drawing.Size(193, 48);
            this.treeViewPropertiesOC.TabIndex = 125;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(240, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 124;
            this.label4.Text = "Property Check";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 123;
            this.label7.Text = "Object Index";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 62);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 13);
            this.label11.TabIndex = 121;
            this.label11.Text = "Negation";
            // 
            // comboBoxObjIndexOC
            // 
            this.comboBoxObjIndexOC.FormattingEnabled = true;
            this.comboBoxObjIndexOC.Location = new System.Drawing.Point(79, 32);
            this.comboBoxObjIndexOC.Name = "comboBoxObjIndexOC";
            this.comboBoxObjIndexOC.Size = new System.Drawing.Size(158, 21);
            this.comboBoxObjIndexOC.TabIndex = 119;
            this.comboBoxObjIndexOC.SelectedIndexChanged += new System.EventHandler(this.comboBoxObjIndexOC_SelectedIndexChanged);
            // 
            // groupBoxNewRC
            // 
            this.groupBoxNewRC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxNewRC.Controls.Add(this.label5);
            this.groupBoxNewRC.Controls.Add(this.comboBoxObjIndex2RC);
            this.groupBoxNewRC.Controls.Add(this.comboBoxNegRC);
            this.groupBoxNewRC.Controls.Add(this.buttonEditPropertyRC);
            this.groupBoxNewRC.Controls.Add(this.treeViewPropertiesRC);
            this.groupBoxNewRC.Controls.Add(this.label1);
            this.groupBoxNewRC.Controls.Add(this.label2);
            this.groupBoxNewRC.Controls.Add(this.label3);
            this.groupBoxNewRC.Controls.Add(this.comboBoxObjIndex1RC);
            this.groupBoxNewRC.Location = new System.Drawing.Point(32, 157);
            this.groupBoxNewRC.Name = "groupBoxNewRC";
            this.groupBoxNewRC.Size = new System.Drawing.Size(523, 113);
            this.groupBoxNewRC.TabIndex = 141;
            this.groupBoxNewRC.TabStop = false;
            this.groupBoxNewRC.Text = "New Relation Check";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 131;
            this.label5.Text = "Object 2 Index";
            // 
            // comboBoxObjIndex2RC
            // 
            this.comboBoxObjIndex2RC.FormattingEnabled = true;
            this.comboBoxObjIndex2RC.Location = new System.Drawing.Point(88, 59);
            this.comboBoxObjIndex2RC.Name = "comboBoxObjIndex2RC";
            this.comboBoxObjIndex2RC.Size = new System.Drawing.Size(149, 21);
            this.comboBoxObjIndex2RC.TabIndex = 130;
            this.comboBoxObjIndex2RC.SelectedIndexChanged += new System.EventHandler(this.comboBoxObjIndex2RC_SelectedIndexChanged);
            // 
            // comboBoxNegRC
            // 
            this.comboBoxNegRC.FormattingEnabled = true;
            this.comboBoxNegRC.Location = new System.Drawing.Point(88, 86);
            this.comboBoxNegRC.Name = "comboBoxNegRC";
            this.comboBoxNegRC.Size = new System.Drawing.Size(149, 21);
            this.comboBoxNegRC.TabIndex = 129;
            this.comboBoxNegRC.SelectedIndexChanged += new System.EventHandler(this.comboBoxNegRC_SelectedIndexChanged);
            // 
            // buttonEditPropertyRC
            // 
            this.buttonEditPropertyRC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEditPropertyRC.Location = new System.Drawing.Point(442, 32);
            this.buttonEditPropertyRC.Name = "buttonEditPropertyRC";
            this.buttonEditPropertyRC.Size = new System.Drawing.Size(75, 23);
            this.buttonEditPropertyRC.TabIndex = 127;
            this.buttonEditPropertyRC.Text = "Edit";
            this.buttonEditPropertyRC.UseVisualStyleBackColor = true;
            this.buttonEditPropertyRC.Click += new System.EventHandler(this.buttonEditPropertyRC_Click);
            // 
            // treeViewPropertiesRC
            // 
            this.treeViewPropertiesRC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewPropertiesRC.Location = new System.Drawing.Point(243, 32);
            this.treeViewPropertiesRC.Name = "treeViewPropertiesRC";
            this.treeViewPropertiesRC.Size = new System.Drawing.Size(193, 75);
            this.treeViewPropertiesRC.TabIndex = 125;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(240, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 124;
            this.label1.Text = "Property Check";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 123;
            this.label2.Text = "Object 1 Index";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 121;
            this.label3.Text = "Negation";
            // 
            // comboBoxObjIndex1RC
            // 
            this.comboBoxObjIndex1RC.FormattingEnabled = true;
            this.comboBoxObjIndex1RC.Location = new System.Drawing.Point(88, 32);
            this.comboBoxObjIndex1RC.Name = "comboBoxObjIndex1RC";
            this.comboBoxObjIndex1RC.Size = new System.Drawing.Size(149, 21);
            this.comboBoxObjIndex1RC.TabIndex = 119;
            this.comboBoxObjIndex1RC.SelectedIndexChanged += new System.EventHandler(this.comboBoxObjIndex1RC_SelectedIndexChanged);
            // 
            // buttonDone
            // 
            this.buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDone.Location = new System.Drawing.Point(393, 280);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(75, 23);
            this.buttonDone.TabIndex = 142;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(480, 280);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 143;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // LogicalExpressionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 315);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.groupBoxNewRC);
            this.Controls.Add(this.groupBoxNewOC);
            this.Controls.Add(this.radioButtonNewRC);
            this.Controls.Add(this.radioButtonNewOC);
            this.Controls.Add(this.radioButtonNewLE);
            this.Controls.Add(this.groupBoxNewLE);
            this.Name = "LogicalExpressionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Logical Expression";
            this.groupBoxNewLE.ResumeLayout(false);
            this.groupBoxNewLE.PerformLayout();
            this.groupBoxNewOC.ResumeLayout(false);
            this.groupBoxNewOC.PerformLayout();
            this.groupBoxNewRC.ResumeLayout(false);
            this.groupBoxNewRC.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxNewLE;
        private System.Windows.Forms.RadioButton radioButtonXOR;
        private System.Windows.Forms.RadioButton radioButtonOR;
        private System.Windows.Forms.RadioButton radioButtonAND;
        private System.Windows.Forms.RadioButton radioButtonNewRC;
        private System.Windows.Forms.RadioButton radioButtonNewOC;
        private System.Windows.Forms.RadioButton radioButtonNewLE;
        private System.Windows.Forms.GroupBox groupBoxNewOC;
        private System.Windows.Forms.ComboBox comboBoxNegOC;
        private System.Windows.Forms.Button buttonEditPropertyOC;
        private System.Windows.Forms.TreeView treeViewPropertiesOC;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxObjIndexOC;
        private System.Windows.Forms.GroupBox groupBoxNewRC;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxObjIndex2RC;
        private System.Windows.Forms.ComboBox comboBoxNegRC;
        private System.Windows.Forms.Button buttonEditPropertyRC;
        private System.Windows.Forms.TreeView treeViewPropertiesRC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxObjIndex1RC;
        private System.Windows.Forms.Button buttonDone;
        private System.Windows.Forms.Button buttonCancel;
    }
}