using EntityComponentSystem;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class VelocityComponent : IComponent
    {
        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public float RoationalVelocity { get; set; } = 0.0f;
    }
}
