using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BIMRuleEditor
{
    public partial class RulesetSelector : Form
    {
        public RuleSet SelectedRuleset { get; internal set; }
        public List<RuleSet> RuleSets { get; internal set; }

        public RulesetSelector(List<RuleSet> ruleSets)
        {
            InitializeComponent();

            RuleSets = ruleSets;
            foreach (RuleSet r in ruleSets)
            {
                this.comboBoxRulesetSelector.Items.Add(r.Name);
            }
            this.comboBoxRulesetSelector.SelectedIndex = 0;
        }

        private void ButtonDone_Click(object sender, EventArgs e)
        {
            SelectedRuleset = RuleSets[this.comboBoxRulesetSelector.SelectedIndex];
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
