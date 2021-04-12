using DbmsApi;
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
    public partial class LoginForm : Form
    {
        public UserData User;
        private DBMSAPIController DBMSAPIController;

        public LoginForm(DBMSAPIController controller)
        {
            InitializeComponent();

            DBMSAPIController = controller;
            this.DialogResult = DialogResult.Cancel;
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            APIResponse<TokenData> response = await DBMSAPIController.LoginAsync(this.textBoxUsername.Text, this.textBoxPassword.Text);

            if (response.Code == System.Net.HttpStatusCode.OK)
            {
                APIResponse<UserData> response2 = await DBMSAPIController.GetUserData(this.textBoxUsername.Text);

                if (response2.Code == System.Net.HttpStatusCode.OK)
                {
                    this.User = response2.Data;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(response2.ReasonPhrase);
                }
            }
            else
            {
                MessageBox.Show(response.ReasonPhrase);
            }
        }

        private async void buttonSignUp_Click(object sender, EventArgs e)
        {
            NewUser newUser = new NewUser(this.textBoxUsername.Text, "", this.textBoxPassword.Text);
            APIResponse<UserData> response = await DBMSAPIController.CreateUser(newUser);

            if (response.Code == System.Net.HttpStatusCode.OK)
            {
                APIResponse<TokenData> response1 = await DBMSAPIController.LoginAsync(newUser.Username, newUser.Password);
                if (response1.Code == System.Net.HttpStatusCode.OK)
                {
                    APIResponse<UserData> response2 = await DBMSAPIController.GetUserData(this.textBoxUsername.Text);

                    if (response2.Code == System.Net.HttpStatusCode.OK)
                    {
                        this.User = response2.Data;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(response2.ReasonPhrase);
                    }
                }
                else
                {
                    MessageBox.Show(response1.ReasonPhrase);
                }
            }
            else
            {
                MessageBox.Show(response.ReasonPhrase);
            }
        }
    }
}