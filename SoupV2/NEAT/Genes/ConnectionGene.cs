using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.NEAT.Genes
{
    public class ConnectionGene: ICloneable, IGene
    {
        public ConnectionGene(ConnectionGene other)
        {
            InnovationId = other.InnovationId;
            Source = other.Source;
            To = other.To;
            Weight = other.Weight;
            Enabled = other.Enabled;
            Recurrent = other.Recurrent;
        }

        public ConnectionGene(int innovationId, int source, int to, double weight, bool enabled=true, bool recurrent= false)
        {
            InnovationId = innovationId;
            Source = source;
            To = to;
            Weight = weight;
            Enabled = enabled;
            Recurrent = recurrent;
        }

        public int InnovationId { get; set; }
        public int Source { get; set; }
        public int To { get; set; }
        public double Weight { get; set; }
        public bool Enabled { get; set; }
        public bool Recurrent { get; set; }
        public object Clone()
        {
            return new ConnectionGene(this);
        }
    }
}
