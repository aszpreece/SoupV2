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
        }

        private void NewSimulationButton_Click(object sender, EventArgs e)
        {
            NewExperimentForm newExperimentForm = new NewExperimentForm();
            newExperimentForm.ShowDialog();
            if (newExperimentForm.DialogResult == DialogResult.OK)
            {
                var newSimTab = new TabPage(newExperimentForm.SimulationName);
                tabControl1.TabPages.Add(newSimTab);
                var simTab = new SimulationTabContent();
                simTab.Dock = DockStyle.Fill;
                newSimTab.Controls.Add(simTab);

            }
        }
    }
}
