namespace RuleAdminApp
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonUpdateUserData = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPublicName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkedListBoxRuleSets = new System.Windows.Forms.CheckedListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonRemoveRule = new System.Windows.Forms.Button();
            this.checkedListBoxRules = new System.Windows.Forms.CheckedListBox();
            this.richTextBoxRuleDisplayText = new System.Windows.Forms.RichTextBox();
            this.richTextBoxRuleDisplayJSON = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.buttonAddRules = new System.Windows.Forms.Button();
            this.buttonDeleteRuleItem = new System.Windows.Forms.Button();
            this.buttonDownloadRuleSetLocally = new System.Windows.Forms.Button();
            this.buttonUploadLocalRuleSet = new System.Windows.Forms.Button();
            this.buttonSignout = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonMethodCheck = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonUpdateUserData);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxPublicName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxUsername);
            this.groupBox1.Location = new System.Drawing.Point(12, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(284, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Info";
            // 
            // buttonUpdateUserData
            // 
            this.buttonUpdateUserData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdateUserData.Location = new System.Drawing.Point(82, 71);
            this.buttonUpdateUserData.Name = "buttonUpdateUserData";
            this.buttonUpdateUserData.Size = new System.Drawing.Size(196, 23);
            this.buttonUpdateUserData.TabIndex = 4;
            this.buttonUpdateUserData.Text = "Update User Data";
            this.buttonUpdateUserData.UseVisualStyleBackColor = true;
            this.buttonUpdateUserData.Click += new System.EventHandler(this.buttonUpdateUserData_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Public Name:";
            // 
            // textBoxPublicName
            // 
            this.textBoxPublicName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPublicName.Location = new System.Drawing.Point(82, 45);
            this.textBoxPublicName.Name = "textBoxPublicName";
            this.textBoxPublicName.Size = new System.Drawing.Size(196, 20);
            this.textBoxPublicName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Username:";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUsername.Location = new System.Drawing.Point(82, 19);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(196, 20);
            this.textBoxUsername.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkedListBoxRuleSets);
            this.groupBox2.Location = new System.Drawing.Point(12, 155);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(284, 240);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RuleSets:";
            // 
            // checkedListBoxRuleSets
            // 
            this.checkedListBoxRuleSets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxRuleSets.FormattingEnabled = true;
            this.checkedListBoxRuleSets.Location = new System.Drawing.Point(6, 19);
            this.checkedListBoxRuleSets.Name = "checkedListBoxRuleSets";
            this.checkedListBoxRuleSets.Size = new System.Drawing.Size(272, 214);
            this.checkedListBoxRuleSets.TabIndex = 0;
            this.checkedListBoxRuleSets.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxRuleSets_ItemCheck);
            this.checkedListBoxRuleSets.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxRuleSets_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonRemoveRule);
            this.groupBox3.Controls.Add(this.checkedListBoxRules);
            this.groupBox3.Location = new System.Drawing.Point(302, 155);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(284, 240);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Rules:";
            // 
            // buttonRemoveRule
            // 
            this.buttonRemoveRule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveRule.Location = new System.Drawing.Point(6, 211);
            this.buttonRemoveRule.Name = "buttonRemoveRule";
            this.buttonRemoveRule.Size = new System.Drawing.Size(272, 23);
            this.buttonRemoveRule.TabIndex = 2;
            this.buttonRemoveRule.Text = "Remove Checked Rules From RuleSet";
            this.buttonRemoveRule.UseVisualStyleBackColor = true;
            this.buttonRemoveRule.Click += new System.EventHandler(this.buttonRemoveRule_Click);
            // 
            // checkedListBoxRules
            // 
            this.checkedListBoxRules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxRules.FormattingEnabled = true;
            this.checkedListBoxRules.Location = new System.Drawing.Point(6, 19);
            this.checkedListBoxRules.Name = "checkedListBoxRules";
            this.checkedListBoxRules.Size = new System.Drawing.Size(272, 184);
            this.checkedListBoxRules.TabIndex = 1;
            this.checkedListBoxRules.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxRules_SelectedIndexChanged);
            // 
            // richTextBoxRuleDisplayText
            // 
            this.richTextBoxRuleDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxRuleDisplayText.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxRuleDisplayText.Name = "richTextBoxRuleDisplayText";
            this.richTextBoxRuleDisplayText.ReadOnly = true;
            this.richTextBoxRuleDisplayText.Size = new System.Drawing.Size(568, 108);
            this.richTextBoxRuleDisplayText.TabIndex = 3;
            this.richTextBoxRuleDisplayText.Text = "";
            // 
            // richTextBoxRuleDisplayJSON
            // 
            this.richTextBoxRuleDisplayJSON.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxRuleDisplayJSON.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxRuleDisplayJSON.Name = "richTextBoxRuleDisplayJSON";
            this.richTextBoxRuleDisplayJSON.ReadOnly = true;
            this.richTextBoxRuleDisplayJSON.Size = new System.Drawing.Size(287, 354);
            this.richTextBoxRuleDisplayJSON.TabIndex = 4;
            this.richTextBoxRuleDisplayJSON.Text = "";
            this.richTextBoxRuleDisplayJSON.WordWrap = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.richTextBoxRuleDisplayJSON);
            this.groupBox4.Location = new System.Drawing.Point(592, 155);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(293, 373);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Rule Display JSON";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.richTextBoxRuleDisplayText);
            this.groupBox5.Location = new System.Drawing.Point(12, 401);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(574, 127);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Rule Display Text";
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.buttonMethodCheck);
            this.groupBox6.Controls.Add(this.buttonAddRules);
            this.groupBox6.Controls.Add(this.buttonDeleteRuleItem);
            this.groupBox6.Controls.Add(this.buttonDownloadRuleSetLocally);
            this.groupBox6.Controls.Add(this.buttonUploadLocalRuleSet);
            this.groupBox6.Location = new System.Drawing.Point(308, 49);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(577, 100);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Options";
            // 
            // buttonAddRules
            // 
            this.buttonAddRules.Location = new System.Drawing.Point(168, 19);
            this.buttonAddRules.Name = "buttonAddRules";
            this.buttonAddRules.Size = new System.Drawing.Size(75, 75);
            this.buttonAddRules.TabIndex = 3;
            this.buttonAddRules.Text = "Add Rules from Other Users";
            this.buttonAddRules.UseVisualStyleBackColor = true;
            this.buttonAddRules.Click += new System.EventHandler(this.buttonAddRules_Click);
            // 
            // buttonDeleteRuleItem
            // 
            this.buttonDeleteRuleItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteRuleItem.Location = new System.Drawing.Point(496, 19);
            this.buttonDeleteRuleItem.Name = "buttonDeleteRuleItem";
            this.buttonDeleteRuleItem.Size = new System.Drawing.Size(75, 75);
            this.buttonDeleteRuleItem.TabIndex = 2;
            this.buttonDeleteRuleItem.Text = "Delete Checked Rules and RuleSets";
            this.buttonDeleteRuleItem.UseVisualStyleBackColor = true;
            this.buttonDeleteRuleItem.Click += new System.EventHandler(this.buttonDeleteRuleItem_Click);
            // 
            // buttonDownloadRuleSetLocally
            // 
            this.buttonDownloadRuleSetLocally.Location = new System.Drawing.Point(87, 19);
            this.buttonDownloadRuleSetLocally.Name = "buttonDownloadRuleSetLocally";
            this.buttonDownloadRuleSetLocally.Size = new System.Drawing.Size(75, 75);
            this.buttonDownloadRuleSetLocally.TabIndex = 1;
            this.buttonDownloadRuleSetLocally.Text = "Download RuleSets Locally";
            this.buttonDownloadRuleSetLocally.UseVisualStyleBackColor = true;
            this.buttonDownloadRuleSetLocally.Click += new System.EventHandler(this.buttonDownloadRuleSetLocally_Click);
            // 
            // buttonUploadLocalRuleSet
            // 
            this.buttonUploadLocalRuleSet.Location = new System.Drawing.Point(6, 19);
            this.buttonUploadLocalRuleSet.Name = "buttonUploadLocalRuleSet";
            this.buttonUploadLocalRuleSet.Size = new System.Drawing.Size(75, 75);
            this.buttonUploadLocalRuleSet.TabIndex = 0;
            this.buttonUploadLocalRuleSet.Text = "Upload Local RuleSet File";
            this.buttonUploadLocalRuleSet.UseVisualStyleBackColor = true;
            this.buttonUploadLocalRuleSet.Click += new System.EventHandler(this.buttonUploadLocalRuleSet_Click);
            // 
            // buttonSignout
            // 
            this.buttonSignout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSignout.Location = new System.Drawing.Point(810, 12);
            this.buttonSignout.Name = "buttonSignout";
            this.buttonSignout.Size = new System.Drawing.Size(75, 31);
            this.buttonSignout.TabIndex = 8;
            this.buttonSignout.Text = "Sign Out";
            this.buttonSignout.UseVisualStyleBackColor = true;
            this.buttonSignout.Click += new System.EventHandler(this.buttonSignout_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefresh.Location = new System.Drawing.Point(729, 12);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 31);
            this.buttonRefresh.TabIndex = 9;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonMethodCheck
            // 
            this.buttonMethodCheck.Location = new System.Drawing.Point(249, 19);
            this.buttonMethodCheck.Name = "buttonMethodCheck";
            this.buttonMethodCheck.Size = new System.Drawing.Size(75, 75);
            this.buttonMethodCheck.TabIndex = 4;
            this.buttonMethodCheck.Text = "View Types / VO / Prop / Relaltion Methods";
            this.buttonMethodCheck.UseVisualStyleBackColor = true;
            this.buttonMethodCheck.Click += new System.EventHandler(this.buttonMethodCheck_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 540);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.buttonSignout);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Enabled = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rule Admin App";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonUpdateUserData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPublicName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox richTextBoxRuleDisplayText;
        private System.Windows.Forms.RichTextBox richTextBoxRuleDisplayJSON;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button buttonDeleteRuleItem;
        private System.Windows.Forms.Button buttonDownloadRuleSetLocally;
        private System.Windows.Forms.Button buttonUploadLocalRuleSet;
        private System.Windows.Forms.CheckedListBox checkedListBoxRuleSets;
        private System.Windows.Forms.CheckedListBox checkedListBoxRules;
        private System.Windows.Forms.Button buttonAddRules;
        private System.Windows.Forms.Button buttonRemoveRule;
        private System.Windows.Forms.Button buttonSignout;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Button buttonMethodCheck;
    }
}

