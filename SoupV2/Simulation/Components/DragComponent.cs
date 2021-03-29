using EntityComponentSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class DragComponent : IComponent
    {
        public float DragCoefficient { get; set; } = 1.0f;
    }
}
