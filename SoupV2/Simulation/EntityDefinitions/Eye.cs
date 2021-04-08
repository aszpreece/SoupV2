using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoupV2.EntityComponentSystem;
using SoupV2.Simulation.Components;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.EntityDefinitions
{
    public static class Eye
    {

        public static EntityDefinition GetEye(Color color, float angle = 0, float fov=60, float distFromParent = 8)
        {
            float radius = 5f;

            var eyeEntity = new Entity("eye");

            var pos = distFromParent * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            var transform = new TransformComponent(eyeEntity)
            {
                LocalPosition = pos,
                LocalRotation = new Rotation(angle),
                LocalDepth = 0.1f,
                Scale = new Vector2(1, 1)
            };

            var graphics = new GraphicsComponent(eyeEntity)
            {
                Texture = TextureAtlas.Eye,
                Dimensions = new Point((int)(radius * 2), (int)(radius * 2)),
                Color = color
            };

            var eyeComp = new EyeComponent(eyeEntity)
            {
                EyeRange = 140,
                Fov = MathHelper.ToRadians(fov),
            };
            eyeEntity.AddComponents(transform, graphics, eyeComp);

            return eyeEntity.ToDefinition();

        }
    }
}
