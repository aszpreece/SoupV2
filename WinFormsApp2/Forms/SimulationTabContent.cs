using Microsoft.Xna.Framework;
using SoupV2.Simulation;
using SoupV2.Simulation.Events;
using SoupV2.Simulation.Settings;
using SoupV2.Simulation.Statistics;
using SoupV2.Simulation.Statistics.StatLoggers;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SoupForm.Forms
{
    public partial class SimulationTabContent : UserControl
    {
        private bool _mouseDown = false;
        private Vector2 _lastLocation;

        private readonly float _zoomSpeed = 0.05f;
        private readonly float _minZoom = 0.1f;
        private readonly float _maxZoom = 2f;

        public int MaxEventsInLog = 50;


        private Thread _statLoggingThread;
        public SimulationTabContent(Stream statsOut, string entityDefinitionPath, SimulationSettings settings)
        {
            InitializeComponent();
            var definitionDatabase = LocalStorage.GetDefinitionDatabase(entityDefinitionPath);

            var newSim = new Simulation(settings, definitionDatabase);
            if (!(statsOut is null))
            {
                // Set up statistics gathering
                StatisticsGatherer gatherer = new StatisticsGatherer();
                newSim.WeaponSystem.OnAttack += gatherer.HandleInfo;
                newSim.ReproductionSystem.BirthEvent += gatherer.HandleInfo;
                newSim.HealthDeathSystem.OnDeath += gatherer.HandleInfo;
                newSim.OldAgeDeathSystem.OnDeath += gatherer.HandleInfo;
                newSim.EnergyDeathSystem.OnDeath += gatherer.HandleInfo;

                newSim.CritterPositionReporter.OnReport += gatherer.HandleInfo;
                newSim.FoodPositionReporter.OnReport += gatherer.HandleInfo;
                newSim.VisibleColourInfoReporter.OnReport += gatherer.HandleInfo;

                // Set a stats logger to run until closed.
                FileStatLogger logger = new FileStatLogger(gatherer, statsOut);
                _statLoggingThread = new Thread(() => logger.LogStats());
                _statLoggingThread.Start();
            }

            // Subscribe to the events for the UI.
            //newSim.WeaponSystem.OnAttack += postSimulationEvent;
            newSim.ReproductionSystem.BirthEvent += postSimulationEvent;
            newSim.HealthDeathSystem.OnDeath += postSimulationEvent;
            newSim.EnergyDeathSystem.OnDeath += postSimulationEvent;
            newSim.OldAgeDeathSystem.OnDeath += postSimulationEvent;

            soupGraphicsControl.CurrentSimulation = newSim;
            soupGraphicsControl.GraphicsInitialized += () =>
            {
                soupGraphicsControl.CurrentSimulation.InitGraphics(soupGraphicsControl.GraphicsDevice, soupGraphicsControl.Editor.Content);
                soupGraphicsControl.CurrentSimulation.SetUp();
            };
            eventView.View = View.Details;
            eventView.GridLines = true;
            
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            // Hack to stop crash.
            // If we do not set ShouldUpdate to false then the component will try and access its disposed self.
            soupGraphicsControl.ShouldUpdate = false;

            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);

            // Stops any further stat logging.
            _statLoggingThread?.Interrupt();
            _statLoggingThread?.Join();
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
                soupGraphicsControl.Editor.MoveCam(detla * 1 / soupGraphicsControl.Editor.Cam.Zoom);
                _lastLocation = newLoc;
            }
        }

        private void soupGraphicsControl_OnMouseWheelUpwards(MouseEventArgs e)
        {
            var newZoom = soupGraphicsControl.Editor.Cam.Zoom += _zoomSpeed;
           
            soupGraphicsControl.Editor.Cam.Zoom = MathHelper.Clamp(newZoom, _minZoom, _maxZoom);
        }


        private void soupGraphicsControl_OnMouseWheelDownwards(MouseEventArgs e)
        {
            var newZoom = soupGraphicsControl.Editor.Cam.Zoom -= _zoomSpeed;
            soupGraphicsControl.Editor.Cam.Zoom = MathHelper.Clamp(newZoom, _minZoom, _maxZoom);
        }


        // Class for storing event info
        class EventListViewItem : ListViewItem
        {
            public EventListViewItem(string[] items) : base(items)
            {

            }
            public AbstractEventInfo EventTag { get; set; }
        }

        private void postSimulationEvent(AbstractEventInfo info)
        {
            var listItem = new EventListViewItem(info.ToInfo());
            listItem.EventTag = info;
            eventView.Items.Insert(0, listItem);
            // Only keep X last events
            if (eventView.Items.Count > MaxEventsInLog)
            {
                eventView.Items.RemoveAt(eventView.Items.Count - 1);
            }

        }

        private void eventView_DoubleClick(object sender, EventArgs e)
        {
            var eventItem = (EventListViewItem) eventView.SelectedItems[0];
            var eventInfo = eventItem.EventTag;
            soupGraphicsControl.Editor.Cam.Position = eventInfo.Location;
        }

        private void soupGraphicsControl_Click(object sender, EventArgs e)
        {

        }
    }
}
