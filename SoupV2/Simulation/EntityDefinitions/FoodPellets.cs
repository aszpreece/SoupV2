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
    public static class FoodPellets
    {
        public static EntityDefinition GetFoodPellet(Color color)
        {
            float radius = 10f;

            var foodEntity = new Entity("food");

            var transform = new TransformComponent(foodEntity)
            {
                LocalPosition = Vector2.Zero,
                LocalRotation = new Rotation(0),
                LocalDepth = 0.09f,
                Scale = new Vector2(1, 1)
            };

            var graphics = new GraphicsComponent(foodEntity)
            {
                Texture = TextureAtlas.Soup,
                Dimensions = new Point((int)(radius * 2), (int)(radius * 2)),
                Color = color
            };

            var colourComp = new VisibleColourComponent(foodEntity)
            {
                RealR = 0.9f,
                RealB = 0.05f,
                RealG = 0.05f,
            
            };

            var energy = new EnergyComponent(foodEntity)
            {
                Energy = 3
            };

            var edible = new EdibleComponent(foodEntity)
            {

            };


            var collider = new CircleColliderComponent(foodEntity)
            {
                Radius = radius,
            };

            foodEntity.AddComponents(transform, graphics, energy, edible, collider, colourComp);

            return foodEntity.ToDefinition();

        }
    }
}
