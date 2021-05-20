using SoupV2.Simulation.Brain;
using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoupForm.Forms
{
    public partial class BrainEdit : UserControl
    {

        BrainComponent _target;


        public List<string> ValidInputs { get; set; }
        public List<string> ValidOutputs { get; set; }

        /// <summary>
        /// Properties that show hwat inputs can still be added
        /// </summary>
        public List<string> InputsToAdd {
            get => ValidInputs.Except(_target.InputMap.Values).ToList();
        }
        public List<string> OutputsToAdd
        {
            get => ValidOutputs.Except(_target.OutputMap.Values).ToList();
        }

        /// <summary>
        /// Component for editing brains
        /// </summary>
        /// <param name="brain"></param>
        public BrainEdit(BrainComponent brain)
        {
            InitializeComponent();
            _target = brain;
            UpdateData();

            inputList.View = View.Details;
            outputList.View = View.Details;
            inputList.Columns.Add("Alias");
            inputList.Columns.Add("Source");

            outputList.Columns.Add("Alias");
            outputList.Columns.Add("Controls");

            // Validate the brain to check for any incorrect wirings.
            if (!BrainMapping.ValidateBrain(brain, brain.Owner, out List<string> invalidInputs, out List<string> invalidOutputs))
            {
                string invalidInputsString = string.Join(",\n", invalidInputs);
                string invalidOutputsString = string.Join(",\n", invalidOutputs);

                MessageBox.Show($"The following brain inputs: {invalidInputsString} and outputs: {invalidOutputsString} are invalid.");
            }

        }

        private void deleteInputButton_Click(object sender, EventArgs e)
        {
            if (inputList.SelectedItems.Count <= 0)
            {
                return;
            }
            KeyValuePair<string, string> selected = (KeyValuePair<string, string>)inputList.SelectedItems[0].Tag;
            _target.InputMap.Remove(selected.Key);
            UpdateData();

        }

        private void deleteSelectedOutput_Click(object sender, EventArgs e)
        {
            if (outputList.SelectedItems.Count <= 0)
            {
                return;
            }
            KeyValuePair<string, string> selected = (KeyValuePair<string, string>)outputList.SelectedItems[0].Tag;
            _target.OutputMap.Remove(selected.Key);
            outputList.Update();
            UpdateData();
        }

        private void addNewInputButton_Click(object sender, EventArgs e)
        {
            if (inputComboBox.SelectedItem is null)
            {
                return;
            }
            string chosenAlias = PromptInputOutputName("Please enter input alias: ", "Alias");
            if (_target.InputMap.ContainsKey(chosenAlias))
            {
                MessageBox.Show("An input already exists with that alias!");
                return;
            }

            _target.InputMap.Add(chosenAlias, (string)inputComboBox.SelectedItem);
            UpdateData();
        }

        private void addNewOuputButton_Click(object sender, EventArgs e)
        {
            if (outputComboBox.SelectedItem is null)
            {
                return;
            }
            string chosenAlias = PromptInputOutputName("Please enter output alias: ", "Alias");
            if (_target.OutputMap.ContainsKey(chosenAlias))
            {
                MessageBox.Show("An output already exists with that alias!");
                return;
            }

            _target.OutputMap.Add(chosenAlias, (string)outputComboBox.SelectedItem);
            UpdateData();
        }

        private void UpdateData()
        {
            inputList.Items.Clear();
            outputList.Items.Clear();

            foreach (var (alias, id) in _target.InputMap)
            {
                var item = new ListViewItem(new string[] { alias, id });
                item.Tag = new KeyValuePair<string, string>(alias, id);
                inputList.Items.Add(item);
            }
            foreach (var (alias, id) in _target.OutputMap)
            {
                var item = new ListViewItem(new string[] { alias, id });
                item.Tag = new KeyValuePair<string, string>(alias, id);
                outputList.Items.Add(item);
            }

            ValidInputs = BrainMapping.GetAllInputs(_target.Owner);
            ValidOutputs = BrainMapping.GetAllControls(_target.Owner);

            outputComboBox.DataSource = null;
            outputComboBox.DataSource = OutputsToAdd;

            inputComboBox.DataSource = null;
            inputComboBox.DataSource = InputsToAdd;
        }

        /// <summary>
        /// Display a message box to enter the name of a new input or output
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static string PromptInputOutputName(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Width = 400 };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
