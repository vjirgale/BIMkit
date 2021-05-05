using DbmsApi;
using RuleAPI;
using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RuleAdminApp
{
    public partial class MainForm : Form
    {
        private RuleUser ruleUser;
        private RuleAPIController RuleAPIController;
        private DBMSAPIController DBMSController;
        private string ruleServiceURL = "https://localhost:44370/api/";
        private string dbmsURL = "https://localhost:44322//api/";

        public MainForm()
        {
            InitializeComponent();

            RuleAPIController = new RuleAPIController(ruleServiceURL);
            DBMSController = new DBMSAPIController(dbmsURL);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Enabled = false;
            LoginForm login = new LoginForm(RuleAPIController);
            login.ShowDialog();
            if (login.DialogResult == DialogResult.Cancel)
            {
                this.Close();
            }
            if (login.DialogResult == DialogResult.OK)
            {
                this.Enabled = true;
                this.ruleUser = login.User;

                this.textBoxUsername.Text = this.ruleUser.Username;
                this.textBoxPublicName.Text = this.ruleUser.PublicName;

                PopulateRuleSetList();
            }
        }

        public async void Relogin()
        {
            APIResponse<RuleUser> response = await RuleAPIController.LoginAsync(ruleUser.Username);

            if (response.Code == System.Net.HttpStatusCode.OK)
            {
                ruleUser = response.Data;
            }
            else
            {
                MessageBox.Show(response.ReasonPhrase);
            }

            PopulateRuleSetList();
        }

        private async void PopulateRuleSetList()
        {
            checkedListBoxRules.Items.Clear();
            this.ResetRuleDisplay();

            checkedListBoxRuleSets.Items.Clear();
            checkedListBoxRuleSets.ValueMember = "Id";
            checkedListBoxRuleSets.DisplayMember = "Name";

            checkedListBoxRuleSets.Items.Add("All Rules");

            List<RuleSet> ruleSets = new List<RuleSet>();
            foreach (string ruleSetId in ruleUser.RuleSetOwnership)
            {
                APIResponse<RuleSet> response = await RuleAPIController.GetRuleSetAsync(ruleSetId);
                if (response.Code == System.Net.HttpStatusCode.OK)
                {
                    RuleSet ruleSet = response.Data;
                    ruleSets.Add(ruleSet);
                }
                else
                {
                    MessageBox.Show(response.ReasonPhrase);
                }
            }

            ruleSets = ruleSets.OrderBy(r => r.Name).ToList();
            foreach (RuleSet rs in ruleSets)
            {
                checkedListBoxRuleSets.Items.Add(rs);
            }
        }

        private async void PopulateRuleList(RuleSet ruleSet)
        {
            checkedListBoxRules.Items.Clear();
            checkedListBoxRules.ValueMember = "Id";
            checkedListBoxRules.DisplayMember = "Name";

            List<Rule> rules = new List<Rule>();
            APIResponse<Rule> response = null;
            List<string> ruleIdList = ruleSet == null ? ruleUser.RuleOwnership : ruleSet.Rules.Select(r => r.Id).ToList();
            foreach (string ruleId in ruleIdList)
            {
                response = await RuleAPIController.GetRuleAsync(ruleId);
                if (response.Code == System.Net.HttpStatusCode.OK)
                {
                    Rule rule = response.Data;
                    rules.Add(rule);
                }
                else
                {
                    MessageBox.Show(response.ReasonPhrase);
                }
            }

            rules = rules.OrderBy(r => r.Name).ToList();
            foreach (Rule r in rules)
            {
                checkedListBoxRules.Items.Add(r);
            }

            this.buttonRemoveRule.Enabled = ruleSet != null;
        }

        private async void buttonUpdateUserData_Click(object sender, EventArgs e)
        {
            RuleUser newData = new RuleUser()
            {
                Username = this.textBoxUsername.Text,
                PublicName = this.textBoxPublicName.Text,
                RuleOwnership = this.ruleUser.RuleOwnership,
                RuleSetOwnership = this.ruleUser.RuleSetOwnership
            };

            APIResponse response = await RuleAPIController.UpdateUserAsync(this.ruleUser.Username, newData);
            if (response.Code == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("User Updated");
                this.ruleUser = newData;
            }
            else
            {
                MessageBox.Show(response.ReasonPhrase);
                this.textBoxUsername.Text = this.ruleUser.Username;
                this.textBoxPublicName.Text = this.ruleUser.PublicName;
            }
        }

        private async void buttonUploadLocalRuleSet_Click(object sender, EventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            foreach (RuleSet rs in ruleSets)
            {
                await RuleAPIController.CreateRuleSetAsync(rs);
            }

            Relogin();
        }

        private void buttonDownloadRuleSetLocally_Click(object sender, EventArgs e)
        {
            List<RuleSet> checkedRuleSets = checkedListBoxRuleSets.CheckedItems.Cast<RuleSet>().ToList();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "RuleSet files (*.json)|*.json|All files (*.*)|*.*";
            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string path = Path.GetDirectoryName(sfd.FileName);
            string name = Path.GetFileNameWithoutExtension(sfd.FileName);
            string extn = Path.GetExtension(sfd.FileName);

            int index = 0;
            foreach (RuleSet rs in checkedRuleSets)
            {
                RuleReadWrite.WriteRuleSet(rs, path + "\\" + name + "(" + index + ")" + extn);
                index++;
            }
        }

        private async void buttonDeleteRuleItem_Click(object sender, EventArgs e)
        {
            List<RuleSet> checkedRuleSets = checkedListBoxRuleSets.CheckedItems.Cast<RuleSet>().ToList();
            foreach (RuleSet rs in checkedRuleSets)
            {
                await RuleAPIController.DeleteRuleSetAsync(rs.Id);
            }

            List<Rule> checkedRules = checkedListBoxRules.CheckedItems.Cast<Rule>().ToList();
            foreach (Rule r in checkedRules)
            {
                await RuleAPIController.DeleteRuleAsync(r.Id);
            }

            Relogin();
        }

        private void checkedListBoxRuleSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetRuleDisplay();

            RuleSet selectedRuleSet = checkedListBoxRuleSets.SelectedItem as RuleSet;
            PopulateRuleList(checkedListBoxRuleSets.SelectedItem as RuleSet);
        }

        private void checkedListBoxRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            Rule selectedRule = checkedListBoxRules.SelectedItem as Rule;
            if (selectedRule != null)
            {
                this.richTextBoxRuleDisplayJSON.Text = RuleReadWrite.ConvertRuleToString(selectedRule);
                this.richTextBoxRuleDisplayText.Text = selectedRule.String();
            }
        }

        private void ResetRuleDisplay()
        {
            this.richTextBoxRuleDisplayJSON.Text = "";
            this.richTextBoxRuleDisplayText.Text = "";
        }

        private void checkedListBoxRuleSets_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index == 0)
            {
                e.NewValue = e.CurrentValue;
            }
        }

        private async void buttonAddRules_Click(object sender, EventArgs e)
        {
            List<RuleSet> ruleSets = new List<RuleSet>();
            foreach (string ruleSetId in ruleUser.RuleSetOwnership)
            {
                APIResponse<RuleSet> response = await RuleAPIController.GetRuleSetAsync(ruleSetId);
                if (response.Code == System.Net.HttpStatusCode.OK)
                {
                    RuleSet ruleSet = response.Data;
                    ruleSets.Add(ruleSet);
                }
                else
                {
                    MessageBox.Show(response.ReasonPhrase);
                }
            }

            ruleSets = ruleSets.OrderBy(r => r.Name).ToList();

            RuleAddForm raf = new RuleAddForm(ruleSets, RuleAPIController);
            if (raf.ShowDialog() == DialogResult.OK)
            {
                // Get the rules and add them to the user rules:
                foreach (var kvp in raf.AddedRules)
                {
                    RuleSet updatedRS = ruleSets.Where(rs => rs.Id == kvp.Key).FirstOrDefault();

                    updatedRS.Rules.AddRange(kvp.Value);
                    APIResponse<string> result = await RuleAPIController.UpdateRuleSetAsync(updatedRS);
                    if (result.Code != System.Net.HttpStatusCode.OK)
                    {
                        MessageBox.Show(result.ReasonPhrase);
                    }
                    else if (result.Data != updatedRS.Id)
                    {
                        MessageBox.Show("ID changed");
                    }
                }

                PopulateRuleSetList();
            }
        }

        private async void buttonRemoveRule_Click(object sender, EventArgs e)
        {
            RuleSet updatedRS = checkedListBoxRuleSets.SelectedItem as RuleSet;
            List<Rule> selectedRules = this.checkedListBoxRules.CheckedItems.Cast<Rule>().ToList();
            updatedRS.Rules.RemoveAll(r => selectedRules.Any(sr => r.Id == sr.Id));
            APIResponse<string> response = await RuleAPIController.UpdateRuleSetAsync(updatedRS);
            if (response.Code != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.ReasonPhrase);
            }
            else if (response.Data != updatedRS.Id)
            {
                MessageBox.Show("ID changed");
            }

            PopulateRuleSetList();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            Relogin();
        }

        private void buttonSignout_Click(object sender, EventArgs e)
        {
            ruleUser = null;
            MainForm_Load(null, null);
        }

        private async void buttonMethodCheck_Click(object sender, EventArgs e)
        {
            APIResponse<List<ObjectTypes>> response = await DBMSController.GetTypesList();
            if (response.Code != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.ReasonPhrase);
            }
            APIResponse<List<ObjectTypes>> response0 = await RuleAPIController.GetTypesList();
            if (response0.Code != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response0.ReasonPhrase);
            }
            APIResponse<Dictionary<ObjectTypes, string>> response1 = await RuleAPIController.GetVOMethodsAsync();
            if (response1.Code != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response1.ReasonPhrase);
            }
            APIResponse<Dictionary<string, Type>> response2 = await RuleAPIController.GetPropertyMethodsAsync();
            if (response2.Code != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response2.ReasonPhrase);
            }
            APIResponse<Dictionary<string, Type>> response3 = await RuleAPIController.GetRelationMethodsAsync();
            if (response3.Code != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response3.ReasonPhrase);
            }
            MethodDisplayForm mdf = new MethodDisplayForm(response0.Data, response1.Data, response2.Data, response3.Data);
            mdf.ShowDialog();
        }
    }
}