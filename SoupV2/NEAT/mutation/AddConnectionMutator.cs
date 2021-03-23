using SoupV2.NEAT.Genes;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoupV2.NEAT.mutation
{
    class AddConnectionMutator : AbstractMutator
    {
        public bool AllowRecurrentConns { get; set; }
        public float NewWeightStd { get; set; }

        public float NewWeightPower { get; set; }

        private readonly Dictionary<(int, int), int> NewConnectionSignatures = new Dictionary<(int, int), int>();

        public override void Mutate(Genotype genotype, Random random, InnovationIdManager innovationIdManager)
        {
            // Shuffle two copies of the list of nodes. We can run through the first and attempt to pair it with a node from the second

            var fromList = random.Shuffle(genotype.NodeGenes);
            // We cannot connect 'to' input or bias nodes so we should ignore them
            var toList = random.Shuffle(genotype.NodeGenes.FindAll(gene => gene.NodeType != NodeType.INPUT && gene.NodeType != NodeType.BIAS));

            ConnectionGene proposedGene;

            //Find if there is already a connection between a potential 'from' node and a 'to' node.
            foreach (var fromNode in fromList)
            {

                var connectionDict =
                    genotype.ConnectionGenes.FindAll(gene => gene.Source == fromNode.InnovationId)
                        .ToDictionary(gene => gene.To, gene => gene);

                foreach (var toNode in toList)
                {
                    if (connectionDict.TryGetValue(toNode.InnovationId, out proposedGene))
                    {
                        // We can re enable the old gene.
                        if (proposedGene.Enabled == false)
                        {
                            // we must must check to see if this causes a cycle
                            bool recurrent = genotype.CreatesCycle(proposedGene);
                            if (!recurrent)
                            {
                                proposedGene.Enabled = true;
                                return;
                            }
                            // Enabling this gene would cause a loop when originally it did not.
                            // How do we deal with this neatly?
                        }
                    }
                    else
                    {
                        // There is no gene here so we can create a new connection
                        var weight = NewWeightPower * (float)random.Normal(0, NewWeightStd);
                        proposedGene = new ConnectionGene(innovationIdManager.PeekNextConnectionInnovationId(), fromNode.InnovationId, toNode.InnovationId, weight);
                        bool recurrent = genotype.CreatesCycle(proposedGene);
                        if (recurrent && !AllowRecurrentConns)
                        {
                            continue;
                        }
                        proposedGene.Recurrent = recurrent;
                        // We can safely add this gene. We need to check if the new gene has been added before.

                        // Check if this mutation has already occured
                        var signature = (proposedGene.Source, proposedGene.To);
                        int innovId;
                        if (NewConnectionSignatures.TryGetValue(signature, out innovId))
                        {
                            proposedGene.InnovationId = innovId;
                        }
                        else
                        {
                            // We only peeked it before, now we need to advance it properly
                            innovationIdManager.GetNextConnectionInnovationId();
                            NewConnectionSignatures[signature] = innovId;
                        }
                        genotype.ConnectionGenes.Add(proposedGene);

                    }

                }
            }
        }
    }
}
