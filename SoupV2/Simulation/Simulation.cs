using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoupV2.NEAT;
using SoupV2.NEAT.mutation;
using SoupV2.Simulation.Brain;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.EntityDefinitions;
using SoupV2.Simulation.Grid;
using SoupV2.Simulation.Physics;
using SoupV2.Simulation.Statistics;
using SoupV2.Simulation.Statistics.StatLoggers;
using SoupV2.Simulation.Systems;
using SoupV2.Simulation.Systems.Abilities;
using SoupV2.Simulation.Systems.Energy;
using SoupV2.Systems;
using SoupV2.util;
using System;
using System.Threading;

namespace SoupV2.Simulation
{
    public class Simulation
    {

        protected EntityPool _main;
        protected RenderSystem _renderSystem;
        protected TransformHierarchySystem _transformHierarchySystem;
        protected VelocitySystem _velocitySystem;

        protected SortAndSweep _rigidbodyCollisionSystem;
        protected SortAndSweep _mouthCollisionSystem;
        protected SortAndSweep _weaponHealthCollisionSystem;
        protected RigidBodySystem _rigidBodySystem;
        protected DragSystem _dragSystem;
        protected MovementControlSystem _movementControlSystem;
        protected BrainSystem _brainSystem;
        protected ReproductionSystem _reproductionSystem;
        protected WorldBorderSystem _worldBorderSystem;
        protected VisionSystem _visionSystem;
        protected RigidBodyCollisionSystem _rigidBodyCollisionSystem;
        protected MouthFoodCollisionSystem _mouthFoodCollisionSystem;
        protected EnergyDeathSystem _energyDeathSystem;
        protected InfoRenderSystem _infoRenderSystem;
        protected FoodRespawnSystem _foodRespawnSystem;
        protected MovementControlEnergyCostSystem _movementControlEnergyCostSystem;
        protected EnergyManager _energyManager;
        protected NoseSystem _noseSystem;
        protected HealthDeathSystem _healthDeathSystem;
        protected WeaponSystem _weaponSystem;
        protected AdjacencyGrid _grid;

        private float _gameSpeed = 1/30f;

        private int _worldWidth = 4000;
        private int _worldHeight = 4000;

        public int WorldMinX { get => -_worldWidth / 2; }
        public int WorldMaxX { get => _worldWidth / 2; }
        public int WorldMinY { get => -_worldHeight / 2; }
        public int WorldMaxY { get => _worldHeight / 2; }

        SimulationSettings _settings;
        
        public Simulation(SimulationSettings settings)
        {

            _settings = settings;
            _grid = new AdjacencyGrid(_settings.WorldWidth, _settings.WorldHeight, 25);

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
            _dragSystem = new DragSystem(_main, _settings.MassDensity);
            _movementControlSystem = new MovementControlSystem(_main);
            _brainSystem = new BrainSystem(_main);
            _worldBorderSystem = new WorldBorderSystem(_main, _worldWidth, _worldHeight);
            _visionSystem = new VisionSystem(_main, _grid);
            _rigidBodyCollisionSystem = new RigidBodyCollisionSystem(_rigidbodyCollisionSystem.Collisions);
            _mouthFoodCollisionSystem = new MouthFoodCollisionSystem(_main, _mouthCollisionSystem.Collisions);
            _energyDeathSystem = new EnergyDeathSystem(_main);
            _infoRenderSystem = new InfoRenderSystem(_main);
            _noseSystem = new NoseSystem(_main, _grid);
            _healthDeathSystem = new HealthDeathSystem(_main, _settings.FoodObjectName);

            // Global energy manager. This Ensures a closed system so there is a fixed amount of enegry in the simulation
            _energyManager = new EnergyManager();
            // These are systems that take into account energy and need the energy manager system
            _weaponSystem = new WeaponSystem(_main, _weaponHealthCollisionSystem.Collisions, _energyManager);
            _movementControlEnergyCostSystem = new MovementControlEnergyCostSystem(_main, _energyManager);
            _foodRespawnSystem = new FoodRespawnSystem(_main, _energyManager, _grid, _settings.FoodObjectName, 2f);

            InnovationIdManager innovationIdManager = new InnovationIdManager(100, 100);
            //TODO remember max species ID
            //_reproductionSystem = new ReproductionSystem(_main, _settings.MutationConfig, innovationIdManager, _energyManager, 0, _settings.SpeciesCompatabilityThreshold);

        }

        public virtual void Initialize()
        {

            Random rand = new Random();
            _main.AddDefinition("Critterling", Critter.GetCritter(TextureAtlas.Circle, Color.Blue));
            _main.AddDefinition("Grabber", Critter.GetGrabber(TextureAtlas.Circle, Color.Green));
            _main.AddDefinition("Food", FoodPellets.GetFoodPellet(Color.White));

            Species originSpecies = new Species();
            for (int i = 0; i < 300; i++)
            {
                var testEntity = _main.AddEntityFromDefinition("Critterling");

                testEntity.GetComponent<TransformComponent>().LocalPosition = new Vector2(rand.Next(WorldMinX, WorldMaxX), rand.Next(WorldMinY, WorldMaxY));

                NeatGenotype genotype = new NeatGenotype();
                genotype.Species = originSpecies;
                if (i == 0)
                {
                    originSpecies.Representative = genotype;
                }

                var brainComponent = testEntity.GetComponent<BrainComponent>();
                genotype.CreateBrain(brainComponent);
                var phenotype = new NeatPhenotype(genotype);
                brainComponent.SetBrain(phenotype);
                brainComponent.SetUpLinks();
                // Temp!!
                // testEntity.RemoveComponent<BrainComponent>();
            }

            for (int i = 0; i < 150; i++)
            {
                var foodEntity = _main.AddEntityFromDefinition("Food");
                foodEntity.GetComponent<TransformComponent>().LocalPosition = new Vector2(rand.Next(WorldMinX, WorldMaxX), rand.Next(WorldMinY, WorldMaxY));
            }

            //_statisticsGatherer = new StatisticsGatherer();
            //if (_statDestination == StatLogDestination.FILE)
            //{
            //    _statLogger = new FileStatLogger(_statisticsGatherer, _statFile);
            //}
            //_statisticsHandlerThread = new Thread(() =>
            //{
            //    _statLogger?.LogStats();
            //});
            //_statisticsHandlerThread.Start();
            // set up event hooks.
            //_reproductionSystem.BirthEvent += _statisticsGatherer.HandleInfo;
            //_weaponSystem.OnAttack += _statisticsGatherer.HandleInfo;
            //_healthDeathSystem.OnDeath += _statisticsGatherer.HandleInfo;
            //_energyDeathSystem.OnDeath += _statisticsGatherer.HandleInfo;
            // _rigidbodyCollisionSystem.OnCollision += _statisticsGatherer.HandleInfo;
        }

        //public void Stop()
        //{
        //    _statisticsHandlerThread.Interrupt();
        //    _statisticsHandlerThread.Join();
        //}

        public virtual void Update(GameTime gameTime)
        {
            //Ignore gametime. We are only bothered about ticks.

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
            _movementControlEnergyCostSystem.Update(_gameSpeed);

            // These systems gather collisions between certain types of entities that are processed by other systems
            _rigidbodyCollisionSystem.GetCollisions();
            _mouthCollisionSystem.GetCollisions();
            _weaponHealthCollisionSystem.GetCollisions();

            // Update the aforementioned systems to process the collisions
            _rigidBodyCollisionSystem.Update();
            _mouthFoodCollisionSystem.Update(_gameSpeed);
            _weaponSystem.Update(_gameSpeed);

            // Calculate forces acting upon each body
            _rigidBodySystem.Update(_gameSpeed);
            _dragSystem.Update();

            // bounce entities on edge of the world
            _worldBorderSystem.Update();
            // Actually modify transforms
            _velocitySystem.Update(_gameSpeed);

            _foodRespawnSystem.Update(_gameSpeed);
            //At the end of each loop update the hierarchy system so that it renders correctly and everything is ready for the next loop
            _transformHierarchySystem.Update();
        }


        public virtual void Draw(SpriteBatch spriteBatch, Matrix camera)
        {
            _worldBorderSystem.Draw(spriteBatch, camera);
#if DEBUG
            //_grid.DrawGrid(_spriteBatch, _camera);
#endif
            _renderSystem.Draw(spriteBatch, camera);
            _infoRenderSystem.Draw(spriteBatch, camera);
        }
    }
}
