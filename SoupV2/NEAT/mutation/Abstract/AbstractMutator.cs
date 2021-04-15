using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SoupV2.NEAT.mutation
{
    /// <summary>
    /// Abstract class for a Mutator operator
    /// </summary>
    public abstract class AbstractMutator
    {
        [JsonProperty(Required = Required.Always)]
        public float ProbabiltiyOfMutation { get; set; } = 1.0f;
        /// <summary>
        /// Mutates a given genotype
        /// </summary>
        /// <param name="genotype">Genotype to mutate</param>
        public abstract void Mutate(NeatBrainGenotype genotype, Random random, InnovationIdManager innovationIdManager);
    }
}
