using SoupV2.NEAT.Genes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoupV2.NEAT
{
    public class Genotype: ICloneable
    {
        public List<ConnectionGene> ConnectionGenes { get; }
        public List<NodeGene> NodeGenes { get; }

        public float? Fitness { get; set; }
        public float? AdjustedFitness { get; set; } = 0;

        public Species Species { get; set; }
        
        public Dictionary<string, int> NodeNameMap { get; set; }

        public int ConnectionIdStart { get; private set; } = 0;
        public int NodeIdStart { get; private set; } = 0;
        public Phenotype Phenotype { get; internal set; }

        public Genotype(Genotype other)
        {
            // Node name map does not need copying as it should remain the same between different genomes
            this.NodeNameMap = other.NodeNameMap;

            // Keep track of, but do not copy, this genotypes's species
            this.Species = other.Species;

            // The genome lists do need to be copied as we can end up doing mutable operations to them
            this.ConnectionGenes = other.ConnectionGenes.ConvertAll(connectionGene => (ConnectionGene)connectionGene.Clone());
            this.NodeGenes = other.NodeGenes.ConvertAll(nodeGene => (NodeGene)nodeGene.Clone());
        }


        public Genotype(List<NodeGene> nodeGenes, List<ConnectionGene> connectionGenes, Species species=null)
        {
            this.NodeNameMap = new Dictionary<string, int>();
            this.Species = species;

            this.ConnectionGenes = connectionGenes;
            this.NodeGenes = nodeGenes;
        }

        public Genotype()
        {
            this.NodeNameMap = new Dictionary<string, int>();

            this.ConnectionGenes = new List<ConnectionGene>();
            this.NodeGenes = new List<NodeGene>();
        }

        /// <summary>
        /// Returns True if the addition/re-enabling of the given gene will create a cycle in the genome's graph
        /// </summary>
        /// <param name="newGene">The gene to query.</param>
        /// <returns></returns>
        public bool CreatesCycle(ConnectionGene newGene)
        {
            if (newGene.Source == newGene.To)
            {
                return true;
            }

            var visited = new HashSet<int>();
            var toExpand = new Stack<int>();
            var connectionDictionary = CreateConnectionDictionary();
            var targetNode = newGene.To;

            //DFS to find if this creates a loop
            toExpand.Push(newGene.Source);

            while (toExpand.Count > 0)
            {
                var currentNode = toExpand.Pop();
                visited.Add(currentNode);

                if (connectionDictionary.ContainsKey(currentNode))
                {
                    foreach (int source in connectionDictionary[currentNode])
                    {
                        if (source == targetNode)
                        {
                            return true;
                        }

                        if (visited.Contains(targetNode))
                        {
                            // Avoid already visited nodes
                            continue;
                        }

                        toExpand.Push(source);
                    }
                }
            }
            return false;
        }

        /**
         * Generates a connection dictionary with each connection being indexed by the id of the node the connection leads to.
         */
        public Dictionary<int, List<int>> CreateConnectionDictionary()
        {
            var connectionDictionary = new Dictionary<int, List<int>>();
            foreach (ConnectionGene connectionGene in this.ConnectionGenes.Where(con => con.Enabled && !con.Recurrent))
            {
                if (connectionDictionary.ContainsKey(connectionGene.To))
                {
                    connectionDictionary[connectionGene.To].Add(connectionGene.Source);
                } else
                {
                    connectionDictionary[connectionGene.To] = new List<int>(new int[connectionGene.Source]);
                }
            }
            return connectionDictionary;
        }

        /**
         * Add a named node to this genotype.
         */
        public void AddNamedNode(string name, NodeType nodeType, string activationFuncName )
        {
            if (NodeNameMap.ContainsKey(name))
            {
                throw new ArgumentException(string.Format("{0} is already a named node.", name));
            }
            
            this.NodeGenes.Add(new NodeGene(NodeIdStart, nodeType, activationFuncName));
            this.NodeNameMap[name] = NodeIdStart;
            NodeIdStart += 1;
        }


        public (int, int, double) CompareConnectionGenes(Genotype other)
        {
            // Calculate number of similar genes

            int thisGenesCount = this.ConnectionGenes.Count;
            int otherGenesCount = other.ConnectionGenes.Count;

            int thisIndex = 0;
            int otherIndex = 0;

            int totalDisjointGenes = 0;

            double totalWeightDiff = 0;
            int totalSharedConnections = 0;

            while (thisIndex < thisGenesCount && otherIndex < otherGenesCount) {
                ConnectionGene thisGene = this.ConnectionGenes[thisIndex];
                ConnectionGene otherGene = other.ConnectionGenes[otherIndex];

                if (thisGene.InnovationId == otherGene.InnovationId)
                {
                    // genes match, progress both pointers
                    thisIndex += 1;
                    otherIndex += 1;
                    totalWeightDiff += Math.Abs(thisGene.Weight - otherGene.Weight);
                    totalSharedConnections += 1;
                    continue;
                }
                else if (thisGene.InnovationId > otherGene.InnovationId)
                {
                    // If the thisGene (top genome) has a larger innovation number
                    // Progress otherIndex counter
                    otherIndex += 1;
                    totalDisjointGenes += 1;
                    continue;
                }
                // This will always happen, because otherGene.InnovationId must be larger if it is not equal or smaller to thisGene.InnovationId
                else {
                    // thisGene.innov_id < otherGene.innov_id
                    // If the otherGene (bottom genome) has a larger innovation number
                    // progress thisGene counter
                    thisIndex += 1;
                    totalDisjointGenes += 1; 
                    continue;
                }
            }

            // Once we've got here we know some of the genes don't match so lets count them
            int totalExcessGenes = (thisGenesCount - (thisIndex + 1)) + (otherGenesCount - (otherIndex + 1));

            // Max(1,...) to avoid divide by 0
            return (totalDisjointGenes, totalExcessGenes, totalWeightDiff / Math.Max(1, totalSharedConnections));
        }
        
        public object Clone()
        {
            return new Genotype(this);
        }
    }
}
