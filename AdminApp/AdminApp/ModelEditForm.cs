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
    public partial class ModelEditForm : Form
    {
        public ModelPermission ModelPermission;

        public ModelEditForm(ModelMetadata modelData, List<string> allUsers, ModelPermission modelPermissions)
        {
            InitializeComponent();

            this.textBoxName.Text = modelData.Name;
            this.textBoxID.Text = modelPermissions.ModelId;
            this.comboBoxOwner.Items.AddRange(allUsers.ToArray());
            this.comboBoxOwner.SelectedItem = modelPermissions.Owner;

            foreach (string user in allUsers)
            {
                this.checkedListBoxAccess.Items.Add(user, modelPermissions.UsersWithAccess.Contains(user));
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            ModelPermission = new ModelPermission()
            {
                ModelId = this.textBoxID.Text,
                Owner = this.comboBoxOwner.Text,
                UsersWithAccess = this.checkedListBoxAccess.CheckedItems.Cast<string>().ToList()
            };

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
