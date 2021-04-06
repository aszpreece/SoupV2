using EntityComponentSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class MouthComponent : AbstractComponent
    {
        public float EnergyPerSecond { get; set; } = 1.0f;

        public float Eating { get; set; } = 0.0f;
        public MouthComponent(Entity owner) : base(owner)
        {

        }
    }
}
