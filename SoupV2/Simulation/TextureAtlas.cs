using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SoupV2.Simulation
{
    public class TextureAtlas
    {

        private Dictionary<string, Texture2D> _atlas = new Dictionary<string, Texture2D>();

        /// <summary>
        /// Given a path of an image, load a texture and cache it.
        /// If the image has already been loaded then return it
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Texture2D GetTexture(string path, GraphicsDevice graphicsDevice)
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

        public bool HasTexture(string path)
        {
            return _atlas.ContainsKey(path);
        }

        public Texture2D Circle { get; set; }
        public Texture2D Eye { get; set; }
        public Texture2D Mouth { get; internal set; }
        public Texture2D Soup { get; internal set; }
        public Texture2D Nose { get; internal set; }
        public Texture2D BoxingGloveRetracted { get; internal set; }
        public Texture2D BoxingGloveExtended { get; internal set; }
        public Texture2D Missing { get; internal set; }

        public SpriteFont Font { get; internal set; }
        public SpriteEffect OutlineEffect { get; set; }


        public static readonly string CirclePath  = "circle";
        public static readonly string EyePath  = "eye";
        public static readonly string MouthPath = "mouth";
        public static readonly string SoupPath = "soup";
        public static readonly string NosePath = "SoupNose";
        public static readonly string BoxingGloveRetractedPath = "BoxingGloveRetracted";
        public static readonly string BoxingGloveExtendedPath = "BoxingGloveExtended";
        public static readonly string MissingPath = "Missing";


        /// <summary>
        /// Load built in sprites
        /// </summary>
        /// <param name="content"></param>
        public TextureAtlas(ContentManager content)
        {    
            Font = content.Load<SpriteFont>("bin/DesktopGL/Energy");
            Circle = content.Load<Texture2D>("bin/DesktopGL/circle");
            Eye = content.Load<Texture2D>("bin/DesktopGL/eye");
            Mouth = content.Load<Texture2D>("bin/DesktopGL/mouth");
            Soup = content.Load<Texture2D>("bin/DesktopGL/soup");
            Nose = content.Load<Texture2D>("bin/DesktopGL/SoupNose");
            BoxingGloveExtended = content.Load<Texture2D>("bin/DesktopGL/BoxingGloveExtended");
            BoxingGloveRetracted = content.Load<Texture2D>("bin/DesktopGL/BoxingGloveRetracted");
            Missing = content.Load<Texture2D>("bin/DesktopGL/Missing");
            _atlas.Add(CirclePath, Circle);
            _atlas.Add(EyePath, Eye);
            _atlas.Add(MouthPath, Mouth);
            _atlas.Add(SoupPath, Soup);
            _atlas.Add(NosePath, Nose);
            _atlas.Add(BoxingGloveExtendedPath, BoxingGloveExtended);
            _atlas.Add(BoxingGloveRetractedPath, BoxingGloveRetracted);
            _atlas.Add(MissingPath, Missing);
        }

    }
}
