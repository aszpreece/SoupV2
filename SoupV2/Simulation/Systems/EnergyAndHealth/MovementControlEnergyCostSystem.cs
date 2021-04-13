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
    public class MovementControlEnergyCostSystem : EntitySystem
    {
        public float CostPerNewtonPerSecond { get; set; } = 0.0025f;
        private EnergyManager _energyManager;
        public MovementControlEnergyCostSystem(EntityPool pool, EnergyManager energyManager) : base(pool, (e) => e.HasComponents(typeof(EnergyComponent), typeof(MovementControlComponent)))
        {
            _energyManager = energyManager;
        }

        public void Update(float gameSpeed)
        {
            for (int i = 0; i < Compatible.Count; i++)
            {
                var movementEntity = Compatible[i];
                var energy = movementEntity.GetComponent<EnergyComponent>();
                var movement = movementEntity.GetComponent<MovementControlComponent>();

                float extertedMove = (float)(Math.Abs(movement.ForwardForce) * CostPerNewtonPerSecond * gameSpeed);
                
                //Figure out if we can afford the energy for both movement and rotation
                if (!energy.CanAfford(extertedMove))
                {
                    movement.WishForceForward = 0;

                }

                // Deposit any used energy into the energy manager
                _energyManager.DepositEnergy(energy.ChargeEnergy(extertedMove));
                

                float extertedRotate = (float)(Math.Abs(movement.RotationForce) * CostPerNewtonPerSecond * gameSpeed);

                if (!energy.CanAfford(extertedRotate))
                {
                    movement.WishRotForce = 0;
                }
                _energyManager.DepositEnergy(energy.ChargeEnergy(extertedRotate));
               


            }
        }
    }
}
