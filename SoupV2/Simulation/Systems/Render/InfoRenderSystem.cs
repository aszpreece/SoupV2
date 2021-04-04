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
    class InfoRenderSystem : EntitySystem
    {
        public InfoRenderSystem(EntityPool pool) : base(pool, (e) => e.HasComponent(typeof(TransformComponent)) && (e.HasComponent(typeof(EnergyComponent))))
        {

        }

        private readonly Vector2 _offset = new Vector2(0, 10);

        public void Draw(SpriteBatch spriteBatch, Camera _camera)
        {

            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, _camera.GetViewTransformationMatrix());

            for (int i = 0; i < Compatible.Count; i++)
            {
                var transform = Compatible[i].GetComponent<TransformComponent>();
                var energy = Compatible[i].GetComponent<EnergyComponent>();

                if (Compatible[i].State == EntityState.Active)
                {

                    var energyAmountString = $"{Math.Round((decimal)energy.Energy, decimals:1)}";
                    var pos = transform.WorldPosition - _offset;
                    pos -= TextureAtlas.Font.MeasureString(energyAmountString) / 2;

                    spriteBatch.DrawString(
                        TextureAtlas.Font, 
                        energyAmountString, 
                        pos, 
                        Color.Green
                    );
                }
            }

            spriteBatch.End();

        }
    }
}
