using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    internal class GraphicsComponent
        : IComponent
    {
        public Texture2D Texture { get; set; }
        public Color Color { get; internal set; } = Color.White;

        public Point? Dimensions { get; set; } = null;


    }
}
