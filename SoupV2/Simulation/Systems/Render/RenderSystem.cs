using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoupV2.Simulation.Components;
using System;

namespace SoupV2.Simulation.Systems
{
    public class RenderSystem : EntitySystem
    {
        Simulation _simulation;

        /// <summary>
        /// Editor mode constantly refreshes textures
        /// </summary>
        public bool EditorMode { get; set; } = false;
        public RenderSystem(EntityManager pool, Simulation simulation) : base(pool, (e) => e.HasComponents(typeof(TransformComponent), typeof(GraphicsComponent)))
        {
            _simulation = simulation;
        }

        public void Draw(SpriteBatch spriteBatch, Matrix camera)
        {

            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, camera);

            for (int i = 0; i < Compatible.Count; i++)
            {
                var transform = Compatible[i].GetComponent<TransformComponent>();
                var graphics = Compatible[i].GetComponent<GraphicsComponent>();

                Color filter = Color.White;
                VisibleColourComponent col;
                if (Compatible[i].TryGetComponent<VisibleColourComponent>(out col))
                {
                    filter = col.Colour * graphics.Multiplier;
                } else
                {
                    filter = graphics.Color * graphics.Multiplier;
                }
                if (Compatible[i].State == EntityState.Active)
                {
                    // hacky way to load textures....
                    // tried to do it in json convertor, but it wouldn't work for some reason (the custom reader would not trigger)
                    // should be past because only done once and null checks are fast.
                    if (graphics.Texture is null || EditorMode)
                    {

                        try
                        {
                            // may fail due to io
                            graphics.Texture = _simulation.TextureAtlas.GetTexture(graphics.TexturePath, spriteBatch.GraphicsDevice);
                        }
                        catch (Exception)
                        {
                            // set missing texture
                            graphics.Texture = _simulation.TextureAtlas.Missing;
                        }
                        
                    }
                    if (!(graphics.Dimensions is null)) {
                        var dest = new Rectangle((int)transform.WorldPosition.X, (int)transform.WorldPosition.Y, (int)(transform.Scale.X * graphics.Dimensions.Value.X), (int)(transform.Scale.Y * graphics.Dimensions.Value.Y));
                        spriteBatch.Draw(
                            graphics.Texture,
                            dest,
                            null,
                            filter,
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
                            filter,
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
