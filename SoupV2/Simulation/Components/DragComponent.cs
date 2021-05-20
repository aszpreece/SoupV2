using EntityComponentSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class DragComponent : AbstractComponent 
    {
        public DragComponent(Entity owner) : base(owner)
        {

        }

        [Description("The drag for the movement")]
        public float MovementDragCoefficient { get; set; } = 1.0f;

        [Description("The drag for the rotation")]
        public float RotationDragCoefficient { get; set; } = 1.0f;

    }
}
