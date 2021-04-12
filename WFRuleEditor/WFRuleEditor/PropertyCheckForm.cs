using MathPackage;
using RuleAPI.Methods;
using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BIMRuleEditor
{
    public partial class PropertyCheckForm : Form
    {
        public PropertyCheck PropertyCheck { get; set; }
        public PropertyCheckBool PropertyCheckBool { get; set; }
        public PropertyCheckNum PropertyCheckNum { get; set; }
        public PropertyCheckString PropertyCheckString { get; set; }
        private bool SingleObj { get; set; }

        public PropertyCheckForm(bool singleObj)
        {
            InitializeComponent();
            SingleObj = singleObj;
            SetUpComboBoxes();

            this.PropertyCheckBool = new PropertyCheckBool(this.comboBoxBoolProp.Items[0] as string, OperatorBool.EQUAL, true);
            this.PropertyCheckNum = new PropertyCheckNum(this.comboBoxNumProp.Items[0] as string, OperatorNum.EQUAL, 1, Unit.CM);
            this.PropertyCheckString = new PropertyCheckString(this.comboBoxStringProp.Items[0] as string, OperatorString.EQUAL, "");
            this.radioButtonPropCheckBool.Checked = true;
            DisplayPropertyCheck();
        }

        public PropertyCheckForm(bool singleObj, PropertyCheck pc)
        {
            InitializeComponent();
            SingleObj = singleObj;
            SetUpComboBoxes();

            this.PropertyCheckBool = new PropertyCheckBool(this.comboBoxBoolProp.Items[0] as string, OperatorBool.EQUAL, true);
            this.PropertyCheckNum = new PropertyCheckNum(this.comboBoxNumProp.Items[0] as string, OperatorNum.EQUAL, 1, Unit.CM);
            this.PropertyCheckString = new PropertyCheckString(this.comboBoxStringProp.Items[0] as string, OperatorString.EQUAL, "");
            if (pc.GetType() == typeof(PropertyCheckBool))
            {
                PropertyCheckBool = pc as PropertyCheckBool;
                this.radioButtonPropCheckBool.Checked = true;
            }
            if (pc.GetType() == typeof(PropertyCheckNum))
            {
                PropertyCheckNum = pc as PropertyCheckNum;
                this.radioButtonPropCheckNum.Checked = true;
            }
            if (pc.GetType() == typeof(PropertyCheckString))
            {
                PropertyCheckString = pc as PropertyCheckString;
                this.radioButtonPropCheckString.Checked = true;
            }
            DisplayPropertyCheck();
        }

        private void SetUpComboBoxes()
        {
            Dictionary<string, Type> allProps = SingleObj ? MethodFinder.GetAllPropertyMethods() : MethodFinder.GetAllRelationMethods();
            foreach (var prop in allProps.Where(kvp=>kvp.Value == typeof(bool)))
            {
                this.comboBoxBoolProp.Items.Add(prop.Key);
            }
            foreach (OperatorBool OperatorBool in Enum.GetValues(typeof(OperatorBool)))
            {
                this.comboBoxOperBool.Items.Add(OperatorBool);
            }
            this.comboBoxBoolValue.Items.Add(true);
            this.comboBoxBoolValue.Items.Add(false);

            foreach (var prop in allProps.Where(kvp => kvp.Value == typeof(double)))
            {
                this.comboBoxNumProp.Items.Add(prop.Key);
            }
            foreach (OperatorNum OperatorNum in Enum.GetValues(typeof(OperatorNum)))
            {
                this.comboBoxOperNum.Items.Add(OperatorNum);
            }
            foreach (Unit Unit in Enum.GetValues(typeof(Unit)))
            {
                this.comboBoxNumUnit.Items.Add(Unit);
            }

            foreach (var prop in allProps.Where(kvp => kvp.Value == typeof(string)))
            {
                this.comboBoxStringProp.Items.Add(prop.Key);
            }
            foreach (OperatorString OperatorString in Enum.GetValues(typeof(OperatorString)))
            {
                this.comboBoxOperString.Items.Add(OperatorString);
            }
        }

        private void DisplayPropertyCheck()
        {
            this.comboBoxBoolProp.SelectedIndex = this.comboBoxBoolProp.Items.IndexOf(PropertyCheckBool.Name);
            this.comboBoxOperBool.SelectedIndex = this.comboBoxOperBool.Items.IndexOf(PropertyCheckBool.Operation);
            this.comboBoxBoolValue.SelectedIndex = this.comboBoxBoolValue.Items.IndexOf(PropertyCheckBool.Value);

            this.comboBoxNumProp.SelectedIndex = this.comboBoxNumProp.Items.IndexOf(PropertyCheckNum.Name);
            this.comboBoxOperNum.SelectedIndex = this.comboBoxOperNum.Items.IndexOf(PropertyCheckNum.Operation);
            this.textBoxNumValue.Text = PropertyCheckNum.Value.ToString();
            this.comboBoxNumUnit.SelectedIndex = this.comboBoxNumUnit.Items.IndexOf(PropertyCheckNum.ValueUnit);

            this.comboBoxStringProp.SelectedIndex = this.comboBoxStringProp.Items.IndexOf(PropertyCheckString.Name);
            this.comboBoxOperString.SelectedIndex = this.comboBoxOperString.Items.IndexOf(PropertyCheckString.Operation);
            this.textBoxStringValue.Text = PropertyCheckString.Value;
        }

        private void RadioButtonPropCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonPropCheckBool.Checked)
            {
                this.comboBoxBoolProp.Enabled = true;
                this.comboBoxOperBool.Enabled = true;
                this.comboBoxBoolValue.Enabled = true;
                this.comboBoxNumProp.Enabled = false;
                this.comboBoxOperNum.Enabled = false;
                this.comboBoxNumUnit.Enabled = false;
                this.textBoxNumValue.BackColor = Color.White;
                this.textBoxNumValue.Enabled = false;
                this.comboBoxStringProp.Enabled = false;
                this.comboBoxOperString.Enabled = false;
                this.textBoxStringValue.Enabled = false;

                this.PropertyCheck = PropertyCheckBool;
            }
            if (this.radioButtonPropCheckNum.Checked)
            {
                this.comboBoxBoolProp.Enabled = false;
                this.comboBoxOperBool.Enabled = false;
                this.comboBoxBoolValue.Enabled = false;
                this.comboBoxNumProp.Enabled = true;
                this.comboBoxOperNum.Enabled = true;
                this.comboBoxNumUnit.Enabled = true;
                if (!int.TryParse(this.textBoxNumValue.Text, out int num))
                {
                    this.textBoxNumValue.BackColor = Color.PaleVioletRed;
                }
                this.textBoxNumValue.Enabled = true;
                this.comboBoxStringProp.Enabled = false;
                this.comboBoxOperString.Enabled = false;
                this.textBoxStringValue.Enabled = false;

                this.PropertyCheck = PropertyCheckNum;
            }
            if (this.radioButtonPropCheckString.Checked)
            {
                this.comboBoxBoolProp.Enabled = false;
                this.comboBoxOperBool.Enabled = false;
                this.comboBoxBoolValue.Enabled = false;
                this.comboBoxNumProp.Enabled = false;
                this.comboBoxOperNum.Enabled = false;
                this.comboBoxNumUnit.Enabled = false;
                this.textBoxNumValue.BackColor = Color.White;
                this.textBoxNumValue.Enabled = false;
                this.comboBoxStringProp.Enabled = true;
                this.comboBoxOperString.Enabled = true;
                this.textBoxStringValue.Enabled = true;

                this.PropertyCheck = PropertyCheckString;
            }
        }

        private void comboBoxBoolProp_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PropertyCheckBool.Name = this.comboBoxBoolProp.SelectedItem as string;
        }

        private void comboBoxNumProp_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PropertyCheckNum.Name = this.comboBoxNumProp.SelectedItem as string;
        }

        private void comboBoxStringProp_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PropertyCheckString.Name = this.comboBoxStringProp.SelectedItem as string;
        }

        private void comboBoxOperBool_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PropertyCheckBool.Operation = Enum.GetValues(typeof(OperatorBool)).Cast<OperatorBool>().ToList()[this.comboBoxOperBool.SelectedIndex];
        }

        private void comboBoxOperNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PropertyCheckNum.Operation = Enum.GetValues(typeof(OperatorNum)).Cast<OperatorNum>().ToList()[this.comboBoxOperNum.SelectedIndex];
        }

        private void comboBoxOperString_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PropertyCheckString.Operation = Enum.GetValues(typeof(OperatorString)).Cast<OperatorString>().ToList()[this.comboBoxOperString.SelectedIndex];
        }

        private void comboBoxBoolValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PropertyCheckBool.Value = (this.comboBoxBoolValue.SelectedItem as bool? == true);
        }

        private void textBoxNumValue_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(this.textBoxNumValue.Text, out int num))
            {
                this.PropertyCheckNum.SetNewValue(Convert.ToDouble(this.textBoxNumValue.Text));
                this.textBoxNumValue.BackColor = Color.White;
            }
            else
            {
                this.textBoxNumValue.BackColor = Color.PaleVioletRed;
                return;
            }
        }

        private void comboBoxNumUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PropertyCheckNum.SetNewUnit(Enum.GetValues(typeof(Unit)).Cast<Unit>().ToList()[this.comboBoxNumUnit.SelectedIndex]);
        }

        private void textBoxStringValue_TextChanged(object sender, EventArgs e)
        {
            this.PropertyCheckString.Value = this.textBoxStringValue.Text;
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}