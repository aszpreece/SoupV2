using EntityComponentSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class DragComponent : AbstractComponent 
    {
        public DragComponent(Entity owner) : base(owner)
        {

        }
        public float DragCoefficient { get; set; } = 1.0f;
    }
}
