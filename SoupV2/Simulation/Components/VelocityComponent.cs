using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class VelocityComponent : AbstractComponent
    {

        public VelocityComponent(Entity owner) : base(owner)
        {

        }

        [JsonIgnore]
        public Vector2 Velocity { get; set; } = Vector2.Zero;

        [JsonIgnore]
        public float RotationalVelocity { get; set; } = 0.0f;
    }
}
