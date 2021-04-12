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

namespace RuleAdminApp
{
    public partial class MethodDisplayForm : Form
    {
        public MethodDisplayForm(List<ObjectTypes> types, Dictionary<ObjectTypes, string> VOs, Dictionary<string, Type> properties, Dictionary<string, Type> relations)
        {
            InitializeComponent();

            foreach (var kvp in types)
            {
                if (VOs.ContainsKey(kvp))
                {
                    continue;
                }
                this.richTextBoxTypes.Text += kvp + "\n";
            }
            foreach (var kvp in VOs)
            {
                this.richTextBoxTypes.Text += kvp.Key.ToString() + " (" + kvp.Value + ")\n";
            }
            foreach (var kvp in properties)
            {
                this.richTextBoxProperties.Text += kvp.Key + " (" + kvp.Value.ToString() + ")\n";
            }
            foreach (var kvp in relations)
            {
                this.richTextBoxRelation.Text += kvp.Key + " (" + kvp.Value.ToString() + ")\n";
            }
        }
    }
}