using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.EntityComponentSystem;
using SoupV2.Simulation.Components;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.EntityDefinitions
{
    public static class Nose
    {

        public static EntityDefinition GetNose(Color color, float angle = 0, float distFromParent = 8)
        {
            float radius = 5f;

            var noseEntity = new Entity("nose");

            var pos = distFromParent * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            var transform = new TransformComponent(noseEntity)
            {
                LocalPosition = pos,
                LocalRotation = new Rotation(angle),
                LocalDepth = 0.095f,
                Scale = new Vector2(1, 1)
            };

            var graphics = new GraphicsComponent(noseEntity)
            {
                Texture = TextureAtlas.Nose,
                Dimensions = new Point((int)(radius * 2), (int)(radius * 2)),
                Color = color
            };

            var noseComp = new NoseComponent(noseEntity)
            {
                NoseRange = 30,
            };
            noseEntity.AddComponents(transform, graphics, noseComp);

            return noseEntity.ToDefinition();

        }
    }
}
