using Newtonsoft.Json;
using SoupV2.Simulation;
using System;
using System.IO;
using System.Windows.Forms;

namespace SoupForm.Forms
{
    public partial class NewExperimentForm : Form
    {

        SimulationSettings NewSimulationSettings { get; set; } = new SimulationSettings();
        public string SimulationName { get; set; } = "New Simulation";
        public NewExperimentForm()
        {
            InitializeComponent();
            settingPropertyGrid.SelectedObject = NewSimulationSettings;
            experimentNameBox.Text = SimulationName;
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
                    settingPropertyGrid.SelectedObject = NewSimulationSettings;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went wrong loading settings file {exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void okButton_Click(object sender, EventArgs e)
        {
           
            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void saveStatsButton_Click_1(object sender, EventArgs e)
        {

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
                    serializer.Serialize(sw, typeof(SimulationSettings));
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
    }
}
