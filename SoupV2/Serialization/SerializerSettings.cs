using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.EntityComponentSystem
{
    public static class SerializerSettings
    {

        public static void InitializeSettings(ContentManager manager) {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            settings.Converters.Add(new Vector2Converter());
            settings.Converters.Add(new TextureConverter(manager));
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;

            DefaultSettings = settings;
        }

        public static JsonSerializerSettings DefaultSettings { get; set; }





    }
}
