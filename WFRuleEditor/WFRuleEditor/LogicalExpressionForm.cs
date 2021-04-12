using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace BIMRuleEditor
{
    public partial class LogicalExpressionForm : Form
    {
        public ObjectCheck ObjectCheck { get; private set; }
        public RelationCheck RelationCheck { get; private set; }
        public LogicalExpression LogicalExpression { get; private set; }
        private int ExistenceClauseCount;

        public LogicalExpressionForm(int existenceClauseCount, List<string> keys)
        {
            InitializeComponent();
            ExistenceClauseCount = existenceClauseCount;
            SetDropdownVals(keys);

            ObjectCheck = new ObjectCheck(keys.First(), Negation.MUST_HAVE, new PropertyCheckBool("MISSING", OperatorBool.EQUAL, true));
            RelationCheck = new RelationCheck(keys.First(), keys.First(), Negation.MUST_HAVE, new PropertyCheckBool("MISSING", OperatorBool.EQUAL, true));
            LogicalExpression = new LogicalExpression(new List<ObjectCheck>(), new List<RelationCheck>(), new List<LogicalExpression>(), LogicalOperator.AND);
            this.radioButtonNewLE.Checked = true;
            SetCheckAndLEValues();
        }
        public LogicalExpressionForm(int existenceClauseCount, ObjectCheck objectCheck, List<string> keys)
        {
            InitializeComponent();
            ExistenceClauseCount = existenceClauseCount;
            SetDropdownVals(keys);
            ObjectCheck = objectCheck;
            RelationCheck = new RelationCheck(keys.First(), keys.First(), Negation.MUST_HAVE, new PropertyCheckBool("MISSING", OperatorBool.EQUAL, true));
            LogicalExpression = new LogicalExpression(new List<ObjectCheck>(), new List<RelationCheck>(), new List<LogicalExpression>(), LogicalOperator.AND);
            this.radioButtonNewOC.Checked = true;
            SetCheckAndLEValues();
        }
        public LogicalExpressionForm(int existenceClauseCount, RelationCheck relationCheck, List<string> keys)
        {
            InitializeComponent();
            ExistenceClauseCount = existenceClauseCount;
            SetDropdownVals(keys);
            ObjectCheck = new ObjectCheck(keys.First(), Negation.MUST_HAVE, new PropertyCheckBool("MISSING", OperatorBool.EQUAL, true));
            RelationCheck = relationCheck;
            LogicalExpression = new LogicalExpression(new List<ObjectCheck>(), new List<RelationCheck>(), new List<LogicalExpression>(), LogicalOperator.AND);
            this.radioButtonNewRC.Checked = true;
            SetCheckAndLEValues();
        }
        public LogicalExpressionForm(int existenceClauseCount, LogicalExpression logicalExpression, bool mainLE, List<string> keys)
        {
            InitializeComponent();
            ExistenceClauseCount = existenceClauseCount;
            SetDropdownVals(keys);
            ObjectCheck = new ObjectCheck(keys.First(), Negation.MUST_HAVE, new PropertyCheckBool("MISSING", OperatorBool.EQUAL, true));
            RelationCheck = new RelationCheck(keys.First(), keys.First(), Negation.MUST_HAVE, new PropertyCheckBool("MISSING", OperatorBool.EQUAL, true));
            LogicalExpression = logicalExpression;
            this.radioButtonNewLE.Checked = true;

            if (mainLE)
            {
                radioButtonNewLE.Enabled = false;
                radioButtonNewOC.Enabled = false;
                radioButtonNewRC.Enabled = false;
            }
            else
            {
                SetCheckAndLEValues();
            }
        }

        private void SetDropdownVals(List<string> keys)
        {
            foreach(string key in keys)
            {
                string objInd = key;
                this.comboBoxObjIndexOC.Items.Add(objInd);
                this.comboBoxObjIndex1RC.Items.Add(objInd);
                this.comboBoxObjIndex2RC.Items.Add(objInd);
            }
            foreach (Negation Negation in Enum.GetValues(typeof(Negation)))
            {
                this.comboBoxNegOC.Items.Add(Negation);
                this.comboBoxNegRC.Items.Add(Negation);
            }
        }

        private void SetCheckAndLEValues()
        {
            this.radioButtonAND.Checked = this.LogicalExpression.LogicalOperator == LogicalOperator.AND;
            this.radioButtonOR.Checked = this.LogicalExpression.LogicalOperator == LogicalOperator.OR;
            this.radioButtonXOR.Checked = this.LogicalExpression.LogicalOperator == LogicalOperator.XOR;

            this.comboBoxObjIndexOC.SelectedItem = this.ObjectCheck.ObjName;
            this.comboBoxNegOC.SelectedIndex = this.comboBoxNegOC.Items.IndexOf(this.ObjectCheck.Negation);
            TreeNode nodeOC = new TreeNode(this.ObjectCheck.PropertyCheck.String());
            nodeOC.Tag = this.ObjectCheck.PropertyCheck;
            this.treeViewPropertiesOC.Nodes.Add(nodeOC);

            this.comboBoxObjIndex1RC.SelectedItem = this.RelationCheck.Obj1Name;
            this.comboBoxObjIndex2RC.SelectedItem = this.RelationCheck.Obj2Name;
            this.comboBoxNegRC.SelectedIndex = this.comboBoxNegRC.Items.IndexOf(this.RelationCheck.Negation);
            TreeNode nodeRC = new TreeNode(this.RelationCheck.PropertyCheck.String());
            nodeRC.Tag = this.RelationCheck.PropertyCheck;
            this.treeViewPropertiesRC.Nodes.Add(nodeRC);
        }

        private void RadioButtonNewLE_OC_RC_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonNewOC.Checked)
            {
                this.groupBoxNewOC.Enabled = true;
                this.groupBoxNewRC.Enabled = false;
                this.groupBoxNewLE.Enabled = false;
            }
            if (this.radioButtonNewRC.Checked)
            {
                this.groupBoxNewOC.Enabled = false;
                this.groupBoxNewRC.Enabled = true;
                this.groupBoxNewLE.Enabled = false;
            }
            if (this.radioButtonNewLE.Checked)
            {
                this.groupBoxNewOC.Enabled = false;
                this.groupBoxNewRC.Enabled = false;
                this.groupBoxNewLE.Enabled = true;
            }
        }

        private void RadioButtonLogicOp_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonAND.Checked)
            {
                this.LogicalExpression.LogicalOperator = LogicalOperator.AND;
            }
            if (this.radioButtonOR.Checked)
            {
                this.LogicalExpression.LogicalOperator = LogicalOperator.OR;
            }
            if (this.radioButtonXOR.Checked)
            {
                this.LogicalExpression.LogicalOperator = LogicalOperator.XOR;
            }
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            if (this.radioButtonNewOC.Checked)
            {
                this.RelationCheck = null;
                this.LogicalExpression = null;
            }
            if (this.radioButtonNewRC.Checked)
            {
                this.ObjectCheck = null;
                this.LogicalExpression = null;
            }
            if (this.radioButtonNewLE.Checked)
            {
                this.ObjectCheck = null;
                this.RelationCheck = null;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void comboBoxObjIndexOC_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ObjectCheck.ObjName = this.comboBoxObjIndexOC.SelectedItem as string;
        }

        private void comboBoxNegOC_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ObjectCheck.Negation = Enum.GetValues(typeof(Negation)).Cast<Negation>().ToList()[this.comboBoxNegOC.SelectedIndex];
        }

        private void comboBoxObjIndex1RC_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RelationCheck.Obj1Name = this.comboBoxObjIndex1RC.SelectedItem as string;
        }

        private void comboBoxObjIndex2RC_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RelationCheck.Obj2Name = this.comboBoxObjIndex2RC.SelectedItem as string;
        }

        private void comboBoxNegRC_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RelationCheck.Negation = Enum.GetValues(typeof(Negation)).Cast<Negation>().ToList()[this.comboBoxNegRC.SelectedIndex];
        }

        private void buttonEditPropertyOC_Click(object sender, EventArgs e)
        {
            if (this.treeViewPropertiesOC.SelectedNode == null)
            {
                MessageBox.Show("No node selected.");
                return;
            }

            PropertyCheck pc = this.treeViewPropertiesOC.Nodes[0].Tag as PropertyCheck;
            PropertyCheckForm pcf = new PropertyCheckForm(true, pc.Copy());
            if (pcf.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            this.ObjectCheck.PropertyCheck = pcf.PropertyCheck;
            this.treeViewPropertiesOC.SelectedNode.Tag = pcf.PropertyCheck;
            this.treeViewPropertiesOC.SelectedNode.Text = pcf.PropertyCheck.String();
        }

        private void buttonEditPropertyRC_Click(object sender, EventArgs e)
        {
            if (this.treeViewPropertiesRC.SelectedNode == null)
            {
                MessageBox.Show("No node selected.");
                return;
            }

            PropertyCheck pc = this.treeViewPropertiesRC.Nodes[0].Tag as PropertyCheck;
            PropertyCheckForm pcf = new PropertyCheckForm(false, pc.Copy());
            if (pcf.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            this.RelationCheck.PropertyCheck = pcf.PropertyCheck;
            this.treeViewPropertiesRC.SelectedNode.Tag = pcf.PropertyCheck;
            this.treeViewPropertiesRC.SelectedNode.Text = pcf.PropertyCheck.String();
        }
    }
}