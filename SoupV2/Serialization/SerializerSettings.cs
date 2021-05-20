using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SoupV2.Simulation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.EntityComponentSystem
{
    public static class SerializerSettings
    {
        public static JsonSerializerSettings GetDefaultSettings(GraphicsDevice device, TextureAtlas textureAtlas)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            settings.Converters.Add(new Vector2Converter());
            settings.Converters.Add(new Texture2DConverter(device, textureAtlas));
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            return settings;
        }
    }
}
