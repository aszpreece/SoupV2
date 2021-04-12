using EntityComponentSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    public class EnergyComponent : AbstractComponent 
    {
        private float _energy;
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

    }
}
