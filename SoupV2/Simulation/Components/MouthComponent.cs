using EntityComponentSystem;
using SoupV2.Simulation.Brain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class MouthComponent : AbstractComponent
    {
        private float _energyPerSecond = 1.0f;
        public float EnergyPerSecond {
            get => _energyPerSecond;
            set
            {
                if (value >= 0)
                {
                    _energyPerSecond = value;
                }
            }
        }

        [Browsable(false)]
        [Input]
        public float Eating { get; set; } = 0.0f;
        public MouthComponent(Entity owner) : base(owner)
        {

        }
    }
}
