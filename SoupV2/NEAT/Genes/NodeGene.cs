using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.NEAT.Genes
{
    public class NodeGene: ICloneable, IGene
    {
        // Copy constructor
        public NodeGene(NodeGene other)
        {
            this.NodeType = other.NodeType;
            this.InnovationId = other.InnovationId;
            this.ActivationFunc = other.ActivationFunc;

        }
        public NodeGene(int innovationId, NodeType nodeType, ActivationFunctionType activationFuncName)
        {
            this.NodeType = nodeType;
            this.InnovationId = innovationId;
            this.ActivationFunc = activationFuncName;
        }

        public NodeType NodeType { get; set; }
        public int InnovationId { get; set; }
        public ActivationFunctionType ActivationFunc { get; set; }

        public object Clone()
        {
            return new NodeGene(this);
        }
    }
}
