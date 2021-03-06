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
    public static class Weapon
    {
        public static Entity GetWeapon(Color color)
        {
            float radius = 8f;

            var weaponEntity = new Entity("weapon");

            var transform = new TransformComponent(weaponEntity)
            {
                LocalPosition = Vector2.Zero,
                LocalRotation = new Rotation(0),
                LocalDepth = 0.09f,
                Scale = new Vector2(1, 1)
            };

            var graphics = new GraphicsComponent(weaponEntity)
            {
                TexturePath = TextureAtlas.BoxingGloveRetractedPath,
                Dimensions = new Point((int)(radius * 2), (int)(radius * 2)),
                Color = color
            };

            var collider = new CircleColliderComponent(weaponEntity)
            {
                Radius = radius,
            };

            var weaponComp = new WeaponComponent(weaponEntity)
            {
                ActivateThreshold = 0.75f,
                AttackCost = 0.1f,
                AttackTimeSeconds = 0.1f,
                CooldownTimeSeconds = 1.2f,
                Damage = 34f

            };

            weaponEntity.AddComponents(transform, graphics, weaponComp, collider);

            return weaponEntity;

        }
    }
}
