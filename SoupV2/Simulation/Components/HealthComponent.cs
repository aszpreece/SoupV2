using EntityComponentSystem;
using SoupV2.Simulation.Brain;
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
                _health = value;
            }
        }

        public float MaxHealth { get; set; }

        [Browsable(false)]
        [Input]
        public float HealthPercent { get => Health / MaxHealth; }

        /// <summary>
        /// cause critter to die
        /// </summary>
        [Control]
        public float Suicude { get; set; } = 0.0f;

        public HealthComponent(Entity owner) : base(owner)
        {

        }
    }
}
