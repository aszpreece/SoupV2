using SoupV2.NEAT.Genes;
using SoupV2.Simulation.Brain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SoupV2.NEAT
{
    public class NeatBrainPhenotype : AbstractBrain
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

        private Dictionary<int, double> tempActivations = new Dictionary<int, double>();

        public NeatBrainGenotype Genotype {get; set;}

        public NeatBrainPhenotype(NeatBrainGenotype genotype)
        {
            Genotype = genotype;

            // Array of the input/bias nodes
            InputNodes = genotype.NodeGenes.Where(gene => gene.NodeType == NodeType.INPUT || gene.NodeType == NodeType.BIAS).Select(gene => gene.InnovationId).ToArray();

            // List of non input/bias nodes
            NonInputNodes = genotype.NodeGenes
                .Where(gene => gene.NodeType != NodeType.INPUT && gene.NodeType != NodeType.BIAS)
                .Select(gene => (gene.InnovationId, ActivationFunctions.Functions[gene.ActivationFunc]))
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

            this.NodeNameMap = genotype.NodeNameMap;
        }
         

        public override void Calculate()
        {
            tempActivations.Clear();
            //Copy over all the inputs that have been set
            foreach(int nodeId in InputNodes)
            {
                tempActivations[nodeId] = NodeActivations.GetValueOrDefault(nodeId);
            }
            foreach (var (nodeId, function) in NonInputNodes)
            {
                if (Connections.TryGetValue(nodeId, out List<(int, double, int)> fromList))
                {
                    double total = 0;
                    foreach (var (source, weight, _) in fromList)
                    {
                        total += NodeActivations.TryGetValue(source, out double activation) ? activation * weight : 0;
#if DEBUG
                        //if (total > 0)
                        //    Debug.WriteLine($"{total}");
#endif
                    }
                    tempActivations[nodeId] = function(total);
                } else
                {
                    tempActivations[nodeId] = 0;
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
            if (NodeActivations.ContainsKey(nodeInnovId)) {
                return NodeActivations[nodeInnovId];
            }
            throw new NodeNotActivatedException(nodeInnovId);
        }

        public override void SetInput(string name, float value)
        {
            var inputInnovId = NodeNameMap[name];
            NodeActivations[inputInnovId] = value;
        }

        public override void SetInput(string name, float[] value)
        {
            throw new NotImplementedException();
        }

        public override float GetInput(string name)
        {
            return (float)Get(name);
        }

        internal override float GetOutput(string name)
        {
            return (float)Get(name);
        }
    }

}
