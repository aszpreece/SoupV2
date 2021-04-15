using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    public class EnergyComponent : AbstractComponent 
    {
        private float _energy;

        /// <summary>
        /// Determines whether or not this component will drop food upon death.
        /// </summary>
        public bool DropAsFoodOnDeath { get; set; } = true;

        /// <summary>
        /// The definition of the food to drop upon death.
        /// </summary>
        public string DropAsFoodEntityDefinition { get; set; } = "DefaultFood";
        public float Energy { get => _energy;
            set
            {
                if (value < 0)
                {
                    _energy = 0;
                } else
                {
                    _energy = value;
                }
            }
        }

        public EnergyComponent(Entity owner) : base(owner)
        {

        }
        public bool EnergyAvailable(float amount)
        {
            return Energy > amount;
        }

        /// <summary>
        /// Attempt to charge the given amount of energy and return the amount charged.
        /// If the amount of charged is more than the amount of energy the component possesses,
        /// Then take what we can down to 0.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public float ChargeEnergy(float amount)
        {
            if (amount <= Energy)
            {
                Energy -= amount;
                return amount;
            } else
            {
                float left = Energy;
                Energy = 0;
                return left;
            }
        }

        public void DepositEnergy(float amount)
        {
            Energy += amount;
        }

        public bool CanAfford(float amount)
        {
            return Energy >= amount;
        }

        /// <summary>
        /// Handles the death of an entity that has energy
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="pool"></param>
        public void HandleDeath(EnergyManager energyManager, EntityManager entityManager, JsonSerializerSettings jsonSettings, Vector2 position)
        {
            // If the component is set to drop as food on death drop a food
            if (DropAsFoodOnDeath)
            {
                var food = entityManager.AddEntityFromDefinition(DropAsFoodEntityDefinition, jsonSettings);
                // There is a small chance the dropped food component does not have an energy component. If not return the energy to the manager after all
                if (food.HasComponent<EnergyComponent>())
                {
                    food.GetComponent<EnergyComponent>().Energy = Energy;
                    food.GetComponent<TransformComponent>().LocalPosition = position;
                    return;
                }
            }
            energyManager.DepositEnergy(Energy);
        }

    }
}
