using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

public class TextureConverter : JsonConverter
{
    private ContentManager _content;
    public TextureConverter(ContentManager content)
    {
        _content = content;
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
        return _content.Load<Texture2D>(path);
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