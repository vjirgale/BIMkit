using DbmsApi;
using RuleAPI;
using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RuleAdminApp
{
    public partial class RuleAddForm : Form
    {
        public Dictionary<string, List<Rule>> AddedRules;
        private RuleAPIController RuleAPIController;

        public RuleAddForm(List<RuleSet> ruleSets, RuleAPIController ruleAPIController)
        {
            InitializeComponent();

            this.checkedListBoxExternal.Items.Clear();
            this.checkedListBoxUser.Items.Clear();

            RuleAPIController = ruleAPIController;
            AddedRules = new Dictionary<string, List<Rule>>();

            // put all user rulesets and the external rules in the check lists
            this.checkedListBoxUser.Items.Clear();
            foreach (RuleSet rs in ruleSets)
            {
                this.checkedListBoxUser.Items.Add(new RuleDisplayer(rs));
                //List<string> rules = rs.Rules.Select(r => r.Id).ToList();
                AddedRules.Add(rs.Id, new List<Rule>());
            }

            LoadRules();
        }

        private async void LoadRules()
        {
            this.checkedListBoxExternal.Items.Clear();
            APIResponse<List<Rule>> response = await RuleAPIController.GetAllRulesAsync();
            if (response.Code != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.ReasonPhrase);
                return;
            }
            List<Rule> rules = response.Data;
            foreach (Rule r in rules)
            {
                this.checkedListBoxExternal.Items.Add(new RuleDisplayer(r));
            }
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonTakeRule_Click(object sender, EventArgs e)
        {
            List<string> rsIds = this.checkedListBoxUser.CheckedItems.Cast<RuleDisplayer>().Select(i => i.Item.Id).ToList();
            List<Rule> rules = this.checkedListBoxExternal.CheckedItems.Cast<RuleDisplayer>().Select(i => i.Item as Rule).ToList();
            foreach (string id in rsIds)
            {
                AddedRules[id].AddRange(rules);
            }
        }
    }

    public class RuleDisplayer
    {
        public string Name = "";
        public BaseRuleItem Item;
        public RuleDisplayer(BaseRuleItem item)
        {
            this.Item = item;
            this.Name = item.Name + " (" + item.Owner + ")";
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
