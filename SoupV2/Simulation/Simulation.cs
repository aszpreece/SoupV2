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

        /// <summary>
        /// Protected 'boring' systems. These do not expose any interesting fields.
        /// </summary>
        protected EntityPool _main;
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
        public ReproductionSystem ReproductionSystem { get; }
        public WorldBorderSystem WorldBorderSystem { get; }
        public RigidBodyCollisionSystem RigidbodyCollisionSystem { get; }
        public MouthFoodCollisionSystem MouthFoodCollisionSystem { get; }
        public FoodRespawnSystem FoodRespawnSystem { get; }
        public EnergyDeathSystem EnergyDeathSystem { get; }
        public HealthDeathSystem HealthDeathSystem { get; }
        public WeaponSystem WeaponSystem { get; }
        
        /// <summary>
        /// Expose info system to enable render toggling.
        /// </summary>
        public InfoRenderSystem InfoRenderSystem { get; set; }


        protected AdjacencyGrid _grid;

        private float _gameSpeed = 1/30f;

        private int _worldWidth = 4000;
        private int _worldHeight = 4000;

        public int WorldMinX { get => -_worldWidth / 2; }
        public int WorldMaxX { get => _worldWidth / 2; }
        public int WorldMinY { get => -_worldHeight / 2; }
        public int WorldMaxY { get => _worldHeight / 2; }

        SimulationSettings _settings;

        private uint _tick = 0;
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
            WorldBorderSystem = new WorldBorderSystem(_main, _worldWidth, _worldHeight);
            _visionSystem = new VisionSystem(_main, _grid);

            // These systems take a reference to a list of collisions. This means that should collision code checking change, the objects do not.
            RigidbodyCollisionSystem = new RigidBodyCollisionSystem(_rigidbodyCollisionSystem.Collisions);
            MouthFoodCollisionSystem = new MouthFoodCollisionSystem(_main, _mouthCollisionSystem.Collisions);
            InfoRenderSystem = new InfoRenderSystem(_main);
            _noseSystem = new NoseSystem(_main, _grid);
            
            
            EnergyDeathSystem = new EnergyDeathSystem(_main);
            HealthDeathSystem = new HealthDeathSystem(_main, _settings.FoodObjectName);

            // Global energy manager. This Ensures a closed system so there is a fixed amount of enegry in the simulation
            _energyManager = new EnergyManager();
            // These are systems that take into account energy and need the energy manager system
            WeaponSystem = new WeaponSystem(_main, _weaponHealthCollisionSystem.Collisions, _energyManager);
            _movementControlEnergyCostSystem = new MovementControlEnergyCostSystem(_main, _energyManager);
            FoodRespawnSystem = new FoodRespawnSystem(_main, _energyManager, _grid, _settings.FoodObjectName, 2f);

            InnovationIdManager innovationIdManager = new InnovationIdManager(100, 100);
            //TODO remember max species ID
            ReproductionSystem = new ReproductionSystem(_main, _settings.MutationConfig, innovationIdManager, _energyManager, 0, _settings.SpeciesCompatabilityThreshold);

        }

        public virtual void Initialize()
        {

            Random rand = new Random();
            // Temp
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

        }

        public virtual void Update(GameTime gameTime)
        {
            // Increment tick
            _tick++;

            //Ignore gametime. We are only bothered about ticks.

            // Make sure all the dead things are removed in case they interfere with other systems
            EnergyDeathSystem.Update(_tick);
            HealthDeathSystem.Update(_tick);

            // Update all systems that regard input values before updating brains
            _visionSystem.Update();
            _noseSystem.Update();

            _brainSystem.Update();
            // Following this update all systems that regard output
            ReproductionSystem.Update(_tick);
            _movementControlSystem.Update();
            // If these systems have energy costs remember to update those systems before anything else happens, in case we need to cancel it
            _movementControlEnergyCostSystem.Update(_gameSpeed);

            // These systems gather collisions between certain types of entities that are processed by other systems
            _rigidbodyCollisionSystem.GetCollisions();
            _mouthCollisionSystem.GetCollisions();
            _weaponHealthCollisionSystem.GetCollisions();

            // Update the aforementioned systems to process the collisions
            RigidbodyCollisionSystem.Update();
            MouthFoodCollisionSystem.Update(_tick, _gameSpeed);
            WeaponSystem.Update(_tick, _gameSpeed);

            // Calculate forces acting upon each body
            _rigidBodySystem.Update(_gameSpeed);
            _dragSystem.Update();

            // bounce entities on edge of the world
            WorldBorderSystem.Update();
            // Actually modify transforms
            _velocitySystem.Update(_gameSpeed);

            FoodRespawnSystem.Update(_tick, _gameSpeed);
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
