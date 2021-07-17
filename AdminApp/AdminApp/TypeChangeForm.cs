using DbmsApi;
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
    public partial class TypeChangeForm : Form
    {
        public ObjectTypes Type;

        public TypeChangeForm()
        {
            InitializeComponent();

            this.comboBoxTypes.Items.Clear();
            this.comboBoxTypes.Items.AddRange(ObjectTypeTree.ObjectDict.Keys.Select(t => t.ToString()).ToArray());
            this.comboBoxTypes.SelectedIndex = 0;
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            this.Type = (ObjectTypes)Enum.Parse(typeof(ObjectTypes), this.comboBoxTypes.SelectedItem.ToString());
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
