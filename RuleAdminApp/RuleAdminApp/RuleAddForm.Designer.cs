namespace RuleAdminApp
{
    partial class RuleAddForm
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
            this.buttonTakeRule = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkedListBoxUser = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkedListBoxExternal = new System.Windows.Forms.CheckedListBox();
            this.buttonDone = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonTakeRule
            // 
            this.buttonTakeRule.Location = new System.Drawing.Point(319, 176);
            this.buttonTakeRule.Name = "buttonTakeRule";
            this.buttonTakeRule.Size = new System.Drawing.Size(83, 40);
            this.buttonTakeRule.TabIndex = 0;
            this.buttonTakeRule.Text = "<=";
            this.buttonTakeRule.UseVisualStyleBackColor = true;
            this.buttonTakeRule.Click += new System.EventHandler(this.buttonTakeRule_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkedListBoxUser);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(301, 380);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User RuleSets";
            // 
            // checkedListBoxUser
            // 
            this.checkedListBoxUser.FormattingEnabled = true;
            this.checkedListBoxUser.Location = new System.Drawing.Point(6, 19);
            this.checkedListBoxUser.Name = "checkedListBoxUser";
            this.checkedListBoxUser.Size = new System.Drawing.Size(289, 349);
            this.checkedListBoxUser.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkedListBoxExternal);
            this.groupBox2.Location = new System.Drawing.Point(408, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(301, 380);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "External Rules";
            // 
            // checkedListBoxExternal
            // 
            this.checkedListBoxExternal.FormattingEnabled = true;
            this.checkedListBoxExternal.Location = new System.Drawing.Point(6, 19);
            this.checkedListBoxExternal.Name = "checkedListBoxExternal";
            this.checkedListBoxExternal.Size = new System.Drawing.Size(289, 349);
            this.checkedListBoxExternal.TabIndex = 0;
            // 
            // buttonDone
            // 
            this.buttonDone.Location = new System.Drawing.Point(319, 349);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(83, 40);
            this.buttonDone.TabIndex = 3;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // RuleAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 404);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonTakeRule);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RuleAddForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Rules";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonTakeRule;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox checkedListBoxUser;
        private System.Windows.Forms.CheckedListBox checkedListBoxExternal;
        private System.Windows.Forms.Button buttonDone;
    }
}