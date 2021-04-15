using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoupV2.Simulation;
using System;
using System.Linq;

public class Texture2DConverter : JsonConverter
{
    private GraphicsDevice _graphicsDevice;
    public Texture2DConverter(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
    }

    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(Texture2D));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);
        var properties = jsonObject.Properties().ToList();
        var path = (string)properties[0].Value;

        return TextureAtlas.GetTexture(path, _graphicsDevice);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Texture2D t = (Texture2D)value;
        writer.WriteStartObject();
        writer.WritePropertyName("Texture2D");
        serializer.Serialize(writer, t.Name);
        writer.WriteEndObject();
    }
}