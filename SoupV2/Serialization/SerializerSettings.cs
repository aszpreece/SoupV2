using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.EntityComponentSystem
{
    public static class SerializerSettings
    {
        public static JsonSerializerSettings GetDefaultSettings(GraphicsDevice device)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            settings.Converters.Add(new Vector2Converter());
            settings.Converters.Add(new Texture2DConverter(device));
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            return settings;
        }
    }
}
