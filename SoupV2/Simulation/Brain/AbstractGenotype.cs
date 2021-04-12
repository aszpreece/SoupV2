using SoupV2.Simulation.Components;
using System;

namespace SoupV2.Simulation.Brain
{
    public abstract class AbstractGenotype: ICloneable
    {
        public abstract void CreateBrain(BrainComponent brainComponent);

        public abstract float CompareSimilarity(AbstractGenotype other);
        public abstract object Clone();
        public Species Species { get; set; }

    }
}
