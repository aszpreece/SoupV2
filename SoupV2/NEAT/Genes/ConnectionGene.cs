using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.NEAT.Genes
{
    public class ConnectionGene: ICloneable, IGene
    {
        public ConnectionGene(ConnectionGene other)
        {
            this.InnovationId = other.InnovationId;
            this.Source = other.Source;
            this.To = other.To;
            this.Weight = other.Weight;
            this.Enabled = other.Enabled;
            this.Recurrent = other.Recurrent;
        }

        public ConnectionGene(int innovationId, int source, int to, double weight, bool enabled=true, bool recurrent= false)
        {
            this.InnovationId = innovationId;
            this.Source = source;
            this.To = to;
            this.Weight = weight;
            this.Enabled = enabled;
            this.Recurrent = recurrent;
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
