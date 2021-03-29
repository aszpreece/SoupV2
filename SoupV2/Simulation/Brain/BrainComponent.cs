using EntityComponentSystem;
using SoupV2.Simulation.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Brain
{
    class BrainComponent : IComponent
    {
        private AbstractBrain brain;
        
        public BrainComponent(AbstractBrain brain)
        {
            foreach (var brainInput in brain.GetInputs()) {

                // take input
                // get property of object the input comes from
                // store it so it can be accessed later

            }

            foreach (var brainOutput in brain.GetInputs())
            {
                var mov = new MovementControlComponent();

                var reference = mov.GetType().GetProperty("WishForceForward");

                reference.SetValue(mov, 0.1);
                // take input
                // get property of object the input comes from
                // store it so it can be accessed later

            }
        }
    }
}
