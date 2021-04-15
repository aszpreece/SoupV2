using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoupV2.Simulation.Components;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Systems
{
    public class WorldBorderSystem : EntitySystem
    {
        public int WorldWidth { get; private set; }
        public int WorldHeight { get; private set; }
        public int WorldWidthRadius { get; private set; }
        public int WorldHeightRadius { get; private set; }

        public WorldBorderSystem(EntityManager pool, int worldWidth, int worldHeight) : base(pool, (e) => e.HasComponents(typeof(VelocityComponent), typeof(TransformComponent)))
        {
            WorldWidth = worldWidth;
            WorldHeight = worldHeight;
            WorldWidthRadius = worldWidth / 2;
            WorldHeightRadius = worldHeight / 2;
        }

        public void Update()
        {
            for(int i = 0; i < Compatible.Count; i ++)
            {
                var transform = Compatible[i].GetComponent<TransformComponent>();
                var velocity = Compatible[i].GetComponent<VelocityComponent>();

                if(transform.WorldPosition.X < -WorldWidthRadius)
                {
                    transform.WorldPosition = new Vector2(-WorldWidthRadius, transform.WorldPosition.Y);
                    velocity.Velocity = new Vector2(-velocity.Velocity.X, velocity.Velocity.Y);
                }
                else if (transform.WorldPosition.X > WorldWidthRadius)
                {
                    transform.WorldPosition = new Vector2(WorldWidthRadius, transform.WorldPosition.Y);
                    velocity.Velocity = new Vector2(-velocity.Velocity.X, velocity.Velocity.Y);
                }
                if (transform.WorldPosition.Y < -WorldHeightRadius)
                {
                    transform.WorldPosition = new Vector2(transform.WorldPosition.X, -WorldHeightRadius);
                    velocity.Velocity = new Vector2(velocity.Velocity.X, -velocity.Velocity.Y);

                }
                else if (transform.WorldPosition.Y > WorldHeightRadius)
                {
                    transform.WorldPosition = new Vector2(transform.WorldPosition.X, WorldHeightRadius);
                    velocity.Velocity = new Vector2(velocity.Velocity.X, -velocity.Velocity.Y);
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch, Matrix camera)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, camera);
            spriteBatch.FillRectangle(new Rectangle(-WorldWidthRadius, -WorldHeightRadius, WorldWidth, WorldHeight), Color.Gray);
            spriteBatch.End();
        }

    }
    
}
