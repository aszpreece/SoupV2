using SoupV2.NEAT.Genes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.NEAT.mutation
{
    public class SplitConnectionMutator : AbstractMutator
    {
        private Dictionary<int, (int, int, int)> SplitSignatures = new Dictionary<int, (int, int, int)>();




        public ActivationFunctionType[] ActivationFunctions { get; set; } = new ActivationFunctionType[] { ActivationFunctionType.SIGMOID };

        /// <summary>
        /// Given a genotype, split a random connection and add an intermediate node.
        /// The weight of the connection between the original source node and the new will be the same as the connection we are splitting.
        /// The connection from the new node to the original from node will have the value 1.
        /// The original connection will be disabled.
        /// </summary>
        /// <param name="genotype">The genotype to mutate.</param>
        /// <param name="random">Instance of random to use.</param>
        public override void Mutate(NeatBrainGenotype genotype, Random random, InnovationIdManager innovationIdManager)
        {
            // Decide on the connection to split

            // Fix: no longer tries to split disabled genes
            var enabledGenes = genotype.ConnectionGenes.FindAll(gene => gene.Enabled);

            // Fix: check if no connections

            if (enabledGenes.Count == 0){
                return;
            }

            // Disable the current connection

            var originalConnection = enabledGenes[random.Next(enabledGenes.Count)];
            originalConnection.Enabled = false;


            int nodeInnovId;
            int fromInnovId;
            int toInnovId;

            // See if this mutation has arisen in this generation and if so use the innovation ids from that mutation
            if (this.SplitSignatures.TryGetValue(originalConnection.InnovationId, out (int, int, int) innovationIdsTuple))
            {
                (
                    nodeInnovId,
                    fromInnovId,
                    toInnovId
                ) = innovationIdsTuple;
            }
            else
            {
                nodeInnovId = innovationIdManager.GetNextNodeInnovationId();
                fromInnovId = innovationIdManager.GetNextConnectionInnovationId();
                toInnovId = innovationIdManager.GetNextConnectionInnovationId();
                // Post the created innovation numbers as a tuple
                SplitSignatures[originalConnection.InnovationId] = (nodeInnovId, fromInnovId, toInnovId);
            }



            // Create a new node with the innovation number
            genotype.NodeGenes.Add(new NodeGene(nodeInnovId, NodeType.HIDDEN, ActivationFunctionType.SIGMOID));

            // New connection from old source to new node with weight of 1.0 to reduce impact
            genotype.ConnectionGenes.Add(new ConnectionGene(fromInnovId, originalConnection.Source, nodeInnovId, 1.0f));

            // New connection from new node to old to. Weight is that of the old connection
            genotype.ConnectionGenes.Add(new ConnectionGene(toInnovId, nodeInnovId, originalConnection.To, originalConnection.Weight));
        }
    }
}
