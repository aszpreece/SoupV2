using EntityComponentSystem;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Physics
{
    public class Collision
    {
        public Entity E1 { get; set; }
        public Entity E2 { get; set; }
        public Vector2 Normal { get; internal set; }
    }
}
