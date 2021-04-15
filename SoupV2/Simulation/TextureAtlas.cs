using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SoupV2.Simulation
{
    public static class TextureAtlas
    {

        private static Dictionary<string, Texture2D> _atlas = new Dictionary<string, Texture2D>();

        /// <summary>
        /// Given a path of an image, load a texture and cache it.
        /// If the image has already been loaded then return it
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Texture2D GetTexture(string path, GraphicsDevice graphicsDevice)
        {
            if (_atlas.ContainsKey(path))
            {
                return _atlas[path];
            }
            using (Stream stream = File.OpenRead(path)) { 

                Texture2D texture = Texture2D.FromStream(graphicsDevice, stream);
                _atlas[path] = texture;
                return texture;
            }

        }

        public static Texture2D Circle { get; set; }
        public static Texture2D Eye { get; set; }
        public static Texture2D Mouth { get; internal set; }
        public static Texture2D Soup { get; internal set; }
        public static Texture2D Nose { get; internal set; }
        public static Texture2D BoxingGloveRetracted { get; internal set; }
        public static Texture2D BoxingGloveExtended { get; internal set; }
        public static SpriteFont Font { get; internal set; }
        public static SpriteEffect OutlineEffect { get; set; }


        public static readonly string CirclePath  = "circle";
        public static readonly string EyePath  = "eye";
        public static readonly string MouthPath = "mouth";
        public static readonly string SoupPath = "soup";
        public static readonly string NosePath = "SoupNose";
        public static readonly string BoxingGloveRetractedPath = "BoxingGloveRetracted";
        public static readonly string BoxingGloveExtendedPath = "BoxingGloveExtended";

        /// <summary>
        /// Load built in sprites
        /// </summary>
        /// <param name="content"></param>
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
            _atlas.Add(CirclePath, TextureAtlas.Circle);
            _atlas.Add(EyePath, TextureAtlas.Eye);
            _atlas.Add(MouthPath, TextureAtlas.Mouth);
            _atlas.Add(SoupPath, TextureAtlas.Soup);
            _atlas.Add(NosePath, TextureAtlas.Nose);
            _atlas.Add(BoxingGloveExtendedPath, TextureAtlas.BoxingGloveExtended);
            _atlas.Add(BoxingGloveRetractedPath, TextureAtlas.BoxingGloveRetracted);

        }

    }
}
