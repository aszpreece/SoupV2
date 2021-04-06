using EntityComponentSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class HealthComponent : AbstractComponent
    {
        public float Health { get; set; }

        public float MaxHealth { get; set; }
        public float HealthPercent { get => Health / MaxHealth; }

        public HealthComponent(Entity owner) : base(owner)
        {

        }
    }
}
