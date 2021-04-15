using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoupV2.Simulation.Systems
{
    public class VelocitySystem : EntitySystem
    {
        public VelocitySystem(EntityManager pool) : base(pool, (e) => e.HasComponents(typeof(TransformComponent), typeof(VelocityComponent)))
        {

        }

        public void Update(float gameSpeed)
        {
            for (int i = 0; i < Compatible.Count; i++)
            {
                var transform = Compatible[i].GetComponent<TransformComponent>();
                var velocity = Compatible[i].GetComponent<VelocityComponent>();

                transform.LocalPosition += velocity.Velocity * gameSpeed;
                transform.LocalRotation += velocity.RotationalVelocity * gameSpeed;

            }


        }

    }
}
