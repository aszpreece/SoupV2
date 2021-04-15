using SoupV2.Simulation.Components;
using System;

namespace SoupV2.Simulation.Brain
{
    public abstract class AbstractBrainGenotype: ICloneable
    {
        public abstract void CreateBrain(BrainComponent brainComponent);
        public abstract float CompareSimilarity(AbstractBrainGenotype other);
        public abstract object Clone();
        public abstract AbstractBrain GetBrain();

    }
}
