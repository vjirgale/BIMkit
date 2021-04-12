using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BIMRuleEditor
{
    public partial class MainForm : Form
    {
        private List<RuleSet> ruleSets = new List<RuleSet>();
        private RuleSet currentRuleset;
        private Rule currentRule;
        private string userAppData = Application.UserAppDataPath + "\\UserRuleSettings.json";

        public MainForm()
        {
            InitializeComponent();

            foreach (ErrorLevel errorLevel in Enum.GetValues(typeof(ErrorLevel)))
            {
                this.comboBoxErrorLevel.Items.Add(errorLevel);
            }

            foreach (string fileName in Directory.GetFiles(Application.UserAppDataPath))
            {
                try
                {
                    ruleSets.Add(RuleReadWrite.ReadRuleSet(fileName));
                }
                catch
                {
                    MessageBox.Show("Existing rules are outdated");
                }
            }
            RedisplayRulesets();
        }

        #region Display Updates

        private void RedisplayRulesets()
        {
            this.treeViewRulesets.Nodes.Clear();
            foreach (RuleSet rs in ruleSets)
            {
                TreeNode ruleSetTn = new TreeNode(rs.Name);
                ruleSetTn.Tag = rs;
                this.treeViewRulesets.Nodes.Add(ruleSetTn);

                foreach (Rule r in rs.Rules)
                {
                    TreeNode ruleTn = new TreeNode(r.Name);
                    ruleTn.Tag = r;
                    ruleSetTn.Nodes.Add(ruleTn);
                }
            }
            this.treeViewRulesets.ExpandAll();
        }

        private void DisplayRule()
        {
            this.textBoxTitle.Text = currentRule.Name;
            this.comboBoxErrorLevel.SelectedIndex = this.comboBoxErrorLevel.Items.IndexOf(currentRule.ErrorLevel);
            this.richTextBoxDescription.Text = currentRule.Description;

            this.comboBoxErrorLevel.Enabled = true;
            this.groupBoxObjSearch.Enabled = true;
            this.groupBoxLogicalExpressions.Enabled = true;

            RedisplayObjectSearches();
            RedisplayLogicalExpressions();
            this.richTextBoxRuleString.Text = currentRule.String();
        }

        private void RedisplayObjectSearches()
        {
            this.treeViewObjSearch.Nodes.Clear();
            foreach (var kvp in currentRule.ExistentialClauses)
            {
                ExistentialClause ec = kvp.Value;
                TreeNode node = new TreeNode(ec.String(kvp.Key));
                node.Tag = kvp.Key;
                this.treeViewObjSearch.Nodes.Add(node);
            }
        }

        private void RedisplayLogicalExpressions()
        {
            this.treeViewLogicalExpressions.Nodes.Clear();
            TreeNode opNode = new TreeNode(currentRule.LogicalExpression.LogicalOperator.ToString());
            opNode.Tag = currentRule.LogicalExpression;
            this.treeViewLogicalExpressions.Nodes.Add(opNode);
            displayLogicalExpressions(opNode, currentRule.LogicalExpression);
        }

        private void displayLogicalExpressions(TreeNode parantTreeNode, LogicalExpression parentLE)
        {
            foreach (ObjectCheck oc in parentLE.ObjectChecks)
            {
                TreeNode node = new TreeNode(oc.String());
                node.Tag = oc;
                parantTreeNode.Nodes.Add(node);
            }
            foreach (RelationCheck rc in parentLE.RelationChecks)
            {
                TreeNode node = new TreeNode(rc.String());
                node.Tag = rc;
                parantTreeNode.Nodes.Add(node);
            }
            foreach (LogicalExpression le in parentLE.LogicalExpressions)
            {
                TreeNode node = new TreeNode(le.LogicalOperator.ToString());
                node.Tag = le;
                parantTreeNode.Nodes.Add(node);
                displayLogicalExpressions(node, le);
            }
            parantTreeNode.Expand();
        }

        private void DisplayRuleset()
        {
            this.textBoxTitle.Text = currentRuleset.Name;
            this.richTextBoxDescription.Text = currentRuleset.Description;

            this.richTextBoxRuleString.Text = "";
            this.comboBoxErrorLevel.SelectedIndex = -1;
            this.treeViewObjSearch.Nodes.Clear();
            this.treeViewLogicalExpressions.Nodes.Clear();

            this.comboBoxErrorLevel.Enabled = false;
            this.groupBoxObjSearch.Enabled = false;
            this.groupBoxLogicalExpressions.Enabled = false;
        }

        #endregion

        #region Rule and Ruleset Selection

        private void treeViewRulesets_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            currentRuleset = null;
            currentRule = null;

            if (e.Node.Tag == null)
            {
                return;
            }

            if (e.Node.Tag.GetType() == typeof(RuleSet))
            {
                currentRuleset = e.Node.Tag as RuleSet;
                DisplayRuleset();
            }
            if (e.Node.Tag.GetType() == typeof(Rule))
            {
                currentRule = e.Node.Tag as Rule;
                DisplayRule();
            }
        }

        private void treeViewRulesets_KeyUp(object sender, KeyEventArgs e)
        {
            TreeNode node = this.treeViewRulesets.SelectedNode;
            if ((e.KeyCode == Keys.Down) || (e.KeyCode == Keys.Up))
            {
                treeViewRulesets_NodeMouseClick(null, new TreeNodeMouseClickEventArgs(node, MouseButtons.Left, 0, 0, 0));
            }
        }

        #endregion

        #region Rule and Ruleset Controlls

        private void ButtonMoveRule_Click(object sender, EventArgs e)
        {
            RulesetSelector rulesetSelector = new RulesetSelector(ruleSets);
            if (rulesetSelector.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            RuleSet selectedRuleset = rulesetSelector.SelectedRuleset;
            TreeNode selectedRSNode = null;
            foreach (TreeNode tn in this.treeViewRulesets.Nodes)
            {
                if ((tn.Tag as RuleSet) == selectedRuleset)
                {
                    selectedRSNode = tn;
                    break;
                }
            }

            for (int i = 0; i < this.treeViewRulesets.Nodes.Count; i++)
            {
                TreeNode node = this.treeViewRulesets.Nodes[i];
                RuleSet rs = node.Tag as RuleSet;
                for (int j = 0; j < node.Nodes.Count; j++)
                {
                    TreeNode child = node.Nodes[j];
                    if (child.Checked && (rs != selectedRuleset))
                    {
                        rs.Rules.Remove(child.Tag as Rule);
                        node.Nodes.Remove(child);
                        selectedRuleset.Rules.Add(child.Tag as Rule);
                        selectedRSNode.Nodes.Add(child);
                        j -= 1;
                    }
                }
            }
        }

        private void buttonAddRuleSet_Click(object sender, EventArgs e)
        {
            RuleSet rs = new RuleSet("New Ruleset", "", new List<Rule>());
            ruleSets.Add(rs);

            TreeNode newTreeNode = new TreeNode(rs.Name);
            newTreeNode.Tag = rs;
            this.treeViewRulesets.Nodes.Add(newTreeNode);
        }

        private void buttonAddRule_Click(object sender, EventArgs e)
        {
            if (currentRuleset == null)
            {
                MessageBox.Show("Select the Ruleset that you would like to add the rule to first.");
                return;
            }

            Rule newRule = new Rule(
                                        "New Rule",
                                        "",
                                        ErrorLevel.Error,
                                        new Dictionary<string, ExistentialClause>(),
                                        new LogicalExpression(
                                                                new List<ObjectCheck>(),
                                                                new List<RelationCheck>(),
                                                                new List<LogicalExpression>(),
                                                                LogicalOperator.AND
                                                              )
                                    );
            currentRuleset.Rules.Add(newRule);
            TreeNode newTreeNode = new TreeNode(newRule.Name);
            newTreeNode.Tag = newRule;
            foreach (TreeNode tn in this.treeViewRulesets.Nodes)
            {
                if (tn.Tag == currentRuleset)
                {
                    tn.Nodes.Add(newTreeNode);
                    tn.Expand();
                    break;
                }
            }
        }

        private void buttonDeleteRule_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.treeViewRulesets.Nodes.Count; i++)
            {
                TreeNode node = this.treeViewRulesets.Nodes[i];
                RuleSet rs = node.Tag as RuleSet;
                if (node.Checked)
                {
                    ruleSets.Remove(node.Tag as RuleSet);
                    this.treeViewRulesets.Nodes.Remove(node);
                    i -= 1;
                    continue;
                }

                for (int j = 0; j < node.Nodes.Count; j++)
                {
                    TreeNode child = node.Nodes[j];
                    if (child.Checked)
                    {
                        rs.Rules.Remove(child.Tag as Rule);
                        node.Nodes.Remove(child);
                        j -= 1;
                    }
                }
            }
        }

        private void buttonImportRuleset_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "RuleSet files (*.json)|*.json|All files (*.*)|*.*";
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            List<RuleSet> ruleSets = new List<RuleSet>();
            try
            {
                ruleSets.Add(RuleReadWrite.ReadRuleSet(ofd.FileName));

                this.ruleSets.AddRange(ruleSets);
                RedisplayRulesets();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonExportRulesets_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "RuleSet files (*.json)|*.json|All files (*.*)|*.*";
            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            List<RuleSet> checkedRuleSets = new List<RuleSet>();
            foreach (TreeNode tn in this.treeViewRulesets.Nodes)
            {
                RuleSet nodeRuleset = tn.Tag as RuleSet;
                if (tn.Checked)
                {
                    checkedRuleSets.Add(tn.Tag as RuleSet);
                }
                else
                {
                    List<Rule> checkedRules = new List<Rule>();
                    foreach (TreeNode childTn in tn.Nodes)
                    {
                        if (childTn.Checked)
                        {
                            checkedRules.Add(childTn.Tag as Rule);
                        }
                    }
                    if (checkedRules.Count > 0)
                    {
                        RuleSet ruleSet = new RuleSet(nodeRuleset.Name, nodeRuleset.Description, checkedRules);
                        checkedRuleSets.Add(ruleSet);
                    }
                }
            }

            string path = Path.GetDirectoryName(sfd.FileName);
            string name = Path.GetFileNameWithoutExtension(sfd.FileName);
            string extn = Path.GetExtension(sfd.FileName);
            int index = 0;
            foreach (RuleSet rs in checkedRuleSets)
            {
                RuleReadWrite.WriteRuleSet(rs, path + "\\" + name + "(" + index + ")" + extn);
            }
        }

        #endregion

        #region Rule editing

        private void comboBoxErrorLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentRule == null)
            {
                return;
            }

            currentRule.ErrorLevel = Enum.GetValues(typeof(ErrorLevel)).Cast<ErrorLevel>().ToList()[this.comboBoxErrorLevel.SelectedIndex];
        }

        private void textBoxTitle_TextChanged(object sender, EventArgs e)
        {
            TreeNode treeNodeOfInterest = null;
            foreach (TreeNode tn in this.treeViewRulesets.Nodes)
            {
                if (tn.Tag == currentRuleset && currentRuleset != null)
                {
                    treeNodeOfInterest = tn;
                    break;
                }
                foreach (TreeNode tnChild in tn.Nodes)
                {
                    if (tnChild.Tag == currentRule && currentRule != null)
                    {
                        treeNodeOfInterest = tnChild;
                        break;
                    }
                }
            }
            if (treeNodeOfInterest == null)
            {
                return;
            }

            if (this.currentRule != null)
            {
                this.currentRule.Name = this.textBoxTitle.Text;
            }
            if (this.currentRuleset != null)
            {
                this.currentRuleset.Name = this.textBoxTitle.Text;
            }
            treeNodeOfInterest.Text = this.textBoxTitle.Text;
        }

        private void richTextBoxDescription_TextChanged(object sender, EventArgs e)
        {
            if (this.currentRule != null)
            {
                this.currentRule.Description = this.richTextBoxDescription.Text;
            }
            if (this.currentRuleset != null)
            {
                this.currentRuleset.Description = this.richTextBoxDescription.Text;
            }
        }

        #endregion

        #region Object Filter Controls

        private void buttonAddObjSearch_Click(object sender, EventArgs e)
        {
            int newIndex = currentRule.ExistentialClauses.Count;
            string newEcName = "Object" + newIndex;
            ExistencialClauseForm osf = new ExistencialClauseForm(newEcName, currentRule.ExistentialClauses.Keys.Where(k => k != newEcName).ToList());
            if (osf.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            ExistentialClause ec = osf.ExistentialClause;
            currentRule.ExistentialClauses.Add(osf.ClauseName, ec);

            TreeNode node = new TreeNode(ec.String(osf.ClauseName));
            node.Tag = osf.ClauseName;
            this.treeViewObjSearch.Nodes.Add(node);

            DisplayRule();
        }

        private void buttonEditObjSearch_Click(object sender, EventArgs e)
        {
            if (this.treeViewObjSearch.SelectedNode == null)
            {
                MessageBox.Show("No Object Search selected.");
                return;
            }

            string previousEcName = this.treeViewObjSearch.SelectedNode.Tag as string;
            ExistencialClauseForm osf = new ExistencialClauseForm(previousEcName, currentRule.ExistentialClauses[previousEcName].Copy(), currentRule.ExistentialClauses.Keys.Where(k => k != previousEcName).ToList());
            if (osf.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            this.currentRule.ExistentialClauses.Remove(previousEcName);
            this.currentRule.ExistentialClauses[osf.ClauseName] = osf.ExistentialClause;
            this.treeViewObjSearch.SelectedNode.Tag = osf.ExistentialClause;
            this.treeViewObjSearch.SelectedNode.Text = osf.ExistentialClause.String(osf.ClauseName);

            RecursivelyUpdateIndex(this.currentRule.LogicalExpression, previousEcName, osf.ClauseName);

            DisplayRule();
        }

        private void RecursivelyUpdateIndex(LogicalExpression parentLE, string oldName, string newName)
        {
            foreach (ObjectCheck oc in parentLE.ObjectChecks)
            {
                oc.ObjName = oc.ObjName == oldName ? newName : oc.ObjName;
            }
            foreach (RelationCheck rc in parentLE.RelationChecks)
            {
                rc.Obj1Name = rc.Obj1Name == oldName ? newName : rc.Obj1Name;
                rc.Obj2Name = rc.Obj2Name == oldName ? newName : rc.Obj2Name;
            }
            foreach (LogicalExpression le in parentLE.LogicalExpressions)
            {
                RecursivelyUpdateIndex(le, oldName, newName);
            }
        }

        private void buttonDeleteObjSearch_Click(object sender, EventArgs e)
        {
            if (this.treeViewObjSearch.SelectedNode == null)
            {
                MessageBox.Show("No Object Search selected.");
                return;
            }

            string ecName = this.treeViewObjSearch.SelectedNode.Tag as string;

            int indexCount = 0;
            RecursivelyCheckIndex(this.currentRule.LogicalExpression, ecName, ref indexCount);
            if (indexCount > 0)
            {
                MessageBox.Show("Object index is being referenced, and therefore cannot be deleted.");
                return;
            }

            this.currentRule.ExistentialClauses.Remove(ecName);
            this.treeViewObjSearch.Nodes.Remove(this.treeViewObjSearch.SelectedNode);

            DisplayRule();
        }

        private void RecursivelyCheckIndex(LogicalExpression parentLE, string name, ref int counter)
        {
            foreach (ObjectCheck oc in parentLE.ObjectChecks)
            {
                counter = oc.ObjName == name ? counter + 1 : counter;
            }
            foreach (RelationCheck rc in parentLE.RelationChecks)
            {
                counter = rc.Obj1Name == name ? counter + 1 : counter;
                counter = rc.Obj2Name == name ? counter + 1 : counter;
            }
            foreach (LogicalExpression le in parentLE.LogicalExpressions)
            {
                RecursivelyCheckIndex(le, name, ref counter);
            }
        }

        #endregion

        #region Expression Controls

        private void buttonAddLogicalExpressions_Click(object sender, EventArgs e)
        {
            if (this.treeViewLogicalExpressions.SelectedNode == null)
            {
                MessageBox.Show("No LogicalExpression selected.");
                return;
            }
            if (this.currentRule.ExistentialClauses.Count == 0)
            {
                MessageBox.Show("Need at least one Existence clause for this.");
                return;
            }

            LogicalExpression parentLogicalExpression = null;
            TreeNode parentLogicalExpressionNode = null;
            var nodeTag = this.treeViewLogicalExpressions.SelectedNode.Tag;
            if (nodeTag.GetType() == typeof(ObjectCheck))
            {
                parentLogicalExpressionNode = this.treeViewLogicalExpressions.SelectedNode.Parent;
                parentLogicalExpression = parentLogicalExpressionNode.Tag as LogicalExpression;
            }
            if (nodeTag.GetType() == typeof(RelationCheck))
            {
                parentLogicalExpressionNode = this.treeViewLogicalExpressions.SelectedNode.Parent;
                parentLogicalExpression = parentLogicalExpressionNode.Tag as LogicalExpression;
            }
            if (nodeTag.GetType() == typeof(LogicalExpression))
            {
                parentLogicalExpressionNode = this.treeViewLogicalExpressions.SelectedNode;
                parentLogicalExpression = nodeTag as LogicalExpression;
            }

            LogicalExpressionForm lef = new LogicalExpressionForm(currentRule.ExistentialClauses.Count, currentRule.ExistentialClauses.Keys.ToList());
            if (lef.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            ObjectCheck oc = lef.ObjectCheck;
            if (oc != null)
            {
                parentLogicalExpression.ObjectChecks.Add(oc);
                TreeNode node = new TreeNode(oc.String());
                parentLogicalExpressionNode.Nodes.Add(node);
                node.Tag = oc;
            }
            RelationCheck rc = lef.RelationCheck;
            if (rc != null)
            {
                parentLogicalExpression.RelationChecks.Add(rc);
                TreeNode node = new TreeNode(rc.String());
                parentLogicalExpressionNode.Nodes.Add(node);
                node.Tag = rc;
            }
            LogicalExpression le = lef.LogicalExpression;
            if (le != null)
            {
                parentLogicalExpression.LogicalExpressions.Add(le);
                TreeNode node = new TreeNode(le.LogicalOperator.ToString());
                parentLogicalExpressionNode.Nodes.Add(node);
                node.Tag = le;
            }

            parentLogicalExpressionNode.Expand();

            DisplayRule();
        }

        private void buttonEditLogicalExpressions_Click(object sender, EventArgs e)
        {
            if (this.treeViewLogicalExpressions.SelectedNode == null)
            {
                MessageBox.Show("No item selected.");
                return;
            }

            LogicalExpression parentLogicalExpression = null;
            TreeNode parentLogicalExpressionNode = this.treeViewLogicalExpressions.SelectedNode.Parent;
            // Special case where your working with the main logical expression. You must keep this a logical expression.
            bool isMainLE = parentLogicalExpressionNode == null;
            if (!isMainLE)
            {
                parentLogicalExpression = parentLogicalExpressionNode.Tag as LogicalExpression;
            }
            var nodeTag = this.treeViewLogicalExpressions.SelectedNode.Tag;
            LogicalExpressionForm lef = null;
            if (nodeTag.GetType() == typeof(ObjectCheck))
            {
                lef = new LogicalExpressionForm(currentRule.ExistentialClauses.Count, (nodeTag as ObjectCheck).Copy(), currentRule.ExistentialClauses.Keys.ToList());
            }
            if (nodeTag.GetType() == typeof(RelationCheck))
            {
                lef = new LogicalExpressionForm(currentRule.ExistentialClauses.Count, (nodeTag as RelationCheck).Copy(), currentRule.ExistentialClauses.Keys.ToList());
            }
            if (nodeTag.GetType() == typeof(LogicalExpression))
            {
                lef = new LogicalExpressionForm(currentRule.ExistentialClauses.Count, (nodeTag as LogicalExpression).Copy(), isMainLE, currentRule.ExistentialClauses.Keys.ToList());
            }

            if (lef.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            if (!isMainLE)
            {
                if (nodeTag.GetType() == typeof(ObjectCheck))
                {
                    parentLogicalExpression.ObjectChecks.Remove(nodeTag as ObjectCheck);
                }
                if (nodeTag.GetType() == typeof(RelationCheck))
                {
                    parentLogicalExpression.RelationChecks.Remove(nodeTag as RelationCheck);
                }
                if (nodeTag.GetType() == typeof(LogicalExpression))
                {
                    parentLogicalExpression.LogicalExpressions.Remove(nodeTag as LogicalExpression);
                }
            }

            ObjectCheck oc = lef.ObjectCheck;
            if (oc != null)
            {
                parentLogicalExpression.ObjectChecks.Add(oc);
                this.treeViewLogicalExpressions.SelectedNode = new TreeNode(oc.String());
                this.treeViewLogicalExpressions.SelectedNode.Text = oc.String();
                this.treeViewLogicalExpressions.SelectedNode.Tag = oc;
                this.treeViewLogicalExpressions.SelectedNode.Nodes.Clear();

            }
            RelationCheck rc = lef.RelationCheck;
            if (rc != null)
            {
                parentLogicalExpression.RelationChecks.Add(rc);
                this.treeViewLogicalExpressions.SelectedNode = new TreeNode(rc.String());
                this.treeViewLogicalExpressions.SelectedNode.Text = rc.String();
                this.treeViewLogicalExpressions.SelectedNode.Tag = rc;
                this.treeViewLogicalExpressions.SelectedNode.Nodes.Clear();
            }
            LogicalExpression le = lef.LogicalExpression;
            if (le != null)
            {
                if (!isMainLE)
                {
                    parentLogicalExpression.LogicalExpressions.Add(le);
                }
                else
                {
                    this.currentRule.LogicalExpression = le;
                }

                this.treeViewLogicalExpressions.SelectedNode = new TreeNode(le.String());
                this.treeViewLogicalExpressions.SelectedNode.Text = le.LogicalOperator.ToString();
                this.treeViewLogicalExpressions.SelectedNode.Tag = le;
            }

            if (!isMainLE)
            {
                parentLogicalExpressionNode.Expand();
            }

            DisplayRule();
        }

        private void buttonDeleteLogicalExpressions_Click(object sender, EventArgs e)
        {
            if (this.treeViewLogicalExpressions.SelectedNode == null)
            {
                MessageBox.Show("No item selected.");
                return;
            }

            LogicalExpression parentLogicalExpression = null;
            TreeNode parentLogicalExpressionNode = null;
            var nodeTag = this.treeViewLogicalExpressions.SelectedNode.Tag;
            if (nodeTag.GetType() == typeof(ObjectCheck))
            {
                parentLogicalExpressionNode = this.treeViewLogicalExpressions.SelectedNode.Parent;
                parentLogicalExpression = parentLogicalExpressionNode.Tag as LogicalExpression;
            }
            if (nodeTag.GetType() == typeof(RelationCheck))
            {
                parentLogicalExpressionNode = this.treeViewLogicalExpressions.SelectedNode.Parent;
                parentLogicalExpression = parentLogicalExpressionNode.Tag as LogicalExpression;
            }
            if (nodeTag.GetType() == typeof(LogicalExpression))
            {
                parentLogicalExpressionNode = this.treeViewLogicalExpressions.SelectedNode.Parent;
                if (parentLogicalExpressionNode == null)
                {
                    MessageBox.Show("Cannot delete main Logical Expression.");
                    return;
                }
                else
                {
                    parentLogicalExpression = parentLogicalExpressionNode.Tag as LogicalExpression;
                }
            }

            if (nodeTag.GetType() == typeof(ObjectCheck))
            {
                parentLogicalExpression.ObjectChecks.Remove(nodeTag as ObjectCheck);
                parentLogicalExpressionNode.Nodes.Remove(this.treeViewLogicalExpressions.SelectedNode);
            }
            if (nodeTag.GetType() == typeof(RelationCheck))
            {
                parentLogicalExpression.RelationChecks.Remove(nodeTag as RelationCheck);
                parentLogicalExpressionNode.Nodes.Remove(this.treeViewLogicalExpressions.SelectedNode);
            }
            if (nodeTag.GetType() == typeof(LogicalExpression))
            {
                parentLogicalExpression.LogicalExpressions.Remove(nodeTag as LogicalExpression);
                parentLogicalExpressionNode.Nodes.Remove(this.treeViewLogicalExpressions.SelectedNode);
            }

            this.treeViewLogicalExpressions.SelectedNode.Nodes.Clear();
            parentLogicalExpressionNode.Expand();

            DisplayRule();
        }

        #endregion

        #region Save data

        private void ButtonSaveAndExit_Click(object sender, EventArgs e)
        {
            // Should save locally when closing
            string path = Path.GetDirectoryName(userAppData);
            string name = Path.GetFileNameWithoutExtension(userAppData);
            string extn = Path.GetExtension(userAppData);
            int index = 0;
            foreach (RuleSet rs in ruleSets)
            {
                RuleReadWrite.WriteRuleSet(rs, path + "\\" + name + "(" + index + ")" + extn);
                index++;
            }
            this.Close();
        }

        #endregion
    }
}