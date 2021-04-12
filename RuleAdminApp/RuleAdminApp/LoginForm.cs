using DbmsApi;
using RuleAPI;
using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RuleAdminApp
{
    public partial class LoginForm : Form
    {
        public RuleUser User;
        RuleAPIController RuleAPIController;

        public LoginForm(RuleAPIController controller)
        {
            InitializeComponent();

            RuleAPIController = controller;
            this.radioButtonExisting.Checked = true;
            this.textBoxPublicName.ReadOnly = this.radioButtonExisting.Checked;
            this.DialogResult = DialogResult.Cancel;
        }

        private void radioButtonLoginType_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxPublicName.ReadOnly = this.radioButtonExisting.Checked;
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            this.User = new RuleUser()
            {
                Username = this.textBoxUsername.Text,
                PublicName = this.textBoxPublicName.Text,
                RuleOwnership = new List<string>(),
                RuleSetOwnership = new List<string>()
            };

            APIResponse<RuleUser> response = null;
            if (this.radioButtonNewUser.Checked)
            {
                response = await RuleAPIController.CreateUserAsync(this.User);
            }
            if (this.radioButtonExisting.Checked)
            {
                response = await RuleAPIController.LoginAsync(this.User.Username);
            }

            if (response.Code == System.Net.HttpStatusCode.OK || response.Code == System.Net.HttpStatusCode.Created)
            {
                this.User = response.Data;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(response.ReasonPhrase);
            }
        }
    }
}