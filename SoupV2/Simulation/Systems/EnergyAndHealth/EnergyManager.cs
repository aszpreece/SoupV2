using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation
{
    class EnergyManager
    {
        public float Energy { get; private set; }

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
            }
            else
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
