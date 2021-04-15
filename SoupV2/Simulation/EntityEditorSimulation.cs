using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation
{
    public class EntityEditorSimulation : Simulation
    {
        private Entity _editTarget;

        public Entity Selected { get; set; }

        public EntityManager Manager { get => _main; }
        public Entity EditTarget {
            get => _editTarget;  
            set {
                if (value is null)
                {
                    return;
                }
                _main.WipeEntities();
                _editTarget = value;
                _main.AddDeserializedEntity(_editTarget, null);
            } 
        }
        public bool Pause { get; set; } = true;

        public EntityEditorSimulation(GameWindow window, SimulationSettings settings) : base(settings, null)
        {
            //_camera.Zoom = 1f;
    
        }

        public override void Update(GameTime gameTime)
        {
            _transformHierarchySystem.Update();

            if (Pause)
            {
                return;
            }


            if (EditTarget.HasComponent<TransformComponent>())
            {
                //_camera.Position = EditTarget.GetComponent<TransformComponent>().WorldPosition;
            }
            var kbState = Keyboard.GetState();

            EditTarget.TryGetComponent<MovementControlComponent>(out MovementControlComponent movement);
            // Controls for entity
            if (!(movement is null))
            {
                movement.WishForceForward = 0f;

                if (kbState.IsKeyDown(Keys.W))
                {
                    movement.WishForceForward = 1f;
                }
                else if (kbState.IsKeyDown(Keys.S))
                {
                    movement.WishForceForward = -1f;
                }

                movement.WishRotForce = 0f;

                if (kbState.IsKeyDown(Keys.A))
                {
                    movement.WishRotForce = -1f;
                }
                else if (kbState.IsKeyDown(Keys.D))
                {
                    movement.WishRotForce = 1f;
                }
            }

            //EnergyDeathSystem.Update();
            //HealthDeathSystem.Update();

            //// Update all systems that regard input values before updating brains
            //_visionSystem.Update();
            //_noseSystem.Update();

            //_brainSystem.Update();
            //// Following this update all systems that regard output
            //ReproductionSystem.Update();

            //_movementControlSystem.Update();
            //// If these systems have energy costs remember to update those systems before anything else happens, in case we need to cancel it
            //_movementControlEnergyCostSystem.Update(1);

            //// These systems gather collisions between certain types of entities that are processed by other systems
            //_rigidbodyCollisionSystem.GetCollisions();
            //_mouthCollisionSystem.GetCollisions();
            //_weaponHealthCollisionSystem.GetCollisions();

            //// Update the aforementioned systems to process the collisions
            //RigidbodyCollisionSystem.Update();
            //MouthFoodCollisionSystem.Update(1);
            //WeaponSystem.Update(1);

            //// Calculate forces acting upon each body
            //_rigidBodySystem.Update(1);
            //_dragSystem.Update();

            //// bounce entities on edge of the world
            //WorldBorderSystem.Update();
            //// Actually modify transforms
            //_velocitySystem.Update(1);

            //FoodRespawnSystem.Update(1);
            ////At the end of each loop update the hierarchy system so that it renders correctly and everything is ready for the next loop
            //_transformHierarchySystem.Update();
        }
    }
}
