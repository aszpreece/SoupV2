using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using SoupV2.EntityComponentSystem;
using SoupV2.Simulation;
using SoupV2.Simulation.Brain;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Components.util;
using SoupV2.Simulation.EntityDefinitions;
using SoupV2.Simulation.Settings;

namespace SoupForm.Forms
{
    public partial class EditorForm : Form
    {
        public EntityDefinition CurrentEntityDefinition { get; set; }
        public Entity CurrentEntity { get; set; }

        private bool _mouseDown = false;
        private Vector2 _lastLocation;

        private readonly float _zoomSpeed = 0.05f;
        private readonly float _minZoom = 0.1f;
        private readonly float _maxZoom = 2f;

        public EditorForm()
        {
            InitializeComponent();

            treeView1.AfterSelect += TreeView1_AfterSelect;

            // Default entity
            soupGraphicControl1.GraphicsInitialized += SoupGraphicControl1_GraphicsInitialized;

            // Add in drop down buttons for built in entities
            openBuiltIn.DropDownItems.AddRange(
                EntityDefinitionDatabase.DefaultEntities
                .Select((name) => new ToolStripMenuItem(name, null,
                    (s, a) =>
                    {
                        var db = new EntityDefinitionDatabase();
                        var definition = db.GetEntityDefinition(name);
                        OpenEntity(definition);
                    }))
                .ToArray());
                

        }

        public void OpenEntity(EntityDefinition e)
        {
            CurrentEntityDefinition = e;
            soupGraphicControl1.CurrentSimulation = new EntityEditorSimulation(DefaultSimulationSettings.GetSettings(), new EntityDefinitionDatabase());
            soupGraphicControl1.CurrentSimulation.InitGraphics(soupGraphicControl1.GraphicsDevice, soupGraphicControl1.Editor.Content);

            if (CurrentEntityDefinition is null)
            {
                // Default entity
                CurrentEntity = soupGraphicControl1
                    .CurrentSimulation
                    .EntityManager
                    .AddEntityFromDefinition("Soupling", soupGraphicControl1.CurrentSimulation.JsonSettings, "Soupling");
            }
            else
            {
                CurrentEntity = Entity.FromDefinition(CurrentEntityDefinition, soupGraphicControl1.CurrentSimulation.JsonSettings);
                soupGraphicControl1
                    .CurrentSimulation
                    .EntityManager
                    .AddDeserializedEntity(CurrentEntity);
            }

            TreeNode root = new TreeNode("");
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(root);
            BuildTree(CurrentEntity, root);
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Check something is selected and display the appropriate edit menu
            if (treeView1.SelectedNode is null)
            {
                return;
            }
            if (treeView1.SelectedNode.Tag is Entity)
            {
                editPanel.Controls.Clear();
                EntityEdit entityEdit = new EntityEdit((Entity)treeView1.SelectedNode.Tag);
                entityEdit.Invalidated += EntityEdit_Invalidated;
                editPanel.Controls.Add(entityEdit);
            }
            else
            {
                editPanel.Controls.Clear();
                if (treeView1.SelectedNode.Tag is BrainComponent)
                {
                    BrainEdit brainEditComponent = new BrainEdit((BrainComponent)treeView1.SelectedNode.Tag);
                    brainEditComponent.Dock = DockStyle.Fill;
                    editPanel.Controls.Add(brainEditComponent);
                } else
                {
                    PropertyGrid editComponent = new PropertyGrid();
                    editComponent.Dock = DockStyle.Fill;
                    editComponent.SelectedObject = treeView1.SelectedNode.Tag;
                    editPanel.Controls.Add(editComponent);
                }

            }
        }

        private void SoupGraphicControl1_GraphicsInitialized()
        {
            // Initial opening of entity.
            OpenEntity(Soupling.GetCritter(Color.White).ToDefinition());
        }

        private void openMenuItem_Click(object sender, System.EventArgs e)
        {
            var result = openEntityFileDialog.ShowDialog();
            if(result != DialogResult.OK)
            {
                return;
            }
            try
            {
                using (StreamReader sr = new StreamReader(openEntityFileDialog.OpenFile()))
                {
                    EntityDefinition d = new EntityDefinition(sr.ReadToEnd());
                    OpenEntity(d);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went wrong opening entity file {exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Performs validation on an entity and saves it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveMenuItem_Click(object sender, System.EventArgs e)
        {

            if (!ComponentCompatibility.ValidateEntity(CurrentEntity, out List<string> reasons)){
                MessageBox.Show($"Entity has errors. Please correct these errors before saving: {string.Join(',', reasons)}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (CurrentEntity.HasComponent<BrainComponent>())
            {
                var brain = CurrentEntity.GetComponent<BrainComponent>();
                // Validate the brain to check for any incorrect wirings.
                if (!BrainMapping.ValidateBrain(brain, brain.Owner, out List<string> invalidInputs, out List<string> invalidOutputs))
                {
                    string invalidInputsString = string.Join(",\n", invalidInputs);
                    string invalidOutputsString = string.Join(",\n", invalidOutputs);

                    MessageBox.Show($"The following brain inputs: {invalidInputsString} and outputs: {invalidOutputsString} are invalid.");
                    return;
                }
            }

            if (saveEntityFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(saveEntityFileDialog.OpenFile()))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Converters.Add(new Vector2Converter());
                    serializer.Converters.Add(new Texture2DConverter(null, null));
                    serializer.TypeNameHandling = TypeNameHandling.All;
                    serializer.Serialize(sw, CurrentEntity);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Something went wrong saving entity file {exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // These methods handle the camera movement
        private void soupGraphicsControl_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDown = true;
            Cursor = Cursors.NoMove2D;
            _lastLocation = new Vector2(e.X, e.Y);
        }

        private void soupGraphicsControl_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
            Cursor = Cursors.Default;

        }

        private void soupGraphicsControl_MouseMove(object sender, MouseEventArgs e)
        {
            // Works by finding the difference between the mouse in its previous position and now.
            if (_mouseDown)
            {
                var newLoc = new Vector2(e.X, e.Y);
                var detla = _lastLocation - newLoc;

                // Movement is inversely proportinate to zoom.
                soupGraphicControl1.Editor.MoveCam(detla * 1 / soupGraphicControl1.Editor.Cam.Zoom);
                _lastLocation = newLoc;
            }
        }

        private void soupGraphicsControl_OnMouseWheelUpwards(MouseEventArgs e)
        {
            var newZoom = soupGraphicControl1.Editor.Cam.Zoom += _zoomSpeed;

            soupGraphicControl1.Editor.Cam.Zoom = MathHelper.Clamp(newZoom, _minZoom, _maxZoom);
        }


        private void soupGraphicsControl_OnMouseWheelDownwards(MouseEventArgs e)
        {
            var newZoom = soupGraphicControl1.Editor.Cam.Zoom -= _zoomSpeed;
            soupGraphicControl1.Editor.Cam.Zoom = MathHelper.Clamp(newZoom, _minZoom, _maxZoom);
        }


        public void BuildTree(Entity target, TreeNode parent)
        {

            var thisEntityNode = new TreeNode(target.Tag);
            thisEntityNode.Tag = target;
            parent.Nodes.Add(thisEntityNode);
           

            foreach (var (_, component) in target.Components)
            {
                var componentNode = thisEntityNode.Nodes.Add(component.GetType().Name);
                componentNode.Tag = component;
            }

            foreach (var (_, child) in target.Children)
            {
                child.Parent = target;
                BuildTree(child, thisEntityNode);
            }
            parent.ExpandAll();
        }

        public void RebuildTree()
        {
            treeView1.Nodes.Clear();
            TreeNode root = new TreeNode("");
            treeView1.Nodes.Add(root);
            BuildTree(CurrentEntity, root);
        }

        private void treeView1_Click(object sender, System.EventArgs e)
        {

        }

        private void EntityEdit_Invalidated(object sender, InvalidateEventArgs e)
        {
            RebuildTree();
        }

        private void OpenEntityEdit()
        {
            editPanel.Controls.Clear();
        }
    }
}
