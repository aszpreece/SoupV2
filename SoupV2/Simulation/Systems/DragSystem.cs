using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SoupV2.Simulation.Systems
{
    class DragSystem : EntitySystem
    {
        private float _massDensity;
        public DragSystem(EntityPool pool, float massDensity) : base(pool, typeof(RigidBodyComponent), typeof(DragComponent), typeof(VelocityComponent))
        {
            _massDensity = massDensity;
        }

        public void Update()
        {
            for (int i = 0; i < Compatible.Count; i++)
            {
                var entity = Compatible[i];
                var dragComponent = entity.GetComponent<DragComponent>();
                var rigidBody = entity.GetComponent<RigidBodyComponent>();
                var velocity = entity.GetComponent<VelocityComponent>();

                var dir = velocity.Velocity;
                if (dir == Vector2.Zero)
                {
                    continue;
                }
                dir.Normalize();


                var dragForce = 0.5f * dragComponent.DragCoefficient * dir * velocity.Velocity.LengthSquared() * _massDensity;
                rigidBody.ApplyForce(-dragForce);
            }
        }
    }
}
