using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SoupV2.EntityComponentSystem;
using SoupV2.NEAT.mutation;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Grid;
using SoupV2.Simulation.Physics;
using SoupV2.Simulation.Settings;
using SoupV2.Simulation.Systems;
using SoupV2.Simulation.Systems.Abilities;
using SoupV2.Simulation.Systems.Energy;
using SoupV2.Simulation.Systems.WorldLogic;
using System;

namespace SoupV2.Simulation
{
    /// <summary>
    /// Extensionc lass for editor
    /// </summary>
    public class EntityEditorSimulation : Simulation
    {

        public EntityEditorSimulation(SimulationSettings settings, EntityDefinitionDatabase entityDefinitionDatabase) : base(settings, entityDefinitionDatabase)
        {

        }

        protected override void Initialize()
        {
         
            _grid = new AdjacencyGrid(_settings.WorldWidth, _settings.WorldHeight, 25);

            _renderSystem = new RenderSystem(EntityManager, this);
            _transformHierarchySystem = new TransformHierarchySystem(EntityManager, _grid);
            _velocitySystem = new VelocitySystem(EntityManager);

            // Declare rigidbody collision system
            _rigidbodyCollisionSystem = new SortAndSweep(EntityManager,
                (e) => e.HasComponents(typeof(CircleColliderComponent), typeof(RigidBodyComponent)),
                (e1, e2) => true); // All collisions should be resolved

            _mouthCollisionSystem = new SortAndSweep(EntityManager,
                (e) => e.HasComponents(typeof(CircleColliderComponent))
                && (e.HasComponents(typeof(MouthComponent)) || e.HasComponents(typeof(EdibleComponent))),
                // Only resolve if 1 has a food and the other a mouth (but not both)
                (e1, e2) =>
                    (e1.HasComponent<MouthComponent>() && e2.HasComponent<EdibleComponent>())
                    ^ (e2.HasComponent<MouthComponent>() && e1.HasComponent<EdibleComponent>())
            );

            _weaponHealthCollisionSystem = new SortAndSweep(EntityManager,
                (e) => e.HasComponents(typeof(CircleColliderComponent))
                && (e.HasComponent<HealthComponent>() || e.HasComponent<WeaponComponent>()),
                // Only resolve if 1 is a weapon and the other a health
                (e1, e2) =>
                    (e1.HasComponent<HealthComponent>() && e2.HasComponent<WeaponComponent>())
                    ^ (e2.HasComponent<HealthComponent>() && e1.HasComponent<WeaponComponent>())
            );

            _rigidBodySystem = new RigidBodySystem(EntityManager);
            _dragSystem = new DragSystem(EntityManager, _settings.MassDensity);
            _movementControlSystem = new MovementControlSystem(EntityManager);
            _brainSystem = new BrainSystem(EntityManager, Settings);
            WorldBorderSystem = new WorldBorderSystem(EntityManager, _worldWidth, _worldHeight);
            _visionSystem = new VisionSystem(EntityManager, _grid);

            // These systems take a reference to a list of collisions. This means that should collision code checking change, the objects do not.
            RigidbodyCollisionSystem = new RigidBodyCollisionSystem(_rigidbodyCollisionSystem.Collisions);
            MouthFoodCollisionSystem = new MouthFoodCollisionSystem(EntityManager, _mouthCollisionSystem.Collisions);
            InfoRenderSystem = new InfoRenderSystem(EntityManager, this);
            _noseSystem = new NoseSystem(EntityManager, _grid);

            // Global energy manager. This Ensures a closed system so there is a fixed amount of enegry in the simulation
            _energyManager = new EnergyManager(_settings.InitialWorldEnergy);

            // These are systems that take into account energy and need the energy manager system

            EnergyDeathSystem = new EnergyDeathSystem(EntityManager);
            HealthDeathSystem = new HealthDeathSystem(EntityManager, this, _energyManager);
            OldAgeDeathSystem = new OldAgeDeathSystem(EntityManager, this, _energyManager, _settings.OldAgeEnabled, _settings.OldAgeMultiplier);

            WeaponSystem = new WeaponSystem(EntityManager, _weaponHealthCollisionSystem.Collisions, _energyManager);
            _movementControlEnergyCostSystem = new MovementControlEnergyCostSystem(EntityManager, _energyManager);

            InnovationIdManager innovationIdManager = new InnovationIdManager(100, 100);

            // So textures are constantly refreshed
            _renderSystem.EditorMode = true;

        }

        public override void Update(GameTime gameTime)
        {
            // Increment tick
            Tick++;

            //Ignore gametime. We are only bothered about ticks.

            // No dying in simulation
            //EnergyDeathSystem.Update(Tick, GameSpeed);
            //HealthDeathSystem.Update(Tick, GameSpeed);
            //OldAgeDeathSystem.Update(Tick, GameSpeed);

            // Update all systems that regard input values before updating brains
            _visionSystem.Update();
            _noseSystem.Update();

            //_brainSystem.Update(_settings.CritterTypes);
            // Following this update all systems that regard output
            _movementControlSystem.Update();
            // If these systems have energy costs remember to update those systems before anything else happens, in case we need to cancel it
            _movementControlEnergyCostSystem.Update(SimDeltaTime);

            // These systems gather collisions between certain types of entities that are processed by other systems
            _rigidbodyCollisionSystem.GetCollisions();
            _mouthCollisionSystem.GetCollisions();
            _weaponHealthCollisionSystem.GetCollisions();

            // Update the aforementioned systems to process the collisions
            RigidbodyCollisionSystem.Update();
            MouthFoodCollisionSystem.Update(Tick, SimDeltaTime);
            WeaponSystem.Update(Tick, SimDeltaTime);

            // Calculate forces acting upon each body
            _rigidBodySystem.Update(SimDeltaTime);
            _dragSystem.Update();

            // bounce entities on edge of the world
            WorldBorderSystem.Update();
            // Actually modify transforms
            _velocitySystem.Update(SimDeltaTime);

            // Di not want this for editor
            //CritterPopulationSystem.Update(Tick, GameSpeed);
            //FoodRespawnSystem.Update(Tick, GameSpeed);

            //At the end of each loop update the hierarchy system so that it renders correctly and everything is ready for the next loop
            _transformHierarchySystem.Update();


        }


        public override void Draw(SpriteBatch spriteBatch, Matrix camera)
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
