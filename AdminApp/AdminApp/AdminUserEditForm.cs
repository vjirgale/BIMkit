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
    public partial class AdminUserEditForm : Form
    {
        public UserData UserData;
        public string NewPassword;
        public bool NewUser;

        public AdminUserEditForm(UserData userData, bool newUser, List<ModelMetadata> allModels)
        {
            InitializeComponent();

            UserData = new UserData()
            {
                Username = userData.Username,
                PublicName = userData.PublicName,
                IsAdmin = userData.IsAdmin,
                AccessibleModels = userData.AccessibleModels,
                OwnedModels = userData.OwnedModels,
                Properties = userData.Properties
            };

            this.textBoxUsername.Text = UserData.Username;
            this.textBoxPublicName.Text = UserData.PublicName;
            this.checkBoxAdmin.Checked = UserData.IsAdmin;
            this.textBoxPassword.Enabled = false;
            foreach (ModelMetadata mm in allModels.OrderBy(m=>m.Name))
            {
                this.checkedListBoxOwn.Items.Add(mm, UserData.OwnedModels.Contains(mm.ModelId));
                this.checkedListBoxAccess.Items.Add(mm, UserData.AccessibleModels.Contains(mm.ModelId));
            }
            //this.checkedListBoxOwn.DisplayMember = "Name";
            //this.checkedListBoxAccess.DisplayMember = "Name";

            NewUser = newUser;
            if (NewUser)
            {
                checkBoxPassword.Checked = true;
                checkedListBoxAccess.Enabled = false;
                checkedListBoxOwn.Enabled = false;
            }
            this.textBoxPassword.Enabled = this.checkBoxPassword.Checked;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.textBoxUsername.Text))
            {
                MessageBox.Show("Username must not be empty");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.textBoxPassword.Text) && checkBoxPassword.Checked)
            {
                MessageBox.Show("Password must not be empty");
                return;
            }

            UserData.Username = this.textBoxUsername.Text;
            UserData.PublicName = this.textBoxPublicName.Text;
            UserData.IsAdmin = this.checkBoxAdmin.Checked;
            if (checkBoxPassword.Checked)
            {
                NewPassword = this.textBoxPassword.Text;
            }

            List<string> ownedModelIds = new List<string>();
            foreach (ModelMetadata item in this.checkedListBoxOwn.CheckedItems)
            {
                ownedModelIds.Add(item.ModelId);
            }
            List<string> accessModelIds = new List<string>();
            foreach (ModelMetadata item in this.checkedListBoxAccess.CheckedItems)
            {
                accessModelIds.Add(item.ModelId);
            }
            UserData.AccessibleModels = accessModelIds;
            UserData.OwnedModels = ownedModelIds;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void checkBoxPassword_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxPassword.Text = "";
            this.textBoxPassword.Enabled = this.checkBoxPassword.Checked;
        }
    }
}