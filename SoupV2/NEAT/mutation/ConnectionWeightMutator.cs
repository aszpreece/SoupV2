using SoupV2.NEAT.Genes;
using SoupV2.util;
using System;

namespace SoupV2.NEAT.mutation
{
    public class ConnectionWeightMutator : AbstractMutator
    {

        public float ProbPerturbWeight { get; set; }
        public double NewWeightStd { get; set; }
        public double ProbResetWeight { get; set; }
        public float NewWeightPower { get; set; }
        public double ProbEnableConn { get; set; }
        public double ProbDisableConn { get; set; }
        public float WeightPerturbScale { get; set; }

        /// <summary>
        /// Mutates the weights of a neural net. For each connection in the genotype there is a random chance of different mutations occuring according to the properties defined on this class.
        /// </summary>
        /// <param name="genotype">Genotype to mutate.</param>
        /// <param name="random">Random for number generation.</param>
        /// <param name="innovationIdManager"></param>
        public override void Mutate(NeatBrainGenotype genotype, Random random, InnovationIdManager innovationIdManager)
        {

            foreach (ConnectionGene gene in genotype.ConnectionGenes)
            {
                if (random.NextDouble() < this.ProbPerturbWeight)
                {
                    // Perturb the weights
                    gene.Weight += (float)random.Normal(0.0d, NewWeightStd) * WeightPerturbScale;

                }
                if (random.NextDouble() < this.ProbResetWeight)
                {
                    // Reset the weight to a new random value
                    gene.Weight = (float)random.Normal(0.0d, NewWeightStd) * NewWeightPower;

                }
                if (random.NextDouble() < this.ProbEnableConn)
                {
                    if (!genotype.CreatesCycle(gene))
                    {
                        gene.Enabled = true;
                    }

                }
                if (random.NextDouble() < this.ProbDisableConn)
                {
                    gene.Enabled = false;

                }

            }
        }
    }
}
