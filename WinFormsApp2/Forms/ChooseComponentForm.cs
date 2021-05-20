using EntityComponentSystem;
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
    public partial class ChooseComponentForm : Form
    {
        public Type Selected;
        public ChooseComponentForm(List<Type> displayList)
        {
            InitializeComponent();
            foreach (var t in displayList)
            {
                var item = new ListViewItem(t.Name);
                item.Tag = t;
                componentList.Items.Add(item);
            }
        }


        private void okButton_Click(object sender, EventArgs e)
        {
            // Error if no component selected
            if (componentList.SelectedItems.Count < 1)
            {
                MessageBox.Show($"Please select a component.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // ok because we disabled multi select
            Selected = (Type)componentList.SelectedItems[0].Tag;
            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
