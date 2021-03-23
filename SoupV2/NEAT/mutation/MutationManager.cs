using SoupV2.NEAT.mutation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.NEAT.Genes
{
    public class MutationManager
    {
        private readonly Random Random = new Random();

        public AbstractMutator[] Mutators { get; set; }

        public MutationManager(MutationConfig config)
        {
            Mutators = config.Mutators;
        }

        /// <summary>
        /// Mutates the given genotype with the assigned mutators.
        /// Mutators each have an assigned probability of activating (ProbabiltiyOfMutation). The list of mutators is read from the lowest index up until the highest.
        /// If a mutator activates, then no more mutators can be activated and the mutattion finishes. 
        /// A mutator with a ProbabiltiyOfMutation will always activate if we reach it in the list.
        /// </summary>
        /// <param name="genotype">Genotype to mutate</param>
        /// <param name="innovationIdManager">Innovation id manager. New innovation ids will be taken from here.</param>
        public void Mutate(Genotype genotype, InnovationIdManager innovationIdManager)
        {
            foreach (AbstractMutator mutator in Mutators)
            {
                if (Random.NextDouble() < mutator.ProbabiltiyOfMutation)
                {
                    mutator.Mutate(genotype, Random, innovationIdManager);
                }
            }
        }
    }
}
