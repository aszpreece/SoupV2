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
    class VelocitySystem : EntitySystem
    {
        public VelocitySystem(EntityPool pool) : base(pool, typeof(TransformComponent), typeof(VelocityComponent))
        {

        }

        public void Update(GameTime gameTime, float gameSpeed)
        {
            for (int i = 0; i < Compatible.Count; i++)
            {
                var transform = Compatible[i].GetComponent<TransformComponent>();
                var velocity = Compatible[i].GetComponent<VelocityComponent>();

                transform.LocalPosition += velocity.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * gameSpeed;
                transform.LocalRotation += velocity.RoationalVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds * gameSpeed;

            }


        }

    }
}
