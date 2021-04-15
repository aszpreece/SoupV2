using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SoupV2.Simulation.Components
{
    public class GraphicsComponent
        : AbstractComponent
    {
        public GraphicsComponent(Entity owner): base(owner) { }

        public string TexturePath { get; set; }

        [Browsable(false)]
        [JsonIgnore]
        public Texture2D Texture { get; set; }
        public Color Color { get; set; } = Color.White;

        public float Multiplier { get; set; } = 1.0f;

        public void ApplyMultiplier(float amount)
        {
            Multiplier *= amount;
        } 

        public Point? Dimensions { get; set; } = null;


    }
}
