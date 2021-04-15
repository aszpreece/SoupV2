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

                rigidBody.ApplyForce(transform.WorldForward * movement.ForwardForce);
                rigidBody.ApplyTorque(movement.RotationForce * 10);
            }
        }
    }
}
