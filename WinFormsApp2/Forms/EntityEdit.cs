using EntityComponentSystem;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Components.util;
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
    public partial class EntityEdit : UserControl
    {
        private Entity _target;
        public EntityEdit(Entity target)
        {
            InitializeComponent();
            _target = target;
            tagTextBox.Text = _target.Tag;
        }

        private void trySetTagButton_Click(object sender, EventArgs e)
        {
            var newTag = tagTextBox.Text;
            string oldTag = _target.Tag;

            if (!(_target.Parent is null))
            {
                foreach ((string key, Entity entity) in _target.Parent.Children)
                {
                    if (key == newTag)
                    {
                        MessageBox.Show($"Duplicate tag. Children of a parent must have a unique tag.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                _target.Parent.Children.Remove(oldTag);
                _target.Parent.Children.Add(newTag, _target);
            }
            _target.Tag = newTag;
            Invalidate();
        }

        /// <summary>
        /// display dialog for adding a component
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addComponentButton_Click(object sender, EventArgs e)
        {
            var chooseComponent = new ChooseComponentForm(ComponentCompatibility.GetCompatibleComponents(_target));
            chooseComponent.ShowDialog();

            if (chooseComponent.DialogResult != DialogResult.OK)
            {
                return;
            }

            var componentType = chooseComponent.Selected;
            var component = Activator.CreateInstance(componentType, _target);
            _target.AddComponents((AbstractComponent)component);
            Invalidate();
        }

        /// <summary>
        /// display dialog for deleting a component
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteComponentButton_Click_1(object sender, EventArgs e)
        {
            // Cannot delete transforms
            var currentComponents = _target.Components.Values
                .Where((AbstractComponent ac) => !(ac is TransformComponent))
                .Select((ac) => ac.GetType()).ToList();

            var chooseComponent = new ChooseComponentForm(currentComponents);
            chooseComponent.ShowDialog();

            if (chooseComponent.DialogResult != DialogResult.OK)
            {
                return;
            }

            var componentType = chooseComponent.Selected;
            _target.RemoveComponent(componentType);
        }

        private void tryDeleteEntityButton_Click(object sender, EventArgs e)
        {
            if (_target.IsRoot)
            {
                MessageBox.Show("Cannot delete root entity");
                return;
            }

            _target.OwnerPool.DestroyEntity(_target);
            Invalidate();
        }

        /// <summary>
        /// Add a new component with a transform
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addChildEntityButton_Click(object sender, EventArgs e)
        {
            Entity def = new Entity();
            def.AddComponents(new TransformComponent(def));
            string newTagAttempt = "Entity";
            // Make sure user can't add a duplicate tag
            while (_target.Children.ContainsKey(newTagAttempt))
            {
                newTagAttempt += "New";
            }
            def.Tag = newTagAttempt;
            _target.AddChild(def);
            _target.OwnerPool.AddDeserializedEntity(def, _target);
            Invalidate();
        }
    }
}
