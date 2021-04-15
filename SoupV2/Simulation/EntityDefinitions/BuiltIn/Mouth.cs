using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.EntityComponentSystem;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Physics;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.EntityDefinitions
{
    public static class Mouth
    {
        public static Entity GetMouth(Color color)
        {
            float radius = 10f;

            var mouthEntity = new Entity("mouth");

            var transform = new TransformComponent(mouthEntity)
            {
                LocalPosition = Vector2.Zero,
                LocalRotation = new Rotation(0),
                LocalDepth = 0.09f,
                Scale = new Vector2(1, 1)
            };

            var graphics = new GraphicsComponent(mouthEntity)
            {
                TexturePath = TextureAtlas.MouthPath,
                Dimensions = new Point((int)(radius * 2), (int)(radius * 2)),
                Color = color
            };

            var mouthComp = new MouthComponent(mouthEntity)
            {

            };

            var collider = new CircleColliderComponent(mouthEntity)
            {
                Radius = radius,
            };

            mouthEntity.AddComponents(transform, graphics, mouthComp, collider);

            return mouthEntity;

        }
    }
}
