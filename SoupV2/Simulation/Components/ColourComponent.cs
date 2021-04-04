using EntityComponentSystem;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class ColourComponent: AbstractComponent
    {
        // Controls the colour filter of the entity
        [Control]
        public float R { get; set; }
        [Control]
        public float G { get; set; }
        [Control]
        public float B { get; set; }

        public ColourComponent(Entity owner): base(owner)
        {

        }

    }
}
