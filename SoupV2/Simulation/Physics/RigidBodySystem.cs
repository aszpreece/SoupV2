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
        public RigidBodySystem(EntityPool pool) : base(pool, typeof(RigidBodyComponent), typeof(VelocityComponent))
        {

        }
        public void Update(GameTime gameTime, float gameSpeed)
        {

            for (int i = 0; i < Compatible.Count; i++)
            {
                var rigidbody = Compatible[i].GetComponent<RigidBodyComponent>();
                var velocity = Compatible[i].GetComponent<VelocityComponent>();

                velocity.Velocity += rigidbody.Impulse * rigidbody.InvMass;

                //f=ma
                //a = f*1/m
                velocity.Velocity += rigidbody.Force * rigidbody.InvMass * (float)gameTime.ElapsedGameTime.TotalSeconds * gameSpeed;

                rigidbody.Reset();
            }
        }
    }
}
