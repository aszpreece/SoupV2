using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using SoupV2.EntityComponentSystem;
using SoupV2.Simulation;
using SoupV2.Simulation.Statistics.StatLoggers;
using SoupV2.UI;
using SoupV2.util;
using System;
using UI;


namespace SoupV2
{
    public class Soup : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Desktop _desktop;

        public Simulation.Simulation CurrentSimulation { get; set; }

        public Soup()
        {
            this.IsFixedTimeStep = true;
           // this.TargetElapsedTime = TimeSpan.FromMilliseconds(10);

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.PreparingDeviceSettings += Graphics_PreparingDeviceSettings;
            _graphics.ApplyChanges();
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        // Antialisaing
        private void Graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            _graphics.PreferMultiSampling = true;
            e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 8;
        }


        protected override void Initialize()
        {
            base.Initialize();

        }

    
        protected override void LoadContent()
        {
            base.LoadContent();

            _graphics.IsFullScreen = true;

            SpriteBatchExtension.InitSpriteBatchExtension(GraphicsDevice);
            SerializerSettings.InitializeSettings(Content);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureAtlas.Circle = Content.Load<Texture2D>("bin/DesktopGL/circle");
            TextureAtlas.Eye = Content.Load<Texture2D>("bin/DesktopGL/eye");
            TextureAtlas.Mouth = Content.Load<Texture2D>("bin/DesktopGL/mouth");
            TextureAtlas.Soup = Content.Load<Texture2D>("bin/DesktopGL/soup");
            TextureAtlas.Nose = Content.Load<Texture2D>("bin/DesktopGL/SoupNose");
            TextureAtlas.BoxingGloveExtended = Content.Load<Texture2D>("bin/DesktopGL/BoxingGloveExtended");
            TextureAtlas.BoxingGloveRetracted = Content.Load<Texture2D>("bin/DesktopGL/BoxingGloveRetracted");
            // TextureAtlas.OutlineEffect = Content.Load<SpriteEffect>("bin/DesktopGL/Selected");

            TextureAtlas.Font = Content.Load<SpriteFont>("bin/DesktopGL/Energy");

            MyraEnvironment.Game = this;

            _desktop = new Desktop();

            var mainMenu = new MainMenuUI(this);
         
            _desktop.Root = mainMenu;
            mainMenu._quit.Selected += (s, a) => Exit();
            

            //mainUI._gameSpeedSlider.ValueChanged += (s, a) => {
            //    _gameSpeed = _maxGameSpeed * a.NewValue / 100;
            // mainUI._gameSpeedLabel.Text = $"Simulation Speed: {Math.Round(_gameSpeed, 2)}";
            // };
            //mainUI._newScenario.Selected += (s, a) => _desktop.Root = new ScenarioUI();

        }
         


        protected override void Update(GameTime gameTime)
        {

            if (!(CurrentSimulation is null))
            {
                CurrentSimulation.Update(gameTime);
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            if (!(CurrentSimulation is null))
            {
                CurrentSimulation.Draw(gameTime, _spriteBatch);
            }

            _desktop.Render();

            base.Draw(gameTime);
        }

        internal void BeginExperiment(SimulationSettings simSettings, StatLogDestination destination, string file)
        {
            CurrentSimulation = new Simulation.Simulation(Window, simSettings, destination, file);
            CurrentSimulation.Initialize();
            _desktop.Root = new MainUI(this);
        }

        internal void EndExperiment()
        {
            CurrentSimulation = null;

            var mainMenu = new MainMenuUI(this);
            _desktop.Root = mainMenu;

        }
    }
}
