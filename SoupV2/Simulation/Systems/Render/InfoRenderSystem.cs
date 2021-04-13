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
    public class InfoRenderSystem : EntitySystem
    {
        public InfoRenderSystem(EntityPool pool) : base(pool, (e) => e.HasComponent(typeof(TransformComponent)) && (e.HasComponent<EnergyComponent>() || e.HasComponent<HealthComponent>() || e.HasComponent<WeaponComponent>()))
        {

        }

        private readonly Vector2 _energyOffset = new Vector2(0, 10);
        private readonly Vector2 _healthOffset = new Vector2(0, 22);


        public void Draw(SpriteBatch spriteBatch, Matrix camera)
        {

            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, camera);

            for (int i = 0; i < Compatible.Count; i++)
            {
                if (Compatible[i].HasComponent<EnergyComponent>())
                {
  
                    var transform = Compatible[i].GetComponent<TransformComponent>();
                    var energy = Compatible[i].GetComponent<EnergyComponent>();


                    var energyAmountString = $"{Math.Round((decimal)energy.Energy, decimals:1)}";
                    var pos = transform.WorldPosition - _energyOffset;
                    pos -= TextureAtlas.Font.MeasureString(energyAmountString) / 2;

                    spriteBatch.DrawString(
                        TextureAtlas.Font, 
                        energyAmountString, 
                        pos, 
                        Color.Green
                    );
                }
                if (Compatible[i].HasComponent<HealthComponent>())
                {

                    var transform = Compatible[i].GetComponent<TransformComponent>();
                    var health = Compatible[i].GetComponent<HealthComponent>();


                    var healthAmountString = $"{Math.Round((decimal)health.Health, decimals: 1)}";
                    var pos = transform.WorldPosition - _healthOffset;
                    pos -= TextureAtlas.Font.MeasureString(healthAmountString) / 2;

                    spriteBatch.DrawString(
                        TextureAtlas.Font,
                        healthAmountString,
                        pos,
                        Color.Red
                    );
                }

                if (Compatible[i].HasComponent<WeaponComponent>())
                {

                    var transform = Compatible[i].GetComponent<TransformComponent>();
                    var weapon = Compatible[i].GetComponent<WeaponComponent>();

                    spriteBatch.DrawString(
                        TextureAtlas.Font,
                        weapon.Active.ToString(),
                        transform.WorldPosition,
                        Color.Red
                    );
                }
            }

            spriteBatch.End();

        }
    }
}
