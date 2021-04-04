using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Systems.Energy
{
    /// <summary>
    /// Charges entities with energy and a movement control system for the movement they make
    /// </summary>
    class MovementControlEnergyCostSystem : EntitySystem
    {
        public float CostPerNewtonPerSecond { get; set; } = 0.1f;
        public MovementControlEnergyCostSystem(EntityPool pool) : base(pool, (e) => e.HasComponents(typeof(EnergyComponent), typeof(MovementControlComponent)))
        {

        }

        public void Update(GameTime gameTime, float gameSpeed)
        {
            for (int i = 0; i < Compatible.Count; i++)
            {
                var movementEntity = Compatible[i];
                var energy = movementEntity.GetComponent<EnergyComponent>();
                var movement = movementEntity.GetComponent<MovementControlComponent>();

                float extertedMove = (float)(Math.Abs(movement.ForwardForce) * CostPerNewtonPerSecond * gameTime.ElapsedGameTime.TotalSeconds);

                if (!energy.CanAfford(extertedMove))
                {
                    movement.WishForceForward = 0;
                } else
                {
                    energy.ChargeEnergy(extertedMove);
                }

                float extertedRotate = (float)(Math.Abs(movement.RotationForce) * CostPerNewtonPerSecond * gameTime.ElapsedGameTime.TotalSeconds);

                if (!energy.CanAfford(extertedRotate))
                {
                    movement.WishRotForce = 0;
                } else
                {
                    energy.ChargeEnergy(extertedRotate);
                }


            }
        }
    }
}
