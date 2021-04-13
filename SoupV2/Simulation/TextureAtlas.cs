using Microsoft.Xna.Framework.Content;
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

        public static void Load(ContentManager content)
        {
            TextureAtlas.Font = content.Load<SpriteFont>("bin/DesktopGL/Energy");
            TextureAtlas.Circle = content.Load<Texture2D>("bin/DesktopGL/circle");
            TextureAtlas.Eye = content.Load<Texture2D>("bin/DesktopGL/eye");
            TextureAtlas.Mouth = content.Load<Texture2D>("bin/DesktopGL/mouth");
            TextureAtlas.Soup = content.Load<Texture2D>("bin/DesktopGL/soup");
            TextureAtlas.Nose = content.Load<Texture2D>("bin/DesktopGL/SoupNose");
            TextureAtlas.BoxingGloveExtended = content.Load<Texture2D>("bin/DesktopGL/BoxingGloveExtended");
            TextureAtlas.BoxingGloveRetracted = content.Load<Texture2D>("bin/DesktopGL/BoxingGloveRetracted");
        }

    }
}
