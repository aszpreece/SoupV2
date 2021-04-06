using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using Newtonsoft.Json;
using SoupV2.EntityComponentSystem;
using SoupV2.NEAT;
using SoupV2.NEAT.Genes;
using SoupV2.NEAT.mutation;
using SoupV2.Simulation;
using SoupV2.Simulation.Brain;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.EntityDefinitions;
using SoupV2.Simulation.Grid;
using SoupV2.Simulation.Physics;
using SoupV2.Simulation.Systems;
using SoupV2.Simulation.Systems.Abilities;
using SoupV2.Simulation.Systems.Energy;
using SoupV2.Systems;
using SoupV2.UI;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoupV2
{
    public class Soup : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Desktop _desktop;
        public Soup()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.PreparingDeviceSettings += Graphics_PreparingDeviceSettings;
            _graphics.ApplyChanges();
            IsMouseVisible = true;
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

        EntityPool _main;
        private RenderSystem _renderSystem;
        private TransformHierarchySystem _transformHierarchySystem;
        private VelocitySystem _velocitySystem;
        
        private SortAndSweep _rigidbodyCollisionSystem;
        private SortAndSweep _mouthCollisionSystem;
        private SortAndSweep _weaponHealthCollisionSystem;


        private RigidBodySystem _rigidBodySystem;
        private DragSystem _dragSystem;
        private MovementControlSystem _movementControlSystem;
        private BrainSystem _brainSystem;
        private ReproductionSystem _reproductionSystem;
        private WorldBorderSystem _worldBorderSystem;
        private VisionSystem _visionSystem;
        private RigidBodyCollisionSystem _rigidBodyCollisionSystem;
        private MouthFoodCollisionSystem _mouthFoodCollisionSystem;
        private EnergyDeathSystem _energyDeathSystem;
        private InfoRenderSystem _infoRenderSystem;
        private FoodRespawnSystem _foodRespawnSystem;
        private MovementControlEnergyCostSystem _movementControlEnergyCostSystem;
        private EnergyManager _energyManager;
        private NoseSystem _noseSystem;
        private HealthDeathSystem _healthDeathSystem;
        private WeaponSystem _weaponSystem;
        private AdjacencyGrid _grid;

        private float _gameSpeed = 1.0f;
        private float _maxGameSpeed = 5.0f;
        private int _worldWidth = 4000;
        private int _worldHeight = 4000;

        public int WorldMinX { get => -_worldWidth / 2; }
        public int WorldMaxX { get => _worldWidth / 2; }
        public int WorldMinY { get => -_worldHeight / 2; }
        public int WorldMaxY { get => _worldHeight / 2; }


        private Camera _camera;
        private Entity _testEntity;

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


            TextureAtlas.Font = Content.Load<SpriteFont>("bin/DesktopGL/Energy");

            MyraEnvironment.Game = this;

            _desktop = new Desktop();


            MainUI mainUI = new MainUI();
            _desktop.Root = mainUI;

            mainUI._quit.Selected += (s, a) => Exit();
            mainUI._gameSpeedSlider.ValueChanged += (s, a) => {
                _gameSpeed = _maxGameSpeed * a.NewValue / 100;
               // mainUI._gameSpeedLabel.Text = $"Simulation Speed: {Math.Round(_gameSpeed, 2)}";
            };
             //mainUI._newScenario.Selected += (s, a) => _desktop.Root = new ScenarioUI();

            _grid = new AdjacencyGrid(_worldWidth, _worldHeight, 25);

            _main = new EntityPool("Main Pool");
            _renderSystem = new RenderSystem(_main);
            _transformHierarchySystem = new TransformHierarchySystem(_main, _grid);
            _velocitySystem = new VelocitySystem(_main);

            // Declare rigidbody collision system
            _rigidbodyCollisionSystem = new SortAndSweep(_main, 
                (e) => e.HasComponents(typeof(CircleColliderComponent), typeof(RigidBodyComponent)), 
                (e1, e2) => true); // All collisions should be resolved

            _mouthCollisionSystem = new SortAndSweep(_main,
                (e) => e.HasComponents(typeof(CircleColliderComponent)) 
                && (e.HasComponents(typeof(MouthComponent)) || e.HasComponents(typeof(EdibleComponent))),
                // Only resolve if 1 has a food and the other a mouth (but not both)
                (e1, e2) =>
                    (e1.HasComponent<MouthComponent>() && e2.HasComponent<EdibleComponent>())
                    ^ (e2.HasComponent<MouthComponent>() && e1.HasComponent<EdibleComponent>())
            );

            _weaponHealthCollisionSystem = new SortAndSweep(_main,
                (e) => e.HasComponents(typeof(CircleColliderComponent))
                && (e.HasComponent<HealthComponent>() || e.HasComponent<WeaponComponent>()),
                // Only resolve if 1 is a weapon and the other a health
                (e1, e2) =>
                    (e1.HasComponent<HealthComponent>() && e2.HasComponent<WeaponComponent>())
                    ^ (e2.HasComponent<HealthComponent>() && e1.HasComponent<WeaponComponent>())
            );



            _rigidBodySystem = new RigidBodySystem(_main);
            _dragSystem = new DragSystem(_main, 1.2f);
            _movementControlSystem = new MovementControlSystem(_main);
            _brainSystem = new BrainSystem(_main);
            _worldBorderSystem = new WorldBorderSystem(_main, _worldWidth, _worldHeight);
            _visionSystem = new VisionSystem(_main, _grid);
            _rigidBodyCollisionSystem = new RigidBodyCollisionSystem(_rigidbodyCollisionSystem.Collisions);
            _mouthFoodCollisionSystem = new MouthFoodCollisionSystem(_main, _mouthCollisionSystem.Collisions);
            _energyDeathSystem = new EnergyDeathSystem(_main);
            _infoRenderSystem = new InfoRenderSystem(_main);
            _noseSystem = new NoseSystem(_main, _grid);
            _healthDeathSystem = new HealthDeathSystem(_main, "Food");

            // Global energy manager. This Ensures a closed system so there is a fixed amount of enegry in the simulation
            _energyManager = new EnergyManager();
            // These are systems that take into account energy and need the energy manager system
            _weaponSystem = new WeaponSystem(_main, _weaponHealthCollisionSystem.Collisions, _energyManager);
            _movementControlEnergyCostSystem = new MovementControlEnergyCostSystem(_main, _energyManager);
            _foodRespawnSystem = new FoodRespawnSystem(_main, _energyManager, _grid, "Food", 2f); 


            var mutators = new List<AbstractMutator> {

                new AddConnectionMutator()
                {
                    AllowRecurrentConns= true,
                    NewWeightPower = 1.0f,
                    NewWeightStd = 1.0f,
                    ProbabiltiyOfMutation = 0.02f

                },
                new SplitConnectionMutator() {
                    ProbabiltiyOfMutation=0.02f,
                    
                },
                new ConnectionWeightMutator() {
                    ProbabiltiyOfMutation = 1f,
                    ProbPerturbWeight = 0.8f,
                    NewWeightStd = 1f,
                    ProbResetWeight=0.1f,
                    NewWeightPower = 1f,
                    ProbEnableConn =0.01f,
                    ProbDisableConn = 0.01f,
                    WeightPerturbScale = 0.4f
                },
            };
            MutationConfig mutationConfig = new MutationConfig() { Mutators = mutators };
            InnovationIdManager innovationIdManager= new InnovationIdManager(100, 100);
            //TODO remember max species ID
            _reproductionSystem = new ReproductionSystem(_main, mutationConfig, innovationIdManager, _energyManager, 0);




            Random rand = new Random();
            _main.AddDefinition("Critterling", Critter.GetCritter(TextureAtlas.Circle, Color.Blue));
            _main.AddDefinition("Grabber", Critter.GetGrabber(TextureAtlas.Circle, Color.Green));
            _main.AddDefinition("Food", FoodPellets.GetFoodPellet(Color.White));

            //_testEntity = _main.AddEntityFromDefinition("Critterling");
            //_testEntity.GetComponent<EnergyComponent>().Energy = 1000;
            //_testEntity.RemoveComponent<BrainComponent>();

            for (int i = 0; i < 150; i++)
            {
                var testEntity = _main.AddEntityFromDefinition("Critterling");

                testEntity.GetComponent<TransformComponent>().LocalPosition = new Vector2(rand.Next(WorldMinX, WorldMaxX), rand.Next(WorldMinY, WorldMaxY));

                NeatGenotype genotype = new NeatGenotype();
                var brainComponent = testEntity.GetComponent<BrainComponent>();
                genotype.CreateBrain(brainComponent);
                var phenotype = new NeatPhenotype(genotype);
                brainComponent.SetBrain(phenotype);
                brainComponent.SetUpLinks();
                // Temp!!
                // testEntity.RemoveComponent<BrainComponent>();
            }

            for (int i = 0; i < 400; i++)
            {
                var foodEntity = _main.AddEntityFromDefinition("Food");
                foodEntity.GetComponent<TransformComponent>().LocalPosition = new Vector2(rand.Next(WorldMinX, WorldMaxX), rand.Next(WorldMinY, WorldMaxY));
            }

            
            _camera = new Camera(Window);
        
        }
         

        private float _cameraMoveSpeed = 500;
        private float _cameraZoomSpeed = 5;

        protected override void Update(GameTime gameTime)
        {

            var kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.Z))
            {
                _camera.Zoom += _cameraZoomSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (kbState.IsKeyDown(Keys.X))
            {
                _camera.Zoom -= _cameraZoomSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kbState.IsKeyDown(Keys.Up))
            {
                _camera.Position += new Vector2(0, -1) * (float)gameTime.ElapsedGameTime.TotalSeconds * _cameraMoveSpeed;
            }
            else if (kbState.IsKeyDown(Keys.Down))
            {
                _camera.Position += new Vector2(0, 1) * (float)gameTime.ElapsedGameTime.TotalSeconds * _cameraMoveSpeed;
            }


            if (kbState.IsKeyDown(Keys.Left))
            {
                _camera.Position += new Vector2(-1, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * _cameraMoveSpeed;
            }
            else if (kbState.IsKeyDown(Keys.Right))
            {
                _camera.Position += new Vector2(1, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * _cameraMoveSpeed;
            }

            //var testMove = _testEntity.GetComponent<MovementControlComponent>();

            //testMove.WishForceForward = 0f;

            //if (kbState.IsKeyDown(Keys.W))
            //{
            //    testMove.WishForceForward = 1f;
            //}
            //else if (kbState.IsKeyDown(Keys.S))
            //{
            //    testMove.WishForceForward = -1f;
            //}

            //testMove.WishRotForce = 0f;

            //if (kbState.IsKeyDown(Keys.A))
            //{
            //    testMove.WishRotForce = -1f;
            //}
            //else if (kbState.IsKeyDown(Keys.D))
            //{
            //    testMove.WishRotForce = 1f;
            //}

            // Make sure all the dead things are removed in case they interfere with other systems
            _energyDeathSystem.Update();
            _healthDeathSystem.Update();

            // Update all systems that regard input values before updating brains
            _visionSystem.Update();
            _noseSystem.Update();

            _brainSystem.Update();
            // Following this update all systems that regard output
            _reproductionSystem.Update();
            _movementControlSystem.Update();
            // If these systems have energy costs remember to update those systems before anything else happens, in case we need to cancel it
            _movementControlEnergyCostSystem.Update(gameTime, _gameSpeed);

            // These systems gather collisions between certain types of entities that are processed by other systems
            _rigidbodyCollisionSystem.GetCollisions();
            _mouthCollisionSystem.GetCollisions();
            _weaponHealthCollisionSystem.GetCollisions();

            // Update the aforementioned systems to process the collisions
            _rigidBodyCollisionSystem.Update();
            _mouthFoodCollisionSystem.Update(gameTime, _gameSpeed);
            _weaponSystem.Update(gameTime, _gameSpeed);

            // Calculate forces acting upon each body
            _rigidBodySystem.Update(gameTime, _gameSpeed);
            _dragSystem.Update();

            // bounce entities on edge of the world
            _worldBorderSystem.Update(gameTime);
            // Actually modify transforms
            _velocitySystem.Update(gameTime, _gameSpeed);

            _foodRespawnSystem.Update(gameTime, _gameSpeed);
            //At the end of each loop update the hierarchy system so that it renders correctly and everything is ready for the next loop
            _transformHierarchySystem.Update();

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _worldBorderSystem.Draw(_spriteBatch, _camera);
#if DEBUG
            //_grid.DrawGrid(_spriteBatch, _camera);
#endif
            _renderSystem.Draw(_spriteBatch, _camera);
            _infoRenderSystem.Draw(_spriteBatch, _camera);

            _desktop.Render();

            base.Draw(gameTime);
        }
    }
}
