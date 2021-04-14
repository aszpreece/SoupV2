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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            newSimulationButton.Click += NewSimulationButton_Click;
            endSimulationButton.Click += EndSimulationButton_Click;
        }

        private void EndSimulationButton_Click(object sender, EventArgs e)
        {
            //Safely end a simulation by removings its tab.
            var tab = tabControl1.SelectedTab;
            if (!(tab is null))
            {
                tabControl1.TabPages.Remove(tab);
                tab.Dispose();
            }
        }

        private void NewSimulationButton_Click(object sender, EventArgs e)
        {
            NewExperimentForm newExperimentForm = new NewExperimentForm();
            newExperimentForm.ShowDialog();
            if (newExperimentForm.DialogResult == DialogResult.OK)
            {

                try
                {
                    var newSimTab = new TabPage(newExperimentForm.SimulationName);
                    tabControl1.TabPages.Add(newSimTab);
                    var simTab = new SimulationTabContent(
                        newExperimentForm.StatsFileStream,
                        newExperimentForm.NewSimulationSettings
                    );
                    simTab.Dock = DockStyle.Fill;
                    newSimTab.Controls.Add(simTab);

                } catch (Exception exception)
                {
                    MessageBox.Show($"Something went wrong initializing the new simulation: {exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }
    }
}
