using SoupV2.NEAT.Genes;
using SoupV2.Simulation.Brain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoupV2.NEAT
{
    public class Phenotype
    {
        // Dict of node activation values indexed by node id
        // (activation values are the node inputs values with the activation function applied)
        Dictionary<int, double> NodeActivations { get; set; } = new Dictionary<int, double>();

        // Connections, indexed by id of node the connection terminates at.
        // First element of pair is id of 'source' node, second is the weight, third is innovation id of the connection
        Dictionary<int, List<(int, double, int)>> Connections { get; } = new Dictionary<int, List<(int, double, int)>>();
        private int[] OutputNodes { get; }

        private int[] InputNodes { get; set; }

        private readonly (int, Func<double, double>)[] NonInputNodes;

        public Dictionary<string, int> NodeNameMap { get; }



        public Phenotype(Genotype genotype)
        {

            // Array of the input/bias nodes
            InputNodes = genotype.NodeGenes.Where(gene => gene.NodeType == NodeType.INPUT || gene.NodeType == NodeType.BIAS).Select(gene => gene.InnovationId).ToArray();

            // List of non input/bias nodes
            NonInputNodes = genotype.NodeGenes
                .Where(gene => gene.NodeType != NodeType.INPUT && gene.NodeType != NodeType.BIAS)
                .Select(gene => (gene.InnovationId, ActivationFunctions.Functions[gene.ActivationFuncName]))
                .ToArray();


            // List of output nodes
            OutputNodes = genotype.NodeGenes
                .Where(gene => gene.NodeType == NodeType.OUTPUT)
                .Select(gene => gene.InnovationId)
                .ToArray();

            foreach (ConnectionGene conn in genotype.ConnectionGenes.FindAll(gene => gene.Enabled))
            {
                // Skip the disabled ones
                List<(int, double, int)> connectionsTo;
                var connectionTuple = (conn.Source, conn.Weight, conn.InnovationId);
                if (Connections.TryGetValue(conn.To, out connectionsTo))
                {
                    connectionsTo.Add(connectionTuple);
                }
                else
                {
                    Connections[conn.To] = new List<(int, double, int)>() { connectionTuple };
                }

            }

            genotype.Phenotype = this;

            this.NodeNameMap = genotype.NodeNameMap;
        }

        private Dictionary<int, double> tempActivations = new Dictionary<int, double>();

        public void Calculate(Dictionary<string, double> inputs)
        {
            tempActivations.Clear();

            foreach (var (key, value) in inputs)
            {
                var inputInnovId = NodeNameMap[key];
                tempActivations[inputInnovId] = value;
                NodeActivations[inputInnovId] = value;
            }

            foreach (var (nodeId, function) in NonInputNodes)
            {
                if (Connections.TryGetValue(nodeId, out List<(int, double, int)> fromList))
                {
                    double total = 0;
                    foreach (var (source, weight, _) in fromList)
                    {
                        total += NodeActivations.TryGetValue(source, out double activation) ? activation * weight : 0;
                    }
                    tempActivations[nodeId] = function(total);
                }
            }

            var old = NodeActivations;
            NodeActivations = tempActivations;
            tempActivations = old;
            old.Clear();
        }
        public double Get(string input)
        {
            return NodeActivations[NodeNameMap[input]];
        }

        public double Get(int nodeInnovId)
        {
            return NodeActivations[nodeInnovId];
        }
    }

}
