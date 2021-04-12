using EntityComponentSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class HealthComponent : AbstractComponent
    {
        private float _health;
        public float Health
        {
            get => _health;
            set
            {
                if (value >= 0)
                {
                    _health = value;
                }
            }
        }

        public float MaxHealth { get; set; }

        [Browsable(false)]
        public float HealthPercent { get => Health / MaxHealth; }

        public HealthComponent(Entity owner) : base(owner)
        {

        }
    }
}
