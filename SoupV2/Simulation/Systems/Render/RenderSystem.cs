using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoupV2.Simulation.Components;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SoupV2.Systems
{
    internal class RenderSystem : EntitySystem
    {
        public RenderSystem(EntityPool pool) : base(pool, (e) => e.HasComponents(typeof(TransformComponent), typeof(GraphicsComponent)))
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, Camera _camera)
        {

            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, _camera.GetViewTransformationMatrix());

            for (int i = 0; i < Compatible.Count; i++)
            {
                var transform = Compatible[i].GetComponent<TransformComponent>();
                var graphics = Compatible[i].GetComponent<GraphicsComponent>();
                
                if (Compatible[i].State == EntityState.Active)
                {

                    if (!(graphics.Dimensions is null)) {
                        var dest = new Rectangle((int)transform.WorldPosition.X, (int)transform.WorldPosition.Y, (int)(transform.Scale.X * graphics.Dimensions.Value.X), (int)(transform.Scale.Y * graphics.Dimensions.Value.Y));
                        spriteBatch.Draw(
                            graphics.Texture,
                            dest,
                            null,
                            graphics.Color * graphics.Multiplier,
                            (float)transform.WorldRotation.Theta,
                            graphics.Texture.Bounds.Center.ToVector2(),
                            SpriteEffects.None,
                            transform.WorldDepth);
                    } else
                    {
                        spriteBatch.Draw(
                            graphics.Texture,
                            transform.WorldPosition,
                            null, 
                            graphics.Color * graphics.Multiplier,
                            (float)transform.WorldRotation.Theta,
                            graphics.Texture.Bounds.Center.ToVector2(),
                            transform.Scale,
                            SpriteEffects.None,
                            transform.WorldDepth);
                    }

                }
            }

            spriteBatch.End();

        }

    }
}
