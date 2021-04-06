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
        public float MovementDragCoefficient { get; set; } = 1.0f;
        public float RotationDragCoefficient { get; set; } = 1.0f;

    }
}
