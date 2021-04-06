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
        public static EntityDefinition GetWeapon(Color color)
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
                Texture = TextureAtlas.BoxingGloveRetracted,
                Dimensions = new Point((int)(radius * 2), (int)(radius * 2)),
                Color = color
            };

            var collider = new CircleColliderComponent(weaponEntity)
            {
                Radius = radius,
            };

            var weaponComp = new WeaponComponent(weaponEntity)
            {
                ActivateThreshold = 0.9f,
                AttackCost = 0.2f,
                AttackTimeSeconds = 0.5f,
                CooldownTimeSeconds = 1,
                DamagePerSecond = 30f

            };

            weaponEntity.AddComponents(transform, graphics, weaponComp, collider);

            return weaponEntity.ToDefinition();

        }
    }
}
