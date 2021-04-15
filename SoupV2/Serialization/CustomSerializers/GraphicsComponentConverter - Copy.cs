using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoupV2.Simulation;
using SoupV2.Simulation.Components;
using System;
using System.Linq;

public class GraphicsComponentConverter : JsonConverter<GraphicsComponent>
{
    private GraphicsDevice _graphicsDevice;
    public GraphicsComponentConverter(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
    }


    public override GraphicsComponent ReadJson(JsonReader reader, Type objectType, GraphicsComponent existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);

        string texturePath = jsonObject["TexturePath"].ToObject<string>();
        var texture = TextureAtlas.GetTexture(texturePath, _graphicsDevice);

        //Entity owner = jsonObject["Owner"].ToObject<Entity>();
        GraphicsComponent graphicsComponent = new GraphicsComponent(null)
        {
            Color = jsonObject["Color"].ToObject<Color>(),
            Dimensions = jsonObject["Dimensions"].ToObject<Point>(),
            Multiplier = jsonObject["Multiplier"].ToObject<float>(),
            Texture = texture,
            TexturePath = texturePath,
        };

        return graphicsComponent;
    }

    public override void WriteJson(JsonWriter writer, GraphicsComponent value, JsonSerializer serializer)
    {
        GraphicsComponent graphicsComponent = (GraphicsComponent)value;
        writer.WriteStartObject();
        //writer.WritePropertyName("Owner");
        //serializer.Serialize(writer, graphicsComponent.Owner);
        writer.WritePropertyName("$type");
        serializer.Serialize(writer, "SoupV2.Simulation.Components.GraphicsComponent, SoupV2");
        writer.WritePropertyName("TexturePath");
        serializer.Serialize(writer, graphicsComponent.TexturePath);
        writer.WritePropertyName("Color");
        serializer.Serialize(writer, graphicsComponent.Color);
        writer.WritePropertyName("Multiplier");
        serializer.Serialize(writer, graphicsComponent.Multiplier);
        writer.WriteEndObject();
    }
}