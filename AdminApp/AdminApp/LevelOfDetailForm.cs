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
    public partial class LevelOfDetailForm : Form
    {
        public LevelOfDetail LOD;

        public LevelOfDetailForm()
        {
            InitializeComponent();

            this.comboBoxLOD.Items.AddRange(Enum.GetValues(typeof(LevelOfDetail)).Cast<LevelOfDetail>().Select(l => l.ToString()).ToArray());
            this.comboBoxLOD.SelectedIndex = 0;
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            LOD = (LevelOfDetail)Enum.Parse(typeof(LevelOfDetail), this.comboBoxLOD.SelectedItem.ToString());
            this.Close();
        }
    }
}
