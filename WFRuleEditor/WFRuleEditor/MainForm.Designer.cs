namespace BIMRuleEditor
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
            this.groupBoxGeneral = new System.Windows.Forms.GroupBox();
            this.richTextBoxDescription = new System.Windows.Forms.RichTextBox();
            this.comboBoxErrorLevel = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxRulesets = new System.Windows.Forms.GroupBox();
            this.buttonMoveRule = new System.Windows.Forms.Button();
            this.buttonAddRuleSet = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonImportRuleset = new System.Windows.Forms.Button();
            this.buttonDeleteRule = new System.Windows.Forms.Button();
            this.buttonExportRulesets = new System.Windows.Forms.Button();
            this.treeViewRulesets = new System.Windows.Forms.TreeView();
            this.buttonAddObjSearch = new System.Windows.Forms.Button();
            this.buttonEditObjSearch = new System.Windows.Forms.Button();
            this.buttonDeleteObjSearch = new System.Windows.Forms.Button();
            this.groupBoxObjSearch = new System.Windows.Forms.GroupBox();
            this.treeViewObjSearch = new System.Windows.Forms.TreeView();
            this.groupBoxLogicalExpressions = new System.Windows.Forms.GroupBox();
            this.treeViewLogicalExpressions = new System.Windows.Forms.TreeView();
            this.buttonDeleteLogicalExpressions = new System.Windows.Forms.Button();
            this.buttonAddLogicalExpressions = new System.Windows.Forms.Button();
            this.buttonEditLogicalExpressions = new System.Windows.Forms.Button();
            this.richTextBoxRuleString = new System.Windows.Forms.RichTextBox();
            this.buttonSaveAndExit = new System.Windows.Forms.Button();
            this.groupBoxGeneral.SuspendLayout();
            this.groupBoxRulesets.SuspendLayout();
            this.groupBoxObjSearch.SuspendLayout();
            this.groupBoxLogicalExpressions.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxGeneral
            // 
            this.groupBoxGeneral.Controls.Add(this.richTextBoxDescription);
            this.groupBoxGeneral.Controls.Add(this.comboBoxErrorLevel);
            this.groupBoxGeneral.Controls.Add(this.label3);
            this.groupBoxGeneral.Controls.Add(this.label2);
            this.groupBoxGeneral.Controls.Add(this.textBoxTitle);
            this.groupBoxGeneral.Controls.Add(this.label1);
            this.groupBoxGeneral.Location = new System.Drawing.Point(272, 12);
            this.groupBoxGeneral.Name = "groupBoxGeneral";
            this.groupBoxGeneral.Size = new System.Drawing.Size(213, 326);
            this.groupBoxGeneral.TabIndex = 0;
            this.groupBoxGeneral.TabStop = false;
            this.groupBoxGeneral.Text = "General";
            // 
            // richTextBoxDescription
            // 
            this.richTextBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxDescription.Location = new System.Drawing.Point(6, 89);
            this.richTextBoxDescription.Name = "richTextBoxDescription";
            this.richTextBoxDescription.Size = new System.Drawing.Size(201, 231);
            this.richTextBoxDescription.TabIndex = 5;
            this.richTextBoxDescription.Text = "";
            this.richTextBoxDescription.TextChanged += new System.EventHandler(this.richTextBoxDescription_TextChanged);
            // 
            // comboBoxErrorLevel
            // 
            this.comboBoxErrorLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxErrorLevel.FormattingEnabled = true;
            this.comboBoxErrorLevel.Location = new System.Drawing.Point(73, 45);
            this.comboBoxErrorLevel.Name = "comboBoxErrorLevel";
            this.comboBoxErrorLevel.Size = new System.Drawing.Size(134, 21);
            this.comboBoxErrorLevel.TabIndex = 4;
            this.comboBoxErrorLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxErrorLevel_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Description:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Error Level:";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTitle.Location = new System.Drawing.Point(50, 19);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(157, 20);
            this.textBoxTitle.TabIndex = 1;
            this.textBoxTitle.TextChanged += new System.EventHandler(this.textBoxTitle_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // groupBoxRulesets
            // 
            this.groupBoxRulesets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxRulesets.Controls.Add(this.buttonMoveRule);
            this.groupBoxRulesets.Controls.Add(this.buttonAddRuleSet);
            this.groupBoxRulesets.Controls.Add(this.buttonAdd);
            this.groupBoxRulesets.Controls.Add(this.buttonImportRuleset);
            this.groupBoxRulesets.Controls.Add(this.buttonDeleteRule);
            this.groupBoxRulesets.Controls.Add(this.buttonExportRulesets);
            this.groupBoxRulesets.Controls.Add(this.treeViewRulesets);
            this.groupBoxRulesets.Location = new System.Drawing.Point(12, 12);
            this.groupBoxRulesets.Name = "groupBoxRulesets";
            this.groupBoxRulesets.Size = new System.Drawing.Size(254, 479);
            this.groupBoxRulesets.TabIndex = 2;
            this.groupBoxRulesets.TabStop = false;
            this.groupBoxRulesets.Text = "Rulesets";
            // 
            // buttonMoveRule
            // 
            this.buttonMoveRule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveRule.Location = new System.Drawing.Point(6, 305);
            this.buttonMoveRule.Name = "buttonMoveRule";
            this.buttonMoveRule.Size = new System.Drawing.Size(242, 23);
            this.buttonMoveRule.TabIndex = 6;
            this.buttonMoveRule.Text = "Move Selected Rule(s)";
            this.buttonMoveRule.UseVisualStyleBackColor = true;
            this.buttonMoveRule.Click += new System.EventHandler(this.ButtonMoveRule_Click);
            // 
            // buttonAddRuleSet
            // 
            this.buttonAddRuleSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddRuleSet.Location = new System.Drawing.Point(6, 334);
            this.buttonAddRuleSet.Name = "buttonAddRuleSet";
            this.buttonAddRuleSet.Size = new System.Drawing.Size(242, 23);
            this.buttonAddRuleSet.TabIndex = 5;
            this.buttonAddRuleSet.Text = "Add Ruleset";
            this.buttonAddRuleSet.UseVisualStyleBackColor = true;
            this.buttonAddRuleSet.Click += new System.EventHandler(this.buttonAddRuleSet_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.Location = new System.Drawing.Point(6, 363);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(242, 23);
            this.buttonAdd.TabIndex = 4;
            this.buttonAdd.Text = "Add Rule";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAddRule_Click);
            // 
            // buttonImportRuleset
            // 
            this.buttonImportRuleset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImportRuleset.Location = new System.Drawing.Point(6, 421);
            this.buttonImportRuleset.Name = "buttonImportRuleset";
            this.buttonImportRuleset.Size = new System.Drawing.Size(242, 23);
            this.buttonImportRuleset.TabIndex = 3;
            this.buttonImportRuleset.Text = "Import";
            this.buttonImportRuleset.UseVisualStyleBackColor = true;
            this.buttonImportRuleset.Click += new System.EventHandler(this.buttonImportRuleset_Click);
            // 
            // buttonDeleteRule
            // 
            this.buttonDeleteRule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteRule.Location = new System.Drawing.Point(6, 392);
            this.buttonDeleteRule.Name = "buttonDeleteRule";
            this.buttonDeleteRule.Size = new System.Drawing.Size(242, 23);
            this.buttonDeleteRule.TabIndex = 2;
            this.buttonDeleteRule.Text = "Delete Selected";
            this.buttonDeleteRule.UseVisualStyleBackColor = true;
            this.buttonDeleteRule.Click += new System.EventHandler(this.buttonDeleteRule_Click);
            // 
            // buttonExportRulesets
            // 
            this.buttonExportRulesets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExportRulesets.Location = new System.Drawing.Point(6, 450);
            this.buttonExportRulesets.Name = "buttonExportRulesets";
            this.buttonExportRulesets.Size = new System.Drawing.Size(242, 23);
            this.buttonExportRulesets.TabIndex = 1;
            this.buttonExportRulesets.Text = "Export";
            this.buttonExportRulesets.UseVisualStyleBackColor = true;
            this.buttonExportRulesets.Click += new System.EventHandler(this.buttonExportRulesets_Click);
            // 
            // treeViewRulesets
            // 
            this.treeViewRulesets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewRulesets.CheckBoxes = true;
            this.treeViewRulesets.Location = new System.Drawing.Point(6, 19);
            this.treeViewRulesets.Name = "treeViewRulesets";
            this.treeViewRulesets.Size = new System.Drawing.Size(242, 280);
            this.treeViewRulesets.TabIndex = 0;
            this.treeViewRulesets.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewRulesets_NodeMouseClick);
            this.treeViewRulesets.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeViewRulesets_KeyUp);
            // 
            // buttonAddObjSearch
            // 
            this.buttonAddObjSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddObjSearch.Location = new System.Drawing.Point(719, 19);
            this.buttonAddObjSearch.Name = "buttonAddObjSearch";
            this.buttonAddObjSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonAddObjSearch.TabIndex = 5;
            this.buttonAddObjSearch.Text = "Add";
            this.buttonAddObjSearch.UseVisualStyleBackColor = true;
            this.buttonAddObjSearch.Click += new System.EventHandler(this.buttonAddObjSearch_Click);
            // 
            // buttonEditObjSearch
            // 
            this.buttonEditObjSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEditObjSearch.Location = new System.Drawing.Point(719, 48);
            this.buttonEditObjSearch.Name = "buttonEditObjSearch";
            this.buttonEditObjSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonEditObjSearch.TabIndex = 6;
            this.buttonEditObjSearch.Text = "Edit";
            this.buttonEditObjSearch.UseVisualStyleBackColor = true;
            this.buttonEditObjSearch.Click += new System.EventHandler(this.buttonEditObjSearch_Click);
            // 
            // buttonDeleteObjSearch
            // 
            this.buttonDeleteObjSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteObjSearch.Location = new System.Drawing.Point(719, 110);
            this.buttonDeleteObjSearch.Name = "buttonDeleteObjSearch";
            this.buttonDeleteObjSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteObjSearch.TabIndex = 7;
            this.buttonDeleteObjSearch.Text = "Delete";
            this.buttonDeleteObjSearch.UseVisualStyleBackColor = true;
            this.buttonDeleteObjSearch.Click += new System.EventHandler(this.buttonDeleteObjSearch_Click);
            // 
            // groupBoxObjSearch
            // 
            this.groupBoxObjSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxObjSearch.Controls.Add(this.treeViewObjSearch);
            this.groupBoxObjSearch.Controls.Add(this.buttonDeleteObjSearch);
            this.groupBoxObjSearch.Controls.Add(this.buttonAddObjSearch);
            this.groupBoxObjSearch.Controls.Add(this.buttonEditObjSearch);
            this.groupBoxObjSearch.Location = new System.Drawing.Point(491, 12);
            this.groupBoxObjSearch.Name = "groupBoxObjSearch";
            this.groupBoxObjSearch.Size = new System.Drawing.Size(800, 139);
            this.groupBoxObjSearch.TabIndex = 8;
            this.groupBoxObjSearch.TabStop = false;
            this.groupBoxObjSearch.Text = "Object Search";
            // 
            // treeViewObjSearch
            // 
            this.treeViewObjSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewObjSearch.Location = new System.Drawing.Point(6, 19);
            this.treeViewObjSearch.Name = "treeViewObjSearch";
            this.treeViewObjSearch.Size = new System.Drawing.Size(707, 114);
            this.treeViewObjSearch.TabIndex = 9;
            // 
            // groupBoxLogicalExpressions
            // 
            this.groupBoxLogicalExpressions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLogicalExpressions.Controls.Add(this.treeViewLogicalExpressions);
            this.groupBoxLogicalExpressions.Controls.Add(this.buttonDeleteLogicalExpressions);
            this.groupBoxLogicalExpressions.Controls.Add(this.buttonAddLogicalExpressions);
            this.groupBoxLogicalExpressions.Controls.Add(this.buttonEditLogicalExpressions);
            this.groupBoxLogicalExpressions.Location = new System.Drawing.Point(491, 157);
            this.groupBoxLogicalExpressions.Name = "groupBoxLogicalExpressions";
            this.groupBoxLogicalExpressions.Size = new System.Drawing.Size(800, 181);
            this.groupBoxLogicalExpressions.TabIndex = 9;
            this.groupBoxLogicalExpressions.TabStop = false;
            this.groupBoxLogicalExpressions.Text = "Logical Expression";
            // 
            // treeViewLogicalExpressions
            // 
            this.treeViewLogicalExpressions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewLogicalExpressions.Location = new System.Drawing.Point(6, 19);
            this.treeViewLogicalExpressions.Name = "treeViewLogicalExpressions";
            this.treeViewLogicalExpressions.Size = new System.Drawing.Size(707, 156);
            this.treeViewLogicalExpressions.TabIndex = 8;
            // 
            // buttonDeleteLogicalExpressions
            // 
            this.buttonDeleteLogicalExpressions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteLogicalExpressions.Location = new System.Drawing.Point(719, 152);
            this.buttonDeleteLogicalExpressions.Name = "buttonDeleteLogicalExpressions";
            this.buttonDeleteLogicalExpressions.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteLogicalExpressions.TabIndex = 7;
            this.buttonDeleteLogicalExpressions.Text = "Delete";
            this.buttonDeleteLogicalExpressions.UseVisualStyleBackColor = true;
            this.buttonDeleteLogicalExpressions.Click += new System.EventHandler(this.buttonDeleteLogicalExpressions_Click);
            // 
            // buttonAddLogicalExpressions
            // 
            this.buttonAddLogicalExpressions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddLogicalExpressions.Location = new System.Drawing.Point(719, 19);
            this.buttonAddLogicalExpressions.Name = "buttonAddLogicalExpressions";
            this.buttonAddLogicalExpressions.Size = new System.Drawing.Size(75, 23);
            this.buttonAddLogicalExpressions.TabIndex = 5;
            this.buttonAddLogicalExpressions.Text = "Add";
            this.buttonAddLogicalExpressions.UseVisualStyleBackColor = true;
            this.buttonAddLogicalExpressions.Click += new System.EventHandler(this.buttonAddLogicalExpressions_Click);
            // 
            // buttonEditLogicalExpressions
            // 
            this.buttonEditLogicalExpressions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEditLogicalExpressions.Location = new System.Drawing.Point(719, 48);
            this.buttonEditLogicalExpressions.Name = "buttonEditLogicalExpressions";
            this.buttonEditLogicalExpressions.Size = new System.Drawing.Size(75, 23);
            this.buttonEditLogicalExpressions.TabIndex = 6;
            this.buttonEditLogicalExpressions.Text = "Edit";
            this.buttonEditLogicalExpressions.UseVisualStyleBackColor = true;
            this.buttonEditLogicalExpressions.Click += new System.EventHandler(this.buttonEditLogicalExpressions_Click);
            // 
            // richTextBoxRuleString
            // 
            this.richTextBoxRuleString.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxRuleString.Location = new System.Drawing.Point(272, 344);
            this.richTextBoxRuleString.Name = "richTextBoxRuleString";
            this.richTextBoxRuleString.ReadOnly = true;
            this.richTextBoxRuleString.Size = new System.Drawing.Size(932, 147);
            this.richTextBoxRuleString.TabIndex = 10;
            this.richTextBoxRuleString.Text = "";
            // 
            // buttonSaveAndExit
            // 
            this.buttonSaveAndExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveAndExit.Location = new System.Drawing.Point(1210, 462);
            this.buttonSaveAndExit.Name = "buttonSaveAndExit";
            this.buttonSaveAndExit.Size = new System.Drawing.Size(81, 29);
            this.buttonSaveAndExit.TabIndex = 11;
            this.buttonSaveAndExit.Text = "Save and Exit";
            this.buttonSaveAndExit.UseVisualStyleBackColor = true;
            this.buttonSaveAndExit.Click += new System.EventHandler(this.ButtonSaveAndExit_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1303, 503);
            this.Controls.Add(this.buttonSaveAndExit);
            this.Controls.Add(this.richTextBoxRuleString);
            this.Controls.Add(this.groupBoxLogicalExpressions);
            this.Controls.Add(this.groupBoxObjSearch);
            this.Controls.Add(this.groupBoxRulesets);
            this.Controls.Add(this.groupBoxGeneral);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rule Editor";
            this.groupBoxGeneral.ResumeLayout(false);
            this.groupBoxGeneral.PerformLayout();
            this.groupBoxRulesets.ResumeLayout(false);
            this.groupBoxObjSearch.ResumeLayout(false);
            this.groupBoxLogicalExpressions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxGeneral;
        private System.Windows.Forms.RichTextBox richTextBoxDescription;
        private System.Windows.Forms.ComboBox comboBoxErrorLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxRulesets;
        private System.Windows.Forms.Button buttonImportRuleset;
        private System.Windows.Forms.Button buttonDeleteRule;
        private System.Windows.Forms.Button buttonExportRulesets;
        private System.Windows.Forms.TreeView treeViewRulesets;
        private System.Windows.Forms.Button buttonAddObjSearch;
        private System.Windows.Forms.Button buttonEditObjSearch;
        private System.Windows.Forms.Button buttonDeleteObjSearch;
        private System.Windows.Forms.GroupBox groupBoxObjSearch;
        private System.Windows.Forms.TreeView treeViewObjSearch;
        private System.Windows.Forms.GroupBox groupBoxLogicalExpressions;
        private System.Windows.Forms.TreeView treeViewLogicalExpressions;
        private System.Windows.Forms.Button buttonDeleteLogicalExpressions;
        private System.Windows.Forms.Button buttonAddLogicalExpressions;
        private System.Windows.Forms.Button buttonEditLogicalExpressions;
        private System.Windows.Forms.RichTextBox richTextBoxRuleString;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonAddRuleSet;
        private System.Windows.Forms.Button buttonMoveRule;
        private System.Windows.Forms.Button buttonSaveAndExit;
    }
}

