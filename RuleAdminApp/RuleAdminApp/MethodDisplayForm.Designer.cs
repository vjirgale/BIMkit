
namespace RuleAdminApp
{
    partial class MethodDisplayForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.richTextBoxTypes = new System.Windows.Forms.RichTextBox();
            this.richTextBoxProperties = new System.Windows.Forms.RichTextBox();
            this.richTextBoxRelation = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.richTextBoxRelation, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.richTextBoxProperties, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.richTextBoxTypes, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(670, 444);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // richTextBoxTypes
            // 
            this.richTextBoxTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxTypes.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxTypes.Name = "richTextBoxTypes";
            this.richTextBoxTypes.Size = new System.Drawing.Size(217, 438);
            this.richTextBoxTypes.TabIndex = 0;
            this.richTextBoxTypes.Text = "";
            // 
            // richTextBoxProperties
            // 
            this.richTextBoxProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxProperties.Location = new System.Drawing.Point(226, 3);
            this.richTextBoxProperties.Name = "richTextBoxProperties";
            this.richTextBoxProperties.Size = new System.Drawing.Size(217, 438);
            this.richTextBoxProperties.TabIndex = 1;
            this.richTextBoxProperties.Text = "";
            // 
            // richTextBoxRelation
            // 
            this.richTextBoxRelation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxRelation.Location = new System.Drawing.Point(449, 3);
            this.richTextBoxRelation.Name = "richTextBoxRelation";
            this.richTextBoxRelation.Size = new System.Drawing.Size(218, 438);
            this.richTextBoxRelation.TabIndex = 2;
            this.richTextBoxRelation.Text = "";
            // 
            // MethodDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 468);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MethodDisplayForm";
            this.Text = "MethodDisplayForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox richTextBoxRelation;
        private System.Windows.Forms.RichTextBox richTextBoxProperties;
        private System.Windows.Forms.RichTextBox richTextBoxTypes;
    }
}