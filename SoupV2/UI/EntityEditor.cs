using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Properties;
using SoupV2.CustomMyra;
using SoupV2.EntityComponentSystem;
using SoupV2.Simulation;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.EntityDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UI;

namespace SoupV2.UI
{
    public class EntityEditor : Panel
    {

        Entity target;
        private PropertyGrid _componentEditor;
        private ScrollViewer _editorScrollPane;
        private EntityEditWidget _entityEditWidget;
        private ScrollViewer _entityTreeScroller;
        private EntityEditorSimulation _simulation;
        /// <summary>
        /// List of all the valid components we can add. 
        /// These are all in the same namespace and all need to inherit AbstractComponent
        /// </summary>
        private List<Type> _addableComponentTypes = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.Namespace == "SoupV2.Simulation.Components")
                       .Where(t => typeof(AbstractComponent).IsAssignableFrom(t))
                      .ToList();


        public EntityEditor(Soup soup)
        {
            var settings = new SimulationSettings();
            _simulation = new EntityEditorSimulation(soup.Window, settings);
            soup.CurrentSimulation = _simulation;
            BuildUI();
        }
        private void BuildUI()
        {

            _saveEntity = new MenuItem();
            _saveEntity.Text = "Save Entity";
            _saveEntity.Id = "_saveEntity";

            _newEntity = new MenuItem();
            _newEntity.Text = "New Entity";
            _newEntity.Id = "_newEntity";

            _quit = new MenuItem();
            _quit.Text = "Quit";
            _quit.Id = "_quit";

            var verticalMenu1 = new VerticalMenu();
            verticalMenu1.Background = new SolidBrush("#404040FF");
            verticalMenu1.Items.Add(_saveEntity);
            verticalMenu1.Items.Add(_newEntity);
            verticalMenu1.Items.Add(_quit);

            var mainGrid = new Grid();
            //mainGrid.ShowGridLines = true;
            //mainGrid.ColumnSpacing = 8;
            //mainGrid.RowSpacing = 8;
            mainGrid.ColumnsProportions.Add(new Proportion
            {
                Type = Myra.Graphics2D.UI.ProportionType.Auto,
            });
            mainGrid.ColumnsProportions.Add(new Proportion
            {
                Type = Myra.Graphics2D.UI.ProportionType.Fill,
            });
            mainGrid.RowsProportions.Add(new Proportion
            {
                Type = Myra.Graphics2D.UI.ProportionType.Part,
            });
            mainGrid.RowsProportions.Add(new Proportion
            {
                Type = Myra.Graphics2D.UI.ProportionType.Fill,
            });

            mainGrid.Widgets.Add(verticalMenu1);


            target = Entity.FromDefinition(Critter.GetCritter(TextureAtlas.Circle, Color.White));
            target.Tag = "Foo";
            _simulation.EditTarget = target;

            _componentEditor = new PropertyGrid();
            _editorScrollPane = new ScrollViewer();
            _editorScrollPane.MinHeight = 250;
            _editorScrollPane.Background = new SolidBrush("#202020FF");
            _editorScrollPane.VerticalAlignment = VerticalAlignment.Bottom;

            _editorScrollPane.Content = _componentEditor;
            _editorScrollPane.GridRow = 1;
            _editorScrollPane.GridColumn = 1;
            mainGrid.Widgets.Add(_editorScrollPane);

            _entityTreeScroller = new ScrollViewer();
            _entityTreeScroller.MinHeight = 250;
            _entityTreeScroller.MinWidth = 250;
            _entityTreeScroller.Background = new SolidBrush("#202020FF");
            _entityTreeScroller.Content = new Tree();
            _entityTreeScroller.GridRow = 1;
            mainGrid.Widgets.Add(_entityTreeScroller);
            RebuildTree();


            _entityEditWidget = new EntityEditWidget(target, this);
            //BuildTree(target, (CustomTree)root, "", entityTree);

            Panel previewPanel = new Panel();
            previewPanel.GridColumn = 1;
            previewPanel.MinWidth = 4000;
            previewPanel.MinHeight = 4000;
            Widgets.Add(previewPanel);
            Widgets.Add(mainGrid);

        }

        public void BuildTree(Entity target, TreeNode parent)
        {

            var thisEntityNode = parent.AddSubNode(target.Tag);
            thisEntityNode.TouchDown += (s, a) =>
            {

                _entityEditWidget = CreateEditWidget(target);
                _editorScrollPane.Content = _entityEditWidget;

            };

            foreach (var (_, component) in target.Components)
            {
                var componentNode = thisEntityNode.AddSubNode(component.GetType().Name);
                componentNode.TouchDown += (s, a) =>
                {
                    if (component is BrainComponent)
                    {
                        _editorScrollPane.Content = new BrainEditor((BrainComponent)component);
                    }
                    else
                    {
                        _componentEditor.Object = component;
                        _editorScrollPane.Content = _componentEditor;
                    }
                };
            }

            foreach (var (_, child) in target.Children)
            {
                child.Parent = target;
                BuildTree(child, thisEntityNode);
            }
        }

        public void RebuildTree()
        {

            var entityTree = new Tree();
            //entityTree.IsExpanded = true;
            //Hack so we don't have top root node
            _entityTreeScroller.Content = entityTree;
            BuildTree(target, entityTree);
        }

        public EntityEditWidget CreateEditWidget(Entity target)
        {
            var widget = new EntityEditWidget(target, this);
            widget._removeComponentButton.Click += (s, a) =>
            {
                if (target.Components.Count <= 0)
                {
                    return;
                }
                Grid grid = new Grid();

                // Make list of all components that are removable
                ListBox box = new ListBox();
                box.Width = 200;
                box.GridColumn = 0;
                box.GridRow = 0;
                box.GridColumnSpan = 2;
                foreach ((int id, AbstractComponent comp) in target.Components)
                {
                    // Logic for removing components
                    if (!(comp is TransformComponent))
                    {
                        var item = new ListItem(comp.GetType().Name, Color.White, comp.GetType());
                        box.Items.Add(item);
                    }
                }
                box.SelectedIndex = 0;

                grid.Widgets.Add(box);

                Dialog removeComponentWindow = new Dialog()
                {
                    Content = grid,
                    Title = "Select Component To Remove",
                    Left = 676,
                    Top = 205,
                    Id = "_removeComponent",
                };

                removeComponentWindow.Closed += (s, a) =>
                {
                    if (!removeComponentWindow.Result)
                    {
                        // Dialog was either closed or "Cancel" clicked
                        return;
                    }

                    // "Ok" was clicked or Enter key pressed
                    // Remove item from list
                    target.RemoveComponent((Type)box.SelectedItem.Tag);
                    RebuildTree();
                };

                removeComponentWindow.ShowModal(Desktop);
            };

            widget._deleteEntityButton.Click += (s, a) =>
            {
                var dialogue = Dialog.CreateMessageBox("Delete Entity", $"Are you sure you would like to delete entity {target.Tag}?");
                dialogue.Closed += (s, a) =>
                {
                    if (!dialogue.Result)
                    {
                        // Escape or "Cancel"
                        return;
                    };
                    target.Parent.Children.Remove(target.Tag);
                    _simulation.Manager.DestroyEntity(target);
                    RebuildTree();
                };
                dialogue.ShowModal(Desktop);
            };

            widget._addChildButton.Click += (s, a) =>
            {
                Grid grid = new Grid();

                Dialog dialog = new Dialog
                {
                    Title = "New Entity"
                };

                var stackPanel = new HorizontalStackPanel
                {
                    Spacing = 8
                };
                stackPanel.Proportions.Add(new Proportion
                {
                    Type = ProportionType.Auto,
                });
                stackPanel.Proportions.Add(new Proportion
                {
                    Type = ProportionType.Fill,
                });

                var label1 = new Label
                {
                    Text = "Tag:"
                };
                stackPanel.Widgets.Add(label1);

                var textBox1 = new TextBox();
                stackPanel.Widgets.Add(textBox1);

                dialog.Content = stackPanel;

                dialog.Closed += (s, a) => {
                    if (!dialog.Result)
                    {
                        // Dialog was either closed or "Cancel" clicked
                        return;
                    }

                    if (!EntityPool.IsValidTag(textBox1.Text))
                    {
                        Dialog.CreateMessageBox("Error", "Invalid tag.").ShowModal(Desktop);
                        return;
                    }
                    if (target.Children.ContainsKey(textBox1.Text))
                    {
                        Dialog.CreateMessageBox("Error", $"Already a child with tag {textBox1.Text}").ShowModal(Desktop);
                        return;
                    }
                    // "Ok" was clicked or Enter key pressed
                    Entity newChild = new Entity(textBox1.Text);
                    newChild.AddComponents(new TransformComponent(newChild));
                    target.AddChild(newChild);
                };

                dialog.ShowModal(Desktop);

            };

            widget._addComponentButton.Click += (s, a) =>
            {
                Grid grid = new Grid();

                // Make list of all components that are removable
                ListBox box = new ListBox();
                box.Width = 200;
                box.GridColumn = 0;
                box.GridRow = 0;
                box.GridColumnSpan = 2;
                foreach (Type component in _addableComponentTypes)
                {
                    if (target.HasComponent(component))
                    {
                        continue;
                    }
                    var item = new ListItem(component.Name, Color.White, component);
                    box.Items.Add(item);
                }
                if (box.Items.Count > 0)
                {
                    box.SelectedIndex = 0;
                }

                grid.Widgets.Add(box);

                Dialog addComponentWindow = new Dialog()
                {
                    Content = grid,
                    Title = "Select component to add...",
                    Left = 676,
                    Top = 205,
                    Id = "_selectAddComponent",
                };

                addComponentWindow.Closed += (s, a) =>
                {
                    if (!addComponentWindow.Result)
                    {
                        // Dialog was either closed or "Cancel" clicked
                        return;
                    }
                    if (box.SelectedItem is null)
                    {
                        // No more types of component to add.
                        return;
                    }
                    // "Ok" was clicked or Enter key pressed
                    // Add selected component if exists.
                    var componentType = (Type)box.SelectedItem.Tag;
                    // Create a new component instance using the tagged type.
                    var component = Activator.CreateInstance(componentType, target);
                    target.AddComponents((AbstractComponent)component);
                    RebuildTree();
                };

                addComponentWindow.ShowModal(Desktop);
            };

            return widget;
        }

        public MenuItem _saveEntity;
        public MenuItem _newEntity;
        public MenuItem _quit;
        public HorizontalSlider _gameSpeedSlider;
        public Panel _gameSpeedPanel;
    }
}

