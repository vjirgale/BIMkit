namespace RuleAdminApp
{
    partial class LoginForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPublicName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.radioButtonNewUser = new System.Windows.Forms.RadioButton();
            this.radioButtonExisting = new System.Windows.Forms.RadioButton();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Public Name:";
            // 
            // textBoxPublicName
            // 
            this.textBoxPublicName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPublicName.Location = new System.Drawing.Point(88, 38);
            this.textBoxPublicName.Name = "textBoxPublicName";
            this.textBoxPublicName.Size = new System.Drawing.Size(226, 20);
            this.textBoxPublicName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Username:";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUsername.Location = new System.Drawing.Point(88, 12);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(226, 20);
            this.textBoxUsername.TabIndex = 0;
            // 
            // radioButtonNewUser
            // 
            this.radioButtonNewUser.AutoSize = true;
            this.radioButtonNewUser.Location = new System.Drawing.Point(12, 67);
            this.radioButtonNewUser.Name = "radioButtonNewUser";
            this.radioButtonNewUser.Size = new System.Drawing.Size(72, 17);
            this.radioButtonNewUser.TabIndex = 4;
            this.radioButtonNewUser.TabStop = true;
            this.radioButtonNewUser.Text = "New User";
            this.radioButtonNewUser.UseVisualStyleBackColor = true;
            this.radioButtonNewUser.CheckedChanged += new System.EventHandler(this.radioButtonLoginType_CheckedChanged);
            // 
            // radioButtonExisting
            // 
            this.radioButtonExisting.AutoSize = true;
            this.radioButtonExisting.Location = new System.Drawing.Point(88, 67);
            this.radioButtonExisting.Name = "radioButtonExisting";
            this.radioButtonExisting.Size = new System.Drawing.Size(86, 17);
            this.radioButtonExisting.TabIndex = 5;
            this.radioButtonExisting.TabStop = true;
            this.radioButtonExisting.Text = "Existing User";
            this.radioButtonExisting.UseVisualStyleBackColor = true;
            // 
            // buttonLogin
            // 
            this.buttonLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLogin.Location = new System.Drawing.Point(180, 64);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(134, 23);
            this.buttonLogin.TabIndex = 6;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // LoginForm
            // 
            this.AcceptButton = this.buttonLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 95);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.radioButtonExisting);
            this.Controls.Add(this.radioButtonNewUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.textBoxPublicName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RMS Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPublicName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.RadioButton radioButtonNewUser;
        private System.Windows.Forms.RadioButton radioButtonExisting;
        private System.Windows.Forms.Button buttonLogin;
    }
}