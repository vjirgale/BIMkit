using DbmsApi.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminApp
{
    public partial class UpdateUserDataForm : Form
    {
        public AuthModel AuthModel;

        public UpdateUserDataForm()
        {
            InitializeComponent();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.textBoxPassword.Text) || string.IsNullOrWhiteSpace(this.textBoxUsername.Text))
            {
                MessageBox.Show("Username and Password must not be empty");
                return;
            }

            AuthModel.Username = this.textBoxUsername.Text;
            AuthModel.Password = this.textBoxPassword.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
