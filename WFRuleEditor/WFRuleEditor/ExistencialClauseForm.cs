using DbmsApi;
using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BIMRuleEditor
{
    public partial class ExistencialClauseForm : Form
    {
        public ExistentialClause ExistentialClause { get; set; }
        public string ClauseName { get; set; }
        public List<string> takenKeys = new List<string>();

        public ExistencialClauseForm(string name, List<string> keys)
        {
            InitializeComponent();
            this.ExistentialClause = new ExistentialClause(OccurrenceRule.ALL, new Characteristic(ObjectTypes.BuildingElement, new List<PropertyCheck>()));
            ClauseName = name;
            takenKeys = keys;

            SetDropdownVals();
            DisplayExistentialClause();
        }

        public ExistencialClauseForm(string name, ExistentialClause ec, List<string> keys)
        {
            InitializeComponent();
            this.ExistentialClause = ec;
            ClauseName = name;
            takenKeys = keys;

            SetDropdownVals();
            DisplayExistentialClause();
        }

        private void SetDropdownVals()
        {
            foreach (OccurrenceRule occurrence in Enum.GetValues(typeof(OccurrenceRule)))
            {
                this.comboBoxObjectExistence.Items.Add(occurrence);
            }
            foreach (ObjectTypes type in Enum.GetValues(typeof(ObjectTypes)))
            {
                this.comboBoxObjectType.Items.Add(type);
            }
        }

        private void DisplayExistentialClause()
        {
            this.textBoxObjectIndex.Text = ClauseName;
            this.comboBoxObjectExistence.SelectedIndex = this.comboBoxObjectExistence.Items.IndexOf(ExistentialClause.OccurrenceRule);
            this.comboBoxObjectType.SelectedIndex = this.comboBoxObjectType.Items.IndexOf(ExistentialClause.Characteristic.Type);

            this.treeViewProperties.Nodes.Clear();
            foreach (PropertyCheck pc in ExistentialClause.Characteristic.PropertyChecks)
            {
                TreeNode node = new TreeNode(pc.String());
                node.Tag = pc;
                this.treeViewProperties.Nodes.Add(node);
            }
        }

        private void comboBoxObjectExistence_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExistentialClause.OccurrenceRule = Enum.GetValues(typeof(OccurrenceRule)).Cast<OccurrenceRule>().ToList()[this.comboBoxObjectExistence.SelectedIndex];
        }

        private void comboBoxObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExistentialClause.Characteristic.Type = Enum.GetValues(typeof(ObjectTypes)).Cast<ObjectTypes>().ToList()[this.comboBoxObjectType.SelectedIndex];
        }

        private void buttonAddProperty_Click(object sender, EventArgs e)
        {
            PropertyCheckForm pcf = new PropertyCheckForm(true);
            if (pcf.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            PropertyCheck newPc = pcf.PropertyCheck;
            this.ExistentialClause.Characteristic.PropertyChecks.Add(newPc);
            TreeNode newTreeNode = new TreeNode(newPc.String());
            newTreeNode.Tag = newPc;
            this.treeViewProperties.Nodes.Add(newTreeNode);
        }

        private void buttonEditProperty_Click(object sender, EventArgs e)
        {
            if (this.treeViewProperties.SelectedNode == null)
            {
                MessageBox.Show("No Property Check selected.");
                return;
            }

            PropertyCheck pc = this.treeViewProperties.SelectedNode.Tag as PropertyCheck;
            int index = ExistentialClause.Characteristic.PropertyChecks.IndexOf(pc);
            PropertyCheckForm pcf = new PropertyCheckForm(true, pc.Copy());
            if (pcf.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            this.ExistentialClause.Characteristic.PropertyChecks[index] = pcf.PropertyCheck;
            this.treeViewProperties.SelectedNode.Tag = pcf.PropertyCheck;
            this.treeViewProperties.SelectedNode.Text = pcf.PropertyCheck.String();
        }

        private void buttonDeleteObjProperty_Click(object sender, EventArgs e)
        {
            if (this.treeViewProperties.SelectedNode == null)
            {
                MessageBox.Show("No Property Check selected.");
                return;
            }

            PropertyCheck pc = this.treeViewProperties.SelectedNode.Tag as PropertyCheck;
            ExistentialClause.Characteristic.PropertyChecks.Remove(pc);
            this.treeViewProperties.Nodes.Remove(this.treeViewProperties.SelectedNode);
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            if (takenKeys.Contains(this.ClauseName))
            {
                MessageBox.Show("Existencial Claus Name already exists. Please use another.");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textBoxObjectIndex_TextChanged(object sender, EventArgs e)
        {
            this.ClauseName = this.textBoxObjectIndex.Text;

            this.textBoxObjectIndex.BackColor = takenKeys.Contains(this.ClauseName) ? Color.Red : Color.White;
        }
    }
}