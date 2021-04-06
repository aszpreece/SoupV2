using EntityComponentSystem;
using Newtonsoft.Json;
using SoupV2.EntityComponentSystem;
using SoupV2.NEAT.Genes;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.Components
{
    class ReproductionComponent : AbstractComponent
    {

        [JsonIgnore]
        [Control]
        // Control whether to try and reproduce
        public float Reproduce { get; set; }

        // Threshold that Reproduce must be over in order to trigger reproduction
        public float ReproductionThreshold { get; set; }

        // The entity that this entity gives birth to.
        // Could be an egg?
        public string ChildDefinitionId { get; set; }

        // The amount of energy that it takes to give birth
        public float ReproductionEnergyCost { get; set; }
        public float RequiredRemaining { get; set; }
        public float Efficency { get; set; }

        public ReproductionComponent(Entity owner) : base(owner)
        {
            
        }

    }
}
