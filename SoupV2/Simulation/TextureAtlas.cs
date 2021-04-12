using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation
{
    public static class TextureAtlas
    {
        public static Texture2D Circle { get; set; }
        public static Texture2D Eye { get; set; }
        public static Texture2D Mouth { get; internal set; }
        public static Texture2D Soup { get; internal set; }
        public static Texture2D Nose { get; internal set; }
        public static Texture2D BoxingGloveRetracted { get; internal set; }
        public static Texture2D BoxingGloveExtended { get; internal set; }
        public static SpriteFont Font { get; internal set; }
        public static SpriteEffect OutlineEffect { get; set; }
    }
}
