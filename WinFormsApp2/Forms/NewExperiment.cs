using Newtonsoft.Json;
using SoupV2.Simulation;
using SoupV2.Simulation.Settings;
using System;
using System.IO;
using System.Windows.Forms;

namespace SoupForm.Forms
{
    public partial class NewExperimentForm : Form
    {

        /// <summary>
        /// The created settings objects
        /// </summary>
        public SimulationSettings NewSimulationSettings { get; set; } = DefaultSimulationSettings.GetSettings();
        /// <summary>
        /// The name of the new simulation (For the tab name)
        /// </summary>
        public string SimulationName { get; set; } = "New Simulation";

        /// <summary>
        /// Open a file stream to wherever we will save our file to.
        /// </summary>
        public Stream StatsFileStream { get; set; }
        public bool SaveStats { get; set; } = false;

        public string ChosenEntityDefinitionFolder { get; set; }
    
        public NewExperimentForm()
        {
            InitializeComponent();
            settingPropertyGrid.SelectedObject = NewSimulationSettings;
            experimentNameBox.Text = SimulationName;
            saveStatsButton.Enabled = false;


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void NewExperiment_Load(object sender, EventArgs e)
        {

        }

        private void loadSettingsButton_Click(object sender, EventArgs e)
        {
            
            if (openSettingsDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                using (StreamReader sr = new StreamReader(openSettingsDialog.OpenFile()))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.TypeNameHandling = TypeNameHandling.All;
                    NewSimulationSettings = (SimulationSettings)serializer.Deserialize(sr, typeof(SimulationSettings));
                    settingPropertyGrid.SelectedObject = null;
                    settingPropertyGrid.SelectedObject = NewSimulationSettings;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went wrong loading settings file {exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// When ok is pressed we need to do some validation to check the right data has been entered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
           
            if (SaveStats && (StatsFileStream is null)) {
                MessageBox.Show($"Please select a file to save statistics to.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ChosenEntityDefinitionFolder is null)
            {
                MessageBox.Show($"Please select an entity definition folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void saveStatsButton_Click_1(object sender, EventArgs e)
        {
            if (saveStatsFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                StatsFileStream = saveStatsFileDialog.OpenFile();
                statsFileLocation.Text = saveStatsFileDialog.FileName;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went wrong saving stats file {exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void saveSettingsButton_Click(object sender, EventArgs e)
        {
            if (saveSettingsFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(saveSettingsFileDialog.OpenFile()))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.TypeNameHandling = TypeNameHandling.All;
                    serializer.Serialize(sw, NewSimulationSettings);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went wrong saving settings file {exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void experimentNameBox_TextChanged(object sender, EventArgs e)
        {
            SimulationName = experimentNameBox.Text;
        }

        private void saveStatsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SaveStats = saveStatsCheckBox.Checked;
            saveStatsButton.Enabled = saveStatsCheckBox.Checked;
            statsFileLocation.Enabled = saveStatsButton.Enabled;
        }

        private void selectDefinitionFolderButton_Click(object sender, EventArgs e)
        {
            if (selectEntityDFolderDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            ChosenEntityDefinitionFolder = selectEntityDFolderDialog.SelectedPath;
            entityDFolderLabel.Text = selectEntityDFolderDialog.SelectedPath;
        }
    }
}
