using DbmsApi;
using ModelConverter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModelContertApp
{
    public partial class IfcMappingForm : Form
    {
        public IfcMappingForm(string ifcType)
        {
            InitializeComponent();
            this.textBoxIfcType.Text = ifcType;
            this.comboBoxBIMPlatformType.DataSource = Enum.GetValues(typeof(ObjectTypes));
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            Enum.TryParse<ObjectTypes>(comboBoxBIMPlatformType.SelectedValue.ToString(), out ObjectTypes value);
            IfcConverter.AddTypeConvert(this.textBoxIfcType.Text, value);
            this.Close();
        }
    }
}
