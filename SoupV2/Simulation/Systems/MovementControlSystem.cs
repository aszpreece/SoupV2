using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SoupV2.Simulation.Systems
{
    public class MovementControlSystem : EntitySystem 
    {
        public MovementControlSystem(EntityManager pool) : base(pool, (e) => e.HasComponents(typeof(MovementControlComponent), typeof(RigidBodyComponent)))
        {

        }

        public void Update()
        {
            for (int i = 0; i < Compatible.Count; i++)
            {
                var entity = Compatible[i];
                var movement = entity.GetComponent<MovementControlComponent>();
                var rigidBody = entity.GetComponent<RigidBodyComponent>();
                var transform = entity.GetComponent<TransformComponent>();

                // Two different movement modes. Imagine one as a tank with two treads that can be controlled independantly.
                // The other is like a car, with forward and backwards motion and a steering wheel.
                if (movement.MovementMode == MovementMode.MoveAndSteer)
                {
                    rigidBody.ApplyForce(transform.WorldForward * movement.ForwardForce);
                    rigidBody.ApplyTorque(movement.RotationForce * 2);
                } else if(movement.MovementMode == MovementMode.TwoWheels)
                {
                    // Apply forces for the two wheels.
                    // right wheel causes negative rotation
                    rigidBody.ApplyForce(transform.WorldForward * movement.LeftWheelForce);
                    rigidBody.ApplyTorque(-movement.LeftWheelForce * 2);

                    rigidBody.ApplyForce(transform.WorldForward * movement.RightWheelForce);
                    rigidBody.ApplyTorque(movement.RightWheelForce * 2);
                }
            }
        }
    }
}
