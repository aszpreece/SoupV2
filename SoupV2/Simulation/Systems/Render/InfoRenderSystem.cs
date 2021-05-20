using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoupV2.Simulation.Components;
using System;

namespace SoupV2.Simulation.Systems
{
    public class InfoRenderSystem : EntitySystem
    {
        Simulation _simulation;

        private readonly Vector2 _energyOffset = new Vector2(0, 10);
        private readonly Vector2 _healthOffset = new Vector2(0, 22);
        private readonly Vector2 _ageOffset = new Vector2(0, 34);


        public InfoRenderSystem(EntityManager pool, Simulation simulation) : base(pool, (e) => e.HasComponent(typeof(TransformComponent)) && (e.HasComponent<EnergyComponent>() || e.HasComponent<HealthComponent>() || e.HasComponent<OldAgeComponent>()))
        {
            _simulation = simulation;
        }

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
                    pos -= _simulation.TextureAtlas.Font.MeasureString(energyAmountString) / 2;

                    spriteBatch.DrawString(
                        _simulation.TextureAtlas.Font, 
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
                    pos -= _simulation.TextureAtlas.Font.MeasureString(healthAmountString) / 2;

                    spriteBatch.DrawString(
                        _simulation.TextureAtlas.Font,
                        healthAmountString,
                        pos,
                        Color.Red
                    );
                }

                if (Compatible[i].HasComponent<OldAgeComponent>())
                {


                    var transform = Compatible[i].GetComponent<TransformComponent>();
                    var age = Compatible[i].GetComponent<OldAgeComponent>();

                    var ageAmountString = $"{Math.Round((decimal)age.CurrentAge, decimals: 1)}";
                    var pos = transform.WorldPosition - _ageOffset;
                    pos -= _simulation.TextureAtlas.Font.MeasureString(ageAmountString) / 2;

                    spriteBatch.DrawString(
                        _simulation.TextureAtlas.Font,
                        ageAmountString,
                        pos,
                        Color.Purple
                    );
                }
            }

            spriteBatch.End();

        }
    }
}
