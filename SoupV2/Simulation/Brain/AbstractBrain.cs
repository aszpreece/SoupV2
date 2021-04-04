using System;
using System.Collections.Generic;

namespace SoupV2.Simulation.Brain
{
    public abstract class AbstractBrain
    {
        /// <summary>
        /// Sets the value of an input of the brain to a specified float value.
        /// </summary>
        /// <param name="name">Name of the brain input.</param>
        /// <param name="value">float value of the input.</param>
        public abstract void SetInput(string name, float value);

        /// <summary>
        /// Set an input on the brain to the given float array
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public abstract void SetInput(string name, float[] value);

        // Set the given 
        public abstract float GetInput(string name);

        public abstract void Calculate();
        internal abstract float GetOutput(string name);
    }
}