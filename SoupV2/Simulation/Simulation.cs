using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using SoupV2.EntityComponentSystem;
using SoupV2.NEAT;
using SoupV2.NEAT.mutation;
using SoupV2.Simulation.Brain;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.EntityDefinitions;
using SoupV2.Simulation.Grid;
using SoupV2.Simulation.Physics;
using SoupV2.Simulation.Settings;
using SoupV2.Simulation.Statistics;
using SoupV2.Simulation.Statistics.StatLoggers;
using SoupV2.Simulation.Systems;
using SoupV2.Simulation.Systems.Abilities;
using SoupV2.Simulation.Systems.Energy;
using SoupV2.Simulation.Systems.WorldLogic;
using SoupV2.Systems;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SoupV2.Simulation
{
    public class Simulation
    {

        /// <summary>
        /// Protected 'boring' systems. These do not expose any interesting fields.
        /// </summary>
        protected EntityManager _main;
        protected RenderSystem _renderSystem;
        protected TransformHierarchySystem _transformHierarchySystem;
        protected VelocitySystem _velocitySystem;
        protected SortAndSweep _rigidbodyCollisionSystem;
        protected SortAndSweep _mouthCollisionSystem;
        protected SortAndSweep _weaponHealthCollisionSystem;
        protected MovementControlSystem _movementControlSystem;
        protected DragSystem _dragSystem;
        protected BrainSystem _brainSystem;
        protected VisionSystem _visionSystem;
        protected RigidBodySystem _rigidBodySystem;
        protected NoseSystem _noseSystem;
        protected MovementControlEnergyCostSystem _movementControlEnergyCostSystem;
        protected EnergyManager _energyManager;


        /// <summary>
        /// Systems that are public expose events that can be subscribed to by interested third parties.
        /// </summary>
        public ReproductionSystem ReproductionSystem { get; internal set; }
        public WorldBorderSystem WorldBorderSystem { get; internal set;  }
        public RigidBodyCollisionSystem RigidbodyCollisionSystem { get; internal set; }
        public MouthFoodCollisionSystem MouthFoodCollisionSystem { get; internal set; }
        public FoodRespawnSystem FoodRespawnSystem { get; internal set; }
        public CritterPopulationSystem CritterPopulationSystem { get; private set; }
        public EnergyDeathSystem EnergyDeathSystem { get; internal set; }
        public HealthDeathSystem HealthDeathSystem { get; internal set; }
        public OldAgeDeathSystem OldAgeDeathSystem { get; internal set; }

        public WeaponSystem WeaponSystem { get; internal set; }

        public SpeciesSystem SpeciesSystem { get; set; }

        /// <summary>
        /// Expose info system to enable render toggling.
        /// </summary>
        public InfoRenderSystem InfoRenderSystem { get; set; }


        protected AdjacencyGrid _grid;

        public float GameSpeed { get; set; } = 1 / 30f;
        public uint Tick { get; set; } = 0;

        private int _worldWidth = 4000;
        private int _worldHeight = 4000;

        public int WorldMinX { get => -_worldWidth / 2; }
        public int WorldMaxX { get => _worldWidth / 2; }
        public int WorldMinY { get => -_worldHeight / 2; }
        public int WorldMaxY { get => _worldHeight / 2; }
        public JsonSerializerSettings JsonSettings { get; set; }

        SimulationSettings _settings;


        private Random _random = new Random();

        /// <summary>
        /// Constructor for creating a simulation.
        /// Must call SetUp to set up simulation.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="entityDefinitionDatabase"></param>
        /// <param name="graphicsDevice"></param>
        public Simulation(SimulationSettings settings, EntityDefinitionDatabase entityDefinitionDatabase)
        {
            _settings = settings;
            _main = new EntityManager("Main Pool", entityDefinitionDatabase);
            Initialize();
        }

        private void Initialize()
        {

            _worldWidth = _settings.WorldWidth;
            _worldHeight = _settings.WorldHeight;

            _grid = new AdjacencyGrid(_settings.WorldWidth, _settings.WorldHeight, 25);

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
            _dragSystem = new DragSystem(_main, _settings.MassDensity);
            _movementControlSystem = new MovementControlSystem(_main);
            _brainSystem = new BrainSystem(_main);
            WorldBorderSystem = new WorldBorderSystem(_main, _worldWidth, _worldHeight);
            _visionSystem = new VisionSystem(_main, _grid);

            // These systems take a reference to a list of collisions. This means that should collision code checking change, the objects do not.
            RigidbodyCollisionSystem = new RigidBodyCollisionSystem(_rigidbodyCollisionSystem.Collisions);
            MouthFoodCollisionSystem = new MouthFoodCollisionSystem(_main, _mouthCollisionSystem.Collisions);
            InfoRenderSystem = new InfoRenderSystem(_main);
            _noseSystem = new NoseSystem(_main, _grid);

            // Global energy manager. This Ensures a closed system so there is a fixed amount of enegry in the simulation
            _energyManager = new EnergyManager(_settings.InitialWorldEnergy);

            // These are systems that take into account energy and need the energy manager system

            EnergyDeathSystem = new EnergyDeathSystem(_main);
            HealthDeathSystem = new HealthDeathSystem(_main, this, _energyManager);
            OldAgeDeathSystem = new OldAgeDeathSystem(_main, this, _energyManager, _settings.OldAgeEnabled, _settings.OldAgeMultiplier);

            WeaponSystem = new WeaponSystem(_main, _weaponHealthCollisionSystem.Collisions, _energyManager);
            _movementControlEnergyCostSystem = new MovementControlEnergyCostSystem(_main, _energyManager);
            FoodRespawnSystem = new FoodRespawnSystem(_main, _energyManager, _grid, _settings.FoodTypes, this);
            CritterPopulationSystem = new CritterPopulationSystem(_main, _energyManager, _grid, _settings.CritterTypes, this);

            InnovationIdManager innovationIdManager = new InnovationIdManager(100, 100);
            //TODO remember max species ID
            ReproductionSystem = new ReproductionSystem(_main, _settings.MutationConfig, innovationIdManager, _energyManager, 0, _settings.SpeciesCompatabilityThreshold, this);
            SpeciesSystem = new SpeciesSystem(_main, this, _settings.SpeciesCompatabilityThreshold);
        }

        /// <summary>
        /// Sets up the simulation with all the provided critters and food etc.
        /// </summary>
        public virtual void SetUp(GraphicsDevice graphicsDevice)
        {
            JsonSettings = SerializerSettings.GetDefaultSettings(graphicsDevice);

            SpeciesSystem.InitializeSpecies(_settings.CritterTypes);



            // Initialize critters
            foreach (CritterTypeSetting critterTypeSetting in _settings.CritterTypes)
            {
                InitialSpawnRandom(critterTypeSetting);
            }

            // Initialize food
            foreach (FoodTypeSetting foodTypeSetting in _settings.FoodTypes)
            {
                InitialSpawnRandom(foodTypeSetting);
            }

            _brainSystem.SetUpBrains(_settings.CritterTypes);
        }

        /// <summary>
        /// Creates the initial population of a critter type.
        /// </summary>
        /// <param name="setting"></param>
        private void InitialSpawnRandom(AbstractEntityTypeSetting setting)
        {
            for (int i = 0; i < setting.InitialCount; i++)
            {
                var entity = _main.AddEntityFromDefinition(setting.DefinitionId, JsonSettings);
                entity.Tag = setting.DefinitionId;
                entity.GetComponent<TransformComponent>().LocalPosition = new Vector2(_random.Next(WorldMinX, WorldMaxX), _random.Next(WorldMinY, WorldMaxY));
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            // Increment tick
            Tick++;

            //Ignore gametime. We are only bothered about ticks.

            // Make sure all the dead things are removed in case they interfere with other systems
            EnergyDeathSystem.Update(Tick, GameSpeed);
            HealthDeathSystem.Update(Tick, GameSpeed);
            OldAgeDeathSystem.Update(Tick, GameSpeed);

            // Update all systems that regard input values before updating brains
            _visionSystem.Update();
            _noseSystem.Update();

            _brainSystem.Update(_settings.CritterTypes);
            // Following this update all systems that regard output
            ReproductionSystem.Update(Tick, GameSpeed);
            _movementControlSystem.Update();
            // If these systems have energy costs remember to update those systems before anything else happens, in case we need to cancel it
            _movementControlEnergyCostSystem.Update(GameSpeed);

            // These systems gather collisions between certain types of entities that are processed by other systems
            _rigidbodyCollisionSystem.GetCollisions();
            _mouthCollisionSystem.GetCollisions();
            _weaponHealthCollisionSystem.GetCollisions();

            // Update the aforementioned systems to process the collisions
            RigidbodyCollisionSystem.Update();
            MouthFoodCollisionSystem.Update(Tick, GameSpeed);
            WeaponSystem.Update(Tick, GameSpeed);

            // Calculate forces acting upon each body
            _rigidBodySystem.Update(GameSpeed);
            _dragSystem.Update();

            // bounce entities on edge of the world
            WorldBorderSystem.Update();
            // Actually modify transforms
            _velocitySystem.Update(GameSpeed);

            CritterPopulationSystem.Update(Tick, GameSpeed);
            FoodRespawnSystem.Update(Tick, GameSpeed);

            //At the end of each loop update the hierarchy system so that it renders correctly and everything is ready for the next loop
            _transformHierarchySystem.Update();
            
    
        }


        public virtual void Draw(SpriteBatch spriteBatch, Matrix camera)
        {
            WorldBorderSystem.Draw(spriteBatch, camera);
#if DEBUG
            //_grid.DrawGrid(_spriteBatch, _camera);
#endif
            _renderSystem.Draw(spriteBatch, camera);
            InfoRenderSystem.Draw(spriteBatch, camera);
        }
    }
}
