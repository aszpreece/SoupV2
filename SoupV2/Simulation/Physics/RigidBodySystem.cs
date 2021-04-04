using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Physics
{
    class RigidBodySystem : EntitySystem
    {
        public RigidBodySystem(EntityPool pool) : base(pool, (e) => e.HasComponents(typeof(RigidBodyComponent), typeof(VelocityComponent), typeof(CircleColliderComponent)))
        {

        }
        public void Update(GameTime gameTime, float gameSpeed)
        {
            

            for (int i = 0; i < Compatible.Count; i++)
            {
                var rigidbody = Compatible[i].GetComponent<RigidBodyComponent>();
                var velocity = Compatible[i].GetComponent<VelocityComponent>();
                var collider = Compatible[i].GetComponent<CircleColliderComponent>();

                velocity.Velocity += rigidbody.Impulse * rigidbody.InvMass;

                //f = ma
                //a = f*1/m
                velocity.Velocity += rigidbody.Force * rigidbody.InvMass * (float)gameTime.ElapsedGameTime.TotalSeconds * gameSpeed;

                //Angular acceleration = torque(N/m)/Moment of Inertia(Kg/m^2)
                // Moment of inertia = 1/2(mass)(radius^2)

                var momentOfInertia = 0.5f * collider.Radius * collider.Radius * rigidbody.Mass;

                var moment = rigidbody.Torque / momentOfInertia * (float)gameTime.ElapsedGameTime.TotalSeconds * gameSpeed;

                velocity.RotationalVelocity += moment;


                rigidbody.Reset();
            }
        }
    }
}
