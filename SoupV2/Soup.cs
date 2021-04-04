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
            IsMouseVisible = true;
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
        private RigidBodySystem _rigidBodySystem;
        private DragSystem _dragSystem;
        private MovementControlSystem _movementControlSystem;
        private BrainSystem _brainSystem;
        private ReproductionSystem _reproductionSystem;
        private WorldBorderSystem _worldBorderSystem;
        private VisionSystem _visionSystem;
        private RigidBodyCollisionSystem _rigidBodyCollisionSystem;
        private MouthFoodCollisionSystem _mouthFoodCollisionSystem;
        private EnergyDeathSystem _edibleSystem;
        private InfoRenderSystem _infoRenderSystem;
        private MovementControlEnergyCostSystem _movementControlEnergyCostSystem;

        private AdjacencyGrid _grid;

        private float _gameSpeed = 1.0f;
        private float _maxGameSpeed = 5.0f;
        private int _worldWidth = 2000;
        private int _worldHeight = 2000;

        public int WorldMinX { get => -_worldWidth / 2; }
        public int WorldMaxX { get => _worldWidth / 2; }
        public int WorldMinY { get => -_worldHeight / 2; }
        public int WorldMaxY { get => _worldHeight / 2; }


        private Camera _camera;
        private Entity _testEntity;

        protected override void LoadContent()
        {
            base.LoadContent();
            base.LoadContent();

            SpriteBatchExtension.InitSpriteBatchExtension(GraphicsDevice);
            SerializerSettings.InitializeSettings(Content);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureAtlas.Circle = Content.Load<Texture2D>("bin/DesktopGL/circle");
            TextureAtlas.Eye = Content.Load<Texture2D>("bin/DesktopGL/eye");
            TextureAtlas.Mouth = Content.Load<Texture2D>("bin/DesktopGL/mouth");
            TextureAtlas.Soup = Content.Load<Texture2D>("bin/DesktopGL/soup");
            TextureAtlas.Font = Content.Load<SpriteFont>("bin/DesktopGL/Energy");

            MyraEnvironment.Game = this;

            _desktop = new Desktop();


            MainUI mainUI = new MainUI();
            _desktop.Root = mainUI;

            mainUI._quit.Selected += (s, a) => Exit();
            mainUI._gameSpeedSlider.ValueChanged += (s, a) => {
                _gameSpeed = _maxGameSpeed * a.NewValue / 100;
                mainUI._gameSpeedLabel.Text = $"Simulation Speed: {Math.Round(_gameSpeed, 2)}";
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

            _rigidBodySystem = new RigidBodySystem(_main);
            _dragSystem = new DragSystem(_main, 1.2f);
            _movementControlSystem = new MovementControlSystem(_main);
            _brainSystem = new BrainSystem(_main);
            _worldBorderSystem = new WorldBorderSystem(_main, _worldWidth, _worldHeight);
            _visionSystem = new VisionSystem(_main, _grid);
            _rigidBodyCollisionSystem = new RigidBodyCollisionSystem(_rigidbodyCollisionSystem.Collisions);
            _mouthFoodCollisionSystem = new MouthFoodCollisionSystem(_mouthCollisionSystem.Collisions);
            _edibleSystem = new EnergyDeathSystem(_main);
            _infoRenderSystem = new InfoRenderSystem(_main);
            _movementControlEnergyCostSystem = new MovementControlEnergyCostSystem(_main);

            var mutators = new List<AbstractMutator> {

                //new AddConnectionMutator()
                //{
                //    AllowRecurrentConns= true,
                //    NewWeightPower = 1.0f,
                //    NewWeightStd = 1.0f,
                //    ProbabiltiyOfMutation = 0.02f

                //},
                new SplitConnectionMutator() {
                    ProbabiltiyOfMutation=1f,
                },
                new ConnectionWeightMutator() {
                    ProbabiltiyOfMutation = 1f,
                    ProbPerturbWeight = 0.8f,
                    NewWeightStd = 1f,
                    ProbResetWeight=0.1f,
                    NewWeightPower = 4f,
                    ProbEnableConn =0.01f,
                    ProbDisableConn = 0.01f,
                    WeightPerturbScale = 1f
                },
            };
            MutationConfig mutationConfig = new MutationConfig() { Mutators = mutators };

            InnovationIdManager innovationIdManager= new InnovationIdManager(10, 10);
                
            _reproductionSystem = new ReproductionSystem(_main, mutationConfig, innovationIdManager);


            Random rand = new Random();
            _main.AddDefinition("Critterling", Critter.GetCritter(TextureAtlas.Circle, Color.Blue));
            _main.AddDefinition("Grabber", Critter.GetGrabber(TextureAtlas.Circle, Color.Green));
            _main.AddDefinition("Food", FoodPellets.GetFoodPellet(Color.White));

            _testEntity = _main.AddEntityFromDefinition("Grabber");
            _testEntity.AddComponents(new ColourComponent(_testEntity));

            for (int i = 0; i < 100; i++)
            {
                var testEntity = _main.AddEntityFromDefinition("Critterling");
                testEntity.GetComponent<TransformComponent>().LocalPosition = new Vector2(rand.Next(WorldMinX, WorldMaxX), rand.Next(WorldMinY, WorldMaxY));

                Genotype genotype = new Genotype();

                genotype.AddNamedNode("eye1", NodeType.INPUT, "tanh");
                genotype.AddNamedNode("eye2", NodeType.INPUT, "tanh");

                genotype.AddNamedNode("forwardback", NodeType.OUTPUT, "tanh");
                genotype.AddNamedNode("rotation", NodeType.OUTPUT, "tanh");

                genotype.ConnectionGenes.Add(new ConnectionGene(0, 0, 2, 1.0f));
                genotype.ConnectionGenes.Add(new ConnectionGene(1, 1, 2, -1.0f));

                genotype.ConnectionGenes.Add(new ConnectionGene(2, 0, 3, 1.0f));
                genotype.ConnectionGenes.Add(new ConnectionGene(3, 1, 3, -1.0f));

                var phenotype = new Phenotype(genotype);

                var brainComponent = testEntity.GetComponent<BrainComponent>();
                brainComponent.SetBrain(phenotype);
                brainComponent.SetUpLinks();
                // Temp!!
                // testEntity.RemoveComponent<BrainComponent>();
            }

            for (int i = 0; i < 1000; i++)
            {
                var testEntity = _main.AddEntityFromDefinition("Food");
                testEntity.GetComponent<TransformComponent>().LocalPosition = new Vector2(rand.Next(WorldMinX, WorldMaxX), rand.Next(WorldMinY, WorldMaxY));

                Genotype genotype = new Genotype();
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
            var testTrans = _testEntity.GetComponent<TransformComponent>();
            testTrans.LocalPosition = _camera.Position;

            _brainSystem.Update();
            _visionSystem.Update();
            _reproductionSystem.Update();
            _movementControlSystem.Update();
            _movementControlEnergyCostSystem.Update(gameTime, _gameSpeed);
            _rigidbodyCollisionSystem.GetCollisions();
            _mouthCollisionSystem.GetCollisions();
            _rigidBodyCollisionSystem.Update();
            _mouthFoodCollisionSystem.Update(gameTime, _gameSpeed);
            _dragSystem.Update();
            _rigidBodySystem.Update(gameTime, _gameSpeed);
            _worldBorderSystem.Update(gameTime);
            _velocitySystem.Update(gameTime, _gameSpeed);
            _edibleSystem.Update();
            //At the end of each loop update the hierarchy system so that it renders correctly
            _transformHierarchySystem.Update();

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _worldBorderSystem.Draw(_spriteBatch, _camera);
#if DEBUG
            _grid.DrawGrid(_spriteBatch, _camera);
#endif
            _renderSystem.Draw(_spriteBatch, _camera);
            _infoRenderSystem.Draw(_spriteBatch, _camera);

            _desktop.Render();

            base.Draw(gameTime);
        }
    }
}
